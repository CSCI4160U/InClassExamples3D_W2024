using UnityEngine;

public class GridGenerator : MonoBehaviour {
    [Header("Mesh")]
    [SerializeField] private int resolutionX = 50;
    [SerializeField] private int resolutionZ = 50;

    [Header("Debugging")]
    [SerializeField] private bool showVertices = true;
    [SerializeField] private float vertexRadius = 0.001f;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvCoords;

    private void Start() {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        GenerateGrid();
    }

    private void Update() {
        GenerateGrid();
    }

    private void GenerateGrid() {
        // generate the vertices
        vertices = new Vector3[(resolutionX + 1) * (resolutionZ + 1)];
        uvCoords = new Vector2[(resolutionX + 1) * (resolutionZ + 1)];
        int vIndex = 0;

        for (int xIndex = 0; xIndex <= resolutionX; xIndex++) {
            float x = (float)xIndex / (float)(resolutionX - 1);
            for (int zIndex = 0; zIndex <= resolutionZ; zIndex++) {
                float z = (float)zIndex / (float)(resolutionZ - 1);
                float y = 0f;
                vertices[vIndex] = new Vector3(x, y, z);
                uvCoords[vIndex] = new Vector2(x, z);
                vIndex++;
            }
        }

        // generate the triangles
        triangles = new int[resolutionX * resolutionZ * 6];
        int tIndex = 0;
        int offset = 0;
        for (int xIndex = 0; xIndex < resolutionX; xIndex++) {
            for (int zIndex = 0; zIndex < resolutionZ; zIndex++) {
                // create a single quad (made up of two triangles) for each four verts
                triangles[tIndex++] = offset + 1;               // same row, next column
                triangles[tIndex++] = offset + resolutionX + 1; // next row, same column
                triangles[tIndex++] = offset + 0;               // same row, same column

                triangles[tIndex++] = offset + resolutionX + 2; // next row, next column
                triangles[tIndex++] = offset + resolutionX + 1; // next row, same column
                triangles[tIndex++] = offset + 1;               // same row, next column

                offset++;
            }
            offset++;
        }


        // update the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvCoords;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos() {
        if (showVertices && (vertices != null) && (vertices.Length > 0)) {
            for (int v = 0; v < vertices.Length; v++) {
                Gizmos.DrawSphere(vertices[v], vertexRadius);
            }
        }
    }
}