using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using RanobeNet.Controllers;
using RanobeNet.Data;
using RanobeNet.Repositories;
using RanobeNet.Utils;


var builder = WebApplication.CreateBuilder(args);

var firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase_credentials.json"),
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
            builder.WithOrigins("http://*.ranobe.net", "https://*.ranobe.net", "http://ranobe.net", "https://ranobe.net")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// Add services to the container.
// Add services to the container.
builder.Services.AddDbContext<RanobeNetContext>(options =>
{
    options.UseLazyLoadingProxies();
    var serverVersion = new MySqlServerVersion(new Version(5, 7, 12));
    var connectionString = builder.Configuration.GetConnectionString("RanobeNetContext");
    options.UseMySql(connectionString, serverVersion);
    if (builder.Environment.IsDevelopment())
    {
        options
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});
builder.Services.AddFirebaseAuthentication(firebaseApp.Options.ProjectId);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INovelRepository, NovelRepository>();
builder.Services.Configure<RouteOptions>(options =>
{
    // URL ���������ɂ���
    options.LowercaseUrls = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RanobeNet", Version = "v1.0.0" });

    c.AddSecurityDefinition("firebaseAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.UseInlineDefinitionsForEnums();
    c.MapType<UserField>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = typeof(UserField).GetEnumNames().Select(name => new OpenApiString(name)).Cast<IOpenApiAny>().ToList(),
        Nullable = true
    });
    c.MapType<NovelField>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = typeof(NovelField).GetEnumNames().Select(name => new OpenApiString(name)).Cast<IOpenApiAny>().ToList(),
        Nullable = true
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<FirebaseAuth>((options) => FirebaseAuth.GetAuth(firebaseApp));

builder.Services.AddHealthChecks()
    .AddCheck<DbPendingMigrationHealthCheck<RanobeNetContext>>("db-migration-check");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
