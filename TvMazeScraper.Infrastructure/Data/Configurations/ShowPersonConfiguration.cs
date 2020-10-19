using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Infrastructure.Data.Configurations
{
    public class ShowPersonConfiguration : IEntityTypeConfiguration<ShowPerson>
    {
        public void Configure(EntityTypeBuilder<ShowPerson> builder)
        {
            builder
                .HasKey(s => new { s.ShowId, s.PersonId });
            builder.Property(t => t.PersonId)
                .ValueGeneratedNever();
            builder.Property(t => t.ShowId)
                .ValueGeneratedNever();
        }
    }
}
