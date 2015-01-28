using Assets.Sources.Scripts.GameRoom;
using Assets.Sources.Scripts.TouchLogic;
using UnityEngine;

namespace Assets.Sources.Scripts.Sculptor
{
    [RequireComponent(typeof(NetworkView))]
    [RequireComponent(typeof(Collider))]
    public class Sculptor : TouchHandling
    {
        private enum FallOff
        {
            Gauss = 1,
            Linear = 2,
            Needle = 3
        }

        private FallOff fallOff = FallOff.Gauss;

        private void OnTouch()
        {
            Debug.Log("Method Sculptor.OnTouch");
            if (!ClientSide.IsDrawer) return;
            if (SculptorCurrentSettings.Move || SculptorCurrentSettings.Rotate)
                return;

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Method Sculptor.OnTouch: hit");
                if (hit.collider != collider) return;
                var filter = (MeshFilter)collider.GetComponent("MeshFilter");
                if (filter)
                {
                    Debug.Log("Method Sculptor.OnTouch: filter");
                    var relativePoint = filter.transform.InverseTransformPoint(hit.point);
                    var power = SculptorCurrentSettings.Pull * Time.deltaTime;
                    var radius = SculptorCurrentSettings.Radius;
                    DeformMesh(filter.mesh, relativePoint, power, radius);
                    UpdateMeshCollider(filter.mesh);
                    Debug.Log("Calling RPC Sculpt");
                    networkView.RPC("Sculpt", RPCMode.Others, relativePoint, power, radius);
                }
            }
        }

        [RPC]
        public void Sculpt(Vector3 relativePoint, float power, float radius)
        {
            Debug.Log("Method Sculptor.Sculpt");
            var filter = (MeshFilter)collider.GetComponent("MeshFilter");
            if (filter)
            {
                DeformMesh(filter.mesh, relativePoint, power, radius);
                UpdateMeshCollider(filter.mesh);
            }
        }

        private void UpdateMeshCollider(Mesh mesh)
        {
            Debug.Log("Method Sculptor.UpdateMeshCollider");
            var meshCollider = (MeshCollider)collider.GetComponent("MeshCollider");
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

        private void DeformMesh(Mesh mesh, Vector3 position, float power, float radius)
        {
            Debug.Log("Method Sculptor.DeformMesh");
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var sqrRadius = radius * radius;

            // Calculate averaged normal of all surrounding vertices	
            var averageNormal = Vector3.zero;
            for (int i = 0; i < vertices.Length; i++)
            {
                var sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                    continue;

                var distance = Mathf.Sqrt(sqrMagnitude);
                averageNormal += LinearFalloff(distance, radius) * normals[i];
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
                        falloff = GaussFalloff(distance, radius);
                        break;
                    case FallOff.Needle:
                        falloff = NeedleFalloff(distance, radius);
                        break;
                    default:
                        falloff = LinearFalloff(distance, radius);
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