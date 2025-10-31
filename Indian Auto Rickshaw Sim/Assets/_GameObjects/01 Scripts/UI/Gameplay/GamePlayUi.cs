using UnityEngine;

public class GamePlayUi : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private FuelUi fuelUi;

    #region Getters / Setters

    public FuelUi FuelUi => fuelUi;

    #endregion
}
