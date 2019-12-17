using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private GenericAudioSource nipAudio = null;
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
    private GameManager gameManager = null;
    private List<NipController> otherNips = null;

    void Start()
    {
        controllerStartPos = transform.position;
        controllerStartRot = transform.rotation;
        tittyStartPos = tittyBone.transform.position;
        grabbable = GetComponent<OVRGrabbable>();
        gameManager = FindObjectOfType<GameManager>();
        nipAudio = GetComponent<GenericAudioSource>();

        var emission = tittyMilk.emission;  
        emission.rateOverTime = 0.0f;
        maxDiff = maxThreshold / thresholdFactor;
        minDiff = minThreshold / thresholdFactor;

        otherNips = new List<NipController>(FindObjectsOfType<NipController>()).Where(nc => nc != this).ToList();
    }

    void Update()
    {
        if (gameManager.milkingAllowed)
        {
            maxDiff = maxThreshold / thresholdFactor;
            minDiff = minThreshold / thresholdFactor;

            if (!grabbable.isGrabbed && previouslyGrabbed)
            {
                StartCoroutine(RetractNip());
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
                nipAudio.playSound();
                milkTimer += Time.deltaTime;

                var scoreDivisor = otherNips.Any(nc => nc.IsMilking()) ? 2 : 1;
                emission.rateOverTime = 10.0f / scoreDivisor;
                gameManager.IncreaseScore((int)(Time.deltaTime * 1000) / scoreDivisor);
            }
            else if (!grabbable.isGrabbed)
            {
                milkTimer = 0.0f;
            }

            previouslyGrabbed = grabbable.isGrabbed;
        }
        else if (!grabbable.isGrabbed)
        {
            transform.position = controllerStartPos;
            transform.rotation = controllerStartRot;
            tittyBone.transform.position = tittyStartPos;
            
            var emission = tittyMilk.emission;
            emission.rateOverTime = 0.0f;
            milkTimer = 0.0f;
            squirtRefractoryPeriod = false;
            previouslyGrabbed = false;
        }
    }

    public bool IsMilking()
    {
        return tittyMilk.emission.rateOverTime.constant > 0.0f;
    }

    private IEnumerator RetractNip()
    {
        squirtRefractoryPeriod = true;
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

        squirtRefractoryPeriod = false;
    }
}
