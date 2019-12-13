using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoFile
{
    public VideoFile(Text t, string p)
    {
        text = t;
        path = p;
    }

    public Text text;
    public string path;
}
