using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWithScaler : MonoBehaviour
{
    public ObjectScaler objectScaler;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectScaler.gameObject.activeInHierarchy)
        {
            if (objectScaler.IsGrowing)
            {
                animator.SetBool("idle", false);
                animator.SetBool("breathIn", true);
            }
            else
            {
                animator.SetBool("idle", false);
                animator.SetBool("breathIn", false);
            }
        }
        else
        {
            animator.SetBool("idle", true);
        }

    }
}
