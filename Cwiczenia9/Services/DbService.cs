using Cwiczenia9.Data;
using Cwiczenia9.DTOs;
using Cwiczenia9.Exceptions;
using Cwiczenia9.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia9.Services;

public interface IDbService
{
    public Task<PatientGetDto> GetPatientDetailsByIdAsync(int patientId);
    public Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescription);
    public Task<PrescriptionGetDto> GetPrescriptionByIdAsync(int prescriptionId);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<PatientGetDto> GetPatientDetailsByIdAsync(int patientId)
    {
        var result =  await data.Patients.Select(p => new PatientGetDto
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate,
                Prescriptions = p.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDetailsGetDto
                    {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentGetDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Medicament.Description,

                        }).ToList(),
                        Doctor = new DoctorGetDto
                        {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                        }
                    }).ToList()
            })
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);
        return result ?? throw new NotFoundException($"Patient with id {patientId} not found");
    }

    public async Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionData)
    {
        var patient = await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionData.Patient.IdPatient);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescriptionData.Patient.FirstName,
                LastName = prescriptionData.Patient.LastName,
                Birthdate = prescriptionData.Patient.Birthdate,
            };
            await data.Patients.AddAsync(patient);
            await data.SaveChangesAsync();
        }

        var medicamentsId = prescriptionData.Medicaments.Select(m=>m.IdMedicament);
        var existingMedicaments = await data.Medicaments
            .Where(m => medicamentsId.Contains(m.IdMedicament))
            .Select(m=>m.IdMedicament)
            .ToListAsync();
        var missingMedicaments = medicamentsId.Except(existingMedicaments).ToList();
        if (missingMedicaments.Any())
        {
            throw new NotFoundException($"Medicaments with ids: {string.Join(", ", missingMedicaments)} not found");
        }
        
        if (prescriptionData.Medicaments.Count > 10)
        {
            throw new BadRequestException("Prescription cannot contain more than 10 medicaments");
        }

        if (prescriptionData.Medicaments.Count <= 0)
        {
            throw new BadRequestException("Prescription cannot contain less than 1 medicaments");
        }
        
        if (prescriptionData.DueDate < prescriptionData.Date)
        {
            throw new BadRequestException("Due date cannot be earlier than date");
        }

        var doctor = await data.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == prescriptionData.IdDoctor);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with id: {prescriptionData.IdDoctor} not found");
        }

        var prescription = new Prescription
        {
            Date = prescriptionData.Date,
            DueDate = prescriptionData.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor
        };
        data.Prescriptions.Add(prescription);
        await data.SaveChangesAsync();

        var prescriptionId = prescription.IdPrescription;
        foreach (var medicamentId in medicamentsId)
        {
            var prescriptionMedicaments = new PrescriptionMedicament
            {
                IdPrescription = prescriptionId,
                IdMedicament = medicamentId,
                Dose = prescriptionData.Medicaments.FirstOrDefault(m=>m.IdMedicament == medicamentId)?.Dose,
                Details = prescriptionData.Medicaments.FirstOrDefault(m=>m.IdMedicament == medicamentId)?.Description ?? string.Empty
            };
            data.PrescriptionMedicaments.Add(prescriptionMedicaments);
        }
        await data.SaveChangesAsync();
        return new PrescriptionGetDto
        {
            IdPrescription = prescriptionId,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = prescription.Doctor.IdDoctor
        };
    }

    public async Task<PrescriptionGetDto> GetPrescriptionByIdAsync(int prescriptionId)
    {
        var result = await data.Prescriptions.Select(p => new PrescriptionGetDto
        {
            IdPrescription = p.IdPrescription,
            Date = p.Date,
            DueDate = p.DueDate,
            IdPatient = p.IdPatient,
            IdDoctor = p.IdDoctor
        }).FirstOrDefaultAsync(p => p.IdPrescription == prescriptionId);
        return result ?? throw new NotFoundException($"Prescription with id: {prescriptionId} not found");
    }
}