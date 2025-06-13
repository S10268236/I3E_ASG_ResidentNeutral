using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // int currentScore = 0;
    // int health = 50;
    // bool canInteract = false;
    // CoinBehaviour currentCoin = null;
    // DoorBehaviour currentDoor = null;


    // [SerializeField]
    // GameObject projectile;

    // [SerializeField]
    // Transform spawnPoint;
    // [SerializeField]
    // float fireStrength = 100f;
    // [SerializeField]
    // float interactionDistance = 5f;
    // void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log(other.gameObject.name);
    //     if (other.CompareTag("Door"))
    //     {
    //         canInteract = true;
    //         currentDoor = other.GetComponent<DoorBehaviour>();
    //     }
    //     else if (other.CompareTag("Collectible"))
    //     {
    //         canInteract = true;
    //         currentCoin = other.GetComponent<CoinBehaviour>();
    //     }
    // }
    // void OnTriggerExit(Collider other)
    // {
    //     // Check if the player has a detected coin or door
    //     if (currentCoin != null)
    //     {
    //         // If the object that exited the trigger is the same as the current coin
    //         if (other.gameObject == currentCoin.gameObject)
    //         {
    //             // Set the canInteract flag to false
    //             // Set the current coin to null
    //             // This prevents the player from interacting with the coin
    //             canInteract = false;
    //             currentCoin = null;
    //         }
    //         else if (other.gameObject == currentDoor.gameObject)
    //         {
    //             canInteract = false;
    //             currentDoor = null;
    //         }
    //     }
    // }
    // public void OnInteract()
    // {
    //     if (canInteract)
    //     {
    //         if (currentDoor != null)
    //         {
    //             Debug.Log("Interacting with door");
    //             currentDoor.Interact();
    //         }
    //         else if (currentCoin != null)
    //         {
    //             Debug.Log("Interacting with coin");
    //             currentCoin.Collect(this);

    //         }
    //     }
    // }
    // public void ModifyScore(int amt)
    // {
    //     currentScore += amt;
    //     Debug.Log(currentScore);
    // }
    // void OnFire()
    // {
    //     Debug.Log("fire");
    //     //Instantiate a new projectile at spawn point
    //     //Store projectile to the 'newProjectile' variable
    //     GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
    //     Vector3 fireForce = spawnPoint.forward * fireStrength;
    //     newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    // }

    // void Start()
    // {
    //     Debug.Log("Health: " + health);
    // }
    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Player collided with: " + collision.gameObject.name);

    //     if (collision.gameObject.CompareTag("Collectible"))
    //     {
    //         ++currentScore;
    //         Debug.Log("Score: " + currentScore);
    //     }

    // }
    // void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Healing") && health < 100)
    //     {
    //         ++health;
    //         Debug.Log("Health: " + health);
    //     }
    //     if (collision.gameObject.CompareTag("Hazard") && health > 0)
    //     {
    //         --health;
    //         Debug.Log("Health: " + health);
    //         if (health <= 0)
    //         {
    //             Debug.Log("Player is Dead");
    //         }
    //     }
    // }
    // void Update()
    // {
    //     RaycastHit hitInfo;
    //     Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
    //     if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
    //     {
    //         //Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name); //Remove after testing works
    //         if (hitInfo.collider.gameObject.CompareTag("Collectible"))
    //         {
    //             if (currentCoin != null)
    //             {
    //                 currentCoin.Unhighlight();
    //             }
    //             canInteract = true;
    //             currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehaviour>();
    //             currentCoin.Highlight();
    //         }
    //     }
    //     else if (currentCoin != null)
    //     {
    //         currentCoin.Unhighlight();
    //     }
    // }

    //New Code

    //Variables & Fields

    /// <Mutagen summary>
    /// Score points & amount required to pass level, collectible and heals
    /// </summary>
    int mutagenAmt = 0;
    public int maxPlayerHealth = 100;
    int currentPlayerHealth;
    bool canInteract = false;
    //Use to track time for damage
    private float damageTimer = 0f;
    //Track held breath time
    private float holdBreath = 5f;
    //Track whether gun is obtained
    bool gotGun = false;
    //Onscreen overlay for taking damage
    // public Image HealthImpact;
    //Set a variable for Door, Mutagen and Gun, set it to null for future storage of Raycast collider
    DoorBehaviour currentDoor = null;
    MutagenBehaviour currentMutagen = null;
    GunBehaviour currentGun = null;

    [SerializeField]
    TextMeshProUGUI playerHealthText;

    [SerializeField]
    TextMeshProUGUI mutagenAmtText;

    [SerializeField]
    Image reticle;

    [SerializeField]
    TextMeshProUGUI holdBreathText;

    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform respawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    [SerializeField]
    int acidDPS = 40;
    [SerializeField]
    int smokeDPS = 10;
    [SerializeField]
    int enemyDPS = 10;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    float fireStrength = 200f;
    // private bool finished = false;
    // [SerializeField]
    // float respawnTime = 0f;
    // float tenthSec = 0.1f;

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        //Add text to UI
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
        mutagenAmtText.text = "Mutagens: " + mutagenAmt.ToString();
    }
    void Update()
    {
        //Raycasting for interactables
        RaycastHit hitInfo;
        // Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
        //Raycast = true when hitting something
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            // Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            //If collectible is within interaction range
            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                //Set currentMutagen to the one in front of player and allow use of Interact
                currentMutagen = hitInfo.collider.gameObject.GetComponent<MutagenBehaviour>();
                canInteract = true;
            }
            //If door is within interaction range
            else if (hitInfo.collider.gameObject.CompareTag("Door"))
            {
                //Set currentDoor to the one in front of player and allow use of Interact
                currentDoor = hitInfo.collider.gameObject.GetComponent<DoorBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.gameObject.CompareTag("Gun"))
            {
                currentGun = hitInfo.collider.gameObject.GetComponent<GunBehaviour>();
                canInteract = true;
            }
        }
        //Reset all variables to default state when Raycast = false
        else
        {
            currentMutagen = null;
            currentDoor = null;
            currentGun = null;
            canInteract = false;
        }
    }
    /// <summary>
    /// Function to take damage and reset timer to control rate of damage taken
    /// </summary>
    /// <param name="dps"></param>
    private void DamageTaken(int dps)
    {
        currentPlayerHealth -= dps;
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
        damageTimer = 0f;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        //Melee Enemy attack
        if (collision.gameObject.CompareTag("Enemy") && currentPlayerHealth > 0 )
        {
            DamageTaken(enemyDPS);
        }
        // Death trigger
        if (currentPlayerHealth <= 0)
        {
            //Set health to 0 so hp isn't shorted
            currentPlayerHealth = 0;
            Respawn();
        }
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision with " + collision.collider.gameObject.name, collision.collider.gameObject);
        if (collision.collider.gameObject.CompareTag("Enemy") && currentPlayerHealth > 0)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 2f)
            {
                DamageTaken(enemyDPS);
            }
        }
        //Death trigger
        if (currentPlayerHealth <= 0)
        {
            //Set health to 0 so hp isn't shorted
            currentPlayerHealth = 0;
            Respawn();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //Reset damage timer
        damageTimer = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        // holdBreathText.text = holdBreath.ToString();
    }

    void OnTriggerStay(Collider other)
    {
        //Acid Damage Over Time
        if (other.gameObject.CompareTag("Acid") && currentPlayerHealth > 0)
        {
            //Track time
            damageTimer += Time.deltaTime;
            if (damageTimer >= 2f)
            {
                DamageTaken(acidDPS);
                // currentPlayerHealth -= acidDPS;
                // playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
                // damageTimer = 0f;
            }
        }
        // Smoke-buffer time of held breath, then Damage Over Time
        else if (other.gameObject.CompareTag("Smoke") && currentPlayerHealth > 0)
        {
            //Reduce holdBreath time to signify breath running out
            holdBreath -= Time.deltaTime;
            //Once out, start taking damage per second
            if (holdBreath <= 0)
            {
                //use damageTimer to time seconds per tick damage
                damageTimer += Time.deltaTime;
                if (damageTimer >= 2f)
                {
                    DamageTaken(smokeDPS);
                }
            }
        }
        //Death Trigger
        if (currentPlayerHealth <= 0)
        {
            currentPlayerHealth = 0;
            Respawn();
        }
    }
    private void Respawn()
    {
        currentPlayerHealth = maxPlayerHealth;
        Debug.Log("Respawned");
        Debug.Log("Before: " + transform.position);
        transform.position = respawnPoint.position;
        Debug.Log("After:" + transform.position);
        transform.rotation = respawnPoint.rotation;
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
    }
    void OnTriggerExit(Collider other)
    {
        //Resets damage timer if player leaves hazard zone
        damageTimer = 0f;
        //Reset Breath after leaving smoke zone
        holdBreath = 5f;
    }

    //What to do when interact is pressed
    public void OnInteract()
    {
        //Check if able to interact
        if (canInteract)
        {
            //Check if Door or Collectible
            if (currentDoor != null)
            {
                Debug.Log("Interacting with Door");
                currentDoor.Interact();
            }
            else if (currentMutagen != null)
            {
                Debug.Log("Mutagen GET");
                currentMutagen.Collect(this);
                ModifyHealth(50);
            }
            else if (currentGun != null)
            {
                Debug.Log("Gun GET!");
                currentGun.Collect(this);
                gotGun = true;
            }
        }
    }
    //Add to mutagen score when collected
    public void ModifyMutagenAmt(int amt)
    {
        mutagenAmt += amt;
        //Update new mutagen amount to UI
        mutagenAmtText.text = "Mutagens: " + mutagenAmt.ToString();
    }
    //Mutagens heal by amount specified in function, sets health to max health if it goes over
    void ModifyHealth(int heal)
    {
        if (currentPlayerHealth + heal > maxPlayerHealth)
        {
            currentPlayerHealth = maxPlayerHealth;
        }
        else
        {
            currentPlayerHealth += heal;
        }
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
    }
    /// <summary>
    /// Fire a projectile with forward motion
    /// </summary>
    public void OnFire()
    {
        if (gotGun)
        {
            //Instantiate a new projectile at spawn point
            //Store projectile to newProjectile variable
            GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            Vector3 fireForce = spawnPoint.forward * fireStrength;
            newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
        }
    }
}
