using Common.Logging;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Host.UseSerilog(Logging.cfg);

builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Register MediatR with the assembly that contains your handlers

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountCommandHanler).Assembly, // we are registering one of handlers in order to give where to scan other ones
};
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
});

builder.Services.AddScoped<IDiscountRepository,DiscountRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

// migrate database
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
}

//app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DiscountService>();
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("--> Communication with grpc endpoints must be made through a grpc client");
    });
});

app.Run();
