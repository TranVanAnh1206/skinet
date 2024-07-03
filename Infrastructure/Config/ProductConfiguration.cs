using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PictureUrl).IsRequired().HasMaxLength(180);
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.PictureUrl).IsRequired();
            builder.HasOne(t => t.ProductType).WithMany().HasForeignKey(t => t.ProductTypeId);
            builder.HasOne(t => t.ProductBrand).WithMany().HasForeignKey(t => t.ProductBrandId);
        }
    }
}