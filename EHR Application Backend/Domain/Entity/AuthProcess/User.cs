using Domain.Entity.Register;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.AuthProcess
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfileImageUrl { get; set; }// comes from feature package
        public string Gender { get; set; }
        [ForeignKey("BloodGroup")]
        public int? BloodGroupId { get; set; }
        public string  Address { get; set; }
        public int CityId { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public string PinCode { get; set; }
        [ForeignKey("UserType")]
        public int UserTypeId { get; set; }
  
        public string? Qualification { get; set; }//below for provider
        [ForeignKey("Specialisation")]
        public int? SpecialisationId { get; set; }
      
        public string? RegistrationNumber { get; set; }
        public float VisitingCharge { get; set; }
        public bool isDeleted { get; set; }
        public Specialisation Specialisation { get; set; }
        public UserType UserType { get; set; }
        public BloodGroup BloodGroup { get; set; }
    }
}
