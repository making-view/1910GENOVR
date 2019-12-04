using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NipController : MonoBehaviour
{
    [SerializeField] private float maxThreshold = 80f;
    [SerializeField] private float minThreshold = -20f;
    [SerializeField] private GameObject tittyBone = null;
    [SerializeField] private ParticleSystem tittyMilk = null;

    private Vector3 tittyStartPos;
    private Vector3 controlObjectStartPos;
    private float thresholdFactor = 1000.0f;
    private OVRGrabbable grabbable = null;
    private bool previouslyGrabbed = false;

    void Start()
    {
        controlObjectStartPos = transform.position;
        tittyStartPos = tittyBone.transform.position;
        grabbable = GetComponent<OVRGrabbable>();

        var emission = tittyMilk.emission;  
        emission.rateOverTime = 0.0f;
    }

    void Update()
    {
        if (!grabbable.isGrabbed && previouslyGrabbed)
        {
            transform.position = controlObjectStartPos;
        }

        var maxDiff = maxThreshold / thresholdFactor;
        var minDiff = minThreshold / thresholdFactor;
        var diff = Mathf.Clamp((controlObjectStartPos - transform.position).y, minDiff, maxDiff);
        var newPos = tittyBone.transform.position;
        newPos.y = tittyStartPos.y - diff;

        tittyBone.transform.position = newPos;

        var emission = tittyMilk.emission;
        if (diff == maxDiff)
            emission.rateOverTime = 10.0f;
        else
            emission.rateOverTime = 0.0f;

        previouslyGrabbed = grabbable.isGrabbed;  
    }
}
