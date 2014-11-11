using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts
{
    public class DisplayUsernameScript : MonoBehaviour
    {
        public Text UsernameText;
        // Use this for initialization
        void Start ()
        {
            UsernameText.text = "Hello " + Player.Current.Username;
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
