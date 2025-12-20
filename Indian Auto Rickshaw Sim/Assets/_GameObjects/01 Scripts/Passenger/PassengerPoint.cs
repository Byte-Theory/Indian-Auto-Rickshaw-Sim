using UnityEngine;

public class PassengerPoint : MonoBehaviour
{
    private GameObject activeEfx = null;

    #region Efx

    public void ShowActiveEfx()
    {
        if (activeEfx != null)
        {
            activeEfx.SetActive(false);
            activeEfx = null;
        }
        
        activeEfx = ObjectPooler.Instance.SpawnFromPool(0, transform.position + Vector3.up * 0.05f, Quaternion.identity);
        activeEfx.transform.localScale = Vector3.one * 1.5f;
    }

    public void RemoveActiveEfx()
    {
        if (activeEfx != null)
        {
            activeEfx.SetActive(false);
            activeEfx = null;
        }
    }

    #endregion
    
    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.darkBlue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    #endregion
}
