using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SmåStad.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SmåStad {
  public class Startup {
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration config) => Configuration = config;

    public void ConfigureServices(IServiceCollection services) {
      services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
      services.AddTransient<IRepository, EFRepository>();
      services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
      services.AddMvc();
      services.AddSession();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseStatusCodePages();
      app.UseStaticFiles();
      app.UseSession();
      app.UseAuthentication();
      app.UseMvcWithDefaultRoute();
    }
  }
}
