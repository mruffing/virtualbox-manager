namespace VBoxSysTray
{
  partial class HiddenForm
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
      this.StatusIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.cmsVirtualServer = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SuspendLayout();
      // 
      // StatusIcon
      // 
      this.StatusIcon.ContextMenuStrip = this.cmsVirtualServer;
      this.StatusIcon.Visible = true;
      // 
      // cmsVirtualServer
      // 
      this.cmsVirtualServer.Name = "cmsStatusIcon";
      this.cmsVirtualServer.Size = new System.Drawing.Size(61, 4);
      // 
      // HiddenForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.ControlBox = false;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "HiddenForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
      this.Load += new System.EventHandler(this.HiddenForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.NotifyIcon StatusIcon;
    private System.Windows.Forms.ContextMenuStrip cmsVirtualServer;
  }
}

