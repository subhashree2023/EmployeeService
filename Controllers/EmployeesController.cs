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
        /*Can put any name Prefixed with Get(case insensitive, can be get),then it will map with HttpGet verb
         * Http verb Get mapped to -Get() or like GetEmployee() or getEmployees()
         * or We can set any method name(without get prefix) then decorate that method with Httpverb attribute
         */
        [HttpGet]
        public HttpResponseMessage LoadEmployees(string gender="All")        
        {
            using (EmployeeDBEntities entities=new EmployeeDBEntities())
            {
                switch (gender!=null?gender.ToLower():"all")
                {
                    case "all":                
                    return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() =="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, gender + " is invalid.Gender must be All,Male or Female.");                
            }
                
                //    if(gender.ToLower()=="all")
                //    { 
                //       return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                //    }else if(gender.ToLower() == "male" || gender.ToLower() == "female")
                //    {
                //        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e=>e.Gender.ToLower()==gender.ToLower()).ToList());
                //    }
                //    else {
                //        return Request.CreateResponse(HttpStatusCode.BadRequest, gender + " is invalid.Gender must be All,Male or Female.");
                //    }

            }
        }

        /* if put return type as employee like this public Employee Get(int id){},and if we do get request for ant id that is not available in db then we will get body as null and statuscode as 200(ok).
         * But according to standards of REST,when item is not found then status code should be 404-not found.
         * To achieve this, Add HttpResponseMessage as return type and do like below.
         */
        public HttpResponseMessage GetEmployee(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity= entities.Employees.FirstOrDefault(e => e.ID == id);
                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id=" + id.ToString() + " not found.");
                }
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

        //Don't put return type as void as it will show status code as 204-no content
        public HttpResponseMessage  Delete(int id) 
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    //do check for existance of that id first
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id=" + id.ToString() + " not found to delete.");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }

        public HttpResponseMessage Put(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    //do check for existance of that id first
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id=" + id.ToString() + " not found to update.");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
