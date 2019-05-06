# Estudos_DotNetCore
Projetos:
--------------------------------------------------------
AddIdentity_ProjetoExistente

Estudo realizado no projeto: Adicionar o Identity em um projeto existente.

Detalhes:
Adicionar o identity em projetos já existentes (Core 2.2):

1- install-package Microsoft.AspNetCore.Identity
2- install-package Microsoft.AspNetCore.Identity.EntityFrameworkCore
3- Criar a class (model) ApplicationUser herdando de IdentityUser. 
	-Nessa classe será possível adicionar novas propriedades para os usuários criados na aplicação
4- Criar a class ApplicationDataContext herdando de DbContext
5- Criar construtor da classe ApplicationDataContext recebendo o objeto do tipo DbContextOptions<ApplicationDataContext>
6- Definir connection string no appsettings.json
7- Registrar serviço do DbContext na applicação no arquivo Startup.cs método ConfigureServices
8- Adicionar serviço do identity no Startup.cs
9- Alterar ApplicationDataContext para herdar de IdentityUser.
	-Ficando assim: public class ApplicationDataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
10- Adicionar o identity ao pipe line da aplicação no Startup.cs método Configure: app.UseAuthentication()
11- Add-Migration Initial
12- Criar controllers e Views necessárias para controlar usuários (CRUD identity).

--------------------------------------------------------
CoreIdentityConfigs

Estudo realizado no projeto: Configurações do Identity.

Detalhes:
Estudo das configurações disponíveis no Identity (Core 2.2).

Os detalhes das configurações estão comentadas no arquivo Startup.cs no services de configurações do Identity.
Algumas configurações desse projeto também foram utilizadas no projeto AddIdentity_ProjetoExistente mas não com tanto detalhes.
