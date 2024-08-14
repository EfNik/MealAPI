using AutoMapper;
using MealAPI.DbContexts;
using MealAPI.Entities;
using MealAPI.Models;
using MealAPI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MealAPI.EndpointHandlers;
using System.Net;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the DbContext 
builder.Services.AddDbContext<MealDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:MealsDBConnectionString"]));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminToBeChef", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("profession","chef")
        );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthNZ",
        new()
        {
            Name = "Authorization",
            Description = "Token-based authentication and authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        });
        options.AddSecurityRequirement(new()
        {
            {
                new ()
                {
                    Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TokenAuthNZ" }
                }, new List<string>()}
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(configureApplicationBuilder =>
    {
        configureApplicationBuilder.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("An unexpected problem happened.");
            });

    });
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterMealsEndpoints();
app.RegisterIngredientsEndpoints();

// Recreate the DB every time the API starts
// (for experimenting/Demo purposes)
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()
    .CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MealDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();

