using RadarSystem;
using Targets;
using UnityEngine;

class RadarScript : MonoBehaviour
{
    Radar radar = new Radar();
    [SerializeField] float radarUpdateInterval = 0.1f;
    [SerializeField] float beamUpdateInterval = 0.5f;
    [SerializeField] BeamManager beamManager;
    public TargetData targetData;

    void Start()
    {
        InvokeRepeating("UpdateRadar", 0f, radarUpdateInterval);
        InvokeRepeating("UpdateBeam", 0f, radarUpdateInterval);
    }
    void UpdateRadar()
    {
        if (targetData != null && (Time.time - targetData.timeStamp) > radarUpdateInterval * 1f)
        {
            targetData = null;
        }
        radar.Process(targetData);

        CancelInvoke("UpdateRadar");
        InvokeRepeating("UpdateRadar", radarUpdateInterval, radarUpdateInterval);
    }
    void UpdateBeam()
    {
        Vector2 beamDir = radar.beam.beamDirection;
        //Debug.Log($"Beam direction: {beamDir}");

        if (beamManager == null)
        {
            Debug.LogError("BeamManager is not assigned.");
            return;
        }

        beamManager.UpdateBeam(radar.beam);
        beamManager.DetectionState(radar.state);

        CancelInvoke("UpdateBeam");
        InvokeRepeating("UpdateBeam", beamUpdateInterval, beamUpdateInterval);
    }
}
