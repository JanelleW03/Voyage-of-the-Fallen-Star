using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class DoorTrigger : MonoBehaviour
{
    public Transform spawnPoint;
    public AudioSource doorAudioSource; // drag an AudioSource with your door sound here

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
            StartCoroutine(TransitionToRoom(other.gameObject));
    }

    private IEnumerator TransitionToRoom(GameObject player)
    {
        isTransitioning = true;

        // Play door sound before fading
        if (doorAudioSource != null)
            doorAudioSource.Play();

        yield return StartCoroutine(FadeManager.Instance.FadeOut());

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

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