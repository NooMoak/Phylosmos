using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
    {
        Idle, Walk, Attack, Ability, Interact, Stagger, Dead
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
    GameObject mainCam;
    int enemyLayer;
    GameObject dataSaver;

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
    [SerializeField] GameObject grab;
    [SerializeField] float grabSpeed;
    [SerializeField] float rockPowerRadius = 5f;
    [SerializeField] float rockPowerForce = 10f;
    public bool rockAb = false;
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
    [SerializeField] Image lianaImage;
    [SerializeField] Image healerImage;
    [SerializeField] Image rockImage;

	void Start ()
    {
		rb = GetComponent<Rigidbody>();
        mainCam = Camera.main.gameObject;
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize(forward);
		right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
        currentState = PlayerState.Idle;
        currentAbility = StolenAbility.None;
        rayPlaneMask = LayerMask.GetMask("RayPlane");
        enemyLayer = LayerMask.GetMask("EnemyToHeal");

        //Data
        dataSaver = FindObjectOfType<DataSaver>().gameObject;
        if(dataSaver != null)
        {
        dataSaver.GetComponent<DataSaver>().player = this.gameObject;
        spikeCharge =  dataSaver.GetComponent<DataSaver>().spikeCharge;
        lianaCharge =  dataSaver.GetComponent<DataSaver>().lianaCharge;
        healerCharge =  dataSaver.GetComponent<DataSaver>().healerCharge;
        rockCharge =  dataSaver.GetComponent<DataSaver>().rockCharge;
        }
	}

    void Update()
    {
        string magazineToDisplay = (10 - bulletFired).ToString() + " / 10";
        magazineText.text = magazineToDisplay;

        //Sword Attack
        if (Input.GetButtonDown ("Fire2"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability) 
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
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability && canShoot)
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
                if(dataSaver.GetComponent<DataSaver>().knowSpike == true)
                    spikeImage.sprite = spikeIcon;
                if(dataSaver.GetComponent<DataSaver>().knowLiana == true)
                    lianaImage.sprite = lianaCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowHealer == true)
                    healerImage.sprite = healerCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowRock == true)
                    rockImage.sprite = rockCDIcon;
            } 
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowSpike == true)
                    spikeImage.sprite = spikeCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowLiana == true)
                    lianaImage.sprite = lianaIcon;
                if(dataSaver.GetComponent<DataSaver>().knowHealer == true)
                    healerImage.sprite = healerCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowRock == true)
                    rockImage.sprite = rockCDIcon;
            }
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowSpike == true)
                    spikeImage.sprite = spikeCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowLiana == true)
                    lianaImage.sprite = lianaCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowHealer == true)
                    healerImage.sprite = healerIcon;
                if(dataSaver.GetComponent<DataSaver>().knowRock == true)
                    rockImage.sprite = rockCDIcon;
            }  
            else if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowSpike == true)
                    spikeImage.sprite = spikeCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowLiana == true)
                    lianaImage.sprite = lianaCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowHealer == true)
                    healerImage.sprite = healerCDIcon;
                if(dataSaver.GetComponent<DataSaver>().knowRock == true)
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
                if(dataSaver.GetComponent<DataSaver>().knowSpike == true)
                {
                    currentAbility = StolenAbility.Spike;
                    if(spikeCharge > 0)
                        abilityIcon.sprite = spikeIcon;
                    else 
                        abilityIcon.sprite = spikeCDIcon;
                    }
            } 
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) > ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowLiana == true)
                {
                    currentAbility = StolenAbility.Liana;
                    if(lianaCharge > 0)
                        abilityIcon.sprite = lianaIcon;
                    else 
                        abilityIcon.sprite = lianaCDIcon;
                }
            }
            else if((Input.mousePosition.y * Screen.width) < (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowHealer == true)
                {
                    currentAbility = StolenAbility.Healer;
                    if(healerCharge > 0)
                        abilityIcon.sprite = healerIcon;
                    else 
                        abilityIcon.sprite = healerCDIcon;
                }
            }  
            else if((Input.mousePosition.y * Screen.width) > (Input.mousePosition.x * Screen.height) && (Input.mousePosition.y * Screen.width) < ((Screen.height * Screen.width) - Input.mousePosition.x * Screen.height))
            {
                if(dataSaver.GetComponent<DataSaver>().knowRock == true)
                {
                    currentAbility = StolenAbility.Rock;
                    if(rockCharge > 0)
                        abilityIcon.sprite = rockIcon;
                    else 
                        abilityIcon.sprite = rockCDIcon;
                }
            } 
        }

        //Camera Changing
        Collider[] nearEnemies = Physics.OverlapSphere(transform.position, 70f, enemyLayer);
        if(nearEnemies.Length > 0)
        {
            mainCam.GetComponent<CameraController>().FightCam();
        }
        else
        {
            mainCam.GetComponent<CameraController>().NormalCam();
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
            if(Physics.Raycast(ray, out hit, 10000, rayPlaneMask)){
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
        yield return new WaitForSeconds(.2f);
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
                mainCam.GetComponent<CameraController>().SpellCam();
                if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask))
                {
                    look = hit.point - new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
                }
                GameObject grabClone;
                grabClone = Instantiate(grab, transform.position + new Vector3(0,5,0), transform.rotation);
                grabClone.GetComponent<PlayerGrab>().player = gameObject;
                Vector3 dir = look;
                dir = dir.normalized;
                grabClone.GetComponent<Rigidbody>().AddForce(dir* grabSpeed * 50);
                yield return new WaitForSeconds(1f);
                abilityReady = true;
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
                lianaCharge -= 1;
                StopCoroutine(LaunchAbility());
            }
            else if(currentAbility == StolenAbility.Spike && spikeCharge > 0)
            {
                mainCam.GetComponent<CameraController>().SpellCam();
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
                    spikeCharge -= 1;
                    yield return new WaitForSeconds(0.2f);
                    currentState = PlayerState.Idle;
                    mainCam.GetComponent<CameraController>().NormalCam();
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
                mainCam.GetComponent<CameraController>().SpellCam();
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, rockPowerRadius);
                rockAb = true;
                foreach(Collider hit in colliders)
                {
                    Rigidbody hitRb = hit.GetComponent<Rigidbody>();
                    
                    if(hitRb != null && hitRb != rb)
                    {
                        float distance = Vector3.Distance(hitRb.transform.position, explosionPos);
                        hitRb.AddForce(-(transform.position - hitRb.transform.position) * rockPowerForce * ((1/distance) * 100));
                    }
                }
                rockCharge -= 1;
                yield return new WaitForSeconds(0.2f);
                rockAb = false;
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
            else if(currentAbility == StolenAbility.Healer && healerCharge > 0)
            {
                mainCam.GetComponent<CameraController>().SpellCam();
                gameObject.GetComponent<PlayerDamage>().playerHealth += 30f;
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                healerCharge -= 1;
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
            else
            {
                abilityReady = true;
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
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
