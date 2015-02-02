using Assets.Sources.Scripts.Sculptor;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom.SolidManagement
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

        public static Object SpawnSolid(GameObject solidprefab, Vector3 position, Quaternion rotation,
            Color? color = null)
        {
            Debug.Log("Method SolidNetworkManager.SpawnSolid");
            return Network.Instantiate(solidprefab, position, rotation, 0);
        }

        private void Awake()
        {
            Debug.Log("Method SolidNetworkManager.Awake");
            if (networkView.isMine)
            {
                var color = SculptorCurrentSettings.MaterialColor;
                renderer.material.color = color;
                //networkView.RPC("SetColor", RPCMode.AllBuffered, new Vector3(color.r, color.g, color.b));
            }
        }

        //[RPC]
        //public void SetColor(Vector3 color)
        //{
        //    Debug.Log("Method SolidNetworkManager.SetColor");
        //    Debug.Log("Color: [" + color.x + ", " + color.y + ", " + color.z + "].");
        //    var materialColor = new Color(color.x, color.y, color.z);
        //    renderer.material.color = materialColor;
        //}

        private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Debug.Log("Method SolidNetworkManager.OnSerializeNetworkView");
            Vector3 color = Vector3.zero;
            if (stream.isWriting)
            {
                Debug.Log("writing...");
                color = new Vector3(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b);
                stream.Serialize(ref color);
            }
            else
            {
                Debug.Log("reading...");
                stream.Serialize(ref color);
                renderer.material.color = new Color(color.x, color.y, color.z);
            }
        }
    }
}
