using DownTheRoad.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownTheRoad
{
    public static class Utils
    {
        //WEB API KEY used for firebase Authentication
        public static string WebAPIKey = "AIzaSyA8sUf6JIuX98AwiZNX32zwzodU3kN4MwA";
        //RealtimeDbURL used for firebase Realtime Database Operations
        public static string RealtimeDbURL = "https://down-the-road-4bd87-default-rtdb.firebaseio.com/";
        public static List<string> Roles= new List<string>() { "User", "Worker" };

        public static void InitializeSession(User loggedUser)
        {
            SessionInfo.Username = loggedUser.Username ?? "";
            SessionInfo.Password = loggedUser.Password ?? "";
            SessionInfo.PhoneNo = loggedUser.PhoneNo ?? "";
            SessionInfo.Key = loggedUser.Key ?? "";
            SessionInfo.Role = loggedUser.Role ?? "";
        }
    }
}
