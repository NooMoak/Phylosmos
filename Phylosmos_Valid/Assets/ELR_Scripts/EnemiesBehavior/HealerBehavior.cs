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
    int randomNumber;
    Animator anim;
    // Use this for initialization
    void Start()
    {
        healLine = GetComponent<LineRenderer>();
        currentState = HealerState.Sleep;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
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
        else if (currentState == HealerState.Heal && Vector3.Distance(player.transform.position, homePosition) <= healRadius + 10)
        {
            if(Vector3.Distance(healTarget.transform.position, transform.position) > 10)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, healTarget.transform.position, healerSpeed * Time.deltaTime));
                anim.SetBool("IsWalking", true);
            }
            transform.LookAt(healTarget.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            healLine.SetPosition(0, new Vector3(transform.position.x, 1, transform.position.z));
            healLine.SetPosition(1, new Vector3(healTarget.transform.position.x, 1, healTarget.transform.position.z));
            if(canHeal)
                StartCoroutine("Heal");
        } 
        
        if (currentState == HealerState.Return && Vector3.Distance(player.transform.position, homePosition) > healRadius + 10 && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = HealerState.Sleep;
            anim.SetBool("IsWalking", false);
        }
        else if ((currentState == HealerState.Heal || currentState == HealerState.Return) && Vector3.Distance(player.transform.position, homePosition) > healRadius + 10 && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = HealerState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, healerSpeed/2 * Time.deltaTime));
            anim.SetBool("IsWalking", true);
        }
        else if (currentState == HealerState.Flee)
        {
            if(isFleeing == false)
            {
                randomNumber = Random.Range(1,4);
                vectorToPlayer = player.transform.position;
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsHealing", false);
                isFleeing = true;
            }
            if(randomNumber == 1)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, vectorToPlayer, -healerSpeed * Time.deltaTime));
                transform.LookAt(vectorToPlayer);
                transform.rotation = transform.rotation * Quaternion.Euler(0,-90,0);
            }
            else if(randomNumber == 2)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, Quaternion.Euler(0,45,0) * vectorToPlayer, -healerSpeed * Time.deltaTime));
                transform.LookAt(Quaternion.Euler(0,45,0) * vectorToPlayer);
                transform.rotation = transform.rotation * Quaternion.Euler(0,-90,0);
            }
            else if(randomNumber == 3)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, Quaternion.Euler(0,-5,0) * vectorToPlayer, -healerSpeed * Time.deltaTime));
                transform.LookAt(Quaternion.Euler(0,-5,0) * vectorToPlayer); 
                transform.rotation = transform.rotation * Quaternion.Euler(0,-90,0);
            }
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
    
    void Update() 
    {
        if(currentState == HealerState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsHealing", false);
            isFleeing = false;
            if(Vector3.Distance(player.transform.position, homePosition) < 100 && Vector3.Distance(player.transform.position, homePosition) > 90)
                StartCoroutine("Respawn");
        }
    }
    IEnumerator Heal()
    {
        canHeal = false;
        anim.SetBool("IsHealing", true);
        if(healTarget.GetComponent<EnemyLife>().health < healTarget.GetComponent<EnemyLife>().maxHealth ) 
        {
            healTarget.GetComponent<EnemyLife>().health += 1;
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsWalking", false);
        canHeal = true;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = homePosition;
        GetComponent<EnemyLife>().health = GetComponent<EnemyLife>().maxHealth;
        currentState = HealerState.Sleep;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
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

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Wall" && currentState == HealerState.Flee)
        {
            currentState = HealerState.Dead;
        }
    }
}