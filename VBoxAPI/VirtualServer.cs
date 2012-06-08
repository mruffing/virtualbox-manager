using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VBoxAPI
{
  //TODO: Refactor towards more efficient VBoxService calls
  public class VirtualServer
  {
    public enum State { Unknown, Poweroff, Running }

    public class VRDE
    {
      public bool IsEnabled { get; set; }
      public int Port { get; set; }
    }

    private String serverName;

    public VRDE VRDEProperty
    {
      get { return GetVRDEStatus(); }
    }

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

    public void Start()
    {
      VBoxService.StartVirtualMachine(serverName);
    }

    public void Stop()
    {
      VBoxService.AcpiPowerOffVirtualMachine(serverName);
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

    private VRDE GetVRDEStatus()
    {
      VRDE returnValue = new VRDE() 
      { 
        IsEnabled = false, 
        Port = -1 
      };

      foreach (String line in VBoxService.GetVirtualMachineInfo(serverName).Split('\n'))
      {
        string formattedLine = line.Trim().ToLower();

        if(formattedLine.Contains("vrdeports="))
        {
          returnValue.Port = Int32.Parse(formattedLine.Split('"')[1]);
          returnValue.IsEnabled = true;
          break;
        }
      }

      return returnValue;
    }

    private string ExecuteGetStateCommand()
    {
      string serverState = "Could not find Server State for (" + serverName + ")";

      foreach (String line in VBoxService.GetVirtualMachineInfo(serverName).Split('\n'))
      {
        string formattedLine = line.Trim().ToLower();

        if (formattedLine.Contains("vmstate="))
        {
          serverState = formattedLine.Split('"')[1];
          break;
        }
      }

      return serverState;
    }
  }
}
