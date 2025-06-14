
using UnityEngine;
using Targets;

class TargetManager : MonoBehaviour
{

    [SerializeField] GameObject targetPrefab;
    GameObject targetObject;

    Target testTarget = new Target();

    private void Start()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned.");
            return;
        }
        testTarget.position = new Vector3(10, 10, 10);
        testTarget.rotation = Quaternion.identity;
        targetObject = CreateTarget(targetPrefab, testTarget.position, testTarget.rotation);
    }

    private void Update()
    {
        if (targetObject != null)
        {
            UpdateTarget(targetObject, testTarget);
        }
    }

    public GameObject CreateTarget(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject target = Object.Instantiate(prefab, position, rotation);
        target.SetActive(true);
        return target;
    }

    public void UpdateTarget(GameObject targetObject, Target target)
    {
        if (targetObject == null || target == null)
        {
            Debug.LogError("Target object or target data is null.");
            return;
        }

        //TargetData targetData = new TargetData(target);
        //targetObject.transform.position = target.position;
        //targetObject.transform.rotation = Quaternion.Euler(targetData.elevation, targetData.azimuth, 0);
        // Optionally, you can add more logic to update the target's visual representation
        // based on the targetData properties like distance, velocity, etc.
    }
}