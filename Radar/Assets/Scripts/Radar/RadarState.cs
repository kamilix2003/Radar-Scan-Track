

using System;
using UnityEngine;

namespace RadarSystem
{
    abstract class RadarState
    {
        public virtual RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            return this;
        }

        public virtual Color GetColor()
        {
            return Color.white; // Default color for the radar state
        }
    }

    class TrackState : RadarState
    {
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target == null)
            {
                return new LostTrackState(); // Transition to lost track state if no target
            }
            return this;
        }

        public override Color GetColor()
        {
            return Color.red; // Color for the track state
        }
    }

    class LostTrackState : RadarState
    {
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target == null)
            {
                return new ScanState(); // Transition to lost track state if no target
            }
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
        }
        public ScanState(ScanStrategy strategy)
        {
            this.strategy = strategy;
        }
        public override RadarState UpdateBeam(RadarBeam beam, Targets.TargetData target)
        {
            if (target != null)
            {
                return new TrackState();
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
