﻿using geekShopping.IdentityServer.Configuration;
using geekShopping.IdentityServer.Model;
using geekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;

namespace geekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<ApplicationUser> _role;

        public DbInitializer(MySQLContext context, UserManager<ApplicationUser> user, RoleManager<ApplicationUser> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult(); 
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult(); 

        }
    }
}
