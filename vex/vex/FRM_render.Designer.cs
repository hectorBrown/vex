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
            this.PB_main = new System.Windows.Forms.PictureBox();
            this.TIM_rotate = new System.Windows.Forms.Timer(this.components);
            this.TIM_render = new System.Windows.Forms.Timer(this.components);
            this.TIM_fps = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PB_main)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_main
            // 
            this.PB_main.BackColor = System.Drawing.Color.Black;
            this.PB_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_main.Location = new System.Drawing.Point(0, 0);
            this.PB_main.Name = "PB_main";
            this.PB_main.Size = new System.Drawing.Size(800, 450);
            this.PB_main.TabIndex = 0;
            this.PB_main.TabStop = false;
            this.PB_main.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_main_Paint);
            this.PB_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_main_MouseDown);
            this.PB_main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_main_MouseUp);
            this.PB_main.Resize += new System.EventHandler(this.PB_main_Resize);
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
            // FRM_render
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PB_main);
            this.Name = "FRM_render";
            this.Text = "Vex";
            this.Load += new System.EventHandler(this.FRM_render_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PB_main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_main;
        private System.Windows.Forms.Timer TIM_rotate;
        private System.Windows.Forms.Timer TIM_render;
        private System.Windows.Forms.Timer TIM_fps;
    }
}

