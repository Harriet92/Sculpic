using Assets.Sources.Common;
using UnityEngine;

namespace Assets.Sources.Scripts.Sculptor
{
    // TODO: redo
    public class Sculptor : MonoBehaviour
    {
        public enum FallOff
        {
            Gauss = 1,
            Linear = 2,
            Needle = 3
        }

        private readonly string[] _fallOffScrings = { "Gauss", "Linear", "Needle" };

        private bool grid;
        private bool carve { get { return SculptorCurrentSettings.Carve; } }
        private float radius {get { return SculptorCurrentSettings.Radius; }}
        private float pull {get { return SculptorCurrentSettings.Pull; }}
        private FallOff fallOff = FallOff.Gauss;
        private MeshFilter unappliedMesh;
        private Rect windowRect = new Rect(20, 20, 120, 50);

        private void Update()
        {
            // When no button is pressed we update the mesh collider
            if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
            {
                // Apply collision mesh when we let go of button
                ApplyMeshCollider();
                return;
            }

            // Did we hit the surface?
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(radius);
                MeshFilter filter = (MeshFilter)hit.collider.GetComponent("MeshFilter");
                if (filter)
                {
                    if (grid)
                    {
                        // Debug.Log("DRAW");
                        DrawGrid(filter.mesh);
                        return;
                    }
                    // Debug.Log("UNDRAW");
                    UnDrawGrid(filter.mesh);
                    // Don't update mesh collider every frame since physX
                    // does some heavy processing to optimize the collision mesh.
                    // So this is not fast enough for real time updating every frame
                    if (filter != unappliedMesh)
                    {
                        ApplyMeshCollider();
                        unappliedMesh = filter;
                    }

                    if (Input.GetMouseButton(0) && !grid)
                    {
                        // Debug.Log("MESH");
                        // Deform mesh
                        var relativePoint = filter.transform.InverseTransformPoint(hit.point);
                        DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);

                    }
                }
                //drawClickedTriangle(hit);
            }
        }

        private static void DrawGrid(Mesh mesh)
        {
            mesh.SetIndices(mesh.GetIndices(0), MeshTopology.LineStrip, 0);
        }

        private static void UnDrawGrid(Mesh mesh)
        {
            mesh.SetIndices(mesh.GetIndices(0), MeshTopology.Triangles, 0);
        }

        private static float LinearFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(1.0f - distance / inRadius);
        }

        private static float GaussFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
        }

        private static float NeedleFalloff(float dist, float inRadius)
        {
            return -(dist * dist) / (inRadius * inRadius) + 1.0f;
        }

        private void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var sqrRadius = inRadius * inRadius;

            // Calculate averaged normal of all surrounding vertices	
            var averageNormal = Vector3.zero;
            for (int i = 0; i < vertices.Length; i++)
            {
                var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                    continue;

                var distance = Mathf.Sqrt(sqrMagnitude);
                var falloff_ = LinearFalloff(distance, inRadius);
                averageNormal += falloff_ * normals[i];
            }
            averageNormal = averageNormal.normalized;

            // Deform vertices along averaged normal
            float falloff;
            for (int i = 0; i < vertices.Length; i++)
            {
                var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                    continue;

                var distance = Mathf.Sqrt(sqrMagnitude);
                switch (fallOff)
                {
                    case FallOff.Gauss:
                        falloff = GaussFalloff(distance, inRadius);
                        break;
                    case FallOff.Needle:
                        falloff = NeedleFalloff(distance, inRadius);
                        break;
                    default:
                        falloff = LinearFalloff(distance, inRadius);
                        break;
                }

                vertices[i] += averageNormal * falloff * power * (carve ? -1 : 1);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }


        private void ApplyMeshCollider()
        {
            if (unappliedMesh && unappliedMesh.GetComponent("MeshCollider"))
            {
                unappliedMesh.mesh = unappliedMesh.mesh;
                unappliedMesh.sharedMesh = unappliedMesh.mesh;
                (unappliedMesh.GetComponent("MeshCollider") as MeshCollider).sharedMesh = unappliedMesh.mesh;
            }
            unappliedMesh = null;
        }
    }
}