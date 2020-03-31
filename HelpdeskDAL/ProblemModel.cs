using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class ProblemModel
    {
        IRepository<Problems> repository;

        public ProblemModel()
        {    // ProblemModel to utilize the new repository
            repository = new HelpdeskRepository<Problems>();
        }

        // GetByDescription
        public Problems GetByDescription(string desc)
        {
            List<Problems> selected = null;
            try
            {
                selected = repository.GetByExpression(emp => emp.Description == desc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return selected.FirstOrDefault();
        }

        public Problems GetById(int id)
        {
            List<Problems> selected = null;
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

            return selected.FirstOrDefault();
        }

        // select all the employee
        public List<Problems> GetAll()
        {
            List<Problems> allProblems = null;
            try
            {
                allProblems = repository.GetAll();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allProblems;
        }

    }
}
