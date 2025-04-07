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
    }
}
