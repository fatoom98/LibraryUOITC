using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Infrastructure
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }
        public string Note { get; set; }
        public string AuthorName { get; set; }
        public string NamePrinting { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public List<string> Photos { get; set; }
        public int? ShelfNumber { get; set; }
    }
    public class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Name);
            builder.Property(f => f.Code);
            builder.Property(f => f.SectionId);
            builder.Property(f => f.Note);
            builder.Property(f => f.AuthorName);
            builder.Property(f => f.NamePrinting);
            builder.Property(f => f.Date);
            builder.Property(f => f.Path);
            builder.Property(f => f.ShelfNumber).HasDefaultValue(null);
            builder.Property(f => f.Photos).HasMaxLength(500).HasConversion(v => String.Join(';', v), v => v.Split(';', StringSplitOptions.None).ToList());

            builder.HasOne(u => u.Section)
            .WithMany(b => b.Books)
            .HasForeignKey(f => f.SectionId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
