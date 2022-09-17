using AutoMapper;
using FluentValidation;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Validation
{
    internal class BusinessValidatorWrapper<T> : IBusinessValidator<T>
    {
        private readonly IValidator<T> _validator;
        private readonly IMapper _mapper;

        public BusinessValidatorWrapper(IValidator<T> validator, IMapper mapper)
        {
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result> ValidateAsync(T instance)
        {
            var validationResult = await _validator.ValidateAsync(instance);
            var result = _mapper.Map<Result>(validationResult);

            return result;
        }
    }
}
