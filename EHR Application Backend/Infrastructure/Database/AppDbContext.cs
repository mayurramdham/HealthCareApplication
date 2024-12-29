using App.Core.Interface;
using Domain.Entity;
using Domain.Entity.Appointments;
using Domain.Entity.AuthProcess;
using Domain.Entity.Register;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class AppDbContext:DbContext,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }
        public DbSet<User> User { get; set; }
        public DbSet<BloodGroup> BloodGroup { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Otp> Otp { get; set; }
        public DbSet<Specialisation> Specialisation { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<SoapNoteTable> SoapNoteTable { get; set; }
    }
}
