using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    public class BusinessValidationBuilder<T> : IBusinessValidationBuilder<T>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IList<IValidationRule<T>> _ruleDefinitions = new List<IValidationRule<T>>();

        public BusinessValidationBuilder(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public IBusinessValidationBuilder<T> AddRule<TRule, TProperty>(Expression<Func<T, TProperty>> propertySelector, Error error)
            where TRule : IBusinessRule<TProperty>
        {
            var ruleInstance = ActivatorUtilities.GetServiceOrCreateInstance<TRule>(_serviceProvider);
            var ruleDefinition = new RuleDefinition<T, TProperty>(ruleInstance, error, propertySelector);
            _ruleDefinitions.Add(ruleDefinition);

            return this;
        }

        public IBusinessValidationBuilder<T> AddRule<TRule>(Error error)
            where TRule : IBusinessRule<T>
        {
            return AddRule<TRule, T>(x => x, error);
        }

        public IBusinessValidationBuilder<T> AddRule<TRule, TProperty, TArgs>(
            Expression<Func<T, TProperty>> propertySelector, TArgs args, Error error)
            where TRule : IBusinessRule<TProperty, TArgs>
        {
            var ruleInstance = ActivatorUtilities.GetServiceOrCreateInstance<TRule>(_serviceProvider);
            var ruleWithArgs = new BusinessRuleArgsWrapper<TProperty, TArgs>(ruleInstance, args);
            var ruleDefinition = new RuleDefinition<T, TProperty>(ruleWithArgs, error, propertySelector);
            _ruleDefinitions.Add(ruleDefinition);

            return this;
        }

        public IBusinessValidationBuilder<T> AddRule<TRule, TArgs>(TArgs args, Error error)
            where TRule : IBusinessRule<T, TArgs>
        {
            return AddRule<TRule, T, TArgs>(x => x, args, error);
        }

        public IBusinessValidationBuilder<T> AddValidator<TValidator, TProperty>(Expression<Func<T, TProperty>> propertySelector)
            where TValidator : IBusinessValidator<TProperty>
        {
            var validatorInstance = ActivatorUtilities.GetServiceOrCreateInstance<TValidator>(_serviceProvider);
            var validationDefinition = new ValidationDefinition<T, TProperty>(_mapper, validatorInstance, propertySelector);
            _ruleDefinitions.Add(validationDefinition);

            return this;
        }

        public IBusinessValidationBuilder<T> AddValidator<TValidator>()
            where TValidator : IBusinessValidator<T>
        {
            return AddValidator<TValidator, T>(x => x);
        }

        public IBusinessValidator<T> Build()
        {
            return new BusinessValidatorWrapper<T>(new BusinessValidator<T>(_ruleDefinitions), _mapper);
        }
    }
}
