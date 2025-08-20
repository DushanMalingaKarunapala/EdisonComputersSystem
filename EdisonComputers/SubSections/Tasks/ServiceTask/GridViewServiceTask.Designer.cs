namespace Presentation.SubSections.Tasks.ServiceTask
{
    partial class GridViewServiceTask
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
            gridviewServiceTaskViewer = new Guna.UI2.WinForms.Guna2DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridviewServiceTaskViewer).BeginInit();
            SuspendLayout();
            // 
            // gridviewServiceTaskViewer
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewServiceTaskViewer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewServiceTaskViewer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewServiceTaskViewer.ColumnHeadersHeight = 20;
            gridviewServiceTaskViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewServiceTaskViewer.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewServiceTaskViewer.GridColor = Color.FromArgb(239, 241, 243);
            gridviewServiceTaskViewer.Location = new Point(58, 100);
            gridviewServiceTaskViewer.Name = "gridviewServiceTaskViewer";
            gridviewServiceTaskViewer.RowHeadersVisible = false;
            gridviewServiceTaskViewer.RowHeadersWidth = 51;
            gridviewServiceTaskViewer.Size = new Size(991, 436);
            gridviewServiceTaskViewer.TabIndex = 5;
            gridviewServiceTaskViewer.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WhiteGrid;
            gridviewServiceTaskViewer.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewServiceTaskViewer.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewServiceTaskViewer.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewServiceTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewServiceTaskViewer.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewServiceTaskViewer.ThemeStyle.BackColor = Color.White;
            gridviewServiceTaskViewer.ThemeStyle.GridColor = Color.FromArgb(239, 241, 243);
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.BackColor = Color.White;
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewServiceTaskViewer.ThemeStyle.HeaderStyle.Height = 20;
            gridviewServiceTaskViewer.ThemeStyle.ReadOnly = false;
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.Height = 29;
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            gridviewServiceTaskViewer.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            gridviewServiceTaskViewer.CellClick += gridviewServiceTaskViewer_CellClick;
            // 
            // GridViewServiceTask
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridviewServiceTaskViewer);
            Name = "GridViewServiceTask";
            Size = new Size(1214, 791);
            Load += GridViewServiceTask_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewServiceTaskViewer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewServiceTaskViewer;
    }
}
