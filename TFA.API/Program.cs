using Microsoft.EntityFrameworkCore;
using TFA.Domain.Authorization;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Storage;

namespace TFA.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Postgres");

            // Add services to the container.

            builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
            builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
            builder.Services.AddScoped<ICreateTopicStorage>();
            builder.Services.AddScoped<IIntentionResolver, TopicIntentionResolver>();
            builder.Services.AddScoped<IIntentionManager, IntentionManager>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ForumDbContext>(options =>
                options.UseNpgsql(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}