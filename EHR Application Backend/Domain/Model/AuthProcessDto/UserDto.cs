using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AuthProcessDto
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IFormFile ProfileImageUrl { get; set; }// comes from feature package
        public string Gender { get; set; }
       
        public int BloodGroupId { get; set; }

        public string Address { get; set; }
        public int CityId { get; set; }

        public int StateId { get; set; }

        public string Mobile { get; set; }
        public int CountryId { get; set; }

        public string PinCode { get; set; }
   
        public int UserTypeId { get; set; }

        public string? Qualification { get; set; }//below for provider
   
        public int? SpecialisationId { get; set; }

        public string? RegistrationNumber { get; set; }
        public float? VisitingCharge { get; set; }
    }
}
