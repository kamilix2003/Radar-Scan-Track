

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

        public TargetData(Target target)
        {
            this.distance = Vector3.Distance(Vector3.zero, target.position);
            this.velocity = target.velocity.magnitude;
            this.elevation = Mathf.Atan2(target.position.y, target.position.x) * Mathf.Rad2Deg;
            this.azimuth = Mathf.Atan2(target.position.z, target.position.x) * Mathf.Rad2Deg;
            this.timeStamp = Time.time;
        }
    }

    class Target
    {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
    }
}