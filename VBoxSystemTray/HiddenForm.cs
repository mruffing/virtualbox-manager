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
    private NotifyIconBalloonService balloonService;

    public HiddenForm()
    {
      InitializeComponent();

      InitializeServices();

      InitializeIcons();

      LocateVirtualServers();

      AddStaticContextMenuItems();
    }

    private void InitializeServices()
    {
      balloonService = new NotifyIconBalloonService(StatusIcon);
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

        string status = String.Format("Managing {0} virtual {1}", servers.Count, servers.Count == 1 ? "machine" : "machines");
        balloonService.DisplayInfo(status);
      }
      else
      {
        balloonService.DisplayError("No virtual machines located");
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
      serverMonitor.StateChanged += UpdateNotifyIconBalloon;
      serverMonitor.Rate = 5000;
      serverMonitor.Start();
    }

    

    private void UpdateNotifyIconBalloon(VirtualServer server, VirtualServer.State state)
    {
      switch (state)
      {
        case VirtualServer.State.Unknown:
          balloonService.DisplayWarning(String.Format("{0} is in an uknown state", server.Name));
          break;
        case VirtualServer.State.Poweroff:
          balloonService.DisplayInfo(String.Format("{0} has been powered off", server.Name));
          break;
        case VirtualServer.State.Running:
          balloonService.DisplayInfo(String.Format("{0} is running", server.Name));
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
