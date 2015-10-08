using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MvcMusicStore.Models
{
    public class UsersRepository: IDisposable
    {
        MusicStoreEntities db = new MusicStoreEntities();

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.AsEnumerable();
        }

        public User GetUser(string userName)
        {
            userName= userName.Trim().ToLower();
            return db.Users.SingleOrDefault(matching => matching.UserName.ToLower().Equals(userName));
        }

        public User GetUser(string userName, string password)
        {
            userName = userName.Trim().ToLower();
            return db.Users.SingleOrDefault(matching => matching.UserName.ToLower().Equals(userName)
                && matching.password.Equals(password));
        }

        public void RegisterUser(User newUser)
        {
            db.Users.Add(newUser);
            db.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            db.Entry(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}