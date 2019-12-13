using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Video;

public class VideoPlayerActions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private Font menuFont = null;
    [SerializeField] private string windowsVideoPath;
    [SerializeField] private string androidVideoPath;

    private string activePath;
    private List<VideoFile> videoFiles;
    private int currentVideoIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        videoFiles = new List<VideoFile>();

        activePath = androidVideoPath;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        activePath = windowsVideoPath;
#endif

        var directoryInfo = new DirectoryInfo(activePath);
        var fileInfos = directoryInfo.GetFiles();

        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var file in fileInfos)
        {
            if (file.Extension == ".MOV")
            {
                var newFile = new GameObject();
                newFile.name = "Video";

                var text = newFile.AddComponent<Text>();
                text.font = menuFont;
                text.fontSize = 35;
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                text.text = file.Name.Substring(0, file.Name.IndexOf(file.Extension));

                newFile.transform.SetParent(verticalLayoutGroup.transform, false);

                videoFiles.Add(new VideoFile(text, file.ToString()));
            }
        }

        videoFiles.First().text.color = Color.red;

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void CycleForward()
    {
        if (!videoPlayer.isPlaying)
        {
            videoFiles[currentVideoIndex].text.color = Color.black;
            currentVideoIndex = (currentVideoIndex + 1) % videoFiles.Count;
            videoFiles[currentVideoIndex].text.color = Color.red;
        }
    }

    public void CycleBackward()
    {
        if(!videoPlayer.isPlaying)
        {
            videoFiles[currentVideoIndex].text.color = Color.black;
            currentVideoIndex = currentVideoIndex - 1 < 0 ? currentVideoIndex = videoFiles.Count - 1 : currentVideoIndex - 1;
            videoFiles[currentVideoIndex].text.color = Color.red;
        }
    }

    public void TogglePlay()
    {
        if (videoPlayer.isPlaying)
        {
            OnVideoFinished(videoPlayer);
        }
        else
        {
            verticalLayoutGroup.gameObject.SetActive(false);
            videoPlayer.url = videoFiles[currentVideoIndex].path;
            videoPlayer.Play();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop();
        vp.url = "";
        verticalLayoutGroup.gameObject.SetActive(true);
    }
}
