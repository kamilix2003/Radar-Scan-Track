

using System;
using UnityEngine;

namespace RadarSystem
{
    abstract class RadarState
    {
        public abstract void UpdateBeam(RadarBeam beam, Target.TargetData target);
        public virtual RadarState NextState()
        {
            return new ScanState();
        }
    }

    class TrackState : RadarState
    {
        public override void UpdateBeam(RadarBeam beam, Target.TargetData target)
        {
            throw new NotImplementedException();
        }
    }

    class LostTrackState : RadarState
    {
        public override void UpdateBeam(RadarBeam beam, Target.TargetData target)
        {
            throw new NotImplementedException();
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

        public override void UpdateBeam(RadarBeam beam, Target.TargetData target)
        {
            strategy.NextBeamDirection(beam);
        }

        public override RadarState NextState()
        {
            return this;
        }
    }
}
