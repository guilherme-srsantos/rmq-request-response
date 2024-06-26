using MassTransit;
using Server;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((ctx, cfg) => {
        cfg.Host("rabbitmq", "/", h => {
           h.Username("rabbitmq");
           h.Password("rabbitmq");
        });
        cfg.ConfigureEndpoints(ctx);
        
    });
    x.AddConsumer<RequestConsumer>(cfg => {

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.Run();
