// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfiniLore.Core.Models.BaseModels;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseOwnedModel<TOwner> : BaseModel where TOwner : BaseModel {
    public Guid OwnerId { get; set; } = Guid.Empty;
    public TOwner? Owner {
        get;
        set {
            OwnerId = value?.Id ?? Guid.Empty;
            field = value;
        }
    } = null;
}

public abstract class BaseOwnedModelConfiguration<TModel, TOwner> : BaseModelConfiguration<TModel> 
    where TModel: BaseOwnedModel<TOwner>
    where TOwner : BaseModel {
    
    public override void Configure(EntityTypeBuilder<TModel> builder) {
        base.Configure(builder);
        
        builder.HasOne(model => model.Owner)
            .WithMany()
            .HasForeignKey(model => model.OwnerId);
        
        builder.HasIndex(model => model.OwnerId);
        builder.Property(model => model.OwnerId)
            .IsRequired();   
    }
}

public abstract class BaseOwnedModelValidator<TModel, TOwner> : BaseModelValidator<TModel> 
    where TModel : BaseOwnedModel<TOwner>
    where TOwner : BaseModel 
{
    protected BaseOwnedModelValidator() {
        RuleFor(model => model.OwnerId).NotEmpty();
    }
}
