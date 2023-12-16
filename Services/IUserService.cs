namespace IMS.Services
{
         public interface IUserService
        {
            User GetUser(int id);
            IEnumerable<User> GetAllUsers();
            User AddUser(User user);
            User UpdateUser(User user);
            void DeleteUser(int id);
            IEnumerable<User> SearchUsers(string searchTerm);
            User GetUserByEmail(string email);
            int GetRoleId(string role);
            IEnumerable<Setting> GetRole();
            IEnumerable<User> FilterByRole(int roleid);
            IEnumerable<User> FilterByStatus(bool status);
            public IEnumerable<User> GetTeacher();
            public IEnumerable<Post> GetPost(int id);
            public bool CheckValid(User user);
    }
}
