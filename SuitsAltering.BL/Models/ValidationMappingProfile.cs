using AutoMapper;
using FluentValidation.Results;
using SuitsAltering.BL.Results;

namespace SuitsAltering.BL.Models
{
    public class ValidationMappingProfile : Profile
    {
        public ValidationMappingProfile()
        {
            CreateMap<Error, ValidationFailure>(MemberList.Source)
                .ForMember(dst => dst.ErrorCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dst => dst.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dst => dst.PropertyName, opt => opt.MapFrom(src => src.FieldName))
                .ForMember(dst => dst.CustomState, opt => opt.MapFrom(src => src.Parameters));

            CreateMap<ValidationFailure, Error>()
                .ForCtorParam("code", opt => opt.MapFrom(src => src.ErrorCode))
                .ForCtorParam("message", opt => opt.MapFrom(src => src.ErrorMessage))
                .ForMember(dst => dst.FieldName, opt => opt.MapFrom(src => src.PropertyName));

            CreateMap<ValidationResult, Result>()
                .ForCtorParam("status", opt => opt.MapFrom(src => src.IsValid ? ResultStatus.Success : ResultStatus.ValidationError))
                .ForCtorParam("errors", opt => opt.MapFrom(src => src.Errors))
                .ForMember(dst => dst.Warnings, opt => opt.Ignore());
        }
    }
}
