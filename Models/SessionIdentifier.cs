using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class SessionIdentifier
    {
        private const string CartSessionKey = "CartId";

        // We're using HttpContextBase to allow access to cookies.
        public static string GetSessionID()
        {
            // We're using HttpContextBase to allow access to cookies.
            HttpContext context = HttpContext.Current;
            if (context.Session[CartSessionKey] == null)
            {
                ResetSessionID();
            }

            return context.Session[CartSessionKey].ToString();
        }

        public static void ResetSessionID()
        {
            HttpContext context = HttpContext.Current;
            if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
            {
                context.Session[CartSessionKey] = context.User.Identity.Name;
            }
            else
            {
                // Generate a new random GUID using System.Guid class
                Guid tempCartId = Guid.NewGuid();

                // Send tempCartId back to client as a cookie
                context.Session[CartSessionKey] = tempCartId.ToString();
            }
        }

    }
}