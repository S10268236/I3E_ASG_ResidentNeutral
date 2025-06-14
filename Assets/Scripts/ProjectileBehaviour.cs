using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    float ProjectileDamage = 20f;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collided with" + collision.gameObject.name);
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(ProjectileDamage);
        }
        
    }
}
