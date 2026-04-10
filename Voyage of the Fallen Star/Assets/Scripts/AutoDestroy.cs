using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        float clipLength = anim.GetCurrentAnimatorStateInfo(0).length;
        
        Destroy(gameObject, clipLength);
    }
}