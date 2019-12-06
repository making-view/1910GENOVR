using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableButton : MonoBehaviour
{
    [SerializeField] private float endpoint;

    public bool pressed;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - startPos.y < endpoint)
        {

        }
    }
}