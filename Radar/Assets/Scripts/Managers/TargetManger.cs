
using UnityEngine;
using Targets;

class TargetManager : MonoBehaviour
{

    [SerializeField] GameObject targetPrefab;
    public GameObject targetObject;

    private void Start()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned.");
            return;
        }
        Vector3 position = new Vector3(10, 10, 10);
        Quaternion rotation = Quaternion.identity;
        //targetObject = CreateTarget(targetPrefab, position, rotation);
    }
    public GameObject CreateTarget(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject target = Object.Instantiate(prefab, position, rotation);
        target.SetActive(true);
        return target;
    }
}