using RadarSystem;
using Target;
using UnityEngine;

class RadarScript : MonoBehaviour
{
    public Radar radar = new Radar();
    [SerializeField] BeamManager beamManager;
    public float radarUpdateInterval = 0.1f;


    public TargetTracer targetTracer;
    [SerializeField] private GameObject dotPreFab;
    [SerializeField] private GameObject lineObject;

    public TargetData targetData;

    void Start()
    {
        targetTracer = new TargetTracer(dotPreFab, lineObject);
        InvokeRepeating("UpdateRadar", 0f, radarUpdateInterval);
    }
    void UpdateRadar()
    {
        if (targetData != null && (Time.time - targetData.timeStamp) > radarUpdateInterval * 1f)
        {
            targetData = null;
        }
        radar.Process(targetData, radarUpdateInterval);

        beamManager.UpdateBeam(radar.Beam);
        beamManager.DetectionState(radar.State);

        targetTracer.Trace(radar.State.TrackedTarget, radar.State);

        CancelInvoke("UpdateRadar");
        InvokeRepeating("UpdateRadar", radarUpdateInterval, radarUpdateInterval);
    }
    
}
