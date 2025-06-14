using RadarSystem;
using UnityEngine;

public class RadarScript : MonoBehaviour
{

    public Radar radar = new Radar();

    [SerializeField] float updateRate = 0.25f;

    [SerializeField] BeamManager beamManager;

    void Start()
    {
        InvokeRepeating("UpdateRadar", 0f, updateRate);
    }

    private void Update()
    {

    }

    void UpdateRadar()
    {
        Vector2 beamDir = radar.GetBeamDirection();
        //Debug.Log($"Beam direction: {beamDir}");

        if (beamManager == null)
        {
            Debug.LogError("BeamManager is not assigned.");
            return;
        }

        beamManager.UpdateBeam(beamManager.beamObject, radar.beam);


        radar.Process();

        CancelInvoke("UpdateRadar");
        InvokeRepeating("UpdateRadar", updateRate, updateRate);
    }

}
