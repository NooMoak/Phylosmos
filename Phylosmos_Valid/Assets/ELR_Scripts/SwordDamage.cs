using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
	PlayerController giveAbility;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag ("Spike"))
        {
            Rigidbody Hit = collision.GetComponent<Rigidbody>();
            if (Hit != null)
            {
                Vector2 difference = Hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                Hit.AddForce(difference, ForceMode.Impulse);
				Hit.GetComponent<EnemyMovement>().currentState = EnemyState.Stagger;
				collision.GetComponent<EnemyMovement>().Knock(Hit, knockTime,damage);
				giveAbility = GetComponentInParent<PlayerController>();
				giveAbility.currentAbility = StolenAbility.Spike;
                giveAbility.abilityIcon.sprite = giveAbility.spikeIcon;
            }
        }
    }



    
}
