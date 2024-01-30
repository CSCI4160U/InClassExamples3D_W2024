using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private float rotationAngle = 0f;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        rotationAngle += rotationSpeed * Time.deltaTime;
        transform.rotation = originalRotation * Quaternion.Euler(0f, rotationAngle, 0f);
    } 
}
