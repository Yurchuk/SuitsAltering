using SuitsAltering.DAL.Enums;

namespace SuitsAltering.BL.Models;

public class AlteringUpdateModel
{
    public Guid Id { get; set; }
    public AlteringStatus AlteringStatus { get; set; }
}