using System;
using System.Collections.Generic;
using DInjectionProvider;
using SANS.BLL;
using SANS.BLL.Interface;
using SANS.Common;
using SANS.Common.Cache;
using SANS.DbEntity.Models;
using SANS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using SANS.WebApp.Controllers;

namespace SANS.WebApp.Areas.SystemManager.Controllers
{
    [Area("System")]
    public class DictController : BaseController
    {
        private readonly WholeInjection injection;
        private ICacheContext _cacheContext;
        public DictController(WholeInjection injection, ICacheContext cacheContext)
        {
            _cacheContext = cacheContext;
            this.injection = injection;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DictAdd()
        {
            return View();
        }
        #region 获取字典列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public string GetDictList(string page, string limit, string searchstr)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                messageModel = injection.GetT<ISysDataDictionaryBLL>()
                    .GetDictList(page, limit, searchstr);
                return JsonHelper.ObjectToJson(new
                {
                    code = 0,
                    msg = "获取成功",
                    count = messageModel.Data.RowCount,
                    data = messageModel.Data.Data
                });
            }
            catch (Exception er)
            {
                return JsonHelper.ObjectToJson(new
                {
                    code = 0,
                    msg = er.Message,
                    count = 0
                });
            }

        }
        #endregion
        #region 添加/编辑字典
        /// <summary>
        /// 添加/编辑用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public string AddorEditDict(SysDataDictionary model)
        {
            var resModel = new ResponseModel();
            var userLogin = injection.GetT<Data.UserAccount>().GetUserInfo();
            var mModel = injection.GetT<ISysDataDictionaryBLL>().AddorEditDict(model, userLogin.UserId);
            resModel.StateCode = mModel.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = mModel.Message;
            resModel.JsonData = mModel.Data;
            if (mModel.Result)
            {
                _cacheContext.Remove("SysName");
            }
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
        #region 批量删除字典
        public string DelDict(string dict_id)
        {
            List<string> DictL = new List<string>();
            if (!string.IsNullOrEmpty(dict_id))
            {
                DictL.Add(dict_id);
            }
            return DelDicts(DictL);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="DictId"></param>
        /// <returns></returns>
        public string DelDicts(List<string> dict_id)
        {
            var resModel = new ResponseModel();
            //此处删除进行逻辑删除
            MessageModel model = injection.GetT<ISysDataDictionaryBLL>().DelDict(dict_id);
            resModel.StateCode = model.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = model.Message;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
    }
}