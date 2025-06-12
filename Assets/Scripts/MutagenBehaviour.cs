using UnityEngine;

public class MutagenBehaviour : MonoBehaviour
{
    [SerializeField]
    int mutagenValue = 1;
    //Method to collect mutagens and destroy it
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Mutagen Collected");
        player.ModifyMutagenAmt(mutagenValue);
        Destroy(gameObject);
    }
}
