using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    public delegate void OnButtonPressedDelegate();
    public OnButtonPressedDelegate buttonPressedDelegate;

    [SerializeField] private GameObject topPoint = null;
    [SerializeField] private GameObject bottomPoint = null;

    private bool resetNeeded = false;
    private Vector3 midPoint;

    void Start()
    {
        midPoint = (topPoint.transform.position + bottomPoint.transform.position) / 2;
    }
    
    void Update()
    {
        if (transform.position.y > topPoint.transform.position.y)
        {
            transform.position = topPoint.transform.position;
        }

        if (transform.position.y < bottomPoint.transform.position.y)
        {
            transform.position = bottomPoint.transform.position;

            if (!resetNeeded)
            {            
                buttonPressedDelegate();
                resetNeeded = true;
            }
        }

        if (transform.position.y > midPoint.y)
        {
            resetNeeded = false;
        }
    }
}
