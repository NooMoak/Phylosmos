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
        None, Liana, Worm, Sticky, Spike, Rock, Healer
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
    float force = 1000f;
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
    public Image abilityIcon;
    public Sprite spikeIcon;
    public Sprite spikeCDIcon;

	// Use this for initialization
	void Start ()
    {
		rb = GetComponent<Rigidbody>();
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize(forward);
		right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
        currentState = PlayerState.Walk;
        currentAbility = StolenAbility.None;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
		Vector3 rightMovement = right * p_Speed * Time.deltaTime * Input.GetAxis("Horizontal");
		Vector3 upMovement = forward * p_Speed * Time.deltaTime * Input.GetAxis("Vertical");
		Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit);

        if (Input.GetButtonDown ("Fire2"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(AttackCO());
            }
        }
        if (Input.GetButton ("Fire1"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && canShoot)
            {
                StartCoroutine(Fire());
            }
        }
        if (Input.GetKeyDown (KeyCode.E))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && abilityReady)
            {
                StartCoroutine(LaunchAbility());
            }
        }
        if (currentState == PlayerState.Walk || currentState == PlayerState.Idle )
        {
            if(hit.transform.tag == "Floor"){
                Vector3 look = hit.point - transform.position;
                transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,30,0);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
            UpdateAnimationAndMove(heading, rightMovement, upMovement);
        }
        
    }
    

    private IEnumerator AttackCO()
    {
        //p_anim.SetBool("Attacking", true);
        currentState = PlayerState.Attack;
       yield return new WaitForSeconds(.2f);
        attackHitbox.SetActive(true);
        yield return null;
        attackHitbox.SetActive(false);
        //p_anim.SetBool("Attacking", false);
        yield return new WaitForSeconds(.1f);
        currentState = PlayerState.Walk;
    }

    private IEnumerator Fire()
    {
        if(hit.transform.tag == "Floor"){
		canShoot = false;
		GameObject clone;
		clone = Instantiate(bullet, transform.position + new Vector3(0,5,0), transform.rotation);
		Vector3 dir = (hit.point + new Vector3(0,5,0)) - clone.transform.position;
		dir = dir.normalized;
		clone.GetComponent<Rigidbody>().AddForce(dir * force);
		yield return new WaitForSeconds(0.2f);
		canShoot = true;
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
            if(currentAbility == StolenAbility.Worm)
            {
                
            }
            if(currentAbility == StolenAbility.Sticky)
            {
            
            }
            if(currentAbility == StolenAbility.Spike)
            {
                GetComponent<LineRenderer>().enabled = true;
                Vector3 look = hit.point - transform.position;
                transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,30,0);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                if(Input.GetButton("Fire1")){
                    GameObject clone;
                    clone = Instantiate(sniperBullet, transform.position + new Vector3(0,5,0), transform.rotation);
                    clone.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,90,90);
                    Vector3 dir = hit.point + new Vector3(0,5,0) - clone.transform.position;
                    dir = dir.normalized;
                    clone.GetComponent<Rigidbody>().AddForce(dir * force * 2f);
                    GetComponent<LineRenderer>().enabled = false;
                    abilityIcon.sprite = spikeCDIcon;
                    yield return new WaitForSeconds(0.5f);
                    currentState = PlayerState.Idle;
                    yield return new WaitForSeconds(5f);
                    abilityReady = true;
                    abilityIcon.sprite = spikeIcon;
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
                Vector3 look = hit.point - transform.position;
                transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,30,0);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                gameObject.GetComponent<PlayerDamage>().playerHealth += 30f;
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                yield return new WaitForSeconds(5f);
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
        }
    }

     

    void UpdateAnimationAndMove(Vector3 heading, Vector3 rightMovement, Vector3 upMovement)
    {
        if (heading != Vector3.zero)
        {
            MovePlayer(heading, rightMovement, upMovement);
            //p_anim.SetFloat("moveX", change.x);
            //p_anim.SetFloat("moveY", change.y);
            //p_anim.SetBool("Moving", true);

        }
        else
        {
            //p_anim.SetBool("Moving", false);
        }

    }

    void MovePlayer(Vector3 heading, Vector3 rightMovement, Vector3 upMovement)
    {
		//transform.forward = heading; -> Rotate the player
		transform.position += rightMovement;
		transform.position += upMovement;
		//rb.velocity = new Vector3(rightMovement.x + upMovement.x, rightMovement.y + upMovement.y, rightMovement.z + upMovement.z) * p_Speed; -> Move the player thanks to velocity
    }

    /*public void Knock(float knockTime)
    {
    	StartCoroutine(KnockCO(knockTime));
    }

    private IEnumerator KnockCO(float knockTime)
    {
        if ( rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
            rb.velocity = Vector2.zero;

        }
    }*/

}
