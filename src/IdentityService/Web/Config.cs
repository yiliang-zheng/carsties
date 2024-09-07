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
            new[]
            {
                new ApiScope("auctionSvc", "Auction Web Service"),
                new ApiScope("bidSvc", "Bidding Web Service")
            };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new[]
            {
                new Client
                {
                    ClientId = "postman.client",
                    ClientName = "Postman Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    //ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "auctionSvc", "bidSvc", "offline_access" },
                    RedirectUris = new List<string>
                    {
                        "https://oauth.pstmn.io/v1/browser-callback"
                    }
                },
                new Client
                {
                    ClientId = "nextApp",
                    ClientName = "NextJs Frontend App",
                    ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = false,
                    RedirectUris =
                    {
                        $"{config["RedirectUris:Clients:NextApp"]}/api/auth/callback/id-server"
                    },
                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "auctionSvc", "bidSvc", "offline_access"},
                    AlwaysIncludeUserClaimsInIdToken = true,
                }
            };
    }
}
