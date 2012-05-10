using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VBoxAPI
{
  public class VirtualServerMonitor
  {
    private IEnumerable<VirtualServer> virtualServers;
    private Thread thread;
    private bool isMonitoring;
    private Dictionary<VirtualServer, VirtualServer.State> status;

    public event Action<VirtualServer, VirtualServer.State> StateChanged; 

    public int Rate
    { get; set; }

    public VirtualServerMonitor(IEnumerable<VirtualServer> virtualServers)
    {
      this.virtualServers = virtualServers;
      this.Rate = 1000;
      this.isMonitoring = false;
      this.status = new Dictionary<VirtualServer, VirtualServer.State>();

      InitilizeStatus();
      InitializeThread();
    }

    public void Start()
    {
      isMonitoring = true;
      thread.Start();
    }

    public void Stop()
    {
      isMonitoring = false;
    }

    private void InitializeThread()
    {
      thread = new Thread(ThreadLogic);
      thread.IsBackground = true;
    }

    private void InitilizeStatus()
    {
      foreach (VirtualServer server in virtualServers)
      {
        status.Add(server, server.Status);
      }
    }

    private void ThreadLogic()
    {
      while (isMonitoring == true)
      {
        foreach (VirtualServer server in virtualServers)
        {
          VBoxAPI.VirtualServer.State state = server.Status;

          //Check to see if the state has changed
          if (state != status[server])
          {
            //Update the current state
            status[server] = state;
            //Fire the StateChanged event
            StateChanged(server, state);
          }
        }

        Thread.Sleep(Rate);
      }
    }


  }
}
