using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, np => np.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                
                OStatus=>OStatus.ToString(),
                OStatus=>(OrderStatus) Enum.Parse(typeof(OrderStatus),OStatus)

                
                );

            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.SupTotal).HasColumnType("decimal(18,2)");

        }
    }
}
