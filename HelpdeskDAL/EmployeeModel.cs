using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class EmployeeModel
    {
       IRepository<Employees> repository;
       
       public EmployeeModel()
       {    // EmployeeModel to utilize the new repository
            repository = new HelpdeskRepository<Employees>();
       }

        // select a employee be employee email address
        public Employees GetByEmail(string email)
        {
            List<Employees> selectedEmployee = null;
            try
            {
                selectedEmployee = repository.GetByExpression(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selectedEmployee.FirstOrDefault();
        }

        public Employees GetByLastname(string name)
        {
            List<Employees> selectedEmployee = null;
            try
            {
                selectedEmployee = repository.GetByExpression(emp => emp.LastName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selectedEmployee.FirstOrDefault();
        }

        // select a employee by employee id
        public Employees GetById(int id)
        {
            List<Employees> selectedEmployee = null;
            try
            {
                selectedEmployee = repository.GetByExpression(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selectedEmployee.FirstOrDefault() ;
        }

        // select all the employee
        public List<Employees> GetAll()
        {
            List<Employees> allEmployees = null;
            try
            {
                allEmployees = repository.GetAll();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allEmployees;
        }

        // insert new employee
        public int Add(Employees newEmployee)
        {
            Employees addEmp = null;
            try
            {
                addEmp = repository.Add(newEmployee) ;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newEmployee.Id;
        }

        // update information of existing employee
        public UpdateStatus Update(Employees updatedEmployee)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed; // enumerated values

            try
            {
                operationStatus = repository.Update(updatedEmployee);
            }
            catch (DbUpdateConcurrencyException) // check for concurrency
            {
                operationStatus = UpdateStatus.Stale;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return operationStatus;
        }

        // delete existing employee
        public int Delete(int id)
        {
            int employeeDeleted = -1;

            try
            {
                employeeDeleted = repository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return employeeDeleted;
        }
    }
}
