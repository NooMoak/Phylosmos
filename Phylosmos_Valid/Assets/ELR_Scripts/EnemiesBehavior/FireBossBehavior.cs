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
    [SerializeField] GameObject insectRush;
    [SerializeField] float rushSpeed;
    [SerializeField] GameObject tornados;
    Vector3 homePosition;
    Rigidbody rb;
    Animator anim;
    int randomNumber;
    bool canShoot = true;
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
            //anim.SetBool("IsWalking", true);
        }
        else if (currentState == SpikeState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 10)
        {
            transform.LookAt(player.transform.position);
            //anim.SetBool("IsWalking", false);
            randomNumber = 1;
            if(canShoot && randomNumber == 1){
                StartCoroutine(InsectAttack());
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

    private IEnumerator InsectAttack()
    {
		canShoot = false;
        //anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        GameObject insectClone;
        insectClone = Instantiate(insectRush, transform.position, transform.rotation);
        insectClone.transform.position += transform.forward * 20;
        insectClone.transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
        yield return new WaitForSeconds(1);
        insectClone.GetComponent<Rigidbody>().AddForce(transform.forward * rushSpeed);
        yield return new WaitForSeconds(2f);
		canShoot = true;
        randomNumber = Random.Range(1,3);
    }
    private IEnumerator TornadoAttack()
    {
		canShoot = false;
        //anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        GameObject tornadoClone;
        tornadoClone = Instantiate(tornados, transform.position, transform.rotation);
        yield return new WaitForSeconds(2f);
		canShoot = true;
        randomNumber = Random.Range(1,3);
    }
}
