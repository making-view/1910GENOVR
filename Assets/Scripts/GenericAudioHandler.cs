using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//struct list for containing lists of soundclips
[System.Serializable]
public struct ClipList
{
    public string name;
    public List<AudioClip> clip;
}

public class GenericAudioHandler : MonoBehaviour
{
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

    [SerializeField]
    protected string startingClips = "";

    //named lists of audio-clips
    [SerializeField]
    public ClipList[] audioClips = null;
    protected List<AudioClip> activeClips = null;
    private AudioClip lastPlayed = null;

    //make sure min variables aren't higher than max variables
    //set starting clips to the one named in editor
    private void Start()
    {
        if (pitchMin > pitchMax)
            pitchMin = pitchMax;

        if (volumeMin > volumeMax)
            volumeMin = volumeMax;

        if (!startingClips.Equals(""))
            updateActiveClips(startingClips);
    }

    //get previously played audioclip
    public AudioClip getAudioClip()
    {
        List<AudioClip> clipsToPlay = new List<AudioClip>(activeClips);

        //don't play same as last time
        if (lastPlayed != null && clipsToPlay.Count > 1)
            clipsToPlay.Remove(lastPlayed);
        //find new clip to play
        AudioClip clip = clipsToPlay[Random.Range(0, clipsToPlay.Count)];

        lastPlayed = clip;

        return clip;
    }

    //get list with name defined in editor
    protected void updateActiveClips(string nameOfList)
    {
        foreach(ClipList c in audioClips)
        {
            if (c.name.ToUpper().Equals(nameOfList.ToUpper()))
            {
                activeClips = c.clip;
                return;
            }
        }

        Debug.Log(this.name + " : update active clips didn't find list: " + nameOfList);
    }
}