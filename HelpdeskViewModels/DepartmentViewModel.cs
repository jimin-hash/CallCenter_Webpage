using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private DepartmentModel _model;

        public string Timer { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        // constructor
        public DepartmentViewModel()
        {
            _model = new DepartmentModel();
        }

        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Departments> allDeparments = _model.GetAll();
                foreach (Departments div in allDeparments)
                {
                    DepartmentViewModel divVm = new DepartmentViewModel();
                    divVm.Id = div.Id;
                    divVm.Name = div.DepartmentName;
                    divVm.Timer = Convert.ToBase64String(div.Timer);
                    allVms.Add(divVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }
    }
}
