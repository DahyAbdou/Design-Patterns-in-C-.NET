using Application.Abstraction.DesignPatterns.Strategy;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Application.Implementation.DesignPatterns.Strategy
{
    public class ShippingCostStrategyResolver : IShippingCostStrategyResolver
    {
        private readonly IReadOnlyDictionary<string, IShippingCostStrategy> _strategies;

        public ShippingCostStrategyResolver(IEnumerable<IShippingCostStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(strategy => strategy.Method, StringComparer.OrdinalIgnoreCase);
        }

        public IShippingCostStrategy Resolve(string method)
        {
            if (string.IsNullOrWhiteSpace(method) || !_strategies.TryGetValue(method, out var strategy))
                throw new CustomHttpException(HttpStatusCode.BadRequest, $"Shipping method '{method}' is not supported. Use standard, express, or free.");

            return strategy;
        }
    }
}
