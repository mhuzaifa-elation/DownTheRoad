using DownTheRoad.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownTheRoad
{
    //Firebase Services is custom class for using firebase operations
    public static class FirebaseServices
    {
        #region Class Variables
        static FirebaseClient fc = new FirebaseClient(Utils.RealtimeDbURL);
        #endregion
        #region Methods
        public static async Task<bool> Signup(User user) //Login Method for firebase Authentication
        {
            List<User> AllUsers =await GetAllUsers();
            if (AllUsers.Exists(x=>x.Username== user.Username))
            {
                throw new Exception("Username already exits");
            }
            else
            {
                await fc
                    .Child("Users")
                    .PostAsync(user);
            }
            return true;
        }
        public static async  Task<User> Login(string Username, string Password) //Login Method for firebase Authentication
        {
            List<User> AllUsers = await GetAllUsers();
            var UserFound = AllUsers.Find(x => x.Username == Username);
            if (UserFound==null)
            {
                throw new Exception("Username not found.");

            }
            if (UserFound.Password != Password)
            {
                throw new Exception("Incorret Password.");

            }
            return UserFound;
        }
        private static async Task<List<User>> GetAllUsers() //Method for getting All Exercises in firebase Realtime DB
        {
            FirebaseClient fc = new FirebaseClient(Utils.RealtimeDbURL);
            var AllExercises = (await fc
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  Key = item.Key,
                  Username=item.Object.Username,
                  Password=item.Object.Password,
                  PhoneNo=item.Object.PhoneNo,
                  Role = item.Object.Role

              }).ToList();
            return AllExercises;
        }
        public static async Task<List<WorkService>> GetAllServices() //Method for getting All Exercises in firebase Realtime DB
        {
            FirebaseClient fc = new FirebaseClient(Utils.RealtimeDbURL);
            var AllWorkServices = (await fc
              .Child("Services")
              .OnceAsync<WorkService>()).Select(item => new WorkService
              {
                  Key = item.Key,
                  Title = item.Object.Title,
                  Description = item.Object.Description,
                  Price = item.Object.Price,
                  ServiceBy = item.Object.ServiceBy,
                  RequestedBy = item.Object.RequestedBy,
                  AssignedTo = item.Object.AssignedTo
              }).ToList();
            return AllWorkServices;
        }
        public static async Task UpdateExercise(string Key, WorkService workService) //Method for Updating existing Exercise in firebase Realtime DB
        {
            await fc
            .Child("Services").Child(Key)
            .PutAsync(workService);
        }
        public static async Task AddExercise(WorkService workService) //Method for adding new Exercise in firebase Realtime DB
        {
            await fc
                 .Child("Services")
                 .PostAsync(workService);
        }
        public static async Task DeleteExercise(string Key) //Method for Deleting existing Exercise in firebase Realtime DB
        {
            await fc
                 .Child("Services").Child(Key).DeleteAsync();
        }
        #endregion
    }
}
