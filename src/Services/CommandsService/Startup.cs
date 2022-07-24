using System;
using CommandsService.Data;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using CommandsService.EventProcessing;
using CommandsService.AsyncDataServices;
using Microsoft.Extensions.Configuration;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommandsService", Version = "v1" });
            });

            services.AddHostedService<MessageBusSubscriber>();
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            services.AddScoped<ICommandRepo, CommandRepo>();
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddScoped<IPlatformDataClient, PlatformDataClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommandsService v1"));
            }
            else
            {
                app.UseHttpsRedirection();
            }            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app);
        }
    }
}