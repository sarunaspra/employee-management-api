namespace SP.EmployeeManagement.DataAccess.Entities
{
    public class Position
    {
        private readonly HashSet<Employee> _employees = new();

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IReadOnlyCollection<Employee>? Employees => _employees;
    }
}
