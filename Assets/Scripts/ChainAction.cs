using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAction : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = startRot;
        transform.position = new Vector3(startPos.x, transform.position.y, startPos.z);
    }
}