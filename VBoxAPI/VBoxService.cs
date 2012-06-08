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

    public static void StartVirtualMachine(string vmName)
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = VBoxPaths.Oracle_VirtualBox_VBoxHeadless_Exe;
      p.StartInfo.Arguments = String.Format("--startvm \"{0}\"", vmName);

      p.Start();
    }

    public static void AcpiPowerOffVirtualMachine(string vmName)
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = VBoxPaths.Oracle_VirtualBox_VBoxManage_Exe;
      p.StartInfo.Arguments = String.Format("controlvm \"{0}\" acpipowerbutton", vmName);
      p.Start();
      p.WaitForExit();
    }

    public static string GetVirtualMachineInfo(string vmName)
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = VBoxPaths.Oracle_VirtualBox_VBoxManage_Exe;
      p.StartInfo.Arguments = String.Format("showvminfo \"{0}\" --machinereadable", vmName);
      p.Start();

      p.WaitForExit();

      return p.StandardOutput.ReadToEnd();
    }

    public static IEnumerable<String> GetVirtualServerNames()
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

    //TODO: Move this outside of class
    public static IEnumerable<VirtualServer> GetVirtualServers()
    {
      return GetVirtualServerNames().Select(name => new VirtualServer(name));
    }
  }
}
