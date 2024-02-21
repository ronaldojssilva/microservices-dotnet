using Duende.IdentityServer.Models;

namespace geekShopping.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        //perfis de usuarios
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        //Identity resource
        //Informações relacionadas à identidade do usuário. Ex: Nome, e-mail etc
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        //Api Scope. Identificadores ou rescursos que um client pode acessar. Ex.: GeekShopping.Web
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> 
            { 
                new ApiScope("geek_shopping", "GeekShopping Server"),
                new ApiScope(name:"read", "Read data."),
                new ApiScope(name:"write", "Write data."),
                new ApiScope(name:"delete", "Delete data."),

            };

        //Clients
        public static IEnumerable<Client> Clients =>
            new List<Client> 
            { 
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = {new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read", "write", "profile"},
                }
            };
    }
}
