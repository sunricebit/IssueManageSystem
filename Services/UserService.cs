using Microsoft.AspNetCore.Identity;
using System;

namespace IMS.Services
{
    public class UserService : IUserService
    {
        private readonly IMSContext _context;

        public UserService(IMSContext context)
        {
            _context = context;
        }
        public IEnumerable<Setting> GetRole()
        {
            return _context.Settings.Where(u => u.Type == "ROLE").ToList();
        }
        public IEnumerable<User> GetTeacher() 
        {
            return _context.Users.Where(u => u.RoleId == 5).ToList();
        }
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public int GetRoleId(string role)
        {
            return _context.Settings.FirstOrDefault(u => u.Value == role).Id;
        }
        public IEnumerable<User> FilterByRole(int roleid)
        {
            return _context.Users.Where( u=> u.RoleId == roleid).ToList();
        }
        public IEnumerable<User> FilterByStatus(bool status) 
        {
            return _context.Users.Where( u=> u.Status == status).ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(x=> x.Id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(x => x.Role).ToList();
       ;
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return _context.Users
                .Where(u => u.Name.Contains(searchTerm)
                    || u.Email.Contains(searchTerm) || u.Phone.Contains(searchTerm));
        }

       public IEnumerable<Post> GetPost(int id) 
        {

            return _context.Posts.Where( p => p.AuthorId == id).ToList();
        }
      
    }
}
