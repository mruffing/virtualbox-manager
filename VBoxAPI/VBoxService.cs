using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VBoxAPI
{

  public class VBoxService
  {
    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    public static IEnumerable<VirtualServer> GetVirtualServers()
    {
      return GetVirtualServerNames().Select(name => new VirtualServer(name));
    }

    public static void StartVirtualBoxGui()
    {
      //Only start a new process if VirtualBox(GUI) is not running
      Process virtualBoxGuiProcess = Process.GetProcessesByName("virtualbox").FirstOrDefault();

      if (virtualBoxGuiProcess != null)
      {
        SetForegroundWindow(virtualBoxGuiProcess.MainWindowHandle);
      }
      else
      {

        Process p = new Process();

        p.StartInfo.WorkingDirectory = VBoxPaths.Oracle_VirtualBox_Directory;
        p.StartInfo.FileName = VBoxPaths.Oracle_VirutalBox_Exe;
        p.Start();
      }
    }

    private static IEnumerable<String> GetVirtualServerNames()
    {
      List<String> serverNames = new List<String>();
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = VBoxPaths.Oracle_VirtualBox_VBoxManage_Exe;
      p.StartInfo.Arguments = @"list vms";
      p.Start();

      p.WaitForExit();

      foreach (String server in p.StandardOutput.ReadToEnd().Split('\n'))
      {
        string serverNameId = server.Trim();
        if (serverNameId != String.Empty)
        {
          serverNames.Add(serverNameId.Split('"')[1]);
        }
      }

      return serverNames;
    }
  }
}
