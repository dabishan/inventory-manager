using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InventoryManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public const string ADMIN = "InventoryAdmin";
        public const string MANAGER = "InventoryManager";
        public const string CUSTOMER_MANAGER = "InventoryCustomerManager";
        public const string AUDITOR = "InventoryAuditor";
        public const string USER = "InventoryUser";

        public ICollection<History> Histories { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        public class UserConfig : EntityTypeConfiguration<ApplicationUser>
        {
            public UserConfig()
            {
                HasMany(u => u.Histories)
                    .WithRequired(u => u.AssignedBy)
                    .HasForeignKey(u => u.AssignedById)
                    .WillCascadeOnDelete(false);

            }
        }
    }
}