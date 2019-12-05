using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NipController : MonoBehaviour
{
    [SerializeField] private float maxThreshold = 80f;
    [SerializeField] private float minThreshold = -20f;
    [SerializeField] private float maxMilkTime = 0.3f;
    [SerializeField] private GameObject tittyBone = null;
    [SerializeField] private ParticleSystem tittyMilk = null;

    private Vector3 tittyStartPos;
    private Vector3 controllerStartPos;
    private Quaternion controllerStartRot;
    private float thresholdFactor = 1000.0f;
    private OVRGrabbable grabbable = null;
    private bool previouslyGrabbed = false;
    private float timer = 0.0f;
    private bool trySquirting = false;

    void Start()
    {
        controllerStartPos = transform.position;
        controllerStartRot = transform.rotation;
        tittyStartPos = tittyBone.transform.position;
        grabbable = GetComponent<OVRGrabbable>();

        var emission = tittyMilk.emission;  
        emission.rateOverTime = 0.0f;
    }

    void Update()
    {
        if (!grabbable.isGrabbed && previouslyGrabbed)
        {
            transform.position = controllerStartPos;
            transform.rotation = controllerStartRot;
        }

        var maxDiff = maxThreshold / thresholdFactor;
        var minDiff = minThreshold / thresholdFactor;
        var diff = Mathf.Clamp((controllerStartPos - transform.position).y, minDiff, maxDiff);
        var newPos = tittyBone.transform.position;
        newPos.y = tittyStartPos.y - diff;

        tittyBone.transform.position = newPos;

        trySquirting = diff == maxDiff;
        var emission = tittyMilk.emission;
        emission.rateOverTime = 0.0f;

        if (trySquirting && timer <= maxMilkTime)
        {
            emission.rateOverTime = 10.0f;
            timer += Time.deltaTime;
        }
        else if (!grabbable.isGrabbed)
        {
            timer = 0.0f;
        }

        previouslyGrabbed = grabbable.isGrabbed;
    }
}
