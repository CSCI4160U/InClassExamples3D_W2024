using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    [Header("Mesh")]
    [SerializeField] private int resolutionX = 50;
    [SerializeField] private int resolutionZ = 50;

    [Header("Noise")]
    [SerializeField] private float scaleY = 0.1f;
    [SerializeField] private float zoomX = 9f;
    [SerializeField] private float zoomZ = 9f;

    [Header("Colouring")]
    [SerializeField] private bool useGradient;
    [SerializeField] private Gradient colourGradient;

    [Header("Debugging")]
    [SerializeField] private bool showVertices = true;
    [SerializeField] private float vertexRadius = 0.001f;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvCoords;
    private float minHeight;
    private float maxHeight;
    private Color[] colours;

    private void Start() {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        GenerateTerrain();
    }

    private void Update() {
        GenerateTerrain();
    }

    private void GenerateTerrain() {
        // generate the vertices
        vertices = new Vector3[(resolutionX + 1) * (resolutionZ + 1)];
        uvCoords = new Vector2[(resolutionX + 1) * (resolutionZ + 1)];
        int vIndex = 0;

        for (int xIndex = 0; xIndex <= resolutionX; xIndex++) {
            float x = (float)xIndex / (float)(resolutionX - 1);
            for (int zIndex = 0; zIndex <= resolutionZ; zIndex++) {
                float z = (float)zIndex / (float)(resolutionZ - 1);
                float y = Mathf.PerlinNoise(x * zoomX, z * zoomZ) * scaleY;
                if (y < minHeight) minHeight = y;
                if (y > maxHeight) maxHeight = y;
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

        // generate the per-vertex colours
        if (useGradient) {
            colours = new Color[(resolutionX + 1) * (resolutionZ + 1)];
            for (int v = 0; v < vertices.Length; v++) {
                float heightScale = Mathf.InverseLerp(minHeight, maxHeight, vertices[v].y);
                colours[v] = colourGradient.Evaluate(heightScale);
            }
            mesh.colors = colours;
        }

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