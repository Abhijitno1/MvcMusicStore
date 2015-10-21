using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcMusicStore.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }        
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        
    }
}