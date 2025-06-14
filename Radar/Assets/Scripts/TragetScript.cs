using Targets;
using UnityEngine;
using UnityEngine.InputSystem;

public class TragetScript : MonoBehaviour
{

    Rigidbody targetBody;

    private void Start()
    {
        targetBody = GetComponent<Rigidbody>();
        if (targetBody == null)
        {
            Debug.LogError("Rigidbody component is missing on the target object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Target enter");
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Target stay");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Target exit");
    }

    public void onMove(InputAction.CallbackContext context)
    {
        Debug.Log("onMove called");
        if (context.performed || true)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            //transform.Translate(moveInput.x, 0, moveInput.y); // Move the object based on input
            targetBody.linearVelocity = new Vector3(moveInput.x, 0, moveInput.y) * 5f; // Adjust speed as needed
            Debug.Log($"Move input: {moveInput}");
            // Handle movement logic here
        }
        
    }   

    public void OnAltitude(InputAction.CallbackContext context)
    {
        Debug.Log("OnAltitude called");
        if (context.performed)
        {
            float altitudeInput = context.ReadValue<float>();
            Debug.Log($"Altitude input: {altitudeInput}");
            // Handle altitude logic here
        }
    }

}
