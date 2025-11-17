// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModel {
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    public bool IsSoftDeleted => SoftDeletedAt != DateTime.MinValue;
    public DateTime SoftDeletedAt { get; set; } = DateTime.MinValue;
    
    [Timestamp] public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public abstract class BaseModelConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel {
    public virtual void Configure(EntityTypeBuilder<TModel> builder) {
        builder.HasKey(model => model.Id);
        builder.HasIndex(model => model.Id)
            .IsUnique();
        
        builder.Property(model => model.CreatedAt)
            .IsRequired();
        
        builder.Property(model => model.ModifiedAt)
            .IsRequired();
        
        builder.Ignore(model => model.IsSoftDeleted);
        builder.Property(model => model.SoftDeletedAt)
            .IsRequired();
        
        builder.Property(model => model.RowVersion)
            .IsRowVersion();

        builder.UseTptMappingStrategy();
    }
}

public abstract class BaseModelValidator<TModel> : AbstractValidator<TModel> where TModel : BaseModel {
    protected BaseModelValidator() {
        RuleFor(model => model.Id).NotEmpty();
        RuleFor(model => model.CreatedAt).NotEmpty();
        RuleFor(model => model.ModifiedAt).NotEmpty();
        RuleFor(model => model.SoftDeletedAt).NotEmpty();
    }
}