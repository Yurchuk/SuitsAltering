using SuitsAltering.BL.Models;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Services;

public interface IAlteringService
{
    Task<Result<AlteringCreateResultModel>> CreateAsync(AlteringCreateModel model);
    Task<Result<AlteringUpdateResultModel>> UpdateAsync(AlteringUpdateModel model);
    Task<Result<PageModel<AlteringGetResultModel>>> Get(int startRow, int endRow);
    Task<Result> SimulatePayment(Guid alteringId);
}