using Application.Auth;
using Application.CommandQuery.Query;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); //obligar que se este autenticado
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
            services.AddDbContext<ActivityDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConnectionDB"));
            });
            services.AddMediatR(typeof(AuthCommandHandler).Assembly);
            //config Identity Core
            var builder = services.AddIdentityCore<User>(); // representacion de la instancia user
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<ActivityDbContext>();// agrega la instancia del entityframework
            identityBuilder.AddSignInManager<SignInManager<User>>(); //el manejador de accesos va a se identitymanager
            services.TryAddSingleton<ISystemClock, SystemClock>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            //inyectamos jwt
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddAutoMapper(typeof(PropertyQueryHandler));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(builder => builder.WithOrigins("*").WithMethods("*").WithHeaders("*"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
