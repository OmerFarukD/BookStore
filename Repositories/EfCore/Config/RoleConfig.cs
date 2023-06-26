using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EfCore.Config;

public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(new IdentityRole()
        {
            Name = "Admin",
            NormalizedName = "ADMIN"
        });
        
        builder.HasData(new IdentityRole()
        {
            Name = "User",
            NormalizedName = "USER"
        });
        
        builder.HasData(new IdentityRole()
        {
            Name = "Editor",
            NormalizedName = "EDITOR"
        });
    }
}