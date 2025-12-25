namespace Minimal.API
{
    public sealed class PeopleService 
    {
        private readonly List<Person> _people = new()
        {
            new Person("Umut Tosun"),
            new Person("Yusuf Balci"),
            new Person("Yusuf Aydin")
        };

        public IEnumerable<Person> Search(string searchTerm)
        {
            return _people.Where(x => x.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

    }
}
