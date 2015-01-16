using System;
using System.Collections.Generic;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Sources.Scripts.GameServer
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        // Player
        public static ClientSide ClientSide = new ClientSide();

        // RoomOwner
        private const int WinnerPoints = 5;
        private readonly List<NetworkPlayer> _drawers = new List<NetworkPlayer>();

        private bool _drawingStarted;
        public static string CurrentPhrase;
        private NetworkPlayer _currentDrawer;

        private void Awake()
        {
            Debug.Log("Method Room.Awake");
            DontDestroyOnLoad(this);
        }

        // RoomOwner
        [RPC]
        public void SignUpForDrawing(NetworkPlayer player)
        {
            Debug.Log("Method Room.SignUpForDrawing");
            _drawers.Add(player);
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        // RoomOwner
        [RPC]
        public void SignOffFromDrawing(NetworkPlayer player)
        {
            Debug.Log("Method Room.SignOffFromDrawing");
            if (_drawers.Remove(player))
                Debug.Log("Removed player from drawing queue.");
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        // Player
        public void OnWantToDrawValueChanged(Toggle callingObject)
        {
            Debug.Log("Method Room.OnWantToDrawValueChanged");
            ClientSide.WantToDraw = callingObject.isOn;
            Debug.Log("Method RoomOwner.WantToDrawToggle: wantToDraw == " + ClientSide.WantToDraw);
            networkView.RPC(ClientSide.WantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { LeaveRoom(); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (Network.isServer)
                if (IsNewGame())
                    StartNewGame();

            if (Network.isClient)
                if (ClientSide.CanRegister)
                    RegisterInGame();
        }

        private void LeaveRoom()
        {
            if (Network.isClient)
                UnregisterInGame();
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }

        void OnApplicationQuit()
        {
            if (Network.isClient)
                UnregisterInGame();
            Clear();
        }

        public static void Clear()
        {
            if (ClientSide != null)
                ClientSide.Clear();
            ClientSide = new ClientSide();
        }

        // Player
        private void RegisterInGame()
        {
            Debug.Log("Method Room.RegisterInGame");
            networkView.RPC("RegisterPlayer", RPCMode.AllBuffered, Network.player, Player.Current == null ? Random.Range(0, 100).ToString() : Player.Current.Username); // TODO: change random to Player.Current.Username
        }

        [RPC]
        public void RegisterPlayer(NetworkPlayer player, string login)
        {
            if (player == Network.player)
                ClientSide.IsRegistered = true;
            Debug.Log("Method Room.RegisterPlayer: adding " + login);
            ClientSide.ConnectedPlayers.Add(new PlayerData { Login = login, NetworkPlayer = player });
            Debug.Log("Players.Count == " + ClientSide.ConnectedPlayers.Count);
        }

        // Player
        private void UnregisterInGame()
        {
            Debug.Log("Method Room.UnregisterInGame");
            networkView.RPC("UnregisterPlayer", RPCMode.AllBuffered, Network.player);
        }

        [RPC]
        public void UnregisterPlayer(NetworkPlayer player)
        {
            Debug.Log("Method Room.UnregisterPlayer");
            ClientSide.ConnectedPlayers.Remove(player);
            Debug.Log("Players.Count == " + ClientSide.ConnectedPlayers.Count);
        }

        private void DisplayAndCheckMessage(MessageToDisplay message)
        {
            ClientSide.DisplayMessage(message.FullMessage);
            if (Network.isServer)
                CheckPhrase(message);
        }

        private void CheckPhrase(MessageToDisplay message)
        {
            if (String.Equals(message.Message, CurrentPhrase, StringComparison.CurrentCultureIgnoreCase))
            {
                Chat.AddMessageToSend(message.WinningMessage, Chat.System);
                CountAndSendScore(message.SenderNetworkPlayer);
            }
        }

        private void CountAndSendScore(NetworkPlayer winner)
        {
            Debug.Log("Method Room.CountAndSendScore");
            var points = WinnerPoints;
            networkView.RPC("SetWinner", RPCMode.Others, winner, points);
            _drawingStarted = false;
        }

        [RPC]
        public void SetWinner(NetworkPlayer winner, int points)
        {
            Debug.Log("Method Room.SetWinner");
            ClientSide.ConnectedPlayers.AddPoints(winner, points);
            if (Network.player == winner)
            {
                DisplayInfoPopup("You've got " + points + " points!");
            }
            else if (Application.loadedLevelName != SceneName.GuesserScreen.ToString())
            {
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
            }
        }

        private bool IsNewGame()
        {
            return !_drawingStarted && _drawers.Count > 0;
        }

        private void StartNewGame()
        {
            Debug.Log("Method Room.StartNewGame");
            _drawingStarted = true;
            SetNewPhrase();
            SetNextDrawer();
        }

        private void SetNextDrawer()
        {
            Debug.Log("Method Room.SetNextDrawer");
            _currentDrawer = _drawers[0];
            if (_drawers.Remove(_currentDrawer))
                if (!_drawers.Contains(_currentDrawer))
                    _drawers.Add(_currentDrawer);
            Chat.AddMessageToSend(String.Format(Chat.NextDrawerMessage, ClientSide.ConnectedPlayers.GetLogin(_currentDrawer)), Chat.System);
            networkView.RPC("SetDrawer", _currentDrawer, CurrentPhrase);
        }

        private void SetNewPhrase()
        {
            Debug.Log("Method Room.SetNewPhrase");
            var phraseService = new PhraseService();
            var newPhrase = phraseService.DrawPhrase();
            Debug.Log("Method RoomOwner.GetNewPhrase: newPhrase == " + newPhrase);
            CurrentPhrase = newPhrase;
        }

        // Player
        [RPC]
        public void SetDrawer(string phrase)
        {
            Debug.Log("Method Room.SetDrawer");
            ClientSide.IsDrawer = true;
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.DrawerScreen));
            CurrentPhrase = phrase;
        }
    }
}
