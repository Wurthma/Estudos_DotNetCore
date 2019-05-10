using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExternalProvidersIdentity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ExternalProvidersIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];

                //Novos aplicativos não conseguem pegar mais doque as configurações básicas do usuário devidos as questões com problema com dados de usuário enfrentados pelo facebook
                //É possível pegar mais informações após a aplicação ser verificada pela equipe do facebook. Exemplos:

                //scope para buscar data de nascimento do usuário
                //facebookOptions.Scope.Add("user_birthday"); 

                //Claim locale para identificar o idioma que o usuário utiliza no facebook
                //facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");

                //Opção para salvar os tokens no banco de dados da aplicação após autenticação do usuário usando o facebook, os tokens são gerados ao buscar as claims do usuário no facebook
                //Para funcionar é necessário alterar o Controller do Identity (ExternalLogin.cshtml.cs)
                facebookOptions.SaveTokens = true;
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                googleOptions.SaveTokens = true;
            })
            .AddTwitter(twitterOptions => 
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                twitterOptions.SaveTokens = true;
                //Para twitter é necessário específicar os dados a serem acessados:
                twitterOptions.RetrieveUserDetails = true;
                twitterOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
            })
            .AddMicrosoftAccount(maOptions => 
            {
                maOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                maOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                maOptions.SaveTokens = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
