using AutoMapper;
using SuitsAltering.API.Common;
using SuitsAltering.BL.Models;
using SuitsAltering.BL.Results;

namespace SuitsAltering.API.Models;

public class MapperApiProfiles: Profile
{
    public MapperApiProfiles()
    {
        CreateMap<Result, ErrorResponse>();
        CreateMap<Result, SuccessResponse>();
        CreateMap(typeof(Result<>), typeof(SuccessResponse<>))
            .ForMember("Result", opt => opt.MapFrom("Value"));
        CreateMap<Error, ErrorResponse.ErrorModel>();
        CreateMap(typeof(PageModel<>), typeof(PageModel<>));

        CreateMap<CreateAlteringRequest, AlteringCreateModel>();
        CreateMap<UpdateAlteringRequest, AlteringUpdateModel>();
        CreateMap<AlteringGetResultModel, AlteringGetResponse>();
        CreateMap<AlteringCreateResultModel, AlteringCreateResponse>();
        CreateMap<AlteringUpdateResultModel, AlteringUpdateResponse>();
    }
}