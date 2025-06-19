

using System;
using Targets;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace RadarSystem
{
    abstract class RadarState
    {
        public TargetData trackedTarget { get; protected set; }
        public string stateName { get; protected set; }
        protected RadarState(string stateName = "BaseRadarState", TargetData trackedData = null)
        {
            this.stateName = stateName;
            this.trackedTarget = trackedData;
        }
        public virtual RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target, float dt)
        {
            return this;
        }
        public virtual Color GetColor()
        {
            return Color.white; // Default color for the radar state
        }
        public override string ToString()
        {
            return stateName;
        }
        protected void predictTargetPosition(TargetData target, float dt)
        {
            Vector3 positionDelta = new Vector3(target.velocity3D.y, target.velocity3D.x, target.velocity3D.z) * dt;
            Vector3 noise = Mathf.PerlinNoise(Time.time, 0.0f) * 0.1f * Vector3.one; // Adding some noise for realism
            Vector3 newPosition = target.GetPosition() + positionDelta + noise;
            target.SetPosition(newPosition);
        }
    }
    class TrackState : RadarState
    {
        public TrackState(TargetData target) : base("TrackState", target)
        {

        }
        public override RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt)
        {
            if (target == null)
            {
                return new LostTrackState(trackedTarget); // Transition to lost track state if no target
            }
            trackedTarget = target;
            predictTargetPosition(trackedTarget, dt);
            beam.beamDirection = trackedTarget.direction;
            return this;
        }

        public override Color GetColor()
        {
            return Color.red; // Color for the track state
        }
    }

    class LostTrackState : RadarState
    {
        const float lostTrackTimeout = 2f; // Timeout for lost track state
        public LostTrackState(TargetData targetData) : base("LostTrackState", targetData)
        {
            
        }
        public override RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt)
        {
            if (target != null)
            {
                return new TrackState(target); // Transition to track state if target is found
            }
            if (Time.time - trackedTarget.timeStamp > lostTrackTimeout)
            {
                Debug.Log("Lost track of target, transitioning to scan state.");
                return new ScanState(); // Transition to scan state if timeout exceeded
            }
            predictTargetPosition(trackedTarget, dt);
            beam.beamDirection = trackedTarget.direction;
            return this;
        }

        public override Color GetColor()
        {
            return Color.blue; // Color for the lost track state
        }
    }

    class ScanState : RadarState
    {
        private ScanStrategy strategy;

        public ScanState() : base("ScanState")
        {
            strategy = new HorizontalScan();
        }
        public ScanState(ScanStrategy strategy)
        {
            this.strategy = strategy;
            stateName = "ScanState"; // Set the state name for scanning
        }
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target, float dt)
        {
            if (target != null)
            {
                RadarState newState = new TrackState(target);
                newState.UpdateBeam(beam, target, dt); // Update the beam with the target data
                return newState;
            }
            strategy.NextBeamDirection(beam);
            return this; // Continue scanning
        }
        public override Color GetColor()
        {
            return Color.green; // Color for the scan state
        }
    }
}
