namespace Presentation.Sections.Jobs
{
    partial class JobsSection
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelJobSection = new Guna.UI2.WinForms.Guna2Panel();
            btnaddJob = new Guna.UI2.WinForms.Guna2Button();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            panelJobSection.SuspendLayout();
            SuspendLayout();
            // 
            // panelJobSection
            // 
            panelJobSection.Controls.Add(btnaddJob);
            panelJobSection.Controls.Add(guna2Separator1);
            panelJobSection.Controls.Add(guna2HtmlLabel1);
            panelJobSection.CustomizableEdges = customizableEdges3;
            panelJobSection.Dock = DockStyle.Fill;
            panelJobSection.Location = new Point(0, 0);
            panelJobSection.Name = "panelJobSection";
            panelJobSection.ShadowDecoration.CustomizableEdges = customizableEdges4;
            panelJobSection.Size = new Size(1538, 793);
            panelJobSection.TabIndex = 0;
            // 
            // btnaddJob
            // 
            btnaddJob.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnaddJob.BorderRadius = 5;
            btnaddJob.CustomizableEdges = customizableEdges1;
            btnaddJob.DisabledState.BorderColor = Color.DarkGray;
            btnaddJob.DisabledState.CustomBorderColor = Color.DarkGray;
            btnaddJob.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnaddJob.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnaddJob.FillColor = Color.FromArgb(216, 30, 44);
            btnaddJob.Font = new Font("Segoe UI", 9F);
            btnaddJob.ForeColor = Color.White;
            btnaddJob.Image = Properties.Resources.Add;
            btnaddJob.Location = new Point(1248, 131);
            btnaddJob.Name = "btnaddJob";
            btnaddJob.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnaddJob.Size = new Size(225, 52);
            btnaddJob.TabIndex = 3;
            btnaddJob.Text = "Add New Job";
            btnaddJob.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            btnaddJob.Click += btnaddJob_Click;
            // 
            // guna2Separator1
            // 
            guna2Separator1.Anchor = AnchorStyles.Top;
            guna2Separator1.Location = new Point(26, 189);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1447, 12);
            guna2Separator1.TabIndex = 2;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(26, 144);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(61, 39);
            guna2HtmlLabel1.TabIndex = 1;
            guna2HtmlLabel1.Text = "Jobs";
            // 
            // JobsSection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelJobSection);
            Name = "JobsSection";
            Size = new Size(1538, 793);
            Load += JobsSection_Load;
            panelJobSection.ResumeLayout(false);
            panelJobSection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelJobSection;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button btnaddJob;
    }
}
