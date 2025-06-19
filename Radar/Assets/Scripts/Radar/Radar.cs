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


        public void Process(TargetData targetData, float dt)
        {
            if (targetData != null)
            {
                targetData.PopulateAngles(beam);
            }
            state = state.UpdateBeam(beam, targetData, dt);
        }

    }

}