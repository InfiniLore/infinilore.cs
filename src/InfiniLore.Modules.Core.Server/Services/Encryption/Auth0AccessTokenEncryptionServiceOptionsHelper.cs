// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Core.Server.Encryption;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class WebApplicationBuilderExtensions {
    public static void AddAuth0AccessTokenEncryptionOptions(this WebApplicationBuilder builder) {
        // Retrieve the configuration section
        IConfigurationSection section = builder.Configuration.GetSection("Auth0AccessTokenEncryption");

        // Check if the section exists
        if (section.Exists()) {
            // Bind the configuration section if it exists
            builder.Services.Configure<Auth0AccessTokenEncryptionServiceOptions>(section);
            return;
        }

        // Register with default values if the section is missing
        builder.Services.Configure<Auth0AccessTokenEncryptionServiceOptions>(options => {
            options.SecretKey = Auth0AccessTokenEncryptionServiceOptions.DefaultSecretKey;
        });
    }
}
