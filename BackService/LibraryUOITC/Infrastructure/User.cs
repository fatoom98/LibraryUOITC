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
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy Policy { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.Property(f => f.Id).ValueGeneratedOnAdd();

            builder.HasIndex(f => f.UserName).IsUnique();
            builder.Property(f => f.Password).IsRequired().HasMaxLength(150).HasConversion(v => JwtAuth.Tools.Hasher.Hash(v), v => v);
            builder.Property(f => f.UserName).IsRequired().HasMaxLength(20);
            builder.Property(f => f.FullName).IsRequired().HasMaxLength(20);
            builder.Property(f => f.PolicyId).IsRequired();
            builder.Property(f => f.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.HasOne(u => u.Policy)
             .WithMany(b => b.Users)
             .HasForeignKey(f => f.PolicyId)
             .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasData(new User
                { Id = 1, UserName = "admin", FullName="ahmed",Password ="123@root",PolicyId=1, IsDeleted = false });
        }
    }

}
