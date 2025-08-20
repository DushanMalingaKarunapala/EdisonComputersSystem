namespace Presentation.Sections.Components
{
    partial class ComponentSection
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
            panelComponentSection = new Guna.UI2.WinForms.Guna2Panel();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            btnaddComponents = new Guna.UI2.WinForms.Guna2Button();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            panelComponentSection.SuspendLayout();
            SuspendLayout();
            // 
            // panelComponentSection
            // 
            panelComponentSection.Controls.Add(guna2Separator1);
            panelComponentSection.Controls.Add(btnaddComponents);
            panelComponentSection.Controls.Add(guna2HtmlLabel1);
            panelComponentSection.CustomizableEdges = customizableEdges3;
            panelComponentSection.Dock = DockStyle.Fill;
            panelComponentSection.Location = new Point(0, 0);
            panelComponentSection.Name = "panelComponentSection";
            panelComponentSection.ShadowDecoration.CustomizableEdges = customizableEdges4;
            panelComponentSection.Size = new Size(1538, 793);
            panelComponentSection.TabIndex = 0;
            // 
            // guna2Separator1
            // 
            guna2Separator1.Anchor = AnchorStyles.Top;
            guna2Separator1.Location = new Point(28, 190);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(1421, 12);
            guna2Separator1.TabIndex = 2;
            // 
            // btnaddComponents
            // 
            btnaddComponents.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnaddComponents.CustomizableEdges = customizableEdges1;
            btnaddComponents.DisabledState.BorderColor = Color.DarkGray;
            btnaddComponents.DisabledState.CustomBorderColor = Color.DarkGray;
            btnaddComponents.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnaddComponents.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnaddComponents.FillColor = Color.FromArgb(216, 30, 44);
            btnaddComponents.Font = new Font("Segoe UI", 9F);
            btnaddComponents.ForeColor = Color.White;
            btnaddComponents.Location = new Point(1233, 138);
            btnaddComponents.Name = "btnaddComponents";
            btnaddComponents.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnaddComponents.Size = new Size(225, 52);
            btnaddComponents.TabIndex = 1;
            btnaddComponents.Text = "Add Component";
            btnaddComponents.Click += btnaddComponents_Click;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(28, 138);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(139, 33);
            guna2HtmlLabel1.TabIndex = 0;
            guna2HtmlLabel1.Text = "Components";
            // 
            // ComponentSection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelComponentSection);
            Name = "ComponentSection";
            Size = new Size(1538, 793);
            panelComponentSection.ResumeLayout(false);
            panelComponentSection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelComponentSection;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button btnaddComponents;
    }
}
