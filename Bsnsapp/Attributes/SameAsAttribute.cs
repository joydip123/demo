using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bsnsapp.Attributes
{
    /// <summary>
    /// Attribute responsible for Tagging on a property which should be similar to the one mentioned as Property
    /// </summary>
    public class SameAsAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public SameAsAttribute(string Property)
        {
            this.Property = Property;

        }
        public override bool IsValid(object value)
        {

            return true;
        }


    }
}