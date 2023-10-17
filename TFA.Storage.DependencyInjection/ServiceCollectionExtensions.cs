﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;
using TFA.Storage.Storages;

namespace TFA.Storage.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
        {
            services
            .AddScoped<ICreateForumStorage, CreateForumStorage>()
            .AddScoped<IGetForumsStorage, GetForumsStorage>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IGetTopicsStorage, GetTopicsStorage>()

            .AddScoped<IGuidFactory, GuidFactory>()
            .AddScoped<IMomentProvider, MomentProvider>()

            .AddDbContext<ForumDbContext>(options =>
                options.UseNpgsql(dbConnectionString));



            services.AddMemoryCache();

            services.AddAutoMapper(congig =>
            {
                congig.AddMaps(Assembly.GetAssembly(typeof(ForumDbContext)));
            });

            return services;
        }
    }
}
