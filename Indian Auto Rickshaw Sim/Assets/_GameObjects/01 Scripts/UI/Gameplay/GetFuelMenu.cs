using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetFuelMenu : MonoBehaviour
{
    [Header("Menu")] 
    [SerializeField] private GameObject getFuelMenu;
    
    [Header("Rate Ui")] 
    [SerializeField] private TMP_Text fuelRateTxt;
    
    [Header("Quantity Ui")]
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject errorTxt;
    
    [Header("Buttons")]
    [SerializeField] private Button openFuelMenuButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button closeButton;
    
    // Ref
    private InGameMoneyManager inGameMoneyManager;
    private FuelTank fuelTank;

    private void Awake()
    {
        openFuelMenuButton.onClick.AddListener(OnClickOpenFuelMenuButton);
        buyButton.onClick.AddListener(OnClickBuyButton);
        closeButton.onClick.AddListener(OnClickCloseButton);

        quantitySlider.onValueChanged.AddListener(SliderOnValueChanged);
        
        getFuelMenu.SetActive(false);
        openFuelMenuButton.gameObject.SetActive(false);
    }

    #region Show / Hide

    public void ShowGetFuelButton(bool show)
    {
        openFuelMenuButton.gameObject.SetActive(show);
    }
    
    private void ShowGetFuelMenu()
    {
        if (inGameMoneyManager == null)
        {
            inGameMoneyManager = InGameMoneyManager.Instance;
        }
        
        if (fuelTank == null)
        {
            fuelTank = Player.Instance.fuelTank;
        }

        fuelRateTxt.text = (int) (Constants.AutoData.FuelRate) + "Rs per Liter";

        float maxQuantity = fuelTank.MaxFuelRequireToFullTank;
        
        quantitySlider.minValue = 0;
        quantitySlider.maxValue = maxQuantity;
        quantitySlider.value = 0;

        UpdateSliderTxt();
        
        getFuelMenu.SetActive(true);
    }

    #endregion

    #region Slider

    private void SliderOnValueChanged(float value)
    {
        float reqMoney = value * Constants.AutoData.FuelRate;
        float carryingMoney = inGameMoneyManager.CarryingMoney;
        errorTxt.SetActive(reqMoney > carryingMoney);
        
        UpdateSliderTxt();
    }
    
    private void UpdateSliderTxt()
    {
        quantityText.text = (int)(quantitySlider.value) + " L";
    }

    #endregion
    
    #region Buttons

    private void OnClickOpenFuelMenuButton()
    {
        ShowGetFuelMenu();
    }
    
    private void OnClickBuyButton()
    {
        int quantityToAdd = (int)(quantitySlider.value);
        float reqMoney = quantityToAdd * Constants.AutoData.FuelRate;

        bool isSpendingSuccessful = inGameMoneyManager.SpendMoney(reqMoney);
        if (isSpendingSuccessful)
        {
            fuelTank.AddFuel(quantityToAdd);

            MissionManager.Instance.OnUpdateMissionProgress(MissionType.FillFuel, quantityToAdd);
            
            OnClickCloseButton();
        }
    }

    private void OnClickCloseButton()
    {
        getFuelMenu.SetActive(false);
    }

    #endregion
}
