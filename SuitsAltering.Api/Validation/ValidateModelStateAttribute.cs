using System.Net;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuitsAltering.API.Common;

namespace SuitsAltering.API.Validation
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private readonly IMapper _mapper;

        public ValidateModelStateAttribute(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            if (context.HttpContext.Items.TryGetValue(nameof(ValidationResult), out var value) &&
                value is ValidationResult validationResult)
            {
                WriteErrorsToResponse(context, validationResult);
            }
            else
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = (int)HttpStatusCode.BadRequest,
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }

        private void WriteErrorsToResponse(ActionExecutingContext context, ValidationResult validationResult)
        {
            var errors = _mapper.Map<IEnumerable<ErrorResponse.ErrorModel>>(validationResult.Errors);

            context.Result = new JsonResult(new ErrorResponse().WithErrors(errors.ToArray()))
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
    }
}
