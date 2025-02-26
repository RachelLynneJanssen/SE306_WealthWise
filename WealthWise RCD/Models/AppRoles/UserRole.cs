using Microsoft.AspNetCore.Identity;

namespace WealthWise_RCD.Models.AppRoles
{
    public class UserRole: IdentityRole
    {
        public UserRole() : base() { }
        public UserRole(string name) : base(name) { }
    }
}
