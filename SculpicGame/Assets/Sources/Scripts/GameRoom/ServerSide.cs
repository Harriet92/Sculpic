using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    public static class ServerSide
    {
        private const int WinnerBasePoints = 20;
        private const int DrawerBasePoints = 30;
        private static readonly List<PlayerData> Drawers = new List<PlayerData>();

        public static bool DrawingStarted { get; set; }
        public static string CurrentPhrase;
        public static PlayerData CurrentDrawer;

        public static bool IsDrawerAvailable { get { return Drawers.Count > 0; } }

        public static void SignUpForDrawing(PlayerData player)
        {
            Debug.Log("Method ServerSide.SignUpForDrawing");
            Drawers.Add(player);
            Debug.Log("Drawers count: " + Drawers.Count);
        }

        public static void SignOffFromDrawing(NetworkPlayer player)
        {
            Debug.Log("Method ServerSide.SignOffFromDrawing");
            RemoveFromDrawers(player);
        }

        public static void RemoveFromDrawers(NetworkPlayer player)
        {
            Debug.Log("Method ServerSide.RemoveFromDrawers");
            var drawer = Drawers.FirstOrDefault(pd => pd.NetworkPlayer == player);
            if (drawer != null && Drawers.Remove(drawer))
            {
                Debug.Log("Removed player from drawing queue.");
                if (CurrentDrawer != null && drawer == CurrentDrawer)
                    ClearDrawer();
            }
            Debug.Log("Drawers count: " + Drawers.Count);
        }

        public static bool MatchesPhrase(string phrase)
        {
            return String.Equals(phrase, CurrentPhrase, StringComparison.CurrentCultureIgnoreCase);
        }

        public static void StartNewRound()
        {
            DrawingStarted = true;
            SetNewPhrase();
            SetNextDrawer();
        }

        private static void SetNewPhrase()
        {
            Debug.Log("Method ServerSide.SetNewPhrase");
            var phraseService = new PhraseService();
            var newPhrase = phraseService.DrawPhrase();
            Debug.Log("newPhrase == " + newPhrase);
            CurrentPhrase = newPhrase;
        }

        private static void SetNextDrawer()
        {
            Debug.Log("Method ServerSide.SetNextDrawer");
            CurrentDrawer = Drawers[0];
            if (Drawers.Remove(CurrentDrawer))
                if (!Drawers.Contains(CurrentDrawer))
                    Drawers.Add(CurrentDrawer);
            Chat.AddMessageToSend(String.Format(Chat.NextDrawerMessage, CurrentDrawer.Login), Chat.System);
        }

        public static void TimeIsUp(string login)
        {
            Chat.AddMessageToSend(String.Format(Chat.TimeIsUp, login), Chat.System);
            StartNewRound();
        }

        public static int PointsForWinner(double pointsPart)
        {
            return (int)(pointsPart * WinnerBasePoints);
        }

        public static int PointsForDrawer(double pointsPart)
        {
            return (int)(pointsPart * DrawerBasePoints);
        }

        public static void ClearDrawer()
        {
            DrawingStarted = false;
            CurrentDrawer = null;
        }

        public static void PlayerLeavingRoom(NetworkPlayer player, string login)
        {
            if (DrawingStarted && CurrentDrawer != null && CurrentDrawer.NetworkPlayer == player)
                Chat.AddMessageToSend(Chat.DrawingPlayerLeft, Chat.System);
            else
                Chat.AddMessageToSend(String.Format(Chat.PlayerHasLeftMessage, login), Chat.System);
            RemoveFromDrawers(player);
        }
    }
}