using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public bool invicible;
    [SerializeField] GameObject healthBar;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.rotation = Quaternion.Euler(0,-45,-30);
        healthBar.transform.localScale = new Vector3(0.5f, 1, health);
    }

    public void TakeDamage(int damage)
    {
        if(invicible == false)
        {
            health -= damage;
            anim.SetTrigger("Hurt");
            CheckHealth();
        }
    }

    void CheckHealth()
    {
        if(health < 1)
        {
            if(gameObject.tag == "Spike")
            {
                gameObject.GetComponent<SpikeBehavior>().currentState = SpikeState.Dead;
            }
            if(gameObject.tag == "Liana")
            {
                gameObject.GetComponent<LianaBehavior>().currentState = LianaState.Dead;
            }
            if(gameObject.tag == "Healer")
            {
                gameObject.GetComponent<HealerBehavior>().currentState = HealerState.Dead;
            }
            if(gameObject.tag == "Rock")
            {
                gameObject.GetComponent<RockBehavior>().currentState = RockState.Dead;
            }
        }
    }
}
