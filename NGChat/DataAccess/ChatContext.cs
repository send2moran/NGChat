using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NGChat.DataAccess.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NGChat.DataAccess
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<HubConnection> HubConnections { get; set; }

        static ChatContext()
        {
            Database.SetInitializer<ChatContext>(null);
        }

        public ChatContext()
            : base("Name=ChatContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ChatContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Chinook Database does not pluralize table names
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().HasMany(j => j.HubConnections).WithRequired();
        }
    }
}