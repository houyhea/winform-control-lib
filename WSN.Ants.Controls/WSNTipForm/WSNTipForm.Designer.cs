namespace WSN.Ants.Controls.WSNTipForm
{
    partial class WSNTipForm
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
            WSN.Ants.Controls.FaceStyle faceStyle1 = new WSN.Ants.Controls.FaceStyle();
            WSN.Ants.Controls.BrushParameter brushParameter1 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter2 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter3 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter4 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter5 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter6 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter7 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter8 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.BrushParameter brushParameter9 = new WSN.Ants.Controls.BrushParameter();
            WSN.Ants.Controls.WSNButton.WSNButtonImageTable wsnButtonImageTable1 = new WSN.Ants.Controls.WSNButton.WSNButtonImageTable();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.closeButton = new WSN.Ants.Controls.WSNButton.WSNButton();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closeButton.Checked = false;
            brushParameter1.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(214)))), ((int)(((byte)(125)))));
            brushParameter1.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(241)))), ((int)(((byte)(211)))));
            brushParameter1.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BackCheckedStyle = brushParameter1;
            brushParameter2.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            brushParameter2.Color2 = System.Drawing.Color.Empty;
            brushParameter2.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BackDisabledStyle = brushParameter2;
            faceStyle1.BackHoverStyle = brushParameter1;
            brushParameter3.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            brushParameter3.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            brushParameter3.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BackNormalStyle = brushParameter3;
            brushParameter4.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            brushParameter4.Color2 = System.Drawing.Color.Empty;
            brushParameter4.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BorderCheckedStyle = brushParameter4;
            brushParameter5.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            brushParameter5.Color2 = System.Drawing.Color.Empty;
            brushParameter5.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BorderDisabledStyle = brushParameter5;
            faceStyle1.BorderHoverStyle = brushParameter4;
            brushParameter6.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            brushParameter6.Color2 = System.Drawing.Color.Empty;
            brushParameter6.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.BorderNormalStyle = brushParameter6;
            brushParameter7.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(88)))), ((int)(((byte)(7)))));
            brushParameter7.Color2 = System.Drawing.Color.Empty;
            brushParameter7.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.TextCheckedStyle = brushParameter7;
            brushParameter8.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            brushParameter8.Color2 = System.Drawing.Color.Empty;
            brushParameter8.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.TextDisabledStyle = brushParameter8;
            faceStyle1.TextHoverStyle = brushParameter7;
            brushParameter9.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            brushParameter9.Color2 = System.Drawing.Color.Empty;
            brushParameter9.Mode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            faceStyle1.TextNormalStyle = brushParameter9;
            this.closeButton.ControlFaceSchema = faceStyle1;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.GlassEnable = true;
            this.closeButton.ImageMode = true;
            wsnButtonImageTable1.ImageChecked = null;
            wsnButtonImageTable1.ImageDisalbed = null;
            wsnButtonImageTable1.ImageHover = null;
            wsnButtonImageTable1.ImageNormal = null;
            this.closeButton.ImageTable = wsnButtonImageTable1;
            this.closeButton.Location = new System.Drawing.Point(205, 1);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "wsnButton1";
            this.closeButton.ToolTipText = "";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // WSNTipForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.closeButton);
            this.Name = "WSNTipForm";
            this.Text = "WSNTipForm";
            this.Shown += new System.EventHandler(this.WSNTipForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private WSNButton.WSNButton closeButton;
        private System.Windows.Forms.Timer timer1;
    }
}