using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NGChat.DataAccess.Models
{
    public class User
    {
        public User()
        {
            HubConnections = new List<HubConnection>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public DateTime LastActivity { get; set; }
        public virtual ICollection<HubConnection> HubConnections { get; set; }
    }
}