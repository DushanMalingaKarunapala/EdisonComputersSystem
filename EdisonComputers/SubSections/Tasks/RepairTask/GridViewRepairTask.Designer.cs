namespace Presentation.SubSections.Tasks.RepairTask
{
    partial class GridViewRepairTask
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
            gridviewRepairTaskViewer = new Guna.UI2.WinForms.Guna2DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridviewRepairTaskViewer).BeginInit();
            SuspendLayout();
            // 
            // gridviewRepairTaskViewer
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewRepairTaskViewer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewRepairTaskViewer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewRepairTaskViewer.ColumnHeadersHeight = 20;
            gridviewRepairTaskViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewRepairTaskViewer.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewRepairTaskViewer.GridColor = Color.FromArgb(239, 241, 243);
            gridviewRepairTaskViewer.Location = new Point(69, 106);
            gridviewRepairTaskViewer.Name = "gridviewRepairTaskViewer";
            gridviewRepairTaskViewer.RowHeadersVisible = false;
            gridviewRepairTaskViewer.RowHeadersWidth = 51;
            gridviewRepairTaskViewer.Size = new Size(732, 307);
            gridviewRepairTaskViewer.TabIndex = 3;
            gridviewRepairTaskViewer.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WhiteGrid;
            gridviewRepairTaskViewer.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewRepairTaskViewer.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewRepairTaskViewer.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewRepairTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewRepairTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewRepairTaskViewer.ThemeStyle.BackColor = Color.White;
            gridviewRepairTaskViewer.ThemeStyle.GridColor = Color.FromArgb(239, 241, 243);
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.BackColor = Color.White;
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewRepairTaskViewer.ThemeStyle.HeaderStyle.Height = 20;
            gridviewRepairTaskViewer.ThemeStyle.ReadOnly = false;
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.Height = 29;
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            gridviewRepairTaskViewer.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            gridviewRepairTaskViewer.CellClick += gridviewRepairTaskViewer_CellClick;
            // 
            // GridViewRepairTask
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridviewRepairTaskViewer);
            Name = "GridViewRepairTask";
            Size = new Size(885, 441);
            Load += GridViewRepairTask_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewRepairTaskViewer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewRepairTaskViewer;
    }
}
