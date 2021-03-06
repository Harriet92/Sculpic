﻿using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Assets.Sources.Common;
using Assets.Sources.DatabaseClient.Models;
using Assets.Sources.DatabaseClient.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Scripts.RoomHosterScreen
{
    public class RoomHosterScreenMenu : MonoBehaviour
    {
        private string gameName = "Test Game";
        private string hosterSettingsPath = Directory.GetCurrentDirectory() + "/RoomSettings.xml";
        public Text GameNameTextField;
        public Text PlayersNumberTextField;

        void Start()
        {
            MasterServerConnectionManager.SetMasterServerLocation();
            MasterServerConnectionManager.RefreshHostList();
            var settings = DeserializeRoomSettings();
            if (settings != null)
                StartCoroutine(InitServerAndHostRoom(MasterServerConnectionManager.RoomPort, settings.UsersLimit, settings.GameName, settings.Password));
            else
                StartCoroutine(InitServerAndHostRoom(MasterServerConnectionManager.RoomPort, MasterServerConnectionManager.ConnectionsNo, gameName));
        }

        private RoomSettings DeserializeRoomSettings()
        {
            XmlSerializer reader = new XmlSerializer(typeof(RoomSettings));
            StreamReader file = new StreamReader(@hosterSettingsPath);
            return (RoomSettings)reader.Deserialize(file);
        }

        private IEnumerator InitServerAndHostRoom(int roomPort, int connectionsNo, string gameName, string password = RoomService.noPasswordMessage)
        {
            while (!MasterServerConnectionManager.HostsRefreshed)
                yield return null;

            Network.incomingPassword = password == RoomService.noPasswordMessage ? "" : password;
            Network.InitializeServer(connectionsNo, roomPort + MasterServerConnectionManager.HostList.Length);
            MasterServerConnectionManager.RegisterHost(gameName);
            GameNameTextField.text = gameName;
        }

        void Update()
        {
            PlayersNumberTextField.text = Network.connections.Count() + "/" + Network.maxConnections;
        }
    }
}
