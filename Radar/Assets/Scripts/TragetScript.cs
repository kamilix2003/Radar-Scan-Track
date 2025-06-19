using Targets;
using UnityEngine;
using UnityEngine.InputSystem;

public class TragetScript : MonoBehaviour
{
    [Header("Target Movement Settings")]
    [SerializeField] float moveFactor = 1000f;
    [SerializeField] float upDownFactor = 250f;
    [SerializeField] float dampingFactor = 0.02f;
    [SerializeField] float dampingFactorExponent = 0.01f;

    [SerializeField] RadarScript radarScript;

    Rigidbody targetBody;

    private Vector2 moveInput;
    private float upDownInput;

    private void Start()
    {
        radarScript = GameObject.FindFirstObjectByType<RadarScript>();
        targetBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (radarScript == null)
        {
            Debug.LogError("RadarScript is not assigned.");
            return;
        }
    }
    private void FixedUpdate()
    {
        if (radarScript == null)
        {
            Debug.LogError("RadarScript is not assigned.");
            return;
        }
        Vector3 moveDirection = new Vector3(moveInput.x * moveFactor, upDownInput * upDownFactor, moveInput.y * moveFactor);
        targetBody.AddForce(moveDirection * Time.fixedDeltaTime);
        targetBody.linearVelocity -= dampingFactor * Mathf.Exp(dampingFactorExponent * targetBody.linearVelocity.magnitude) * targetBody.linearVelocity;
    }
    private void OnTriggerStay(Collider other)
    {
        TargetData targetData = new TargetData(gameObject);
        radarScript.targetData = targetData;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>().normalized;
        }
        if (context.canceled)
        {
            moveInput = Vector2.zero;
        }
    }   

    public void OnUpDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            upDownInput = context.ReadValue<float>();
        }
        if (context.canceled)
        {
            upDownInput = 0f;
        }
    }

}
