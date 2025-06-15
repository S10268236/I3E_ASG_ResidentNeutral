using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    //Audioclip input field
    [SerializeField]
    AudioClip GunPickUpClip;
    //Audio position input field
    [SerializeField]
    Transform SoundPosition;
    public void Collect(PlayerBehaviour player)
    {
        AudioSource.PlayClipAtPoint(GunPickUpClip, SoundPosition.position);
        Destroy(gameObject);
    }
}
