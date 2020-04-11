using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmåStad.Models {
  public class Sample {
    public int SampleId { get; set; }
    public String SampleName { get; set; }
    public Errand ErrandId { get; set; }
  }
}
