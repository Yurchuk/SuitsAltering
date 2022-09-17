using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuitsAltering.DAL.Enums;

namespace SuitsAltering.DAL.Entities;

public class Altering
{
    public Guid Id { get; set; }
    public ClothingType ClothingType { get; set; }
    public int LeftAdjustment { get; set; }
    public int RightAdjustment { get; set; }
    public AlteringStatus AlteringStatus { get; set; }
}

public class AlteringConfiguration : IEntityTypeConfiguration<Altering>
{
    public void Configure(EntityTypeBuilder<Altering> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LeftAdjustment).HasDefaultValue(0);
        builder.Property(x => x.RightAdjustment).HasDefaultValue(0);

    }
}