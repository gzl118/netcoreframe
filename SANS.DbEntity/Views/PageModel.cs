using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.DbEntity.Views
{
   public class PageModel
    {
        #region 当前页号
        private int _pageIndex;
        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        #endregion

        #region 每页记录数
        private int _pageSize;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
        #endregion

        #region 记录总数
        private int _rowCount;
        /// <summary>
        /// 记录总数
        /// </summary>
        public int RowCount
        {
            get { return _rowCount; }
            set { _rowCount = value; }
        }
        #endregion

        #region 分页记录对象列表
        private object _data;
        /// <summary>
        /// 分页记录对象列表
        /// </summary>
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }
        #endregion
    }
}
