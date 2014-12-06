using UnityEngine;

namespace Assets.Sources.Scripts.GameServer.SolidManagement
{
    public class SolidNetworkManager : MonoBehaviour
    {
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView");
            if (stream.isWriting)
                WriteData(stream);
            else
                ReadData(stream);
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

            // vertices
            var meshVertices = mesh.vertices;
            var verticesNo = meshVertices.Length;
            stream.Serialize(ref verticesNo);
            for (int i = 0; i < meshVertices.Length; i++)
            {
                //var temp = Vector3.one;
                //stream.Serialize(ref temp);
                stream.Serialize(ref meshVertices[i]);
                Debug.Log("Vertex [" + i + "]: (" + meshVertices[i].x + ", " + meshVertices[i].y + ", " + meshVertices[i].z + ")");
            }

            //// uv
            //var meshUv = mesh.uv;
            //var uvNo = meshUv.Length;
            //stream.Serialize(ref uvNo);
            //for (int i = 0; i < meshUv.Length; i++)
            //{
            //    //var temp = Vector3.one;
            //    //stream.Serialize(ref temp);
            //    var temporaryUv = new Vector3(meshUv[i].x, meshUv[i].y, 0);
            //    stream.Serialize(ref temporaryUv);
            //    Debug.Log("UV [" + i + "]: (" + temporaryUv.x + ", " + temporaryUv.y + ")");
            //}

            // triangles
            var meshTriangles = mesh.triangles;
            var trianglesNo = meshTriangles.Length;
            stream.Serialize(ref trianglesNo);
            for (int i = 0; i < meshTriangles.Length; i++)
            {
                //var temp = 1;
                //stream.Serialize(ref temp);
                stream.Serialize(ref meshTriangles[i]);
                Debug.Log("Triangle [" + i + "]: " + meshTriangles[i]);
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

            // vertices
            var newVerticesNo = 0;
            stream.Serialize(ref newVerticesNo);
            Debug.Log("New vertices no: " + newVerticesNo);
            var newVertices = new Vector3[newVerticesNo];
            for (int i = 0; i < newVertices.Length; i++)
            {
                stream.Serialize(ref newVertices[i]);
                Debug.Log("New vertex [" + i + "]: (" + newVertices[i].x + ", " + newVertices[i].y + ", " + newVertices[i].z + ")");
            }

            //// uv
            //var newUvNo = 0;
            //stream.Serialize(ref newUvNo);
            //Debug.Log("New UV no: " + newUvNo);
            //var newUv = new Vector2[newUvNo];
            //for (int i = 0; i < newUv.Length; i++)
            //{
            //    var temporaryUv = Vector3.zero;
            //    stream.Serialize(ref temporaryUv);
            //    Debug.Log("New UV [" + i + "]: (" + temporaryUv.x + ", " + temporaryUv.y + ")");
            //    newUv[i] = new Vector2(temporaryUv.x, temporaryUv.y);
            //}

            // triangles
            var newTrianglesNo = 0;
            stream.Serialize(ref newTrianglesNo);
            Debug.Log("New triangles no: " + newTrianglesNo);
            var newTrangles = new int[newTrianglesNo];
            for (int i = 0; i < newTrangles.Length; i++)
            {
                stream.Serialize(ref newTrangles[i]);
                Debug.Log("New triangle [" + i + "]: " + newTrangles[i]);
            }

            mesh.Clear();
            mesh.vertices = newVertices;
            //mesh.uv = newUv;
            mesh.triangles = newTrangles;
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

        // TODO: removing object from scene
    }
}
