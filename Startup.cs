using Dorywcza.Data;
using Dorywcza.Services.EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Dorywcza
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // To receiving data in format JSON with proper parsing, NewtonsoftJson Added
            services.AddControllers().AddNewtonsoftJson();

            // Adding ConnectionString and DatabaseProvider to Database context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DorywczaConnection")));

            services.AddMvc();

            // EmailService - providing injection of configuration class anywhere in app
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));

            services.AddSingleton<IEmailConfiguration>(sp =>
               sp.GetRequiredService<IOptions<EmailConfiguration>>().Value);

            services.AddTransient<IEmailProvider, EmailProvider>();

            #region CORS Policies
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });

                options.AddPolicy("GetOnly", 
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .WithMethods("GET");
                    });

                options.AddPolicy("AnyOriginAllowed", 
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
