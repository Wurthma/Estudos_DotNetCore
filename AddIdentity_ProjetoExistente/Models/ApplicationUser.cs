using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddIdentity_ProjetoExistente.Models
{
    public class ApplicationUser : IdentityUser<Guid> //O tipo padrão utilizado no id do usuário é string. Com IdentityUser<Guid> o tipo será alterado para Guid
    {
    }
}
