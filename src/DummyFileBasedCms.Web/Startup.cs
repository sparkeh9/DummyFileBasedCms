using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DummyFileBasedCms.Web
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.Extensions.Options;
    using MvcInfrastructureStuff;

    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.Configure<FlatFileCmsGitOptions>( Configuration.GetSection( "FlatFileCmsGit" ) );

            var flatFileCmsOptions = services.BuildServiceProvider().GetService<IOptions<FlatFileCmsGitOptions>>()?.Value;
            var fileProvider = new PhysicalFileProvider(flatFileCmsOptions.FilePath);

            services.AddSingleton(fileProvider);
            services.AddTransient<FlatFileCmsProviderRazorProjectFileSystem>();
            services.AddSingleton<IPageRouteModelProvider, FlatFileCmsRazorProjectPageRouteModelProvider>();

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