using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class CallModel
    {
       IRepository<Calls> repository;
       
       public CallModel()
       {    // EmployeeModel to utilize the new repository
            repository = new HelpdeskRepository<Calls>();
       }

        // select a Calls by employee id
        public Calls GetById(int id)
        {
            List<Calls> selected = null;
            try
            {
                selected = repository.GetByExpression(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selected.FirstOrDefault() ;
        }

        // select all the employee
        public List<Calls> GetAll()
        {
            List<Calls> allCalls = null;
            try
            {
                allCalls = repository.GetAll();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allCalls;
        }

        public int Add(Calls newCall)
        {
            Calls addCall = null;
            try
            {
                addCall = repository.Add(newCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return addCall.Id;
        }

        public int Delete(int id)
        {
            int callDeleted = -1;

            try
            {
                callDeleted = repository.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return callDeleted;
        }

        public UpdateStatus Update(Calls updatedCall)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed; // enumerated values

            try
            {
                operationStatus = repository.Update(updatedCall);
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
    }
}
