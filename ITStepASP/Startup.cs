using System.Text;
using ASP.NETAuthITStep.Auth;
using ASP.NETAuthITStep.Auth.Model;
using ASP.NETAuthITStep.Auth.Options;
using ITStepASP.Auth.Policies.RequirePermissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ASP.NETAuthITStep
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>();
            services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<JwtOptions>(Configuration.GetSection(JwtOptions.Jwt));
            services.AddScoped<JwtService>();
            
            JwtOptions jwtOptions = Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SigningKey)
                        )
                    };
                });

            services.AddCors(
                options => options.AddPolicy("devCors", opts => opts
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            services.AddAuthorization(o =>
            {
                o.AddPolicy("RequireRestrictedAccess", p =>
                {
                    p.Requirements.Add(new RequirePermissions(Permission.ExtendedAccess));
                });
            });

            services.AddSingleton<IAuthorizationHandler, RequirePermissionHandler>();

            services.AddSwaggerGen(c => 
                { c.SwaggerDoc("v1", new OpenApiInfo() {Title = "ITStepASP", Version = "v1"}); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITStepASP v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("devCors");
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(e => e.MapControllers());
        }
    }
}