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
       
    public enum Category1
    {


    Entertainment,
    Gaming,Lifestyle,Offbeat,Politics,Science,Sports,Technology,WorldNews


    }

    public enum subCategory1
    { 
    
   A,B,C 
    
    }
    public class catlist
    {
        public int Id { get; set; }
        public string catname { get; set; }
        //public IEnumerable<Vendor> Divisions { get; set; }
    }
    public class Vendor
    {
        public string EMailUser { get; set; }
        public int Id { get; set; }
        //[Required]
        [Required]
        [DisplayName("Business Titles")]
        public string BusinessTitles { get; set; }
        [DataType(DataType.Text)]

        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DataType(DataType.Text)]

        [Required]
        [DisplayName("Category")]
        public string Catgry { get; set; }

        [Required]
        [DisplayName("Sub Category")]
        public subCategory1 SubCatgry { get; set; }

        [Required(ErrorMessage = "Please enter your valid Email_id")]
        [DisplayName("Email Id")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter your valid email which contains the @ Sign")]
        [DataType(DataType.EmailAddress)]
       // public States Email { get; set; }
        public string  Email { get; set; }

        [Required]
        [DisplayName("Name")]
        [RegularExpression(@"A-Z", ErrorMessage = "Only string please!..")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Address1")]
        [DataType(DataType.MultilineText)]
        public string Address1 { get; set; }

        [Required]
        [DisplayName("Address2")]
        [DataType(DataType.MultilineText)]
        public string Address2 { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        //[Required]
        //[DisplayName("State")]
        public string State { get; set; }

        [Required]
        [DisplayName("Pin")]
        [RegularExpression(@"1-9", ErrorMessage = "Only number please!..")]
        public string Pin { get; set; }

        [Required]
        [DisplayName("Longitude")]
        public string Longitude { get; set; }

        [Required]
        [DisplayName("Latitude")]
        public string Latitude { get; set; }

        //[Required]
        //[DisplayName("Country")]
        public string Country { get; set; }

        [Required]
        [DisplayName("Phone No")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        public List<SelectListItem> Manufacturers { set;get;}
        public Vendor()
        {
            Manufacturers = new List<SelectListItem>();
        }  
       // public enum catname { }
        //public string catname { get; set; }
        //public IEnumerable<Vendor> Divisions { get; set; }
        public List<State1> StateModel { get; set; }
        //public SelectList FilteredCity { get; set; }


    }
    public class State1
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }
    //public class City
    //{
    //    public int Id { get; set; }
    //    public int StateId { get; set; }
    //    public string CityName { get; set; }
    //}
   
}