namespace Presentation.SubSections.Customers
{
    partial class CustomerGridView
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
            gridviewCustomer = new Guna.UI2.WinForms.Guna2DataGridView();
            cmbboxcustomerFilterCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbboxcustomerFilterValue = new Guna.UI2.WinForms.Guna2ComboBox();
            searchboxCustomerSection = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)gridviewCustomer).BeginInit();
            SuspendLayout();
            // 
            // gridviewCustomer
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewCustomer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            gridviewCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(232, 234, 237);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewCustomer.ColumnHeadersHeight = 25;
            gridviewCustomer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewCustomer.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewCustomer.GridColor = Color.LightGray;
            gridviewCustomer.Location = new Point(30, 166);
            gridviewCustomer.Margin = new Padding(3, 2, 3, 2);
            gridviewCustomer.Name = "gridviewCustomer";
            gridviewCustomer.ReadOnly = true;
            gridviewCustomer.RowHeadersVisible = false;
            gridviewCustomer.RowHeadersWidth = 100;
            gridviewCustomer.RowTemplate.Height = 29;
            gridviewCustomer.Size = new Size(1229, 421);
            gridviewCustomer.TabIndex = 4;
            gridviewCustomer.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            gridviewCustomer.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewCustomer.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewCustomer.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewCustomer.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewCustomer.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewCustomer.ThemeStyle.BackColor = Color.White;
            gridviewCustomer.ThemeStyle.GridColor = Color.LightGray;
            gridviewCustomer.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(232, 234, 237);
            gridviewCustomer.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewCustomer.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gridviewCustomer.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            gridviewCustomer.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewCustomer.ThemeStyle.HeaderStyle.Height = 25;
            gridviewCustomer.ThemeStyle.ReadOnly = true;
            gridviewCustomer.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewCustomer.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewCustomer.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewCustomer.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            gridviewCustomer.ThemeStyle.RowsStyle.Height = 29;
            gridviewCustomer.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            gridviewCustomer.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            gridviewCustomer.CellClick += gidviewCustomer_CellClick;
            // 
            // cmbboxcustomerFilterCategory
            // 
            cmbboxcustomerFilterCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbboxcustomerFilterCategory.BackColor = Color.Transparent;
            cmbboxcustomerFilterCategory.CustomizableEdges = customizableEdges1;
            cmbboxcustomerFilterCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxcustomerFilterCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxcustomerFilterCategory.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxcustomerFilterCategory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxcustomerFilterCategory.Font = new Font("Segoe UI", 10F);
            cmbboxcustomerFilterCategory.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxcustomerFilterCategory.ItemHeight = 30;
            cmbboxcustomerFilterCategory.Location = new Point(825, 126);
            cmbboxcustomerFilterCategory.Margin = new Padding(3, 2, 3, 2);
            cmbboxcustomerFilterCategory.Name = "cmbboxcustomerFilterCategory";
            cmbboxcustomerFilterCategory.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbboxcustomerFilterCategory.Size = new Size(264, 36);
            cmbboxcustomerFilterCategory.TabIndex = 5;
            cmbboxcustomerFilterCategory.SelectedIndexChanged += cmbboxcustomerFilterCategory_SelectedIndexChanged;
            // 
            // cmbboxcustomerFilterValue
            // 
            cmbboxcustomerFilterValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbboxcustomerFilterValue.BackColor = Color.Transparent;
            cmbboxcustomerFilterValue.CustomizableEdges = customizableEdges3;
            cmbboxcustomerFilterValue.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxcustomerFilterValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxcustomerFilterValue.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxcustomerFilterValue.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxcustomerFilterValue.Font = new Font("Segoe UI", 10F);
            cmbboxcustomerFilterValue.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxcustomerFilterValue.ItemHeight = 30;
            cmbboxcustomerFilterValue.Location = new Point(1105, 126);
            cmbboxcustomerFilterValue.Margin = new Padding(3, 2, 3, 2);
            cmbboxcustomerFilterValue.Name = "cmbboxcustomerFilterValue";
            cmbboxcustomerFilterValue.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbboxcustomerFilterValue.Size = new Size(154, 36);
            cmbboxcustomerFilterValue.TabIndex = 6;
            cmbboxcustomerFilterValue.SelectedIndexChanged += cmbboxcustomerFilterValue_SelectedIndexChanged;
            // 
            // searchboxCustomerSection
            // 
            searchboxCustomerSection.CustomizableEdges = customizableEdges5;
            searchboxCustomerSection.DefaultText = "";
            searchboxCustomerSection.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            searchboxCustomerSection.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            searchboxCustomerSection.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            searchboxCustomerSection.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            searchboxCustomerSection.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxCustomerSection.Font = new Font("Segoe UI", 9F);
            searchboxCustomerSection.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxCustomerSection.Location = new Point(30, 126);
            searchboxCustomerSection.Name = "searchboxCustomerSection";
            searchboxCustomerSection.PlaceholderText = "Search Customer";
            searchboxCustomerSection.SelectedText = "";
            searchboxCustomerSection.ShadowDecoration.CustomizableEdges = customizableEdges6;
            searchboxCustomerSection.Size = new Size(201, 27);
            searchboxCustomerSection.TabIndex = 7;
            searchboxCustomerSection.TextChanged += searchboxCustomerSection_TextChanged;
            // 
            // CustomerGridView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(searchboxCustomerSection);
            Controls.Add(cmbboxcustomerFilterValue);
            Controls.Add(cmbboxcustomerFilterCategory);
            Controls.Add(gridviewCustomer);
            Margin = new Padding(3, 2, 3, 2);
            Name = "CustomerGridView";
            Size = new Size(1346, 595);
            Load += CustomerGridView_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewCustomer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewCustomer;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxcustomerFilterCategory;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxcustomerFilterValue;
        private Guna.UI2.WinForms.Guna2TextBox searchboxCustomerSection;
    }
}
