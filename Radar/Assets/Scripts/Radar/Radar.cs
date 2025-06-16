using System;
using NUnit.Framework;
using Targets;
using Unity.VisualScripting;
using UnityEngine;

namespace RadarSystem
{ 
    class Radar 
    {
        public RadarBeam beam { get; private set; }

        public RadarState state { get; private set; }

        //private List<TargetSample> targetHistory;

        public Radar()
        {
            beam = new WideBeam();
            state = new ScanState();
        }


        public Vector3 Process(TargetData targetData)
        {
            state = state.UpdateBeam(beam, targetData);
            return Vector3.zero;
        }

    }

}