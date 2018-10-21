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

            // Adding a changing file providers here changes were MVC looks for views/pages. If you need
            // to get the file provider(s) from DI you can resolve IRazorViewEngineFileProviderAccessor from
            // DI
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // You'll need to decide if you want to include files in the application's directory
                // or not.
                // options.FileProviders.Clear();

                // File Providers throw if the directory doesn't exist. 
                if (Directory.Exists(cmsOptions.FilePath))
                {
                    options.FileProviders.Add(new PhysicalFileProvider(cmsOptions.FilePath));
                }
            });

            services.Configure<CookiePolicyOptions>( options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            } );


            services.AddMvc()
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
                    .AddRazorPagesOptions( o => { o.AllowAreas = true; } );
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