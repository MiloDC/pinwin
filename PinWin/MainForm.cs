﻿using System;
using System.Drawing;
using System.Windows.Forms;
using PinWin.BusinessLayer;
using PinWin.Controls;

namespace PinWin
{
  /// <summary>
  ///  Main form of the application. Currently a place for prototyping.
  /// </summary>
  public partial class MainForm : TrayAppForm
  {
#region " Constructor "
    /// <summary>
    ///  Constructor - no special processing.
    /// </summary>
    public MainForm()
    {
      InitializeComponent();
    }
    #endregion

    protected override void RegisterHotKeys()
    {
      this.RegisterHotKey(Keys.Control | Keys.F11, this.AddPinnedWindowFromClick);
      //TODO: temporary, for testing multiple hot keys
      this.RegisterHotKey(Keys.Control | Keys.F12, () => MessageBox.Show(@"Hello"));
    }

    #region " Event handlers - System tray icon "
    private void MainForm_Resize(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
      {
        this.notifyIcon_Main.ShowBalloonTip(500, "Minimized to tray",
          "PinWin is now running from system tray.", ToolTipIcon.Info);
        this.Hide();
      }
    }

    private void contextMenu_CloseApplication_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void contextMenu_OpenApplication_Click(object sender, EventArgs e)
    {
      this.AllowFormShow = true;
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void notifyIcon_Main_DoubleClick(object sender, EventArgs e)
    {
      contextMenu_OpenApplication.PerformClick();
    }
    #endregion

#region " Event handlers - Other"
    private void btn_OpenDesktopOverlay_Click(object sender, EventArgs e)
    {
      this.AddPinnedWindowFromClick();
    }

    private void AddPinnedWindowFromClick()
    {
      var desktopOverlayForm = new DesktopOverlayForm();
      desktopOverlayForm.ShowDialog(this);
      Nullable<Point> mouseClickPoint = desktopOverlayForm.MouseClickPoint;

      if (mouseClickPoint != null)
      {
        IntPtr formHandle = WinApiForm.Select(mouseClickPoint.Value);
        if (formHandle != IntPtr.Zero)
        {
          this.pinnedWindowListControl.AddWindow(formHandle);
        }
      }
    }

    #endregion
  }
}