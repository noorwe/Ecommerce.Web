using Ecommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistance.Data.Configurations.ProductConfiguration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.Property(p => p.Name)
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .HasMaxLength(500);
            builder.Property(p => p.PictureUrl)
                .HasMaxLength(200);
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            builder.HasOne(p => p.ProductBranda)
                .WithMany()
                .HasForeignKey(p => p.BrandId);
            builder.HasOne(p => p.ProductTypes)
                .WithMany()
                .HasForeignKey(p => p.TypeId);
        }
    }
}
