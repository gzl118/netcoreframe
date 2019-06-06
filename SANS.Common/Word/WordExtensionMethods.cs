using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SANS.Common.Word
{
    public static class WordExtensionMethods
    {
        /// <summary>
        /// 将IWorkbook转换为byte数组
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this XWPFDocument doc)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                doc.Write(ms);
                result = ms.ToArray();
            }

            return result;
        }
    }
}
