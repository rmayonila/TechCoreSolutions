using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCoreSolutions.Models
{
    public class Payroll
    {
        public int PayrollID { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 31)]
        [Display(Name = "Days Worked")]
        public int DaysWorked { get; set; }

        [Required]
        [Range(0, 50000)]
        public decimal Deduction { get; set; }

        [NotMapped]
        [Display(Name = "Gross Pay")]
        public decimal GrossPay => Employee != null ? DaysWorked * Employee.DailyRate : 0;

        [NotMapped]
        [Display(Name = "Net Pay")]
        public decimal NetPay => GrossPay - Deduction;

        public Employee? Employee { get; set; }
    }
}