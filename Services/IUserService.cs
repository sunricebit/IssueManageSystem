﻿namespace IMS.Services
{
         public interface IUserService
        {
            User GetUser(int id);
            IEnumerable<User> GetAllUsers();
            User AddUser(User user);
            User UpdateUser(User user);
            void DeleteUser(int id);
            IEnumerable<User> SearchUsers(string searchTerm);
            IEnumerable<User> FilterUsers(Dictionary<string, object> filters);
        IEnumerable<Setting> GetRole();
        }
}