using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// Used for Angular application study | Call web api from Angular app
    /// </summary>
    public class EmployeeAngularModel
    {
        public string code { get; set; }

        public string name { get; set; }

        public string gender { get; set; }

        public double annualSalary { get; set; }

        public string dateOfBirth { get; set; }
    }
}
