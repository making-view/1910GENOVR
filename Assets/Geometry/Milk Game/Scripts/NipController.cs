using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NipController : MonoBehaviour
{
    [SerializeField] private float maxThreshold = 80f;
    [SerializeField] private GameObject tittyBone = null;
    [SerializeField] private ParticleSystem tittyMilk = null;

    private Vector3 tittyStartPos;
    private Vector3 controlObjectStartPos;
    private float thresholdFactor = 1000.0f;
    private CapsuleCollider collider = null;

    void Start()
    {
        controlObjectStartPos = transform.position;
        tittyStartPos = tittyBone.transform.position;
        collider = GetComponent<CapsuleCollider>();

        var emission = tittyMilk.emission;  
        emission.rateOverTime = 0.0f;
    }

    void Update()
    {
        var threshold = maxThreshold / thresholdFactor;
        var diff = Mathf.Clamp((controlObjectStartPos - transform.position).y, 0.0f, threshold);
        var newPos = tittyBone.transform.position;
        newPos.y = tittyStartPos.y - diff;

        tittyBone.transform.position = newPos;

        var emission = tittyMilk.emission;
        if (diff == threshold)
            emission.rateOverTime = 10.0f;
        else
            emission.rateOverTime = 0.0f;
    }
}
