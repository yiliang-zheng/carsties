using Duende.IdentityServer.Models;

namespace Web
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("auctionSvc", "Auction Web Service")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "postman.client",
                    ClientName = "Postman Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    //ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    RequireClientSecret = false,
                    AllowedScopes = { "openid", "profile", "auctionSvc" },
                    RedirectUris = new List<string>
                    {
                        "https://oauth.pstmn.io/v1/browser-callback"
                    }
                }
            };
    }
}
