﻿using AddIdentity_ProjetoExistente.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddIdentity_ProjetoExistente.Data
{
    public class ApplicationDataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) 
            : base(options)
        {

        }
    }
}
