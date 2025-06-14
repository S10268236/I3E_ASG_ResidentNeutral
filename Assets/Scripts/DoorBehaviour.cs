using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    /// <summary>
    /// Opening and closing door, smooth operation
    /// </summary>

    //Set default state of door to be closed
    public bool isOpen = false;
    //Variable for vectors for when closed
    private Vector3 closedRotation;
    //Variable for vectors for when open
    private Vector3 openRotation;
    AudioSource doorOpenAudio;
    AudioSource doorCloseAudio;

    void Start()
    {
        //Closed vectors
        closedRotation = transform.eulerAngles;
        //Add 90 degrees rotation to open door
        openRotation = closedRotation + new Vector3(0, 90, 0);
        doorOpenAudio = GetComponent<AudioSource>();
    }
    public void Interact()
    {
        //Check whether Door is open or closed
        if (!isOpen)
        {
            //Play Open audio
            doorOpenAudio.Play();
            //Set transform vectors to the open vectors
            transform.eulerAngles = openRotation;
            isOpen = true;
        }
        else
        {
            //Play Close audio
            doorCloseAudio.Play();
            //Set transform vectors to the closed vectors
            transform.eulerAngles = closedRotation;
            isOpen = false;
        }
    }
}
