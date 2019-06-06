using SANS.Common.Word;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Common.Word
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TablePlaceHolderAttribute : Attribute
    {
        public TablePlaceHolderAttribute(PlaceHolderEnum placeHolder)
        {
            this.PlaceHolder = placeHolder;
        }

        public PlaceHolderEnum PlaceHolder { get; set; }
    }
}
