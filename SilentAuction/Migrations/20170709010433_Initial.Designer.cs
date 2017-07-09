using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SilentAuction.Data;

namespace SilentAuction.Migrations
{
    [DbContext(typeof(AuctionContext))]
    [Migration("20170709010433_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SilentAuction.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ItemName");

                    b.Property<string>("ItemType");

                    b.Property<double>("RetailPrice");

                    b.Property<double>("StartingBid");

                    b.HasKey("Id");

                    b.ToTable("Item");
                });
        }
    }
}
