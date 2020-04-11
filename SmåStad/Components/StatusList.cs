using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace SmåStad.Components {
  public class StatusList : ViewComponent {
    public StatusList() { }

    public IViewComponentResult Invoke() {

      return View("StatusList");
    }
  }
}
