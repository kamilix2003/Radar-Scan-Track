using Target;

namespace RadarSystem
{ 
    class Radar 
    {
        public RadarBeam Beam { get; set; }
        public RadarState State { get; set; }
        public Radar()
        {
            Beam = new WideBeam();
            State = new ScanState();
        }
        public void Process(TargetData targetData, float dt)
        {
            if (targetData != null)
            {
                targetData.PopulateAngles(Beam);
            }
            State = State.UpdateBeam(Beam, targetData, dt);
        }
    }
}