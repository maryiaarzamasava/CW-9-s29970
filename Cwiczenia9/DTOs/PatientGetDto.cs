using Cwiczenia9.Models;

namespace Cwiczenia9.DTOs;

public class PatientGetDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthdate { get; set; }
    public IEnumerable<PrescriptionDetailsGetDto> Prescriptions { get; set; }
}

public class PrescriptionDetailsGetDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public IEnumerable<MedicamentGetDto> Medicaments { get; set; }
    public DoctorGetDto Doctor { get; set; } 
}

public class MedicamentGetDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int? Dose { get; set; }
    public string Description { get; set; }
}

public class DoctorGetDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
}