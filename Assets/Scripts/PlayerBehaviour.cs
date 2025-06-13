using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.ProBuilder.Shapes;

public class PlayerBehaviour : MonoBehaviour
{
    //Variables & Fields

    /// <Mutagen summary>
    /// Score points & amount required to pass level, collectible and heals
    /// </summary>
    int mutagenAmt = 0;
    private int totalMutagens = 10;
    int mutagensLeft;
    public int maxPlayerHealth = 100;
    int currentPlayerHealth;
    bool canInteract = false;
    //Use to track time for damage
    private float damageTimer = 0f;
    //Track held breath time
    private float maxBreath = 5f;
    private float currentBreath;
    //Track whether gun is obtained
    bool gotGun = false;
    //Onscreen overlay for taking damage
    public Image HealthImpact;
    //Set a variable for Door, Mutagen and Gun, set it to null for future storage of Raycast collider
    DoorBehaviour currentDoor = null;
    MutagenBehaviour currentMutagen = null;
    GunBehaviour currentGun = null;
    ExitBehaviour currentExit = null;

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
    bool hideInteractScreen = true;
    [SerializeField]
    TextMeshProUGUI InteractMessage;
    [SerializeField]
    GameObject DeathScreen;
    [SerializeField]
    TextMeshProUGUI DeathMessage;
    [SerializeField]
    GameObject WinScreen;
    [SerializeField]
    TextMeshProUGUI WinMessage;
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        currentBreath = maxBreath;
        //Add text to UI
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
        mutagenAmtText.text = "Mutagens: " + mutagenAmt.ToString();
    }
    void Update()
    {
        //Show damage screen when damaged
        DamageScreen();
        //Raycasting for interactables
        RaycastHit hitInfo;
        // Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
        //Raycast = true when hitting something
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            //If collectible is within interaction range
            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                hideInteractScreen = false;
                InteractScreen();
                //Set currentMutagen to the one in front of player and allow use of Interact
                currentMutagen = hitInfo.collider.gameObject.GetComponent<MutagenBehaviour>();
                canInteract = true;
            }
            //If door is within interaction range
            else if (hitInfo.collider.gameObject.CompareTag("Door"))
            {
                hideInteractScreen = false;
                InteractScreen();
                //Set currentDoor to the one in front of player and allow use of Interact
                currentDoor = hitInfo.collider.gameObject.GetComponent<DoorBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.gameObject.CompareTag("Gun"))
            {
                hideInteractScreen = false;
                InteractScreen();
                currentGun = hitInfo.collider.gameObject.GetComponent<GunBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.gameObject.CompareTag("Win"))
            {
                hideInteractScreen = false;
                InteractScreen();
                currentExit = hitInfo.collider.gameObject.GetComponent<ExitBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.gameObject.CompareTag("Untagged"))
            {
                ResetRaycast();
            }
        }
        //Reset all variables to default state when Raycast = false
        else
        {
            ResetRaycast();
        }
    }
    /// <ResetRaycast summary>
    /// Reset all the stored variables when raycast hits untagged and when raycast is false
    /// </summary>
    void ResetRaycast()
    {
        currentMutagen = null;
        currentDoor = null;
        currentGun = null;
        canInteract = false;
        hideInteractScreen = true;
        InteractScreen();
    }
    /// <InteractScreen summary>
    /// Default is hidden, text is transparent, Show Interact screen when able to interact
    /// </summary>
    void InteractScreen()
    {
        if (!hideInteractScreen)
        //while raycast hits interactable,show this screen
        {
            float transparency = 1f;
            Color imageColor = Color.white;
            //Set alpha of imageColor to be transparency variable
            imageColor.a = transparency;
            InteractMessage.color = imageColor;
        }
        else
        {
            float transparency = 0f;
            Color imageColor = Color.white;
            //Set alpha of imageColor to be transparency variable
            imageColor.a = transparency;
            InteractMessage.color = imageColor;
        }
    }
    void DamageScreen()
    {
        //As damage is taken, transparency increases
        float transparency = 1f - (currentPlayerHealth / 100f);
        Color imageColor = Color.white;
        //Set alpha of imageColor to be transparency variable
        imageColor.a = transparency;
        HealthImpact.color = imageColor;

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
        //Death Trigger
        if (currentPlayerHealth <= 0)
        {
            currentPlayerHealth = 0;
            StartCoroutine(Respawn());
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //Melee Enemy attack
        if (collision.gameObject.CompareTag("Enemy") && currentPlayerHealth > 0 )
        {
            DamageTaken(enemyDPS);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy") && currentPlayerHealth > 0)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 2f)
            {
                DamageTaken(enemyDPS);
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //Reset damage timer
        damageTimer = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        //Obtaining O2 resets breath
        if (other.gameObject.CompareTag("Oxygen"))
        {
            currentBreath = maxBreath;
            Destroy(other.gameObject);
        }
        // holdBreathText.text = maxBreath.ToString();
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
            //Reduce maxBreath time to signify breath running out
            currentBreath-= Time.deltaTime;
            //Once out, start taking damage per second
            if (currentBreath <= 0)
            {
                //use damageTimer to time seconds per tick damage
                damageTimer += Time.deltaTime;
                if (damageTimer >= 2f)
                {
                    DamageTaken(smokeDPS);
                }
            }
        }
    }
    IEnumerator Respawn()
    {
        DeathScreen.SetActive(true);
        DeathMessage.text = "You Are Dead";
        currentPlayerHealth = maxPlayerHealth;
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        yield return new WaitForSeconds(2);
        DeathScreen.SetActive(false);
        DeathMessage.text = null;
        playerHealthText.text = "Health: " + currentPlayerHealth.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        //Resets damage timer if player leaves hazard zone
        damageTimer = 0f;
        //Reset Breath after leaving smoke zone
        currentBreath = maxBreath;
        if (other.gameObject.CompareTag("Door"))
        {
            currentDoor = other.gameObject.GetComponent<DoorBehaviour>();
            if (currentDoor.isOpen == true)
            {
                currentDoor.Interact();
            }
            currentDoor = null;
        }
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
                currentDoor.Interact();
            }
            else if (currentMutagen != null)
            {
                currentMutagen.Collect(this);
                ModifyHealth(50);
            }
            else if (currentGun != null)
            {
                currentGun.Collect(this);
                gotGun = true;
            }
            else if (currentExit != null)
            {
                if (mutagenAmt >= 10)
                {
                    StartCoroutine(WinLoadNew());
                }
                else
                {
                    StartCoroutine(NeedMore());
                }
            }
        }
    }
    IEnumerator WinLoadNew()
    {
        WinScreen.SetActive(true);
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        WinMessage.text = "Congratulations!\n\n You get this hideous Win Screen!";
        yield return new WaitForSeconds(3);
        WinMessage.text = "Game is Restarting";
        yield return new WaitForSeconds(3);
        WinScreen.SetActive(false);
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
    IEnumerator NeedMore()
    {
        if (mutagensLeft > 1)
        {
            WinMessage.text = "You're not strong enough, find " + mutagensLeft + " more mutagens";
            yield return new WaitForSeconds(3);
            WinMessage.text = null;
        }
        else
        {
            WinMessage.text = "You're not strong enough, find the Last";
            yield return new WaitForSeconds(3);
            WinMessage.text = null;
        }

    }
    //Add to mutagen score when collected
    public void ModifyMutagenAmt(int amt)
    {
        mutagenAmt += amt;
        mutagensLeft = totalMutagens - mutagenAmt;
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
