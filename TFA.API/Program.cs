using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;
using TFA.API.Mapping;
using TFA.API.Midddlewares;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;

namespace TFA.API
{
    public partial class  Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "TFA.API")
                .Enrich.WithProperty("Envirotment", builder.Environment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft")))
                    .WriteTo.OpenSearch(
                builder.Configuration.GetConnectionString("Logs"),
                "forum-logs-{0:yyyy.MM.dd}")
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()));


            // Add services to the container.

            builder.Services
                .AddForumDomain()
                .AddForumStorage(builder.Configuration.GetConnectionString("Postgres"));

            builder.Services.AddAutoMapper(config => config.AddProfile<ApiProfile>());



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            var mapper = app.Services.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.Run();

        }

    }
    public partial class Program { }

}