using UnityEngine;
using TMPro;

public class PassengersUi : MonoBehaviour
{
    [SerializeField] private TMP_Text passengerDropCtTxt;

    public void UpdatePassengerDropCt(int val)
    {
        passengerDropCtTxt.text = val.ToString();
    }
}
