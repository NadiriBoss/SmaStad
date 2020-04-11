using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmåStad.Models;
using SmåStad.Infrastructure;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmåStad.Controllers {
  public class HomeController : Controller {

   private IRepository repository;

    public HomeController(IRepository repo) {
      repository = repo;
    }
    // GET: /<controller>/
    public IActionResult Index() {
      var myErrand = HttpContext.Session.GetJson<Errand>("NewErrand");
      if(myErrand == null) {
        return View();
      } else {
        return View(myErrand);
      }
    }
    public ViewResult FAQ() {
      ViewBag.Title = "SmåStad - FAQ";
      return View();
    }
    public ViewResult Tjanster() {
      ViewBag.Title = "SmåStad - Tjänster";
      return View();
    }
    public ViewResult Kontakt() {
      ViewBag.Title = "SmåStad - Kontakt";
      return View();
    }
    [HttpPost]
    public ViewResult Validering(Errand errand) {

      if (ModelState.IsValid) {
        HttpContext.Session.SetJson("NewErrand", errand);
        return View(errand);
      } else {
        return View();
      }
    }
    public ViewResult Tack(Errand errand) {
      ViewBag.Title = "SmåStad - Tack";
      var myErrand = HttpContext.Session.GetJson<Errand>("NewErrand");
      repository.SaveErrand(myErrand);

      HttpContext.Session.Remove("NewErrand");
      return View(myErrand);
    }
    public ViewResult LogIn() {
      ViewBag.Title = "SmåStad - Login";
      return View();
    }

    public ViewResult Logut() {
      
      return View();
    }
  }
}