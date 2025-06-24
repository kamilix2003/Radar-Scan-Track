

using RadarSystem;
using UnityEngine;

namespace Target
{
    class TargetData
    {
        public float distance;
        public Vector3 velocity3D;
        public float elevation;
        public float azimuth;
        public float timeStamp;
        public TargetData()
        {
            this.distance = 0f;
            this.velocity3D = Vector3.zero;
            this.elevation = 0f;
            this.azimuth = 0f;
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
            this.distance = target.transform.position.magnitude;
            this.velocity3D = targetBody.linearVelocity;
            this.elevation = 0f;
            this.azimuth = 0f;
            this.timeStamp = Time.time;
        }

        public void PopulateAngles(RadarBeam beam)
        {
            if (beam == null)
            {
                Debug.LogError("RadarBeam is null.");
                return;
            }
            this.elevation = beam.Elevation;
            this.azimuth = beam.Azimuth;
        }
        public void PopulateAngles(Vector3 position)
        {
            this.elevation = Mathf.Asin(position.y / position.magnitude) * Mathf.Rad2Deg;
            this.azimuth = Mathf.Atan2(position.z, position.x) * Mathf.Rad2Deg;
        }
        public Vector3 GetPosition()
        {
            Vector3 position = Vector3.zero;

            float elevationRad = this.elevation* Mathf.Deg2Rad;
            float azimuthRad = this.azimuth * Mathf.Deg2Rad;

            position.x = distance * Mathf.Cos(elevationRad) * Mathf.Cos(azimuthRad);
            position.z = distance * Mathf.Cos(elevationRad) * Mathf.Sin(azimuthRad);
            position.y = distance * Mathf.Sin(elevationRad);
            
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
            this.elevation = Mathf.Asin(position.y / position.magnitude) * Mathf.Rad2Deg;
            this.azimuth = Mathf.Atan2(position.z, position.x) * Mathf.Rad2Deg;
        }
        public override string ToString()
        {
            return $"TargetData: Distance={distance}, Velocity={velocity3D.magnitude}, Elevation={elevation}, Azimuth={azimuth}, TimeStamp={timeStamp}";
        }
    }

}