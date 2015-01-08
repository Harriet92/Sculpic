using System;
using System.Collections.Generic;
using System.Text;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameServer
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        private const int WinnerPrize = 5;
        public Text ChatTextField;
        // Player
        private bool _isDrawer;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // RoomOwner
        private readonly List<NetworkPlayer> _drawers = new List<NetworkPlayer>();

        private bool _drawingStarted;
        public static string CurrentPhrase;
        private NetworkPlayer _currentDrawer;

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
            var wantToDraw = callingObject.isOn;
            Debug.Log("Method RoomOwner.WantToDrawToggle: wantToDraw == " + wantToDraw);
            networkView.RPC(wantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (Network.isServer)
                if (IsNewGame())
                    StartNewGame();
        }

        private void DisplayAndCheckMessage(MessageToDisplay message)
        {
            //DisplayMessage(message.FullMessage);
            if (Network.isServer)
                CheckPhrase(message);
        }

        private void DisplayMessage(string message)
        {
            var builder = new StringBuilder(ChatTextField.text);
            ChatTextField.text = builder.AppendLine(message).ToString();
        }

        private void CheckPhrase(MessageToDisplay message)
        {
            if (String.Equals(message.Message, CurrentPhrase, StringComparison.CurrentCultureIgnoreCase))
            {
                //DisplayMessage(message.WinningMessage);
                CountAndSendScore(message.SenderNetworkPlayer);
            }
        }

        private void CountAndSendScore(NetworkPlayer winner)
        {
            Debug.Log("Method Room.CountAndSendScore");
            var points = WinnerPrize;
            networkView.RPC("SetWinner", RPCMode.All, winner, points);
            _drawingStarted = false;
        }

        [RPC]
        public void SetWinner(NetworkPlayer winner, int points)
        {
            // TODO: add points to the winner in player dictionary
            Debug.Log("Method Room.SetWinner");
            if (Network.player == winner)
            {
                Debug.Log("WINNER!");
                DisplayInfoPopup("You've got " + points + " points!");
            }
            else if (Application.loadedLevelName != SceneName.GuesserScreen.ToString())
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
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
            _isDrawer = true;
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.DrawerScreen));
            CurrentPhrase = phrase;
        }

        // TODO: player dictionary

        // TODO: recieving data (winner, points, next drawer)
        // TODO: adding points to winner
        // TODO: load screen (drawer/guesser)

        // TODO: add self to drawing queue
    }
}
