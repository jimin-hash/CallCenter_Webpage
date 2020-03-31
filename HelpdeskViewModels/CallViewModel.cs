using System;
using HelpdeskDAL;
using System.Reflection;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private CallModel _model;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public int TechId { get; set; }
        public string EmployeeName { get; set; }
        public string TechName { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }


        public CallViewModel()
        {
            _model = new CallModel();
        }

        
        // select a employee be employee email address
        public void GetById()
        {
            try
            {
                Calls c = _model.GetById(Id);

                Id = c.Id;
                EmployeeId = c.EmployeeId;
                ProblemId = c.ProblemId;
                TechId = c.TechId;
                //EmployeeName = c.Employee;
                //TechName = c.Tech;
                //ProblemDescription = c
                DateOpened = c.DateOpened;
                DateClosed = c.DateClosed;
                OpenStatus = c.OpenStatus;
                Notes = c.Notes;
                Timer = Convert.ToBase64String(c.Timer);
            }
            catch (NullReferenceException nex)
            {
                //Id = "not found";
            }
            catch (Exception ex)
            {
                //Id = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // select all the calls
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                List<Calls> allCalls = _model.GetAll();
                foreach (Calls c in allCalls)
                {
                    CallViewModel cVm = new CallViewModel();
                    EmployeeViewModel evm = new EmployeeViewModel();
                    ProblemViewModel pvm = new ProblemViewModel();

                    cVm.Id = c.Id;
                    cVm.EmployeeId = c.EmployeeId;
                    cVm.ProblemId = c.ProblemId;
                    cVm.TechId = c.TechId;
                    cVm.DateOpened = c.DateOpened;
                    cVm.DateClosed = c.DateClosed;
                    cVm.OpenStatus = c.OpenStatus;
                    cVm.Notes = c.Notes;
                    cVm.Timer = Convert.ToBase64String(c.Timer);

                    evm.Id = c.EmployeeId;
                    evm.GetById();
                    cVm.EmployeeName = evm.Lastname;

                    evm.Id = c.TechId;
                    evm.GetById();
                    cVm.TechName = evm.Lastname;

                    pvm.Id = c.ProblemId;
                    pvm.GetById();
                    cVm.ProblemDescription= pvm.Description;

                    allVms.Add(cVm);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allVms;
        }

        public void Add()
        {
            Id = -1;
            try
            {

                Calls c = new Calls();

                c.EmployeeId = EmployeeId;
                c.ProblemId = ProblemId;
                c.TechId = TechId;
                c.DateOpened = DateOpened;
                c.DateClosed = DateClosed;
                c.OpenStatus = OpenStatus;
                c.Notes = Notes;

                Id = _model.Add(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public int Delete()
        {
            int callsDeleted = -1;

            try
            {
                callsDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return callsDeleted;
        }

        public int Update()
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                Calls c = new Calls();

                c.Id = Id;
                c.EmployeeId = EmployeeId;
                c.ProblemId = ProblemId;
                c.TechId = TechId;
                c.DateOpened = DateOpened;
                c.DateClosed = DateClosed;
                c.OpenStatus = OpenStatus;
                c.Notes = Notes;
                c.Timer = Convert.FromBase64String(Timer);

                operationStatus = _model.Update(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(operationStatus);
        }
    }
}
