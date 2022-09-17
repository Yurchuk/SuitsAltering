using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuitsAltering.API.Common;
using SuitsAltering.API.Models;
using SuitsAltering.BL.Models;
using SuitsAltering.BL.Services;
using SuitsAltering.DAL.Enums;

namespace SuitsAltering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlteringController : ResultController
    {
        private readonly ILogger<AlteringController> _logger;
        private readonly IMapper _mapper;
        private readonly IAlteringService _alteringService;

        public AlteringController(
            ILogger<AlteringController> logger, 
            IMapper mapper, 
            IAlteringService alteringService) 
            : base(mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _alteringService = alteringService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponse<AlteringGetResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery] int startRow = 0, [FromQuery] int endRow = 100)
        {
            var result = await _alteringService.Get(startRow, endRow);
            return OperationResult<PageModel<AlteringGetResultModel>, PageModel<AlteringGetResponse>>(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<AlteringCreateResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create(CreateAlteringRequest request)
        {
            var result = await _alteringService.CreateAsync(_mapper.Map<AlteringCreateModel>(request));
            return OperationResult<AlteringCreateResultModel, AlteringCreateResponse>(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(SuccessResponse<AlteringUpdateResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Update(UpdateAlteringRequest request)
        {
            var result = await _alteringService.UpdateAsync(_mapper.Map<AlteringUpdateModel>(request));
            return OperationResult<AlteringUpdateResultModel, AlteringUpdateResponse>(result);
        }

        //can be used only from function
        //TODO cover with APIKey
        [HttpPut("[action]/{alteringId:guid}")]
        [ProducesResponseType(typeof(SuccessResponse<AlteringUpdateResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> SetPaidStatus(Guid alteringId)
        {
            var result = await _alteringService.UpdateAsync(new AlteringUpdateModel {Id = alteringId, AlteringStatus = AlteringStatus.Paid});
            return OperationResult<AlteringUpdateResultModel, AlteringUpdateResponse>(result);
        }

        [HttpPost("[action]/{alteringId:guid}")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        public async Task<IActionResult> SimulatePayment(Guid alteringId)
        {
            var result = await _alteringService.SimulatePayment(alteringId);
            return OperationResult(result);
        }
    }
}