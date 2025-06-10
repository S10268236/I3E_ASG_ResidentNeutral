using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OllisionEnter(Collision collision)
    {
        Debug.Log("Projectile collided with" + collision.gameObject.name);
        Destroy(gameObject);
    }
}
