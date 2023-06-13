using NUnit.Framework;
using dotnetapp.Models;
using dotnetapp.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace TestHotel
{
    [TestFixture]
    public class BloodDonorsTests
    {
        private BloodDonorController _controller;
         private BloodDonorDbContext _context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BloodDonorDbContext>()
                .UseInMemoryDatabase(databaseName: "DonorDatabase")
                .Options;
            _context = new BloodDonorDbContext(options);
            // seed the database with test data
            _context.BloodDonors.Add(new BloodDonor { Id = 101, Name = "Donor1",DOB="12/12/2000", BloodGroup="A+ve", Gender="Male", Location="Chennai", MobileNumber="9876543210", Email = "john@example.com" });
            _context.BloodDonors.Add(new BloodDonor { Id = 102, Name = "Donor2",DOB="1/12/2000", BloodGroup="B+ve", Gender="Female", Location="Chennai", MobileNumber="9876234210", Email = "jane@example.com" });
            _context.SaveChanges();
            // create an instance of the controller to test
          _controller = new BloodDonorController(_context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
[Test]
        public void Index_ReturnsViewWithListOfUsers()
        {
            // arrange
            // act
            var result = _controller.Index() as ViewResult;
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<System.Collections.Generic.List<BloodDonor>>(result.Model);
            Assert.AreEqual(2, (result.Model as System.Collections.Generic.List<BloodDonor>).Count);
        }

        [Test]
        public void Create_ReturnsView()
        {
            // arrange
            // act
            var result = _controller.Create() as ViewResult;
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_ValidDonor_RedirectsToIndex()
        {
            // arrange
            BloodDonor user = new BloodDonor { Name = "Donor2",DOB="1/12/2000", BloodGroup="B+ve", Gender="Female", Location="Chennai", MobileNumber="9876234210", Email = "test@example.com" };
            // act
            var result = _controller.Create(user) as RedirectToActionResult;
             // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", result.ActionName);
            // check if the user was added to the database
            Assert.AreEqual(3, _context.BloodDonors.Count());
            Assert.IsTrue(_context.BloodDonors.Any(u => u.Name == "Donor2" && u.Email == "test@example.com"));
        }       
    }
}
