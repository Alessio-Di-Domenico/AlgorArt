using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlgorArt.Models;

namespace AlgorArt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AlgorArt.Models.RequestFunds> RequestFunds { get; set; }
        public DbSet<AlgorArt.Models.Funders> Funders { get; set; }
    }
}
