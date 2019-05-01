using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
    {
        Idle,Walk,Attack,Ability,Interact, Stagger, Dead
    }
public enum StolenAbility
    {
        None, Liana, Spike, Rock, Healer
    }

public class PlayerController : MonoBehaviour
{
    //Player Variables
	[SerializeField] float p_Speed;
    public PlayerState currentState;
    public StolenAbility currentAbility;
    [SerializeField] Animator anim;
	Rigidbody rb;

    //Movement Variables
	Vector3 forward; 
	Vector3 right;
    Vector3 mousePos;
    RaycastHit hit;
	Ray ray;
    int rayPlaneMask;
    Vector3 look;

    //Attack Variables
    [SerializeField] GameObject attackHitbox;

    //Shoot Variables
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletForce = 1000f;
    bool canShoot = true;
    [SerializeField] Text magazineText;
    int bulletFired = 0;

    //Ability Variables
    [SerializeField] GameObject sniperBullet;
    [SerializeField] float rockPowerRadius = 5f;
    [SerializeField] float rockPowerForce = 10f;
    public int lianaCharge;
    public int spikeCharge;
    public int rockCharge;
    public int healerCharge;
    public bool abilityReady = true;
    public Image abilityIcon;
    public Sprite lianaIcon;
    public Sprite lianaCDIcon;
    public Sprite spikeIcon;
    public Sprite spikeCDIcon;
    public Sprite rockIcon;
    public Sprite rockCDIcon;
    public Sprite healerIcon;
    public Sprite healerCDIcon;
    [SerializeField] GameObject selectionUI;
    [SerializeField] Image spikeImage;
    [SerializeField] Image liniaImage;
    [SerializeField] Image healerImage;
    [SerializeField] Image rockImage;

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
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability && canShoot)
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
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability && abilityReady)
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

        //Selecting Ability
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentState = PlayerState.Stagger;
            selectionUI.SetActive(true);
            Time.timeScale = 0.2f;
            StartCoroutine("CursorChange");
        }
        if(Input.GetKey(KeyCode.Space))
        {
            if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                spikeImage.sprite = spikeIcon;
                //lianaImage.sprite = lianaCDIcon;
                healerImage.sprite = healerCDIcon;
                rockImage.sprite = rockCDIcon;
            } 
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                spikeImage.sprite = spikeCDIcon;
                //lianaImage.sprite = lianaIcon;
                healerImage.sprite = healerCDIcon;
                rockImage.sprite = rockCDIcon;
            }
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                spikeImage.sprite = spikeCDIcon;
                //lianaImage.sprite = lianaCDIcon;
                healerImage.sprite = healerIcon;
                rockImage.sprite = rockCDIcon;
            }  
            else if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                spikeImage.sprite = spikeCDIcon;
                //lianaImage.sprite = lianaCDIcon;
                healerImage.sprite = healerCDIcon;
                rockImage.sprite = rockIcon;
            } 
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            selectionUI.SetActive(false);
            Time.timeScale = 1f;
            currentState = PlayerState.Idle;
            if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                currentAbility = StolenAbility.Spike;
                if(spikeCharge > 0)
                    abilityIcon.sprite = spikeIcon;
                else 
                    abilityIcon.sprite = spikeCDIcon;
            } 
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                currentAbility = StolenAbility.Liana;
                if(lianaCharge > 0)
                    abilityIcon.sprite = lianaIcon;
                else 
                    abilityIcon.sprite = lianaCDIcon;
            }
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                currentAbility = StolenAbility.Healer;
                if(healerCharge > 0)
                    abilityIcon.sprite = healerIcon;
                else 
                    abilityIcon.sprite = healerCDIcon;
            }  
            else if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                currentAbility = StolenAbility.Rock;
                if(rockCharge > 0)
                    abilityIcon.sprite = rockIcon;
                else 
                    abilityIcon.sprite = rockCDIcon;
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
                look = hit.point - new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
                transform.rotation = Quaternion.LookRotation (look);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
            if (heading != Vector3.zero)
            {
                rb.MovePosition(transform.position + heading);
                anim.SetBool("Moving", true);
                if(currentState != PlayerState.Attack)
                    currentState = PlayerState.Walk;
            }
            else
            {
                anim.SetBool("Moving", false);
                if(currentState != PlayerState.Attack)
                    currentState = PlayerState.Idle; 
            }
        }
        
    }
    
    IEnumerator AttackCO()
    {
        currentState = PlayerState.Attack;
        yield return new WaitForSeconds(.3f);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(.1f);
        attackHitbox.SetActive(false);
        currentState = PlayerState.Idle;
    }

    IEnumerator Fire()
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

    IEnumerator LaunchAbility()
    {
        if(currentAbility != StolenAbility.None)
        {
            abilityReady = false;
            currentState = PlayerState.Ability;
            yield return null;
            if(currentAbility == StolenAbility.Liana && lianaCharge > 0)
            {
                lianaCharge -= 1;
            }
            else if(currentAbility == StolenAbility.Spike && spikeCharge > 0)
            {
                if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask))
                {
                    look = hit.point - new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
                    transform.rotation = Quaternion.LookRotation (look);
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                }
                GetComponent<LineRenderer>().enabled = true;
                if(Input.GetButtonDown("Fire1")){
                    GameObject clone;
                    clone = Instantiate(sniperBullet, transform.position + new Vector3(0,5,0), transform.rotation);
                    clone.transform.rotation = Quaternion.LookRotation(look) * Quaternion.Euler(90,0,0);
                    Vector3 dir = look;
                    dir = dir.normalized;
                    clone.GetComponent<Rigidbody>().AddForce(dir * bulletForce * 2f);
                    GetComponent<LineRenderer>().enabled = false;
                    abilityIcon.sprite = spikeCDIcon;
                    spikeCharge -= 1;
                    yield return new WaitForSeconds(0.2f);
                    currentState = PlayerState.Idle;
                    abilityReady = true;
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
            else if(currentAbility == StolenAbility.Rock && rockCharge > 0)
            {
                Vector3 explosionPos = transform.position + new Vector3(0,5,0);
                Collider[] colliders = Physics.OverlapSphere(explosionPos, rockPowerRadius);
                foreach(Collider hit in colliders)
                {
                    Rigidbody hitRb = hit.GetComponent<Rigidbody>();
                    
                    if(hitRb != null && hitRb != rb)
                        hitRb.AddExplosionForce(rockPowerForce, explosionPos, rockPowerRadius);
                }
                abilityIcon.sprite = rockCDIcon;
                rockCharge -= 1;
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
            else if(currentAbility == StolenAbility.Healer && healerCharge > 0)
            {
                gameObject.GetComponent<PlayerDamage>().playerHealth += 30f;
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                abilityIcon.sprite = healerCDIcon;
                healerCharge -= 1;
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
            else
            {
                abilityReady = true;
                currentState = PlayerState.Idle;
            }
        }
    }

    IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        bulletFired = 0;
        canShoot = true;
    }
    IEnumerator CursorChange()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }
}
