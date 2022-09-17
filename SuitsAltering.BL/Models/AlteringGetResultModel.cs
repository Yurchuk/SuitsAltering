using SuitsAltering.DAL.Enums;

namespace SuitsAltering.BL.Models;

public class AlteringGetResultModel
{
    public Guid Id { get; set; }
    public ClothingType ClothingType { get; set; }
    public int LeftAdjustment { get; set; }
    public int RightAdjustment { get; set; }
    public AlteringStatus AlteringStatus { get; set; }
}