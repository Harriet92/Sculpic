using System.Collections.Generic;
using Assets.Sources.DatabaseClient.Models;
using Assets.Sources.DatabaseClient.Services;
using Assets.Sources.Enums;
using UnityEngine;

namespace Assets.Sources.Scripts.RankingScreen
{
    public class RankingMenu : MenuBase
    {
        public GameObject RankingPanel;
        public GameObject PlayerRankingElement;
        private readonly UserService userService = new UserService();
        private const int DisplayedItemsCount = 20;
        void Start()
        {
            UpdateRankingList();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(SceneName.RoomChoiceScreen.ToString()); }
        }

        public void BackButtonClick()
        {
            Application.LoadLevel(SceneName.RoomChoiceScreen.ToString());
        }

        private void UpdateRankingList()
        {
            var users = userService.GetTopRanking(DisplayedItemsCount.ToString());
            ClearRankingPanel();
            foreach (var user in users)
                AddRankingElement(user);
        }

        private void ClearRankingPanel()
        {
            var children = new List<GameObject>();
            foreach (Transform child in RankingPanel.transform)
                children.Add(child.gameObject);
            children.ForEach(Destroy);
        }

        private void AddRankingElement(User user)
        {
            var button = (GameObject)Instantiate(PlayerRankingElement);
            var rankingScript = button.GetComponentInChildren<RankingElement>();
            rankingScript.SetPlayerData(user, RankingPanel);
        }
    }
}
