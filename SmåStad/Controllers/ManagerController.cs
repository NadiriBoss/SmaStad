using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmåStad.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmåStad.Controllers {
  [Authorize(Roles = "Manager")]
  public class ManagerController : Controller {
    private IRepository repository;
    public ManagerController(IRepository repo) {
      repository = repo;
    }
    // GET: /<controller>/
    public ViewResult startManager() {
      ViewBag.Title = "SmåStad  Chef start";
      return View(repository);
    }
    public ViewResult crimeManager(string id) {
      ViewBag.ID = id;
      TempData["id"] = id;
      ViewBag.ListOfEmployees = repository.ManagerEmployees;
      return View();
    }
    [HttpPost]
    public IActionResult Help(Errand errand) {
      errand.RefNumber = TempData["id"].ToString();
      if (errand.EmployeeId == "Välj handläggare" && errand.NoAction == false ) {
        return RedirectToAction("startManager");
      } else {
        repository.UpdateErrandEmployee(errand);
        return RedirectToAction("startManager");
      }
    }
  }
}
