using BusinessLogic;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((ctx, cfg) => {
        cfg.Host("rabbitmq", "/", h => {
           h.Username("rabbitmq");
           h.Password("rabbitmq");
        });
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


app.MapGet("/", async (
    [FromServices] ILogger<Program> logger, 
    [FromServices] IRequestClient<GetDataRequest> client) => {

    try
    {
        var response = await client.GetResponse<GetDataResponse>(new GetDataRequest {
        Id = Guid.NewGuid()
    });

    logger.LogInformation("Teste");
    return Results.Ok(response);
    }
    catch (Exception ex)
    {        
        logger.LogError("ERROR {ex}", ex.Message);
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    
});

app.Run();