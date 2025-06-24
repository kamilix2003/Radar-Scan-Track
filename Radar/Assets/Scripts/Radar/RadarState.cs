using Target;
using UnityEngine;

namespace RadarSystem
{
    abstract class RadarState
    {
        public TargetData TrackedTarget { get; protected set; }
        public string StateName { get; protected set; }
        protected Color stateColor;
        protected RadarState(Color stateColor, string stateName = "Unknown Radar State", TargetData trackedData = null)
        {
            this.stateColor = stateColor;
            this.StateName = stateName;
            this.TrackedTarget = trackedData;
        }
        public abstract RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt);
        public Color GetColor()
        {
            return stateColor;
        }
        public override string ToString()
        {
            return StateName;
        }
        protected void predictTargetPosition(TargetData target, float dt)
        {
            Vector3 positionDelta = target.velocity3D * dt;
            positionDelta.x = - positionDelta.x;
            Vector3 newPosition = target.GetPosition() + positionDelta;
            target.SetPosition(newPosition);
        }
    }
    class TrackState : RadarState
    {
        public TrackState(TargetData target) : base(Color.red, "Tracking", target) { }
        public override RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt)
        {
            if (target == null)
            {
                return new LostTrackState(TrackedTarget);
            }
            TrackedTarget = target;
            predictTargetPosition(TrackedTarget, dt);
            beam.Elevation = TrackedTarget.elevation;
            beam.Azimuth = TrackedTarget.azimuth;
            return this;
        }
    }

    class LostTrackState : RadarState
    {
        const float lostTrackTimeout = 2f;
        public LostTrackState(TargetData targetData) : base(Color.blue, "Lost Track", targetData) { }
        public override RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt)
        {
            if (target != null)
            {
                return new TrackState(target);
            }
            if (Time.time - TrackedTarget.timeStamp > lostTrackTimeout)
            {
                Debug.Log("Lost track of target, transitioning to scan state.");
                return new ScanState();
            }
            predictTargetPosition(TrackedTarget, dt);
            beam.Elevation = TrackedTarget.elevation;
            beam.Azimuth = TrackedTarget.azimuth;
            return this;
        }
    }
    class ScanState : RadarState
    {
        private ScanStrategy strategy;
        public ScanState() : base(Color.green, "Scanning")
        {
            strategy = new HorizontalScan();
        }
        public ScanState(ScanStrategy strategy) : base(Color.green, "Scanning")
        {
            this.strategy = strategy;
        }
        public override RadarState UpdateBeam(RadarBeam beam, TargetData target, float dt)
        {
            if (target != null)
            {
                return new TrackState(target);
            }
            strategy.NextBeamDirection(beam);
            return this;
        }
    }
}
