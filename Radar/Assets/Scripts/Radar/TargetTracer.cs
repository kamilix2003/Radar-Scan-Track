using System.Collections.Generic;
using NUnit.Framework;
using RadarSystem;
using Target;
using UnityEngine;
class TargetTracer
{
    public GameObject dotPreFab;
    public GameObject LineObject;
    public TargetTracer(GameObject dotPreFab, GameObject lineObject)
    {
        this.dotPreFab = dotPreFab;
        LineObject = lineObject;
    }
    public void Trace(TargetData target, RadarState state)
    {
        if (target == null || state == null)
        {
            return;
        }
        Vector3 temp = target.GetPosition();
        temp.x = -temp.x;
        GameObject newObject = Object.Instantiate(dotPreFab, temp, Quaternion.identity);
        Color stateColor = state.GetColor();
        stateColor.a = 0.4f;
        newObject.GetComponent<MeshRenderer>().material.color = stateColor;
        newObject.transform.SetParent(LineObject.transform);
    }
    public void Clear()
    {
        foreach (Transform child in LineObject.transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
    public void ChangeVisibility(bool visible)
    {
        LineObject.SetActive(visible);
    }
}