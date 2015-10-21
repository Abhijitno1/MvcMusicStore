using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;

namespace MvcMusicStore.Infrastructure
{
    public class UserIdentity: IIdentity, IPrincipal
    {
        private FormsAuthenticationTicket ticket;

        public UserIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
        }

        public string AuthenticationType
        {
            get { return "User"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return ticket.Name; }
        }

        public string UserId
        {
            get { return ticket.UserData; }
        }

        public IIdentity Identity
        {
            get { return this; }
        }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }
    }
}