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
//using WebApiContrib.Formatting.Jsonp;

namespace WebApi.Controllers
{
    //[Authorize]
    // Commented above code to avoid the error "Authorization has been denied for this request."

    /// <summary>
    /// To enable CORS to allow cross domain ajax calls within a controller
    /// </summary>
    [EnableCorsAttribute("*", "*", "*")]
    [Authorize]
    public class ValuesController : ApiController
    {
        static List<string> strings = new List<string>() { "String-1", "String-2", "String-3" };


        //static List<EmployeeModel> employeeList;
        //static ValuesController()
        //{
        //    //employeeList = new List<EmployeeModel>();
        //    employeeList = new List<EmployeeModel>
        //                    {
        //                        new EmployeeModel {FirstName = "Arun", LastName = "Mathew", Gender = "Male" },
        //                        new EmployeeModel {FirstName = "Kiran", LastName = "Jose", Gender = "Male" }
        //                    };
        //    employeeList.Add(new Models.EmployeeModel { FirstName = "Vipn", LastName = "S", Gender = "Male" });
        //}

        //// OR

        private static List<EmployeeModel> employeeList = new List<EmployeeModel> {
            new EmployeeModel {ID=1, FirstName = "Arun", LastName = "Mathew", Gender = "Male", Salary=2000 },
            new EmployeeModel {ID=2, FirstName = "Kiran", LastName = "Jose", Gender = "Male" , Salary=3000},
            new EmployeeModel {ID=3, FirstName = "Vikram", LastName = "Mathew", Gender = "Male" , Salary=4000},
            new EmployeeModel {ID=4, FirstName = "Vikram", LastName = "Jose", Gender = "Male" , Salary=5000}
        };



        // GET api/values
        public IEnumerable<string> Get()
        {
            //return new string[] { "value1", "value2" };
            return strings;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return strings[id];
        }

        #region GET samples
        // GET 
        [HttpGet]
        [Route("api/values/GetAll")]
        public IEnumerable<string> GetAll()
        {
            return new List<string>() { "ABC", "XYZ", "PQR" };
        }

        // GET 
        [HttpGet]
        [Route("api/values/GetAll/{EmpId}")]
        public IEnumerable<string> GetAll(int EmpId)
        {
            return new List<string>() { "111", "222", "333" };
        }

        //[Route("api/Discount/GetData/{CategoryCode}/{Gender}")]
        //public List<string> GetData(string CategoryCode, string Gender)

        //[HttpGet]
        //[Route("api/values/GetAll/{EmpId}/{EmpName}")]
        //[HttpGet("{EmpId}/{EmpName}")]
        //public IEnumerable<string> GetAll(int EmpId, string EmpName)
        //{
        //    return new List<string>() { "111", "222", "333" };
        //}
        
        // GET 
        [HttpGet]
        [Route("api/values/GetEmployeeList")]
        public List<EmployeeModel> GetEmployeeList()
        {
            return employeeList;
        }

        // GET 
        [HttpGet]
        [Route("api/values/GetEmployee/{firstName}")]
        public HttpResponseMessage GetEmployeeList(string firstName)
        {
            ////IEnumerable<EmployeeModel> employees;
            List<EmployeeModel> employees = new List<EmployeeModel>();
            if (employeeList.Count > 0)
            {
                //var employee=from employeelist.
                employees = employeeList.Where(s => s.FirstName == firstName).ToList();
                if (employees.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with FirstName " + firstName + " is not found");
        }

        #endregion

        #region POST Samples

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //    strings.Add(value);
        //}

        // POST api/values
        public HttpResponseMessage Post([FromBody]string value)
        {
            try
            {
                strings.Add(value);

                var message = Request.CreateResponse(HttpStatusCode.Created, value);
                message.Headers.Location = new Uri(Request.RequestUri +"/"+ value.ToString());

                //throw new DivideByZeroException();

                return message;
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/values/SaveEmployee")]
        public HttpResponseMessage SaveEmployee([FromBody]EmployeeModel employee)
        {
            try
            {
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);

                employeeList.Add(employee);

                message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.FirstName.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        #endregion

        #region DELETE
        // DELETE api/values/5
        [HttpDelete]
        [Route("api/values/DeleteItem/{item}")]
        public HttpResponseMessage Delete(string item)
        {
            try
            {
                //throw new DivideByZeroException();

                bool isExit = strings.Count() == 0 ? false : strings.Contains(item);

                if (!isExit)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "item is Not extst");
                }
                //else
                //{
                //    //strings.RemoveAt(id);
                //    strings.Remove(item);
                //    return Request.CreateResponse(HttpStatusCode.OK);
                //}

                //strings.RemoveAt(id);
                strings.Remove(item);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        #endregion

        // PUT api/values/5
        [HttpPut]
        [Route("api/values/EditItem/{index}")]    ////Use this if we want to use like http://localhost:50101/api/values/EditItem/3
        //[Route("api/values/EditItem")]      ////Use this if we want to use Query String like http://localhost:50101/api/values/EditItem?index=3 
        public HttpResponseMessage Put(int index, [FromBody]string value)
        {
            try
            {
                bool isExit = strings.Count() >= (index + 1) ? true : false;
                if (!isExit)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "item is Not extst");
                }

                strings[index] = value;

                return Request.CreateResponse(HttpStatusCode.OK, strings);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
