using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bsnsapp.Models;
using Bsnsapp.Attributes;
using Newtonsoft.Json;


namespace Bsnsapp.Models
{
    public class Mov
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(".+\\@.+\\..+",ErrorMessage="Enter Valid Email")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-Mail Address")]
        [Remote("DisallowName","Vendor", HttpMethod="POST")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Name")]
        [RegularExpression(@"A-Z",ErrorMessage="Only string please!..")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Address Line1")]
        public string Address1 { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Address Line2")]
        public string Address2 { get; set; }
        
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string Country { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        
        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        public string Dob { get; set; }

        [Required]
        [StringLength(10, ErrorMessage="Password Must Be Minimum 6 Charachters And Maximum 10 Characters", MinimumLength=6)]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Retype New Password")]
        [DataType(DataType.Password)]
        [SameAs("Password",ErrorMessage="Both Password Should Match")]
        public string RetypePassword { get; set; }

        public List<State> StateModel { get; set; }
        public SelectList FilteredCity { get; set; }

       
    }
    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }
    public class City
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }

        [Required]  
        [DisplayName("Category Name")]
        public string Categories { get; set; }

        [Required]
        [DisplayName("Sub Category Name")]
        public string Sub_Categories { get; set; }
    }
   
   
   
    
}