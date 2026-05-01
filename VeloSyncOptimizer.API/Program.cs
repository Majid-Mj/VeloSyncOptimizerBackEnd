using Microsoft.OpenApi.Models;
using VeloSyncOptimizer.API.Extensions;
using VeloSyncOptimizer.Application;
using VeloSyncOptimizer.Infrastructure;
using VeloSyncOptimizer.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.SeedDatabaseAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
        c.HeadContent += @"
            <script>
                (function() {
                    const originalFetch = window.fetch;
                    window.fetch = async (...args) => {
                        const response = await originalFetch(...args);
                        const url = typeof args[0] === 'string' ? args[0] : args[0].url;
                        if (url && url.includes('/api/auth/login') && response.ok) {
                            const clone = response.clone();
                            clone.json().then(json => {
                                const token = json.data?.token;
                                if (token && window.ui) {
                                    window.ui.authActions.authorize({
                                        Bearer: {
                                            name: 'Bearer',
                                            schema: {
                                                type: 'apiKey',
                                                in: 'header',
                                                name: 'Authorization',
                                                description: 'Bearer {token}'
                                            },
                                            value: 'Bearer ' + token
                                        }
                                    });
                                    console.log('Swagger: Automatically authorized from login response.');
                                }
                            });
                        }
                        return response;
                    };
                })();
            </script>";
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();