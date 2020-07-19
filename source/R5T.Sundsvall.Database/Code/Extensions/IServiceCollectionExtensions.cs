using System;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using R5T.Dacia;


namespace R5T.Sundsvall.Database
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="EndpointRepository{TDbContext}"/> implementation of <see cref="IEndpointRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddEndpointRepository<TDbContext>(this IServiceCollection services)
            where TDbContext: DbContext, IEndpointDbContext
        {
            services.AddSingleton<IEndpointRepository, EndpointRepository<TDbContext>>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="EndpointRepository{TDbContext}"/> implementation of <see cref="IEndpointRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IEndpointRepository> AddEndpointRepositoryAction<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IEndpointDbContext
        {
            var serviceAction = ServiceAction.New<IEndpointRepository>(() => services.AddEndpointRepository<TDbContext>());
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="EndpointTypeRepository{TDbContext}"/> implementation of <see cref="IEndpointTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddEndpointTypeRepository<TDbContext>(this IServiceCollection services)
            where TDbContext: DbContext, IEndpointDbContext
        {
            services.AddSingleton<IEndpointTypeRepository, EndpointTypeRepository<TDbContext>>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="EndpointTypeRepository{TDbContext}"/> implementation of <see cref="IEndpointTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IEndpointTypeRepository> AddEndpointTypeRepositoryAction<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IEndpointDbContext
        {
            var serviceAction = ServiceAction.New<IEndpointTypeRepository>(() => services.AddEndpointTypeRepository<TDbContext>());
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="EndpointAndTypeRepository{TDbContext}"/> implementation of <see cref="IEndpointAndTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddEndpointAndTypeRepository<TDbContext>(this IServiceCollection services)
            where TDbContext: DbContext, IEndpointDbContext
        {
            services.AddSingleton<IEndpointAndTypeRepository, EndpointAndTypeRepository<TDbContext>>();

            return services;
        }

        /// <summary>
        /// Adds the <see cref="EndpointAndTypeRepository{TDbContext}"/> implementation of <see cref="IEndpointAndTypeRepository"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IEndpointAndTypeRepository> AddEndpointAndTypeRepositoryAction<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext, IEndpointDbContext
        {
            var serviceAction = ServiceAction.New<IEndpointAndTypeRepository>(() => services.AddEndpointAndTypeRepository<TDbContext>());
            return serviceAction;
        }
    }
}
