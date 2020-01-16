using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dorywcza.Models
{
    public class JobOffer
    {
        public int JobOfferId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }
        public string TimeFrame { get; set; }
        public int AmountOfPlaces { get; set; }
        public DateTime AddDate { get; set; }
        public bool QualificationIsRequired { get; set; }
        public bool State { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        
        public List<JobOfferEmployee> JobOfferEmployees { get; set; }

        public List<Employee> Employees()
        {
            var employees = new List<Employee>();

            foreach (var join in JobOfferEmployees)
            {
                employees.Add(join.Employee);
            }

            return employees;
        }

        public JobOffer()
        {
            JobOfferEmployees = new List<JobOfferEmployee>();
        }
    }
}
