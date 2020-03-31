using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace HelpdeskDAL
{
    public class DepartmentModel
    {
        IRepository<Departments> repository;

        public DepartmentModel()
        {
            repository = new HelpdeskRepository<Departments>();
        }

        public List<Departments> GetAll()
        {
            List<Departments> allDepartments = null;

            try
            {
                allDepartments = repository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allDepartments;
        }
    }
}
