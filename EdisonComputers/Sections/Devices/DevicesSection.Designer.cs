namespace Presentation.Sections.Devices
{
    partial class DevicesSection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicesSection));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lbldashboardTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            btnaddDevice = new Guna.UI2.WinForms.Guna2Button();
            paneldeviceSection = new Guna.UI2.WinForms.Guna2Panel();
            paneldeviceSection.SuspendLayout();
            SuspendLayout();
            // 
            // lbldashboardTitle
            // 
            lbldashboardTitle.AutoSize = false;
            lbldashboardTitle.BackColor = Color.Transparent;
            lbldashboardTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbldashboardTitle.Location = new Point(27, 105);
            lbldashboardTitle.Margin = new Padding(3, 2, 3, 2);
            lbldashboardTitle.Name = "lbldashboardTitle";
            lbldashboardTitle.Size = new Size(82, 32);
            lbldashboardTitle.TabIndex = 4;
            lbldashboardTitle.Text = "Devices";
            // 
            // guna2Separator1
            // 
            guna2Separator1.Anchor = AnchorStyles.Top;
            guna2Separator1.FillColor = Color.FromArgb(224, 224, 224);
            guna2Separator1.Location = new Point(27, 139);
            guna2Separator1.Margin = new Padding(3, 2, 3, 2);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1267, 9);
            guna2Separator1.TabIndex = 3;
            // 
            // btnaddDevice
            // 
            btnaddDevice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnaddDevice.BorderRadius = 4;
            btnaddDevice.CustomizableEdges = customizableEdges1;
            btnaddDevice.DisabledState.BorderColor = Color.DarkGray;
            btnaddDevice.DisabledState.CustomBorderColor = Color.DarkGray;
            btnaddDevice.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnaddDevice.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnaddDevice.FillColor = Color.FromArgb(216, 30, 44);
            btnaddDevice.Font = new Font("Segoe UI", 9F);
            btnaddDevice.ForeColor = Color.White;
            btnaddDevice.Image = (Image)resources.GetObject("btnaddDevice.Image");
            btnaddDevice.ImageAlign = HorizontalAlignment.Left;
            btnaddDevice.Location = new Point(1107, 95);
            btnaddDevice.Margin = new Padding(3, 2, 3, 2);
            btnaddDevice.Name = "btnaddDevice";
            btnaddDevice.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnaddDevice.Size = new Size(197, 39);
            btnaddDevice.TabIndex = 6;
            btnaddDevice.Text = "Add New Device";
            btnaddDevice.Click += btnaddDevice_Click;
            // 
            // paneldeviceSection
            // 
            paneldeviceSection.Controls.Add(lbldashboardTitle);
            paneldeviceSection.Controls.Add(btnaddDevice);
            paneldeviceSection.Controls.Add(guna2Separator1);
            paneldeviceSection.CustomizableEdges = customizableEdges3;
            paneldeviceSection.Dock = DockStyle.Fill;
            paneldeviceSection.Location = new Point(0, 0);
            paneldeviceSection.Margin = new Padding(3, 2, 3, 2);
            paneldeviceSection.Name = "paneldeviceSection";
            paneldeviceSection.ShadowDecoration.CustomizableEdges = customizableEdges4;
            paneldeviceSection.Size = new Size(1346, 595);
            paneldeviceSection.TabIndex = 7;
            // 
            // DevicesSection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(paneldeviceSection);
            Margin = new Padding(3, 2, 3, 2);
            Name = "DevicesSection";
            Size = new Size(1346, 595);
            paneldeviceSection.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lbldashboardTitle;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button btnaddDevice;
        private Guna.UI2.WinForms.Guna2Panel paneldeviceSection;
    }
}
