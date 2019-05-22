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
    float shootAngle;
    Vector3 heading;

    //Attack Variables
    [SerializeField] GameObject attackHitbox;

    //Shoot Variables
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletForce = 1000f;
    [SerializeField] GameObject bulletStart;
    bool canShoot = true;
    [SerializeField] Text magazineText;
    [SerializeField] Text reloadText;
    int bulletFired = 0;

    //Ability Variables
    [SerializeField] GameObject sniperBullet;
    [SerializeField] GameObject grab;
    [SerializeField] float grabSpeed;
    [SerializeField] float rockPowerRadius = 5f;
    [SerializeField] float rockPowerForce = 10f;
    public bool rockAb = false;
    public bool flowerAnim = false;
    public int lianaCharge;
    public int spikeCharge;
    public int rockCharge;
    public int healerCharge;
    public bool abilityReady = true;
    public Image abilityIcon;
    [SerializeField] Image chargeJauge;
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
    bool inWheel = false;
    GameObject particleContainer;
    GameObject healParticle;
    GameObject shockwaveParticle;
    [SerializeField] AudioClip audioShock;
    [SerializeField] AudioClip audioGrab;

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

        particleContainer = transform.Find("Particles").gameObject;
        healParticle = particleContainer.transform.Find("HealParticle").gameObject;
        shockwaveParticle = particleContainer.transform.Find("ShockwaveParticle").gameObject;

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
        else
        {
            Debug.Log("DataSaver not found! Several errors will occur.");
        }
	}

    void Update()
    {
        //Bullet UI
        string magazineToDisplay;
        if(bulletFired == 0)
            magazineToDisplay = (10 - bulletFired).ToString();
        else 
            magazineToDisplay = "0" + (10 - bulletFired).ToString();
        magazineText.text = magazineToDisplay;

        //UI Charge
        if(currentAbility != StolenAbility.None)
        {
            if(currentAbility == StolenAbility.Liana)
                if(lianaCharge != 3)
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 0.333f * lianaCharge, 0.2f);
                else
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 1, 0.2f);
            if(currentAbility == StolenAbility.Spike)
                if(spikeCharge != 3)
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 0.333f * spikeCharge, 0.2f);
                else
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 1, 0.2f);
            if(currentAbility == StolenAbility.Healer)
                if(healerCharge != 3)
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 0.333f * healerCharge, 0.2f);
                else
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 1, 0.2f);
            if(currentAbility == StolenAbility.Rock)
                if(rockCharge != 3)
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 0.333f * rockCharge, 0.2f);
                else
                    chargeJauge.fillAmount = Mathf.Lerp(chargeJauge.fillAmount, 1, 0.2f);
        }
        else 
        {
            chargeJauge.fillAmount = 0;
        }

        //Sword Attack
        if (Input.GetButtonDown ("Fire2"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability) 
            {        
                anim.SetTrigger("Attack");
                currentState = PlayerState.Attack;
            }
        }

        //Rifle Shoot
        if (Input.GetButton ("Fire1"))
        {
            if(currentState != PlayerState.Attack && currentState != PlayerState.Stagger && currentState != PlayerState.Ability && canShoot)
            {
                if(bulletFired < 10)
                {
                    anim.SetBool("Shooting", true);
                } 
                else 
                {
                    anim.SetBool("Shooting", false);
                    StartCoroutine(Reload());
                }
            }
        }
        if (Input.GetButtonUp ("Fire1"))
        {
            anim.SetBool("Shooting", false);
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
            if(currentState != PlayerState.Stagger && currentState != PlayerState.Ability)
            {
                currentState = PlayerState.Stagger;
                inWheel = true;
                selectionUI.SetActive(true);
                Time.timeScale = 0.2f;
                StartCoroutine("CursorChange");
            }
        }
        if(Input.GetKey(KeyCode.Space))
        {
            if(inWheel)
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
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(inWheel)
            {
                selectionUI.SetActive(false);
                Time.timeScale = 1f;
                currentState = PlayerState.Idle;
                inWheel = false;
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
		Vector3 upMovement = forward * p_Speed * Time.deltaTime * Input.GetAxis("Vertical") * 1.4f;
		heading = rightMovement + upMovement;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (currentState == PlayerState.Walk || currentState == PlayerState.Idle || currentState == PlayerState.Attack )
        {
            if(Physics.Raycast(ray, out hit, 10000, rayPlaneMask)){
                look = hit.point - new Vector3 (transform.position.x, transform.position.y + 6.5f, transform.position.z);
                transform.rotation = Quaternion.LookRotation (look);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
            if (heading != Vector3.zero)
            {
                if(rightMovement == Vector3.zero || upMovement == Vector3.zero)
                {
                    rb.MovePosition(transform.position + heading * 1.4f);
                }
                else
                {
                    rb.MovePosition(transform.position + heading);
                }          
                if(currentState != PlayerState.Attack)
                {
                    currentState = PlayerState.Walk;
                }
             
                shootAngle = Vector3.Angle(heading, look);
                Vector3 cross = Vector3.Cross(heading, look);
                if(cross.y < 0)
                {
                    shootAngle = -shootAngle;
                }
                if((shootAngle <= 90 && shootAngle >= 0) || (shootAngle >= -90 && shootAngle <= 0))
                {
                    anim.SetBool("MovingForward", true);
                    anim.SetBool("MovingBackward", false);
                }
                else if((shootAngle > 90 && shootAngle >= 0) || (shootAngle < -90 && shootAngle <= 0))
                {
                    anim.SetBool("MovingBackward", true);
                    anim.SetBool("MovingForward", false);
                }
                anim.SetFloat("RunBlend", shootAngle);
                anim.SetBool("Moving", true);
            }
            else
            {
                StartCoroutine(WaitForIdle());
            }
        }
        
    }
    
    public void AttackCO()
    {
        attackHitbox.SetActive(true);
    }

    public void Fire()
    {
        if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask)){
            canShoot = false;
            GameObject clone;
            clone = Instantiate(bullet, bulletStart.transform.position, bulletStart.transform.rotation);
            Vector3 dir = look;
            dir = dir.normalized;
            clone.GetComponent<Rigidbody>().AddForce(dir * bulletForce);
            bulletFired += 1;
            canShoot = true;
        }
    }

    IEnumerator LaunchAbility()
    {
        if(currentAbility != StolenAbility.None)
        {
            abilityReady = false;
            currentState = PlayerState.Ability;
            anim.SetBool("Moving", false);
            yield return null;
            if(currentAbility == StolenAbility.Liana && lianaCharge > 0)
            {
                mainCam.GetComponent<CameraController>().SpellCam();
                if(Physics.Raycast(ray, out hit, 1000, rayPlaneMask))
                {
                    look = hit.point - new Vector3 (transform.position.x, transform.position.y + 6.5f, transform.position.z);
                }
                GameObject grabClone;
                grabClone = Instantiate(grab, bulletStart.transform.position, bulletStart.transform.rotation);
                grabClone.GetComponent<PlayerGrab>().player = gameObject;
                Vector3 dir = look;
                dir = dir.normalized;
                grabClone.GetComponent<Rigidbody>().AddForce(dir* grabSpeed * 50);
                GetComponent<AudioSource>().clip = audioGrab;
                GetComponent<AudioSource>().Play();
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
                    look = hit.point - new Vector3 (transform.position.x, transform.position.y + 6.5f, transform.position.z);
                    transform.rotation = Quaternion.LookRotation (look);
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                }
                anim.SetBool("Sniper", true);
                if(Input.GetButtonDown("Fire1")){
                    anim.SetTrigger("SniperShoot");
                    StopCoroutine(LaunchAbility());
                } 
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<LineRenderer>().enabled = false;
                    anim.SetBool("Sniper", false);
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
                shockwaveParticle.GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().clip = audioShock;
                GetComponent<AudioSource>().Play();
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, rockPowerRadius);
                rockAb = true;
                Debug.Log("RockAb");
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
                yield return new WaitForSeconds(1f);
                rockAb = false;
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
                abilityReady = true;
                StopCoroutine(LaunchAbility());
            }
            else if(currentAbility == StolenAbility.Healer && healerCharge > 0)
            {
                flowerAnim = true;
                mainCam.GetComponent<CameraController>().SpellCam();
                gameObject.GetComponent<PlayerDamage>().playerHealth += 30f;
                healParticle.GetComponent<ParticleSystem>().Play();
                flowerAnim = true;
                healerCharge -= 1;
                yield return new WaitForSeconds(0.2f);
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
                abilityReady = true;
                flowerAnim = false;
                StopCoroutine(LaunchAbility());
            }
            else
            {
                flowerAnim = false;
                abilityReady = true;
                currentState = PlayerState.Idle;
                mainCam.GetComponent<CameraController>().NormalCam();
            }
        }
    }

    IEnumerator Reload()
    {
        reloadText.text = "Reloading";
        canShoot = false;
        yield return new WaitForSeconds(0.3f);
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(1.2f);
        bulletFired = 0;
        canShoot = true;
        reloadText.text = "";
    }

    IEnumerator CursorChange()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator WaitForIdle()
    {
        yield return new WaitForSeconds(0.05f);
        if(heading == Vector3.zero)
        {
            anim.SetBool("MovingForward", false);
            anim.SetBool("MovingBackward", false);
            anim.SetBool("Moving", false);
            if(currentState != PlayerState.Attack)
            {
                currentState = PlayerState.Idle;
            } 
        }
    }

    public void AttackStop()
    {
        attackHitbox.SetActive(false);
        currentState = PlayerState.Idle;
    }

    public void Laser()
    {
        GetComponent<LineRenderer>().enabled = true;
    }

    public void SniperShoot()
    {
        GameObject clone;
        clone = Instantiate(sniperBullet, bulletStart.transform.position, bulletStart.transform.rotation);
        clone.transform.rotation = Quaternion.LookRotation(look) * Quaternion.Euler(90,0,0);
        Vector3 dir = look;
        dir = dir.normalized;
        clone.GetComponent<Rigidbody>().AddForce(dir * bulletForce * 2f);
        GetComponent<LineRenderer>().enabled = false;
        anim.SetBool("Sniper", false);
        spikeCharge -= 1;
        StartCoroutine(SniperStop());
    }

    IEnumerator SniperStop()
    {
        yield return new WaitForSeconds(0.5f);
        currentState = PlayerState.Idle;
        mainCam.GetComponent<CameraController>().NormalCam();
        abilityReady = true;
    }
}
