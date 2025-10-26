using UnityEngine;

public class PassengerPoint : MonoBehaviour
{
    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.darkBlue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    #endregion
}
