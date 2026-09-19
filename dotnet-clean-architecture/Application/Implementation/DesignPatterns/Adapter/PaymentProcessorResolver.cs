using Application.Abstraction.DesignPatterns.Adapter;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Application.Implementation.DesignPatterns.Adapter
{
    public class PaymentProcessorResolver : IPaymentProcessorResolver
    {
        private readonly IReadOnlyDictionary<string, IPaymentProcessor> _processors;

        public PaymentProcessorResolver(IEnumerable<IPaymentProcessor> processors)
        {
            _processors = processors.ToDictionary(processor => processor.Provider, StringComparer.OrdinalIgnoreCase);
        }

        public IPaymentProcessor Resolve(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider) || !_processors.TryGetValue(provider, out var processor))
                throw new CustomHttpException(HttpStatusCode.BadRequest, $"Payment provider '{provider}' is not supported. Use stripe or paypal.");

            return processor;
        }
    }
}
