namespace LibraryAppModelFirst.Models
{
    public class Book : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? PublicationYear { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
    }
}
