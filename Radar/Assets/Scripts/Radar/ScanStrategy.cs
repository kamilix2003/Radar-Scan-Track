
using System;
using NUnit.Framework;
using UnityEngine;

namespace RadarSystem
{
    abstract class ScanStrategy
    {
        protected const float YAW_UPPER_LIMIT = 85f;
        protected const float YAW_LOWER_LIMIT = 5f;

        public abstract void NextBeamDirection(RadarBeam beam);
    }
    class HorizontalScan : ScanStrategy
    {
        public HorizontalScan() { }
        public override void NextBeamDirection(RadarBeam beam)
        {
            Vector2 nextDirection = new Vector2(beam.Azimuth, beam.Elevation);
            nextDirection.x += beam.BeamWidth;

            if (nextDirection.x > 360)
            {
                nextDirection.x = 0;
                nextDirection.y += beam.BeamWidth;
            }
            if (nextDirection.y > YAW_UPPER_LIMIT || nextDirection.y < YAW_LOWER_LIMIT)
            {
                nextDirection.y = YAW_LOWER_LIMIT;
            }

            beam.Elevation = nextDirection.y;
            beam.Azimuth = nextDirection.x;
        }
    }

    class VerticalScan : ScanStrategy
    {
        public VerticalScan() { }
        public override void NextBeamDirection(RadarBeam beam)
        {
            Vector2 nextDirection = new Vector2(beam.Elevation, beam.Azimuth);
            nextDirection.x += beam.BeamWidth;
            if (nextDirection.x > 360)
            {
                nextDirection.x = 0;
                nextDirection.y += beam.BeamWidth;
            }
            if (nextDirection.y > YAW_UPPER_LIMIT || nextDirection.y < YAW_LOWER_LIMIT)
            {
                nextDirection.y = YAW_LOWER_LIMIT;
            }
            beam.Elevation = nextDirection.y;
            beam.Azimuth = nextDirection.x;
        }
    }
}