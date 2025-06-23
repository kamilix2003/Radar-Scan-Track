using RadarSystem;
using Target;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

class UIManager : MonoBehaviour
{
    [Header("Real Target Data")]
    [SerializeField] GameObject targetObject;
    public TargetData trackedTargetData;

    [Header("Radar Data")]
    public RadarSystem.Radar radar;

    //[Header("UI Elements")]
    [Header("Real Target Data")]
    [SerializeField] TextMeshProUGUI realVelocity;
    [SerializeField] TextMeshProUGUI realDistance;
    [SerializeField] TextMeshProUGUI realElevation;
    [SerializeField] TextMeshProUGUI realAzimuth;

    [Header("Estimated Target Data")]
    [SerializeField] TextMeshProUGUI estimatedVelocity;
    [SerializeField] TextMeshProUGUI estimatedDistance;
    [SerializeField] TextMeshProUGUI estimatedElevation;
    [SerializeField] TextMeshProUGUI estimatedAzimuth;

    [Header("Radar UI")]
    [SerializeField] TextMeshProUGUI radarStatus;
    [SerializeField] TextMeshProUGUI radarBeam;

    [Header("Radar Settings")]
    [SerializeField] TextMeshProUGUI radarUpdateRateText;
    [SerializeField] RadarScript radarScript;
    [SerializeField] TragetScript targetScript;

    public void Start()
    {
        OnUpdateRateChange(0.1f);
    }
    public void Update()
    {
        UpdateReal();
        UpdateEstimated();
        UpdateRadarUI();
    }
    private void UpdateReal()
    {
        TargetData targetData = new TargetData(targetObject);
        targetData.PopulateAngles(targetObject.transform.position);
        realVelocity.text = "Velocity: " + targetData.velocity3D.magnitude.ToString("F1") + " m/s";
        realDistance.text = "Distance: " + targetData.distance.ToString("F1") + " m";
        realElevation.text = "Elevation: " + targetData.elevation.ToString("F1") + "°";
        realAzimuth.text = "Azimuth: " + targetData.azimuth.ToString("F1") + "°";
    }
    private void UpdateEstimated()
    {
        if (trackedTargetData == null)
        {
            estimatedVelocity.text = "Velocity: N/A";
            estimatedDistance.text = "Distance: N/A";
            estimatedElevation.text = "Elevation: N/A";
            estimatedAzimuth.text = "Azimuth: N/A";
            return;
        }
        estimatedVelocity.text = "Velocity: " + trackedTargetData.velocity3D.magnitude.ToString("F1") + " m/s";
        estimatedDistance.text = "Distance: " + trackedTargetData.distance.ToString("F1") + " m";
        estimatedElevation.text = "Elevation: " + trackedTargetData.elevation.ToString("F1") + "°";
        estimatedAzimuth.text = "Azimuth: " + trackedTargetData.azimuth.ToString("F1") + "°";
    }
    private void UpdateRadarUI()
    {
        radarStatus.text = radar.State.ToString();
        radarStatus.color = radar.State.GetColor();
        radarBeam.text = radar.Beam.ToString();
    }
    public void OnTargetTraceToggle(bool traceEnable)
    {
        radarScript.targetTracer.ChangeVisibility(traceEnable);
    }
    public void OnTargetTraceClear()
    {
        radarScript.targetTracer.Clear();
    }
    public void OnTargetVisibility(bool hideTarget)
    {
        targetScript.HideTarget(hideTarget);
    }
    public void OnUpdateRateChange(float value)
    {
        value = Mathf.Clamp(value, 0.03f, 1f); // Ensure the value is within a reasonable range
        radarScript.radarUpdateInterval = value;
        radarUpdateRateText.text = "Update Rate: " + (value * 1000).ToString("F0") + " ms";
    }
    public void OnReset()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void OnQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        #endif
    }
    public void OnState()
    {
        if (radarScript.radar.State.StateName != "Searching")
        {
            radarScript.radar.State = new RadarSystem.ScanState();
            radarScript.radar.Beam.Elevation = 90f; // Reset elevation to 90 degrees
            radarScript.radar.Beam.Azimuth = 0f; // Reset azimuth to 0 degrees
        }
    }
    public void OnBeam()
    {
        if (radarScript.radar.Beam.BeamName == "Wide Beam")
        {
            radarScript.radar.Beam = new NarrowBeam(radarScript.radar.Beam);
        }
        else
        {
            radarScript.radar.Beam = new WideBeam(radarScript.radar.Beam);
        }
    }
}
