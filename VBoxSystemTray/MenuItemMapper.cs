using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBoxAPI;
using System.Drawing;
using VBoxSysTray.Properties;
using System.ComponentModel;

namespace VBoxSysTray
{
  public class MenuItemMapper
  {
    private Dictionary<VirtualServer, MenuItemPresenter> mapping = new Dictionary<VirtualServer, MenuItemPresenter>();
    private Action<VirtualServer, VirtualServer.State> updateWithoutThreadChecksAction;
    private ISynchronizeInvoke dispatcher;

    public MenuItemMapper(ISynchronizeInvoke dispatcher)
    {
      this.dispatcher = dispatcher;
      this.updateWithoutThreadChecksAction = UpdateWithoutThreadChecks;
    }

    public void Register(MenuItemPresenter vServerMenuItem)
    {
      mapping.Add(vServerMenuItem.VirtualServer, vServerMenuItem);
    }

    public void Update(VirtualServer vServer, VirtualServer.State state)
    {
      if (dispatcher.InvokeRequired)
      {
        dispatcher.Invoke(updateWithoutThreadChecksAction, new object[] { vServer, state });
      }
      else
      {
        UpdateWithoutThreadChecks(vServer, state);
      }
    }

    private void UpdateWithoutThreadChecks(VirtualServer vServer, VirtualServer.State state)
    {
      mapping[vServer].Update(state);
    }
  }
}
