using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public static Player Instance;

    [Header("Class References")]
    public GameObject dragonFireSource; //Set in inspector, where the dragon fire originates, replace with dragon ideally when ready.
    public DragonFire dragonFire; //Set in inspector, prefab for dragonFire!
    public Transform dragonParent; //Parent for dragon when picked up
    public Animator boyAnim;
    public Transform lanternIndicator, dragonIndicator, indicatorParent;
    public BoysGhost playerGhost;

    [Header("Torch/Energy Settings")]
    public float dragonFireCost = 2f; //how much lantern fuel to fire dragon fire


    [Header("Ability Settings")]
    public float dragonPickupTimer = 3f; //time before dragon can be picked up (Inspector)
    public float smallHitRadius = 1f; //set short stun duration in Inspector, Default 1
    public float largeHitRadius = 3f; //set long stun duration in Inspector, Default 3
    public float dragonFireCoolDownTimer = 1f; //min time between dragon fire (set in Inspector)
    public float shortFleeDistance = 5f; //set short stun distance in Inspecetor, default 5
    public float largeFleeDistance = 10f; //set long stun distance in Inspecetor, Default 10
    [Tooltip("amount of torch lifre reduced on big swing")] public float swingTorchReduction = 2f; //amount of torch time to reduce on "big swing"

    torchlight _torchlight; //Reference to the torch. The torch handles the fuel (shared for dragon attack), the ambient sounds and visual fx.
    Lantern activeLantern; //Reference to the currently active lantern/beacon lighting the way
    UIManager uiManager;
    public Transform dragon;
    float laternFuel = 0f; //initialization only
    bool playerInRangeOfLantern = false;
    bool dragonFireCoolDown = false;
    public bool hasDragon = false;
    public bool isDead;
    bool canPickUpDragon = true;
    private bool justHit;
    public bool Recovering
    {
        get { return boyAnim.GetCurrentAnimatorStateInfo(0).IsName("Hit"); }
    }

    // the torch diminish based on movement 
    public float torchDiminishRate = 1;
    public float runTorchDiminishRate = 2;

    private PlayerMove pm;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Incorrect number of Lantern Guides");
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.StartGame();
        pm = GetComponent<PlayerMove>();
        _torchlight = FindObjectOfType<torchlight>();
        uiManager = FindObjectOfType<UIManager>();
        dragon = GameObject.Find("Dragon").transform;
        indicatorParent.SetParent(null);
        StartCoroutine("FlashLanternIndicator");
    }

    public void PlayerWalkingState() //animation controller call for walking
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FillLantern();
        }
    }

    public void PickUpDragon(Transform dragon) //pick up our friendly Dragon
    {
        Dragon.Instance.Pickup(dragonParent);
        if (canPickUpDragon)
        {
            hasDragon = true;
        }
        else
        {
            uiManager.ShowDragonWarning();
        }
    }

    public void DropDragon() //dragon has been knocked away from the player
    {
        canPickUpDragon = false;
        StartCoroutine(DragonFireCoolDownTimer());
    }

    private IEnumerator DragonFireCoolDownTimer()
    {
        yield return new WaitForSeconds(dragonPickupTimer);
        canPickUpDragon = true;
    }

    public void InRangeOfLantern(bool isInRange, float laternFillAmount, Lantern currentLantern) // lantern interactable
    {
        activeLantern = currentLantern;
        playerInRangeOfLantern = isInRange;
        laternFuel = laternFillAmount;
    }

    private void FillLantern() // fill the torch
    {
        bool fillTorch = activeLantern.LanternUsed();
        if (fillTorch)
        {
            _torchlight.FillTorch(laternFuel);
            StartCoroutine("FlashLanternIndicator");
        }
    }

    private IEnumerator FlashLanternIndicator()
    {
        lanternIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        lanternIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lanternIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        lanternIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lanternIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        lanternIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    public void DragonFire() //pull energy from the torch and fire
    {
        if (!Recovering)
        {
            //float torchLife = _torchlight.GetTorchLife();
            //if ((torchLife > dragonFireCost) && hasDragon)
            //{
            //    Instantiate(dragonFire, dragonFireSource.transform, false);
            //    _torchlight.ReduceTorchLife(dragonFireCost);
            //}
            //else
            //{
                SwingTorch();
            //}
        }
    }

    private void SwingTorch()
    {
        boyAnim.SetTrigger("Attack");
        float torchFuel = _torchlight.GetTorchLife();
        float torchMaxFuel = _torchlight.GetMaxTorchLife();
        float torchLifeRatio = torchFuel / torchMaxFuel;
        uiManager.FlashTorchIcon();


        if (torchLifeRatio <= .25) //do a weaker swing if short on fuel (will not consume fuel)
        {
            StunEnemies(smallHitRadius, shortFleeDistance);


        }
        else
        {
            StunEnemies(largeHitRadius, largeFleeDistance);
            _torchlight.ReduceTorchLife(swingTorchReduction);
        }
          
    }


    Enemy[] GetNearbyEnemies(float radius)
    {

        Vector3 pos = transform.position + transform.forward;
        List<Enemy> enemies = new List<Enemy>();

        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].transform.tag == "Enemy")
                {
                    Enemy e = colliders[i].transform.GetComponent<Enemy>();

                    enemies.Add(e);
                }
            }
        }

        return enemies.ToArray();
    }


    private void StunEnemies(float hitRadius, float fleeDistance)
    {
        Enemy[] nearbyEnemies = GetNearbyEnemies(hitRadius);
        Debug.Log("found " + nearbyEnemies.Length);
        if (nearbyEnemies.Length > 0)
        {
            for (int i = 0; i < nearbyEnemies.Length; i++)
            {
                nearbyEnemies[i].Scatter(transform , fleeDistance);
            }
        }
    }

    public IEnumerator TakeHit()
    {
        Debug.Log("TakeHit " + Recovering + " " + justHit);
        if (!Recovering && !justHit)
        {
            justHit = true;
            boyAnim.SetTrigger("Hit");
            if (hasDragon)
            {
                DropDragon();
                yield return new WaitForSeconds(0.7f);
                Dragon.Instance.Drop(transform.position + (-transform.forward * 3f), transform.rotation);
                hasDragon = false;
            }
            yield return new WaitForSeconds(0.7f);
            justHit = false;
        }
        yield return null;
    }

    
    public void DragonTakeDamage(float damage)
    {
        _torchlight.ReduceTorchLife(damage);
        uiManager.FlashDragonIcon();
    }

    private IEnumerator StartDragonFireCoolDown()
    {
        yield return new WaitForSeconds(dragonFireCoolDownTimer);
        dragonFireCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRangeOfLantern && Input.GetKeyDown(KeyCode.E))
        {
            FillLantern();
        }
        if ((Input.GetMouseButtonDown(0)&&!dragonFireCoolDown) && (!uiManager.GamePaused()))
        {
            dragonFireCoolDown = true;
            DragonFire();
            StartCoroutine(StartDragonFireCoolDown());
        }

        float torchDiminish = pm.IsRunning ? runTorchDiminishRate : torchDiminishRate;
        torchDiminish *= Time.deltaTime;
        _torchlight.ReduceTorchLife(torchDiminish);
        indicatorParent.position = transform.position + (Vector3.up * 4f);

        //Show and align indicators
        if (LanternGuide.Instance == null)
        {
            print("No Lanterns");
        }
        else
        {
            Lantern currentLantern = LanternGuide.Instance.CurrentLantern;
        

        bool showDragonIndicator = Vector3.Distance(transform.position, dragon.position) > 5f && !hasDragon; 
        lanternIndicator.LookAt(transform, currentLantern.transform.position - transform.position);
        dragonIndicator.gameObject.SetActive(showDragonIndicator);
        if (showDragonIndicator) 
            dragonIndicator.LookAt(transform, dragon.position - transform.position);
        }

        if (_torchlight.GetTorchLife() <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(PlayerDeath());
        }
    }

    public IEnumerator PlayerDeath()
    {
        if (playerGhost != null)
        {
            Instantiate(playerGhost, this.transform);
        }
        boyAnim.SetTrigger("Hit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("ExitLose");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "Dragon" && !hasDragon && canPickUpDragon)
        {
            // pickUPDragon
            print("hit the dragon");
            PickUpDragon(collision.collider.transform);
        }
    }
}
