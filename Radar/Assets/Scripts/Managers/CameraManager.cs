using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera secondaryCamera;

    [SerializeField] TragetScript targetScript;
    void Update()
    {
        float distance = Vector3.Distance(mainCamera.transform.position, targetScript.gameObject.transform.position);
        mainCamera.orthographicSize = 3 + 0.7f * Mathf.Clamp(distance, 10f, 30f);

        if (targetScript.gameObject.transform.position.magnitude > 75f)
        {
            targetScript.gameObject.transform.position = Vector3.zero + 10f * Vector3.up;
        }
    }
}
