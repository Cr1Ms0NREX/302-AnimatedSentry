using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollison : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            EnemyHealth healthComponent = collision.GetComponent<EnemyHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(40);
            }
        }
        if (collision.tag == "Player")
        {
            PlayerHealth healthComponent = collision.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(20);
            }
        }
    }
}
