using System;
using System.Collections.Generic;
using Api.CrossCutting.DependencyInjection;
using Api.CrossCutting.Mappings;
using Api.Domain.Secutiry;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment ;
        }

        public IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(_environment.IsEnvironment("Testing")) 
            {
                Environment.SetEnvironmentVariable("DB_CONNECTION","Persist Security Info=True;Server=localhost;Port=3306;Database=dbApi_Integration;Uid=root;Pwd=joaopjk");
                Environment.SetEnvironmentVariable("DATABASE","MYSQL");
                Environment.SetEnvironmentVariable("MIGRATION","APLICAR");
                Environment.SetEnvironmentVariable("Audience","ExemploAudience");
                Environment.SetEnvironmentVariable("Issuer","ExemploIssuer");
                Environment.SetEnvironmentVariable("Seconds","12000");
            }
            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            var singning = new SigningConfiguration();
            services.AddSingleton(singning);
            var tokenConfiguration = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfiguration);
            services.AddSingleton(tokenConfiguration);
            services.AddControllers();

            var config = new AutoMapper.MapperConfiguration(config =>
            {
                config.AddProfile(new DtoToModelProfile());
                config.AddProfile(new EntityToDtoProfile());
                config.AddProfile(new ModelToEntityProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
            {
                var paramsValidation = Options.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = singning.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = false;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "App", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o JWT Token ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                 {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
