using System;
using Gabinet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gabinet.Data
{
    public class GabinetDbContext : IdentityDbContext<IdentityModel>

    {
        public GabinetDbContext(DbContextOptions<GabinetDbContext> options) :base(options)
        {
        }

        public DbSet<PatientModel> GetPatients { get; set; } //
        public DbSet<UserModel> GetUsers { get; set; }
        public DbSet<AppointmentModel> GetAppointment { get; set; }
        public DbSet<PlaceModel> GetPlaces { get; set; }//
        

    }
}
