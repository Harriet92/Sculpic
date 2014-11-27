using Assets.Sources.Common;
using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen
{
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
        private bool carve;
        private float radius = 1.0f;
        private string radiusString = "0.3";
        private float pull = 10.5f;
        private string pullString = "2.0";
        private FallOff fallOff = FallOff.Gauss;
        private MeshFilter unappliedMesh;
        private Rect windowRect = new Rect(20, 20, 120, 50);

        private void OnGUI()
        {
            GUI.BeginGroup(new Rect(0, 0, 400, 180));

            GUI.Box(new Rect(0, 0, 400, 180),
                "Hello " + (Player.Current == null ? "Stanger!" : Player.Current.Username + "!") + "\n" +
                "Sculpt the surface using the mouse. Use arrow keys to rotate view.");
            fallOff = (FallOff)GUI.Toolbar(new Rect(20, 35, 360, 30), (int)fallOff, _fallOffScrings);
            carve = GUI.Toggle(new Rect(20, 65, 60, 20), carve, "Carve");
            grid = GUI.Toggle(new Rect(100, 65, 60, 20), grid, "Grid");
            GUI.Label(new Rect(20, 90, 40, 20), "Radius");
            radiusString = GUI.TextField(new Rect(70, 90, 100, 20), radiusString, 4);
            if (!radiusString.EndsWith(".") || !radiusString.EndsWith(".0"))
            {
                float.TryParse(radiusString, out radius);
            }
            GUI.Label(new Rect(20, 120, 40, 20), "Pull");
            pullString = GUI.TextField(new Rect(70, 120, 100, 20), pullString, 4);
            if (!pullString.EndsWith(".") || !pullString.EndsWith(".0"))
            {
                float.TryParse(pullString, out pull);
            }

            GUI.EndGroup();
        }

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