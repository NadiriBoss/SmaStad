using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmåStad.Models {
  public class Errand {

    public int ErrandID { get; set; }
    public String RefNumber { get; set; }

    [Required(ErrorMessage ="Du måste ange platsen borttet skedde på")]
    public String Place { get; set; }

    [Required(ErrorMessage = "Du måste ange typ av brott")]
    public String TypeOfCrime { get; set; }

    [Required(ErrorMessage ="Du måste ange ett datum (dd/mm/åå)")]
    public DateTime DateOfObservation { get; set; }

    public String Observation { get; set; }
    public String InvestigatorInfo { get; set; }
    public String InvestigatorAction { get; set; }

    [Required(ErrorMessage = "Du måste fylla i ditt namn")]
    public String InformerName { get; set; }

    [Required(ErrorMessage = "Du måste anget ditt telefonnummer")]
    [RegularExpression(pattern: @"^07([0-9][ -]*){7}[0-9]$", ErrorMessage = "Formatet för mobilnummer ska vara xxxxxxxx")]
    public String InformerPhone { get; set; }

    public String StatusId { get; set; }
    public String DepartmentId { get; set; }
    public String EmployeeId { get; set; }

    [NotMapped]
    public bool NoAction { get; set; }
    public ICollection<Sample> Samples { get; set; }
    public ICollection<Picture> Pictures { get; set; }
  }
}
