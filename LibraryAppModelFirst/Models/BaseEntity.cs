namespace LibraryAppModelFirst.Models
{
    public abstract class BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
