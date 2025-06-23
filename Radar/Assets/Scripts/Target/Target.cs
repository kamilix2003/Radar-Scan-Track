

using NUnit.Framework;
using RadarSystem;
using UnityEngine;

namespace Target
{
    class TargetData
    {
        public float distance;
        public float velocity;
        public Vector3 velocity3D;
        public float elevation;
        public float azimuth;
        public float timeStamp;
        public TargetData()
        {
            this.distance = 0f;
            this.velocity = 0f;
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
            this.velocity = targetBody.linearVelocity.magnitude;
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
            return $"TargetData: Distance={distance}, Velocity={velocity}, Elevation={elevation}, Azimuth={azimuth}, TimeStamp={timeStamp}";
        }
    }

    public class TargetTest
    {
        [Test]
        [TestCase(0f, 0f, 0f)]
        [TestCase(1f, 1f, 1f)]
        [TestCase(-1f, 1f, -1f)]
        [TestCase(-1f, 0f, 1f)]
        public void ConvertionTest(float x, float y, float z)
        {
            Vector3 position = new Vector3(x, y, z);
            TargetData testTargetData = new TargetData();
            testTargetData.SetPosition(position);
            Vector3 convertedPosition = testTargetData.GetPosition();
            Assert.AreEqual(position, convertedPosition, "The converted position does not match the original position.");
        }
        [Test]
        public void GetPositionTest()
        {
            TargetData testTargetData = new TargetData();
            testTargetData.distance = 1f;
            Vector3 position = testTargetData.GetPosition();
            Debug.Log(position);
            Assert.AreEqual(1f, position.magnitude, "The position magnitude does not match the distance.");
        }
    }
}