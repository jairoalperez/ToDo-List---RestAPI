using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoList_RestAPI.Helpers;
using System.Text;


namespace ToDoList_RestAPI.Helpers
{
    public static class ConfigureJWT
    {
        public static void AddJWT(this IServiceCollection services)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException(Messages.API.JWTNotConfigured);
            }
            var key = Encoding.UTF8.GetBytes(jwtKey);
            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                            {
                                context.Response.StatusCode,
                                Message = Messages.API.Unauthorized
                            }));
                        }
                    };
                });
        }
    }
}