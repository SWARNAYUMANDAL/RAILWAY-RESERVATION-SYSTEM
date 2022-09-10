using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRMSEntitiesLib; //for Entities Layer
using System.Data.SqlClient; //for ADO.NET classes 
using System.Data;

namespace HRMSDataAccessLib
{
    public class HRMSDataAccessLayer
    {
        SqlConnection con;

        public HRMSDataAccessLayer()
        {
            //1. configure the connection
            con = new SqlConnection();
            //con.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=CGDB;Integrated Security=True;";
            con.ConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=CGDB;Integrated Security=True;";
        }

        #region DML methods
        public int AddEmployee(Employee emp)
        {
            int recordsAffected = 0;
            try
            {
                //INSERT operation 
                //2. Configure the SqlCommand for INSERT statement
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                //3. specify the command text for INSERT
                cmd.CommandText = "insert into tbl_employee values(@ec,@en,@sal,@did)";
                //4. specify the values for the parameters
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ec", emp.Ecode);
                cmd.Parameters.AddWithValue("@en", emp.Ename);
                cmd.Parameters.AddWithValue("@sal", emp.Salary);
                cmd.Parameters.AddWithValue("@did", emp.Deptid);
                //5. open the connection
                con.Open();
                //6. attach the connection with the command
                cmd.Connection = con;
                //7. execute the command 
                recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected == 0)
                {
                    throw new Exception("Could not insert record");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //8. close the connection
                con.Close();
            }        

            return recordsAffected;           
        }
        public int DeleteById(int ecode)
        {
            int recordsAffected = 0;
            try
            {
                //2. configure the command for delete 
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from tbl_employee where ecode=@ec";
                //3.specify the parametr value
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ec", ecode);
                //attach connection with command
                cmd.Connection = con;
                //open connection 
                con.Open();
                //execute the command
                recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected == 0)
                {
                    throw new Exception("Ecode does not exist, no record deleted");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //close the connection
                con.Close();
            }

            return recordsAffected;
        }
        public int UpdateEmployee(Employee emp)
        {
            int recordsAffected = 0;
            try
            {
                //INSERT operation 
                //2. Configure the SqlCommand for UPDATE statement
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                //3. specify the command text for UPDATE
                cmd.CommandText = "update tbl_employee set ename=@en,salary=@sal,deptid=@did where ecode=@ec";
                //4. specify the values for the parameters
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ec", emp.Ecode);
                cmd.Parameters.AddWithValue("@en", emp.Ename);
                cmd.Parameters.AddWithValue("@sal", emp.Salary);
                cmd.Parameters.AddWithValue("@did", emp.Deptid);
                //5. open the connection
                con.Open();
                //6. attach the connection with the command
                cmd.Connection = con;
                //7. execute the command 
                recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected == 0)
                {
                    throw new Exception("Could not update record");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //8. close the connection
                con.Close();
            }

            return recordsAffected;
        }
        #endregion

        #region Query operations
        public List<Employee> GetAllEmps()
        {
            List<Employee> lstEmps = new List<Employee>();

            //2. configure the command for SELECT ALL
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType=CommandType.Text;
            cmd.CommandText = "select * from tbl_employee";
            //attach connection
            cmd.Connection = con;
            //open connection
            con.Open();
            //execute the command
            SqlDataReader sdr = cmd.ExecuteReader();
            //traverse the record and add them to the collection
            while(sdr.Read())
            {
                Employee emp = new Employee
                {
                    Ecode=(int)sdr[0],
                    Ename=sdr[1].ToString(),
                    Salary=(int)sdr[2],
                    Deptid=(int)sdr[3]
                };
                //add it to the collection
                lstEmps.Add(emp);
            }
            //close the reader 
            sdr.Close();
            //close the connection
            con.Close();

            //return the records
            return lstEmps;
        }
        public Employee GetEmpById(int ecode)
        {
            Employee emp = null;
            try
            {
                //2. Get the employee record by ecode
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from tbl_employee where ecode=@ec";
                //specify the parameter value
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ec", ecode);
                //attach the connection with command
                cmd.Connection = con;
                //open connection and execute command
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if(sdr.Read())
                {
                    emp = new Employee
                    {
                        Ecode=(int)sdr[0],
                        Ename=sdr[1].ToString(),
                        Salary=(int)sdr[2],
                        Deptid=(int)sdr[3]
                    };
                    sdr.Close();
                }
                else
                {
                    throw new Exception("No record found, ecode does not exist");                    
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                //close the connection
                con.Close();
            }

            return emp;
        }
        #endregion

        #region Stored Procedures calling
        public void UpdateEmpUsingSP(int ecode)
        {
            try
            {
                //2. configure the command for stored procedure
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_updatesalary";
                //3.configure the parameter needed by the stored procedure
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ec", ecode);
                //4.attach connection
                cmd.Connection = con;
                //5.open the connection
                con.Open();
                //6.execute the command
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                //close the connection
                con.Close();
            }
        }   

        public double GetBonus(int salary)
        {
            double bonus = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType=CommandType.StoredProcedure;
            cmd.CommandText = "sp_getbonus";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@sal", salary);
            cmd.Parameters.AddWithValue("@bonus", bonus);

            //specify the parameter direction
            cmd.Parameters[1].Direction = ParameterDirection.InputOutput;

            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery(); //procedure is executed and bonus gets modified
                                   
            //get the value of the bonus out of procedure
            bonus=(double)cmd.Parameters[1].Value;

            con.Close();

            return bonus;

        }
        #endregion

        public void DoTransaction()
        {
            SqlTransaction T=null;

            try
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "update tbl_employee set salary=salary + 1000 where ecode=101";
                cmd1.CommandType = CommandType.Text;

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "insert into tbl_employee values(104,'Ramnath',4444,201)";
                cmd2.CommandType = CommandType.Text;

                cmd1.Connection = con;
                cmd2.Connection = con;
                
                con.Open();
                //Begin the transaction with the Connection object
                T= con.BeginTransaction();
                //attach all the commands with the transaction
                cmd1.Transaction = T;
                cmd2.Transaction = T;

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                T.Commit();
            }
            catch (Exception e)
            {
                T.Rollback();
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
