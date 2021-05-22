using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Documents {
  public partial class DocumentModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeDocument _item;
    private string _uploadFile;

    // Properties
    public EmployeeDocument Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeDocument OriginalItem { get; set; }

    // Constructors
    private DocumentModifyWindow() {
      InitializeComponent();
    }

    public DocumentModifyWindow(Employee emp) : this() {
      Item = new EmployeeDocument() { EmployeeID = emp.EmployeeID };
    }
    public DocumentModifyWindow(EmployeeDocument item) : this() {
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone();
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.DocumentID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.DocumentID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.NameTextBox.Focus();
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDocument(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDocument result, Exception error) {
      if (error == null) {
        this.Close(OperationResult.Delete);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      Item.TrimStrings();

      this.SetBusy();
      if (Item.DocumentID == 0)
        EmployeesDataManager.AddOrUpdateDocument(Item, _uploadFile, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateDocument(Item, _uploadFile, OnUpdated);
      }
    }

    private void OnAdded(EmployeeDocument result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeDocument result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    private void BrowseButton_Click(object sender, RoutedEventArgs e) {
      try {
        _uploadFile = Popup.SelectFile(FileType.Any);
        Item.OriginalFilename = System.IO.Path.GetFileName(_uploadFile);
        if (this.NameTextBox.Text.Trim().Length == 0)
          Item.Name = Item.OriginalFilename;
        this.NameTextBox.Rebind();
        this.FilenameTextBox.Rebind();

      } catch (Exception ex) {
        this.AlertError(ex);
      }
    }

    //private void OnUploaded(string result, Exception error) {
    //  if (error == null) {
    //    Item.Url = result;
    //    Item.OriginalFilename = _originalFilename;
    //    if (string.IsNullOrWhiteSpace(Item.Name)) {
    //      Item.Name = Item.OriginalFilename;
    //    }
    //    this.FilenameTextBox.Rebind();
    //  } else
    //    this.AlertError(error);
    //  this.ClearBusy();
    //}


  }

}