using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NGChat.DataAccess.Models
{
    public class HubConnection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ConnectionId { get; set; }
    }
}