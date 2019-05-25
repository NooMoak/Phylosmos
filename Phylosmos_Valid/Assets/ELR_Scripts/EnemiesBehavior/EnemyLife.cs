using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool invicible;
    //[SerializeField] GameObject healthBar;
    //[SerializeField] GameObject healthBarContainer;
    [SerializeField] AudioClip audioHurt1;
    [SerializeField] AudioClip audioHurt2;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        anim = GetComponentInChildren<Animator>();
        //healthBar.transform.localScale = new Vector3(0.5f, 0.5f, maxHealth);     
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.transform.rotation = Quaternion.Euler(0,0,0);
        //healthBarContainer.transform.rotation = Quaternion.Euler(0,-45,-30);
        //float healthToDisplay = Mathf.Lerp(healthBarContainer.transform.localScale.z, health/maxHealth, 0.2f);
        //healthBarContainer.transform.localScale = new Vector3 (1, 1, healthToDisplay);
    }

    public void TakeDamage(int damage)
    {
        if(invicible == false)
        {
            health -= damage;
            anim.SetTrigger("Hurt");
            int random = Random.Range(1,3);
            if(random == 1)
            {
                GetComponent<AudioSource>().clip = audioHurt1;
            }
            if(random == 2)
            {
                GetComponent<AudioSource>().clip = audioHurt2;
            }
            GetComponent<AudioSource>().Play();
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
            if(gameObject.tag == "Boss")
            {
                gameObject.GetComponent<FireBossBehavior>().currentState = BossState.Dead;
            }
        }
    }
}
