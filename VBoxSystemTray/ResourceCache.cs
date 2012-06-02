
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using VBoxSysTray.Properties;

namespace VBoxSysTray
{
  public static class ResourceCache
  {
    public static readonly Bitmap UnknownImage = Resources.unknown.ToBitmap();
    public static readonly Bitmap PoweroffImage = Resources.poweroff.ToBitmap();
    public static readonly Bitmap RunningImage = Resources.running.ToBitmap();
  }
}
