using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0); //rotation speed in degrees per second
    public float floatAmplitude = 0.2f; //how much the object floats
    public float floatSpeed = 2f; //how fast the object floats

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //rotate the object
        transform.Rotate(rotationSpeed * Time.deltaTime);

        //floating effect
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
