using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Provana_Assignment.Models
{
    public class EmployeeModel
    {
        public int EmpId { get; set; }
        [Required]
        public string EmpName { get; set; }
        [Required]
        [StringLength(10)]
        [MinLength(10)]
        public string EmpMobileNo { get; set; }
        [Required]
        [EmailAddress]
        public string EmpEmail { get; set; }

    }
}