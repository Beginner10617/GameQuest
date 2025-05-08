using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialFlash : MonoBehaviour
{
    public float timeToFlash = 10f; // Time in seconds to flash
    float timer = 0f; // Timer to track elapsed time
    void Update()
    {
        timer += Time.deltaTime; // Increment timer by the time since last frame
        if(timer >= timeToFlash) // Check if the timer has reached the specified time
        {
            Destroy(gameObject); // Destroy the GameObject this script is attached to
        }
    }
}
