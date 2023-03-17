using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Class_类
{
    /// <summary>
    /// DataGridView相关的类
    /// </summary>
    public static class DgvHelper
    {
        /// <summary>
        /// 设置 DataGridView 添加 RowHead 序号
        /// </summary>
        /// <param name="myDGV"></param>
        static public void SetRowHeadNum(DataGridView myDGV)
        {
            int index = 1;
            foreach (DataGridViewRow gridRow in myDGV.Rows)
            {
                gridRow.HeaderCell.Value = index.ToString();
                index++;
            }
        }

        /// <summary>
        /// 设置 DataGridView 的行号宽度
        /// </summary>
        /// <param name="myDGV"></param>
        static public void SetRowWid(DataGridView myDGV)
        {
            //1-40
            //2-45
            //3-50
            //4-55
            //5-62
            //6-68
            //7-73
            //8-80
            //9-85
            int[] iAryWid = { 42, 45, 50, 55, 60, 70, 75, 80, 90 };
            string strRowCount = myDGV.Rows.Count.ToString();
            int iLen = strRowCount.Length;
            if (iLen <= iAryWid.GetLength(0))
                myDGV.RowHeadersWidth = iAryWid[iLen - 1];
        }

        /// <summary>
        /// DataGridView 将每个行设为只读
        /// </summary>
        /// <param name="myDGV"></param>
        static public void SetRowsReadOnly(DataGridView myDGV)
        {
            for (int i = 0; i < myDGV.Rows.Count; i++)
            {
                myDGV.Rows[i].ReadOnly = true;
                myDGV.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// 将奇偶行设成 斑马线
        /// </summary>
        /// <param name="myDGV"></param>
        /// <param name="Flag"></param>
        static public void SetBanMaXian(DataGridView myDGV, bool NeedBanMaXian = true)
        {
            myDGV.DefaultCellStyle.BackColor = Color.LightGray;
            if (!NeedBanMaXian) return;

            for (int i = 1; i < myDGV.RowCount; i = i + 2)
            {
                if (i >= myDGV.RowCount) continue;

                myDGV.Rows[i].DefaultCellStyle.BackColor = Color.LightSteelBlue;
            }
        }

        /// <summary>
        /// 初始化 dataGridVew 各个列的信息
        /// </summary>
        /// <param name="aryDGVCol"></param>
        /// <param name="srcDgv"></param>
        static public void AddCols(DgvCol[] aryDGVCol, DataGridView srcDgv, bool ColumnWidthModeIsFill = false)
        {
            //                                              ,CheckBox,DataGridViewColumnDateMin,comboBox
            //  {"See=N","Read=Y","050","XuanQu","选取","","CheckBox"},
            srcDgv.Columns.Clear();
            for (int i = 0; i < aryDGVCol.GetLength(0); i++)
            {
                DgvCol itemCol = aryDGVCol[i];
                string strColName = itemCol.ColName;
                string strColTitle = itemCol.ColTitle;

                #region 添加一列
                if (itemCol.DgvType == EDgvColType.CheckBoxColumn)
                {
                    DataGridViewCheckBoxColumn newCol = new DataGridViewCheckBoxColumn();
                    newCol.Name = strColName;
                    newCol.HeaderText = itemCol.ColTitle;
                    newCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    //newCol.ThreeState = true;
                    //newCol.CellTemplate = new DataGridViewCheckBoxCell();

                    newCol.TrueValue = true;
                    newCol.FalseValue = false;

                    srcDgv.Columns.Add(newCol);
                }
                else if (itemCol.DgvType == EDgvColType.ComboBoxColumn)
                {
                    DataGridViewComboBoxColumn newCol = new DataGridViewComboBoxColumn();
                    newCol.Name = strColName;
                    newCol.HeaderText = strColTitle;
                    newCol.MaxDropDownItems = 6;
                    newCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

                    srcDgv.Columns.Add(newCol);
                }
                else if (itemCol.DgvType == EDgvColType.ButtonColumn)
                {
                    DataGridViewButtonColumn newCol = new DataGridViewButtonColumn();
                    newCol.Name = strColName;
                    newCol.HeaderText = strColTitle;
                    newCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    srcDgv.Columns.Add(newCol);
                }
                else
                {
                    srcDgv.Columns.Add(strColName, strColTitle);
                }
                #endregion

                #region 是否绑定数据

                if (itemCol.DBAName.Length > 1)
                {
                    srcDgv.Columns[strColName].DataPropertyName = itemCol.DBAName;
                }
                #endregion

                #region 是否可见

                srcDgv.Columns[strColName].Visible = itemCol.See;

                #endregion

                #region 是否只读

                srcDgv.Columns[strColName].ReadOnly = itemCol.Read;

                #endregion

                srcDgv.Columns[strColName].DisplayIndex = i;
                srcDgv.Columns[strColName].Width = itemCol.Width;
                srcDgv.Columns[strColName].SortMode = DataGridViewColumnSortMode.NotSortable;
                srcDgv.Columns[strColName].DefaultCellStyle.Alignment = itemCol.DgvColAlignment;
                srcDgv.Columns[strColName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (ColumnWidthModeIsFill && i == aryDGVCol.GetLength(0) - 1) // 已经是最后一列了
                {
                    srcDgv.Columns[strColName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }



        /// <summary>
        /// 初始化dgv
        /// </summary>
        /// <param name="srcDgv"></param>
        /// <param name="aryCol"></param>
        public static void InitDgvCol(DataGridView srcDgv, DgvCol[] aryCol, bool ColumnWidthModeIsFill = false)
        {
            srcDgv.ColumnHeadersHeight = 40;
            srcDgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            srcDgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            srcDgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            srcDgv.RowHeadersWidth = 60;
            srcDgv.AllowUserToDeleteRows = false;
            srcDgv.AllowUserToAddRows = false;
            srcDgv.ReadOnly = true;
            srcDgv.MultiSelect = false; // 此处设为 false ，保存是我只保存一条记录就可以了
            srcDgv.AllowUserToResizeRows = false;
            srcDgv.EditMode = DataGridViewEditMode.EditOnEnter;

            srcDgv.DefaultCellStyle.Font = new System.Drawing.Font("Simsun", 9.0f);
            srcDgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Simsun", 9.0f);
            srcDgv.RowHeadersDefaultCellStyle.Font = new System.Drawing.Font("Simsun", 9.0f);

            DgvHelper.AddCols(aryCol, srcDgv, ColumnWidthModeIsFill);

            srcDgv.RowHeadersVisible = false;
            srcDgv.DefaultCellStyle.BackColor = Color.LightGray;
            DgvHelper.SetBanMaXian(srcDgv, true);
        }

        /// <summary>
        /// 将 dataGridView1 导出为Csv
        /// </summary>
        /// <param name="dataGridView1"></param>
        /// <param name="FileHead"></param>
        static public void ToCsv(DataGridView dataGridView1, string FileHead = "")
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Data!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string FileName = "";

            if (true)
            {
                #region 得到文件名称

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "Execl files (*.csv)|*.csv";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                //saveFileDialog1.Title = "Export file save path";
                saveFileDialog1.Title = "导出文件保存路径";
                saveFileDialog1.FileName = FileHead + " " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ".csv";

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                FileName = saveFileDialog1.FileName;

                if (FileName.Length == 0)
                    return;

                #endregion
            }

            DataTable myTable = new DataTable();
            DataView myView = myTable.DefaultView;

            if (true)
            {
                #region 得到 dataTable

                DataColumn newCol_A = new DataColumn("Name", System.Type.GetType("System.String"));
                DataColumn newCol_B = new DataColumn("DisplayIndex", System.Type.GetType("System.Int32"));
                myTable.Columns.Add(newCol_A);
                myTable.Columns.Add(newCol_B);
                foreach (DataGridViewColumn theCol in dataGridView1.Columns)
                {
                    if (theCol.Visible == true)
                    {
                        DataRow newRow = myTable.NewRow();
                        newRow["Name"] = theCol.Name;
                        newRow["DisplayIndex"] = theCol.DisplayIndex;
                        myTable.Rows.Add(newRow);
                    }
                }

                #endregion

                myView.Sort = "DisplayIndex";
            }

            List<string> lstLine = new List<string>();

            if (true)
            {
                #region 得到输出文本

                try
                {
                    StringBuilder sbHead = new StringBuilder();
                    sbHead.Append("Serial Number");
                    foreach (DataRow myRow in myView.ToTable().Rows)
                    {
                        string ColName = myRow["Name"].ToString();
                        string HeadText = dataGridView1.Columns[ColName].HeaderText;
                        if (HeadText.Contains("\r")) HeadText = HeadText.Replace("\r", "");
                        if (HeadText.Contains("\n")) HeadText = HeadText.Replace("\n", "");
                        if (HeadText.Contains(",")) HeadText = HeadText.Replace(",", "，");

                        sbHead.AppendFormat(",{0}", HeadText);
                    }

                    lstLine.Add(sbHead.ToString());

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        StringBuilder sbLine = new StringBuilder();

                        DataGridViewRow gridRow = dataGridView1.Rows[i];

                        sbLine.AppendFormat("{0}", (i + 1));
                        foreach (DataRow myRow in myView.ToTable().Rows)
                        {
                            string ColName = myRow["Name"].ToString();
                            string strTmp = dataGridView1[ColName, i].FormattedValue.ToString();

                            if (strTmp.Contains("\r")) strTmp = strTmp.Replace("\r", "");
                            if (strTmp.Contains("\n")) strTmp = strTmp.Replace("\n", "");
                            if (strTmp.Contains(",")) strTmp = strTmp.Replace(",", "，");

                            sbLine.AppendFormat(",{0}", strTmp);
                        }
                        lstLine.Add(sbLine.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Debug.Assert(false, ex.Message);
                }
                finally
                {
                }

                #endregion
            }

            using (StreamWriter sw = new StreamWriter(FileName, false, UnicodeEncoding.GetEncoding("GB2312")))
            {
                foreach (string line in lstLine)
                {
                    sw.WriteLine(line);
                }
            }
            //MessageBox.Show("Finished exporting CSV file!", "Attention");
            MessageBox.Show("导出文件完成", "Attention");
        }



        /// <summary>
        /// 从csv中导入数据到dgv
        /// </summary>
        /// <param name="dgv"></param>
        public static void ImportInCsv(DataGridView dgv)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "CSV Files|*.csv";

            if (openFileDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            List<string> lstLine = new List<string>();

            try
            {
                using (StreamReader sw = new StreamReader(openFileDlg.FileName, Encoding.GetEncoding("GB2312")))
                {
                    while (!sw.EndOfStream)
                    {
                        lstLine.Add(sw.ReadLine());
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Export fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("导出失败!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lstLine == null || lstLine.Count < 2)
            {
                return;
            }


            for (int i = 1; i < lstLine.Count; i++)
            {
                string line = lstLine[i];
                string[] aryData = line.Split(new char[] { ',', }, StringSplitOptions.RemoveEmptyEntries);

                int newRowIndex = dgv.Rows.Add();
                DataGridViewRow gridRow = dgv.Rows[newRowIndex];

                int fillCount = Math.Min(aryData.GetLength(0), dgv.Columns.Count);

                for (int col = 0; col < fillCount; col++)
                {
                    gridRow.Cells[col].Value = aryData[col];
                }
            }

        }

        /// <summary>
        /// 把dgv中的数据导出为csv
        /// </summary>
        /// <param name="dgv"></param>

        public static void ImportToCsv___(DataGridView dgv)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "CSV Files|*.csv";

            if (saveFileDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }



            List<string> lstLine = new List<string>();

            {
                StringBuilder sb = new StringBuilder();
                foreach (DataGridViewColumn gridCol in dgv.Columns)
                {
                    sb.AppendFormat("{0},", gridCol.HeaderText.Trim());
                }

                lstLine.Add(sb.ToString());
            }

            if (dgv.Rows.Count > 0)
            {
                foreach (DataGridViewRow gridRow in dgv.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataGridViewColumn gridCol in dgv.Columns)
                    {
                        object objValue = gridRow.Cells[gridCol.Name].Value;
                        string sValue = "";
                        if (objValue != null)
                        {
                            sValue = objValue.ToString().Trim();
                        }
                        sb.AppendFormat("{0},", sValue);
                    }
                    lstLine.Add(sb.ToString());
                }
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(saveFileDlg.FileName, false, Encoding.GetEncoding("GB2312")))
                {
                    foreach (string line in lstLine)
                    {
                        sw.WriteLine(line);
                    }
                }

                //MessageBox.Show("Export success!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("导出成功!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                //MessageBox.Show("Export fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("导出失败!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        public static string GetCell_String(DataGridViewCell gridCell)
        {
            object obj = gridCell.Value;

            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static int GetCell_Int(DataGridViewCell gridCell)
        {
            object obj = gridCell.Value;
            if (obj == null)
            {
                return 0;
            }
            else
            {
                string sValue = obj.ToString();
                int iValue = 0;
                int.TryParse(sValue, out iValue);
                return iValue;
            }
        }

        public static double GetCell_Double(DataGridViewCell gridCell)
        {
            object obj = gridCell.Value;
            if (obj == null)
            {
                return 0;
            }
            else
            {
                string sValue = obj.ToString();
                double dValue = 0;
                double.TryParse(sValue, out dValue);
                return dValue;
            }
        }
        public static float GetCell_Float(DataGridViewCell gridCell)
        {
            object obj = gridCell.Value;
            if (obj == null)
            {
                return 0;
            }
            else
            {
                string sValue = obj.ToString();
                float fValue = 0;
                Single.TryParse(sValue, out fValue);
                return fValue;
            }
        }

        /// <summary>
        /// 上移一行
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void MoveUp(DataGridView dgv)
        {
            if (dgv.ColumnCount < 2 || dgv.CurrentCell == null)
            {
                return;
            }

            int selIndex = dgv.CurrentRow.Index;
            if (selIndex > 0)
            {
                DataGridViewRow moveRow = dgv.Rows[selIndex - 1];
                dgv.Rows.Remove(moveRow);
                dgv.Rows.Insert(selIndex, moveRow);
            }
        }


        /// <summary>
        /// 下移一行
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void MoveDown(DataGridView dgv)
        {
            if (dgv.ColumnCount < 2 || dgv.CurrentCell == null)
            {
                return;
            }

            int selIndex = dgv.CurrentRow.Index;
            if (selIndex >= 0 & (dgv.RowCount - 1) != selIndex)//如果该行不是最后一行
            {
                DataGridViewRow moveRow = dgv.Rows[selIndex + 1];
                dgv.Rows.Remove(moveRow);
                dgv.Rows.Insert(selIndex, moveRow);//将选中行的上一行插入到选中行的后面 
            }
        }
    }
}
