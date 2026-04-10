using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed via old Input System!");
        }
    }
}