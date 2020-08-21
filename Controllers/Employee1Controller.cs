using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using Provana_Assignment.Models;
using Provana_Assignment.Mechanism;

namespace Provana_Assignment.Controllers
{
    [BasicAuthentication]
    public class Employee1Controller : ApiController
    {
        public HttpResponseMessage GetAllEmployees()
        {
            IList<EmployeeModel> employees = null;

            using (var ctx = new EmployeeDBEntitiesEF())
            {
                employees = ctx.Employees
                            .Select(s => new EmployeeModel()
                            {
                                EmpId = s.EmpId,
                                EmpName = s.EmpName,
                                EmpMobileNo = s.EmpMobileNo,
                                EmpEmail = s.EmpEmail
                            }).ToList<EmployeeModel>();
            }

            try
            {

                if (employees != null && employees.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emplyee not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }  
        }



        public HttpResponseMessage GetEmployeeById(int id) {

            EmployeeModel emp = null;

            using (var ctx = new EmployeeDBEntitiesEF())
            {
                emp = ctx.Employees
                    .Where(s => s.EmpId == id)
                    .Select(s => new EmployeeModel()
                    {
                        EmpId = s.EmpId,
                        EmpName = s.EmpName,
                        EmpMobileNo = s.EmpMobileNo,
                        EmpEmail= s.EmpEmail
                    }).FirstOrDefault<EmployeeModel>();
            }

            try
            {

                if (emp != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emplyee not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }  
        
        }


        public HttpResponseMessage PostNewStudent(EmployeeModel emp)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Invalid data.");

            using (var ctx = new EmployeeDBEntitiesEF())
            {
                ctx.Employees.Add(new Employee()
                {
                    EmpName = emp.EmpName,
                    EmpMobileNo = emp.EmpMobileNo,
                    EmpEmail = emp.EmpEmail
                });

                ctx.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.Created,"New Employee Created");
        }

        public HttpResponseMessage PutEmployee(EmployeeModel emp)
    {
        if (!ModelState.IsValid)
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");


        using (var ctx = new EmployeeDBEntitiesEF())
        {
            var existingEmp = ctx.Employees.Where(s => s.EmpId == emp.EmpId)
                                                    .FirstOrDefault<Employee>();

            if (existingEmp != null)
            {
                existingEmp.EmpName = emp.EmpName;
                existingEmp.EmpMobileNo = emp.EmpMobileNo;
                existingEmp.EmpEmail = emp.EmpEmail;
                ctx.SaveChanges();
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emplyee not found");
            }
        }

        return Request.CreateResponse(HttpStatusCode.Accepted, "New Employee Created");
    }


        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");

            using (var ctx = new EmployeeDBEntitiesEF())
            {
                var emp = ctx.Employees
                    .Where(s => s.EmpId == id)
                    .FirstOrDefault();

                if (emp != null)
                {
                    ctx.Entry(emp).State = EntityState.Deleted;
                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Accepted, " Employee Deleted");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emplyee not found");
                }
                
            }
        }
    }
}
