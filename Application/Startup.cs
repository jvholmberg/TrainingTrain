using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsvcAuth;
using MsvcUser;

namespace Application
{
	public class Startup
	{
		public IConfiguration _Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			_Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // Configure Auth
            var msvcAuthConnectionString = _Configuration.GetConnectionString("MsvcAuthDatabaseConnection");
            services.SetupMsvcAuth(msvcAuthConnectionString);

            // Configure User
            var msvcUserConnectionString = _Configuration.GetConnectionString("MsvcUserDatabaseConnection");
            services.SetupMsvcUser(msvcUserConnectionString);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseCors(options => options
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);

			app.SetupMsvcAuth();

			app.UseMvc();
		}
	}
}
