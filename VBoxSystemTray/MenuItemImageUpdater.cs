using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBoxAPI;
using System.Drawing;
using VBoxSysTray.Properties;

namespace VBoxSysTray
{
  public class MenuItemImageUpdater
  {
    private Dictionary<VirtualServer, ToolStripMenuItem> mapping = new Dictionary<VirtualServer, ToolStripMenuItem>();

    private static readonly Bitmap unknown = Resources.unknown.ToBitmap();
    private static readonly Bitmap poweroff = Resources.poweroff.ToBitmap();
    private static readonly Bitmap running = Resources.running.ToBitmap();

    public void Register(ToolStripMenuItem tsmi, VirtualServer vServer)
    {
      mapping.Add(vServer, tsmi);
    }

    public ToolStripMenuItem Get(VirtualServer vServer)
    {
      return mapping[vServer];
    }

    public void UpdateImage(VirtualServer vServer, VirtualServer.State state)
    {
      ToolStripMenuItem tsmi = mapping[vServer];

      switch (state)
      {
        case VirtualServer.State.Unknown:
          tsmi.Image = unknown;
          break;
        case VirtualServer.State.Poweroff:
          tsmi.Image = poweroff;
          break;
        case VirtualServer.State.Running:
          tsmi.Image = running;
          break;
        default:
          break;
      }

    }
  }
}
