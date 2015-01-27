using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using Assets.Sources.Scripts.Music;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameRoom
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        private SoundManager _soundManager;
        private bool _isDestroyed;
        private void Awake()
        {
            Debug.Log("Method Room.Awake");
            _soundManager = FindObjectOfType<SoundManager>();
            DontDestroyOnLoad(this);
        }

        private void UpdateTime(float deltaTime)
        {
            if (_isDestroyed) return;
            ClientSide.UpdateTime(deltaTime);
            if (ClientSide.HasFinished)
            {
                networkView.RPC("TimeIsUp", RPCMode.Server, Player.Name);
                DisplayInfoPopup("Time's up!");
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
                _isDestroyed = true;
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
            Network.Disconnect();
        }

        public void Update()
        {
            if (_isDestroyed) return;
            if (ClientSide.IsLoading) return;

            if (Input.GetKeyDown(KeyCode.Escape)) { LeaveRoom(); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (ClientSide.ConnectedPlayers.GameEnds)
            {
                EndGame();
                return;
            }

            if (Network.isServer)
            {
                if (CanStartNewRound)
                    StartNewRound();
                if (ClientSide.ConnectedPlayers.Count == 1 && ServerSide.CurrentDrawer != null)
                {
                    networkView.RPC("StopDrawing", ServerSide.CurrentDrawer.NetworkPlayer);
                    ServerSide.ClearDrawer();
                }
            }

            if (Network.isClient)
            {
                if (ClientSide.CanRegister)
                    RegisterInGame();
                if (ClientSide.IsDrawer)
                    UpdateTime(Time.deltaTime);
            }
        }



        public void LeaveRoom()
        {
            if (Network.isClient)
            {
                UnregisterFromGame();
                Clear();
                Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
                Network.Disconnect();
                _isDestroyed = true;
            }
        }

        // Player
        private void EndGame()
        {
            Debug.Log("Method Room.EndGame");
            if (Network.isServer)
            {
                Debug.Log("Method Room.EndGame: Network.isServer");
                var userService = new UserService();
                string usernames;
                string scores;
                ClientSide.ConnectedPlayers.GetRankingData(out usernames, out scores);
                userService.UpdateRanking(usernames, scores);
                Application.Quit();
            }
            if (Network.isClient)
            {
                Debug.Log("Method Room.EndGame: Network.isClient");
                Application.LoadLevel(SceneName.RankingScreen.ToString());
                Network.Disconnect();
                _isDestroyed = true;
            }
        }

        private bool CanStartNewRound
        {
            get
            {
                return !ServerSide.DrawingStarted && ServerSide.IsDrawerAvailable &&
                       ClientSide.ConnectedPlayers.IsGuesserAvailable;
            }
        }

        public static void Clear()
        {
            ClientSide.Reset();
        }

        // Player
        private void RegisterInGame()
        {
            Debug.Log("Method Room.RegisterInGame");
            networkView.RPC("RegisterPlayer", RPCMode.AllBuffered, Network.player, Player.Current.Username);
            Chat.AddMessageToDisplay(Chat.YouHaveJoinedMessage, Chat.System, Network.player);
            Chat.AddMessageToSend(String.Format(Chat.PlayerHasJoinedMessage, Player.Current.Username), Chat.System, false);
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
            var winnerPoints = ServerSide.PointsForWinner(pointsPart);
            var drawerPoints = ServerSide.PointsForDrawer(pointsPart);
            networkView.RPC("SetPoints", RPCMode.AllBuffered, winner, winnerPoints, ServerSide.CurrentDrawer.NetworkPlayer, drawerPoints);
            ServerSide.ClearDrawer();
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
                _soundManager.PlayYouGuessedSound();
                DisplayInfoPopup("You've got " + winnerPoints + " points!");
            }
            else if (Network.player == drawer)
            {
                DisplayInfoPopup("You've got " + drawer + " points!");
                UnloadDrawerScreen();
            }
        }

        private void UnloadDrawerScreen()
        {
            if (Application.loadedLevelName == SceneName.DrawerScreen.ToString())
            {
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
                _isDestroyed = true;
            }
        }

        private void StartNewRound()
        {
            Debug.Log("Method Room.StartNewRound");
            ServerSide.StartNewRound();
            networkView.RPC("SetDrawer", ServerSide.CurrentDrawer.NetworkPlayer, ServerSide.CurrentPhrase);
        }

        [RPC]
        public void StopDrawing()
        {
            Chat.AddMessageToDisplay(Chat.AllPlayersLeft, Chat.System, Network.player);
            UnloadDrawerScreen();
        }

        // Player
        [RPC]
        public void SetDrawer(string phrase)
        {
            Debug.Log("Method Room.SetDrawer");
            ClientSide.SetDrawer(phrase);
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.DrawerScreen));
            _isDestroyed = true;
        }
    }
}
