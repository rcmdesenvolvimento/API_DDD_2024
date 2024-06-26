using Domain.Interfaces.Generics;
using Domain;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Infrastructure.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.InterfaceServices;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApis.Token;
using AutoMapper;
using WebApis.Models;

namespace WebApis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /****************************************************************/
            // ConfigServices
            /****************************************************************/
            builder.Services.AddDbContext<ContextBase>(options =>
              options.UseSqlServer(
                  builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ContextBase>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            /****************************************************************/
            // Interface e Reposit�rio
            /****************************************************************/
            builder.Services.AddSingleton(typeof(IGenerics<>), typeof(RepositoryGenerics<>));
            builder.Services.AddSingleton<IMessage, RepositoryMessage>();

            /****************************************************************/
            // Servi�o Dom�nio
            /****************************************************************/
            builder.Services.AddSingleton<IServiceMessage, ServiceMessage>();

            /****************************************************************/
            // JWT
            /****************************************************************/
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(option =>
              {
                  option.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = "Teste.Securiry.Bearer",
                      ValidAudience = "Teste.Securiry.Bearer",
                      IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
                  };

                  option.Events = new JwtBearerEvents
                  {
                      OnAuthenticationFailed = context =>
                      {
                          Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                          return Task.CompletedTask;
                      },
                      OnTokenValidated = context =>
                      {
                          Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                          return Task.CompletedTask;
                      }
                  };
              });

            /****************************************************************/
            // Auto Mapper
            /****************************************************************/
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageViewModel, Message>();
                cfg.CreateMap<Message, MessageViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            builder.Services.AddSingleton(mapper);

            /****************************************************************/


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //var urlDev = "https://dominiodocliente.com.br";
            //var urlHML = "https://dominiodocliente2.com.br";
            //var urlPROD = "https://dominiodocliente3.com.br";

            //app.UseCors(b => b.WithOrigins(urlDev, urlHML, urlPROD));

            var devClient = "http://localhost:4200";
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader().WithOrigins(devClient));

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
