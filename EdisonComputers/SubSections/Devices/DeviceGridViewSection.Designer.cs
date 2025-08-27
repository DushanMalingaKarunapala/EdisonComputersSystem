namespace Presentation.SubSections.Devices
{
    partial class DeviceGridViewSection
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
            gridviewDevice = new Guna.UI2.WinForms.Guna2DataGridView();
            cmbboxdevicegridviewfilterCateogory = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbboxdevicegridviewfilterValue = new Guna.UI2.WinForms.Guna2ComboBox();
            searchboxdeviceSection = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)gridviewDevice).BeginInit();
            SuspendLayout();
            // 
            // gridviewDevice
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            gridviewDevice.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            gridviewDevice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridviewDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridviewDevice.ColumnHeadersHeight = 22;
            gridviewDevice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            gridviewDevice.DefaultCellStyle = dataGridViewCellStyle3;
            gridviewDevice.GridColor = Color.FromArgb(239, 241, 243);
            gridviewDevice.Location = new Point(28, 200);
            gridviewDevice.Margin = new Padding(3, 2, 3, 2);
            gridviewDevice.Name = "gridviewDevice";
            gridviewDevice.RowHeadersVisible = false;
            gridviewDevice.RowHeadersWidth = 51;
            gridviewDevice.RowTemplate.Height = 29;
            gridviewDevice.Size = new Size(1228, 428);
            gridviewDevice.TabIndex = 0;
            gridviewDevice.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WhiteGrid;
            gridviewDevice.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            gridviewDevice.ThemeStyle.AlternatingRowsStyle.Font = null;
            gridviewDevice.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            gridviewDevice.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            gridviewDevice.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            gridviewDevice.ThemeStyle.BackColor = Color.White;
            gridviewDevice.ThemeStyle.GridColor = Color.FromArgb(239, 241, 243);
            gridviewDevice.ThemeStyle.HeaderStyle.BackColor = Color.White;
            gridviewDevice.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            gridviewDevice.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            gridviewDevice.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            gridviewDevice.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            gridviewDevice.ThemeStyle.HeaderStyle.Height = 22;
            gridviewDevice.ThemeStyle.ReadOnly = false;
            gridviewDevice.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridviewDevice.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridviewDevice.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            gridviewDevice.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            gridviewDevice.ThemeStyle.RowsStyle.Height = 29;
            gridviewDevice.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(239, 241, 243);
            gridviewDevice.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            gridviewDevice.CellClick += gridviewDevice_CellClick;
            gridviewDevice.CellContentClick += gridviewDevice_CellContentClick;
            // 
            // cmbboxdevicegridviewfilterCateogory
            // 
            cmbboxdevicegridviewfilterCateogory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbboxdevicegridviewfilterCateogory.BackColor = Color.Transparent;
            cmbboxdevicegridviewfilterCateogory.CustomizableEdges = customizableEdges1;
            cmbboxdevicegridviewfilterCateogory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxdevicegridviewfilterCateogory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxdevicegridviewfilterCateogory.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxdevicegridviewfilterCateogory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxdevicegridviewfilterCateogory.Font = new Font("Segoe UI", 10F);
            cmbboxdevicegridviewfilterCateogory.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxdevicegridviewfilterCateogory.ItemHeight = 30;
            cmbboxdevicegridviewfilterCateogory.Location = new Point(867, 160);
            cmbboxdevicegridviewfilterCateogory.Margin = new Padding(3, 2, 3, 2);
            cmbboxdevicegridviewfilterCateogory.Name = "cmbboxdevicegridviewfilterCateogory";
            cmbboxdevicegridviewfilterCateogory.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbboxdevicegridviewfilterCateogory.Size = new Size(217, 36);
            cmbboxdevicegridviewfilterCateogory.TabIndex = 1;
            cmbboxdevicegridviewfilterCateogory.SelectedIndexChanged += cmbboxdevicegridviewfilterCateogory_SelectedIndexChanged;
            // 
            // cmbboxdevicegridviewfilterValue
            // 
            cmbboxdevicegridviewfilterValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbboxdevicegridviewfilterValue.BackColor = Color.Transparent;
            cmbboxdevicegridviewfilterValue.CustomizableEdges = customizableEdges3;
            cmbboxdevicegridviewfilterValue.DrawMode = DrawMode.OwnerDrawFixed;
            cmbboxdevicegridviewfilterValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbboxdevicegridviewfilterValue.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbboxdevicegridviewfilterValue.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbboxdevicegridviewfilterValue.Font = new Font("Segoe UI", 10F);
            cmbboxdevicegridviewfilterValue.ForeColor = Color.FromArgb(68, 88, 112);
            cmbboxdevicegridviewfilterValue.ItemHeight = 30;
            cmbboxdevicegridviewfilterValue.Location = new Point(1102, 160);
            cmbboxdevicegridviewfilterValue.Margin = new Padding(3, 2, 3, 2);
            cmbboxdevicegridviewfilterValue.Name = "cmbboxdevicegridviewfilterValue";
            cmbboxdevicegridviewfilterValue.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbboxdevicegridviewfilterValue.Size = new Size(154, 36);
            cmbboxdevicegridviewfilterValue.TabIndex = 2;
            cmbboxdevicegridviewfilterValue.SelectedIndexChanged += cmbboxdevicegridviewfilterValue_SelectedIndexChanged;
            // 
            // searchboxdeviceSection
            // 
            searchboxdeviceSection.CustomizableEdges = customizableEdges5;
            searchboxdeviceSection.DefaultText = "";
            searchboxdeviceSection.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            searchboxdeviceSection.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            searchboxdeviceSection.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            searchboxdeviceSection.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            searchboxdeviceSection.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxdeviceSection.Font = new Font("Segoe UI", 9F);
            searchboxdeviceSection.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            searchboxdeviceSection.Location = new Point(28, 168);
            searchboxdeviceSection.Name = "searchboxdeviceSection";
            searchboxdeviceSection.PlaceholderText = "Search Device";
            searchboxdeviceSection.SelectedText = "";
            searchboxdeviceSection.ShadowDecoration.CustomizableEdges = customizableEdges6;
            searchboxdeviceSection.Size = new Size(192, 27);
            searchboxdeviceSection.TabIndex = 3;
            searchboxdeviceSection.TextChanged += searchboxdeviceSection_TextChanged;
            // 
            // DeviceGridViewSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(searchboxdeviceSection);
            Controls.Add(cmbboxdevicegridviewfilterValue);
            Controls.Add(cmbboxdevicegridviewfilterCateogory);
            Controls.Add(gridviewDevice);
            Margin = new Padding(3, 2, 3, 2);
            Name = "DeviceGridViewSection";
            Size = new Size(1346, 661);
            Load += DeviceGridViewSection_Load;
            ((System.ComponentModel.ISupportInitialize)gridviewDevice).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView gridviewDevice;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxdevicegridviewfilterCateogory;
        private Guna.UI2.WinForms.Guna2ComboBox cmbboxdevicegridviewfilterValue;
        private Guna.UI2.WinForms.Guna2TextBox searchboxdeviceSection;
    }
}
