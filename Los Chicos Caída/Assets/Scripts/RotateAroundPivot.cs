using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPivot : MonoBehaviour
{
    public float rotateSpeed = 10;
    private int direction = 1;

    private void Awake()
    {
        direction = Random.Range(0, 2) * 2 - 1;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * direction * rotateSpeed, 0);
    }
}
