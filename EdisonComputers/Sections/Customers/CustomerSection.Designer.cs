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
            guna2Separator1.Location = new Point(19, 69);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1450, 12);
            guna2Separator1.TabIndex = 1;
            // 
            // lbldashboardTitle
            // 
            lbldashboardTitle.Anchor = AnchorStyles.Left;
            lbldashboardTitle.BackColor = Color.Transparent;
            lbldashboardTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbldashboardTitle.Location = new Point(19, 24);
            lbldashboardTitle.Name = "lbldashboardTitle";
            lbldashboardTitle.Size = new Size(139, 39);
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
            btnaddCustomer.Location = new Point(1244, 14);
            btnaddCustomer.Name = "btnaddCustomer";
            btnaddCustomer.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnaddCustomer.Size = new Size(225, 52);
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
            panelCustomerSection.Name = "panelCustomerSection";
            panelCustomerSection.ShadowDecoration.CustomizableEdges = customizableEdges4;
            panelCustomerSection.Size = new Size(1538, 793);
            panelCustomerSection.TabIndex = 5;
            // 
            // CustomerSection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelCustomerSection);
            Name = "CustomerSection";
            Size = new Size(1538, 793);
            panelCustomerSection.ResumeLayout(false);
            panelCustomerSection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbldashboardTitle;
        private Guna.UI2.WinForms.Guna2Button btnaddCustomer;
        private Guna.UI2.WinForms.Guna2Panel panelCustomerSection;
    }
}
