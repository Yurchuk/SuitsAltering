using SuitsAltering.DAL.Enums;

namespace SuitsAltering.BL.Models;

public class AlteringCreateModel
{
    public ClothingType ClothingType { get; set; }
    public int LeftAdjustment { get; set; }
    public int RightAdjustment { get; set; }
}