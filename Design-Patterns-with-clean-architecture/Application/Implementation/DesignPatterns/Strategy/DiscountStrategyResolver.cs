using Application.Abstraction.DesignPatterns.Strategy;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Application.Implementation.DesignPatterns.Strategy
{
    public class DiscountStrategyResolver : IDiscountStrategyResolver
    {
        private readonly IReadOnlyDictionary<string, IDiscountStrategy> _strategies;

        public DiscountStrategyResolver(IEnumerable<IDiscountStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(strategy => strategy.CustomerType, StringComparer.OrdinalIgnoreCase);
        }

        public IDiscountStrategy Resolve(string customerType)
        {
            if (string.IsNullOrWhiteSpace(customerType) || !_strategies.TryGetValue(customerType, out var strategy))
                throw new CustomHttpException(HttpStatusCode.BadRequest, $"Customer type '{customerType}' is not supported. Use regular, member, or premium.");

            return strategy;
        }
    }
}
