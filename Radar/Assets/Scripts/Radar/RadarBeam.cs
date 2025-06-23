
namespace RadarSystem
{
    public abstract class RadarBeam
    {
        public string BeamName { get; protected set; } = "Generic Beam";
        public float BeamWidth { private set; get; }
        public float BeamRange { private set; get; }
        public float Elevation { set; get; }
        public float Azimuth { set; get; }
        public RadarBeam(float width, float range)
        {
            BeamWidth = width;
            BeamRange = range;
            Elevation = 0f;
            Azimuth = 0f;
        }
        public RadarBeam(float width, float range, RadarBeam oldBeam)
        {
            BeamWidth = width;
            BeamRange = range;
            Elevation = oldBeam.Elevation;
            Azimuth = oldBeam.Azimuth;
        }
        public override string ToString()
        {
            return BeamName;
        }
    }
    class NarrowBeam : RadarBeam
    {
        const float narrowBeamWidth = 10f;
        const float narrowBeamRange = 100f;
        public NarrowBeam() : base(narrowBeamWidth, narrowBeamRange)
        {
            BeamName = "Narrow Beam";
        }
        public NarrowBeam(RadarBeam oldBeam) : base(narrowBeamWidth, narrowBeamRange, oldBeam)
        {
            BeamName = "Narrow Beam";
        }
    }
    class WideBeam : RadarBeam
    {
        const float wideBeamWidth = 20f;
        const float wideBeamRange = 50f;
        public WideBeam() : base(wideBeamWidth, wideBeamRange)
        {
            BeamName = "Wide Beam";
        }
        public WideBeam(RadarBeam oldBeam) : base(wideBeamWidth, wideBeamRange, oldBeam)
        {
            BeamName = "Wide Beam";
        }
    }
}