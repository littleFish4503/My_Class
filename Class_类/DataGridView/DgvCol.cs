using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Class_类
{
    public class DgvCol
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool See { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// 设置列宽
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 列的名称
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 列的标题
        /// </summary>
        public string ColTitle { get; set; }

        /// <summary>
        /// 列的DataPropertyName
        /// </summary>
        public string DBAName { get; set; }

        /// <summary>
        /// 列的类型
        /// </summary>
        public EDgvColType DgvType = EDgvColType.TextBoxColumn;

        public DataGridViewContentAlignment DgvColAlignment = DataGridViewContentAlignment.MiddleLeft;

        public DgvCol(bool See, bool Read, int Width, string ColName, string ColTitle, string DBAName, EDgvColType DgvType, DataGridViewContentAlignment DgvColAlignment)
        {
            this.See = See;
            this.Read = Read;
            this.Width = Width;
            this.ColName = ColName;
            this.ColTitle = ColTitle;
            this.DBAName = DBAName;
            this.DgvType = DgvType;
            this.DgvColAlignment = DgvColAlignment;
        }

        public DgvCol(bool See, bool Read, int Width, string ColName, string ColTitle, EDgvColType DgvType, DataGridViewContentAlignment DgvColAlignment)
            : this(See, Read, Width, ColName, ColTitle, ColName, DgvType, DgvColAlignment)
        {
        }

        public DgvCol(bool See, int Width, string ColName, string ColTitle, EDgvColType DgvType, DataGridViewContentAlignment DgvColAlignment)
            : this(See, false, Width, ColName, ColTitle, ColName, DgvType, DgvColAlignment)
        {
        }
    }

    public enum EDgvColType
    {
        TextBoxColumn = 1,
        ButtonColumn = 2,
        CheckBoxColumn = 3,
        ComboBoxColumn = 4,
        ImageColumn = 5,
        LinkColumn = 6,
    }
}
