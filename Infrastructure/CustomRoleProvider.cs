using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MvcMusicStore.Models;

namespace MvcMusicStore.Infrastructure
{
    public class CustomRoleProvider: RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return "MVCMusicStore";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {            
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            string[] result = new string[0];
            using (var context = new MusicStoreEntities())
            {
                var roleNames = from role in context.Roles
                                select role.RoleName;
                result = roleNames.ToArray();
            }
            return result;
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] result = new string[0];
            using (var context = new MusicStoreEntities())
            {

                var roleNames = from user in context.Users
                                where user.UserName.Trim().ToLower().Equals(username.Trim().ToLower())
                                from userrole in user.UserRoles
                                select userrole.Role.RoleName;
                result = roleNames.ToArray();
                /*
                 * alternate method
                var user = context.Users.SingleOrDefault(match => match.UserName.Trim().ToLower().Equals(username.Trim().ToLower()));
                if (user.UserRoles != null)
                    result = user.UserRoles.Select(urole => urole.Role).Select(role => role.RoleName).ToArray();
                */ 
            }
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool result = false;
            using (var context = new MusicStoreEntities())
            {
                var user = context.Users.SingleOrDefault(match => match.UserName.Trim().ToLower().Equals(username.Trim().ToLower()));
                if (user.UserRoles != null)
                    result = user.UserRoles.Select(urole => urole.Role).Any(role => role.RoleName.Trim().ToLower().Equals(roleName.Trim().ToLower()));
            }
            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}