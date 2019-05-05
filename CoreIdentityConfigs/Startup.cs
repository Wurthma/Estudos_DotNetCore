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
using CoreIdentityConfigs.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreIdentityConfigs
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
            services
                //Configurações identity core
                .AddDefaultIdentity<IdentityUser>(options => {
                    //Lockout
                    options.Lockout.AllowedForNewUsers = true; //Bloquei é permitido para usuários novos da aplicação
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Tempo de bloqueio da conta
                    options.Lockout.MaxFailedAccessAttempts = 5; //Quantidade de tentativas para bloqueio de conta: o lockOutOnFailure deve ser setado para true na controller Login para o funcionamento

                    //Password
                    options.Password.RequireDigit = true; //Pelo menos um número na senha
                    options.Password.RequiredLength = 6; //Mínimo de caracteres da senha
                    options.Password.RequiredUniqueChars = 1; //Quantidade de caracteres diferentes na senha
                    options.Password.RequireLowercase = true; //Requer pelo menos uma letra mínuscula
                    options.Password.RequireUppercase = true; //Requer no mínimo uma letra maíuscula
                    options.Password.RequireNonAlphanumeric = true; //Requer pelo menos um caractere especial na senha

                    //SignIn
                    options.SignIn.RequireConfirmedEmail = false; //Requer um e-mail confirmado na conta para login
                    options.SignIn.RequireConfirmedPhoneNumber = false; //Requer um número de celular confirmado para fazer o login

                    //Token
                    //options.Tokens.AuthenticatorTokenProvider //obtem ou define o token provider utilizado para o two factor authentication
                    //options.Tokens.ChangeEmailTokenProvider //Token provider utilizado no e-mail de confirmação do processo de alteração de e-mail
                    //options.Tokens.ChangePhoneNumberTokenProvider //Token provider utilizado alteração de número de celular do usuário
                    //options.Tokens.EmailConfirmationTokenProvider //Token Utilizando na confirmação de e-mail do usuário
                    //options.Tokens.PasswordResetTokenProvider //Gerar tokens para utilização na troca de senha dos usuários

                    //User - opções de criação de usuários com Identity
                    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.useroptions.allowedusernamecharacters?view=aspnetcore-2.2
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //opção padrão utiliza todo alfabeto minúsculo, maiúsculo, números, -, ., _,@ e +
                    options.User.RequireUniqueEmail = false; //Requer e-mail único por usuário
                })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Configurações de Cookies Identity Core
            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Account/AccessDenied"; //Informa o caminho para o handler que vai tratar o erro 403 (Forbidden). Valor padrão = /Account/AccessDenied
                //options.ClaimsIssuer = ""; //Definir um Issuer para criar claims. Deve herdar de AuthenticationScamOptions se for alterado
                //options.Cookie.Domain = ""; //Defirnir o domínio ao qual o cookie que será criado pertence
                //options.Cookie.Expiration = ""; //Define ou obtem o tempo de vida do Cookie http (cookie http não é o nosso cookie de autenticação)
                options.Cookie.HttpOnly = true; //Indica se o cookie pode ou não ser acessado pelo client side. Padrão = true.
                options.Cookie.Name = ".AspNeCore.Cookies"; //Nome do cookie. Valor padrão = ".AspNeCore.Cookies"
                //options.Cookie.Path = ""; //Determina o caminho do cookie
                options.Cookie.SameSite = SameSiteMode.Lax; //São Cookies que não devem ser anexados as solicitações Cross Site. Valor padrão = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; //Configurações de politica de segurança do cookie
                //options.CookieManager //Define o componente que será utilizado para obter os cookies nos requests ou definir os cookies dos responses. Necessário fazer apontamento de uma classe que faça a implementação do ICookieManager
                //options.DataProtectionProvider //Necessita de classe que implemente IDataProtectProvider. Se definido será o provider utilizado pelo cookie authentication handler para proteção dos dados
                //options.Events //É o handler responsável por chamar os métodos do provider que dará o controle da aplicação onde ocorre o processamento
                //options.EventsType //Responsável por obter a instância dos eventos. Deve herdar de AuthenticationSchemeOptions
                options.ExpireTimeSpan = TimeSpan.FromDays(14); //Controla quanto tempo o ticket de autenticação armazenado no cookie permanecerá valido a partir do momento que foi criado o valo padrão é de 14 dias
                options.LoginPath = "/Account/Login"; //Define o caminho onde o login deve ser realizado. Quando um usuário estiver acessando determinado controller que precisa de autenticação ele será direcionado para esse caminho. Padrão = "/Account/Login"
                options.LogoutPath = "/Account/Logout"; //Define o caminho do logout.
                options.ReturnUrlParameter = "ReturnUrl"; //Nome do parametro que recebá a URL que o usuário deverá ser redirecionado após realizar o login. Padrão = "ReturnUrl".
                //options.SessionStore = // Define um container opcional que armazenará a identidade do usuário durante as requisicões
                options.SlidingExpiration = true; //Quando habilitado um novo cookie será criado com uma nova hora de expiração quando o cookie atual tiver ultrapassado a metade do tempo de expiração. Valor padrão = true.
                //options.TicketDataFormat = //Utilizado para proteger e desproteger a identidade de outras propriedades armazenadas no cookie
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
