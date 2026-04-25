using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class DoorTrigger : MonoBehaviour
{
    public Transform spawnPoint;
    public AudioSource doorAudioSource; // drag an AudioSource with your door sound here

    private bool isTransitioning = false;

    private bool _isLocked = false;

    public void SetLocked(bool locked)
    {
        _isLocked = locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning && !_isLocked)
            StartCoroutine(TransitionToRoom(other.gameObject));
    }

    private IEnumerator TransitionToRoom(GameObject player)
    {
        isTransitioning = true;

        Debug.Log($"spawnPoint: {spawnPoint}, player: {player}, FadeManager: {FadeManager.Instance}");

        if (doorAudioSource != null)
            doorAudioSource.Play();

        yield return StartCoroutine(FadeManager.Instance.FadeOut());

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        if (spawnPoint == null)
        {
            Debug.LogError("spawnPoint is null!");
            yield break;
        }

        Vector3 delta = spawnPoint.position - player.transform.position;

        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
            vcam.OnTargetObjectWarped(player.transform, delta);

        yield return StartCoroutine(FadeManager.Instance.FadeIn());

        isTransitioning = false;
    }
}