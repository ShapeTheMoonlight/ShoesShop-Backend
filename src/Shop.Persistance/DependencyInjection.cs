﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoesShop.Application.Interfaces;
using ShoesShop.Entities;
using ShoesShop.Persistence.Repository;

namespace ShoesShop.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TestConnection");

            services.AddDbContext<ShopDbContext>(option => option.UseSqlServer(connectionString));
            services.AddUnitOfWork();
            services.AddRepositories();
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //return services.AddScoped<IRepositoryOf<Model>, ShoesRepository>()
            //               .AddScoped<IRepositoryOf<Description>, DescriptionRepository>()
            //               .AddScoped<IRepositoryOf<ModelSize>, ShoesSizeRepository>();
            return services;
        }
    }
}