using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Properties;
using System;
using System.Windows;
using Csc.Wpf; using Csc.Components.Common;

namespace Csc.IntelliSchool.Views.Account {
  public partial class LoginWindow : Csc.Wpf.WindowBase {
    public LoginWindow() {
      InitializeComponent();

      this.UsernameTextBox.Text = (Settings.Default.LastUsername ?? "").Trim();


      //#if DEBUG
      //      this.UsernameTextBox.Text = "elsheimy";
      //      this.PasswordTextBox.Password = "im1elsheimy";
      //      OnTryLogin();
      //#endif

      if (System.Diagnostics.Debugger.IsAttached) {
        this.UsernameTextBox.Text = "mohammad.elsheimy";
        this.PasswordTextBox.Password = "123456";

        OnTryLogin();
      }

    }

    private void LoginButton_Click(object sender, RoutedEventArgs e) {
      OnTryLogin();
    }

    private void OnTryLogin() {
      if (this.UsernameTextBox.Text.Trim().Length == 0 || this.PasswordTextBox.Password.Length == 0) {
        OnLoginFailed();
        return;
      }

      this.SetBusy();
      AccountDataManager.Login (this.UsernameTextBox.Text.Trim(), this.PasswordTextBox.Password, OnLoginCompleted);
    }

    private void OnLoginCompleted(bool loggedOn, Exception ex) {
      // TODO: handle exception
      this.ClearBusy();

      if (ex == null) {
        if (loggedOn == false)
          OnLoginFailed();
        else {
          Settings.Default.LastUsername = this.UsernameTextBox.Text.Trim();
          Settings.Default.Save();
          this.DialogResult = true;
          this.Close();
        }
      } else {
        if (ex is System.DirectoryServices.AccountManagement.PrincipalServerDownException)
          OnLoginFailed();
        else
          this.AlertError(ex);
      }
    }

    private void OnLoginFailed() {
      this.AlertError(Csc.IntelliSchool.Assets.Resources.Account.Error_LoginFailed);
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      if (this.UsernameTextBox.Text.Trim().Length == 0)
        this.UsernameTextBox.Focus();
      else
        this.PasswordTextBox.Focus();
    }

    private void PasswordTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter) {
        this.LoginButton.Focus();
        OnTryLogin();
      }
    }

    private void UsernameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter) {
        if (this.PasswordTextBox.Password.Length == 0)
          this.PasswordTextBox.Focus();
        else if (this.UsernameTextBox.Text.Length > 0 && this.PasswordTextBox.Password.Length > 0) {
          this.LoginButton.Focus();
          OnTryLogin();
        }
      }
    }
  }
}
