namespace Presentation.Sections.Customers
{
    partial class CustomerSection
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerSection));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            lbldashboardTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnaddCustomer = new Guna.UI2.WinForms.Guna2Button();
            panelCustomerSection = new Guna.UI2.WinForms.Guna2Panel();
            panelCustomerSection.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Separator1
            // 
            guna2Separator1.Anchor = AnchorStyles.Top;
            guna2Separator1.FillColor = Color.FromArgb(224, 224, 224);
            guna2Separator1.Location = new Point(27, 116);
            guna2Separator1.Margin = new Padding(3, 2, 3, 2);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1284, 9);
            guna2Separator1.TabIndex = 1;
            // 
            // lbldashboardTitle
            // 
            lbldashboardTitle.AutoSize = false;
            lbldashboardTitle.BackColor = Color.Transparent;
            lbldashboardTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbldashboardTitle.Location = new Point(27, 82);
            lbldashboardTitle.Margin = new Padding(3, 2, 3, 2);
            lbldashboardTitle.Name = "lbldashboardTitle";
            lbldashboardTitle.Size = new Size(113, 32);
            lbldashboardTitle.TabIndex = 2;
            lbldashboardTitle.Text = "Customers";
            // 
            // btnaddCustomer
            // 
            btnaddCustomer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnaddCustomer.BorderRadius = 4;
            btnaddCustomer.CustomizableEdges = customizableEdges1;
            btnaddCustomer.DisabledState.BorderColor = Color.DarkGray;
            btnaddCustomer.DisabledState.CustomBorderColor = Color.DarkGray;
            btnaddCustomer.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnaddCustomer.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnaddCustomer.FillColor = Color.FromArgb(216, 30, 44);
            btnaddCustomer.Font = new Font("Segoe UI", 9F);
            btnaddCustomer.ForeColor = Color.White;
            btnaddCustomer.Image = (Image)resources.GetObject("btnaddCustomer.Image");
            btnaddCustomer.ImageAlign = HorizontalAlignment.Left;
            btnaddCustomer.Location = new Point(1114, 73);
            btnaddCustomer.Margin = new Padding(3, 2, 3, 2);
            btnaddCustomer.Name = "btnaddCustomer";
            btnaddCustomer.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnaddCustomer.Size = new Size(197, 39);
            btnaddCustomer.TabIndex = 4;
            btnaddCustomer.Text = "Add New Customer";
            btnaddCustomer.Click += btnaddCustomer_Click;
            // 
            // panelCustomerSection
            // 
            panelCustomerSection.Controls.Add(btnaddCustomer);
            panelCustomerSection.Controls.Add(guna2Separator1);
            panelCustomerSection.Controls.Add(lbldashboardTitle);
            panelCustomerSection.CustomizableEdges = customizableEdges3;
            panelCustomerSection.Dock = DockStyle.Fill;
            panelCustomerSection.Location = new Point(0, 0);
            panelCustomerSection.Margin = new Padding(3, 2, 3, 2);
            panelCustomerSection.Name = "panelCustomerSection";
            panelCustomerSection.ShadowDecoration.CustomizableEdges = customizableEdges4;
            panelCustomerSection.Size = new Size(1346, 595);
            panelCustomerSection.TabIndex = 5;
            // 
            // CustomerSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelCustomerSection);
            Margin = new Padding(3, 2, 3, 2);
            Name = "CustomerSection";
            Size = new Size(1346, 595);
            panelCustomerSection.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbldashboardTitle;
        private Guna.UI2.WinForms.Guna2Button btnaddCustomer;
        private Guna.UI2.WinForms.Guna2Panel panelCustomerSection;
    }
}
