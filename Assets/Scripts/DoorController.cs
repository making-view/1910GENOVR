using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState {
    Open,
    Close
}

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] Animator rightChain;
    [SerializeField] Animator leftChain;
    [SerializeField] Animator rightBarrel;
    [SerializeField] Animator middleBarrel;
    [SerializeField] Animator leftBarrel;

    float animationLength = 2.0f;
    bool isAnimating = false;

    public void Start()
    {
        if (middleBarrel != null && middleBarrel.runtimeAnimatorController.animationClips[0])
        {
            animationLength = middleBarrel.runtimeAnimatorController.animationClips[0].length;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Close();
        }
    }

    public void Open()
    {
        rightBarrel.SetTrigger("Open");
        middleBarrel.SetTrigger("Open");
        leftBarrel.SetTrigger("Open");
    }

    public void Close()
    {
        rightBarrel.SetTrigger("Close");
        middleBarrel.SetTrigger("Close");
        leftBarrel.SetTrigger("Close");
    }

    IEnumerator AnimateDoor(AnimState animState)
    {
        isAnimating = true;
        var timer = 0.0f;

        rightBarrel.Play("Entry");
        middleBarrel.Play("Entry");
        leftBarrel.Play("Entry");

        while (timer <= animationLength)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        isAnimating = false;
    }
}