using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public enum HealerState
    {
        Sleep, Heal, Return, Flee, Dead
    }
public class HealerBehavior : MonoBehaviour
{
    GameObject healTarget;
    GameObject player;
    public HealerState currentState;
    [SerializeField] float healRadius;
    [SerializeField] float healerSpeed;
    Vector3 homePosition;
    Rigidbody rb;
    bool canHeal = true;
    LineRenderer healLine;
    LayerMask enemyToHealLayer;
    Vector3 vectorToPlayer;
    bool isFleeing = false;
    int index;
    Collider[] enemies;
    public GameObject test;
    // Use this for initialization
    void Start()
    {
        healLine = GetComponent<LineRenderer>();
        currentState = HealerState.Sleep;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        homePosition = transform.position;
        enemyToHealLayer = LayerMask.GetMask("EnemyToHeal");
    }

    void FixedUpdate() 
    {
        if((currentState == HealerState.Sleep || currentState == HealerState.Return) && Vector3.Distance(player.transform.position, homePosition) <= healRadius){
            currentState = HealerState.Heal;
            index = 0;
            Scan();
        } 
        else if (currentState == HealerState.Heal && Vector3.Distance(player.transform.position, homePosition) <= healRadius)
        {
            if(Vector3.Distance(healTarget.transform.position, transform.position) > 20)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, healTarget.transform.position, healerSpeed * Time.deltaTime));
            }
            transform.LookAt(healTarget.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            healLine.SetPosition(0, transform.position);
            healLine.SetPosition(1, healTarget.transform.position);
            if(canHeal)
                StartCoroutine("Heal");
        } 
        if ((currentState == HealerState.Heal || currentState == HealerState.Return) && Vector3.Distance(player.transform.position, homePosition) > healRadius + 10 && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = HealerState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, healerSpeed/2 * Time.deltaTime));
        }
        else if (currentState == HealerState.Return && Vector3.Distance(player.transform.position, homePosition) > healRadius + 10 && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = HealerState.Sleep;
        }
        else if (currentState == HealerState.Flee)
        {
            if(isFleeing == false)
            {
                vectorToPlayer = player.transform.position - transform.position ;
                test.transform.position = vectorToPlayer ;
                isFleeing = true;
            }
            rb.MovePosition(Vector3.MoveTowards(transform.position, -(vectorToPlayer), healerSpeed * Time.deltaTime));
        }

        if(healTarget != null)
        {
            if((healTarget.tag == "Spike" && healTarget.GetComponent<SpikeBehavior>().currentState == SpikeState.Dead) || (healTarget.tag == "Liana" && healTarget.GetComponent<LianaBehavior>().currentState == LianaState.Dead) || healTarget.tag == "Rock" && healTarget.GetComponent<RockBehavior>().currentState == RockState.Dead)
            {
                if(index < enemies.Length - 1)
                {
                    index += 1;
                    healTarget = enemies[index].gameObject;
                }
                else 
                {
                    currentState = HealerState.Flee;
                }
            }
        }

        if(currentState == HealerState.Heal)
        {
            healLine.enabled = true;
        } 
        else 
        {
            healLine.enabled = false;
        }
    }
    
    IEnumerator Heal()
    {
        canHeal = false;
        if(healTarget.GetComponent<EnemyLife>().health < healTarget.GetComponent<EnemyLife>().maxHealth ) 
        {
            healTarget.GetComponent<SpikeEnemy>().health += 2;
        }
        yield return new WaitForSeconds(0.5f);
        canHeal = true;
    }

    void Scan()
    {
        enemies = Physics.OverlapSphere(homePosition, healRadius, enemyToHealLayer);
        if(enemies.Length > 0)
        {
            healTarget = enemies[0].gameObject;
        }
        else
        {
            currentState = HealerState.Flee;
        }
    }
}