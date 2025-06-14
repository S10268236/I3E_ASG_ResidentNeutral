using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    Camera HPcamera;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = HPcamera.transform.rotation;
        transform.position = target.position + offset;
    }
}
