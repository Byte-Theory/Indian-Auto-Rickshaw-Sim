using UnityEngine;
using UnityEngine.UI;

public class FuelUi : MonoBehaviour
{
    [SerializeField] private Slider fuelSlider;

    #region SetUp

    public void SetUp(float curValue, float maxValue)
    {
        fuelSlider.minValue = 0.0f;
        fuelSlider.maxValue = maxValue;
        
        fuelSlider.value = curValue;
    }

    #endregion

    #region Update Slider Ui

    public void UpdateSlider(float curVal)
    {
        fuelSlider.value = curVal >= 0 ? curVal : 0.0f;
    }

    #endregion
}
