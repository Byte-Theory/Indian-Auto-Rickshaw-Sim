using UnityEngine;

public class InteractableBuilding : MonoBehaviour
{
    [SerializeField] private InteractableBuildingType interactableBuildingType;
    
    public InteractableBuildingType InteractableBuildingType => interactableBuildingType;
}
