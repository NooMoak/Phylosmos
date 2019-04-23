using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
    {
        Idle,Walk,Attack,Interact, Stagger
    }
public enum StolenAbility
    {
        None, Liana, Spike, Rock, Healer
    }

public class PlayerController : MonoBehaviour
{
	[SerializeField]
    float p_Speed;
	private Rigidbody rb;
	Vector3 forward; 
	Vector3 right;
    Vector3 mousePos;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject sniperBullet;
    [SerializeField]
    float bulletForce = 1000f;
    bool canShoot = true;
    public bool abilityReady = true;
    public PlayerState currentState;
    public StolenAbility currentAbility;
	RaycastHit hit;
	Ray ray;
    [SerializeField]
    GameObject attackHitbox;
    [SerializeField] 
    Animator anim;
    [SerializeField] 
    Text magazineText;
    int bulletFired = 0;
    public Image abilityIcon;
    public Sprite spikeIcon;
    public Sprite spikeCDIcon;
    public Sprite healerIcon;
    public Sprite healerCDIcon;
    int rayPlaneMask;
    Vector3 look;

	void Start ()
    {
		rb = GetComponent<Rigidbody>();
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize(forward);
		right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
        currentState = PlayerState.Idle;
        currentAbility = StolenAbility.None;
        rayPlaneMask = LayerMask.GetMask("RayPlane");
	}

    void Update()
    {
        string magazineToDisplay = (10 - bulletFired).ToString() + " / 10";
        magazineText.text = magazineToDisplay;

        //Sword Attack
        if (Input.GetButtonDown ("Fire2"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(AttackCO());
            }
        }

        //Rifle Shoot
        if (Input.GetButton ("Fire1"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && canShoot)
            {
                if(bulletFired < 10)
                {
                    StartCoroutine(Fire());
                } else 
                {
                    StartCoroutine(Reload());
                }
            }
        }

        //Ability Launch
        if (Input.GetKeyDown (KeyCode.E))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && abilityReady)
            {
                StartCoroutine(LaunchAbility());
            }
        }

        //Reloading
        if (Input.GetKeyDown (KeyCode.R))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && canShoot)
            {
                StartCoroutine(Reload());
            }
        }

    }

    void FixedUpdate()
    {
        //Player Movement & Rotation
		Vector3 rightMovement = right * p_Speed * Time.deltaTime * Input.GetAxis("Horizontal");
		Vector3 upMovement = forward * p_Speed * Time.deltaTime * Input.GetAxis("Vertical");
		Vector3 heading = rightMovement + upMovement;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (currentState == PlayerState.Walk || currentState == PlayerState.Idle || currentState == PlayerState.Attack )
        {
            if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask)){
                look = hit.point - transform.position;
                transform.rotation = Quaternion.LookRotation (look);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
            if (heading != Vector3.zero)
            {
                rb.MovePosition(transform.position + heading);
                anim.SetBool("Moving", true);
            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }
        
    }
    
    private IEnumerator AttackCO()
    {
        currentState = PlayerState.Attack;
        yield return new WaitForSeconds(.3f);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(.1f);
        attackHitbox.SetActive(false);
        currentState = PlayerState.Walk;
    }

    private IEnumerator Fire()
    {
        if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask)){
            canShoot = false;
            GameObject clone;
            clone = Instantiate(bullet, transform.position + new Vector3(0,5,0), transform.rotation);
            Vector3 dir = look;
            dir = dir.normalized;
            clone.GetComponent<Rigidbody>().AddForce(dir * bulletForce);
            yield return new WaitForSeconds(0.2f);
            canShoot = true;
            bulletFired += 1;
        }
    }

    private IEnumerator LaunchAbility()
    {
        if(currentAbility != StolenAbility.None)
        {
            abilityReady = false;
            currentState = PlayerState.Attack;
            yield return null;
            if(currentAbility == StolenAbility.Liana)
            {

            }
            if(currentAbility == StolenAbility.Spike)
            {
                GetComponent<LineRenderer>().enabled = true;
                if(Input.GetButton("Fire1")){
                    GameObject clone;
                    clone = Instantiate(sniperBullet, transform.position + new Vector3(0,5,0), transform.rotation);
                    clone.transform.rotation = Quaternion.LookRotation(look) * Quaternion.Euler(90,0,0);
                    Vector3 dir = look;
                    dir = dir.normalized;
                    clone.GetComponent<Rigidbody>().AddForce(dir * bulletForce * 2f);
                    GetComponent<LineRenderer>().enabled = false;
                    abilityIcon.sprite = spikeCDIcon;
                    yield return new WaitForSeconds(0.2f);
                    currentState = PlayerState.Idle;
                    StartCoroutine("AbilityCooldown");
                    StopCoroutine(LaunchAbility());
                } 
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<LineRenderer>().enabled = false;
                    abilityReady = true;
                    currentState = PlayerState.Idle;
                    StopCoroutine(LaunchAbility());
                }
                else
                {
                    StartCoroutine(LaunchAbility());
                }
            }
            if(currentAbility == StolenAbility.Rock)
            {
                
            }
            if(currentAbility == StolenAbility.Healer)
            {
                gameObject.GetComponent<PlayerDamage>().playerHealth += 30f;
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                abilityIcon.sprite = healerCDIcon;
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                StartCoroutine("AbilityCooldown");
                StopCoroutine(LaunchAbility());
            }
        }
    }

    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(5f);
        abilityReady = true;
        if(currentAbility == StolenAbility.Liana)
            {

            }
            if(currentAbility == StolenAbility.Spike)
            {
                abilityIcon.sprite = spikeIcon;
            }
            if(currentAbility == StolenAbility.Rock)
            {
                
            }
            if(currentAbility == StolenAbility.Healer)
            {
                abilityIcon.sprite = healerIcon;
            }
    }
    IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        bulletFired = 0;
        canShoot = true;
    }
}
