using Application.Abstraction;
using Application.Abstraction.DesignPatterns.Adapter;
using Application.Abstraction.DesignPatterns.Facade;
using Application.Abstraction.DesignPatterns.Strategy;
using Application.Abstraction.Services;
using Application.Implementation;
using Application.Implementation.DesignPatterns.Adapter;
using Application.Implementation.DesignPatterns.Adapter.Adaptees;
using Application.Implementation.DesignPatterns.Facade;
using Application.Implementation.DesignPatterns.Strategy;
using Application.Implementation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dependency
{
    public static class ServicesContainer
    {
        public static void AddApplictionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Application.Mapping.MappingProfile));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDoctorJobTitleService, DoctorJobTitleService>();

            services.AddSingleton<IInventoryService, InventoryService>();
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IOrderStore, InMemoryOrderStore>();
            services.AddTransient<IShippingService, ShippingService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IOrderFacade, OrderFacade>();

            services.AddSingleton<StripeGateway>();
            services.AddSingleton<PayPalGateway>();
            services.AddTransient<IPaymentProcessor, StripePaymentAdapter>();
            services.AddTransient<IPaymentProcessor, PayPalPaymentAdapter>();
            services.AddTransient<IPaymentProcessorResolver, PaymentProcessorResolver>();

            services.AddTransient<IShippingCostStrategy, StandardShippingStrategy>();
            services.AddTransient<IShippingCostStrategy, ExpressShippingStrategy>();
            services.AddTransient<IShippingCostStrategy, FreeShippingStrategy>();
            services.AddTransient<IShippingCostStrategyResolver, ShippingCostStrategyResolver>();
            services.AddTransient<IShippingCostCalculator, ShippingCostCalculator>();

            services.AddTransient<IDiscountStrategy, RegularDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, MemberDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, PremiumDiscountStrategy>();
            services.AddTransient<IDiscountStrategyResolver, DiscountStrategyResolver>();
            services.AddTransient<IDiscountCalculator, DiscountCalculator>();
        }
    }
}
