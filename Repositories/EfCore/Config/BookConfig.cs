using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EfCore.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book{Id = 1,Price = 40,Title = "Annunakiler"},
            new Book{Id = 2,Price = 40,Title = "Planet X"}
        );
    }
}