using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.EntityTypeConfiguration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Login).IsRequired();

        builder.Property(x => x.Password).IsRequired();
    }
}
