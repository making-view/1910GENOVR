using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayerActions : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private string windowsVideoPath;
    [SerializeField] private string androidVideoPath;

    private string activePath;

    // Start is called before the first frame update
    void Start()
    {
        activePath = androidVideoPath;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        activePath = windowsVideoPath;
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CycleForward()
    {

    }

    public void CycleBackward()
    {

    }

    public void TogglePlay()
    {

    }
}
