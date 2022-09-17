using Microsoft.EntityFrameworkCore;
using SuitsAltering.BL.Errors;
using SuitsAltering.BL.Models;
using SuitsAltering.BL.Results;
using SuitsAltering.BL.Validation;
using SuitsAltering.DAL;
using SuitsAltering.DAL.Enums;

namespace SuitsAltering.BL.Validators;

public class UpdateAlteringValidator : BaseBusinessValidator<AlteringUpdateModel>
{
    private readonly SuitsDbContext _suitsDbContext;

    public UpdateAlteringValidator(IBusinessValidationBuilder<AlteringUpdateModel> builder, SuitsDbContext suitsDbContext)
        : base(builder)
    {
        _suitsDbContext = suitsDbContext;
    }   

    public override async Task<Result> ValidateAsync(AlteringUpdateModel updateModel)
    {
        var altering = await _suitsDbContext.Alterings.FirstOrDefaultAsync(x => x.Id == updateModel.Id);

        if (altering == null)
        {
            return Result.Failure(AlteringErrors.AlteringNotFound);
        }

        if ((altering.AlteringStatus == AlteringStatus.Created && updateModel.AlteringStatus == AlteringStatus.Paid)
             || (altering.AlteringStatus == AlteringStatus.Paid && updateModel.AlteringStatus == AlteringStatus.Started)
             || (altering.AlteringStatus == AlteringStatus.Started && updateModel.AlteringStatus == AlteringStatus.Done))
        {
            return Result.Success();
        }

        return Result.Failure(AlteringErrors.CannotBeUpdatedWithThisStatus(altering.AlteringStatus.ToString(), updateModel.AlteringStatus.ToString()));
    }
}