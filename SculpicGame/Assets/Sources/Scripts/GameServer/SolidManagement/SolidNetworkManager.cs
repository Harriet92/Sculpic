using Assets.Sources.Scripts.DrawerScreen;
using UnityEngine;

namespace Assets.Sources.Scripts.GameServer.SolidManagement
{
    [RequireComponent(typeof(NetworkView))]
    [RequireComponent(typeof(Renderer))]
    public class SolidNetworkManager : MonoBehaviour
    {
        private const bool IsSendVertices = true;
        private const bool IsSendNormals = true;
        private const bool IsSendTangents = false;
        private const bool IsSendUv = false;
        private const bool IsSendTriangles = true;

        private bool _isRecieving;

        void Awake()
        {
            Debug.Log("Method NetworkSolidManager.Awake");
            if (!networkView.isMine)
                renderer.enabled = false;
        }

        void Update()
        {
            if (DrawerGUI.IsSendingScene)
            {
                Debug.Log("Method NetworkSolidManager.Update: DrawerGUI.IsSendingScene");
                DrawerGUI.SynchronizeNextObject();
                networkView.RPC("SynchronizeScene", RPCMode.All);
            }
        }

        [RPC]
        void SynchronizeScene()
        {
            Debug.Log("Method NetworkSolidManager.SynchronizeScene");
            if (!renderer.enabled)
                renderer.enabled = true;
            _isRecieving = true;
        }

        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            if (_isRecieving)
            {
                Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView: _isRecieving");
                if (stream.isWriting)
                    WriteData(stream);
                else
                    ReadData(stream);
                _isRecieving = false;
            }
        }

        private void WriteData(BitStream stream)
        {
            Debug.Log("Method NetworkSolidManager.WriteData");
            var color = renderer.material.color;
            var meshFilter = collider.GetComponent("MeshFilter") as MeshFilter;
            if (meshFilter != null && meshFilter.mesh != null)
            {
                WriteMesh(ref stream, meshFilter.mesh);
            }
            WriteColor(ref stream, color);
        }

        private void ReadData(BitStream stream)
        {
            Debug.Log("Method NetworkSolidManager.ReadData");
            var meshFilter = collider.GetComponent("MeshFilter") as MeshFilter;
            if (meshFilter != null && meshFilter.mesh != null)
            {
                var mesh = new Mesh();
                ReadMesh(ref stream, ref mesh);
                meshFilter.mesh = mesh;
            }
            Color color;
            ReadColor(ref stream, out color);
            renderer.material.color = color;
        }

        private static void WriteMesh(ref BitStream stream, Mesh mesh)
        {
            Debug.Log("Method NetworkSolidManager.WriteMesh");
            if (IsSendVertices) WriteMeshVertices(stream, mesh);
            if (IsSendNormals) WriteMeshNormals(stream, mesh);
            if (IsSendTangents) WriteMeshTangents(stream, mesh);
            if (IsSendUv) WriteMeshUv(stream, mesh);
            if (IsSendTriangles) WriteMeshTriangles(stream, mesh);
        }

        private static void WriteMeshVertices(BitStream stream, Mesh mesh)
        {
            var meshVertices = mesh.vertices;
            var verticesNo = meshVertices.Length;
            stream.Serialize(ref verticesNo);
            for (int i = 0; i < meshVertices.Length; i++)
            {
                stream.Serialize(ref meshVertices[i]);
                //Debug.Log("Vertex [" + i + "]: (" + meshVertices[i].x + ", " + meshVertices[i].y + ", " +
                //          meshVertices[i].z + ")");
            }
        }

        private static void WriteMeshNormals(BitStream stream, Mesh mesh)
        {
            var meshNormals = mesh.normals;
            var normalsNo = meshNormals.Length;
            stream.Serialize(ref normalsNo);
            for (int i = 0; i < meshNormals.Length; i++)
            {
                stream.Serialize(ref meshNormals[i]);
                //Debug.Log("Normal [" + i + "]: (" + meshNormals[i].x + ", " + meshNormals[i].y + ", " +
                //          meshNormals[i].z + ")");
            }
        }

        private static void WriteMeshTangents(BitStream stream, Mesh mesh)
        {
            var meshTangents = mesh.tangents;
            var tangentsNo = meshTangents.Length;
            stream.Serialize(ref tangentsNo);
            for (int i = 0; i < meshTangents.Length; i++)
            {
                var quaternion = new Quaternion(meshTangents[i].x, meshTangents[i].y, meshTangents[i].z,
                    meshTangents[i].w);
                stream.Serialize(ref quaternion);
                //Debug.Log("Tangent [" + i + "]: (" + quaternion.x + ", " + quaternion.y + ", " +
                //          quaternion.z + ", " + quaternion.w + ")");
            }
        }

        private static void WriteMeshUv(BitStream stream, Mesh mesh)
        {
            var meshUv = mesh.uv;
            var uvNo = meshUv.Length;
            stream.Serialize(ref uvNo);
            for (int i = 0; i < meshUv.Length; i++)
            {
                var temporaryUv = new Vector3(meshUv[i].x, meshUv[i].y, 0);
                stream.Serialize(ref temporaryUv);
                //Debug.Log("UV [" + i + "]: (" + temporaryUv.x + ", " + temporaryUv.y + ")");
            }
        }

        private static void WriteMeshTriangles(BitStream stream, Mesh mesh)
        {
            var meshTriangles = mesh.triangles;
            var trianglesNo = meshTriangles.Length;
            stream.Serialize(ref trianglesNo);
            for (int i = 0; i < meshTriangles.Length; i++)
            {
                stream.Serialize(ref meshTriangles[i]);
                //Debug.Log("Triangle [" + i + "]: " + meshTriangles[i]);
            }
        }

        private static void WriteColor(ref BitStream stream, Color color)
        {
            Debug.Log("Method NetworkSolidManager.WriteColor");
            var colorRgb = new Vector3(color.r, color.g, color.b);
            stream.Serialize(ref colorRgb);
        }

        private static void ReadMesh(ref BitStream stream, ref Mesh mesh)
        {
            Debug.Log("Method NetworkSolidManager.ReadMesh");

            mesh.Clear();
            if (IsSendVertices) mesh.vertices = ReadMeshVertices(stream);
            if (IsSendNormals) mesh.normals = ReadMeshNormals(stream);
            if (IsSendTangents) mesh.tangents = ReadMeshTangents(stream);
            if (IsSendUv) mesh.uv = ReadMeshUv(stream);
            if (IsSendTriangles) mesh.triangles = ReadMeshTriangles(stream);
        }

        private static Vector3[] ReadMeshVertices(BitStream stream)
        {
            var newVerticesNo = 0;
            stream.Serialize(ref newVerticesNo);
            //Debug.Log("New vertices no: " + newVerticesNo);
            var newVertices = new Vector3[newVerticesNo];
            for (int i = 0; i < newVertices.Length; i++)
            {
                stream.Serialize(ref newVertices[i]);
                //Debug.Log("New vertex [" + i + "]: (" + newVertices[i].x + ", " + newVertices[i].y + ", " +
                //          newVertices[i].z + ")");
            }
            return newVertices;
        }

        private static Vector3[] ReadMeshNormals(BitStream stream)
        {
            var newNormalsNo = 0;
            stream.Serialize(ref newNormalsNo);
            //Debug.Log("New normals no: " + newNormalsNo);
            var newNormals = new Vector3[newNormalsNo];
            for (int i = 0; i < newNormals.Length; i++)
            {
                stream.Serialize(ref newNormals[i]);
                //Debug.Log("New normal [" + i + "]: (" + newNormals[i].x + ", " + newNormals[i].y + ", " +
                //          newNormals[i].z + ")");
            }
            return newNormals;
        }

        private static Vector4[] ReadMeshTangents(BitStream stream)
        {
            var newTangentsNo = 0;
            stream.Serialize(ref newTangentsNo);
            //Debug.Log("New tangents no: " + newTangentsNo);
            var newTangents = new Vector4[newTangentsNo];
            for (int i = 0; i < newTangents.Length; i++)
            {
                var quaternion = Quaternion.identity;
                stream.Serialize(ref quaternion);
                newTangents[i] = new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                //Debug.Log("New tangent [" + i + "]: (" + newTangents[i].x + ", " + newTangents[i].y + ", " +
                //          newTangents[i].z + ", " + newTangents[i].w + ")");
            }
            return newTangents;
        }

        private static Vector2[] ReadMeshUv(BitStream stream)
        {
            var newUvNo = 0;
            stream.Serialize(ref newUvNo);
            //Debug.Log("New UV no: " + newUvNo);
            var newUv = new Vector2[newUvNo];
            for (int i = 0; i < newUv.Length; i++)
            {
                var temporaryUv = Vector3.zero;
                stream.Serialize(ref temporaryUv);
                //Debug.Log("New UV [" + i + "]: (" + temporaryUv.x + ", " + temporaryUv.y + ")");
                newUv[i] = new Vector2(temporaryUv.x, temporaryUv.y);
            }
            return newUv;
        }

        private static int[] ReadMeshTriangles(BitStream stream)
        {
            var newTrianglesNo = 0;
            stream.Serialize(ref newTrianglesNo);
            //Debug.Log("New triangles no: " + newTrianglesNo);
            var newTrangles = new int[newTrianglesNo];
            for (int i = 0; i < newTrangles.Length; i++)
            {
                stream.Serialize(ref newTrangles[i]);
                //Debug.Log("New triangle [" + i + "]: " + newTrangles[i]);
            }
            return newTrangles;
        }

        private void ReadColor(ref BitStream stream, out Color color)
        {
            Debug.Log("Method NetworkSolidManager.ReadColor");
            var colorRgb = Vector3.zero;
            stream.Serialize(ref colorRgb);
            color = new Color(colorRgb.x, colorRgb.y, colorRgb.z);
        }

        public static Object SpawnSolid(GameObject solidprefab, Vector3 position, Quaternion rotation)
        {
            Debug.Log("Method GameManager.SpawnSolid");
            return Network.Instantiate(solidprefab, position, rotation, 0);
        }
    }
}
