
using System.ComponentModel;

namespace SANS.DbEntity.Models
{
    /// <summary>
    /// 此类包含了项目所有枚举
    /// </summary>
    public static class SysEnum
    {

        #region 用户状态
        /// <summary>
        /// 用户状态
        /// </summary>
        public enum Enum_UserStatus
        {

            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Status_normal = 1,
            /// <summary>
            /// 停用
            /// </summary>
            [Description("停用")]
            Status_Discontinuation = 0
        }
        #endregion
        #region 权限类型
        /// <summary>
        /// 权限类型
        /// </summary>
        public enum Enum_AuthorityType
        {
            /// <summary>
            /// 所有类型
            /// </summary>
            ALL = 0,
            /// <summary>
            /// 菜单类型
            /// </summary>
            [Description("菜单类型")]
            Type_Menu = 1,
            /// <summary>
            /// 按钮
            /// </summary>
            [Description("按钮")]
            Type_Button = 2
        }
        #endregion
        #region 删除标识
        /// <summary>
        /// 删除标识
        /// </summary>
        public enum Enum_DeleteSign
        {
            /// <summary>
            /// 未删除
            /// </summary>
            [Description("未删除")]
            Sing_Deleted = 1,
            /// <summary>
            /// 已删除
            /// </summary>
            [Description("已删除")]
            Sign_Undeleted = 2
        }
        #endregion
        #region 性别标识
        /// <summary>
        /// 性别
        /// </summary>
        public enum Enum_Sex
        {
            /// <summary>
            /// 男孩
            /// </summary>
            [Description("男孩")]
            Boy = 1,
            /// <summary>
            /// 女孩
            /// </summary>
            [Description("女孩")]
            Girl = 0
        }
        #endregion

    }
}
