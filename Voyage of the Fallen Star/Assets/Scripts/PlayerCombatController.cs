using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
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

    // TODO: Sometimes just stops working...
    private void PerformMelee()
    {
        meleeAudio.Play();
        _lastMeleeTime = Time.time;
        
        Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, meleeRange, enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            HealthComponent enemyHealth = enemy.GetComponentInParent<HealthComponent>();
            
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(meleeDamage);
            }
        }
    }

    private void PerformRanged()
    {
        rangedAudio.Play();
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
        if (aoeParticlePrefab)
        {
            GameObject aoeInstance = Instantiate(aoeParticlePrefab, transform.position, Quaternion.identity);
            Destroy(aoeInstance, 2f);
        }

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, aoeRadius, enemyLayer);
        
        foreach (Collider enemy in hitEnemies)
        {
            HealthComponent enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(aoeDamage);
            }
        }
    }
}