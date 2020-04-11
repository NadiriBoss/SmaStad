using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmåStad.Models;

namespace SmåStad.Components {
  public class ErrandDetail : ViewComponent {
    private IRepository repository;
    public ErrandDetail(IRepository repo) {
      repository = repo;
    }
    public async Task<IViewComponentResult> InvokeAsync(string id) {
      var objectErrand = await repository.GetErrand(id);
      return View("ErrandDetail", objectErrand);
    }
  }
}
