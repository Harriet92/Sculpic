using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameRoom
{
    public static class ClientSide
    {
        private const int MaxScore = 150;
        private static Text _chatTextField;
        private static StringBuilder _chatHistory = new StringBuilder();

        private static Toggle _wantToDrawToggle;
        public static bool WantToDraw { get; set; }

        private static DrawingTimer _timer = new DrawingTimer();
        public static string RemainingTime { get { return _timer.ToString(); } }
        public static float PointsPart { get { return _timer.PointsPart; } }

        public static bool IsLoading { get; set; }

        public static bool HasFinished
        {
            get
            {
                if (!_timer.HasFinished) return false;
                _timer = new DrawingTimer();
                return true;
            }
        }

        public static bool IsDrawer
        {
            get { return _timer.IsOn; }
            set
            {
                if (!value)
                {
                    _timer = new DrawingTimer();
                }
                else _timer.IsOn = true;
            }
        }

        public static ActivePlayers ConnectedPlayers = new ActivePlayers(MaxScore);

        private static bool _isRegistered;

        public static bool CanRegister
        {
            get
            {
                if (_isRegistered) return false;
                _isRegistered = true;
                return true;
            }
        }

        public static string Phrase { get; private set; }
        
        public static void Reset()
        {
            _chatTextField = null;
            _chatHistory = new StringBuilder();
            _wantToDrawToggle = null;
            WantToDraw = false;
            _timer = new DrawingTimer();
            ConnectedPlayers = new ActivePlayers(MaxScore);
            _isRegistered = false;
        }

        public static void OnNewScreenLoad(Text chatTextField, Toggle wantToDrawToggle = null)
        {
            _chatTextField = chatTextField;
            RefreshChat();

            if (wantToDrawToggle != null)
            {
                _wantToDrawToggle = wantToDrawToggle;
                wantToDrawToggle.isOn = WantToDraw;
            }
        }

        private static void RefreshChat()
        {
            _chatTextField.text = _chatHistory.ToString();
        }

        public static void DisplayMessage(string message)
        {
            if (_chatTextField == null)
            {
                Debug.Log("Room.ChatTextField is null.");
                return;
            }
            _chatHistory.AppendLine(message);
            RefreshChat();
        }

        public static void SetDrawer(string phrase)
        {
            Debug.Log("Method ClientSide.SetDrawer");
            IsDrawer = true;
            Phrase = phrase;
        }

        public static void RegisterPlayer(NetworkPlayer player, string login)
        {
            Debug.Log("Method ClientSide.RegisterPlayer: adding " + login);
            if (player == Network.player)
                _isRegistered = true;
            ConnectedPlayers.Add(new PlayerData { Login = login, NetworkPlayer = player });
            Debug.Log("Players.Count == " + ConnectedPlayers.Count);
        }

        public static void UnregisterPlayer(NetworkPlayer player)
        {
            Debug.Log("Method ClientSide.UnregisterPlayer");
            ConnectedPlayers.Remove(player);
            Debug.Log("Players.Count == " + ConnectedPlayers.Count);
        }

        public static void UpdateTime(float deltaTime)
        {
            _timer.UpdateTime(deltaTime);
        }

        public static void ClearScene()
        {
            Debug.Log("Method ClientSide.ClearScene");
            Network.RemoveRPCs(Network.player);
            Network.DestroyPlayerObjects(Network.player);
        }
    }
}
