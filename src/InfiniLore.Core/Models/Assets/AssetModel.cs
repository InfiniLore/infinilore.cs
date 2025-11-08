// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using InfiniLore.Core.Models.BaseModels;

namespace InfiniLore.Core.Models.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AssetModelBase : BaseModel {
    public Guid AssetTypeId { get; set; } = Guid.Empty;
    public AssetType AssetType { get; set; } = null!;
    public string? ReadableReferenceName { get; set; }
    public string? DataJson { get; set; }
}

public class AssetModel<T> : AssetModelBase where T : new() {
    [NotMapped]
    [field: MaybeNull, NotMapped]
    public T Data {
        get {
            if (field is not null) return field;

            if (DataJson is not null) {
                try {
                    field = JsonSerializer.Deserialize<T>(DataJson)!;
                }
                catch (Exception) {
                    field = new T();
                }    
            }
            else {
                field = new T();
            }
            
            return field;

        }
        set {
            field = value;
            DataJson = JsonSerializer.Serialize(value);
        }
    }
}

public class AssetModelConfiguration<T> : IEntityTypeConfiguration<AssetModel<T>> where T : new() {
    public void Configure(EntityTypeBuilder<AssetModel<T>> builder) {
        builder.HasKey(a => a.Id);

        builder.Ignore(a => a.Data);

        builder.HasOne<AssetType>()
            .WithMany()
            .HasForeignKey(a => a.AssetTypeId);
    }
}
