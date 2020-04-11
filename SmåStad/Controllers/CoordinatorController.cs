using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmåStad.Models;
using SmåStad.Infrastructure;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmåStad.Controllers {
  [Authorize(Roles = "Coordinator")]
  public class CoordinatorController : Controller {
    private IRepository repository;

    public CoordinatorController(IRepository repo) {
      repository = repo;
    }

    // GET: /<controller>/
    public ViewResult startCoordinator() {
      ViewBag.Title = "SmåStad - Samordnare start";

      return View(repository);
    }
    public ViewResult crimeCoordinator(string id) {
      ViewBag.ID = id;
      TempData["id"] = id;
      ViewBag.ListOfDepartments = repository.Departments;
      return View();
    }
    public ViewResult reportCoordinator() {
      ViewBag.Title = "SmåStad - Handläggare Rapportera";
      var myErrand = HttpContext.Session.GetJson<Errand>("NewErrand");
      if (myErrand == null) {
        return View();
      }
      else {
        return View(myErrand);
      }
    }
    [HttpPost]
    public ViewResult validateCoordinator(Errand errand) {
      ViewBag.Title = "SmåStad - Handläggare validera";

      if (ModelState.IsValid) {
        HttpContext.Session.SetJson("NewErrand", errand);
        return View(errand);
      } else {
        return View();
      }
    }
    public ViewResult thankCoordinator() {
      ViewBag.Title = "SmåStad - Handläggare Tack";

      var myErrand = HttpContext.Session.GetJson<Errand>("NewErrand");
      repository.SaveErrand(myErrand);

      HttpContext.Session.Remove("NewErrand");
      return View();
    }
    [HttpPost]
    public IActionResult Help(Errand errand) {
      errand.RefNumber = TempData["id"].ToString();
      if(errand.DepartmentId == "D00" || errand.DepartmentId == "Välj Avdelning") {
        return RedirectToAction("startCoordinator");
      } else {
        repository.UpdateErrandDepartment(errand);
        return RedirectToAction("startCoordinator");
      }
    }
  }
}
