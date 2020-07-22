using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Infrastructure
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
    public class SectionConfigurations : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Name).IsRequired();

            builder
                .HasData(new Section
                { Id = 1, Name = "Programming" });
            builder
                .HasData(new Section
                { Id = 2, Name = "Management" });
            builder
                .HasData(new Section
                { Id = 3, Name = "PMP" });
            builder
                .HasData(new Section
                { Id = 4, Name = "Engineering" });
            builder
                .HasData(new Section
                { Id = 5, Name = "Math" });
        }
    }
}
