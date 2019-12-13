using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains resources for nip-audio as well as logic for checking and handling when bucket is milky
public class NipAudioHandler : MonoBehaviour
{
    //inspector variables
    [SerializeField]
    private GameManager gameManager = null;

    [SerializeField]
    [Range(0, 500)]
    [Tooltip("Score threshold when milking sounds turn milky")]
    private int scoreWhenBucketMilky = 50;

    [Range(0.0f, 1.0f)]
    [Tooltip("Minimum volume in random variation")]
    public float volumeMin = 0.8f;

    [Range(0.0f, 1.0f)]
    [Tooltip("Maximum volume in random variation")]
    public float volumeMax = 1.0f;

    [Range(0.0f, 1.0f)]
    [Tooltip("Minimum pitch in random variation")]
    public float pitchMin = 0.8f;

    [Range(0.0f, 2.0f)]
    [Tooltip("Maximum pitch in random variation")]
    public float pitchMax = 1.2f;

    public List<AudioClip> metalMilk = null;
    public List<AudioClip> wetMilk = null;

    //internal variables
    private bool isMilking = false;
    private bool bucketMilky = false;

    //make sure min variables aren't higher than max variables
    private void Start()
    {
        if(pitchMin > pitchMax)
            pitchMin = pitchMax;

        if (volumeMin > volumeMax)
            volumeMin = volumeMax;
    }

    // Update is called once per frame
    void Update()
    {
        //if not milking check if should be
        if(!isMilking)
        {
            if (gameManager.milkingAllowed)
            {
                //milking has started.
                isMilking = true;
                //bucket starts out empty
                bucketMilky = false;
                UpdateBucketMilky();
            }
            else return; //return if nothing happening
        }
        else //if currently milking
        {
            //check if bucket should get wet
            if (!bucketMilky && gameManager.getScore() > scoreWhenBucketMilky)
            {
                bucketMilky = true;
                UpdateBucketMilky();
            }
        } 
    }

    private void UpdateBucketMilky()
    {
        foreach (NipAudio n in GetComponentsInChildren<NipAudio>())
            n.setBucketMilky(bucketMilky);
    }
}
