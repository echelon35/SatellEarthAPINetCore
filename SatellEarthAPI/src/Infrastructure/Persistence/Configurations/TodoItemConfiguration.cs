﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
    }
}