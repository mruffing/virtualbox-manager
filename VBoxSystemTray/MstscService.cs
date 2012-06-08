using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace VBoxSysTray
{

  public class MstscService
  {
    public class ConnectionParams
    {
      public string Server { get; set; }
      public int Port { get; set; }
      public int ScreenWidth { get; set; }
      public int ScreenHeight { get; set; }
    }

    public static void Connect(ConnectionParams connectionParams)
    {
      Connect(connectionParams.Server, connectionParams.Port, connectionParams.ScreenWidth, connectionParams.ScreenHeight);
    }

    public static void Connect(string server, int port, int screenWidth, int screenHeight)
    {
      Process p = new Process();

      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.FileName = "mstsc";
      p.StartInfo.Arguments = String.Format("/v:{0}:{1} /w:{2} /h:{3}", server, port, screenWidth, screenHeight);
      p.Start();

      p.WaitForExit();
    }
  }
}
