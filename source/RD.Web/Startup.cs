using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RD.Core.Messages;
using RD.Core.Services;
using RD.Core.ValueObjects;
using ReactiveDomain.Foundation;
using ReactiveDomain.Messaging;
using ReactiveDomain.Messaging.Bus;

namespace RD.Web {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
          
            var bootstrap = new Bootstrap();
            bootstrap.Configure();

            var mainBus = bootstrap.MainBus;
            var cmd = MessageBuilder.New(() => new DeviceMsgs.Provision(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "TID",
                "TestDevice",
                DateTime.Now));

            mainBus.Send(cmd);

            var deviceList = bootstrap.DeviceListReadModel.Devices;

            mainBus.Send(MessageBuilder.New(() =>new DeviceMsgs.Activate(deviceList[0].Id,Guid.NewGuid(), DateTime.Now)));
          
            
            /*
            //TODO: How does this get wired up so that I can communicate over the IDispatcher (which seems to implement IBus, etc.)
            IDispatcher dispatcher = default;
            ICorrelatedRepository repository = default;

            new Core.Services.Device.DeviceService(dispatcher, repository);
            new Core.Services.Project.ProjectService(dispatcher, repository);

            // Ninject would allow you to do an AutoActivate.  I need to find out
            // if something can be done similar, or if there needs to be a piece of middleware
            // implemented where all instances of an IAutoActivate are resolved as to wire
            // them up.
            //
            // If we're doing a pattern *like* this, then is there something similar to MediatR to handle this?
            services
                .AddScoped<IAutoActivate, Core.Services.Device.DeviceService>()
                .AddScoped<IAutoActivate, Core.Services.Project.ProjectService>();
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // for all classes that wire-up to the bus, this may be the best way to get them
            // "newed-up" and usable.
            /*
            app.Use(async (HttpContext context, Func<Task> _next) => {
                context.RequestServices.GetServices<IAutoActivate>();
                await _next();    
            });

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            */
        }
    }
}