using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Infrastructure.Data.Configurations
{
    public class ShowConfiguration : IEntityTypeConfiguration<Show>
    {
        public void Configure(EntityTypeBuilder<Show> builder)
        {
            builder.Property(t => t.Id)
                .ValueGeneratedNever();
        }
    }
}
