using System;
using Dorywcza.Models;
using Dorywcza.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Dorywcza.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        #region Initial values
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOffer>().HasData(
                new
                {
                    JobOfferId = 1,
                    Name = "Praca budowlana na Zawodziu",
                    Description = "Praca na budowie sklepu spożywczego w 5-osobowym zespole",
                    Salary = 6000M,
                    TimeFrame = "12.12.2019 - 14.12.2019",
                    AmountOfPlaces = 1,
                    AddDate = DateTime.Today,
                    QualificationIsRequired = false,
                    State = true,
                    CategoryId = 1,
                    EmployeeId = 1,
                    EmployerId = 1
                });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    CategoryId = 1,
                    TypeOfJob = "Prace budowlane",
                    Workplace = "Katowice"
                },
                new Category()
                {
                    CategoryId = 2,
                    TypeOfJob = "Prace biurowe",
                    Workplace = "Katowice"
                },
                new Category()
                {
                    CategoryId = 3,
                    TypeOfJob = "Prace transportowe",
                    Workplace = "Katowice"
                },
                new Category()
                {
                    CategoryId = 4,
                    TypeOfJob = "Opieka",
                    Workplace = "Katowice"
                }
                );

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    EmployeeId = 1,
                    Name = "Jan Kowalski",
                    Gender = "M",
                    Age = 22,
                    Email = "example@example",
                    Phone = "+48 000 000 000",
                    Qualification = "Certyfikat QWERTY, Ukończone technikum budowlane",
                    Experience = "3 lata na budowie",
                    Comment = "Czy jest przerwa na piwo?",
                    AgreementRodo = true,
                });

            modelBuilder.Entity<Employer>().HasData(
                new Employer()
                {
                    EmployerId = 1,
                    CompanyName = "ConstructNext",
                    Description = "Zajmujemy się budową obiektów różnego przeznaczenia"
                });
        }
        #endregion
    }
}
