using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using Csc.Wpf.Views;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalInsurancePage : Csc.Wpf.PageBase {
    #region Fields
    private bool _pauseRowActivated;
    private ObservableCollection<Employee> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    //private bool _needsSorting = true;
    private EmployeeMedicalRequest _pendingRequest;
    private EmployeeMedicalProposal _pendingProposal;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public ObservableCollection<Employee> AllItems { get; set; }
    public ObservableCollection<Employee> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeBranch[] Branches { get { return _branches; } set { _branches = value; OnPropertyChanged(() => Branches); } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public EmployeePosition[] Positions { get { return _positions; } set { _positions = value; OnPropertyChanged(() => Positions); } }
    public int? SelectedBranchID {
      get {
        if (this.BranchComboBox.SelectedValue == null || (int)this.BranchComboBox.SelectedValue == 0)
          return null;
    
        return (int)this.BranchComboBox.SelectedValue;
      }
    }
    public int? SelectedDepartmentID {
      get {
        if (this.DepartmentComboBox.SelectedValue == null || (int)this.DepartmentComboBox.SelectedValue == 0)
          return null;

        return (int)this.DepartmentComboBox.SelectedValue;
      }
    }
    public int? SelectedPositionID {
      get {
        if (this.PositionComboBox.SelectedValue == null || (int)this.PositionComboBox.SelectedValue == 0)
          return null;

        return (int)this.PositionComboBox.SelectedValue;
      }
    }
    public EmployeeMedicalRequest PendingRequest { get { return _pendingRequest; } set { _pendingRequest = value; OnPropertyChanged(() => PendingRequest); } }
    public EmployeeMedicalProposal PendingProposal { get { return _pendingProposal; } set { _pendingProposal = value; OnPropertyChanged(() => PendingProposal); } }
    #endregion

    // Constructors
    public MedicalInsurancePage() {
      InitializeComponent();

    }


    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }

    private void OnLoadFilterData() {
      // one for each call
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
    }

    private void OnGetBranches() {
      this.SetBusy();
      EmployeesDataManager.GetBranches(false, OnGetBranchesCompleted);
    }

    private void OnGetDepartments() {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, null, OnGetDepartmentsCompleted);
    }

    private void OnGetPositions() {
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, null, OnGetPositionsCompleted);
    }

    private void OnGetLists() {
      this.SetBusy();
      EmployeesDataManager.GetLists((res, err) => {
        Action<EmployeeList> nullCallback = s => s.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_Unlisted;
        this.ListsComboBox.FillAsyncItems(res, err, nullCallback, this);
        this.ListsComboBox.SelectAll();
      });
    }

    private void OnGetBranchesCompleted(EmployeeBranch[] result, Exception error) {
      if (error == null) {
        Branches = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    private void OnGetDepartmentsCompleted(EmployeeDepartment[] result, Exception error) {
      if (error == null) {
        Departments = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    private void OnGetPositionsCompleted(EmployeePosition[] result, Exception error) {
      if (error == null) {
        Positions = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Employment", "Position", "Program");

    }

    private void OnLoadData() {
      if (DataInitialized == false)
        return;

      OnResetRequest();
      OnResetProposal();

      this.SetBusy();

      EmployeesDataManager.GetApplicableMedicalEmployees(OnDataLoaded);
    }
    private void OnDataLoaded(Employee[] result, Exception error) {
      if (error == null) {
        AllItems = new ObservableCollection<Employee>(result.OrderBy(s => s.Person.FullName));
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter)
        OnFilterData();
    }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }

    private void FilterMedicalComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }

    private void OnFilterData() {
      if (AllItems == null)
        return;

      var lists = ListsComboBox.SelectedItems.Cast<EmployeeList>();

      if (AllItems.Count() > 0 && lists.Count() > 0) {
        var qry = AllItems.AsQueryable();

        if (lists.Count() != ListsComboBox.Items.Count) {
          int?[] listIds = lists.Select(s => s.ListID == 0 ? new int?() : s.ListID).ToArray();
          qry = qry.Where(s => listIds.Contains(s.ListID));
        }

        if (SelectedBranchID != null)
          qry = qry.Where(s => s.BranchID == SelectedBranchID);
        if (SelectedDepartmentID != null)
          qry = qry.Where(s => s.DepartmentID == SelectedDepartmentID);
        if (SelectedPositionID != null)
          qry = qry.Where(s => s.PositionID == SelectedPositionID);
        if (this.NameTextBox.Text.Length > 0) {
          string filterText = this.NameTextBox.Text.ToLower();

          qry = qry.Where(s =>
            s.Person.FullName.ToLower().Contains(filterText)
            || s.Dependants.Select(x => x.Person.FullName.ToLower()).Any(y => y.Contains(filterText))
            || s.MedicalInfo.Certificates.Any(x => x.Code != null && x.Code.ToLower().Contains(filterText)));
        }

        //if (this.FilterMedicalToggleButton.IsChecked == true) {
        //  qry = qry.Where(s => s.MedicalInfo.Certificates.Count() > 0);

        //  if (this.FilterMedicalComboBox.SelectedIndex == 1)
        //    qry = qry.Where(s => s.MedicalInfo.Certificates.Where(x => x.IsActive).Count() > 0);
        //  else if (this.FilterMedicalComboBox.SelectedIndex == 2)
        //    qry = qry.Where(s => s.MedicalInfo.Certificates.Where(x => x.IsActive == false).Count() > 0);
        //} else if (this.FilterUncoveredToggleButton.IsChecked == true)
        //  qry = qry.Where(s => s.MedicalInfo.Certificates.Count() == 0);
        //else if (this.FilterUpdatesToggleButton.IsChecked == true)
        //  qry = qry.Where(s => s.MedicalInfo.RequiresSalaryUpdate == true);

        Items = new ObservableCollection<Employee>(qry.ToArray());
      } else {
        Items = new ObservableCollection<Employee>();
      }

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Name");
      //}
      //_needsSorting = Items.Count() == 0;
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Edit

    private void EditEmployeeButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEditEmployee();
    }

    private void OnEditEmployee() {
      EmployeeModifyWindow wnd = new EmployeeModifyWindow((Employee)this.ItemsGridView.SelectedItem, EmployeeModificationSection.Personal | EmployeeModificationSection.Contact);
      this.DisplayModal<EmployeeModifyWindow>(wnd, OnEmployeeModifyWindow);
    }

    private void OnEmployeeModifyWindow(EmployeeModifyWindow wnd) {
      if (wnd.DialogResult != true)
        return;

      if (wnd.Result == OperationResult.Update) {
        var itm = AllItems.SingleOrDefault(a => a.EmployeeID == wnd.OriginalItem.EmployeeID);
        if (itm != null) {
          AllItems.Remove(itm);
          Items.Remove(itm);
        }

        wnd.Item.MedicalCertificate = wnd.OriginalItem.MedicalCertificate;
        AllItems.Add(wnd.Item);
        this.OnFilterData();
        this.ItemsGridView.SelectItem(wnd.Item);
      };
    }
    #endregion

    #region Dependant
    private void DeleteDependantButton_Click(object sender, RoutedEventArgs e) {
      var dep = e.SelectParentRow<EmployeeDependant>();

      OnDeleteDependant(dep);
    }

    private void OnDeleteDependant(EmployeeDependant dep) {
      Popup.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDependant(dep, OnDependantDeleted);
      });
    }

    private void OnDependantDeleted(EmployeeDependant result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        result.Employee.Dependants.Remove(result);
        this.ItemsGridView.Rebind();
      } else
        Popup.AlertError(error);
    }

    private void AddDependantButton_Click(object sender, RoutedEventArgs e) {
      var emp = e.SelectParentRow<Employee>();
      OnAddDependant();
    }

    private void OnAddDependant() {
      EmployeeDependantModifyWindow wnd = new EmployeeDependantModifyWindow((Employee)this.ItemsGridView.SelectedItem);
      this.DisplayModal<EmployeeDependantModifyWindow>(wnd, OnEmployeeDependantModifyWindowClosed);
    }
    private void EditDependantButton_Click(object sender, RoutedEventArgs e) {
      var dep = e.SelectParentRow<EmployeeDependant>();
      OnEditDependant(dep);
    }

    private void OnEditDependant(EmployeeDependant dep) {

      EmployeeDependantModifyWindow wnd = new EmployeeDependantModifyWindow(dep);
      this.DisplayModal<EmployeeDependantModifyWindow>(wnd, OnEmployeeDependantModifyWindowClosed);
    }

    private void OnEmployeeDependantModifyWindowClosed(EmployeeDependantModifyWindow obj) {
      if (obj.Result == OperationResult.Add) {
        obj.Employee.Dependants.Add(obj.Item);
        this.ItemsGridView.RowFromItem(obj.Employee).IsExpanded = true;
      } else if (obj.Result == OperationResult.Update) {
        obj.Employee.Dependants.Remove(obj.OriginalItem);
        obj.Employee.Dependants.Add(obj.Item);
      } else if (obj.Result == OperationResult.Delete) {
        obj.Employee.Dependants.Remove(obj.OriginalItem);
      }

      this.ItemsGridView.Rebind();
    }

    #endregion

    #region Medical
    private void EmployeeMedicalButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEmployeeMedical();
    }

    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      if (_pauseRowActivated) {
        _pauseRowActivated = false;
        return;
      }

      if (e.Row.IsSelected == false)
        return;

      OnEmployeeMedical();
    }

    private void OnEmployeeMedical() {
      var emp = (Employee)this.ItemsGridView.SelectedItem;

      MedicalCertificateModifyWindow wnd = new MedicalCertificateModifyWindow(emp);
      this.DisplayModal<MedicalCertificateModifyWindow>(wnd, OnEmployeeMedicalWindow);
    }

    private void OnEmployeeMedicalWindow(MedicalCertificateModifyWindow wnd) {
      if (wnd.DialogResult != true)
        return;

      var itm = AllItems.SingleOrDefault(a => a.EmployeeID == wnd.Employee.EmployeeID);

      if (wnd.Result == OperationResult.Add || wnd.Result == OperationResult.Update) {
        itm.MedicalCertificate = wnd.Item;
        itm.MedicalCertificateID = wnd.Item.CertificateID;
        this.ItemsGridView.Rebind();
      } else if (wnd.Result == OperationResult.Delete) {
        itm.MedicalCertificate = null;
        itm.MedicalCertificateID = null;
        RemoveIfApplicable(itm);
        this.ItemsGridView.Rebind();
      }
    }

    private void RemoveIfApplicable(Employee itm) {
      if (itm.IsMonthEmployee(DateTime.Today.ToMonth()) == false && itm.MedicalInfo.Certificates.Count() == 0) {
        AllItems.Remove(itm);
        Items.Remove(itm);
      }
    }

    private void DependantsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      OnDependantMedical((EmployeeDependant)e.Row.Item);
    }

    private void DependantMedicalButton_Click(object sender, RoutedEventArgs e) {
      var dep = e.SelectParentRow<EmployeeDependant>();
      OnDependantMedical(dep);
    }

    private void OnDependantMedical(EmployeeDependant dep) {
      if (dep.Employee.MedicalCertificate == null && dep.MedicalCertificate == null) {
        Popup.Alert(Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_DependantUncoveredMember);
        return;
      }

      MedicalCertificateModifyWindow wnd = new MedicalCertificateModifyWindow(dep);
      this.DisplayModal<MedicalCertificateModifyWindow>(wnd, OnDependantMedicalWindow);
    }

    private void OnDependantMedicalWindow(MedicalCertificateModifyWindow wnd) {
      if (wnd.DialogResult != true)
        return;

      _pauseRowActivated = true;
      var emp = AllItems.SingleOrDefault(a => a.EmployeeID == wnd.Dependant.EmployeeID);
      var itm = emp.Dependants.SingleOrDefault(s => s.DependantID == wnd.Dependant.DependantID);
      if (itm.Employee == null)
        itm.Employee = emp;

      if (wnd.Result == OperationResult.Add || wnd.Result == OperationResult.Update) {
        itm.MedicalCertificate = wnd.Item;
        itm.MedicalCertificateID = wnd.Item.CertificateID;
        this.ItemsGridView.Rebind();

      } else if (wnd.Result == OperationResult.Delete) {
        itm.MedicalCertificate = null;
        itm.MedicalCertificateID = null;
        RemoveIfApplicable(emp);
        this.ItemsGridView.Rebind();
      }
    }
    #endregion

    #region Apply to Salaries
    private void ApplySalariesButton_Click(object sender, RoutedEventArgs e) {
      MedicalApplySalariesWindow wnd = new MedicalApplySalariesWindow();
      wnd.GridView = this.ItemsGridView;

      this.DisplayModal<MedicalApplySalariesWindow>(wnd, OnApplySalaries);
    }

    private void OnApplySalaries(MedicalApplySalariesWindow obj) {
      if (obj.Result != OperationResult.Update)
        return;

      this.ItemsGridView.Rebind();
    }
    #endregion


    #region Employees ContextMenu
    private void EmployeesContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      if (row != null && row.Item is EmployeeDependant) {
        e.Handled = true;
        return;
      }


      int selectionCount = this.ItemsGridView.SelectedItems.Count;

      if (row != null && selectionCount <= 1) {
        row.FocusSelect();
        selectionCount = 1;
      }


      menu.FindMenuItem("EmployeeMedicalMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null && selectionCount == 1;
      menu.FindMenuItem("AddToMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null && selectionCount > 0;

      menu.FindMenuItem("EditEmployeeMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null && selectionCount == 1;
      menu.FindMenuItem("AddDependantMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null && selectionCount == 1;
      menu.FindMenuItem("EmployeeDocumentsMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null && selectionCount == 1;
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }

    private void EmployeeDocumentsMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEmployeeDocuments(); }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData();
    }
    private void EmployeeMedicalMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEmployeeMedical();
    }

    private void EditEmployeeMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditEmployee();
    }

    private void AddDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnAddDependant();
    }

    #endregion

    #region Dependants ContextMenu
    private void DependantsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      if (row != null && row.Item is Employee) {
        e.Handled = true;
        return;
      }

      row?.FocusSelect();

      menu.FindMenuItem("DependantMedicalMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DependantRequestMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("EditDependantMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteDependantMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void DependantMedicalMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDependantMedical((EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item);
    }

    private void EditDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditDependant((EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item);
    }

    private void DeleteDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteDependant((EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item);
    }

    #endregion

    #region Recalculate
    private void RecalculateMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      AdvancedButton.IsOpen = false;

      MedicalRecalculateWindow wnd = new MedicalRecalculateWindow();
      wnd.GridView = this.ItemsGridView;

      this.DisplayModal<MedicalRecalculateWindow>(wnd, OnRecalculateWindowClosed);
    }

    private void OnRecalculateWindowClosed(MedicalRecalculateWindow obj) {
      if (obj.Result != OperationResult.None)
        OnLoadData();
    }
    #endregion

    #region Documents
    private void DocumentsButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow<Employee>();
      OnEmployeeDocuments();

    }

    private void OnEmployeeDocuments() {
      Documents.DocumentsWindow wnd = new Documents.DocumentsWindow((Employee)this.ItemsGridView.SelectedItem);
      this.DisplayModal(wnd);
    }
    #endregion

    #region Print
    private void PrintConsentMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.PrintButton.IsOpen = false;

      ProgramItemsSelectionWindow wnd = new ProgramItemsSelectionWindow(false);
      wnd.GridView = this.ItemsGridView;
      this.DisplayModal<ProgramItemsSelectionWindow>(wnd, OnPrintConsent);
    }

    private void OnPrintConsent(ProgramItemsSelectionWindow obj) {
      if (obj.DialogResult != true)
        return;

      this.SetBusy();
      EmployeesDataManager.GenerateMedicalCertificates(
        obj.Items.Select(s => s.EmployeeID).ToArray(),
        obj.Programs.Select(s => s.ProgramID).ToArray(),
        OnConsentItemsGenerated);
    }

    private void OnConsentItemsGenerated(EmployeeMedicalCertificateGroup[] result, Exception error) {
      if (error == null) {
        this.DisplayReport(new Reports.MedicalConsentReport(), result);
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }


    private void PrintListMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.PrintButton.IsOpen = false;
      if (Items == null)
        return;


      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.GridView = this.ItemsGridView;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnPrintList);
    }


    private void OnPrintList(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      var items = obj.ItemsAs<Employee>().ToArray();
      items = items.Where(s => s.MedicalCertificate != null).OrderBy(s => s.MedicalCertificate.Program.FullName).Concat(items.Where(s => s.MedicalCertificate == null)).ToArray();

      this.DisplayReport(new Reports.MedicalListReport(), items.GroupBy(s => s.MedicalCertificate != null ? s.MedicalCertificate.ProgramID : 0).ToArray(), null);
    }
    #endregion

    #region Requests
    private void OnResetRequest() {
      PendingRequest = new EmployeeMedicalRequest();
      PendingRequest.Date = DateTime.Today;
    }

    private void PendingRequestButton_Click(object sender, RoutedEventArgs e) {
      MedicalRequestWindow wnd = new MedicalRequestWindow(PendingRequest);
      this.DisplayModal<MedicalRequestWindow>(wnd, OnMedicalRequestWindowClosed);
    }

    private void OnMedicalRequestWindowClosed(MedicalRequestWindow obj) {
      if (obj.DialogResult == true)
        PendingRequest = obj.Item;
    }

    private void EmployeeRequestMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var employees = this.ItemsGridView.SelectedItemsAs<Employee>();

      foreach (var itm in employees) {
        AddPendingRequestEmployee(itm);
      }
    }

    private void AddPendingRequestEmployee(Employee itm) {
      string certCode = itm.MedicalInfo.GetSuggestedMedicalCertificateCode();
      if (CertCodeAvailable(certCode, itm.EmployeeID) == false)
        certCode = string.Empty;

      PendingRequest.AddItem(itm, certCode);

      if (itm.Dependants.Count() > 0) {
        foreach (var dep in itm.Dependants) {
          AddPendingRequestDependant(dep);
        }

        //Popup.Confirm(null, Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_AppendDependantRequest, () => {
        //  foreach (var dep in itm.Dependants) {
        //    OnAppendRequestDependant(dep);
        //  }
        //});
      }
    }

    private void AddPendingRequestDependant(EmployeeDependant itm) {
      string certCode = itm.GetSuggestedMedicalCertificateCode();
      if (CertCodeAvailable(certCode, itm.EmployeeID) == false)
        certCode = string.Empty;

      PendingRequest.AddItem(itm, certCode);
    }


    private void DependantRequestMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      EmployeeDependant itm = (EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item;
      AddPendingRequestDependant(itm);
    }




    private bool CertCodeAvailable(string code, int employeeId) {
      var certCodes = AllItems.Where(s => s.MedicalCertificate != null && s.MedicalCertificate.Code != null && s.EmployeeID != employeeId)
        .Select(s => s.MedicalCertificate.Code.Trim().ToLower()).ToArray();
      return certCodes.Contains(code.ToLower()) == false;
    }


    #endregion

    #region Proposal
    //private void PendingProposalButton_Click(object sender, RoutedEventArgs e) {
    //  MedicalProposalListWindow wnd = new MedicalProposalListWindow();
    //  wnd.AllItems = PendingProposal.Employees.Select(s => s.Employee).ToArray();
    //  this.DisplayModal<MedicalProposalListWindow>(wnd);
    //}

    private void PendingProposalMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      MedicalProposalListWindow wnd = new MedicalProposalListWindow();
      wnd.AllItems = PendingProposal.Employees.Select(s => s.Employee).ToArray();
      this.DisplayModal<MedicalProposalListWindow>(wnd);
    }

    private void OnResetProposal() {
      PendingProposal = new Data.EmployeeMedicalProposal();
    }

    private void EmployeeProposalMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      ItemsListWindow wnd = new ItemsListWindow();
      wnd.DisplayMemberPath = "Name";
      wnd.ItemsCallback = (callback) => {
        EmployeesDataManager.GetMedicalPrograms(false, (o, err) => callback(o != null ? (IEnumerable<object>)o : null, err));
      };
      this.DisplayModal<ItemsListWindow>(wnd, OnProposalProgramSelecetd);
    }

    private void OnProposalProgramSelecetd(ItemsListWindow obj) {
      if (obj.DialogResult != true)
        return;

      var employeeIds = this.ItemsGridView.SelectedItemsAs<Employee>().Select(s => s.EmployeeID).ToArray();
      var programIds = obj.SelectedItemsAs<EmployeeMedicalProgram>().Select(s => s.ProgramID).ToArray();
      this.SetBusy();
      EmployeesDataManager.GenerateMedicalProgramCertificates(programIds, employeeIds, OnAppendProposal);
    }

    private void OnAppendProposal(Employee[] result, Exception error) {
      if (error == null)
        PendingProposal.AddRange(result);
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    #endregion
  
  }
}

//#region Proposal (Old)
//private void ProposalMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
//  this.AdvancedButton.IsOpen = false;

//  ProgramItemsSelectionWindow wnd = new ProgramItemsSelectionWindow();
//  wnd.GridView = this.ItemsGridView;
//  this.DisplayModal<ProgramItemsSelectionWindow>(wnd, OnGenerateProposal);
//}

//private void OnGenerateProposal(ProgramItemsSelectionWindow obj) {
//  if (obj.DialogResult != true)
//    return;



//  this.SetBusy();
//  EmployeesDataManager.GenerateMedicalProgramCertificates(
//    obj.Programs.Select(s => s.ProgramID).ToArray(),
//    obj.Items.Select(s => s.EmployeeID).ToArray(),
//    OnProposalGenerated);
//}

//private void OnProposalGenerated(Employee[] result, Exception error) {
//  this.ClearBusy();
//  if (error == null) {
//    MedicalProposalListWindow wnd = new MedicalProposalListWindow();
//    wnd.AllItems = result;
//    this.DisplayModal<MedicalProposalListWindow>(wnd);
//  } else
//    Popup.AlertError(error);
//}
//#endregion
