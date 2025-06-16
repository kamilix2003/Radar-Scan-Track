

using RadarSystem;
using UnityEngine;

namespace Targets
{
    class TargetData
    {
        public float distance;
        public float velocity;
        public float elevation;
        public float azimuth;
        public float timeStamp;

        public TargetData(float distance, float velocity, float elevation, float azimuth)
        {
            this.distance = distance;
            this.velocity = velocity;
            this.elevation = elevation;
            this.azimuth = azimuth;
            this.timeStamp = Time.time;
        }

        public TargetData(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("Target is null.");
                return;
            }
            Rigidbody targetBody = target.GetComponent<Rigidbody>();
            this.distance = Vector3.Distance(Vector3.zero, target.transform.position);
            this.velocity = targetBody.linearVelocity.magnitude;
            this.elevation = 0;
            this.azimuth = 0;
            this.timeStamp = Time.time;
        }

        public void PopulateAngles(RadarBeam beam)
        {
            if (beam == null)
            {
                Debug.LogError("RadarBeam is null.");
                return;
            }
            this.elevation = beam.beamDirection.y;
            this.azimuth = beam.beamDirection.x;
        }
        public Vector3 GetPosition()
        {
            Vector3 position = Vector3.zero;
            position.x = distance * Mathf.Cos(elevation) * Mathf.Cos(azimuth);
            position.y = distance * Mathf.Sin(elevation);
            position.z = distance * Mathf.Cos(elevation) * Mathf.Sin(azimuth);
            return position;
        }
    }
}