using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Provana_Assignment.Models;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Provana_Assignment.Mechanism;


namespace Provana_Assignment.Controllers
{
    //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    [BasicAuthentication]
    public class EmployeeController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        public List<EmployeeModel> GetAllStudents()
        {
            string AllEmployeeQry = "select * from employee";
            List<EmployeeModel> listEmployees = new List<EmployeeModel>();

            using (SqlConnection scon = new SqlConnection(connStr))
            {

                SqlCommand command = new SqlCommand(AllEmployeeQry, scon);
                try
                {
                    scon.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeModel em = new EmployeeModel();
                        em.EmpId = Convert.ToInt32(reader[0]);
                        em.EmpName = reader[1].ToString();
                        em.EmpMobileNo = reader[2].ToString();
                        em.EmpEmail = reader[3].ToString();

                        listEmployees.Add(em);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

                return listEmployees;
            }
        }

        public EmployeeModel Get(int id)
        {

            string GetEmployeeQry = "select * from employee where EmpId=" + id;
            EmployeeModel em = new EmployeeModel();
            if (id <= 0)
                return null;

            using (SqlConnection scon = new SqlConnection(connStr))
            {

                SqlCommand command = new SqlCommand(GetEmployeeQry, scon);
                
                try
                {
                    scon.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            em.EmpId = Convert.ToInt32(reader[0]);
                            em.EmpName = reader[1].ToString();
                            em.EmpMobileNo = reader[2].ToString();
                            em.EmpEmail = reader[3].ToString();
                        }
                        reader.Close();

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
            if (em != null)
                return em;
            else
                return null;

            
        }


        public string PostNewEmployee([FromBody]EmployeeModel emp)
        {
            if (emp == null)
            {
                return "NullPointerExp";
            }

            int id = emp.EmpId;
            string name = emp.EmpName;
            string mobile = emp.EmpMobileNo;
            string email = emp.EmpEmail;
            string EmpInsertQry = "insert into employee values(" +
                       "'" + name +
                       "','" + mobile +
                       "','" + email + "'" +
                ")";

            using (SqlConnection scon = new SqlConnection(connStr))
            {
                int c = -1;
                SqlCommand command = new SqlCommand(EmpInsertQry, scon);
                try
                {
                    scon.Open();

                    c = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                }
                if (c > 0)
                {
                    return "Record Inserted";
                }
                else
                {
                    return "Insertion Failed!";
                }
            }
        }

        public string PutEmployee([FromBody] EmployeeModel emp)
        {
            int c = -1;
            string GetEmployeeQry = "select * from employee where EmpId=" + emp.EmpId;
            string UpdateEmployeeQry = "update employee set " +
                "EmpName=" + "'" + emp.EmpName + "'" +
                ",EmpMobileNo=" + "'" + emp.EmpMobileNo + "'" +
                ",EmpEmail=" + "'" + emp.EmpEmail + "'" +
                " where EmpId=" + emp.EmpId;
            using (SqlConnection scon = new SqlConnection(connStr))
            {

                SqlCommand command = new SqlCommand(GetEmployeeQry, scon);
                SqlCommand command1 = new SqlCommand(UpdateEmployeeQry, scon);
                try
                {
                    scon.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader != null)
                    {
                        reader.Close();
                        c = command1.ExecuteNonQuery();

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                if (c > 0)
                {
                    return "Record update successful";
                }
                else
                {
                    return "Record update failed";
                }
            }
        }


        public string DeleteEmployee(int id)
        {

            int c = -1;
            string GetEmployeeQry = "select * from employee where EmpId=" + id;
            string DeleteEmployeQry = "delete  from employee where EmpId=" + id;
            if (id <= 0)
                return "Invalid id";
            using (SqlConnection scon = new SqlConnection(connStr))
            {

                SqlCommand command = new SqlCommand(GetEmployeeQry, scon);
                SqlCommand command1 = new SqlCommand(DeleteEmployeQry, scon);

                try
                {
                    scon.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader != null)
                    {
                        reader.Close();
                        c = command1.ExecuteNonQuery();

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                if (c > 0)
                {
                    return "Record Delete successful";
                }
                else
                {
                    return "Record Delete failed";
                }
            }

        }

    }

}


