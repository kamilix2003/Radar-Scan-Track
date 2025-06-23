
using UnityEngine;

namespace target
{
    public abstract class TargetController
    {
        protected Rigidbody targetBody;
        protected GameObject targetObject;

        public Vector2 move;
        public float yaw;
        public float AOA;

        public float maxSpeed = 10f; // Maximum speed of the target
        public float minSpeed = 2f; // Minimum speed of the target

        public float maxElevation = 45f; // Maximum elevation angle
        
        public float accelerationFactor = 10f;
        public float dragForceFactor = 20f;
        public float pitchFactor = 50f;
        public float yawFactor = 50f;

        public TargetController(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Target is null.");
                return;
            }
            targetObject = target;
            targetBody = target.GetComponent<Rigidbody>();
            if (targetBody == null)
            {
                Debug.LogError("Target does not have a Rigidbody component.");
            }
        }
        public abstract void Update();
    }

    public class DirectTargetController : TargetController
    {
        public DirectTargetController(GameObject target) : base(target) { }
        public override void Update()
        {
            Accelerate();
            ChangeDirection();
            DragForce();
        }
        public void Accelerate()
        {
            if (targetBody == null)
            {
                Debug.LogError("Target Rigidbody is null.");
                return;
            }
            Vector3 force = targetObject.transform.forward * this.AOA;
            targetBody.AddRelativeForce(force);
        }
        public void ChangeDirection()
        {
            Vector3 torque = new Vector3(move.y, move.x, yaw);
            targetBody.AddRelativeTorque(torque);
        }
        public void DragForce()
        {
            if (targetBody == null)
            {
                Debug.LogError("Target Rigidbody is null.");
                return;
            }
            Vector3 dragForce = -0.1f * targetObject.transform.forward * targetBody.linearVelocity.magnitude * targetBody.linearVelocity.magnitude;
            targetBody.AddForce(dragForce);
        }
    }
    public class IndirectTargetController : TargetController
    {
        public IndirectTargetController(GameObject target) : base(target) { }
        public override void Update()
        {
            Accelerate();
            Move();
            Elevation();
            LimitSpeed();
        }
        private void Accelerate()
        {
            float temp = 0.001f;
            if ( targetBody.linearVelocity.magnitude < minSpeed )
            {
                targetBody.linearVelocity += targetBody.linearVelocity.normalized * minSpeed / 5;
            }
            if (move.magnitude > 0.01f)
            { 
                targetBody.linearVelocity += new Vector3(move.x, 0, move.y).normalized;
            }
            else
            {
                targetBody.linearVelocity = Vector3.Lerp(targetBody.linearVelocity, targetBody.linearVelocity.normalized * minSpeed, temp);
            }
        }
        private void Move()
        {
            float moveAngle = Mathf.Atan2(targetBody.linearVelocity.x, targetBody.linearVelocity.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(targetObject.transform.rotation.x, moveAngle, 0);
            targetObject.transform.rotation = targetRotation;
        }
        private void Elevation()
        {
            float elevationAngle = - Mathf.LerpAngle(0, AOA * maxElevation, 0.5f);
            Quaternion elevationRotation = Quaternion.Euler(elevationAngle, targetObject.transform.rotation.eulerAngles.y, 0);
            targetObject.transform.rotation = Quaternion.Slerp(targetObject.transform.rotation, elevationRotation, 1f);
            float altitudeDelta = Mathf.Lerp(0, targetBody.linearVelocity.magnitude * Mathf.Sin(AOA * maxElevation), 0.5f);
            targetBody.MovePosition(targetBody.position + (altitudeDelta * Time.fixedDeltaTime * Vector3.up));
        }
        private void LimitSpeed()
        {
            targetBody.linearVelocity = Vector3.ClampMagnitude(targetBody.linearVelocity, maxSpeed);
        }
    }
    public class AutoPilotController : TargetController
    {
        float height = 10f; // Desired height for autopilot
        float distance = 10f; // Radius for autopilot path
        float speed = 1f; // Speed for autopilot

        float currentDistance = 0f; // Current distance traveled along the path
        Vector3 moveDirection = Vector3.forward; // Direction of movement
        public AutoPilotController(GameObject target) : base(target) 
        { 
            targetBody.position = new Vector3(distance, height, 0); // Set initial position
        }
        public override void Update()
        {
            if (currentDistance >= distance)
            {
                currentDistance = 0f;
                moveDirection = Quaternion.Euler(0, 90, 0) * moveDirection; // Change direction
            }
            currentDistance += speed * Time.deltaTime; // Update distance traveled
            Vector3 targetPosition = targetBody.position + moveDirection * speed * Time.deltaTime;
        }
    }
}