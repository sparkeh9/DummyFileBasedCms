using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace DummyFileBasedCms.Web
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services )
        {
            // Contruct an options instance so we can use it to configure things in startup.
            var cmsOptions = new FlatFileCmsGitOptions();
            Configuration.Bind("FlatFileCmsGit", cmsOptions);

            // Bind the configuration in DI so it's accessible to services.
            services.Configure<FlatFileCmsGitOptions>("FlatFileCmsGit", Configuration);

            services.Configure<CookiePolicyOptions>( options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(o =>
                {
                    o.AllowAreas = true;
                    o.RootDirectory = "/"; // Look starting at the root instead of in /Pages
                })
                .AddRazorOptions(options =>
                {
                    // Order matters with options. Putting this after AddMvc will result in
                    // the expected ordering of file providers.
                    foreach (var directory in cmsOptions.Directories)
                    {
                        if (Directory.Exists(directory.FilePath))
                        {
                            options.FileProviders.Add(new PhysicalFileProvider(directory.FilePath));
                        }
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc( routes =>
                        {
                            routes.MapRoute(
                                            name : "default",
                                            template : "{controller=Home}/{action=Index}/{id?}" );
                        } );
        }
    }
}