using System;
using System.Data.Entity.ModelConfiguration;

namespace InventoryManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PassCode { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            Property(u => u.FirstName)
                .HasMaxLength(42)
                .IsRequired();

            Property(u => u.LastName)
                .HasMaxLength(42)
                .IsRequired();

            Property(u => u.Email)
                .HasMaxLength(42)
                .IsRequired();

            Property(u => u.UserName)
                .HasMaxLength(42)
                .IsRequired();

            Property(u => u.Password)
                .HasMaxLength(80)
                .IsRequired();

            Property(u => u.PassCode)
                .HasMaxLength(48);

        }
    }
}