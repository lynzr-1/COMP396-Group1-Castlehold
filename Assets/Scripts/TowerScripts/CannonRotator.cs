using UnityEngine;

public class CannonRotator : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;
    public Vector3 rotationOffset = new Vector3(0, 180, 0);

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("No target assigned to CannonRotator.");
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Ignore vertical axis

        if (direction.sqrMagnitude > 0.01f) // Only rotate if there's direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion adjustedRotation = targetRotation * Quaternion.Euler(rotationOffset); //to account for the tower's backwards rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log($"Cannon rotating towards {target.name}");
        }
    }
}
