using apiPeluqueria.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace apiPeluqueria.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Invitado> invitados { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Cita> citas { get; set; }
    }
}
