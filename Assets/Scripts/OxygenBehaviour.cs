using UnityEngine;

public class OxygenBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip OxygenCollectSound;
    [SerializeField]
    Transform SoundLocation;
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Collected Oxygen");
        AudioSource.PlayClipAtPoint(OxygenCollectSound, SoundLocation.position);
        Destroy(gameObject);
    }
}
