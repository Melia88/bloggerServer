using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using bloggerServer.Repositories;
using bloggerServer.Services;


namespace bloggerServer
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
      // TODO[epic=Auth] copy/paste
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
        options.Audience = Configuration["Auth0:Audience"];
      });

      services.AddCors(options =>
      {
        options.AddPolicy("CorsDevPolicy", builder =>
              {
                builder
                        .WithOrigins(new string[]{
                          "http://localhost:8080",
                                "http://localhost:8081"
                              })
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
              });
      });
      // end copy/paste

      services.AddControllers();

      // NOTE Transient Services
      services.AddTransient<AccountsService>();
      //   services.AddTransient<RecipesService>();
      //   services.AddTransient<IngredientsService>();

      // NOTE Transient Repo's 
      services.AddTransient<AccountsRepository>();
      //   services.AddTransient<RecipesRepository>();
      //   services.AddTransient<IngredientsRepository>();


      // TODO[epic=DB] database Connection
      services.AddScoped<IDbConnection>(x => CreateDbConnection());



      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "auth_cs_gregslist", Version = "v1" });
      });
    }

    // TODO[epic=DB] database Connection 
    private IDbConnection CreateDbConnection()
    {
      string connectionString = Configuration["db:gearhost"];
      return new MySqlConnection(connectionString);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "auth_cs_gregslist v1"));
        // TODO[epic=Auth] Add Cors Policy when in development mode
        app.UseCors("CorsDevPolicy");
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      // TODO[epic=Auth] Add Authenentication so bearer gets validated
      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
