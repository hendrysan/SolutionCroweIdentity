using Microsoft.OpenApi.Models;
using WebApi;
using WebApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials();
    });
});

builder.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.ApiName = "api1";
                    options.Authority = "http://103.171.164.79:49504";
                });

builder.Services.AddDbContext<DatabaseContext>(options =>
{
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://103.171.164.79:49504/connect/authorize"),
                TokenUrl = new Uri("http://103.171.164.79:49504/connect/token"),
                Scopes = new Dictionary<string, string>
                            {
                                {"api1", "Demo API - full access"}
                            }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});


var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
