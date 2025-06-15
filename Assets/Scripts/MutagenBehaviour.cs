using UnityEngine;

public class MutagenBehaviour : MonoBehaviour
{
    //Allow setting of Mutagen value
    [SerializeField]
    int mutagenValue = 1;
    //Audioclip input field
    [SerializeField]
    AudioClip CollectSound;

    //Method to collect mutagens and destroy it
    public void Collect(PlayerBehaviour player)
    {
        //Call ModifyMutagenScore in PlayerBehaviour
        player.ModifyMutagenScore(mutagenValue);
        AudioSource.PlayClipAtPoint(CollectSound, transform.position);
        Destroy(gameObject);
    }
}
