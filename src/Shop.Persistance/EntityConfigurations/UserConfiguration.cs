﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoesShop.Entities;

namespace ShoesShop.Persistence.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.UserName)
                   .HasMaxLength(128)
                   .IsRequired();
            builder.Property(x => x.Login)
                   .HasMaxLength(128)
                   .IsRequired();
            builder.Property(x => x.Password)
                   .HasMaxLength(256)
                   .IsRequired();
            builder.Property(x => x.Phone)
                   .HasMaxLength(50);
            builder.HasOne(x => x.Address)
                   .WithMany(x => x.Users)
                   .HasForeignKey(x =>x.AddressId);

            builder.Navigation(x => x.Address)
                   .AutoInclude();
            builder.Navigation(x => x.ShopCarts)
                   .AutoInclude();
            builder.Navigation(x => x.Favorites)
                   .AutoInclude();
            builder.Navigation(x => x.Orders)
                   .AutoInclude();
        }
    }
}
