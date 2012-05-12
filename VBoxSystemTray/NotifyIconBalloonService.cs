using System.Windows.Forms;
using System.Collections.Generic;
namespace VBoxSysTray
{
 
  public class NotifyIconBalloonService
  {
    public class NotifyIconState
    {
      public ToolTipIcon BalloonTipIcon { get; private set; }

      public string BalloonTipTitle { get; private set; }

      public string BalloonTipText { get; private set; }

      public NotifyIconState(ToolTipIcon balloonTipIcon, string balloonTipTitle, string balloonTipText)
      {
        BalloonTipIcon = balloonTipIcon;
        BalloonTipTitle = balloonTipTitle;
        BalloonTipText = balloonTipText;
      }

      public static NotifyIconState getState(NotifyIcon notifyIcon)
      {
        return new NotifyIconState(notifyIcon.BalloonTipIcon, notifyIcon.BalloonTipTitle, notifyIcon.BalloonTipText);
      }
    }

    private NotifyIcon notifyIcon;
    private NotifyIconState lastMessage;

    public int BalloonTipTimeout
    { get; set; }

    public string BalloonTipTitle
    { get; set; }


    public NotifyIconBalloonService(NotifyIcon notifyIcon)
    {
      this.notifyIcon = notifyIcon;

      SetDefaultValues();
      RegisterForEvents();
    }

    private void SetDefaultValues()
    {
      BalloonTipTimeout = 20000;
      BalloonTipTitle = "VirtualBox Manager";
    }

    private void RegisterForEvents()
    {
      notifyIcon.DoubleClick += DisplayLastMessage;

    }

    private void SaveState()
    {
      lastMessage = NotifyIconState.getState(notifyIcon); 
    }

    private void DisplayLastMessage(object sender, System.EventArgs e)
    {
      if (lastMessage != null)
      {
        notifyIcon.BalloonTipIcon = lastMessage.BalloonTipIcon;
        notifyIcon.BalloonTipTitle = lastMessage.BalloonTipTitle;
        notifyIcon.BalloonTipText = lastMessage.BalloonTipText;
        notifyIcon.ShowBalloonTip(BalloonTipTimeout);
      }
    }

    public void DisplayError(string errorMessage)
    {
      notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
      notifyIcon.BalloonTipTitle = BalloonTipTitle;
      notifyIcon.BalloonTipText = errorMessage;
      notifyIcon.ShowBalloonTip(BalloonTipTimeout);

      SaveState();
    }

    public void DisplayWarning(string warningMessage)
    {
      notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
      notifyIcon.BalloonTipTitle = BalloonTipTitle;
      notifyIcon.BalloonTipText = warningMessage;
      notifyIcon.ShowBalloonTip(BalloonTipTimeout);

      SaveState();
    }

    public void DisplayInfo(string infoMessage)
    {
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = BalloonTipTitle;
      notifyIcon.BalloonTipText = infoMessage;
      notifyIcon.ShowBalloonTip(BalloonTipTimeout);

      SaveState();
    }

    public void DisplayGeneric(string genericMessage)
    {
      notifyIcon.BalloonTipIcon = ToolTipIcon.None;
      notifyIcon.BalloonTipTitle = BalloonTipTitle;
      notifyIcon.BalloonTipText = genericMessage;
      notifyIcon.ShowBalloonTip(BalloonTipTimeout);

      SaveState();
    }

  }
}
