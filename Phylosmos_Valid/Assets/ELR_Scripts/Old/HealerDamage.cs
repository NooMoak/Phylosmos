using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
		if(collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            GetComponent<EnemyMovement>().TakeDamage(1f);
        }
		if(collision.gameObject.CompareTag("SniperBullet"))
        {
            Destroy(collision.gameObject);
            GetComponent<EnemyMovement>().TakeDamage(3f);
        }
    }  
}
