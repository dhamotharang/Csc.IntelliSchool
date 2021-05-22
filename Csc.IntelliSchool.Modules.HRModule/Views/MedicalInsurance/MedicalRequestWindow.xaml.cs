using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {

  public partial class MedicalRequestWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeMedicalRequest _item;
    private ObservableCollection<IEmployeeMedicalRequestItem> _requestItems;

    // Properties
    public EmployeeMedicalRequest Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeMedicalRequest OriginalItem { get; set; }
    public ObservableCollection<IEmployeeMedicalRequestItem> RequestItems { get { return _requestItems; } set { _requestItems = value; OnPropertyChanged(() => RequestItems); } }



    // Constructors
    public MedicalRequestWindow() {
      InitializeComponent();
    }
    public MedicalRequestWindow(EmployeeMedicalRequest itm)
      : this() {
      OriginalItem = itm;
      Item = itm.Clone();
      RequestItems = new ObservableCollection<IEmployeeMedicalRequestItem>(Item.GetRequestItems());
    }


    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      //this.DeleteButton.Visibility = Item.RequestID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.SetBusy();
      this.SetBusy();
      EmployeesDataManager.GetMedicalPrograms(false, (res, err) => {
        this.ProgramComboBox.FillAsyncItems(res, err, null, this);
        //this.ProgramComboBox.SelectedValue = Item.ProgramID;
      });
      EmployeesDataManager.GetMedicalRequestTypes(false, (res, err) =>
        this.TypeComboBox.FillAsyncItems(res != null ? res.OrderBy(x => x.Name).ToArray() : null, err, null, this));
    }
    #endregion Loading

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      this.Close(OperationResult.None);
    }
    #endregion Basic

    //#region Delete
    //private void DeleteButton_Click(object sender, RoutedEventArgs e) {
    //  this.ConfirmDelete(() => {
    //    this.SetBusy();
    //    EmployeesDataManager.Delete(Item, Item.RequestID, OnDeleted);
    //  });
    //}

    //private void OnDeleted(EmployeeMedicalRequest result, Exception error) {
    //  if (error == null) {
    //    this.Close(OperationResult.Delete);
    //  } else
    //    this.AlertError(error);
    //  this.ClearBusy();
    //}
    //#endregion Delete

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      Item.TrimStrings();

      this.SetBusy();
      //EmployeesDataManager.CheckEmployeeMedicalCodeUsed(Item, OnCheckCompleted);
    }
    #endregion Saving

    private void NewButton_Click(object sender, RoutedEventArgs e) {
      OriginalItem = null;
      Item = new EmployeeMedicalRequest() {
        Date = DateTime.Today
      };
      this.Rebind();
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      this.SetBusy();
      EmployeesDataManager.GetProgramTemplate(Item, OnGetTemplate);
    }

    private void OnGetTemplate(EmployeeMedicalProgramTemplate result, Exception error) {
      if (error == null) {
        if (result == null || string.IsNullOrEmpty(result.Path)) {
          Popup.Alert(Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_NoTemplate);
          this.ClearBusy();
        } else {
          var filename = Popup.SaveFile(FileType.Excel);
          if (null == filename) {
            this.ClearBusy();
            return;
          }
          EmployeesDataManager.GenerateMedicalRequest(Item, filename, OnGenerateRequest);

        }
      } else {
        Popup.AlertError(error);
        this.ClearBusy();
      }
    }

    private void OnGenerateRequest(string result, Exception error) {
      if (error == null) {
        this.Confirm(string.Format(Csc.IntelliSchool.Assets.Resources.Text.File_Generated_Confirm, result), () => { ProcessExtensions.Start(result); });
      } else {
        Popup.AlertError(error);
      }
      this.ClearBusy();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      Item.SetRequestItems(RequestItems);
      this.Close(true);
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e) {
      RequestItems = new ObservableCollection<IEmployeeMedicalRequestItem>();
      Item.Employees.Clear();
      Item.Dependants.Clear();
      Item.Notes = null;
      //this.NotesTextBox.Rebind();
    }

  }
}