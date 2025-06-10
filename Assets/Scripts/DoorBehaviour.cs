using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedRotation;
    private Vector3 openRotation;

    void Start()
    {
        closedRotation = transform.eulerAngles;
        openRotation = closedRotation + new Vector3(0, 90, 0);
    }
    public void Interact()
    {
        if (!isOpen)
        {
            transform.eulerAngles = openRotation;
            isOpen = true;
        }
        else
        {
            transform.eulerAngles = closedRotation;
            isOpen = false;
        }

    }
}
