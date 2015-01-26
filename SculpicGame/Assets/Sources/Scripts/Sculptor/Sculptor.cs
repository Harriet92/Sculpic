using UnityEngine;

namespace Assets.Sources.Scripts.Sculptor
{
    public class Sculptor : MonoBehaviour
    {
        private enum FallOff
        {
            Gauss = 1,
            Linear = 2,
            Needle = 3
        }

        private FallOff fallOff = FallOff.Gauss;

        private void Update()
        {
            if (SculptorCurrentSettings.Move || SculptorCurrentSettings.Rotate)
                return;
            // TODO: change input to Input.Touch

            // Did we hit the surface?
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var filter = (MeshFilter)hit.collider.GetComponent("MeshFilter");
                if (filter)
                {
                    if (Input.GetMouseButton(0))
                    {
                        var relativePoint = filter.transform.InverseTransformPoint(hit.point);
                        DeformMesh(filter.mesh, relativePoint);
                        UpdateMeshCollider(hit, filter.mesh);
                    }
                }
            }
        }

        private static void UpdateMeshCollider(RaycastHit hit, Mesh mesh)
        {
            var meshCollider = (MeshCollider) hit.collider.GetComponent("MeshCollider");
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
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

        private void DeformMesh(Mesh mesh, Vector3 position)
        {
            var power = SculptorCurrentSettings.Pull * Time.deltaTime;
            var inRadius = SculptorCurrentSettings.Radius;

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
                averageNormal += LinearFalloff(distance, inRadius) * normals[i];
            }
            averageNormal = averageNormal.normalized;

            // Deform vertices along averaged normal
            for (int i = 0; i < vertices.Length; i++)
            {
                var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                    continue;

                var distance = Mathf.Sqrt(sqrMagnitude);
                float falloff;
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

                vertices[i] += averageNormal * falloff * power * (SculptorCurrentSettings.Carve ? -1 : 1);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}