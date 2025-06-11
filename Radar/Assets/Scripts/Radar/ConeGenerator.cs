using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeGenerator : MonoBehaviour
{
    [Header("Cone Settings")]
    public float height = 2f;
    public float beamWidth = 15f;
    public int segments = 20;

    private void Start()
    {
        //GetComponent<MeshFilter>().mesh = GenerateCone(height, beamWidth, segments);
    }

    Mesh GenerateBeamMesh(float height, float beamWidth, int segments)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Cone";

        float radius = Mathf.Tan(beamWidth * Mathf.Deg2Rad) * height;

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
