using System;
using System.Text;
using AutoMapper;
using DocumentManagement.Repository;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Account;
using DocumentManagement.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DocumentManagement
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
      var connection = Configuration["ConnectionStrings:DefaultConnection"];

      services
        .AddEntityFrameworkSqlServer()
        .AddDbContext<AppDbContext>(options => options.UseSqlServer(connection),
          optionsLifetime: ServiceLifetime.Transient);


      services.AddTransient<IAccountService, AccountService>();
      services.AddTransient<IJwtService, JwtService>();

      services.AddAutoMapper();

      //  var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtOptions));

      services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
          options.Password.RequireDigit = true;
          options.Password.RequiredLength = 4;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = false;
          options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

      var key = Configuration["JwtOptions:Key"];

      var signingKeyCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        SecurityAlgorithms.HmacSha256);

      services.Configure<JwtOptions>(options =>
      {
        options.Issuer = Configuration["JwtOptions:Issuer"];
        options.Audience = Configuration["JwtOptions:Audience"];
        options.SigningCredentials = signingKeyCredentials;
      });

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = Configuration["JwtOptions:Issuer"],

        ValidateAudience = true,
        ValidAudience = Configuration["JwtOptions:Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKeyCredentials.Key,

        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(configureOptions =>
      {
        configureOptions.ClaimsIssuer = Configuration["JwtOptions:Issuer"];
        configureOptions.TokenValidationParameters = tokenValidationParameters;
        configureOptions.SaveToken = true;
      });

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseCors(builder =>
        builder.WithOrigins("*")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials());

      app.UseMvc();
    }
  }
}