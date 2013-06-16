using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NGChat.DataAccess.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using NGChat.DataAccess.Migrations;

namespace NGChat.DataAccess
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<HubConnection> HubConnections { get; set; }

        public ChatContext()
            : base("Name=ChatContext")
        {
            Database.SetInitializer<ChatContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChatContext, Configuration>());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>().HasMany(j => j.HubConnections).WithRequired();
        }
    }
}