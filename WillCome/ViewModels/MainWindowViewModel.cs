using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using WillCome.Models.MainView;
using WillCome.Repositories;
using WillCome.Utils;

namespace WillCome.ViewModels
{
    // ViewModel для главного окна приложения
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Fields

        private readonly IMainViewStorage _mainViewStorage;
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly DealViewModel _dealViewModel;
        private readonly TaskCasesViewModel _taskCasesViewModel;
        private readonly CustomerBaseViewModel _customerBaseViewModel;

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        #endregion

        #region Constructor

        public MainWindowViewModel(IMainViewStorage mainViewStorage, DashboardViewModel dashboardViewModel,
                                   DealViewModel dealViewModel, TaskCasesViewModel taskCasesViewModel,
                                   CustomerBaseViewModel customerBaseViewModel)
        {
            _mainViewStorage = mainViewStorage;
            _dashboardViewModel = dashboardViewModel;
            _dealViewModel = dealViewModel;
            _taskCasesViewModel = taskCasesViewModel;
            _customerBaseViewModel = customerBaseViewModel;
        }

        #endregion

        #region Properties

        public ReadOnlyObservableCollection<MenuItemModel> ItemsMenuData => _mainViewStorage.GetMenuItemData();

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        private WorkspaceViewModel _selectedWorkspace;
        public WorkspaceViewModel SelectedWorkspace
        {
            get => _selectedWorkspace;
            set
            {
                _selectedWorkspace = value;
                _selectedWorkspace?.UpdateModule();
            }
        }

        #endregion

        #region Commands

        public RelayCommand MenuCommandDashboard => new RelayCommand(param => OpenWorkspace(_dashboardViewModel));
        public RelayCommand MenuCommandDeal => new RelayCommand(param => OpenWorkspace(_dealViewModel));
        public RelayCommand MenuCommandTaskCases => new RelayCommand(param => OpenWorkspace(_taskCasesViewModel));
        public RelayCommand MenuCommandBase => new RelayCommand(param => OpenWorkspace(_customerBaseViewModel));
        public RelayCommand CloseAppCommand => new RelayCommand(param => Application.Current.Shutdown());

        #endregion

        #region Private Methods

        private void OpenWorkspace(WorkspaceViewModel viewModel)
        {
            if (!Workspaces.Contains(viewModel))
                Workspaces.Add(viewModel);

            SetActiveWorkspace(viewModel);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            collectionView?.MoveCurrentTo(workspace);
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += OnWorkspaceRequestClose;

            if (e.OldItems != null)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= OnWorkspaceRequestClose;
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            if (sender is WorkspaceViewModel workspace)
            {
                workspace.Dispose();
                Workspaces.Remove(workspace);
            }
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Base Class Overrides

        public override string DisplayName => "CRM";

        #endregion

        #region Events 

        public void MainView_Loaded()
        {
            _mainViewStorage.CommandMenuItemDashboard = MenuCommandDashboard;
            _mainViewStorage.CommandMenuItemDeal = MenuCommandDeal;
            _mainViewStorage.CommandMenuItemTaskCases = MenuCommandTaskCases;
            _mainViewStorage.CommandMenuItemCustomerBase = MenuCommandBase;

            _mainViewStorage.DataMenuItem();
        }

        #endregion
    }
}