using UnityEngine;

public class MutagenBehaviour : MonoBehaviour
{
    [SerializeField]
    int mutagenValue = 1;
    [SerializeField]
    AudioClip CollectSound;

    //Method to collect mutagens and destroy it
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyMutagenScore(mutagenValue);
        AudioSource.PlayClipAtPoint(CollectSound, transform.position);
        Destroy(gameObject);
    }
}
