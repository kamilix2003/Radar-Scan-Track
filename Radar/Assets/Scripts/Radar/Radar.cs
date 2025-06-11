using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace RadarSystem
{ 
    public class Radar 
    {
        public RadarBeam beam { get; private set; }

        private RadarState state;

        //private List<TargetSample> targetHistory;

        public Radar()
        {
            beam = new WideBeam();
            state = new ScanState();
        }


        public Vector3 Process()
        {
            state.UpdateBeam(beam, null);
            state = state.NextState();
            return Vector3.zero;
        }

        public Vector2 GetBeamDirection()
        {
            return beam.beamDirection;
        }

    }

}