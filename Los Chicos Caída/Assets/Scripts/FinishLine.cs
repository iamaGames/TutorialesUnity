using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject confetti; //Confetti particle system
    public Transform confettiSpawningPoint;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject confettiInstance = Instantiate(confetti);
            confettiInstance.transform.position = confettiSpawningPoint.position;
        }
    }
}
