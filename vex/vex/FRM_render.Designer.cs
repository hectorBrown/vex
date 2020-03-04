namespace vex
{
    partial class FRM_render
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRM_render));
            this.TIM_rotate = new System.Windows.Forms.Timer(this.components);
            this.TIM_render = new System.Windows.Forms.Timer(this.components);
            this.TIM_fps = new System.Windows.Forms.Timer(this.components);
            this.TSC_main = new System.Windows.Forms.ToolStripContainer();
            this.PB_main = new System.Windows.Forms.PictureBox();
            this.TS_main = new System.Windows.Forms.ToolStrip();
            this.TSB_new = new System.Windows.Forms.ToolStripButton();
            this.TSB_open = new System.Windows.Forms.ToolStripButton();
            this.TSB_save = new System.Windows.Forms.ToolStripButton();
            this.TSS_print_help = new System.Windows.Forms.ToolStripSeparator();
            this.TSB_help = new System.Windows.Forms.ToolStripButton();
            this.TSTXT_input = new System.Windows.Forms.ToolStripTextBox();
            this.TSB_input = new System.Windows.Forms.ToolStripButton();
            this.TSB_rotateLeft = new System.Windows.Forms.ToolStripButton();
            this.TSB_rotateLeftFine = new System.Windows.Forms.ToolStripButton();
            this.TSL_rotateSpeed = new System.Windows.Forms.ToolStripLabel();
            this.TSB_rotateRightFine = new System.Windows.Forms.ToolStripButton();
            this.TSB_rotateRight = new System.Windows.Forms.ToolStripButton();
            this.PN_data = new System.Windows.Forms.Panel();
            this.RTXT_data = new System.Windows.Forms.RichTextBox();
            this.SFD_main = new System.Windows.Forms.SaveFileDialog();
            this.OFD_main = new System.Windows.Forms.OpenFileDialog();
            this.TIM_autoRotate = new System.Windows.Forms.Timer(this.components);
            this.TSC_main.ContentPanel.SuspendLayout();
            this.TSC_main.TopToolStripPanel.SuspendLayout();
            this.TSC_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_main)).BeginInit();
            this.TS_main.SuspendLayout();
            this.PN_data.SuspendLayout();
            this.SuspendLayout();
            // 
            // TIM_rotate
            // 
            this.TIM_rotate.Interval = 10;
            this.TIM_rotate.Tick += new System.EventHandler(this.TIM_rotate_Tick);
            // 
            // TIM_render
            // 
            this.TIM_render.Enabled = true;
            this.TIM_render.Interval = 10;
            this.TIM_render.Tick += new System.EventHandler(this.TIM_render_Tick);
            // 
            // TIM_fps
            // 
            this.TIM_fps.Enabled = true;
            this.TIM_fps.Interval = 1000;
            this.TIM_fps.Tick += new System.EventHandler(this.TIM_fps_Tick);
            // 
            // TSC_main
            // 
            // 
            // TSC_main.ContentPanel
            // 
            this.TSC_main.ContentPanel.Controls.Add(this.PB_main);
            this.TSC_main.ContentPanel.Size = new System.Drawing.Size(694, 479);
            this.TSC_main.Dock = System.Windows.Forms.DockStyle.Left;
            this.TSC_main.Location = new System.Drawing.Point(0, 0);
            this.TSC_main.Name = "TSC_main";
            this.TSC_main.Size = new System.Drawing.Size(694, 504);
            this.TSC_main.TabIndex = 2;
            this.TSC_main.Text = "toolStripContainer1";
            // 
            // TSC_main.TopToolStripPanel
            // 
            this.TSC_main.TopToolStripPanel.Controls.Add(this.TS_main);
            // 
            // PB_main
            // 
            this.PB_main.BackColor = System.Drawing.Color.Black;
            this.PB_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_main.Location = new System.Drawing.Point(0, 0);
            this.PB_main.Margin = new System.Windows.Forms.Padding(2);
            this.PB_main.Name = "PB_main";
            this.PB_main.Size = new System.Drawing.Size(694, 479);
            this.PB_main.TabIndex = 1;
            this.PB_main.TabStop = false;
            this.PB_main.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_main_Paint);
            this.PB_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_main_MouseDown);
            this.PB_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_main_MouseUp);
            // 
            // TS_main
            // 
            this.TS_main.Dock = System.Windows.Forms.DockStyle.None;
            this.TS_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_new,
            this.TSB_open,
            this.TSB_save,
            this.TSS_print_help,
            this.TSB_help,
            this.TSTXT_input,
            this.TSB_input,
            this.TSB_rotateLeft,
            this.TSB_rotateLeftFine,
            this.TSL_rotateSpeed,
            this.TSB_rotateRightFine,
            this.TSB_rotateRight});
            this.TS_main.Location = new System.Drawing.Point(3, 0);
            this.TS_main.Name = "TS_main";
            this.TS_main.Size = new System.Drawing.Size(401, 25);
            this.TS_main.TabIndex = 0;
            // 
            // TSB_new
            // 
            this.TSB_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_new.Image = ((System.Drawing.Image)(resources.GetObject("TSB_new.Image")));
            this.TSB_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_new.Name = "TSB_new";
            this.TSB_new.Size = new System.Drawing.Size(23, 22);
            this.TSB_new.Text = "&New";
            this.TSB_new.Click += new System.EventHandler(this.TSB_new_Click);
            // 
            // TSB_open
            // 
            this.TSB_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_open.Image = ((System.Drawing.Image)(resources.GetObject("TSB_open.Image")));
            this.TSB_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_open.Name = "TSB_open";
            this.TSB_open.Size = new System.Drawing.Size(23, 22);
            this.TSB_open.Text = "&Open";
            this.TSB_open.Click += new System.EventHandler(this.TSB_open_Click);
            // 
            // TSB_save
            // 
            this.TSB_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_save.Image = ((System.Drawing.Image)(resources.GetObject("TSB_save.Image")));
            this.TSB_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_save.Name = "TSB_save";
            this.TSB_save.Size = new System.Drawing.Size(23, 22);
            this.TSB_save.Text = "&Save";
            this.TSB_save.Click += new System.EventHandler(this.TSB_save_Click);
            // 
            // TSS_print_help
            // 
            this.TSS_print_help.Name = "TSS_print_help";
            this.TSS_print_help.Size = new System.Drawing.Size(6, 25);
            // 
            // TSB_help
            // 
            this.TSB_help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_help.Image = ((System.Drawing.Image)(resources.GetObject("TSB_help.Image")));
            this.TSB_help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_help.Name = "TSB_help";
            this.TSB_help.Size = new System.Drawing.Size(23, 22);
            this.TSB_help.Text = "He&lp";
            this.TSB_help.Click += new System.EventHandler(this.TSB_help_Click);
            // 
            // TSTXT_input
            // 
            this.TSTXT_input.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TSTXT_input.Name = "TSTXT_input";
            this.TSTXT_input.Size = new System.Drawing.Size(100, 25);
            // 
            // TSB_input
            // 
            this.TSB_input.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSB_input.Image = ((System.Drawing.Image)(resources.GetObject("TSB_input.Image")));
            this.TSB_input.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_input.Name = "TSB_input";
            this.TSB_input.Size = new System.Drawing.Size(76, 22);
            this.TSB_input.Text = "Enter Object";
            this.TSB_input.Click += new System.EventHandler(this.TSB_input_Click);
            // 
            // TSB_rotateLeft
            // 
            this.TSB_rotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSB_rotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("TSB_rotateLeft.Image")));
            this.TSB_rotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_rotateLeft.Name = "TSB_rotateLeft";
            this.TSB_rotateLeft.Size = new System.Drawing.Size(27, 22);
            this.TSB_rotateLeft.Text = "<<";
            this.TSB_rotateLeft.Click += new System.EventHandler(this.TSB_rotateLeft_Click);
            // 
            // TSB_rotateLeftFine
            // 
            this.TSB_rotateLeftFine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSB_rotateLeftFine.Image = ((System.Drawing.Image)(resources.GetObject("TSB_rotateLeftFine.Image")));
            this.TSB_rotateLeftFine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_rotateLeftFine.Name = "TSB_rotateLeftFine";
            this.TSB_rotateLeftFine.Size = new System.Drawing.Size(23, 22);
            this.TSB_rotateLeftFine.Text = "<";
            this.TSB_rotateLeftFine.Click += new System.EventHandler(this.TSB_rotateLeftFine_Click);
            // 
            // TSL_rotateSpeed
            // 
            this.TSL_rotateSpeed.Name = "TSL_rotateSpeed";
            this.TSL_rotateSpeed.Size = new System.Drawing.Size(13, 22);
            this.TSL_rotateSpeed.Text = "0";
            // 
            // TSB_rotateRightFine
            // 
            this.TSB_rotateRightFine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSB_rotateRightFine.Image = ((System.Drawing.Image)(resources.GetObject("TSB_rotateRightFine.Image")));
            this.TSB_rotateRightFine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_rotateRightFine.Name = "TSB_rotateRightFine";
            this.TSB_rotateRightFine.Size = new System.Drawing.Size(23, 22);
            this.TSB_rotateRightFine.Text = ">";
            this.TSB_rotateRightFine.Click += new System.EventHandler(this.TSB_rotateRightFine_Click);
            // 
            // TSB_rotateRight
            // 
            this.TSB_rotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSB_rotateRight.Image = ((System.Drawing.Image)(resources.GetObject("TSB_rotateRight.Image")));
            this.TSB_rotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_rotateRight.Name = "TSB_rotateRight";
            this.TSB_rotateRight.Size = new System.Drawing.Size(27, 22);
            this.TSB_rotateRight.Text = ">>";
            this.TSB_rotateRight.Click += new System.EventHandler(this.TSB_rotateRight_Click);
            // 
            // PN_data
            // 
            this.PN_data.Controls.Add(this.RTXT_data);
            this.PN_data.Dock = System.Windows.Forms.DockStyle.Right;
            this.PN_data.Location = new System.Drawing.Point(694, 0);
            this.PN_data.Name = "PN_data";
            this.PN_data.Size = new System.Drawing.Size(250, 504);
            this.PN_data.TabIndex = 3;
            // 
            // RTXT_data
            // 
            this.RTXT_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTXT_data.Location = new System.Drawing.Point(0, 0);
            this.RTXT_data.Name = "RTXT_data";
            this.RTXT_data.ReadOnly = true;
            this.RTXT_data.Size = new System.Drawing.Size(250, 504);
            this.RTXT_data.TabIndex = 0;
            this.RTXT_data.Text = "";
            // 
            // SFD_main
            // 
            this.SFD_main.FileName = "system.VEXSYS";
            this.SFD_main.Filter = "Vex files|*.VEXSYS";
            this.SFD_main.FileOk += new System.ComponentModel.CancelEventHandler(this.SFD_main_FileOk);
            // 
            // OFD_main
            // 
            this.OFD_main.Filter = "Vex files|*.VEXSYS";
            this.OFD_main.FileOk += new System.ComponentModel.CancelEventHandler(this.OFD_main_FileOk);
            // 
            // TIM_autoRotate
            // 
            this.TIM_autoRotate.Enabled = true;
            this.TIM_autoRotate.Interval = 10;
            this.TIM_autoRotate.Tick += new System.EventHandler(this.TIM_autoRotate_Tick);
            // 
            // FRM_render
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 504);
            this.Controls.Add(this.PN_data);
            this.Controls.Add(this.TSC_main);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FRM_render";
            this.Text = "Vex";
            this.Load += new System.EventHandler(this.FRM_render_Load);
            this.TSC_main.ContentPanel.ResumeLayout(false);
            this.TSC_main.TopToolStripPanel.ResumeLayout(false);
            this.TSC_main.TopToolStripPanel.PerformLayout();
            this.TSC_main.ResumeLayout(false);
            this.TSC_main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_main)).EndInit();
            this.TS_main.ResumeLayout(false);
            this.TS_main.PerformLayout();
            this.PN_data.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer TIM_rotate;
        private System.Windows.Forms.Timer TIM_render;
        private System.Windows.Forms.Timer TIM_fps;
        private System.Windows.Forms.ToolStripContainer TSC_main;
        private System.Windows.Forms.PictureBox PB_main;
        private System.Windows.Forms.ToolStrip TS_main;
        private System.Windows.Forms.ToolStripButton TSB_new;
        private System.Windows.Forms.ToolStripButton TSB_open;
        private System.Windows.Forms.ToolStripButton TSB_save;
        private System.Windows.Forms.ToolStripSeparator TSS_print_help;
        private System.Windows.Forms.ToolStripButton TSB_help;
        private System.Windows.Forms.ToolStripTextBox TSTXT_input;
        private System.Windows.Forms.ToolStripButton TSB_input;
        private System.Windows.Forms.Panel PN_data;
        private System.Windows.Forms.RichTextBox RTXT_data;
        private System.Windows.Forms.SaveFileDialog SFD_main;
        private System.Windows.Forms.OpenFileDialog OFD_main;
        private System.Windows.Forms.ToolStripButton TSB_rotateLeft;
        private System.Windows.Forms.ToolStripButton TSB_rotateLeftFine;
        private System.Windows.Forms.ToolStripLabel TSL_rotateSpeed;
        private System.Windows.Forms.ToolStripButton TSB_rotateRightFine;
        private System.Windows.Forms.ToolStripButton TSB_rotateRight;
        private System.Windows.Forms.Timer TIM_autoRotate;
    }
}

