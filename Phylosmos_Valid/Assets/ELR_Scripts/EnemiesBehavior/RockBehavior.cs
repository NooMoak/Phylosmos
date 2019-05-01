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
    Rigidbody rb;
    bool canShockWave = true;
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
        if(currentState == RockState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            anim.SetBool("IsWalking", false);
            if(Vector3.Distance(player.transform.position, homePosition) < 100 && Vector3.Distance(player.transform.position, homePosition) > 90)
                StartCoroutine("Respawn");
        }
    }

    void FixedUpdate() 
    {
        if((currentState == RockState.Sleep || currentState == RockState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = RockState.Fight;
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
            if(canShockWave == true)
            {
                StartCoroutine("ShockWave");
            }
        }
        if ((currentState == RockState.Fight || currentState == RockState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = RockState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, rockSpeed/2 * Time.deltaTime));
        }
        else if (currentState == RockState.Return && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = RockState.Sleep;
            anim.SetBool("IsWalking", false);
        }
    
    }
    
    IEnumerator ShockWave()
    {
        canShockWave = false;
        yield return new WaitForSeconds(2);
        Vector3 explosionPos = transform.position + new Vector3(0,5,0);
        Collider[] colliders = Physics.OverlapSphere(explosionPos, rockRadius);
        foreach(Collider hit in colliders)
        {
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            
            if(hitRb == player.GetComponent<Rigidbody>())
            {
                hitRb.AddExplosionForce(rockForce, explosionPos, rockRadius);
            }
        }
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
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
    }

}
