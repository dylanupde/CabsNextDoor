using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickUp")
        {
            Debug.Log("pickup pls");
            Destroy(this);
        }
    }
}
