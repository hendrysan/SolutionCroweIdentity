using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

//builder.Services.AddControllersWithViews();
//builder.Services.AddIdentityServer(options =>
//{
//    options.Events.RaiseErrorEvents = true;
//    options.Events.RaiseInformationEvents = true;
//    options.Events.RaiseFailureEvents = true;
//    options.Events.RaiseSuccessEvents = true;
//})
//.AddTestUsers(TestUsers.Users)
//.AddInMemoryIdentityResources(Config.IdentityResources)
//.AddInMemoryApiScopes(Config.ApiScopes)
//.AddInMemoryApiResources(Config.ApiResources)
//.AddInMemoryClients(Config.Clients)
//.AddDeveloperSigningCredential();


builder.Services.AddIdentityServer()
.AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
})
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    options.EnableTokenCleanup = true;
});

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

//app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.MapDefaultControllerRoute();

app.Run();
