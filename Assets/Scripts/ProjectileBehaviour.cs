using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //Allow setting of projectile damage
    [SerializeField]
    float ProjectileDamage = 20f;
    /// <OnCollisionEnter summary>
    /// If enemy,Make enemies take damage and remove the projectile object
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Projectile collided with" + collision.gameObject.name);
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(ProjectileDamage);
        }
        
    }
}
