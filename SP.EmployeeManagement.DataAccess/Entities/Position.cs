namespace SP.EmployeeManagement.DataAccess.Entities
{
    public class Position
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
