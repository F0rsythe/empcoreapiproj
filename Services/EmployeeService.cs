using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using empcoreapiproj.Data;
using empcoreapiproj.Models;
using empcoreapiproj.ViewModel;

namespace empcoreapiproj.Services
{
    public class EmployeeService
    {
        public ApplicationDbContext context;
        public EmployeeService(ApplicationDbContext con)
        {
            context = con;
        }

        public void AddEmployee(EmployeeVm vm)
        {
            var emp = new Employee()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Department = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                Role = vm.Role
            };
            context.Employees.Add(emp);
            context.SaveChanges();

        }
        public List<Employee> GetEmployee()
        {
            var res = context.Employees.ToList();
            return res;
        }
        public Employee GetEmployeeById(int id)
        {
            var res = context.Employees.FirstOrDefault(x => x.Id == id);
            return res;
        }
        public Employee UpdateEmployeeById(int id, EmployeeVm vm)
        {
            var res = context.Employees.FirstOrDefault(x => x.Id == id);
            if (res != null)
            {
                res.FirstName = vm.FirstName;
                res.LastName = vm.LastName;
                res.DateOfBirth = vm.DateOfBirth;
                res.Role = vm.Role;
                res.Department = vm.Department;
                context.SaveChanges();

            }
            return (res);
        }
        public void DeleteEmployeebyId(int id)
        {
            var delete = context.Employees.FirstOrDefault(x => x.Id == id);
            if (delete != null)
            {
                context.Employees.Remove(delete);
                context.SaveChanges();
            }
        }
    }
}