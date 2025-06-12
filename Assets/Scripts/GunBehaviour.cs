using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public void Collect(PlayerBehaviour player)
    {
        Destroy(gameObject);
    }
}
