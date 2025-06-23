using target;
using Target;
using UnityEngine;
using UnityEngine.InputSystem;

public class TragetScript : MonoBehaviour
{
    [Header("Target Movement Settings")]
    [SerializeField] float accelerationFactor = 10f;
    [SerializeField] float dragForceFactor = 20f;
    [SerializeField] float pitchFactor = 50f;
    [SerializeField] float yawFactor = 50f;
    [SerializeField] float rollFactor = 50f;

    [SerializeField] RadarScript radarScript;

    private TargetController controller;

    private void Start()
    {
        radarScript = GameObject.FindFirstObjectByType<RadarScript>();

        //controller = new DirectTargetController(this.gameObject);
        controller = new IndirectTargetController(this.gameObject);
        //controller = new AutoPilotController(this.gameObject);
    }
    private void Update()
    { 
        controller.accelerationFactor = accelerationFactor;
        controller.dragForceFactor = dragForceFactor ;
        controller.pitchFactor = pitchFactor ;
        controller.yawFactor = yawFactor ;
    }
    private void FixedUpdate()
    {
        controller.Update();
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
            controller.move = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            controller.move = Vector2.zero;
        }
    }
    public void OnYaw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.yaw = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            controller.yaw = 0;
        }
    }
    public void OnAOA(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.AOA = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            controller.AOA = 0;
        }
    }
    public void HideTarget(bool isHidden)
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = !isHidden;
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = !isHidden;
    }
}
