using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuitsAltering.API.Common;
using SuitsAltering.BL.Results;
using System.Runtime.CompilerServices;
using SuitsAltering.API.Models;

namespace SuitsAltering.API.Controllers;

public class ResultController : ControllerBase
{
    protected ResultController(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected IMapper Mapper { get; }

    [NonAction]
    public IActionResult OperationResult(Result result)
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        var response = Mapper.Map<SuccessResponse>(result);

        return Ok(response);
    }

    [NonAction]
    public IActionResult OperationResult<TResult, TViewModel>(Result<TResult> result)
        where TResult : class
        where TViewModel : class
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        var response = Mapper.Map<SuccessResponse<TViewModel>>(result);

        return Ok(response);
    }

    [NonAction]
    public IActionResult OperationResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        var response = Mapper.Map<SuccessResponse<T>>(result);

        return Ok(response);
    }

    [NonAction]
    public IActionResult CreatedResult<TResult, TViewModel>(string actionName, Result<TResult> result)
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        var response = Mapper.Map<SuccessResponse<TViewModel>>(result);

        return CreatedAtAction(actionName, response);
    }

    [NonAction]
    public IActionResult CreatedResult<T>(Result<T> result, [CallerMemberName] string actionName = "")
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        var response = Mapper.Map<SuccessResponse<T>>(result);

        return CreatedAtAction(actionName, response);
    }

    [NonAction]
    public IActionResult RedirectResult(Result<string> result)
    {
        if (!result.IsSuccess)
        {
            return ErrorResult(result);
        }

        return Redirect(result.Value);
    }

    [NonAction]
    public IActionResult ErrorResult(Result result)
    {
        var response = Mapper.Map<ErrorResponse>(result);

        return result.Status switch
        {
            ResultStatus.ValidationError => BadRequest(response),
            ResultStatus.NotFound => NotFound(response),
            _ => StatusCode(512, response),
        };
    }
}