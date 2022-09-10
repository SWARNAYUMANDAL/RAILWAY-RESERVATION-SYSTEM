using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSEntitiesLib; //for Entities classes
using System.Data.SqlClient; //ADO.NET classes
using System.Data;

namespace HRMSDataAccessLib
{
    public class HRMSDisconnectedDAL
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;

        public HRMSDisconnectedDAL()
        {
            //1. configure the connection
            con = new SqlConnection();
            con.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=CGDB;Integrated Security=True;";
            //2.Configure Select command
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from tbl_employee";
            //3.attach connection with command
            cmd.Connection = con;
            //4.configure DataAdapter using Command
            da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            //5. create and fill DataSet using DataAdapter
            ds=new DataSet();
            da.Fill(ds, "tbl_employee");

            //add primary key constraint
            ds.Tables[0].Constraints.Add("pk1", ds.Tables[0].Columns[0], true);
        }


        #region DML methods
        public int AddEmployee(Employee emp)
        {
            //1.create a new row in the DataSet Table
            var row = ds.Tables[0].NewRow();
            //2.specify the column value of this new row
            row[0] = emp.Ecode;
            row[1] = emp.Ename;
            row[2] = emp.Salary;
            row[3] = emp.Deptid;
            //3.Attach the row to the DataSet Table's Rows 
            ds.Tables[0].Rows.Add(row);
            //4.Save changes to Database
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.Update(ds, "tbl_employee");
            return 1;
        }
        public int DeleteById(int ecode)
        {
            try
            {
                //1.Find the row to be deleted in the DataSet Rows
                DataRow row = ds.Tables[0].Rows.Find(ecode);
                if (row != null)
                {
                    //2.Delete the row
                    row.Delete();
                    //3.Save changes to database
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(ds, "tbl_employee");
                    return 1;
                }
                else
                {
                    throw new Exception("Ecode does not exist, could not delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateEmployee(Employee emp)
        {
            try
            {
                //1.Find the row to be updated in the DataSet Rows
                DataRow row = ds.Tables[0].Rows.Find(emp.Ecode);
                if (row != null)
                {
                    //2.Update the row values
                    row[1] = emp.Ename;
                    row[2] = emp.Salary;
                    row[3] = emp.Deptid;
                    //3.Save changes to database
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    da.Update(ds, "tbl_employee");
                    return 1;
                }
                else
                {
                    throw new Exception("Ecode does not exist, could not update");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Query operations
        public List<Employee> GetAllEmps()
        {
            List<Employee> lstEmps = new List<Employee>();
            //traverse the rows in the DataSet Table and add them to the collection
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Employee emp = new Employee
                {
                    Ecode = (int)row[0],
                    Ename = row[1].ToString(),
                    Salary = (int)row[2],
                    Deptid = (int)row[3]
                };
                //add to the collection
                lstEmps.Add(emp);
            }

            return lstEmps;
        }
        public Employee GetEmpById(int ecode)
        {
            Employee emp = null;
            try
            {
                //1.Find the row in the DataSet Rows
                DataRow row = ds.Tables[0].Rows.Find(ecode);
                if (row != null)
                {
                    //2.Access the row and return the record
                    emp = new Employee
                    {
                        Ecode = (int)row[0],
                        Ename = row[1].ToString(),
                        Salary = (int)row[2],
                        Deptid = (int)row[3]
                    };
                }
                else
                {
                    throw new Exception("Ecode does not exist, could not find the record");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return the record
            return emp;
        }

        public List<Employee> GetEmpsByDeptid(int deptid)
        {
            List<Employee> lstEmps = new List<Employee>();
            //get the records by deptid  --where deptid=201
            DataRow[] rows =ds.Tables[0].Select("deptid=" + deptid);
            if(rows != null)
            {
                //add them to the collection
                foreach (DataRow row in rows)
                {
                    Employee emp = new Employee
                    {
                        Ecode = (int)row[0],
                        Ename = row[1].ToString(),
                        Salary = (int)row[2],
                        Deptid = (int)row[3]
                    };
                    //add to the collection
                    lstEmps.Add(emp);
                }
            }

            //return the records
            return lstEmps;
        }

        public List<Employee> GetEmpsBySalary(int minSal, int maxSal)
        {
            List<Employee> lstEmps = new List<Employee>();
            
            DataView v1 = new DataView();
            v1.Table = ds.Tables[0];
            v1.RowFilter = "salary>=" + minSal + " and salary<=" + maxSal;
            v1.Sort = "salary asc";

            //traverse the view and add the rows into the collection
           
            return lstEmps;
        }
        #endregion
    }
}
