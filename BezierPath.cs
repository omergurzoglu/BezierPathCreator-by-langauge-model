    using UnityEngine;

    public class BezierPath : MonoBehaviour
    {
        // An array of control points for the Bezier curve
        public Vector3[] points;

        // The number of segments in the Bezier curve
        public int numSegments = 50;

        // The width of the Bezier curve
        public float width = 1.0f;

        // The material to use for the Bezier curve
        public Material material;

        // A reference to the MeshFilter component
        private MeshFilter meshFilter;

        void Start()
        {
            // Get a reference to the MeshFilter component
            meshFilter = GetComponent<MeshFilter>();

            // Generate the Bezier curve mesh
            GenerateBezierCurve();
        }

        void GenerateBezierCurve()
        {
            // Calculate the length of the curve
            float curveLength = CalculateCurveLength();

            // Create a new mesh
            Mesh mesh = new Mesh();

            // Create an array of vertices for the mesh
            Vector3[] vertices = new Vector3[numSegments * 2];

            // Create an array of UVs for the mesh
            Vector2[] uvs = new Vector2[numSegments * 2];

            // Create an array of triangles for the mesh
            int[] triangles = new int[numSegments * 6];

            // Set the initial position of the curve
            Vector3 currentPosition = CalculateBezierPoint(0.0f);

            // Set the initial UVs for the curve
            Vector2 currentUV = new Vector2(0.0f, 0.0f);

            // Set the initial tangent for the curve
            Vector3 tangent = CalculateBezierTangent(0.0f);

            // Calculate the initial normal for the curve
            Vector3 normal = Vector3.Cross(tangent, Vector3.up).normalized;

            // Set the initial position of the left side of the curve
            Vector3 left = currentPosition - normal * width * 0.5f;

            // Set the initial position of the right side of the curve
            Vector3 right = currentPosition + normal * width * 0.5f;

            // Set the initial vertex indices
            int v0 = 0;
            int v1 = 1;
            int v2 = 2;
            int v3 = 3;

            // Set the initial triangle indices
            int t0 = 0;
            int t1 = 1;
            int t2 = 2;
            int t3 = 3;
            int t4 = 4;
            int t5 = 5;

            // Set the first vertices and UVs for the curve
            vertices[v0] = left;
            vertices[v1] = right;
            uvs[v0] = currentUV;
            uvs[v1] = currentUV;

            // Set the first triangles for the curve
            triangles[t0] = v0;
            triangles[t1] = v1;
            triangles[t2] = v2;
            triangles[t3] = v2;
            triangles[t4] = v1;
            triangles[t5] = v3;

            // Loop
            // Loop through the segments of the curve
            for (int i = 1; i < numSegments; i++)
            {
                // Calculate the t value for the current segment
                float t = (float)i / (float)numSegments;

                // Calculate the position of the curve at t
                currentPosition = CalculateBezierPoint(t);

                // Calculate the tangent of the curve at t
                tangent = CalculateBezierTangent(t);

                // Calculate the normal of the curve at t
                normal = Vector3.Cross(tangent, Vector3.up).normalized;

                // Calculate the left and right positions of the curve at t
                left = currentPosition - normal * width * 0.5f;
                right = currentPosition + normal * width * 0.5f;

                // Update the vertex and UV indices
                v0 = i * 2;
                v1 = i * 2 + 1;
                v2 = i * 2 + 2;
                v3 = i * 2 + 3;

                // Make sure the vertex indices are within the bounds of the vertices array
                if (v2 >= vertices.Length || v3 >= vertices.Length)
                {
                    break;
                }

                // Update the triangle indices
                t0 = i * 6;
                t1 = i * 6 + 1;
                t2 = i * 6 + 2;
                t3 = i * 6 + 3;
                t4 = i * 6 + 4;
                t5 = i * 6 + 5;

                // Set the vertices and UVs for the segment
                vertices[v0] = left;
                vertices[v1] = right;
                uvs[v0] = currentUV;
                uvs[v1] = currentUV;

                // Set the triangles for the segment
                triangles[t0] = v0;
                triangles[t1] = v1;
                triangles[t2] = v2;
                triangles[t3] = v2;
                triangles[t4] = v1;
                triangles[t5] = v3;

                // Update the UVs for the next segment
                currentUV.x += curveLength;
            }



            // Assign the vertices, UVs, and triangles to the mesh
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            // Set the material for the mesh
            meshFilter.mesh = mesh;
            meshFilter.GetComponent<Renderer>().material = material;
        }

        // Calculate the length of the Bezier curve
        private float CalculateCurveLength()
        {
            // Set the initial and final positions of the curve
            Vector3 initialPosition = CalculateBezierPoint(0.0f);
            Vector3 finalPosition = CalculateBezierPoint(1.0f);

            // Calculate and return the length of the curve
            return Vector3.Distance(initialPosition, finalPosition);
        }

        // Calculate the position of the Bezier curve at t
        // Calculate the position of the Bezier curve at t
            Vector3 CalculateBezierPoint(float t)
            {
                // Calculate the Bezier point using the De Casteljau algorithm
                Vector3 point = 
                    Mathf.Pow(1 - t, 3) * points[0] +
                    3 * Mathf.Pow(1 - t, 2) * t * points[1] +
                    3 * (1 - t) * Mathf.Pow(t, 2) * points[2] +
                    Mathf.Pow(t, 3) * points[3];

                // Return the Bezier point
                return point;
            }

        // Calculate the tangent of the Bezier curve at t
            Vector3 CalculateBezierTangent(float t)
            {
                // Calculate the tangent of the Bezier curve using the derivative of the curve
                Vector3 tangent = 
                    3 * Mathf.Pow(1 - t, 2) * (points[1] - points[0]) +
                    6 * (1 - t) * t * (points[2] - points[1]) +
                    3 * Mathf.Pow(t, 2) * (points[3] - points[2]);

                // Return the tangent of the Bezier curve
                return tangent;
            }
        }


