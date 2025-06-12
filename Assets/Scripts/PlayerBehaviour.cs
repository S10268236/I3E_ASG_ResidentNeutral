using UnityEngine;
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
    int playerHealth = 100;

    bool canInteract = false;
    DoorBehaviour currentDoor = null;
    MutagenBehaviour currentMutagen = null;
    [SerializeField]
    TextMeshProUGUI playerHealthText;
    [SerializeField]
    TextMeshProUGUI mutagenAmtText;

    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    float interactionDistance = 5f;
    void Start()
    {
        //Add text to UI
        playerHealthText.text = "Health: " + playerHealth.ToString();
        mutagenAmtText.text = "Mutagens: " + mutagenAmt.ToString();
    }
    void Update()
    {
        //Raycasting for interactables
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.red);
        //Raycast = true when hitting something
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);
            //If collectible is within interaction range
            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                Debug.Log("Interactable Mutagen");
                //Set currentMutagen to the one in front of player and allow use of Interact
                currentMutagen = hitInfo.collider.gameObject.GetComponent<MutagenBehaviour>();
                canInteract = true;
            }
            //If door is within interaction range
            else if (hitInfo.collider.gameObject.CompareTag("Door"))
            {
                Debug.Log("Interactable Door");
                //Set currentDoor to the one in front of player and allow use of Interact
                currentDoor = hitInfo.collider.gameObject.GetComponent<DoorBehaviour>();
                canInteract = true;
            }
        }
        //Reset all variables to default state when Raycast = false
        else
        {
            Debug.Log("Reset");
            currentMutagen = null;
            currentDoor = null;
            canInteract = false;
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
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
    public void OnFire()
    {
        Debug.Log("Firing");
    }
    // void OnFire()
    // {
    //     Debug.Log("fire");
    //     //Instantiate a new projectile at spawn point
    //     //Store projectile to the 'newProjectile' variable
    //  GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
    //     Vector3 fireForce = spawnPoint.forward * fireStrength;
    //     newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    // }
}
