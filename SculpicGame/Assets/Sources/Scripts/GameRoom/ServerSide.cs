using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;

namespace Assets.Sources.Scripts.GameRoom
{
    internal class ServerSide
    {
        public const int WinnerPoints = 5;
        private readonly List<PlayerData> _drawers = new List<PlayerData>();

        public bool DrawingStarted { get; set; }
        public string CurrentPhrase;
        public PlayerData CurrentDrawer;

        public bool CanStartNewRound
        {
            get { return !DrawingStarted && _drawers.Count > 0; }
        }

        public void SignUpForDrawing(PlayerData player)
        {
            Debug.Log("Method ServerSide.SignUpForDrawing");
            _drawers.Add(player);
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        public void SignOffFromDrawing(NetworkPlayer player)
        {
            Debug.Log("Method Room.SignOffFromDrawing");
            RemoveFromDrawers(player);
        }

        public void RemoveFromDrawers(NetworkPlayer player)
        {
            Debug.Log("Method ServerSide.RemoveFromDrawers");
            var drawer = _drawers.FirstOrDefault(pd => pd.NetworkPlayer == player);
            if (drawer != null &&_drawers.Remove(drawer))
                Debug.Log("Removed player from drawing queue.");
            if (drawer == CurrentDrawer)
            {
                DrawingStarted = false;
                Chat.AddMessageToSend("Drawing player has left...", Chat.System);
            }
            Debug.Log("Drawers count: " + _drawers.Count);
        }

        public bool MatchesPhrase(string phrase)
        {
            return String.Equals(phrase, CurrentPhrase, StringComparison.CurrentCultureIgnoreCase);
        }

        public void StartNewRound()
        {
            DrawingStarted = true;
            SetNewPhrase();
            SetNextDrawer();
        }

        private void SetNewPhrase()
        {
            Debug.Log("Method ServerSide.SetNewPhrase");
            var phraseService = new PhraseService();
            var newPhrase = phraseService.DrawPhrase();
            Debug.Log("newPhrase == " + newPhrase);
            CurrentPhrase = newPhrase;
        }

        private void SetNextDrawer()
        {
            Debug.Log("Method ServerSide.SetNextDrawer");
            CurrentDrawer = _drawers[0];
            if (_drawers.Remove(CurrentDrawer))
                if (!_drawers.Contains(CurrentDrawer))
                    _drawers.Add(CurrentDrawer);
            Chat.AddMessageToSend(String.Format(Chat.NextDrawerMessage, CurrentDrawer.Login), Chat.System);
        }

        public void TimeIsUp(string login)
        {
            Chat.AddMessageToSend(String.Format(Chat.TimeIsUp, login), Chat.System);
            StartNewRound();
        }
    }
}