using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains resources for nip-audio as well as logic for checking and handling when bucket is milky
public class NipAudioHandler : GenericAudioHandler
{
    //inspector variables
    [SerializeField]
    private GameManager gameManager = null;

    [SerializeField]
    [Tooltip("Score threshold when milking sounds turn milky")]
    private int scoreWhenBucketMilky = 500;    


    //internal variables
    private bool isMilking = false;
    private bool bucketMilky = false;

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
                updateActiveClips("milkyMetal");
            }
            else return; //return if nothing happening
        }
        else //if currently milking
        {
            //check if bucket should get wet
            if (!bucketMilky && gameManager.getScore() > scoreWhenBucketMilky)
            {
                bucketMilky = true;
                updateActiveClips("milkyWet");
            }
        } 
    }
}
