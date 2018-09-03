﻿using BaseBooking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BaseBooking.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20180903123723_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("BaseBooking.Models.Reservation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDateTime");

                    b.Property<DateTime>("StartDateTime");

                    b.HasKey("ID");

                    b.ToTable("Reservation");
                });
#pragma warning restore 612, 618
        }
    }
}
