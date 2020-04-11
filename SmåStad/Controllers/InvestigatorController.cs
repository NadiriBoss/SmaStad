using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SmåStad.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmåStad.Controllers {
  [Authorize(Roles = "Investigator")]
  public class InvestigatorController : Controller {
    private IHostingEnvironment environment;
    private IRepository repository;
    public InvestigatorController(IRepository repo, IHostingEnvironment env) {
      repository = repo;
      environment = env;
    }

    // GET: /<controller>/
    public ViewResult startInvestigator() {
      ViewBag.Start = "SmåStad - Samorndare Start";
      return View(repository);
    }
    public ViewResult crimeInvestigator(string id) {
      ViewBag.ID = id;
      ViewBag.StatusList = repository.ErrandStatuses;
      TempData["id"] = id;
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> Help(Errand errand, IFormFile documents, IFormFile pictures) {

      errand.RefNumber = TempData["id"].ToString();

      if (documents != null && documents.Length > 0) {
        var tempPath = Path.GetTempFileName();
        using (var stream = new FileStream(tempPath, FileMode.Create)) {
          await documents.CopyToAsync(stream);
        }
        var path = Path.Combine(environment.WebRootPath, "Samples", documents.FileName);
        System.IO.File.Move(tempPath, path);
        repository.UploadSample(errand, documents.FileName);
      }

      if (pictures != null && pictures.Length > 0) {
        var tempPath2 = Path.GetTempFileName();
        using (var stream2 = new FileStream(tempPath2, FileMode.Create)) {
          await pictures.CopyToAsync(stream2);
        }
        var path2 = Path.Combine(environment.WebRootPath, "Pictures", pictures.FileName);
        System.IO.File.Move(tempPath2, path2);
        repository.UploadPicture(errand, pictures.FileName);
      }
      if (errand.StatusId != "Välj status") {
        repository.UpdateErrandInfo(errand);
      }

      return RedirectToAction("startInvestigator");
    }
  }
}

