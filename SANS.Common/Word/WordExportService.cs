
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace SANS.Common.Word
{
    public static class WordExportService
    {
        /// <summary>
        /// 获取渲染后的Word
        /// </summary>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public static XWPFDocument ExportFromTemplate<T>(string templateUrl, T wordData)
             where T : class, new()
        {
            XWPFDocument word = GetTemplateWord(templateUrl, wordData);

            Render(word, wordData);

            return word;
        }

        /// <summary>
        /// 图片Id
        /// </summary>
        private static uint PicId
        {
            get
            {
                picId++;
                return picId;
            }
        }

        private static uint picId = 0;


        /// <summary>
        /// 获取插入图片配置信息
        /// </summary>
        /// <returns></returns>
        private static List<AddPictureOptions> GetAddPictureOptionsList<T>(T wordData)
           where T : class, new()
        {
            List<AddPictureOptions> listAddPictureOptions = new List<AddPictureOptions>();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            List<string> listPictureUrl;
            string picName;
            foreach (PropertyInfo prop in props)
            {
                if (prop.IsDefined(typeof(PicturePlaceHolderAttribute)))
                {
                    try
                    {
                        listPictureUrl = (List<string>)prop.GetValue(wordData);
                    }
                    catch (Exception)
                    {
                        throw new Exception("图片占位符必须为字符串集合类型");
                    }

                    picName = prop.GetCustomAttribute<PicturePlaceHolderAttribute>().ImageType.ToString();
                    for (int i = 0; i < listPictureUrl.Count; i++)
                    {
                        string picUrl = listPictureUrl[i];
                        listAddPictureOptions.Add(new AddPictureOptions()
                        {
                            PicId = PicId,
                            PictureName = $"{picName}_{i + 1}",
                            LocalPictureUrl = picUrl,
                            PlaceHolder = prop.GetCustomAttribute<PicturePlaceHolderAttribute>().PlaceHolder,
                            Extension = WordHelper.GetRemoteFileExtention(listPictureUrl[i]),
                            ImageType = prop.GetCustomAttribute<PicturePlaceHolderAttribute>().ImageType
                        });
                    }
                }
            }

            return listAddPictureOptions;
        }

        /// <summary>
        /// 获取模板文件
        /// </summary>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        private static XWPFDocument GetTemplateWord<T>(string templateUrl, T wordData)
            where T : class, new()
        {
            XWPFDocument word;

            Type type = typeof(T);
           
            if (!File.Exists(templateUrl))
            {
                throw new Exception("template not found");
            }

            try
            {
                using (FileStream fs = File.OpenRead(templateUrl))
                {
                    word = new XWPFDocument(fs);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("fail to open template");
            }

            return word;
        }

        /// <summary>
        /// 渲染Word文件
        /// </summary>
        /// <param name="word"></param>
        private static void Render<T>(XWPFDocument word, T wordData)
            where T : class, new()
        {
            if (word == null)
            {
                throw new ArgumentNullException("word");
            }

            Dictionary<string, string> placeHolderAndValueDict = GetPlaceHolderAndValueDict<T>(wordData);

            List<AddPictureOptions> listAddPictureOptions = GetAddPictureOptionsList(wordData);
            List<AddTableOptions> listAddTableOptions = GetTablePlaceHolderAndValueDict<T>(wordData);

            List<string> listAllPlaceHolder = GetAllPlaceHolder<T>();

            WordHelper.ReplacePlaceHolderInWord(word, placeHolderAndValueDict, listAddPictureOptions, listAllPlaceHolder, listAddTableOptions);
        }

        private static List<string> GetAllPlaceHolder<T>()
            where T : class, new()
        {
            List<string> listAllPlaceHolder = new List<string>();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.IsDefined(typeof(PlaceHolderAttribute)))
                {
                    listAllPlaceHolder.Add(prop.GetCustomAttribute<PlaceHolderAttribute>().PlaceHolder.ToString());
                }

                if (prop.IsDefined(typeof(PicturePlaceHolderAttribute)))
                {
                    listAllPlaceHolder.Add(prop.GetCustomAttribute<PicturePlaceHolderAttribute>().PlaceHolder.ToString());
                }
                if (prop.IsDefined(typeof(TablePlaceHolderAttribute)))
                {
                    listAllPlaceHolder.Add(prop.GetCustomAttribute<TablePlaceHolderAttribute>().PlaceHolder.ToString());
                }
            }

            return listAllPlaceHolder;
        }

        /// <summary>
        /// 获取展位符和值字典
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetPlaceHolderAndValueDict<T>(T wordData)
           where T : class, new()
        {
            Dictionary<string, string> placeHolderAndValueDict = new Dictionary<string, string>();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.IsDefined(typeof(PlaceHolderAttribute)))
                {
                    placeHolderAndValueDict.Add(prop.GetCustomAttribute<PlaceHolderAttribute>().PlaceHolder.ToString(), prop.GetValue(wordData)?.ToString());
                }
            }

            return placeHolderAndValueDict;
        }
        private static List<AddTableOptions> GetTablePlaceHolderAndValueDict<T>(T wordData)
           where T : class, new()
        {
            List<AddTableOptions> placeHolderAndValueDict = new List<AddTableOptions>();
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.IsDefined(typeof(TablePlaceHolderAttribute)))
                {
                    AddTableOptions ao = new AddTableOptions();
                    ao.PlaceHolder = prop.GetCustomAttribute<TablePlaceHolderAttribute>().PlaceHolder;
                    ao.dataTable =(DataTable) prop.GetValue(wordData);
                    placeHolderAndValueDict.Add(ao);
                }
            }

            return placeHolderAndValueDict;
        }
    }
}
