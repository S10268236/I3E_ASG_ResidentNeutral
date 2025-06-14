using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    Camera camera;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        Debug.Log("Slider value " + slider);
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
