using System;

namespace SANS.Common.Word
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PicturePlaceHolderAttribute : Attribute
    {
        public PicturePlaceHolderAttribute(PlaceHolderEnum placeHolder, string imageType)
        {
            this.PlaceHolder = placeHolder;
            this.ImageType = imageType;
        }

        public PlaceHolderEnum PlaceHolder { get; set; }
        public string ImageType { get; set; }
    }
}
