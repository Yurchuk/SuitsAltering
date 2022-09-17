using SuitsAltering.DAL.Enums;

namespace SuitsAltering.API.Models;

public class CreateAlteringRequest
{
    public ClothingType ClothingType { get; set; }
    public int LeftAdjustment { get; set; }
    public int RightAdjustment { get; set; }
}