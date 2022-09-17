using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SuitsAltering.BL.Errors;
using SuitsAltering.BL.Models;
using SuitsAltering.BL.Results;
using SuitsAltering.BL.ServiceBus;
using SuitsAltering.BL.Validation;
using SuitsAltering.Contracts;
using SuitsAltering.DAL;
using SuitsAltering.DAL.Entities;
using SuitsAltering.DAL.Enums;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;

namespace SuitsAltering.BL.Services;

public class AlteringService: IAlteringService
{
    private ILogger<AlteringService> _logger;
    private readonly IBusinessValidator<AlteringUpdateModel> _updateValidator;
    private readonly SuitsDbContext _suitsDbContext;
    private readonly IMapper _mapper;
    private readonly IServiceBusMessageSender _serviceBusMessageSender;
    private readonly IConfiguration _configuration;

    public AlteringService(
        SuitsDbContext suitsDbContext, 
        IMapper mapper,
        IServiceBusMessageSender serviceBusMessageSender, 
        IBusinessValidator<AlteringUpdateModel> updateValidator, 
        ILogger<AlteringService> logger, 
        IConfiguration configuration)
    {
        _suitsDbContext = suitsDbContext;
        _mapper = mapper;
        _serviceBusMessageSender = serviceBusMessageSender;
        _updateValidator = updateValidator;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<AlteringCreateResultModel>> CreateAsync(AlteringCreateModel model)
    {
        var altering = _mapper.Map<Altering>(model);
        altering.AlteringStatus = AlteringStatus.Created;
        await _suitsDbContext.Alterings.AddAsync(altering);

        await _suitsDbContext.SaveChangesAsync();

        return Result.Success(_mapper.Map<AlteringCreateResultModel>(altering));
    }

    public async Task<Result<AlteringUpdateResultModel>> UpdateAsync(AlteringUpdateModel model)
    {
        var altering = await _suitsDbContext.Alterings.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (altering == null)
        {
            return Result.NotFound<AlteringUpdateResultModel>(AlteringErrors.AlteringNotFound);
        }

        var validationResult = await _updateValidator.ValidateAsync(model);
        if (validationResult.IsFailed)
        {
            return Result.ValidationError<AlteringUpdateResultModel>(validationResult.Errors);
        }

        await using var transaction = await _suitsDbContext.Database.BeginTransactionAsync();

        try
        {
            altering.AlteringStatus = model.AlteringStatus;
            await _suitsDbContext.SaveChangesAsync();

            if (altering.AlteringStatus == AlteringStatus.Done)
            {
                await _serviceBusMessageSender.OrderDoneSendAsync(new OrderDone { AlteringId = altering.Id, Email = _configuration["ClientEmail"] });
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, ex.Message);
            throw;
        }

        return Result.Success(_mapper.Map<AlteringUpdateResultModel>(altering));
    }

    public async Task<Result<PageModel<AlteringGetResultModel>>> Get(int startRow, int endRow)
    {
        var alterings = await _suitsDbContext.Alterings.Skip(startRow).Take(endRow - startRow).ToListAsync();
        var totalItems = await _suitsDbContext.Alterings.CountAsync();
        return new Result<PageModel<AlteringGetResultModel>>(ResultStatus.Success, new PageModel<AlteringGetResultModel>
        {
            Items = _mapper.Map<IEnumerable<AlteringGetResultModel>>(alterings),
            TotalItems = totalItems
        });
    }

    public async Task<Result> SimulatePayment(Guid alteringId)
    {
        await _serviceBusMessageSender.OrderPaidSendAsync(new OrderPaid { AlteringId = alteringId });
        return Result.Success();
    }
}