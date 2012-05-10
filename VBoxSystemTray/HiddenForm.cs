using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBoxAPI;
using System.Collections;
using System.Reflection;
using VBoxSysTray.Properties;

namespace VBoxSysTray
{
  public partial class HiddenForm : Form
  {
    private VirtualServerMonitor serverMonitor;
    private MenuItemImageUpdater menuItemImageUpdater = new MenuItemImageUpdater();

    public HiddenForm()
    {
      InitializeComponent();

      InitializeIcons();

      LocateVirtualServers();

      AddStaticContextMenuItems();
    }

    private void InitializeIcons()
    {
      StatusIcon.Icon = Resources.virtualbox; 
    }

    private void LocateVirtualServers()
    {
      var servers = VBoxService.GetVirtualServers().ToList();

      if (servers.Count > 0)
      {

        foreach (VirtualServer vServer in servers)
        {

          AddVirtualServerContextMenuItems(vServer);
        }

        RegisterForStateChanges(servers);
      }
      else
      {
        //No servers found, display error on ballon tip
      }


    }

    private void AddStaticContextMenuItems()
    {
      cmsVirtualServer.Items.Add(new ToolStripSeparator());

      ToolStripMenuItem tsmiVirtualBoxGui = new ToolStripMenuItem("VirtualBox");
      tsmiVirtualBoxGui.Click += (obj, e) => VBoxService.StartVirtualBoxGui();
      cmsVirtualServer.Items.Add(tsmiVirtualBoxGui);

      cmsVirtualServer.Items.Add(new ToolStripSeparator());

      ToolStripMenuItem tsmiExit = new ToolStripMenuItem("Exit");
      tsmiExit.Click += (obj, e) => this.Close();
      cmsVirtualServer.Items.Add(tsmiExit);
    }

    private void AddVirtualServerContextMenuItems(VirtualServer vServer)
    {
      ToolStripMenuItem tsmiRoot = new ToolStripMenuItem(vServer.Name);
      tsmiRoot.Image = Resources.unknown.ToBitmap();
      tsmiRoot.CheckOnClick = true;

      ToolStripMenuItem tsmiStart = new ToolStripMenuItem("Start");
      tsmiStart.Click += (obj, e) => vServer.Start();
      tsmiRoot.DropDownItems.Add(tsmiStart);

      ToolStripMenuItem tsmiStop = new ToolStripMenuItem("Stop");
      tsmiStop.Click += (obje, e) => vServer.Stop();
      tsmiRoot.DropDownItems.Add(tsmiStop);

      cmsVirtualServer.Items.Add(tsmiRoot);

      menuItemImageUpdater.Register(tsmiRoot, vServer);

    }

    private void RegisterForStateChanges(IEnumerable<VirtualServer> servers)
    {
      serverMonitor = new VirtualServerMonitor(servers);
      serverMonitor.StateChanged += menuItemImageUpdater.UpdateImage;
      serverMonitor.Rate = 5000;
      serverMonitor.Start();
    }

    

    private void UpdateSystemTrayIcon(VirtualServer server, VirtualServer.State state)
    {
      switch (state)
      {
        case VirtualServer.State.Unknown:
          StatusIcon.Icon = Resources.unknown;
          StatusIcon.Text = String.Format("{0} unknown", server.Name);
          break;
        case VirtualServer.State.Poweroff:
          StatusIcon.Icon = Resources.poweroff;
          StatusIcon.Text = String.Format("{0} poweroff", server.Name);
          break;
        case VirtualServer.State.Running:
          StatusIcon.Icon = Resources.running;
          StatusIcon.Text = String.Format("{0} running", server.Name);
          break;
        default:
          break;
      }
    }

    private void HiddenForm_Load(object sender, EventArgs e)
    {
      this.Hide();
    }
  }
}
