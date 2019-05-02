using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
    {
        Sleep, Fight, Return, Dead
    }
public class FireBossBehavior : MonoBehaviour
{
    GameObject player;
    public SpikeState currentState;
    [SerializeField] float fightRadius;
    [SerializeField] float bossSpeed;
    Vector3 homePosition;
    Rigidbody rb;
    Animator anim;
    int randomNumber;
    bool canShoot;
    // Start is called before the first frame update
    void Start()
    {
        currentState = SpikeState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
        randomNumber = Random.Range(1,3);
    }

    void Update()
    {
        if(currentState == SpikeState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            //anim.SetBool("IsWalking", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((currentState == SpikeState.Sleep || currentState == SpikeState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = SpikeState.Fight;
            //anim.SetFloat("StateSpeed", 1f); 
        }
        else if(currentState == SpikeState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 10){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, bossSpeed * Time.deltaTime));
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            //anim.SetBool("IsWalking", true);
        }
        else if (currentState == SpikeState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 10)
        {
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            anim.SetBool("IsWalking", false);
            if(canShoot && randomNumber == 1){
                StartCoroutine(SpikeShoot());
            }
            else if (canShoot && randomNumber == 2)
            {

            }
        }
        if ((currentState == SpikeState.Fight || currentState == SpikeState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = SpikeState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, bossSpeed/2 * Time.deltaTime));
            //anim.SetFloat("StateSpeed", -0.5f); 
        }
        else if (currentState == SpikeState.Return && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = SpikeState.Sleep;
            //anim.SetBool("IsWalking", false);
        }
    }

    private IEnumerator SpikeShoot()
    {
		canShoot = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        /* 
        GameObject clone1;
        GameObject clone2;
        GameObject clone3;
        Vector3 look = player.transform.position - transform.position;
        clone1 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone1.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,90,90);
        Vector3 dir = (player.transform.position + new Vector3(0,2,0)) - clone1.transform.position;
        dir = dir.normalized;
        clone1.GetComponent<Rigidbody>().AddForce(dir * launchForce);

        clone2 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone2.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,135,90);
        clone2.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(45f, Vector3.up) * dir * launchForce);

        clone3 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone3.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,45,90);
        clone3.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(-45f, Vector3.up) * dir * launchForce);

		yield return new WaitForSeconds(2f);
		canShoot = true;
        randomNumber = Random.Range(1,5);
        */
    }
}
