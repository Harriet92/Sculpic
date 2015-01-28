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
        public SoundManager SoundManager;
        private bool _destroyed;
        private void Awake()
        {
            if (_destroyed) return;
            Debug.Log("Method Room.Awake");
            DontDestroyOnLoad(this);
        }

        private void UpdateTime(float deltaTime)
        {
            if (_destroyed) return;
            ClientSide.UpdateTime(deltaTime);
            if (ClientSide.HasFinished)
            {
                networkView.RPC("TimeIsUp", RPCMode.Server, Player.Name);
                DisplayInfoPopup("Time's up!");
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
                _destroyed = true;
            }
        }

        // RoomOwner
        [RPC]
        public void TimeIsUp(string login)
        {
            if (_destroyed) return;
            ServerSide.TimeIsUp(login);
            networkView.RPC("SetDrawer", ServerSide.CurrentDrawer.NetworkPlayer, ServerSide.CurrentPhrase);
        }

        // RoomOwner
        [RPC]
        public void SignUpForDrawing(NetworkPlayer player, string login)
        {
            if (_destroyed) return;
            ServerSide.SignUpForDrawing(new PlayerData { NetworkPlayer = player, Login = login });
        }

        // RoomOwner
        [RPC]
        public void SignOffFromDrawing(NetworkPlayer player, string login)
        {
            if (_destroyed) return;
            ServerSide.SignOffFromDrawing(player);
        }

        // Player
        public void OnWantToDrawValueChanged(Toggle callingObject)
        {
            if (_destroyed) return;
            Debug.Log("Method Room.OnWantToDrawValueChanged to: " + callingObject.isOn);
            ClientSide.WantToDraw = callingObject.isOn;
            Debug.Log("Method RoomOwner.WantToDrawToggle: wantToDraw == " + ClientSide.WantToDraw);
            networkView.RPC(ClientSide.WantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player, Player.Name);
        }

        void OnApplicationQuit()
        {
            if (_destroyed) return;
            if (Network.isClient)
                UnregisterFromGame();
            Network.Disconnect();
        }

        public void Update()
        {
            if (_destroyed) return;
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
            if (_destroyed) return;
            if (Network.isClient)
            {
                UnregisterFromGame();
                Clear();
                Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
                Network.Disconnect();
                _destroyed = true;
            }
        }

        // Player
        private void EndGame()
        {
            if (_destroyed) return;
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
                _destroyed = true;
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
            if (_destroyed) return;
            Debug.Log("Method Room.RegisterInGame");
            networkView.RPC("RegisterPlayer", RPCMode.AllBuffered, Network.player, Player.Name);
            Chat.AddMessageToDisplay(Chat.YouHaveJoinedMessage, Chat.System, Network.player);
            Chat.AddMessageToSend(String.Format(Chat.PlayerHasJoinedMessage, Player.Current.Username), Chat.System, false);
        }

        [RPC]
        public void RegisterPlayer(NetworkPlayer player, string login)
        {
            if (_destroyed) return;
            ClientSide.RegisterPlayer(player, login);
        }

        // Player
        private void UnregisterFromGame()
        {
            if (_destroyed) return;
            Debug.Log("Method Room.UnregisterFromGame");
            networkView.RPC("UnregisterPlayer", RPCMode.AllBuffered, Network.player, Player.Current.Username);
        }

        [RPC]
        public void UnregisterPlayer(NetworkPlayer player, string login)
        {
            if (_destroyed) return;
            ClientSide.UnregisterPlayer(player);
            if (Network.isServer)
            {
                ServerSide.RemoveFromDrawers(player);
                Chat.AddMessageToSend(String.Format(Chat.PlayerHasLeftMessage, login), Chat.System);
            }
        }

        private void DisplayAndCheckMessage(MessageToDisplay message)
        {
            if (_destroyed) return;
            ClientSide.DisplayMessage(message.FullMessage);
            if (Network.isServer)
                CheckPhrase(message);
        }

        private void CheckPhrase(MessageToDisplay message)
        {
            if (_destroyed) return;
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
            if (_destroyed) return;
            Debug.Log("Method Room.GetPointsPart");
            var pointsPart = ClientSide.PointsPart;
            networkView.RPC("SendPointsPart", RPCMode.Server, winner, pointsPart);
        }

        // RoomOwner
        [RPC]
        private void SendPointsPart(NetworkPlayer winner, float pointsPart)
        {
            if (_destroyed) return;
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
            if (_destroyed) return;
            Debug.Log("Method Room.SetPoints");
            ClientSide.ConnectedPlayers.AddPoints(winner, winnerPoints);
            ClientSide.ConnectedPlayers.AddPoints(drawer, drawerPoints);
            if (Network.player == winner)
            {
                SoundManager.PlayYouGuessedSound();
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
            if (_destroyed) return;
            if (Application.loadedLevelName == SceneName.DrawerScreen.ToString())
            {
                ClientSide.IsDrawer = false;
                StartCoroutine(ScreenHelper.LoadLevel(SceneName.GuesserScreen));
                _destroyed = true;
            }
        }

        private void StartNewRound()
        {
            if (_destroyed) return;
            Debug.Log("Method Room.StartNewRound");
            ServerSide.StartNewRound();
            networkView.RPC("SetDrawer", ServerSide.CurrentDrawer.NetworkPlayer, ServerSide.CurrentPhrase);
        }

        [RPC]
        public void StopDrawing()
        {
            if (_destroyed) return;
            Chat.AddMessageToDisplay(Chat.AllPlayersLeft, Chat.System, Network.player);
            UnloadDrawerScreen();
        }

        // Player
        [RPC]
        public void SetDrawer(string phrase)
        {
            if (_destroyed) return;
            Debug.Log("Method Room.SetDrawer");
            ClientSide.SetDrawer(phrase);
            StartCoroutine(ScreenHelper.LoadLevel(SceneName.DrawerScreen));
            _destroyed = true;
        }
    }
}
