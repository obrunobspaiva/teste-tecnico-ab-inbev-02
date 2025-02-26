using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.Property(s => s.SaleDate)
               .HasConversion(
                   v => v.ToUniversalTime(),   // Converte para UTC ao salvar
                   v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Define como UTC ao carregar
               )
               .HasColumnType("timestamp with time zone"); // Define o tipo correto no PostgreSQL
    }
}
