using TMPro;
using UnityEngine;

public class InGameMoneyUi : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyTxt;

    public void UpdateMoneyTxt(float val)
    {
        moneyTxt.text = val.ToString();
    }
}
