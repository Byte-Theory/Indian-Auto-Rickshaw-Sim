using TMPro;
using UnityEngine;

public class GamePlayUi : MonoBehaviour
{
    [Header("Ui")] 
    [SerializeField] private TMP_Text dayTxt;
    
    [Header("Refs")]
    [SerializeField] private FuelUi fuelUi;
    [SerializeField] private PassengersUi passengersUi;
    [SerializeField] private InGameMoneyUi inGameMoneyUi;
    [SerializeField] private GetFuelMenu getFuelMenu;

    #region Getters / Setters

    public FuelUi FuelUi => fuelUi;
    public PassengersUi PassengersUi => passengersUi;
    public InGameMoneyUi InGameMoneyUi => inGameMoneyUi;
    public GetFuelMenu GetFuelMenu => getFuelMenu;

    #endregion

    public void UpdateDatText(int dayVal)
    {
        dayTxt.text = "Day " + dayVal;
    }
}
