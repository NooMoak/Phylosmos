using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpikeState
    {
        Sleep, Fight, Return, Dead
    }
public class SpikeBehavior : MonoBehaviour
{
    GameObject player;
    public SpikeState currentState;
    [SerializeField] float fightRadius;
    [SerializeField] float spikeSpeed;
    [SerializeField] GameObject spikeProjectile;
    [SerializeField] float launchForce = 1000f;
    [SerializeField] GameObject targetL;
    [SerializeField] GameObject targetR;
    Vector3 homePosition;
    Rigidbody rb;
    bool canShoot = true;
    int randomNumber;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentState = SpikeState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        randomNumber = Random.Range(1,5);
    }

    void Update()
    {
        if(currentState == SpikeState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            if(Vector3.Distance(player.transform.position, homePosition) < 100 && Vector3.Distance(player.transform.position, homePosition) > 90)
                StartCoroutine("Respawn");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((currentState == SpikeState.Sleep || currentState == SpikeState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = SpikeState.Fight;
        }
        else if(currentState == SpikeState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 10){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, spikeSpeed * Time.deltaTime));
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
        }
        else if (currentState == SpikeState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 10)
        {
            transform.LookAt(player.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
            if(canShoot && randomNumber == 1 || canShoot && randomNumber == 2){
                StartCoroutine(SpikeShoot());
            }
            else if (randomNumber == 3)
            {
                MoveToSpot(targetL.transform.position);
            }
            else if (randomNumber == 4)
            {
                MoveToSpot(targetR.transform.position);
            }
        }
        if ((currentState == SpikeState.Fight || currentState == SpikeState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = SpikeState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, spikeSpeed/2 * Time.deltaTime));
        }
        else if (currentState == SpikeState.Return && Vector3.Distance(player.transform.position, homePosition) > fightRadius + 10 && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = SpikeState.Sleep;
        }
    }

    private IEnumerator SpikeShoot()
    {
		canShoot = false;

        yield return new WaitForSeconds(0.2f);

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
    }

    void MoveToSpot(Vector3 spot)
    {
        if(timer < 1f)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, spot, spikeSpeed * Time.deltaTime);
            rb.MovePosition(temp);
            timer += Time.deltaTime;
        } 
        else 
        {
            randomNumber = Random.Range(1,5);
            timer = 0f;
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = homePosition;
        GetComponent<EnemyLife>().health = GetComponent<EnemyLife>().maxHealth;
        currentState = SpikeState.Sleep;
        GetComponent<CapsuleCollider>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
    }
}
