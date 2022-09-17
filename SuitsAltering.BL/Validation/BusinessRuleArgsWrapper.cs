namespace SuitsAltering.BL.Validation
{
    internal class BusinessRuleArgsWrapper<T, TArgs> : IBusinessRule<T>
    {
        private readonly IBusinessRule<T, TArgs> _rule;
        private readonly TArgs _args;

        public BusinessRuleArgsWrapper(IBusinessRule<T, TArgs> rule, TArgs args)
        {
            _rule = rule;
            _args = args;
        }

        public async Task<bool> IsValidAsync(T model)
        {
            return await _rule.IsValidAsync(model, _args);
        }
    }
}
