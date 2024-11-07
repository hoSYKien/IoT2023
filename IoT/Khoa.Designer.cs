namespace IoT
{
    partial class Khoa
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gunaAreaDataset1 = new Guna.Charts.WinForms.GunaAreaDataset();
            this.gunaAreaDataset2 = new Guna.Charts.WinForms.GunaAreaDataset();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gunaAreaDataset1
            // 
            this.gunaAreaDataset1.BorderColor = System.Drawing.Color.Empty;
            this.gunaAreaDataset1.FillColor = System.Drawing.Color.Empty;
            this.gunaAreaDataset1.Label = "Area1";
            // 
            // gunaAreaDataset2
            // 
            this.gunaAreaDataset2.BorderColor = System.Drawing.Color.Empty;
            this.gunaAreaDataset2.FillColor = System.Drawing.Color.Empty;
            this.gunaAreaDataset2.Label = "Area2";
            // 
            // Khoa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Linen;
            this.Name = "Khoa";
            this.Size = new System.Drawing.Size(1439, 663);
            this.Load += new System.EventHandler(this.Khoa_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.Charts.WinForms.GunaAreaDataset gunaAreaDataset1;
        private Guna.Charts.WinForms.GunaAreaDataset gunaAreaDataset2;
        private System.Windows.Forms.Timer timer1;
    }
}