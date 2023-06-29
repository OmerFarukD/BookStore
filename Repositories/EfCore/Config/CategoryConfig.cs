using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EfCore.Config;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.CategoryId);
        builder.HasMany(b => b.Books).WithOne();
        builder.Property(c => c.CategoryName).IsRequired();
        builder.HasData(new Category()
        {
            CategoryId = 1,
            CategoryName = "Bilgisyar Bilimleri"
        },
        new Category()
        {
            CategoryId = 2,
            CategoryName = "Network"
        });

    }
}