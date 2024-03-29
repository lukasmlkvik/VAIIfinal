﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VAII.Models;

namespace VAII.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FoundModel> Founds { get; set; }
        public DbSet<NewsModel> News { get; set; }
        public DbSet<ServerInfoModel> ServerInfo { get; set; }
        public DbSet<UserFoundModel> UsersFounds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserFoundModel>().HasKey(uf => new {uf.Id, uf.symbol });
        }
    }
}
