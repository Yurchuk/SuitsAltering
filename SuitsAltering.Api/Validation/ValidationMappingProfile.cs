using AutoMapper;
using FluentValidation.Results;
using SuitsAltering.API.Common;

namespace SuitsAltering.API.Validation
{
    public class ValidationMappingProfile : Profile
    {
        public ValidationMappingProfile()
        {
            CreateMap<ValidationFailure, ErrorResponse.ErrorModel>()
                .ForMember(dst => dst.FieldName, opt => opt.MapFrom(src => src.PropertyName.Replace(".", string.Empty)))
                .ForMember(dst => dst.Message, opt => opt.MapFrom(src => src.ErrorMessage))
                .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.ErrorCode));
        }
    }
}
