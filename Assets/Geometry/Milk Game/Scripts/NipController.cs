using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NipController : MonoBehaviour
{
    [SerializeField] private float maxThreshold = 80f;
    [SerializeField] private float minThreshold = -20f;
    [SerializeField] private float maxMilkTime = 0.3f;
    [SerializeField] private float retractionTime = 0.3f;
    [SerializeField] private GameObject tittyBone = null;
    [SerializeField] private ParticleSystem tittyMilk = null;

    private Vector3 tittyStartPos;
    private Vector3 controllerStartPos;
    private Quaternion controllerStartRot;
    private float thresholdFactor = 1000.0f;
    private OVRGrabbable grabbable = null;
    private bool previouslyGrabbed = false;
    private float milkTimer = 0.0f;
    private bool trySquirting = false;
    private bool squirtRefractoryPeriod = false;
    private float maxDiff = 0.0f;
    private float minDiff = 0.0f;

    void Start()
    {
        controllerStartPos = transform.position;
        controllerStartRot = transform.rotation;
        tittyStartPos = tittyBone.transform.position;
        grabbable = GetComponent<OVRGrabbable>();

        var emission = tittyMilk.emission;  
        emission.rateOverTime = 0.0f;
        maxDiff = maxThreshold / thresholdFactor;
        minDiff = minThreshold / thresholdFactor;
    }

    void Update()
    {
        maxDiff = maxThreshold / thresholdFactor;
        minDiff = minThreshold / thresholdFactor;

        if (!grabbable.isGrabbed && previouslyGrabbed)
        {
            StartCoroutine(retractNip());
        }

        var diff = Mathf.Clamp((controllerStartPos - transform.position).y, minDiff, maxDiff);
        var newPos = tittyBone.transform.position;
        newPos.y = tittyStartPos.y - diff;

        tittyBone.transform.position = newPos;

        trySquirting = diff == maxDiff;
        var emission = tittyMilk.emission;
        emission.rateOverTime = 0.0f;

        if (!squirtRefractoryPeriod && trySquirting && milkTimer <= maxMilkTime)
        {
            emission.rateOverTime = 10.0f;
            milkTimer += Time.deltaTime;
        }
        else if (!grabbable.isGrabbed)
        {
            milkTimer = 0.0f;
        }

        previouslyGrabbed = grabbable.isGrabbed;
    }

    private IEnumerator retractNip()
    {
        squirtRefractoryPeriod = true;

        transform.rotation = controllerStartRot;
        var diff = (controllerStartPos - transform.position).y;
        if (diff > maxDiff)
        {
            diff += 0.01f;
            transform.position = new Vector3(controllerStartPos.x, controllerStartPos.y - diff, controllerStartPos.z);
        }

        var initialPos = transform.position;
        var animTimer = 0.0f;
        var destination = controllerStartPos - initialPos;

        while (animTimer <= retractionTime && !grabbable.isGrabbed)
        {
            animTimer += Time.deltaTime;
            var animPercent = animTimer / retractionTime;
            var speedFactor = 1 + (1 - animPercent);

            transform.position = initialPos + (destination * animPercent * speedFactor);

            yield return null;
        }

        squirtRefractoryPeriod = false;
    }
}
