using System;
using System.Collections.Generic;
using System.Text;

namespace DownTheRoad.Model
{
    public static class SessionInfo
    {
        public static string Key { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string PhoneNo { get; set; }
        public static string Role { get; set; }
    }
    public class User
    {
        public string Key { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }
    }
    public class WorkService
    {

        public string Key { get; set; }
        public string ServiceBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }

    }
}
