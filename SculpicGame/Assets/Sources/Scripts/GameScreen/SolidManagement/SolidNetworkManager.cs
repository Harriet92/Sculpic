using UnityEngine;

namespace Assets.Sources.Scripts.GameScreen.SolidManagement
{
    public class SolidNetworkManager : MonoBehaviour
    {
        //void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        //{
        //    Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView");
        //    var meshFilter = collider.GetComponent("MeshFilter") as MeshFilter;
        //    if (meshFilter != null && meshFilter.mesh != null)
        //    {
        //        if (stream.isWriting)
        //        {
        //            WriteMesh(ref stream, meshFilter.mesh);
        //        }
        //        else
        //        {
        //            var mesh = new Mesh();
        //            ReadMesh(ref stream, ref mesh);
        //            meshFilter.mesh = mesh;
        //        }
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

        void Update()
        {
            if (Network.isServer && Input.GetKey(KeyCode.C))
                ChangeColor();
        }

        private void ChangeColor()
        {
            renderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Debug.Log("Method NetworkSolidManager.OnSerializeNetworkView");
            var colorRgb = Vector3.zero;
            if (stream.isWriting)
            {
                var color = renderer.material.color;
                colorRgb = new Vector3(color.r, color.g, color.b);
            }

            stream.Serialize(ref colorRgb);

            if (stream.isReading)
            {
                var color = new Color(colorRgb.x, colorRgb.y, colorRgb.z);
                renderer.material.color = color;
            }
        }

        private static void WriteMesh(ref BitStream stream, Mesh mesh)
        {
            Debug.Log("Method NetworkSolidManager.WriteMesh");
            var meshVertices = mesh.vertices;
            var verticesNo = meshVertices.Length;

            stream.Serialize(ref verticesNo);
            for (int i = 0; i < meshVertices.Length; i++)
            {
                stream.Serialize(ref meshVertices[i]);
            }
        }

        private static void ReadMesh(ref BitStream stream, ref Mesh mesh)
        {
            Debug.Log("Method NetworkSolidManager.ReadMesh");
            var newVerticesNo = 0;
            stream.Serialize(ref newVerticesNo);

            var newVertices = new Vector3[newVerticesNo];
            for (int i = 0; i < newVertices.Length; i++)
            {
                stream.Serialize(ref newVertices[i]);
            }

            mesh.vertices = newVertices;
        }

    }
}
