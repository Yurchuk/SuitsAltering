using SuitsAltering.DAL.Enums;

namespace SuitsAltering.API.Models;

public class UpdateAlteringRequest
{
    public Guid Id { get; set; }
    public AlteringStatus AlteringStatus { get; set; }
}