using System;
using System.Collections.Generic;
using System.Text;

namespace DownTheRoad.Model
{
    public static class SessionInfo //Session Info Static Class to store logged session 
    {
        public static string Key { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string PhoneNo { get; set; }
        public static string Role { get; set; }
    }
    public class User //User Model to save User Details
    {
        public string Key { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }
    }
    public class WorkService //Work Service Model for Services Data
    {
        public string Key { get; set; }
        public string ServiceBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public string Location { get; set; }
        public bool Completed { get; set; }
    }
    public class WorkerProfile //Worker Profile Model to Save the profile
    {
        public string Key { get; internal set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImageText { get; set; }
    }
}
