using AutoMapper;
using SuitsAltering.DAL.Entities;

namespace SuitsAltering.BL.Models;

public class MapperBLProfiles : Profile
{
    public MapperBLProfiles()
    {
        CreateMap<Altering, AlteringCreateResultModel>();
        CreateMap<AlteringCreateModel, Altering>();
        CreateMap<Altering, AlteringUpdateResultModel>();
        CreateMap<Altering, AlteringGetResultModel>();
    }
}