using Cwiczenia9.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia9.Data;

public class AppDbContext : DbContext
{
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var patient = new Patient
        {
            IdPatient = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Birthdate = new DateOnly(1990, 1, 1),
        };

        var doctor = new Doctor
        {
            IdDoctor = 1,
            FirstName = "AAA",
            LastName = "BBB",
            Email = "aaabbb@gmail.com",
        };

        var medicament = new Medicament
        {
            IdMedicament = 1,
            Name = "AAA",
            Description = "Some description",
            Type = "Some type",
        };

        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date = new DateOnly(2012, 1, 1),
            DueDate = new DateOnly(2012, 1, 1),
            IdPatient = 1,
            IdDoctor = 1
        };
        
        var medicamentPrescription = new PrescriptionMedicament
        {
            IdPrescription = 1,
            IdMedicament = 1,
            Dose = 3,
            Details = "Some details"
        };

        modelBuilder.Entity<Patient>().HasData(patient);
        modelBuilder.Entity<Doctor>().HasData(doctor);
        modelBuilder.Entity<Medicament>().HasData(medicament);
        modelBuilder.Entity<Prescription>().HasData(prescription);
        modelBuilder.Entity<PrescriptionMedicament>().HasData(medicamentPrescription);
    }
}