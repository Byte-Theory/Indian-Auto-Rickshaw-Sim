using UnityEngine;

public class GamePlayUi : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private FuelUi fuelUi;
    [SerializeField] private PassengersUi passengersUi;
    [SerializeField] private InGameMoneyUi inGameMoneyUi;

    #region Getters / Setters

    public FuelUi FuelUi => fuelUi;
    public PassengersUi PassengersUi => passengersUi;
    public InGameMoneyUi InGameMoneyUi => inGameMoneyUi;

    #endregion
}
