using UnityEngine;

public class PassengerVisuals : MonoBehaviour
{
    public Mesh[] passengerMeshes;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    #region SetUp

    public void SetUp()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
        int randIndex = Random.Range(0, passengerMeshes.Length);
        skinnedMeshRenderer.sharedMesh = passengerMeshes[randIndex];
        
        skinnedMeshRenderer.updateWhenOffscreen = true;
    }

    #endregion
    
    #region Optimization

    public void EnableVisuals()
    {
    }

    public void DisableVisuals()
    {
    }

    #endregion
}
