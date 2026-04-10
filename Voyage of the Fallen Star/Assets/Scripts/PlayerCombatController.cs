using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    private static readonly int MeleeAttackTrigger = Animator.StringToHash("MeleeAttackTrigger");
    private static readonly int RangedAttackTrigger = Animator.StringToHash("RangedAttackTrigger");
    
    [Header("2.5D Hitbox Lenience")]
    public float meleeDepth = 1.5f; // How forgiving the Z-axis is for melee
    public float aoeDepth = 4.0f;   // How forgiving the Z-axis is for AOE
    
    public float meleeDamage = 10f;
    public float meleeRange = 1.5f;
    public float meleeCooldown = 0.5f;
    public float meleeEnergyCost = 15f;
    public Transform meleeAttackPoint;
    public LayerMask enemyLayer;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public float rangedCooldown = 1.5f;
    public float rangedEnergyCost = 25f;

    public float aoeDamage = 30f;
    public float aoeRadius = 5f;
    public GameObject aoeParticlePrefab;

    private InputAction _meleeAction;
    private InputAction _rangedAction;
    private InputAction _aoeAction;
    private float _lastMeleeTime;
    private float _lastRangedTime;
    private Camera _mainCamera;
    private StatsManager _statsManager;
    private Animator _animator;

    [Header("Audio")]
    [SerializeField]
    AudioSource meleeAudio;
    [SerializeField]
    AudioSource rangedAudio;
    [SerializeField]
    AudioSource AOEAudio;


    private void Start()
    {
        _meleeAction = InputSystem.actions.FindAction("Melee"); 
        _rangedAction = InputSystem.actions.FindAction("Ranged"); 
        _aoeAction = InputSystem.actions.FindAction("Special");
        _mainCamera = Camera.main;
        _statsManager = GetComponent<StatsManager>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_meleeAction.WasPerformedThisFrame() && Time.time >= _lastMeleeTime + meleeCooldown)
        {
            if (_statsManager.TryConsumeEnergy(meleeEnergyCost)) PerformMelee();
        }

        if (_rangedAction.WasPerformedThisFrame() && Time.time >= _lastRangedTime + rangedCooldown)
        {
            if (_statsManager.TryConsumeEnergy(rangedEnergyCost)) PerformRanged();
        }

        if (_aoeAction.WasPerformedThisFrame() && _statsManager.TryConsumeMana())
        {
            PerformAoe();
        }
    }

    private void PerformMelee()
    {
        meleeAudio.Play();
        _animator.SetTrigger(MeleeAttackTrigger);
        _lastMeleeTime = Time.time;
        
        foreach (Collider enemy in Physics.OverlapBox(meleeAttackPoint.position, new Vector3(meleeRange, 2f, meleeDepth) / 2f, meleeAttackPoint.rotation, enemyLayer))
        {
            HealthComponent enemyHealth = enemy.GetComponentInParent<HealthComponent>();
            if (enemyHealth) enemyHealth.TakeDamage(meleeDamage);
        }
    }

    private void PerformRanged()
    {
        rangedAudio.Play();
        _animator.SetTrigger(RangedAttackTrigger);
        _lastRangedTime = Time.time;

        Vector2 mousePos2D = Mouse.current.position.ReadValue();
        Vector3 mousePos3D = new Vector3(mousePos2D.x, mousePos2D.y, 0f);
        Ray ray = _mainCamera.ScreenPointToRay(mousePos3D);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
        {
            targetPoint = hit.collider.transform.position;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            if (groundPlane.Raycast(ray, out float distance)) targetPoint = ray.GetPoint(distance);
            else targetPoint = transform.position + transform.forward;
        }

        Vector3 aimDirection = targetPoint - projectileSpawnPoint.position;
        aimDirection.y = 0; 
        aimDirection.Normalize();

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(aimDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = aimDirection * projectileSpeed;
        }
    }
    
    private void PerformAoe()
    {
        AOEAudio.Play();
        _animator.SetTrigger(RangedAttackTrigger);
        if (aoeParticlePrefab) Instantiate(aoeParticlePrefab, transform.position, Quaternion.identity);
        
        foreach (Collider enemy in Physics.OverlapBox(transform.position, new Vector3(aoeRadius, 2f, aoeDepth) / 2f, Quaternion.identity, enemyLayer))
        {
            HealthComponent enemyHealth = enemy.GetComponentInParent<HealthComponent>();
            if (enemyHealth) enemyHealth.TakeDamage(aoeDamage);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draws the Melee Hitbox (Red)
        Gizmos.color = Color.red;
        if (meleeAttackPoint != null)
        {
            Gizmos.matrix = Matrix4x4.TRS(meleeAttackPoint.position, meleeAttackPoint.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(meleeRange, 2f, meleeDepth));
        }

        // Draws the AOE Hitbox (Blue)
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(aoeRadius, 2f, aoeDepth));
    }
}