using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class EmployeeController : ApiController
    {
        /// <summary>
        /// Used for Angular application study | Call web api from Angular app
        /// </summary>
        private static List<EmployeeAngularModel> employeeList = new List<EmployeeAngularModel> {
            new EmployeeAngularModel { code= "emp101", name= "Tom" ,  gender= "Male",   annualSalary= 5500,     dateOfBirth= "06/25/1988" },
            new EmployeeAngularModel { code= "emp103", name= "Mike",  gender= "Male",   annualSalary= 5900,     dateOfBirth= "12/08/1979" },
            new EmployeeAngularModel { code= "emp104", name= "Mary",  gender= "Female", annualSalary= 6500.825, dateOfBirth= "10/14/1980" },
            new EmployeeAngularModel { code= "emp105", name= "Nancy", gender= "Female", annualSalary= 6700.826, dateOfBirth= "11/05/1982" },
            new EmployeeAngularModel { code= "emp106", name= "John",  gender= "Male",   annualSalary= 7000,     dateOfBirth= "07/15/1979" },

        };


        // GET 
        [HttpGet]
        [Route("api/employee/GetEmployeeList")]
        public List<EmployeeAngularModel> GetEmployeeList()
        {
            return employeeList;
        }

        // GET 
        [HttpGet]
        [Route("api/employee/GetEmployee/{code}")]
        public HttpResponseMessage GetEmployeeList(string code)
        {
            ////IEnumerable<EmployeeModel> employees;
            List<EmployeeAngularModel> employees = new List<EmployeeAngularModel>();
            if (employeeList.Count > 0)
            {
                //var employee=from employeelist.
                employees = employeeList.Where(s => s.code == code).ToList();
                if (employees.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with code " + code + " is not found");
        }
    }
}
