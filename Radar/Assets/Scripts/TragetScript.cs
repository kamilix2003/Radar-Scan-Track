using Targets;
using UnityEngine;
using UnityEngine.InputSystem;

public class TragetScript : MonoBehaviour
{
    [SerializeField] float moveFactor = 10f;
    [SerializeField] float altitudeFactor = 5f;

    [SerializeField] RadarScript radarScript;

    private void Start()
    {
        radarScript = GameObject.FindFirstObjectByType<RadarScript>();
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Target stay");
        TargetData targetData = new TargetData(gameObject);
        radarScript.targetData = targetData;
    }
    public void onMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        transform.position += new Vector3(moveInput.x, 0, moveInput.y).normalized * moveFactor * Time.deltaTime;
        if (context.performed)
        {
            Debug.Log($"Move input: {moveInput}");
        }
        
    }   

    public void OnAltitude(InputAction.CallbackContext context)
    {
        float altitudeInput = context.ReadValue<float>();
        transform.position += new Vector3(0, altitudeInput) * altitudeFactor * Time.deltaTime;
        if (context.performed)
        {
            Debug.Log($"Altitude input: {altitudeInput}");
        }
    }

}
