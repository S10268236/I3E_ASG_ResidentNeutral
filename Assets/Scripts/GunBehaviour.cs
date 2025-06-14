using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip GunPickUpClip;
    [SerializeField]
    Transform SoundPosition;
    public void Collect(PlayerBehaviour player)
    {
        AudioSource.PlayClipAtPoint(GunPickUpClip, SoundPosition.position);
        Destroy(gameObject);
    }
}
