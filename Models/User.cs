using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcMusicStore.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }        
        public string UserName { get; set; }
        public string password { get; set; }
        public string UserEmail { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}