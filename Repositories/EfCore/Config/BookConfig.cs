using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EfCore.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasOne(b => b.Category).WithMany().HasForeignKey(C => C.CategoryId).IsRequired();
        builder.HasData(
            new Book{Id = 1,CategoryId = 1,Price = 40,Title = "Annunakiler"},
            new Book{Id = 2,CategoryId = 2, Price = 40,Title = "Planet X"}
        );
    }
}