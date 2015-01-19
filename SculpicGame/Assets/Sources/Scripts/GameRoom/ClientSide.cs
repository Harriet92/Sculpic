using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameRoom
{
    public class ClientSide
    {
        private Text _chatTextField;
        private readonly StringBuilder _chatHistory = new StringBuilder();

        private Toggle _wantToDrawToggle;
        public bool WantToDraw { get; set; }

        private DrawingTimer _timer = new DrawingTimer();
        public string RemainingTime { get { return _timer.ToString(); } }
        public float PointsPart { get { return _timer.PointsPart; } }

        public bool HasFinished
        {
            get
            {
                if (!_timer.HasFinished) return false;
                _timer = new DrawingTimer();
                return true;
            }
        }

        public bool IsDrawer
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

        public readonly ActivePlayers ConnectedPlayers = new ActivePlayers();
        public bool IsRegistered { get; set; }
        private bool _isActive;

        public bool CanRegister
        {
            get { return !IsRegistered && _isActive; }
        }

        public string Phrase { get; set; }

        public void OnNewScreenLoad(Text chatTextField, Toggle wantToDrawToggle = null)
        {
            _chatTextField = chatTextField;
            RefreshChat();

            if (wantToDrawToggle != null)
            {
                _wantToDrawToggle = wantToDrawToggle;
                wantToDrawToggle.isOn = WantToDraw;
            }
            _isActive = true;
        }

        private void RefreshChat()
        {
            _chatTextField.text = _chatHistory.ToString();
        }

        public void DisplayMessage(string message)
        {
            if (_chatTextField == null)
            {
                Debug.Log("Room.ChatTextField is null.");
                return;
            }
            _chatHistory.AppendLine(message);
            RefreshChat();
        }

        public void SetDrawer(string phrase)
        {
            Debug.Log("Method ClientSide.SetDrawer");
            IsDrawer = true;
            Phrase = phrase;
        }

        public void RegisterPlayer(NetworkPlayer player, string login)
        {
            Debug.Log("Method ClientSide.RegisterPlayer: adding " + login);
            if (player == Network.player)
                IsRegistered = true;
            ConnectedPlayers.Add(new PlayerData { Login = login, NetworkPlayer = player });
            Debug.Log("Players.Count == " + ConnectedPlayers.Count);
        }

        public void UnregisterPlayer(NetworkPlayer player)
        {
            Debug.Log("Method Room.UnregisterPlayer");
            ConnectedPlayers.Remove(player);
            Debug.Log("Players.Count == " + ConnectedPlayers.Count);
        }

        public void TimerTick()
        {
            _timer.Tick();
        }
    }
}
