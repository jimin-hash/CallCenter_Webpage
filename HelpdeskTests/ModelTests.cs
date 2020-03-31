using System;
using HelpdeskDAL;
using Xunit;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace HelpdeskTests
{
    public class ModelTests
    {
        private readonly ITestOutputHelper output;

        public ModelTests(ITestOutputHelper output) {
            this.output = output;
        }
        [Fact]
        public void Employee_GetByEmailTest()
        {
            EmployeeModel model = new EmployeeModel();
            Employees employee = model.GetByEmail("bs@abc.com");
            Assert.NotNull(employee);
        }

        [Fact]
        public void Employee_GetById()
        {
            EmployeeModel model = new EmployeeModel();
            Employees employee = model.GetById(1);
            Assert.NotNull(employee);
        }

        [Fact]
        public void Employee_GetAll()
        {
            EmployeeModel model = new EmployeeModel();
            List<Employees> allEmployees = new List<Employees>();
            allEmployees = model.GetAll();
            Assert.NotNull(allEmployees);
        }

        [Fact]
        public void Employee_Add()
        {
            EmployeeModel model = new EmployeeModel();
            Employees newEmployee = new Employees();
            newEmployee.FirstName = "Harry";
            newEmployee.LastName = "Park";
            newEmployee.PhoneNo = "(555)000-0000";
            newEmployee.Title = "Mrs.";
            newEmployee.DepartmentId = 100;
            newEmployee.Email = "harryP@someschool.com";

            int newEmpId = model.Add(newEmployee);
            Assert.True(newEmpId > 1);
        }

        [Fact]
        public void Employee_Update()
        {
            EmployeeModel model = new EmployeeModel();
            Employees currentEmployee = model.GetById(13);

            if (currentEmployee != null)
            {
                string oldPhoneNo = currentEmployee.PhoneNo;
                string newphoneNo = oldPhoneNo == "(555)000-0000" ? "(222)000-0000" : "(555)000-0000";
                currentEmployee.PhoneNo = newphoneNo;
            }
            Assert.True(model.Update(currentEmployee) == UpdateStatus.Ok);
        }

        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeModel model1 = new EmployeeModel();
            EmployeeModel model2 = new EmployeeModel();
            Employees employeeForUpdate1 = model1.GetByEmail("jm@abc.com");
            Employees employeeForUpdate2 = model2.GetByEmail("jm@abc.com");

            if (employeeForUpdate1 != null)
            {
                string oldPhoneNo = employeeForUpdate1.PhoneNo;
                string newPhoneNo = oldPhoneNo == "(222)000-0000" ? "(333)000-0000" : "(222)000-0000";
                employeeForUpdate1.PhoneNo = newPhoneNo;
                if (model1.Update(employeeForUpdate1) == UpdateStatus.Ok)
                {
                    //need to change the phone # to something else
                    employeeForUpdate2.PhoneNo = "666-666-6666";
                    Assert.True(model2.Update(employeeForUpdate2) == UpdateStatus.Stale);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public void Employee_Delete()
        {
            EmployeeModel model = new EmployeeModel();
            int employeeDeleted = model.Delete(12);
            Assert.True(employeeDeleted == 1);
        }

        [Fact]
        public void Employee_LoadPicsTest()
        {
            DALUtil util = new DALUtil();
            Assert.True(util.AddStudentPicsToDb());
        }

        [Fact]
        public void Call_ComprehensiveTest() {
            CallModel cmodel = new CallModel();
            EmployeeModel emodel = new EmployeeModel();
            ProblemModel pmodel = new ProblemModel();
            Calls call = new Calls();

            call.DateOpened = DateTime.Now;
            //call.DateClosed = null;
            call.OpenStatus = true;
            call.EmployeeId = emodel.GetByLastname("Park").Id;
            call.TechId = emodel.GetByLastname("Burner").Id;
            call.ProblemId = pmodel.GetByDescription("Hard Drive Failure").Id;
            call.Notes = "Jimin's drive is shot, Burner to fix it";

            int newCallId = cmodel.Add(call);
            output.WriteLine("New Call Generated - Id = " + newCallId);
            call = cmodel.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            output.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new drive!";

            if (cmodel.Update(call) == UpdateStatus.Ok)
            {
                output.WriteLine("Call was updated " + call.Notes);
            }
            else {
                output.WriteLine("Call was not updated!");
            }

            call.Timer = oldtimer;
            call.Notes = "doesn't matter data is stale now";
            if (cmodel.Update(call) == UpdateStatus.Stale) {
                output.WriteLine("Call was not updated due to stale data");
            }

            cmodel = new CallModel();
            call = cmodel.GetById(newCallId);

            if (cmodel.Delete(newCallId) == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else {
                output.WriteLine("Call was noe deleted");
            }
            Assert.Null(cmodel.GetById(newCallId));
        }
    }
}
