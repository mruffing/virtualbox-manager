using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VBoxAPI;
using System.Windows.Forms;
using VBoxSysTray.Properties;

namespace VBoxSysTray
{
  
  public class MenuItemPresenter
  {
    private static readonly string PowerOffText = "Power Off";
    private static readonly string PowerOnText = "Power On";

    private VirtualServer.State vServerState;
    private ToolStripMenuItem tsmiOnOff;
    private ToolStripMenuItem tsmiRDP;
    

    public ToolStripMenuItem RootMenuItem
    { get; private set; }

    public VirtualServer VirtualServer
    { get; private set; }

    public MenuItemPresenter(VirtualServer vServer)
      : this(vServer, vServer.Status)
    { }

    public MenuItemPresenter(VirtualServer vServer, VirtualServer.State vServerState)
    {
      VirtualServer = vServer;
    
      //Build the root and sub-menu items
      BuildMenuItemStructure();

      //Update menu items to the current state
      Update(vServerState);
    }

    public void Update(VirtualServer.State vServerState)
    {
      this.vServerState = vServerState;
      UpdateRootMenuImage();
      UpdateTsmiOnOffText();
    }

    private void BuildMenuItemStructure()
    {
      RootMenuItem = new ToolStripMenuItem(VirtualServer.Name);

      tsmiOnOff = new ToolStripMenuItem();
      tsmiOnOff.Click += TsmiOnOffClickEvent;
      RootMenuItem.DropDownItems.Add(tsmiOnOff);

      if (VirtualServer.VRDEProperty.IsEnabled)
      {
        tsmiRDP = new ToolStripMenuItem("RDP Connect");
        tsmiRDP.Click += (obj, e) => MstscService.Connect("localhost", VirtualServer.VRDEProperty.Port, 1024, 700);
        RootMenuItem.DropDownItems.Add(tsmiRDP);
      }
    }

    private void UpdateRootMenuImage()
    {
      switch (vServerState)
      {
        case VirtualServer.State.Unknown:
          RootMenuItem.Image = ResourceCache.UnknownImage;
          break;
        case VirtualServer.State.Poweroff:
          RootMenuItem.Image = ResourceCache.PoweroffImage;
          break;
        case VirtualServer.State.Running:
          RootMenuItem.Image = ResourceCache.RunningImage;
          break;
        default:
          break;
      }
    }

    private void UpdateTsmiOnOffText()
    {
      if (vServerState == VirtualServer.State.Running)
      {
        tsmiOnOff.Text = PowerOffText;
      }
      else
      {
        tsmiOnOff.Text = PowerOnText;
      }
    }

    private void TsmiOnOffClickEvent(object obj, EventArgs e)
    {
      if (vServerState == VirtualServer.State.Running)
      {
        VirtualServer.Stop();
      }
      else
      {
        VirtualServer.Start();
      }
    }

  }
}
