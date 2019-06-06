#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/6/23 15:37:29 
 * 当前版本：1.0.0.1 
 *  
 * 描述说明： 
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ ZhouLi 2018 All rights reserved 
 *┌──────────────────────────────────┐
 *│　此技术信息周黎的机密信息，未经本人书面同意禁止向第三方披露．　│
 *│　版权所有：周黎 　　　　　　　　　　　　　　│
 *└──────────────────────────────────┘
*****************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Text;


namespace SANS.BLL
{

    /// <summary>
    /// 消息模型(用于逻辑层与UI层通信规范)
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 结果(默认:true)
        /// </summary>
        public bool Result { set; get; } = true;
        /// <summary>
        /// 提示信息(默认:"成功")
        /// </summary>
        public string Message { set; get; } = "成功";
        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { set; get; }
    }
}
