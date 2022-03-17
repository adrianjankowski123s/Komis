namespace Komis
{
    public class ApplicationRole : Microsoft.AspNet.Identity.EntityFramework.IdentityRole
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string name) : base(name) { }
    public string Description { get; set; }
}
}