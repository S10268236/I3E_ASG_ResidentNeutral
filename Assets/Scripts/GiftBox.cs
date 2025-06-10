using UnityEngine;

public class GiftBox : MonoBehaviour
{
    [SerializeField]
    GameObject coin;
    [SerializeField]
    Transform giftSpawn;
    [SerializeField]
    int coinAmount = 3;

    bool isHit = false;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Giftbox collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Projectile"))
        {
            while (coinAmount > 0)
            {
                GameObject newCoin = Instantiate(coin, giftSpawn.position, coin.transform.rotation);
                coinAmount -= 1;
            }
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
