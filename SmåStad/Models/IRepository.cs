using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SmåStad.Models {
  public interface IRepository {

    IQueryable<Errand> Errands { get;  }
    IQueryable<ErrandStatus> ErrandStatuses { get; }
    IQueryable<Employee> Employees { get; }
    IQueryable<Department> Departments { get; }
    IQueryable<NewErrand> CoordinatorTable { get; }
    IQueryable<NewErrand> InvestigatorTable { get; }
    IQueryable<NewErrand> ManagerTable { get; }
    IQueryable<Employee> ManagerEmployees { get; }

    Task<Errand> GetErrand(String id);

    //Create and Update Errand
    string SaveErrand(Errand errand);

    void UpdateRefNumber(Sequence sequence);

    void UpdateErrandDepartment(Errand errand);

    void UpdateErrandEmployee(Errand errand);

    void UpdateErrandInfo(Errand errand);

    void UploadSample(Errand errand, string path);

    void UploadPicture(Errand errand, string path2);

  }
}
