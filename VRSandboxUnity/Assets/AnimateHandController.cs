using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandController : MonoBehaviour
{

    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;
    public InputActionReference thumbInputActionReference;

    private Animator handAnimator;
    private float gripValue;
    private float triggerValue;
    private float thumbValue;

    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        AnimateGrip();
        AnimateTrigger();
        AnimateThumb();
        {

        }
    }

    private void AnimateThumb()
    {
        thumbValue = thumbInputActionReference.action.ReadValue<float>();
        if(thumbValue > 0)
        {
            Debug.Log("Thumb Pressed");
        }
    }

    private void AnimateGrip()
    {
        gripValue = gripInputActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

    private void AnimateTrigger()
    {
        triggerValue = triggerInputActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
    }

}
