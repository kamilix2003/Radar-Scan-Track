using RadarSystem;
using UnityEngine;

public class RadarScript : MonoBehaviour
{

    public RadarSystem.Radar radar = new RadarSystem.Radar();

    [SerializeField] GameObject beam;

    [SerializeField] float updateRate = 0.25f;

    void Start()
    {
        InvokeRepeating("UpdateRadar", 0f, updateRate);
        beam.GetComponent<MeshFilter>().mesh = GenerateBeamMesh(radar.beam, 20);
    }

    void UpdateRadar()
    {
        Vector2 beamDir = radar.GetBeamDirection();

        Debug.Log($"Beam direction: {beamDir}");

        beam.transform.rotation = Quaternion.Euler(beamDir.y, beamDir.x, 0);

        radar.Process();
    }

    Mesh GenerateBeamMesh(RadarBeam beam, int segments)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Cone";

        float halfBeamWidth = beam.beamWidth / 2;
        float height = beam.beamRange;
        float radius = Mathf.Tan(halfBeamWidth * Mathf.Deg2Rad) * height;

        // Vertices
        Vector3[] vertices = new Vector3[segments + 2];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uvs = new Vector2[vertices.Length];

        // Tip of the cone
        vertices[0] = Vector3.zero;
        normals[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 1);

        // Base center
        vertices[vertices.Length - 1] = Vector3.zero;
        normals[vertices.Length - 1] = Vector3.down;
        uvs[vertices.Length - 1] = new Vector2(0.5f, 0);

        float angleStep = 2 * Mathf.PI / segments;

        // Circle vertices
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            vertices[i + 1] = new Vector3(x, height, z);
            normals[i + 1] = (new Vector3(x, height, z)).normalized;
            uvs[i + 1] = new Vector2((float)i / segments, 0);
        }

        // Triangles
        int[] triangles = new int[segments * 6];

        // Side triangles
        for (int i = 0; i < segments; i++)
        {
            int current = i + 1;
            int next = i == segments - 1 ? 1 : current + 1;

            // Side
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = next;
            triangles[i * 3 + 2] = current;
        }

        // Base triangles
        int baseIndex = segments * 3;
        int baseCenterIndex = vertices.Length - 1;
        for (int i = 0; i < segments; i++)
        {
            int current = i + 1;
            int next = i == segments - 1 ? 1 : current + 1;

            triangles[baseIndex + i * 3] = baseCenterIndex;
            triangles[baseIndex + i * 3 + 1] = current;
            triangles[baseIndex + i * 3 + 2] = next;
        }

        // Assign to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        return mesh;
    }

}
