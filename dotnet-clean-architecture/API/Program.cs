using API.Filters;
using Application.Dependency;
using Infrastructure.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddFluentValidation(x =>
    {
        x.DisableDataAnnotationsValidation = true;
        x.RegisterValidatorsFromAssemblyContaining<Application.ValidationLocation>();
    });

builder.Services.AddApplictionServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(options =>
{
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    //options.IncludeXmlComments(xmlPath);

    options.SwaggerDoc(
        "V1",
        new OpenApiInfo()
        {
            Title = "Physician API",
            Description = "Documentation for the API with UI to simulate the http requests",
            Version = "V1",
            //Contact = new OpenApiContact() { Email = "e-m.hasouna@lean.sa", Name = "Mohamed Hasouna" }
        }
    );

    options.DocumentFilter<SwaggerDocumentFilter>();
    //options.OperationFilter<SwaggerOperationFilter>();

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "https"
                },
                new string[] { "https" }
            }
        }
    );
});

builder.Services.AddLocalization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Default",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        }
    );
});

builder.Services.AddInfraServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseForwardedHeaders();
app.UseCustomExceptionHandling();
app.UseStaticFiles();

IList<CultureInfo> supportedCultures = new List<CultureInfo>
{
    new CultureInfo("ar"),
    new CultureInfo("en"),
    new CultureInfo("ar-SA")
};

app.UseRequestLocalization(
    new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    }
);

app.UseHttpsRedirection();

app.UseCors("Default");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/healthcheck");
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("V1/swagger.json", "Physician API");
});

app.Run();
