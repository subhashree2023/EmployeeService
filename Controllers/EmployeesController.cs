using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeesDataAccess;

namespace EmployeeService.Controllers
{
    /*
     * using EmployeesDataAccess:this project(class library) is to create database entity(edmx file) and added reference of this class library proj to current project to get access to database entities.
     * Make sure entity framework is installed and reference added in web config file under <configSections> section.otherwise get an error while writing EmployeeDBEntities entities=new EmployeeDBEntities().Reference not found error
     * Add Connectionstring path of App.config of EmployeesDataAccess project to this project's web config file to get access of data otherwise it will through no connection error.
*/
    public class EmployeesController : ApiController
    {
        // GET api/employess
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities=new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        /*If return void on Post method then we will get 204-no content as output
        * but we should have 201-created successfully msg with uri to new content*/
        public HttpResponseMessage Post([FromBody] Employee employee) //Data of Employee come from request body ,so [FromBody]
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    //we need created status and employee object that we added to db.Httpstatuscode is an Enum,created is 201.
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    //uri of newly created item
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
