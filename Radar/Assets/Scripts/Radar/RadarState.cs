

using System;
using Targets;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace RadarSystem
{
    abstract class RadarState
    {
        public string StateName { get; protected set; }
        public virtual RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            return this;
        }

        public virtual Color GetColor()
        {
            return Color.white; // Default color for the radar state
        }
        public override string ToString()
        {
            return StateName;
        }
        protected void predictTargetPosition(TargetData target, float dt = 0.1f)
        {
            Vector3 positionDelta = new Vector3(target.velocity3D.y, target.velocity3D.x, target.velocity3D.z) * dt;
            Vector3 newPosition = target.GetPosition() + positionDelta;
            target.SetPosition(newPosition);
            //Debug.Log(target.ToString());
            //Debug.Log($"Position delta: {positionDelta}");
        }
    }
    class TrackState : RadarState
    {
        TargetData trackedTarget;
        public TrackState(TargetData target)
        {
            trackedTarget = target;
            StateName = "TrackState"; // Set the state name for tracking
        }
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target == null)
            {
                return new LostTrackState(trackedTarget); // Transition to lost track state if no target
            }
            trackedTarget = target;
            predictTargetPosition(trackedTarget);
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
        TargetData lostTarget;
        const float lostTrackTimeout = 2f; // Timeout for lost track state
        public LostTrackState(TargetData targetData)
        {
            StateName = "LostTrackState"; // Set the state name for lost track
            lostTarget = targetData;
        }
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target != null)
            {
                return new TrackState(target); // Transition to track state if target is found
            }
            if (Time.time - lostTarget.timeStamp > lostTrackTimeout)
            {
                Debug.Log("Lost track of target, transitioning to scan state.");
                return new ScanState(); // Transition to scan state if timeout exceeded
            }
            predictTargetPosition(lostTarget);
            beam.beamDirection = lostTarget.direction;
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

        public ScanState()
        {
            strategy = new HorizontalScan();
            StateName = "ScanState"; // Set the state name for scanning
        }
        public ScanState(ScanStrategy strategy)
        {
            this.strategy = strategy;
            StateName = "ScanState"; // Set the state name for scanning
        }
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target != null)
            {
                RadarState newState = new TrackState(target);
                newState.UpdateBeam(beam, target); // Update the beam with the target data
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
