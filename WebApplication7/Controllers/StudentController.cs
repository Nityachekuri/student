using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class StudentController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index()
        {
            using (var db = new SqlConnection(connectionString))
            {
                IEnumerable<Student> students = db.Query<Student>("SELECT * FROM Students");
                return View(students);
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                Student student = db.QuerySingleOrDefault<Student>("SELECT * FROM Students WHERE StudentID = @StudentID", new { StudentID = id });
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Students (FirstName, LastName, DateOfBirth, Gender, Address, Email, Phone) 
                                 VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Address, @Email, @Phone)";
                    db.Execute(query, student);
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                Student student = db.QuerySingleOrDefault<Student>("SELECT * FROM Students WHERE StudentID = @StudentID", new { StudentID = id });
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Students SET FirstName = @FirstName, LastName = @LastName, 
                             DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address, 
                             Email = @Email, Phone = @Phone WHERE StudentID = @StudentID";
                    db.Execute(query, student);
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                db.Execute(query, new { StudentID = id });
            }
            return RedirectToAction("Index");
        }


    }
}