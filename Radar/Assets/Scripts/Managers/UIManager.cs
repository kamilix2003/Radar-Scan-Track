using RadarSystem;
using Target;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class UIManager : MonoBehaviour
{
    [Header("Real Target Data")]
    [SerializeField]GameObject realTargetDataPanel;

    [Header("Estimated Target Data")]
    [SerializeField]GameObject estimatedTargetDataPanel;

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
        TargetData targetData = new TargetData(targetScript.gameObject);
        targetData.PopulateAngles(targetScript.gameObject.transform.position);
        realTargetDataPanel.transform.Find("velocity").GetComponent<TextMeshProUGUI>().text = "Velocity: " + targetData.velocity3D.magnitude.ToString("F1") + " m/s";
        realTargetDataPanel.transform.Find("distance").GetComponent<TextMeshProUGUI>().text = "Distance: " + targetData.distance.ToString("F1") + " m";
        Transform positionObject = realTargetDataPanel.transform.Find("position");
        positionObject.Find("x").GetComponent<TextMeshProUGUI>().text = "X\n" + targetData.GetPosition().x.ToString("F1") + " m";
        positionObject.Find("y").GetComponent<TextMeshProUGUI>().text = "Y\n" + targetData.GetPosition().y.ToString("F1") + " m";
        positionObject.Find("z").GetComponent<TextMeshProUGUI>().text = "Z\n" + targetData.GetPosition().z.ToString("F1") + " m";
        //realElevation.text = "Elevation: " + targetData.elevation.ToString("F1") + "°";
        //realAzimuth.text = "Azimuth: " + targetData.azimuth.ToString("F1") + "°";
    }
    private void UpdateEstimated()
    {
        Transform positionObject = estimatedTargetDataPanel.transform.Find("position");
        TargetData trackedTargetData = radarScript.radar.State.TrackedTarget;
        if (trackedTargetData == null)
        {
            estimatedTargetDataPanel.transform.Find("velocity").GetComponent<TextMeshProUGUI>().text = "Velocity: N/A";
            estimatedTargetDataPanel.transform.Find("distance").GetComponent<TextMeshProUGUI>().text = "Distance: N/A";
            //estimatedElevation.text = "Elevation: N/A";
            //estimatedAzimuth.text = "Azimuth: N/A";
            positionObject.Find("x").GetComponent<TextMeshProUGUI>().text = "X\n" + "N/A";
            positionObject.Find("y").GetComponent<TextMeshProUGUI>().text = "Y\n" + "N/A";
            positionObject.Find("z").GetComponent<TextMeshProUGUI>().text = "Z\n" + "N/A";
            return;
        }
        estimatedTargetDataPanel.transform.Find("velocity").GetComponent<TextMeshProUGUI>().text = "Velocity: " + trackedTargetData.velocity3D.magnitude.ToString("F1") + " m/s";
        estimatedTargetDataPanel.transform.Find("distance").GetComponent<TextMeshProUGUI>().text = "Distance: " + trackedTargetData.distance.ToString("F1") + " m";
        //estimatedElevation.text = "Elevation: " + trackedTargetData.elevation.ToString("F1") + "°";
        //estimatedAzimuth.text = "Azimuth: " + trackedTargetData.azimuth.ToString("F1") + "°";
        positionObject.Find("x").GetComponent<TextMeshProUGUI>().text = "X\n" + (-trackedTargetData.GetPosition().x).ToString("F1") + " m";
        positionObject.Find("y").GetComponent<TextMeshProUGUI>().text = "Y\n" + trackedTargetData.GetPosition().y.ToString("F1") + " m";
        positionObject.Find("z").GetComponent<TextMeshProUGUI>().text = "Z\n" + trackedTargetData.GetPosition().z.ToString("F1") + " m";
    }
    private void UpdateRadarUI()
    {
        radarStatus.text = radarScript.radar.State.ToString();
        radarStatus.color = radarScript.radar.State.GetColor();
        radarBeam.text = radarScript.radar.Beam.ToString();
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
