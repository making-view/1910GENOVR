using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAudioSource : MonoBehaviour
{
    
    protected AudioSource audioSource;

    [SerializeField]
    [Tooltip("Audio handler containing resources for audiosource to play. can be in parent or defined here")]
    protected GenericAudioHandler audioHandler = null;

    //internal list updated each time it plays audio
    protected float pitchMin, pitchMax, volumeMin, volumeMax;

    //find audio handler on start
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        //try get audio handler from parent if reference not set in editor
        if (audioHandler == null)
            audioHandler = gameObject.GetComponentInParent<GenericAudioHandler>();

        if (audioHandler == null)
            throw new System.Exception("no audio handler found", null);
        else
            Debug.Log("AudioHandler found, " + this.gameObject.name);

        updateVariablesFromHandler();
    }

    //update variables from audioHandler
    virtual protected void updateVariablesFromHandler()
    {
        pitchMax = audioHandler.pitchMax;
        pitchMin = audioHandler.pitchMin;
        volumeMax = audioHandler.volumeMax;
        volumeMin = audioHandler.volumeMin;

        Debug.Log("updated variables from Generic audio source handler");
    }

    virtual public void playSound()
    {
        //stop if still playing sound
        if (audioSource.isPlaying)
            return;

        //add variations to audio
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        //play audio
        audioSource.clip = audioHandler.getAudioClip();
        audioSource.Play();
    }



}