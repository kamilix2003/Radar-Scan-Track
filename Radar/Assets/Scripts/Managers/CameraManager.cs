using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera secondaryCamera;

    [SerializeField] GameObject targetObject;
    void Start()
    {
        
    }

    void Update()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, targetObject.transform.position);
        mainCamera.orthographicSize = 3 + Mathf.Clamp(distance, 10f, 30f);

        if (targetObject.transform.position.magnitude > 75f)
        {
            targetObject.transform.position = Vector3.zero + 10f * Vector3.up;
        }
    }
}
