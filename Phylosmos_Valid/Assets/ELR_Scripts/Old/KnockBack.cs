using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    /*/public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag ("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody Hit = collision.GetComponent<Rigidbody>();
            if (Hit != null)
            {
                Vector2 difference = Hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                Hit.AddForce(difference, ForceMode.Impulse);
                if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger )
                {
                    Hit.GetComponent<EnemyMovement>().currentState = EnemyState.Stagger;
                    collision.GetComponent<EnemyMovement>().Knock(Hit, knockTime,damage);

                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    Hit.GetComponent<PlayerController>().currentState = PlayerState.Stagger;
                    collision.GetComponent<PlayerController>().Knock(knockTime);
                }
            }
        }
		if(collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            GetComponent<EnemyMovement>().TakeDamage(1f);
        }
    }*/



    
}
