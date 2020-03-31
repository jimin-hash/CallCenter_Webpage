using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskViewModels;
using Xunit;
using Xunit.Abstractions;

namespace HelpdeskTests
{
    public class EmployeeViewModelTests
    {
        private readonly ITestOutputHelper output;

        public EmployeeViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Employee_GetByEmailTest()
        {
            EmployeeViewModel em = new EmployeeViewModel();
            em.Email = "bs@abc.com";
            em.GetByEmail();
            Assert.NotNull(em.Email);
        }

        [Fact]
        public void Employee_GetByIdTest()
        {
            EmployeeViewModel em = new EmployeeViewModel();
            em.Id = 10;
            em.GetById();
            Assert.True(em.Id > 0);
        }

        [Fact]
        public void Employee_GetByAllTest()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            EmployeeViewModel em = new EmployeeViewModel();
            allVms = em.GetAll();
            Assert.NotNull(allVms);
        }

        [Fact]
        public void Employee_AddTest()
        {
            EmployeeViewModel em = new EmployeeViewModel();
            em.Firstname = "Jimin";
            em.Lastname = "Park";
            em.Phoneno = "(111)000-0000";
            em.Title = "Ms.";
            em.DepartmentId = 100;
            em.Email = "jm@abc.com";

            em.Add();
            Assert.True(em.Id > 0);
        }

        [Fact]
        public void Employee_UpdateTest()
        {
            EmployeeViewModel em = new EmployeeViewModel();
            em.Email = "harryP@someschool.com";
            em.GetByEmail();
            em.Phoneno = em.Phoneno == "(111)000-0000" ? "(333)222-1111" : "(111)000-0000";

            Assert.True(em.Update() > 0);
        }

        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Id = 13;
            vm2.Id= 13;
            vm1.GetById();
            vm2.GetById();
            vm1.Email = (vm1.Email.IndexOf(".ca") > 0 ? "el@abc.com" : "el@abc.ca");

            if (vm1.Update() == 1)
            {
                vm2.Email = "something@different.com";
                Assert.True(vm2.Update() == -2);
            }
            else
                Assert.True(false);
        }

        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeViewModel em = new EmployeeViewModel();
            em.Email = "harryP@someschool.com";
            em.GetByEmail();
            int employeeDelete = em.Delete();
            Assert.True(employeeDelete > 0);
        }

        [Fact]
        public void Call_ComprehensiveVMTest()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();

            cvm.DateOpened = DateTime.Now;
            //cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Email = "jm@shd.com";
            evm.GetByEmail();
            cvm.EmployeeId = evm.Id;
            //evm.Lastname = "Park";
            evm.Email = "sj@abc.com";
            evm.GetByEmail();
            cvm.TechId = evm.Id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Jimin has bad RAM, Burner to fix it";
            cvm.Add();
            output.WriteLine("New Call Generated - Id = "+cvm.Id);

            int id = cvm.Id;
            cvm.GetById();
            cvm.Notes += "\n Ordered new RAM!!";

            if (cvm.Update() == 1)
            {
                output.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }

            //cvm.Notes = "Another change to comments that should not works";
            //if (cvm.Update() == -2) {
            //    output.WriteLine("Call was not updated data was stale");
            //}

            //cvm = new CallViewModel();
            //cvm.Id = id;
            //cvm.GetById();

            //if (cvm.Delete() == 1)
            //{
            //    output.WriteLine("Call was deleted!");
            //}
            //else {
            //    output.WriteLine("Call was not deleted!");
            //}

            //Exception ex = Assert.Throws<NullReferenceException>(() => cvm.GetById());
            //Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
        }
    }
}
