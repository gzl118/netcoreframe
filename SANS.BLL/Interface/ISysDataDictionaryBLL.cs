using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.BLL.Interface
{
    public interface ISysDataDictionaryBLL : IBaseBLL<SysDataDictionary>
    {
        #region 字典
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        MessageModel GetDictList(string page, string limit, string searchstr);
        MessageModel GetDictListByTypeCode(string typecode);
        MessageModel AddorEditDict(SysDataDictionary model, string userId);
        MessageModel DelDict(IEnumerable<string> DictId);
        #endregion
    }
}
