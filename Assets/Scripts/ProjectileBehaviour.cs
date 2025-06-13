using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    int ProjectileDamage = 20;
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
