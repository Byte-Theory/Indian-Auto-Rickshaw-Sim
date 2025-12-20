using System;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [Header("Layers")] 
    [SerializeField] private LayerMask interactiveBuildingLayer;
    
    // Ref
    private GetFuelMenu getFuelMenu;

    private void Start()
    {
        getFuelMenu = UiManager.Instance.GamePlayUi.GetFuelMenu;
    }

    private void OnTriggerEnter(Collider other)
    {
        int objectLayer = 1 << other.gameObject.layer;
        Debug.Log(objectLayer);
        Debug.Log(interactiveBuildingLayer);
        Debug.Log(objectLayer & interactiveBuildingLayer);
        
        if ((objectLayer & interactiveBuildingLayer) != 0)
        {
            InteractableBuilding interactableBuilding = other.GetComponent<InteractableBuilding>();
            InteractableBuildingType interactableBuildingType = interactableBuilding.InteractableBuildingType;

            if (interactableBuildingType == InteractableBuildingType.PetrolPump)
            {
                getFuelMenu.ShowGetFuelButton(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int objectLayer = 1 << other.gameObject.layer;
        
        if ((objectLayer & interactiveBuildingLayer) != 0)
        {
            InteractableBuilding interactableBuilding = other.GetComponent<InteractableBuilding>();
            InteractableBuildingType interactableBuildingType = interactableBuilding.InteractableBuildingType;

            if (interactableBuildingType == InteractableBuildingType.PetrolPump)
            {
                getFuelMenu.ShowGetFuelButton(false);
            }
        }
    }
}
