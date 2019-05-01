using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LianaState
    {
        Sleep, Fight, Stun, Return, Dead
    }
public class LianaBehavior : MonoBehaviour
{
    public LianaState currentState;
     GameObject player;
    [SerializeField] float fightRadius;
    [SerializeField] float lianaSpeed;
    [SerializeField] GameObject grab;
    [SerializeField] float grabSpeed;
    Vector3 homePosition;
    Vector3 grabTarget;
    Rigidbody rb;
    bool canGrab = true;
    bool grabbing = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        currentState = LianaState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == LianaState.Dead)
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
        if((currentState == LianaState.Sleep || currentState == LianaState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = LianaState.Fight;
        }
        else if(currentState == LianaState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 20){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, lianaSpeed * Time.deltaTime));
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            grab.transform.position = transform.position;
            anim.SetBool("IsWalking", true);
        }
        else if (currentState == LianaState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 20 && grabbing == false)
        {
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            anim.SetBool("IsWalking", false);
            if(canGrab == true)
            {
                StartCoroutine("Grab");
            }
        }
        if ((currentState == LianaState.Fight || currentState == LianaState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = LianaState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, lianaSpeed/2 * Time.deltaTime));
        }
        else if (currentState == LianaState.Return && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = LianaState.Sleep;
            //anim.SetBool("IsWalking", false);
        }
    
    }

    IEnumerator Grab()
    {
        canGrab = false;
        anim.SetTrigger("Grab");
        yield return new WaitForSeconds(0.5f);
        grabbing = true;
        GameObject lianaClone;
        lianaClone = Instantiate(grab, transform.position + new Vector3(0,3,0), transform.rotation);
        lianaClone.GetComponent<GrabScript>().lianaEnemy = gameObject;
        lianaClone.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(-(transform.position - player.transform.position)) * grabSpeed * 50);
        yield return new WaitForSeconds(2f);
        grabbing = false;
        yield return new WaitForSeconds(6f);
        canGrab = true;
    }

    void GrabTravel(Vector3 target)
    {
        
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = homePosition;
        GetComponent<EnemyLife>().health = GetComponent<EnemyLife>().maxHealth;
        currentState = LianaState.Sleep;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
    }
}
