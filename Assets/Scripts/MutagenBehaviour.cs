using UnityEngine;

public class MutagenBehaviour : MonoBehaviour
{
    [SerializeField]
    int mutagenValue = 1;
    //Method to collect
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyMutagenAmt(mutagenValue);
        Destroy(gameObject);
    }
}
