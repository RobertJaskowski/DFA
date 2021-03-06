﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

class SettingsWindowViewModel : BaseViewModel
{

    #region Properties
    public string _appVersion;
    public string AppVersion
    {
        get => string.IsNullOrEmpty(_appVersion) ? Data.Version.ToString() : _appVersion;
        set
        {
            _appVersion = value;
            OnPropertyChanged(nameof(AppVersion));
        }
    }
    public double BackgroundTransparency
    {
        get => Data.MainWindowSettingsSave.BackgroundTransparency;
        set
        {
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).CurrentWindow.GetAssociatedWindow.Background.Opacity = value;//use  ioc di todo

            Data.MainWindowSettingsSave.BackgroundTransparency = value;
            Data.SaveWindowSettings();
            OnPropertyChanged(nameof(BackgroundTransparency));
        }
    }


    private ObservableCollection<ViewModuleSettings> _settingsControls;
    public ObservableCollection<ViewModuleSettings> ModuleSettingsControls
    {
        get
        {
            if (_settingsControls == null)
            {


                _settingsControls = new ObservableCollection<ViewModuleSettings>();

                foreach (var item in ModuleController.Instance.CoreModuleValues)
                {

                    var v = item.Loader.Instance.GetSettingsUserControlView();
                    if (v != null)
                    {

                        _settingsControls.Add(new ViewModuleSettings
                        {
                            ModuleName = item.Loader.Instance.GetModuleName(),
                            View = item.Loader.Instance.GetSettingsUserControlView()
                        });
                    }
                }

            }

            return _settingsControls;
        }
        set
        {
            _settingsControls = value;

            OnPropertyChanged(nameof(ModuleSettingsControls));
        }
    }

    public class ViewModuleSettings
    {
        public UserControl View { get; set; }
        public string ModuleName { get; set; }
    }

    //private ObservableCollection<UserControl> _settingsControls;
    //public ObservableCollection<UserControl> ModuleSettingsControls
    //{
    //    get
    //    {
    //        if (_settingsControls == null)
    //        {
    //            _settingsControls = new ObservableCollection<UserControl>();
    //            var ret = ModuleController.Instance.GetSettingsViews();
    //            if (ret != null)
    //                _settingsControls = ret;

    //        }

    //        return _settingsControls;
    //    }
    //    set
    //    {
    //        _settingsControls = value;

    //        OnPropertyChanged(nameof(ModuleSettingsControls));
    //    }
    //}




    #endregion



    public SettingsWindowViewModel()
    {
        InjectSettingsFromModules();
    }

    private void InjectSettingsFromModules()
    {

    }
}
