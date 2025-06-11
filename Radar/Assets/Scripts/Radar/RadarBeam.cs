using System;
using NUnit.Framework;
using UnityEngine;

namespace RadarSystem
{
    public abstract class RadarBeam
    {
        public float beamWidth { private set; get; }          // Beam width in degrees
        public float beamRange { private set; get; }
        public Vector2 beamDirection { set; get; }    // Beam direction in (azimuth, elevation)

        public RadarBeam(float width, float range)
        {
            beamWidth = width;
            beamRange = range;
            beamDirection = Vector2.zero; // Default direction
        }

    }

    class NarrowBeam : RadarBeam
    {

        [SerializeField] const float narrowBeamWidth = 5f;
        [SerializeField] const float narrowBeamRange = 100f;

        public NarrowBeam() : base(narrowBeamWidth, narrowBeamRange)
        {

        }
    }

    class WideBeam : RadarBeam
    {

        [SerializeField] const float wideBeamWidth = 20f;
        [SerializeField] const float wideBeamRange = 50f;

        public WideBeam() : base(wideBeamWidth, wideBeamRange)
        {
        }
    }

}