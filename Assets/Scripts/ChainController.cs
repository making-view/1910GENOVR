using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    public delegate void OnChainActionTriggeredDelegate();
    public OnChainActionTriggeredDelegate onChainActionTriggered;

    [SerializeField] private float maxPull = 200f;
    [SerializeField] private float triggerPoint = 150f;
    [SerializeField] private float retractionTime = 0.3f;
    [SerializeField] private GameObject chain = null;

    private Vector3 chainStartPos;
    private Vector3 controllerStartPos;
    private Quaternion controllerStartRot;
    private float thresholdFactor = 1000.0f;
    private OVRGrabbable grabbable = null;
    private bool previouslyGrabbed = false;
    private float maxDiff = 0.0f;
    private float triggerDiff = 0.0f;
    private bool chainNeedsReset = false;

    void Start()
    {
        controllerStartPos = transform.position;
        controllerStartRot = transform.rotation;
        chainStartPos = chain.transform.position;
        grabbable = GetComponent<OVRGrabbable>();
       
        maxDiff = maxPull / thresholdFactor;
        triggerDiff = triggerPoint / thresholdFactor;
    }

    void Update()
    {
        maxDiff = maxPull / thresholdFactor;
        triggerDiff = triggerPoint / thresholdFactor;

        if (!grabbable.isGrabbed && previouslyGrabbed)
        {
            StartCoroutine(RetractChain());
        }

        var diff = Mathf.Clamp((controllerStartPos - transform.position).y, 0.0f, maxDiff);
        var newPos = chain.transform.position;
        newPos.y = chainStartPos.y - diff;

        chain.transform.position = newPos;
           
        if (!chainNeedsReset && diff >= triggerPoint)
        {
            onChainActionTriggered();
            chainNeedsReset = true;
        }
        else if (diff < triggerPoint)
        {
            chainNeedsReset = false;
        }

        previouslyGrabbed = grabbable.isGrabbed;
    }

    private IEnumerator RetractChain()
    {
        transform.rotation = controllerStartRot;

        var diff = (controllerStartPos - transform.position).y;
        if (diff > maxDiff)
        { 
            transform.position = new Vector3(controllerStartPos.x, controllerStartPos.y - maxDiff, controllerStartPos.z);
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
    }
}