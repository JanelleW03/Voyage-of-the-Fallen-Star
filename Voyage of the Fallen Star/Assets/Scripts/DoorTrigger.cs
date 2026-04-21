using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class DoorTrigger : MonoBehaviour
{
    public Transform spawnPoint;

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TransitionToRoom(other.gameObject));
        }
    }

    private IEnumerator TransitionToRoom(GameObject player)
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeManager.Instance.FadeOut());

        // Teleport player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 delta = spawnPoint.position - player.transform.position;

        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        // Snap camera
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
            vcam.OnTargetObjectWarped(player.transform, delta);

        yield return StartCoroutine(FadeManager.Instance.FadeIn());

        isTransitioning = false;
    }
}