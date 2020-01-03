using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState {
    Open,
    Close
}

public class DoorController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] Animator leftDoor;
    [SerializeField] Animator rightDoor;
    [SerializeField] Animator rightChain;
    [SerializeField] Animator leftChain;
    [SerializeField] Animator rightBarrel;
    [SerializeField] Animator middleBarrel;
    [SerializeField] Animator leftBarrel;

    [Header("Settings")]
    [SerializeField] float chainShakeAdvanceEndTime = 0.3f;

    float animationLength = 2.0f;
    bool isAnimating = false;
    bool isDoorOpen = false;

    public void Start()
    {
        if (middleBarrel != null && middleBarrel.runtimeAnimatorController.animationClips[0])
        {
            animationLength = middleBarrel.runtimeAnimatorController.animationClips[0].length;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Close();
        }
    }

    public void Open()
    {
        if (!isAnimating && !isDoorOpen)
        {
            StartCoroutine(AnimateDoor(AnimState.Open));
        }
    }

    public void Close()
    {
        if (!isAnimating && isDoorOpen)
        {
            StartCoroutine(AnimateDoor(AnimState.Close));
        }
    }

    IEnumerator AnimateDoor(AnimState animState)
    {
        isAnimating = true;
        isDoorOpen = animState == AnimState.Open;

        var timer = 0.0f;
        var mainTriggerText = isDoorOpen ? "Open" : "Close";
        var reverseTriggerText = isDoorOpen ? "Close" : "Open";
        
        leftBarrel.SetTrigger(mainTriggerText);
        middleBarrel.SetTrigger(mainTriggerText);
        rightBarrel.SetTrigger(reverseTriggerText);

        leftDoor.SetTrigger(mainTriggerText);
        rightDoor.SetTrigger(mainTriggerText);

        rightChain.SetBool("Shake", true);
        leftChain.SetBool("Shake", true);

        while (timer <= animationLength)
        {
            timer += Time.deltaTime;

            if (timer >= animationLength - chainShakeAdvanceEndTime)
            {
                rightChain.SetBool("Shake", false);
                leftChain.SetBool("Shake", false);
            }

            yield return null;
        }

        isAnimating = false;
    }
}