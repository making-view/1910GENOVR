using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject objectToTeleport;
    [SerializeField] private Transform destination;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand")
        {
            objectToTeleport.transform.position = destination.position;
        }
    }
}
