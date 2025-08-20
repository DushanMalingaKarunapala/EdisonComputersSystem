namespace Presentation.SubSections.Tasks.UpgradeTask
{
    partial class GridViewUpgradeTask
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            gridviewUpgradeTaskViewer = new Guna.UI2.WinForms.Guna2DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridviewUpgradeTaskViewer).BeginInit();
            SuspendLayout();
            // 
            // gridviewUpgradeTaskViewer
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewUpgradeTaskViewer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewUpgradeTaskViewer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewUpgradeTaskViewer.ColumnHeadersHeight = 20;
            gridviewUpgradeTaskViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewUpgradeTaskViewer.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewUpgradeTaskViewer.GridColor = Color.FromArgb(239, 241, 243);
            gridviewUpgradeTaskViewer.Location = new Point(65, 107);
            gridviewUpgradeTaskViewer.Name = "gridviewUpgradeTaskViewer";
            gridviewUpgradeTaskViewer.RowHeadersVisible = false;
            gridviewUpgradeTaskViewer.RowHeadersWidth = 51;
            gridviewUpgradeTaskViewer.Size = new Size(986, 483);
            gridviewUpgradeTaskViewer.TabIndex = 4;
            gridviewUpgradeTaskViewer.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WhiteGrid;
            gridviewUpgradeTaskViewer.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewUpgradeTaskViewer.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewUpgradeTaskViewer.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewUpgradeTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewUpgradeTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewUpgradeTaskViewer.ThemeStyle.BackColor = Color.White;
            gridviewUpgradeTaskViewer.ThemeStyle.GridColor = Color.FromArgb(239, 241, 243);
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.BackColor = Color.White;
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewUpgradeTaskViewer.ThemeStyle.HeaderStyle.Height = 20;
            gridviewUpgradeTaskViewer.ThemeStyle.ReadOnly = false;
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.Height = 29;
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            gridviewUpgradeTaskViewer.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            gridviewUpgradeTaskViewer.CellClick += gridviewUpgradeTaskViewer_CellClick;
            // 
            // GridViewUpgradeTask
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridviewUpgradeTaskViewer);
            Name = "GridViewUpgradeTask";
            Size = new Size(1221, 776);
            Load += GridViewUpgradeTask_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewUpgradeTaskViewer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewUpgradeTaskViewer;
    }
}
