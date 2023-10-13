﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.ModelsDTO;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services) 
        {
            services
                .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                .AddScoped<IGetTopicsUseCase,  GetTopicsUseCase>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>();

            services
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIdentityProvider, IdentityProvider>();

            services
                .AddValidatorsFromAssemblyContaining<ForumDTO>(includeInternalTypes: true);

            services.AddMemoryCache();

            return services;
        } 

    }
}
