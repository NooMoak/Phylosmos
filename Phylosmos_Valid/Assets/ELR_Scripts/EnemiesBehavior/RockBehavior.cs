using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RockState
    {
        Sleep, Fight, Return, Dead
    }
public class RockBehavior : MonoBehaviour
{
    GameObject player;
    public RockState currentState;
    [SerializeField] float fightRadius;
    [SerializeField] float rockSpeed;
    [SerializeField] float rockForce;
    [SerializeField] float rockRadius;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject targetRotation;
    Vector3 homePosition;
    Vector3 sight;
    float fieldOfShield = 180f;
    Rigidbody rb;
    bool canShockWave = true;
    bool shockWaving = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        currentState = RockState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        sight = -transform.right;
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, sight);
        if(angle < fieldOfShield * 0.5f){
            GetComponent<EnemyLife>().invicible = true;
        } else 
        {
            GetComponent<EnemyLife>().invicible = false;
        }

        if(currentState == RockState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            //GetComponentInChildren<MeshRenderer>().enabled = false;
            anim.SetBool("IsWalking", false);
            if(Vector3.Distance(player.transform.position, homePosition) < 120 && Vector3.Distance(player.transform.position, homePosition) > 100)
                StartCoroutine("Respawn");
        }
    }

    void FixedUpdate() 
    {
        if((currentState == RockState.Sleep || currentState == RockState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = RockState.Fight;
            anim.SetFloat("StateSpeed", 1f); 
        }
        else if(currentState == RockState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 20){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, rockSpeed * Time.deltaTime));
            targetRotation.transform.LookAt(player.transform.position);
            targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", true);
        }
        else if (currentState == RockState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 20)
        {
            targetRotation.transform.LookAt(player.transform.position);
            targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", false);
            if(canShockWave == true && shockWaving == false)
            {
                StartCoroutine("ShockWave");
            }
            else if (canShockWave == false && shockWaving == false && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 35 && Vector3.Distance(player.transform.position, homePosition) < fightRadius + 10)
            {
                targetRotation.transform.LookAt(player.transform.position);
                targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
                rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, rockSpeed * Time.deltaTime));
                anim.SetBool("IsWalking", true);
            }
        }
        if ((currentState == RockState.Fight || currentState == RockState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = RockState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, rockSpeed/2 * Time.deltaTime));
            anim.SetFloat("StateSpeed", -0.5f); 
        }
        else if (currentState == RockState.Return && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = RockState.Sleep;
            anim.SetBool("IsWalking", false); 
        }
    
    }
    
    IEnumerator ShockWave()
    {
        canShockWave = false;
        shockWaving = true;
        anim.SetTrigger("ShockWave");
        yield return new WaitForSeconds(0.5f);
        shockWaving = false;
        yield return new WaitForSeconds(5);
        canShockWave = true;
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = homePosition;
        GetComponent<EnemyLife>().health = GetComponent<EnemyLife>().maxHealth;
        currentState = RockState.Sleep;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        //GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    public void ShockWaveAnim()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, rockRadius);
        foreach(Collider hit in colliders)
        {
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            
            if(hitRb == player.GetComponent<Rigidbody>())
            {
                float distance = Vector3.Distance(hitRb.transform.position, explosionPos);
                hitRb.AddForce(-(transform.position - player.transform.position) * rockForce * ((1/distance) * 100));
                player.GetComponent<PlayerDamage>().TakeDamage(10f);
            }
        }
    }

}
