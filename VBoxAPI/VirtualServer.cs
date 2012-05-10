using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VBoxAPI
{
  public class VirtualServer
  {
    public enum State { Unknown, Poweroff, Running }

    private String serverName;

    public String Name
    {
      get { return serverName; }
    }

    public State Status
    {
      get { return GetVirtualServerState(); }
    }

    public VirtualServer(string serverName)
    {
      this.serverName = serverName;
    }

    public bool Start()
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = @"C:\Program Files\Oracle\VirtualBox\VBoxHeadless.exe";
      p.StartInfo.Arguments = String.Format("--startvm \"{0}\"", serverName);
    
      p.Start();

      return true;

    }

    public bool Stop()
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = @"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe";
      p.StartInfo.Arguments = String.Format("controlvm \"{0}\" acpipowerbutton", serverName);
      p.Start();
      p.WaitForExit();

      return true;

    }

    private State GetVirtualServerState()
    {
      State returnValue = State.Unknown;

      string value = ExecuteGetStateCommand();

      if (value == "poweroff")
      {
        returnValue = State.Poweroff;
      }
      else if (value == "running")
      {
        returnValue = State.Running;
      }

      return returnValue;

    }

    private string ExecuteGetStateCommand()
    {
      string serverState = "Could not find Server State for (" + serverName + ")";
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = @"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe";
      p.StartInfo.Arguments = String.Format("showvminfo \"{0}\" --machinereadable", serverName);
      p.Start();

      p.WaitForExit();


      foreach (String line in p.StandardOutput.ReadToEnd().Split('\n'))
      {
        string lineNoFormat = line.Trim().ToLower();

        if (lineNoFormat.Contains("vmstate="))
        {
          serverState = lineNoFormat.Split('"')[1];
          break;
        }
      }

      return serverState;

    }

  }
}
