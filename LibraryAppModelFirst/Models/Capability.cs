namespace LibraryAppModelFirst.Models;

public class Capability
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
