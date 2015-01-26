using System;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.GameRoom
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        private const float DestructionTime = 10;
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
            Network.Disconnect();
        }

        public void Update()
        {
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
                if (CanStartNewRound)
                    StartNewRound();

            if (Network.isClient)
                if (ClientSide.CanRegister)
                    RegisterInGame();
        }

        private void LeaveRoom()
        {
            if (Network.isClient)
            {
                UnregisterFromGame();
                Clear();
                Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
                Network.Disconnect();
                Destroy(this, DestructionTime);
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
            Chat.AddMessageToSend(String.Format(Chat.PlayerHasJoinedMessage, Player.Current.Username), Chat.System);
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
