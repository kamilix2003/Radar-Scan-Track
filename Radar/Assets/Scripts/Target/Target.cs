

using RadarSystem;
using UnityEngine;

namespace Targets
{
    class TargetData
    {
        public float distance;
        public float velocity;
        public Vector3 velocity3D;
        public Vector2 direction;
        public float timeStamp;

        public TargetData(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Target is null.");
                return;
            }
            Rigidbody targetBody = target.GetComponent<Rigidbody>();
            this.distance = target.transform.position.magnitude;
            this.velocity = targetBody.linearVelocity.magnitude;
            this.velocity3D = targetBody.linearVelocity;
            this.direction = Vector2.zero;
            this.timeStamp = Time.time;
        }

        public void PopulateAngles(RadarBeam beam)
        {
            if (beam == null)
            {
                Debug.LogError("RadarBeam is null.");
                return;
            }
            this.direction = beam.beamDirection;
        }
        public Vector3 GetPosition()
        {
            Vector3 position = Vector3.zero;

            float elevationRad = this.direction.x * Mathf.Deg2Rad;
            float azimuthRad = this.direction.y * Mathf.Deg2Rad;

            position.x = distance * Mathf.Cos(elevationRad) * Mathf.Cos(azimuthRad);
            position.y = distance * Mathf.Sin(elevationRad);
            position.z = distance * Mathf.Cos(elevationRad) * Mathf.Sin(azimuthRad);
            return position;
        }

        public void SetPosition(Vector3 position)
        {
            if (position == null)
            {
                Debug.LogError("Position is null.");
                return;
            }
            this.distance = position.magnitude;
            this.direction = new Vector2(Mathf.Asin(position.y / position.magnitude), Mathf.Atan2(position.z, position.x)) * Mathf.Rad2Deg;
        }

        public override string ToString()
        {
            return $"TargetData: Distance={distance}, Velocity={velocity}, Direction={direction}, TimeStamp={timeStamp}";
        }

    }
}