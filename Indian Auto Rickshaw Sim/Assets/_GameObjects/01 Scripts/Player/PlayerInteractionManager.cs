using System;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [Header("Layers")] 
    [SerializeField] private LayerMask interactiveBuildingLayer;
    [SerializeField] private LayerMask buildingLayer;
    
    // Ref
    private GetFuelMenu getFuelMenu;

    private void Start()
    {
        getFuelMenu = UiManager.Instance.GamePlayUi.GetFuelMenu;
    }

    private void OnCollisionEnter(Collision other)
    {
        int objectLayer = 1 << other.gameObject.layer;
        if ((objectLayer & buildingLayer) != 0)
        {
            AudioManager.Instance.PlayAudio(AudioClipType.AutoHit);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int objectLayer = 1 << other.gameObject.layer;
        
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
