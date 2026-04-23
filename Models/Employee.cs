using System.ComponentModel.DataAnnotations;

namespace TechCoreSolutions.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        [Display(Name = "Daily Rate")]
        public decimal DailyRate { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Now;

        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}