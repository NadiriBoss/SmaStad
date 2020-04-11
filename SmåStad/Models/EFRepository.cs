using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmåStad.Models {
  public class EFRepository : IRepository {
    private AppDbContext context;
    private IHttpContextAccessor contextAcc;
    public EFRepository(AppDbContext ctx, IHttpContextAccessor cont) {
      contextAcc = cont;
      context = ctx;
    }
    public IQueryable<Department> Departments => context.Departments;
    public IQueryable<Employee> Employees => context.Employees;
    public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);
    public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;
    public IQueryable<Picture> Pictures => context.Pictures;
    public IQueryable<Sample> Samples => context.Samples;
    public IQueryable<Sequence> Sequences => context.Sequences;
    public IQueryable<NewErrand> CoordinatorTable => GetCoordinatorTable();
    public IQueryable<NewErrand> InvestigatorTable => GetInvestigatorTable();
    public IQueryable<NewErrand> ManagerTable => GetManagerTable();
    public IQueryable<Employee> ManagerEmployees => GetManagerEmployees();

    public string SaveErrand(Errand errand) {

      if (errand.ErrandID == 0) {

        var cValue = Sequences.Where(cv => cv.Id == 1).First();
        int refNumber = cValue.CurrentValue;

        errand.RefNumber = "2018-45-" + refNumber;
        errand.StatusId = "S_A";

        UpdateRefNumber(cValue);
        context.Errands.Add(errand);

      }
      context.SaveChanges();
      return errand.RefNumber;
    }

    public void UpdateErrandDepartment(Errand errand) {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.RefNumber == errand.RefNumber);
      dbEntry.DepartmentId = errand.DepartmentId;
      context.SaveChanges();
    }

    public void UpdateRefNumber(Sequence sequence) {
      Sequence dbEntry = context.Sequences.FirstOrDefault(s => s.Id == 1);
      dbEntry.CurrentValue = sequence.CurrentValue;
      sequence.CurrentValue++;
      context.SaveChanges();
    }

    public void UpdateErrandEmployee(Errand errand) {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.RefNumber == errand.RefNumber);
      if (errand.NoAction == true) {
        dbEntry.StatusId = "S_B";
      }
      else {
        dbEntry.EmployeeId = errand.EmployeeId;
        dbEntry.StatusId = "S_A";
      }
      context.SaveChanges();
    }

    public void UpdateErrandInfo(Errand errand) {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.RefNumber == errand.RefNumber);
      dbEntry.InvestigatorAction += errand.InvestigatorAction;
      dbEntry.InvestigatorInfo += errand.InvestigatorInfo;
      dbEntry.StatusId = errand.StatusId;
      context.SaveChanges();
    }

    public void UploadSample(Errand errand, string sampleName) {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.RefNumber == errand.RefNumber);
      context.Samples.Add(new Sample { ErrandId = dbEntry, SampleName = sampleName });
      context.SaveChanges();
    }

    public void UploadPicture(Errand errand, string pictureName) {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.RefNumber == errand.RefNumber);
      context.Pictures.Add(new Picture { ErrandId = dbEntry, PictureName = pictureName });
      context.SaveChanges();
    }

    public Task<Errand> GetErrand(string id) {
      return Task.Run(() => {
        var errandDetail = Errands.Where(ed => ed.RefNumber == id).First();
        return errandDetail;
      });
    }

    public IQueryable<NewErrand> GetCoordinatorTable() {
      var errandList =
        from err in Errands
        join stat in ErrandStatuses on err.StatusId equals stat.StatusId
        join dep in Departments on err.DepartmentId equals dep.DepartmentId
        into departmentErrand
        from deptE in departmentErrand.DefaultIfEmpty()
        join em in Employees on err.EmployeeId equals em.EmployeeId
        into employeeErrand
        from empE in employeeErrand.DefaultIfEmpty()
        orderby err.RefNumber descending
        select new NewErrand {
          DateOfObservation = err.DateOfObservation,
          ErrandId = err.ErrandID,
          RefNumber = err.RefNumber,
          TypeOfCrime = err.TypeOfCrime,
          StatusName = stat.StatusName,
          DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
          EmployeeName = (empE.EmployeeName == null ? "ej tillsat" : empE.EmployeeName)
        };
      return errandList;
    }

    public IQueryable<NewErrand> GetInvestigatorTable() {

      var userName = contextAcc.HttpContext.User.Identity.Name;

      var errandList =
        from err in Errands
        join stat in ErrandStatuses on err.StatusId equals stat.StatusId
        join dep in Departments on err.DepartmentId equals dep.DepartmentId
        into departmentErrand
        from deptE in departmentErrand.DefaultIfEmpty()
        join em in Employees on err.EmployeeId equals em.EmployeeId
        into employeeErrand
        from empE in employeeErrand.DefaultIfEmpty()
        where err.EmployeeId == userName
        orderby err.RefNumber descending
        select new NewErrand {
          DateOfObservation = err.DateOfObservation,
          ErrandId = err.ErrandID,
          RefNumber = err.RefNumber,
          TypeOfCrime = err.TypeOfCrime,
          StatusName = stat.StatusName,
          DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
          EmployeeName = (empE.EmployeeName == null ? "ej tillsat" : empE.EmployeeName)
        };
      return errandList;
    }

    public IQueryable<NewErrand> GetManagerTable() {

      var userName = contextAcc.HttpContext.User.Identity.Name;

      Employee dbEntry = context.Employees.FirstOrDefault(e => e.EmployeeId == userName);

      var errandList =
        from err in Errands
        join stat in ErrandStatuses on err.StatusId equals stat.StatusId
        join dep in Departments on err.DepartmentId equals dep.DepartmentId
        into departmentErrand
        from deptE in departmentErrand.DefaultIfEmpty()
        join em in Employees on err.EmployeeId equals em.EmployeeId
        into employeeErrand
        from empE in employeeErrand.DefaultIfEmpty()
        where dbEntry.DepartmentId == err.DepartmentId
        orderby err.RefNumber descending
        select new NewErrand {
          DateOfObservation = err.DateOfObservation,
          ErrandId = err.ErrandID,
          RefNumber = err.RefNumber,
          TypeOfCrime = err.TypeOfCrime,
          StatusName = stat.StatusName,
          DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
          EmployeeName = (empE.EmployeeName == null ? "ej tillsat" : empE.EmployeeName)
        };
      return errandList;
    }

    public IQueryable<Employee> GetManagerEmployees() {

      var userName = contextAcc.HttpContext.User.Identity.Name;
      Employee dbEntry = context.Employees.FirstOrDefault(e => e.EmployeeId == userName);

      var employeeList =
        from e in Employees
        join d in Departments on e.DepartmentId equals d.DepartmentId
        into employeeDepartment
        from empDep in employeeDepartment.DefaultIfEmpty()
        where dbEntry.DepartmentId == e.DepartmentId
        orderby e.EmployeeName descending
        select e;

      return employeeList;
    }
  }
}