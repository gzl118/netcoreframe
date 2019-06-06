using SANS.BLL.Interface;
using SANS.DAL.Implements;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using SANS.DbEntity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SANS.BLL.Implements
{
    public class SysDataDictionaryBLL:BaseBLL<SysDataDictionary>, ISysDataDictionaryBLL
    {
        private ISysDataDictionaryDAL sysDataDictionaryDAL;
        /// <summary>
        ///  _sysDataDictionaryDAL
        /// </summary>
        /// <param name="_sysDataDictionaryDAL"></param>
        public SysDataDictionaryBLL(ISysDataDictionaryDAL _sysDataDictionaryDAL) : base(_sysDataDictionaryDAL)
        {
            this.sysDataDictionaryDAL = _sysDataDictionaryDAL;
        }
        #region 获取字典列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public MessageModel GetDictList(string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            Expression<Func<SysDataDictionary, bool>> expression = t => (string.IsNullOrEmpty(searchstr) || t.type_name.Contains(searchstr) ||
                t.name.Contains(searchstr));
            PageModel.RowCount = sysDataDictionaryDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = sysDataDictionaryDAL.SqlQuery<SysDataDictionary>($@"                                           
                                    SELECT  T1.*
                                    FROM Sys_DataDictionary T1
                                    WHERE (T1.type_name LIKE '%{searchstr}%'
                                            OR T1.name LIKE '%{searchstr}%'
                                           )
                                          order by T1.creat_time 
                                limit   {iBeginRow} , {limit}");

            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        public MessageModel GetDictListByTypeCode(string typecode)
        {
            var messageModel = new MessageModel();

            var list = sysDataDictionaryDAL.SqlQuery<SysDataDictionary>($@"                                           
                                    SELECT  T1.*
                                    FROM Sys_DataDictionary T1
                                    WHERE T1.type_code='{typecode}'");
            messageModel.Data = list;
            return messageModel;
        }
        #endregion
        #region 添加/编辑字典
        /// <summary>
        /// 添加/编辑用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="userId">当前登录用户id</param>
        /// <returns></returns>
        public MessageModel AddorEditDict(SysDataDictionary model, string userId)
        {
            var messageModel = new MessageModel();
            int intcount = sysDataDictionaryDAL.GetCount(t => t.dict_id.Equals(model.dict_id));
            //添加
            if (string.IsNullOrEmpty(model.dict_id))
            {
                if (intcount == 0)
                {
                    model.dict_id = Guid.NewGuid().ToString();
                    model.creat_time = DateTime.Now;
                    //添加用户
                    if (Add(model))
                    {
                        messageModel.Message = "添加成功";
                    }
                    else
                    {
                        messageModel.Message = "添加失败";
                        messageModel.Result = false;
                    }
                }
                else
                {
                    messageModel.Message = "用户名已经被注册";
                    messageModel.Result = false;
                }
            }
            //修改
            else
            {
                var dict_edit = GetModels(t => t.dict_id.Equals(model.dict_id)).SingleOrDefault();
                dict_edit.code = model.code;
                dict_edit.code_sort = model.code_sort;
                dict_edit.code_value= model.code_value;
                dict_edit.name = model.name;
                dict_edit.parent_code = model.parent_code;
                dict_edit.remark = model.remark;
                dict_edit.type_code = model.type_code;
                dict_edit.type_name = model.type_name;
                if (Update(dict_edit))
                {
                    messageModel.Message = "修改成功";
                }
                else
                {
                    messageModel.Message = "修改失败";
                }

            }

            return messageModel;
        }
        #endregion
        #region (批量删除)
        /// <summary>
        /// 删除字典(批量删除)
        /// </summary>
        /// <param name="DictId"></param>
        /// <returns></returns>
        public MessageModel DelDict(IEnumerable<string> DictId)
        {
            var model = new MessageModel();
            try { 
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine(value: $"delete FROM Sys_DataDictionary  WHERE dict_id IN (");
            builder.AppendLine($"'{String.Join("','", DictId)}');");
            bool bResult = ExecuteSql(builder.ToString()) > 0;
            model.Result = bResult;
            model.Message = bResult ? "删除成功" : "删除失败";
            }
            catch (Exception er) {
                model.Result = false;
                model.Message =  "删除失败"+er.Message;
            }
            return model;
        }
        #endregion
    }
}
