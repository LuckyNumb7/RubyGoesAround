using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCandy : MonoBehaviour
{
    public AudioClip candyClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.SpeedBoost(1);
            Destroy(gameObject);

            controller.PlaySound(candyClip);
        }

    }
} 
