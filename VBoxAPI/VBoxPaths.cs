using Microsoft.Win32;
namespace VBoxAPI
{

  public static class VBoxPaths
  {
    static VBoxPaths()
    {
      Oracle_VirtualBox_Directory = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Oracle\\VirtualBox", "InstallDir", "").ToString();
      Oracle_VirutalBox_Exe = Oracle_VirtualBox_Directory + "VirtualBox.exe";
      Oracle_VirtualBox_VBoxManage_Exe = Oracle_VirtualBox_Directory + "VBoxManage.exe";
      Oracle_VirtualBox_VBoxHeadless_Exe = Oracle_VirtualBox_Directory + "VBoxHeadless.exe";
    }

    public static readonly string Oracle_VirtualBox_Directory;
    public static readonly string Oracle_VirutalBox_Exe;
    public static readonly string Oracle_VirtualBox_VBoxManage_Exe;
    public static readonly string Oracle_VirtualBox_VBoxHeadless_Exe;
  }
}
