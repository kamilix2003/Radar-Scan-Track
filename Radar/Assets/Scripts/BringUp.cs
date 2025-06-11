

namespace Target
{
    class TargetData
    {
        public float distance { get; private set; }
        public float velocity { get; private set; }
        public float elevation { get; set; }
        public float azimuth { get; set; }
        public uint age { get; private set; }
    }
}