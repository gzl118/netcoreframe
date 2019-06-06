

namespace SANS.Common.Word
{
    public class AddPictureOptions
    {
        /// <summary>
        /// 占位符
        /// </summary>
        public PlaceHolderEnum PlaceHolder { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string PictureName { get; set; }

        /// <summary>
        /// 本地全路径
        /// </summary>
        public string LocalPictureUrl { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public string ImageType { get; set; }

        /// <summary>
        /// 后缀名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 图片ID（NPOI插入图片使用，id不正确会有BUG）
        /// </summary>
        public uint PicId { get; set; }
    }
}
