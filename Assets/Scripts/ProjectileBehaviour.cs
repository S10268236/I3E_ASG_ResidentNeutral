using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collided with" + collision.gameObject.name);
        Destroy(gameObject);
    }
}
