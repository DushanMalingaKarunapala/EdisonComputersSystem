namespace Presentation.SubSections.Jobs
{
    partial class JobGridViewSection
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            jobgridView = new Guna.UI2.WinForms.Guna2DataGridView();
            cmbboxjobsectionfilterCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbboxjobsectionfilterValue = new Guna.UI2.WinForms.Guna2ComboBox();
            searchboxjobsgriviewSection = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)jobgridView).BeginInit();
            SuspendLayout();
            // 
            // jobgridView
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            jobgridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            jobgridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            jobgridView.ColumnHeadersHeight = 20;
            jobgridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            jobgridView.DefaultCellStyle = dataGridViewCellStyle3;
            jobgridView.GridColor = Color.FromArgb(239, 241, 243);
            jobgridView.Location = new Point(41, 294);
            jobgridView.Name = "jobgridView";
            jobgridView.RowHeadersVisible = false;
            jobgridView.RowHeadersWidth = 51;
            jobgridView.Size = new Size(1202, 589);
            jobgridView.TabIndex = 0;
            jobgridView.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WhiteGrid;
            jobgridView.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            jobgridView.ThemeStyle.AlternatingRowsStyle.Font = null;
            jobgridView.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            jobgridView.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            jobgridView.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            jobgridView.ThemeStyle.BackColor = Color.White;
            jobgridView.ThemeStyle.GridColor = Color.FromArgb(239, 241, 243);
            jobgridView.ThemeStyle.HeaderStyle.BackColor = Color.White;
            jobgridView.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            jobgridView.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            jobgridView.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            jobgridView.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            jobgridView.ThemeStyle.HeaderStyle.Height = 20;
            jobgridView.ThemeStyle.ReadOnly = false;
            jobgridView.ThemeStyle.RowsStyle.BackColor = Color.White;
            jobgridView.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            jobgridView.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            jobgridView.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            jobgridView.ThemeStyle.RowsStyle.Height = 29;
            jobgridView.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            jobgridView.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            jobgridView.CellClick += jobgridView_CellClick;
            // 
            // cmbboxjobsectionfilterCategory
            // 
            cmbboxjobsectionfilterCategory.BackColor = Color.Transparent;
            cmbboxjobsectionfilterCategory.CustomizableEdges = customizableEdges1;
            cmbboxjobsectionfilterCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxjobsectionfilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxjobsectionfilterCategory.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxjobsectionfilterCategory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxjobsectionfilterCategory.Font = new Font("Segoe UI", 10F);
            cmbboxjobsectionfilterCategory.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxjobsectionfilterCategory.ItemHeight = 30;
            cmbboxjobsectionfilterCategory.Location = new Point(856, 243);
            cmbboxjobsectionfilterCategory.Name = "cmbboxjobsectionfilterCategory";
            cmbboxjobsectionfilterCategory.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbboxjobsectionfilterCategory.Size = new Size(175, 36);
            cmbboxjobsectionfilterCategory.TabIndex = 1;
            cmbboxjobsectionfilterCategory.SelectedIndexChanged += cmbboxjobsectionfilterCategory_SelectedIndexChanged_1;
            // 
            // cmbboxjobsectionfilterValue
            // 
            cmbboxjobsectionfilterValue.BackColor = Color.Transparent;
            cmbboxjobsectionfilterValue.CustomizableEdges = customizableEdges3;
            cmbboxjobsectionfilterValue.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxjobsectionfilterValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxjobsectionfilterValue.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxjobsectionfilterValue.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxjobsectionfilterValue.Font = new Font("Segoe UI", 10F);
            cmbboxjobsectionfilterValue.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxjobsectionfilterValue.ItemHeight = 30;
            cmbboxjobsectionfilterValue.Location = new Point(1070, 243);
            cmbboxjobsectionfilterValue.Name = "cmbboxjobsectionfilterValue";
            cmbboxjobsectionfilterValue.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbboxjobsectionfilterValue.Size = new Size(175, 36);
            cmbboxjobsectionfilterValue.TabIndex = 2;
            cmbboxjobsectionfilterValue.SelectedIndexChanged += cmbboxjobsectionfilterValue_SelectedIndexChanged_1;
            // 
            // searchboxjobsgriviewSection
            // 
            searchboxjobsgriviewSection.CustomizableEdges = customizableEdges5;
            searchboxjobsgriviewSection.DefaultText = "";
            searchboxjobsgriviewSection.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            searchboxjobsgriviewSection.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            searchboxjobsgriviewSection.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            searchboxjobsgriviewSection.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            searchboxjobsgriviewSection.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxjobsgriviewSection.Font = new Font("Segoe UI", 9F);
            searchboxjobsgriviewSection.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxjobsgriviewSection.Location = new Point(41, 243);
            searchboxjobsgriviewSection.Margin = new Padding(3, 4, 3, 4);
            searchboxjobsgriviewSection.Name = "searchboxjobsgriviewSection";
            searchboxjobsgriviewSection.PlaceholderText = "Search Job";
            searchboxjobsgriviewSection.SelectedText = "";
            searchboxjobsgriviewSection.ShadowDecoration.CustomizableEdges = customizableEdges6;
            searchboxjobsgriviewSection.Size = new Size(234, 36);
            searchboxjobsgriviewSection.TabIndex = 3;
            searchboxjobsgriviewSection.TextChanged += searchboxjobsgriviewSection_TextChanged;
            // 
            // JobGridViewSection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(searchboxjobsgriviewSection);
            Controls.Add(cmbboxjobsectionfilterValue);
            Controls.Add(cmbboxjobsectionfilterCategory);
            Controls.Add(jobgridView);
            Name = "JobGridViewSection";
            Size = new Size(1315, 911);
            Load += JobGridViewSection_Load;
            ((System.ComponentModel.ISupportInitialize)jobgridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView jobgridView;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxjobsectionfilterCategory;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxjobsectionfilterValue;
        private Guna.UI2.WinForms.Guna2TextBox searchboxjobsgriviewSection;
    }
}
