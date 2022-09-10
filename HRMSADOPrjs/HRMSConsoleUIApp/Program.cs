using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMSEntitiesLib; //for Entities classes
using HRMSBusinessLib; //for Business Layer classes

namespace HRMSConsoleUIApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice = 0;
            do
            {
                Console.WriteLine("1.Add Employee");
                Console.WriteLine("2.Delete Employee");
                Console.WriteLine("3.Update Employee");
                Console.WriteLine("4.Display All Employees");
                Console.WriteLine("5.Find Employee By Id");
                Console.WriteLine("6.Update using Stored Procedure");
                Console.WriteLine("7.Exit");
                Console.WriteLine("8.GetBonus - Parameter direction");
                Console.WriteLine("9.DoTransaction");

                Console.Write("Enter choice:");
                choice=int.Parse(Console.ReadLine());

                switch(choice)
                {
                    case 1:
                        //Add record
                        AddEmployee();
                        break;
                    case 2:
                        //delete the record
                        DeleteEmployee();
                        break;
                    case 3:
                        //update the record
                        UpdateEmployee();
                        break;
                    case 4:
                        //display all records
                        DisplayAll();
                        break;
                    case 5:
                        //search record by id
                        GetEmpById();
                        break;
                    case 6:
                        //update using Stored Procedure
                        UpdataUsingStoredProcedure();
                        break;
                    case 7:
                        //exit the app
                        break;
                    case 8:
                        Console.Write("Enter salary to get bonus:");
                        int salary = int.Parse(Console.ReadLine());
                        var bll=new HRMSBusinessLayer();
                        var bonus = bll.GetBonus(salary);
                        Console.WriteLine("Salary:{0}\tBonus:{1}",salary,bonus);
                        break;
                    case 9:
                        DoTransaction();
                        break;
                    default:
                        Console.WriteLine("invalid choice!!!");
                        break;
                }

            } while (choice!=7);
        }
        static void AddEmployee()
        {
            try
            {
                //Take employee input data
                Employee emp = new Employee();
                Console.Write("Enter ecode:");
                emp.Ecode = int.Parse(Console.ReadLine());
                Console.Write("Enter ename:");
                emp.Ename = Console.ReadLine();
                Console.Write("Enter salary:");
                emp.Salary = int.Parse(Console.ReadLine());
                Console.Write("Enter deptid:");
                emp.Deptid = int.Parse(Console.ReadLine());

                //insert using Business Layer
                HRMSBusinessLayer bll = new HRMSBusinessLayer();
                int count = bll.AddEmployee(emp);
                Console.WriteLine("Record inserted successfully, records affected:" + count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void DisplayAll()
        {
            HRMSBusinessLayer bll = new HRMSBusinessLayer();
            var lstEmps = bll.GetAllEmps();
            if(lstEmps.Count!=0)
            {
                foreach (var e in lstEmps)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}",e.Ecode,e.Ename,e.Salary,e.Deptid);
                }
            }
            else
            {
                Console.WriteLine("No employee records in database table");
            }
        }
        static void DeleteEmployee()
        {
            try
            {
                //Take input for ecode for deletion
                Console.Write("Enter ecode for delete:");
                int ecode=int.Parse(Console.ReadLine());
                //delete using Business Layer
                HRMSBusinessLayer bll = new HRMSBusinessLayer();
                var count = bll.DeleteEmpById(ecode);
                Console.WriteLine("Record deleted, rows affected:" + count);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void UpdateEmployee()
        {
            try
            {
                //Take employee input data
                Employee emp = new Employee();
                Console.Write("Enter ecode for update:");
                emp.Ecode = int.Parse(Console.ReadLine());
                Console.Write("Enter ename:");
                emp.Ename = Console.ReadLine();
                Console.Write("Enter salary:");
                emp.Salary = int.Parse(Console.ReadLine());
                Console.Write("Enter deptid:");
                emp.Deptid = int.Parse(Console.ReadLine());

                //update using Business Layer
                HRMSBusinessLayer bll = new HRMSBusinessLayer();
                int count = bll.UpdateEmployee(emp);
                Console.WriteLine("Record updated successfully, records affected:" + count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void GetEmpById()
        {
            try
            {
                //Take ecode input for searching employee
                Console.Write("Enter ecode for search:");
                int ecode = int.Parse(Console.ReadLine());
                //get the employee record using Business Layer
                var bll = new HRMSBusinessLayer();
                var emp = bll.GetEmpById(ecode);
                if(emp!=null)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}",emp.Ecode,emp.Ename,emp.Salary,emp.Deptid);
                }
                else
                {
                    Console.WriteLine("No record found");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void UpdataUsingStoredProcedure()
        {
            try
            {
                //Take input for ecode for update
                Console.Write("Enter ecode:");
                int ecode=int.Parse(Console.ReadLine());
                //Update using Business Layer(SP)
                var bll = new HRMSBusinessLayer();
                bll.UpdateUsingSP(ecode);
                Console.WriteLine("Salary updated using stored procedure");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void DoTransaction()
        {
            try
            {
                var businessLayer = new HRMSBusinessLayer();
                businessLayer.DoTransaction();
                Console.WriteLine("Transaction successfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transaction failed");
                Console.WriteLine(ex.Message);
            }
        }
    }

}
