using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SANS.Common.Word
{
    public class AddTableOptions
    {
        /// <summary>
        /// 占位符
        /// </summary>
        public PlaceHolderEnum PlaceHolder { get; set; }
        public DataTable dataTable { get; set; }
    }
}
