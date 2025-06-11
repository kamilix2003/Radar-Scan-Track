
using System;
using NUnit.Framework;
using UnityEngine;

namespace RadarSystem
{
    abstract class ScanStrategy
    {
        protected const float YAW_UPPER_LIMIT = 85f;
        protected const float YAW_LOWER_LIMIT = 5f;

        public ScanStrategy()
        {

        }

        public abstract void NextBeamDirection(RadarBeam beam);
    }

    class PolarScan : ScanStrategy
    {

        public PolarScan() : base() { }

        public override void NextBeamDirection(RadarBeam beam)
        {
            throw new NotImplementedException();
        }

    }

    class HorizontalScan : ScanStrategy
    {
        public HorizontalScan() : base() { }

        public override void NextBeamDirection(RadarBeam beam)
        {
            Vector2 nextDirection = beam.beamDirection;
            nextDirection.x += beam.beamWidth;

            if (nextDirection.x > 360)
            {
                nextDirection.x = 0;
                nextDirection.y += beam.beamWidth;
            }
            if (nextDirection.y > YAW_UPPER_LIMIT || nextDirection.y < YAW_LOWER_LIMIT)
            {
                nextDirection.y = YAW_LOWER_LIMIT;
            }

            beam.beamDirection = nextDirection;
        }
        
    }

    class VerticalScan : ScanStrategy
    {
        public VerticalScan() : base()
        {

        }

        public override void NextBeamDirection(RadarBeam beam)
        {

        }

    }

}