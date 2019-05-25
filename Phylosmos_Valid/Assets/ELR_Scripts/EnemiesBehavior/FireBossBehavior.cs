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
    public BossState currentState;
    [SerializeField] float fightRadius;
    [SerializeField] float bossSpeed;
    [SerializeField] GameObject targetRotation;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject insectRush;
    [SerializeField] float rushSpeed;
    [SerializeField] GameObject tornados;
    [SerializeField] float tornadoSpeed;
    Vector3 homePosition;
    Rigidbody rb;
    Animator anim;
    int randomNumber;
    public bool canShoot = true;
    public bool shooting = false;
    [SerializeField] AudioClip audioBall;
    [SerializeField] AudioClip audioTornado;
    [SerializeField] AudioClip audioBurn;
    [SerializeField] AudioClip audioFly;
    [SerializeField] AudioClip audioRoar;
    [SerializeField] GameObject particles;

    bool intro = false;
    bool firstPhase = false;
    bool secondPhase = false;
    bool burning = false;
    bool flying = false;
    bool landing = false; 
    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
        randomNumber = Random.Range(1,3);
        StartCoroutine(RoarStart());
    }

    void Update()
    {
        if(GetComponent<EnemyLife>().health <= 50 && secondPhase == false)
        {   
            secondPhase = true;
            anim.SetBool("Flying", true);
            GetComponent<AudioSource>().clip = audioFly;
            GetComponent<AudioSource>().Play();
            GetComponent<Rigidbody>().isKinematic = true;
            canShoot = false;
            StartCoroutine(PhaseTwo());
        }
        if(currentState == BossState.Dead)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            //transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            anim.SetBool("IsWalking", false);
        }
        if(burning)
        {
            //player.GetComponent<PlayerDamage>().TakeDamage(0.26f);
        }
        if(flying && transform.position.y < 200)
        {
            transform.position += new Vector3(0,0.7f,0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        }
        if(landing && transform.position.y > 7)
        {
            if(firstPhase)
            {
                firstPhase = false;
                GetComponent<Rigidbody>().isKinematic = false;
                canShoot = false;
            }
            transform.position -= new Vector3(0,1f,0);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        }
        else if(landing && transform.position.y <= 7)
        {
            anim.SetBool("Landing", true);
            anim.SetBool("Flying", false);
            canShoot = true;
            landing = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((currentState == BossState.Sleep || currentState == BossState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = BossState.Fight;
            anim.SetFloat("StateSpeed", 1f); 
        }
        else if(currentState == BossState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 130){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, bossSpeed * Time.deltaTime));
            targetRotation.transform.LookAt(player.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", true);
        }
        else if (currentState == BossState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 130)
        {
            targetRotation.transform.LookAt(player.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", false);
            if(canShoot && !shooting && randomNumber == 1){
                canShoot = false;
                shooting = true;
                anim.SetTrigger("FireBall");
            }
            else if (canShoot && !shooting && randomNumber == 2)
            {
                canShoot = false;
                shooting = true;
                anim.SetTrigger("FireTornado");
            }
            else if (canShoot == false && shooting == false && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 40 && Vector3.Distance(player.transform.position, homePosition) < fightRadius + 10)
            {
                rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, bossSpeed * Time.deltaTime));
                targetRotation.transform.LookAt(player.transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
                anim.SetBool("IsWalking", true);
            }
        }
        if ((currentState == BossState.Fight || currentState == BossState.Return) && Vector3.Distance(player.transform.position, homePosition) > fightRadius && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = BossState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, bossSpeed/2 * Time.deltaTime));
            anim.SetFloat("StateSpeed", -0.5f); 
        }
        else if (currentState == BossState.Return && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = BossState.Sleep;
            anim.SetBool("IsWalking", false);
        }
    }

    public void FireBall()
    {
        StartCoroutine(InsectAttack());
    }
    private IEnumerator InsectAttack()
    {
        yield return new WaitForSeconds(0.35f);
        shooting = true;
        GameObject insectClone;
        insectClone = Instantiate(insectRush, transform.position, transform.rotation);
        insectClone.transform.position += transform.forward * 20;
        insectClone.transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().clip = audioBall;
        GetComponent<AudioSource>().Play();
        insectClone.GetComponent<Rigidbody>().AddForce(transform.forward * rushSpeed);
        Destroy(insectClone, 3f);
        yield return new WaitForSeconds(0.5f);
        shooting = false;
        yield return new WaitForSeconds(7f);
		canShoot = true;
        randomNumber = Random.Range(1,3);
    }
    
    public void FireTornado()
    {
        StartCoroutine(TornadoAttack());
    }
    private IEnumerator TornadoAttack()
    {
        yield return new WaitForSeconds(0.35f);
        GetComponent<AudioSource>().clip = audioTornado;
        GetComponent<AudioSource>().Play();
        GameObject tornadoClone;
        tornadoClone = Instantiate(tornados, transform.position, transform.rotation);
        tornadoClone.GetComponent<Rigidbody>().AddForce(transform.forward * tornadoSpeed);
        Destroy(tornadoClone, 3f);
        yield return new WaitForSeconds(0.5f);
        shooting = false;
        yield return new WaitForSeconds(7f);
		canShoot = true;
        randomNumber = Random.Range(1,3);
    }
    IEnumerator PhaseTwo()
    {
        yield return new WaitForSeconds(2);
        particles.SetActive(true);
        flying = true;
        burning = true;
        GetComponent<AudioSource>().clip = audioBurn;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(20);
        burning = false;
        particles.SetActive(false);
        GetComponent<AudioSource>().Stop();
        secondPhase = true;
        flying = false;
        landing = true;
        firstPhase = true;
    }

    IEnumerator RoarStart()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().clip = audioRoar;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5f);
        GetComponent<AudioSource>().volume = 0.5f;
    }
}
