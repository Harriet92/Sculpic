﻿using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Sources.Scripts.GameRoom
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        public static ClientSide ClientSide = new ClientSide();
        private static readonly ServerSide ServerSide = new ServerSide();
        private static bool _gameOver;

        private void Awake()
        {
            Debug.Log("Method Room.Awake");
            DontDestroyOnLoad(this);
            InvokeRepeating("UpdateTime", 0, 1);
        }

        public void UpdateTime()
        {
            ClientSide.TimerTick();
            if (ClientSide.HasFinished)
            {
                networkView.RPC("TimeIsUp", RPCMode.Server, Player.Name);
                DisplayInfoPopup("Time's up!");
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
            }
        }

        // RoomOwner
        [RPC]
        public void TimeIsUp(string login)
        {
            ServerSide.TimeIsUp(login);
            networkView.RPC("SetDrawer", ServerSide.CurrentDrawer.NetworkPlayer, ServerSide.CurrentPhrase);
        }

        // RoomOwner
        [RPC]
        public void SignUpForDrawing(NetworkPlayer player, string login)
        {
            ServerSide.SignUpForDrawing(new PlayerData { NetworkPlayer = player, Login = login });
        }

        // RoomOwner
        [RPC]
        public void SignOffFromDrawing(NetworkPlayer player, string login)
        {
            ServerSide.SignOffFromDrawing(player);
        }

        // Player
        public void OnWantToDrawValueChanged(Toggle callingObject)
        {
            Debug.Log("Method Room.OnWantToDrawValueChanged to: " + callingObject.isOn);
            ClientSide.WantToDraw = callingObject.isOn;
            Debug.Log("Method RoomOwner.WantToDrawToggle: wantToDraw == " + ClientSide.WantToDraw);
            networkView.RPC(ClientSide.WantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player, Player.Name);
        }

        void OnApplicationQuit()
        {
            if (Network.isClient)
                UnregisterFromGame();
            Clear();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { LeaveRoom(); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (!_gameOver)
            {
                if (ClientSide.ConnectedPlayers.GameEnds)
                {
                    EndGame();
                    return;
                }

                if (Network.isServer)
                    if (CanStartNewRound)
                        StartNewRound();

                if (Network.isClient)
                    if (ClientSide.CanRegister)
                        RegisterInGame();
            }
            else
            {
                if (Network.isServer)
                    Application.Quit();
            }
        }

        private void LeaveRoom()
        {
            if (Network.isClient)
                UnregisterFromGame();
            Clear();
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }

        // Player
        private void EndGame()
        {
            Debug.Log("Method Room.EndGame");
            if (Network.isServer)
            {
                var userService = new UserService();
                string usernames;
                string scores;
                ClientSide.ConnectedPlayers.GetRankingData(out usernames, out scores);
                userService.UpdateRanking(usernames, scores);
            }
            _gameOver = true;
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); // TODO: load ranking
            Destroy(this);
        }

        public bool CanStartNewRound
        {
            get
            {
                return !ServerSide.DrawingStarted && ServerSide.IsDrawerAvailable &&
                       ClientSide.ConnectedPlayers.IsGuesserAvailable;
            }
        }

        public static void Clear()
        {
            ClientSide = new ClientSide();
        }

        // Player
        private void RegisterInGame()
        {
            Debug.Log("Method Room.RegisterInGame");
            networkView.RPC("RegisterPlayer", RPCMode.AllBuffered, Network.player,
                Player.Current == null ? Random.Range(0, 100).ToString() : Player.Current.Username);
            Chat.AddMessageToDisplay(Chat.YouHaveJoinedMessage, Chat.System, Network.player);
        }

        [RPC]
        public void RegisterPlayer(NetworkPlayer player, string login)
        {
            ClientSide.RegisterPlayer(player, login);
        }

        // Player
        private void UnregisterFromGame()
        {
            Debug.Log("Method Room.UnregisterFromGame");
            networkView.RPC("UnregisterPlayer", RPCMode.AllBuffered, Network.player, Player.Current.Username);
        }

        [RPC]
        public void UnregisterPlayer(NetworkPlayer player, string login)
        {
            ClientSide.UnregisterPlayer(player);
            if (Network.isServer)
            {
                ServerSide.RemoveFromDrawers(player);
                Chat.AddMessageToSend(String.Format(Chat.PlayerHasLeftMessage, login), Chat.System);
            }
        }

        private void DisplayAndCheckMessage(MessageToDisplay message)
        {
            ClientSide.DisplayMessage(message.FullMessage);
            if (Network.isServer)
                CheckPhrase(message);
        }

        private void CheckPhrase(MessageToDisplay message)
        {
            if (ServerSide.MatchesPhrase(message.Message))
            {
                var winner = message.SenderNetworkPlayer;
                networkView.RPC("GetPointsPart", ServerSide.CurrentDrawer.NetworkPlayer, winner);
                Chat.AddMessageToSend(message.WinningMessage, Chat.System);
            }
        }

        // Player
        [RPC]
        public void GetPointsPart(NetworkPlayer winner)
        {
            Debug.Log("Method Room.GetPointsPart");
            var pointsPart = ClientSide.PointsPart;
            networkView.RPC("SendPointsPart", RPCMode.Server, winner, pointsPart);
        }

        // RoomOwner
        [RPC]
        private void SendPointsPart(NetworkPlayer winner, float pointsPart)
        {
            Debug.Log("Method Room.SendPointsPart");
            CountAndSendScore(winner, pointsPart);
        }

        // RoomOwner
        private void CountAndSendScore(NetworkPlayer winner, double pointsPart)
        {
            Debug.Log("Method Room.CountAndSendScore");
            var winnerPoints = ServerSide.PointsForWinner(pointsPart);
            var drawerPoints = ServerSide.PointsForDrawer(pointsPart);
            networkView.RPC("SetPoints", RPCMode.Others, winner, winnerPoints, ServerSide.CurrentDrawer.NetworkPlayer, drawerPoints);
            ServerSide.DrawingStarted = false;
        }

        // Player
        [RPC]
        public void SetPoints(NetworkPlayer winner, int winnerPoints, NetworkPlayer drawer, int drawerPoints)
        {
            Debug.Log("Method Room.SetPoints");
            ClientSide.ConnectedPlayers.AddPoints(winner, winnerPoints);
            ClientSide.ConnectedPlayers.AddPoints(drawer, drawerPoints);
            if (Network.player == winner)
            {
                DisplayInfoPopup("You've got " + winnerPoints + " points!");
            }
            else if (Network.player == drawer)
            {
                DisplayInfoPopup("You've got " + drawer + " points!");
                if (Application.loadedLevelName == SceneName.DrawerScreen.ToString())
                {
                    ClientSide.IsDrawer = false;
                    StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
                }
            }
        }

        private void StartNewRound()
        {
            Debug.Log("Method Room.StartNewRound");
            ServerSide.StartNewRound();
            networkView.RPC("SetDrawer", ServerSide.CurrentDrawer.NetworkPlayer, ServerSide.CurrentPhrase);
        }

        // Player
        [RPC]
        public void SetDrawer(string phrase)
        {
            Debug.Log("Method Room.SetDrawer");
            ClientSide.SetDrawer(phrase);
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.DrawerScreen));
        }
    }
}
