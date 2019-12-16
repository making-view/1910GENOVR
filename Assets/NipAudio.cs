using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NipAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private NipAudioHandler nipAudioHandler = null;

    private List<AudioClip> metalMilk = null;
    private List<AudioClip> wetMilk = null;
    private List<AudioClip> clipsToPlay = null;

    private bool bucketMilky = false;
    private float pitchMin, pitchMax, volumeMin, volumeMax;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        //get nip audio handler containing audio resources
        nipAudioHandler = gameObject.GetComponentInParent<NipAudioHandler>();

        if (nipAudioHandler == null)
            throw new System.Exception("no nip audio handler found", null);
        else
            Debug.Log("NipAudioHandler found, " + this.gameObject.name);

        //variables to get from audio handler on start (and update in editor)
        //lists of sounds
        metalMilk = nipAudioHandler.metalMilk;
        wetMilk = nipAudioHandler.wetMilk;
        //floats
        pitchMax = nipAudioHandler.pitchMax;
        pitchMin = nipAudioHandler.pitchMin;
        volumeMax = nipAudioHandler.volumeMax;
        volumeMin = nipAudioHandler.volumeMin;
    }

    //play milking sound
    public void playSound()
    {

        //stop if still playing sound
        if (audioSource.isPlaying)
            return;

        //check what type of sound to play
        if(bucketMilky)
            clipsToPlay = new List<AudioClip>(wetMilk);
        else
            clipsToPlay = new List<AudioClip>(metalMilk);

        //don't play same as last time
        if (nipAudioHandler.getLast() != null)
        clipsToPlay.Remove(nipAudioHandler.getLast());
        //find new clip to play
        AudioClip clip = clipsToPlay[Random.Range(0, clipsToPlay.Count)];
        //add variations to audio
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        //play audio
        audioSource.clip = clip;
        audioSource.Play();
        //update previous clip
        nipAudioHandler.setLast(clip);
    }

    //update if bucket is full of milk or not
    public void setBucketMilky(bool bucketMilky)
    {
        this.bucketMilky = bucketMilky;
    }
}
