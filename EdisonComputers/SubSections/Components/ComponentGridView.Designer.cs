namespace Presentation.SubSections.Components
{
    partial class ComponentGridView
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
            gridviewComponents = new Guna.UI2.WinForms.Guna2DataGridView();
            cmbboxcomponentsectionfilterCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbboxcomponentsectionfilterValue = new Guna.UI2.WinForms.Guna2ComboBox();
            searchboxcomponentSection = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)gridviewComponents).BeginInit();
            SuspendLayout();
            // 
            // gridviewComponents
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewComponents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewComponents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewComponents.ColumnHeadersHeight = 20;
            gridviewComponents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewComponents.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewComponents.GridColor = Color.FromArgb(231, 229, 255);
            gridviewComponents.Location = new Point(25, 272);
            gridviewComponents.Name = "gridviewComponents";
            gridviewComponents.RowHeadersVisible = false;
            gridviewComponents.RowHeadersWidth = 51;
            gridviewComponents.Size = new Size(1312, 506);
            gridviewComponents.TabIndex = 0;
            gridviewComponents.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewComponents.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewComponents.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewComponents.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewComponents.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewComponents.ThemeStyle.BackColor = Color.White;
            gridviewComponents.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            gridviewComponents.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            gridviewComponents.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewComponents.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            gridviewComponents.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            gridviewComponents.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewComponents.ThemeStyle.HeaderStyle.Height = 20;
            gridviewComponents.ThemeStyle.ReadOnly = false;
            gridviewComponents.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewComponents.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewComponents.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewComponents.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            gridviewComponents.ThemeStyle.RowsStyle.Height = 29;
            gridviewComponents.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            gridviewComponents.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            gridviewComponents.CellClick += gridviewComponents_CellClick;
            gridviewComponents.DataBindingComplete += gridviewComponents_DataBindingComplete;
            // 
            // cmbboxcomponentsectionfilterCategory
            // 
            cmbboxcomponentsectionfilterCategory.BackColor = Color.Transparent;
            cmbboxcomponentsectionfilterCategory.CustomizableEdges = customizableEdges1;
            cmbboxcomponentsectionfilterCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxcomponentsectionfilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxcomponentsectionfilterCategory.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxcomponentsectionfilterCategory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxcomponentsectionfilterCategory.Font = new Font("Segoe UI", 10F);
            cmbboxcomponentsectionfilterCategory.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxcomponentsectionfilterCategory.ItemHeight = 30;
            cmbboxcomponentsectionfilterCategory.Location = new Point(964, 230);
            cmbboxcomponentsectionfilterCategory.Name = "cmbboxcomponentsectionfilterCategory";
            cmbboxcomponentsectionfilterCategory.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbboxcomponentsectionfilterCategory.Size = new Size(175, 36);
            cmbboxcomponentsectionfilterCategory.TabIndex = 1;
            cmbboxcomponentsectionfilterCategory.SelectedIndexChanged += cmbboxcomponentsectionfilterCategory_SelectedIndexChanged;
            // 
            // cmbboxcomponentsectionfilterValue
            // 
            cmbboxcomponentsectionfilterValue.BackColor = Color.Transparent;
            cmbboxcomponentsectionfilterValue.CustomizableEdges = customizableEdges3;
            cmbboxcomponentsectionfilterValue.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxcomponentsectionfilterValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxcomponentsectionfilterValue.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxcomponentsectionfilterValue.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxcomponentsectionfilterValue.Font = new Font("Segoe UI", 10F);
            cmbboxcomponentsectionfilterValue.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxcomponentsectionfilterValue.ItemHeight = 30;
            cmbboxcomponentsectionfilterValue.Location = new Point(1162, 230);
            cmbboxcomponentsectionfilterValue.Name = "cmbboxcomponentsectionfilterValue";
            cmbboxcomponentsectionfilterValue.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbboxcomponentsectionfilterValue.Size = new Size(175, 36);
            cmbboxcomponentsectionfilterValue.TabIndex = 2;
            cmbboxcomponentsectionfilterValue.SelectedIndexChanged += cmbboxcomponentsectionfilterValue_SelectedIndexChanged;
            // 
            // searchboxcomponentSection
            // 
            searchboxcomponentSection.CustomizableEdges = customizableEdges5;
            searchboxcomponentSection.DefaultText = "";
            searchboxcomponentSection.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            searchboxcomponentSection.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            searchboxcomponentSection.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            searchboxcomponentSection.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            searchboxcomponentSection.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxcomponentSection.Font = new Font("Segoe UI", 9F);
            searchboxcomponentSection.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxcomponentSection.Location = new Point(25, 230);
            searchboxcomponentSection.Margin = new Padding(3, 4, 3, 4);
            searchboxcomponentSection.Name = "searchboxcomponentSection";
            searchboxcomponentSection.PlaceholderText = "Search Components";
            searchboxcomponentSection.SelectedText = "";
            searchboxcomponentSection.ShadowDecoration.CustomizableEdges = customizableEdges6;
            searchboxcomponentSection.Size = new Size(218, 36);
            searchboxcomponentSection.TabIndex = 3;
            searchboxcomponentSection.TextChanged += searchboxcomponentSection_TextChanged;
            // 
            // ComponentGridView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(searchboxcomponentSection);
            Controls.Add(cmbboxcomponentsectionfilterValue);
            Controls.Add(cmbboxcomponentsectionfilterCategory);
            Controls.Add(gridviewComponents);
            Name = "ComponentGridView";
            Size = new Size(1538, 793);
            Load += ComponentGridView_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewComponents).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewComponents;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxcomponentsectionfilterCategory;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxcomponentsectionfilterValue;
        private Guna.UI2.WinForms.Guna2TextBox searchboxcomponentSection;
    }
}
