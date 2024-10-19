using System.Diagnostics.Metrics;
using Grpc.Core;
using MassTransit;
using MassTransit.Monitoring;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Prometheus;
using Server;
internal class Program
{
    private static readonly Meter MyMeter = new Meter("Teste.Server", "1.0");


    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        // var meterProvider = Sdk.CreateMeterProviderBuilder()
        //     .AddMeter("Teste.Server")
        //     // .AddOtlpExporter((exporterOptions, metricReaderOptions) => {
        //     //     exporterOptions.Endpoint = new Uri("https://prometheus");
        //     //     exporterOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        //     //     metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
        //     // })
        //     .AddPrometheusExporter()
        //     .Build();

        // builder.Services.AddSingleton(meterProvider);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("rabbitmq");
                    h.Password("rabbitmq");
                });
                cfg.ConfigureEndpoints(ctx);

            });
            x.AddConsumer<RequestConsumer>(cfg =>
            {

            });
        });

        builder.Services.AddOpenTelemetry()
        .ConfigureResource(r =>
        {
            r.AddService("Teste.Server",
            serviceVersion: "1.0",
            serviceInstanceId: Environment.MachineName);
        }).WithMetrics(metrics =>
        metrics
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter("Microsoft.AspNetCore.Hosting",
                         "Microsoft.AspNetCore.Server.Kestrel")
        .AddMeter(InstrumentationOptions.MeterName)
                         );




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseHttpsRedirection();

        app.Run();
    }
}

