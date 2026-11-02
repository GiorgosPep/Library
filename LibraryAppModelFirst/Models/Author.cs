using System.Diagnostics.Eventing.Reader;

namespace LibraryAppModelFirst.Models
{
    public class Author : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
