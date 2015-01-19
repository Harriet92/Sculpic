using Assets.Sources.Common;
using Assets.Sources.Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Sources.Scripts.GameRoom
{
    [RequireComponent(typeof(NetworkView))]
    class Room : MenuBase
    {
        // Player
        public static ClientSide ClientSide = new ClientSide();

        // RoomOwner
        private static readonly ServerSide ServerSide = new ServerSide();

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
        }

        // RoomOwner
        [RPC]
        public void SignUpForDrawing(NetworkPlayer player, string login)
        {
            ServerSide.SignUpForDrawing(new PlayerData { NetworkPlayer = player, Login = login });
        }

        // RoomOwner
        [RPC]
        public void SignOffFromDrawing(NetworkPlayer player)
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

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { LeaveRoom(); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (Network.isServer)
                if (ServerSide.CanStartNewRound)
                    StartNewRound();

            if (Network.isClient)
                if (ClientSide.CanRegister)
                    RegisterInGame();
        }

        private void LeaveRoom()
        {
            if (Network.isClient)
                UnregisterInGame();
            Clear();
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
            ClientSide.RegisterPlayer(player, login);
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
            ClientSide.UnregisterPlayer(player);
            if (Network.isServer)
                ServerSide.RemoveFromDrawers(player);
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
