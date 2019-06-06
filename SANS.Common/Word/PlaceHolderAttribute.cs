using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Common.Word
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PlaceHolderAttribute : Attribute
    {
        public PlaceHolderAttribute(PlaceHolderEnum placeHolder)
        {
            this.PlaceHolder = placeHolder;
        }

        public PlaceHolderEnum PlaceHolder { get; set; }
    }
}
