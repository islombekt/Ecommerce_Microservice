using Common.Logging;
using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extension;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog(Logging.cfg);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});



// add Application services 
builder.Services.AddApplicationServices();

// add Infratstructure services 
builder.Services.AddInfraServices(builder.Configuration);

//consumer class
builder.Services.AddScoped<BasketOrderingConsumer>();
builder.Services.AddScoped<BasketOrderingConsumerV2>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ordering.API", Version = "v1" });
});



// mass transit
builder.Services.AddMassTransit(cnf =>
{
    // mark this as consumer
    cnf.AddConsumer<BasketOrderingConsumer>();
    cnf.AddConsumer<BasketOrderingConsumerV2>();
    cnf.UsingRabbitMq((ctx,cofg) =>
    {
        cofg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cofg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c => 
        {  
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });

        // V2 version
        cofg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueueV2, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumerV2>(ctx);
        });
    });
});

builder.Services.AddMassTransitHostedService();
var app = builder.Build();
// Apply db migration
app.MigrateDatabase<OrderContext>((context, services) =>
{ 
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
