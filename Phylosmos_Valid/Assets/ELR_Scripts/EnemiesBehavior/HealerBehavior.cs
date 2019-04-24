using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public enum HealerState
    {
        Sleep, Heal, Flee, Dead
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
        if(currentState == HealerState.Sleep && Vector3.Distance(player.transform.position, homePosition) <= healRadius){
            currentState = HealerState.Heal;
            Collider[] enemies = Physics.OverlapSphere(homePosition, healRadius, enemyToHealLayer);
            healTarget = enemies[0].gameObject;
            if(enemies[0] == null)
                currentState = HealerState.Flee;
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
        else if (currentState == HealerState.Heal && Vector3.Distance(player.transform.position, homePosition) > healRadius && Vector3.Distance(homePosition, transform.position) > 0)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, healerSpeed * Time.deltaTime));
        }
        else if (currentState == HealerState.Heal && Vector3.Distance(player.transform.position, homePosition) > healRadius && Vector3.Distance(homePosition, transform.position) == 0)
        {
            currentState = HealerState.Sleep;
        }
        else if (currentState == HealerState.Flee)
        {
            Vector3 vectorToPlayer = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            rb.MovePosition(transform.position + -vectorToPlayer * healerSpeed);
        }
        if(healTarget != null)
        {
            if((healTarget.tag == "Spike" && healTarget.GetComponent<SpikeBehavior>().currentState == SpikeState.Dead) || (healTarget.tag == "Liana" && healTarget.GetComponent<LianaBehavior>().currentState == LianaState.Dead) || (healTarget.tag == "Healer" && healTarget.GetComponent<HealerBehavior>().currentState == HealerState.Dead) || healTarget.tag == "Rock" && healTarget.GetComponent<RockBehavior>().currentState == RockState.Dead)
            {
                currentState = HealerState.Sleep;
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
}