using System.ComponentModel.DataAnnotations;
using Cwiczenia9.Models;

namespace Cwiczenia9.DTOs;

public class PrescriptionCreateDto
{   [Required]
    public PatientDetailsGetDto Patient { get; set; }
    [Required]
    public int IdDoctor { get; set; }
    [Required]
    public ICollection<PrescriptionMedicamentGetDto> Medicaments { get; set; }
    [Required]
    public DateOnly Date { get; set; }
    [Required]
    public DateOnly DueDate { get; set; }

}
public class PrescriptionMedicamentGetDto
{
    public int IdPrescription { get; set; }
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    public string Description { get; set; }
}

public class PrescriptionGetDto
{
    public int IdPrescription { get; set; }

    public int IdPatient { get; set; }

    public DateOnly Date { get; set; }

    public DateOnly DueDate { get; set; }

    public int IdDoctor { get; set; }
}

public class PatientDetailsGetDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly Birthdate { get; set; }
}