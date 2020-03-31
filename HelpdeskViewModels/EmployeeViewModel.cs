using System;
using HelpdeskDAL;
using System.Reflection;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private EmployeeModel _model;

        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Id { get; set; }
        public string Picture64 { get; set; }
        public bool? IsTech { get; set; }

        public EmployeeViewModel()
        {
            _model = new EmployeeModel();
        }

        public void GetByEmail()
        {
            try
            {
                Employees emp = _model.GetByEmail(Email);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;

                if (emp.StaffPicture != null)
                {
                    Picture64 = Convert.ToBase64String(emp.Timer);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Email = "not found";
            }
            catch (Exception ex)
            {
                Email = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // select a employee be employee email address
        public void GetById()
        {
            try
            {
                // get employee to emp(object)
                Employees emp = _model.GetById(Id);
                //set elements
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;

                if (emp.StaffPicture  != null)
                {
                    Picture64 = Convert.ToBase64String(emp.Timer);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // select all the employee
        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employees> allEmployees = _model.GetAll();
                foreach (Employees emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.Firstname = emp.FirstName;
                    empVm.Lastname = emp.LastName;
                    empVm.Phoneno = emp.PhoneNo;
                    empVm.Email = emp.Email;
                    empVm.Id = emp.Id;
                    empVm.DepartmentId = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.Timer = Convert.ToBase64String(emp.Timer);
                    empVm.IsTech = emp.IsTech ?? false;

                    if (emp.StaffPicture != null)
                    {
                        empVm.Picture64 = Convert.ToBase64String(emp.StaffPicture);
                    }

                    allVms.Add(empVm);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allVms;
        }

        // insert new employee
        public void Add()
        {
            Id = -1;
            try
            {
                Employees emp= new Employees();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = DepartmentId;
                //emp.StaffPicture = Picture64;

                if (Picture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(Picture64);
                }

                //if (emp.StaffPicture != null)
                //{
                //    empVm.Picture64 = Convert.ToBase64String(emp.StaffPicture);
                //}


                Id = _model.Add(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // update information of existing employee
        public int Update()
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                Employees emp= new Employees();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.Id = Id;
                emp.DepartmentId = DepartmentId;
                if (Picture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(Picture64);
                }

                emp.Timer = Convert.FromBase64String(Timer);
                operationStatus = _model.Update(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(operationStatus);
        }

        // delete existing employee
        public int Delete()
        {
            int employeeDeleted = -1;

            try
            {
                employeeDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return employeeDeleted;
        }
    }
}
