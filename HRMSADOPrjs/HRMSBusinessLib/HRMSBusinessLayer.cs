using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRMSEntitiesLib; //for Entities classes
using HRMSDataAccessLib; //for DAL classes

namespace HRMSBusinessLib
{
    public class HRMSBusinessLayer
    {
        public int AddEmployee(Employee emp)
        {
            //HRMSDataAccessLayer dal = new HRMSDataAccessLayer();
            HRMSDisconnectedDAL dal = new HRMSDisconnectedDAL();
            var count = dal.AddEmployee(emp);
            return count;
        }
        public int DeleteEmpById(int ecode)
        {
            //HRMSDataAccessLayer dal = new HRMSDataAccessLayer();
            HRMSDisconnectedDAL dal = new HRMSDisconnectedDAL();
            var count=dal.DeleteById(ecode);
            return count;
        }
        public int UpdateEmployee(Employee emp)
        {
            //HRMSDataAccessLayer dal = new HRMSDataAccessLayer();
            HRMSDisconnectedDAL dal = new HRMSDisconnectedDAL();
            var count = dal.UpdateEmployee(emp);
            return count;
        }

        public List<Employee> GetAllEmps()
        {
            //HRMSDataAccessLayer dal = new HRMSDataAccessLayer();
            HRMSDisconnectedDAL dal = new HRMSDisconnectedDAL();
            var lstEmps = dal.GetAllEmps();
            return lstEmps;
        }
        public Employee GetEmpById(int ecode)
        {
            //var dal = new HRMSDataAccessLayer();
            var dal = new HRMSDisconnectedDAL();
            var emp = dal.GetEmpById(ecode);
            return emp;
         }

        public void UpdateUsingSP(int ecode)
        {
            var dal = new HRMSDataAccessLayer();
            dal.UpdateEmpUsingSP(ecode);
        }
        public double GetBonus(int salary)
        {
            var dal = new HRMSDataAccessLayer();
            var bonus=dal.GetBonus(salary);
            return bonus;
        }

        public void DoTransaction()
        {
            var dal=new HRMSDataAccessLayer();
            dal.DoTransaction();
        }
    }
}
