using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Models.Account
{
    // You can add profile data for the user by adding more properties to your ApplicationDbUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public partial class ApplicationUser : IdentityUser
    {
        public string avatar { get; set; }
        public string name { get; set; }
        public DateTime? dateJoin { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authencationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authencationType);
            // Add custom user claims here
            this.avatar = this.avatar == null ? "default.png" : this.avatar;
            this.name = this.name == null ? "default" : this.name;
            this.dateJoin = this.dateJoin == null ? DateTime.Now : this.dateJoin;
            userIdentity.AddClaim(new Claim("avatar", this.avatar));
            userIdentity.AddClaim(new Claim("name", this.name));
            return userIdentity;
        }
        
    }
    public static class IdentityExtensions
    {
        public static string GetUserAvatar(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("avatar");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserNickName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("name");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
       
    }

}