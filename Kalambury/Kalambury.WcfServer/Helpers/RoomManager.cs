using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Helpers
{
    public static class RoomManager
    {
        private static Dictionary<int, Process> RoomHosts;
        private static string sculpicHosterLocation = "C:\\Users\\deemisoftware\\Desktop\\WcfServer\\sculpicHoster.exe";
        private static string hostersWorkingDirectoriesRootPath = "./";
        static RoomManager()
        {
            RoomHosts = new Dictionary<int, Process>();
        }

        public static void CreateNewRoom(int userId, RoomSettings settings)
        {
            string workingDirectory = CreateRoomHosterWorkingDirectory();
            SaveRoomSettings(settings, workingDirectory);
            RoomHosts.Add(RoomHosts.Count, CreateNewHoster(workingDirectory));//TODO: Find a better way of identifying Rooms (UserId?)
        }

        private static Process CreateNewHoster(string workingDirectory)
        {
            var hoster = new Process();
            hoster.StartInfo.FileName = sculpicHosterLocation;
            hoster.StartInfo.WorkingDirectory = workingDirectory;
            hoster.Start();
            return hoster;
        }
        private static string CreateRoomHosterWorkingDirectory()
        {
            string directoryName = GetHosterFolderName();
            string result = Directory.CreateDirectory(hostersWorkingDirectoriesRootPath + directoryName).FullName;
            //Directory.CreateDirectory(result + "/sculpicHoster_Data");
            return result;
        }
        private static string GetHosterFolderName()
        {
            return DateTime.Now.ToFileTimeUtc().ToString();
        }

        private static void SaveRoomSettings(RoomSettings roomSettings, string directory)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(RoomSettings));

            var path = directory + "//RoomSettings.xml";
            FileStream file = File.Create(path);
            writer.Serialize(file, roomSettings);
            file.Close();
        }
    }
}
