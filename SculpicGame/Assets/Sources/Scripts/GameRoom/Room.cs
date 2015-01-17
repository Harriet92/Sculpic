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
        }

        // RoomOwner
        [RPC]
        public void SignUpForDrawing(NetworkPlayer player, string login)
        {
            ServerSide.SignUpForDrawing(new PlayerData {NetworkPlayer = player, Login = login});
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
            networkView.RPC(ClientSide.WantToDraw ? "SignUpForDrawing" : "SignOffFromDrawing", RPCMode.Server, Network.player, Player.Current != null ? Player.Current.Username : "Stranger");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { LeaveRoom(); }
            if (Chat.HasMessageToDisplay)
                DisplayAndCheckMessage(Chat.GetMessageToDisplay());

            if (Network.isServer)
                if (ServerSide.CanStartNewGame)
                    StartNewGame();

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
                Chat.AddMessageToSend(message.WinningMessage, Chat.System);
                CountAndSendScore(message.SenderNetworkPlayer);
            }
        }

        private void CountAndSendScore(NetworkPlayer winner)
        {
            Debug.Log("Method Room.CountAndSendScore");
            var points = ServerSide.WinnerPoints;
            networkView.RPC("SetWinner", RPCMode.Others, winner, points);
            ServerSide.DrawingStarted = false;
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

        private void StartNewGame()
        {
            Debug.Log("Method Room.StartNewGame");
            ServerSide.StartNewGame();
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
