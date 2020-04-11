using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmåStad.Models {
  public class ErrandStatus {

    [Key]
    public String StatusId { get; set; }
    public String StatusName { get; set; }
  }
}
