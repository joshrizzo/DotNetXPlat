using DotNetXPlat.Models;
using DotNetXPlat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetXPlat
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (env.IsDevelopment()) {
                services.AddDbContext<MyDB>(opt => opt.UseInMemoryDatabase("MyDB"));
            } else {
                services.AddDbContext<MyDB>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:MyDB"]));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyDB>();

            services.AddAuthentication();

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EditProduct", policy => policy.RequireClaim(Claims.Product.Edit));
            });

            services.AddTransient<IDataSeeder, DataSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IDataSeeder dataSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                dataSeeder.SeedDB();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}