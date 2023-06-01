namespace SP.EmployeeManagement.Dto.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public string? DepartmentName { get; set; }
        public string? PositionName { get; set; }
    }
}
