using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen.SolidManagement
{
    public class SolidNetworkManager : MonoBehaviour
    {
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView");
            var meshFilter = collider.GetComponent("MeshFilter") as MeshFilter;
            if (meshFilter != null && meshFilter.mesh != null)
            {
                if (stream.isWriting)
                {
                    WriteMesh(ref stream, meshFilter.mesh);
                }
                else
                {
                    var mesh = new Mesh();
                    ReadMesh(ref stream, ref mesh);
                    meshFilter.mesh = mesh;
                }
            }
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

        //void Update()
        //{
        //    if (Network.isServer && Input.GetKey(KeyCode.C))
        //        ChangeColor();
        //}

        //private void ChangeColor()
        //{
        //    renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //}

        //void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        //{
        //    Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView");
        //    var colorRgb = Vector3.zero;
        //    if (stream.isWriting)
        //    {
        //        var color = renderer.material.color;
        //        colorRgb = new Vector3(color.r, color.g, color.b);
        //    }

        //    stream.Serialize(ref colorRgb);

        //    if (stream.isReading)
        //    {
        //        var color = new Color(colorRgb.x, colorRgb.y, colorRgb.z);
        //        renderer.material.color = color;
        //    }
        //}

        //private static void WriteMesh(ref BitStream stream, Mesh mesh)
        //{
        //    Debug.Log("Method NetworkSolidManager.WriteMesh");
        //    var meshVertices = mesh.vertices;
        //    var verticesNo = meshVertices.Length;

        //    stream.Serialize(ref verticesNo);
        //    for (int i = 0; i < meshVertices.Length; i++)
        //    {
        //        stream.Serialize(ref meshVertices[i]);
        //    }
        //}

        //private static void ReadMesh(ref BitStream stream, ref Mesh mesh)
        //{
        //    Debug.Log("Method NetworkSolidManager.ReadMesh");
        //    var newVerticesNo = 0;
        //    stream.Serialize(ref newVerticesNo);

        //    var newVertices = new Vector3[newVerticesNo];
        //    for (int i = 0; i < newVertices.Length; i++)
        //    {
        //        stream.Serialize(ref newVertices[i]);
        //    }

        //    mesh.vertices = newVertices;
        //}

    }
}
