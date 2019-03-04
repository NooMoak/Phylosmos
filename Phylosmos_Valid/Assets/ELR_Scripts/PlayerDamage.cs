using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    int PlayerHealth;
    [SerializeField]
    Image healthBar;

    private void Start() 
    {
        PlayerHealth = 100;
    }
    private void OnTriggerEnter(Collider collision)
    {
		if(collision.gameObject.CompareTag("SpikeProjectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
        healthBar.fillAmount = PlayerHealth;
        CheckHealth();
    }

    void CheckHealth()
    {
        if(PlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
