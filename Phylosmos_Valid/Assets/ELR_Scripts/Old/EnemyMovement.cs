using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,Walk,Attack,Stagger
}

public class EnemyMovement : MonoBehaviour
{
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }



    public void Knock (Rigidbody rb , float knockTime, float damage)
    {
        StartCoroutine(KnockCO(rb, knockTime));
        TakeDamage(damage);
    }
	

    private IEnumerator KnockCO(Rigidbody rb, float knockTime)
    {
        if (rb != null )
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = EnemyState.Idle;
            rb.velocity = Vector2.zero;
        }
    }
}
