using System;
using HelpdeskDAL;
using System.Reflection;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private ProblemModel _model;

        public string Timer { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }


        public ProblemViewModel()
        {
            _model = new ProblemModel();
        }

        public void GetByDescription()
        {
            try
            {
                Problems p = _model.GetByDescription(Description);
                Id = p.Id;
                Description = p.Description;

                Timer = Convert.ToBase64String(p.Timer);
            }
            catch (NullReferenceException nex)
            {
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        
        // select all the employee
        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                List<Problems> allProblems = _model.GetAll();
                foreach (Problems p in allProblems)
                {
                    ProblemViewModel pVm = new ProblemViewModel();
                    pVm.Id = p.Id;
                    pVm.Description = p.Description;
                    pVm.Timer = Convert.ToBase64String(p.Timer);

                    allVms.Add(pVm);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allVms;
        }

        public void GetById()
        {
            try
            {
                Problems p = _model.GetById(Id);
                Id = p.Id;
                Description = p.Description;

                Timer = Convert.ToBase64String(p.Timer);
            }
            catch (NullReferenceException nex)
            {
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}
