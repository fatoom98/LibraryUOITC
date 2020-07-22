using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Infrastructure
{
    public class Policy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool? IsDeleted { get; set; }
        public ICollection<User> Users { get; set; }
    }
    public class PolicyConfigurations : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Name).IsRequired().HasMaxLength(30);
            builder.Property(f => f.Number).IsRequired();
            builder.Property(f => f.IsDeleted).IsRequired().HasDefaultValue(false);

            builder
                .HasData(new Policy
                { Id = 1, Name = "ADMIN", Number = 1000, IsDeleted = false });
        }
    }
}
