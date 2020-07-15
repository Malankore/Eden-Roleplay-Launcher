using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace Eden_Roleplay.ViewModel
{
    public class MainWindow : BindableBase
    {
        private ObservableCollection<Model.ChangeLog> _changeLog;
        private string _version;
        private bool _updateAvailable = false;

        public ObservableCollection<Model.ChangeLog> ChangeLog
        {
            get
            {
                return _changeLog;
            }
            set
            {
                SetProperty(ref _changeLog, value);
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                SetProperty(ref _version, value);
            }
        }

        public System.Windows.Visibility UpdateVisible
        {
            get
            {
                return _updateAvailable ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
            set
            {
            }
        }

        public string UpdateText
        {
            get
            {
                return (_updateAvailable) ? "Update Available" : "";
            }
            set
            {
            }
        }

        public ICommand ViewSiteCommand { get; private set; }
        public ICommand DiscordCommand { get; private set; }
        public ICommand LaunchEdenCommand { get; private set; }
        public ICommand QuitCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        public MainWindow()
        {
            ViewSiteCommand = new DelegateCommand(ViewSite);
            DiscordCommand = new DelegateCommand(OpenDiscord);
            LaunchEdenCommand = new DelegateCommand(LaunchEden);
            QuitCommand = new DelegateCommand(Quit);
            UpdateCommand = new DelegateCommand(Update);
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //Version = "0.3b";
            LoadChangeLog();
            GetNextVersion();

        }

        private void Update()
        {
            Process.Start(new ProcessStartInfo("https://https://github.com/Malankore/Eden-Roleplay-Launcher/releases/latest") { UseShellExecute = true });
        }

        private void LoadChangeLog()
        {
            var client = new RestClient("http://tjeynon.us/");
            var request = new RestRequest("edenchanges.json", DataFormat.Json);
            var response = client.Get(request);
            var data = JsonConvert.DeserializeObject<List<Model.ChangeLog>>(response.Content);
            ChangeLog = new ObservableCollection<Model.ChangeLog>(data.OrderByDescending(o => o.Date));
        }

        private void GetNextVersion()
        {
            var client = new RestClient("https://github.com/Malankore/");
            var request = new RestRequest("Eden-Roleplay-Launcher/releases/latest");
            var response = client.Get(request);
            var gr = JsonConvert.DeserializeObject<Model.GitRelease>(response.Content);
            if (gr.tag_name != Version)
            {
                _updateAvailable = true;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateVisible"));
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateText"));
            }
        }

        private void Quit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LaunchEden()
        {
            Process.Start(new ProcessStartInfo("fivem://connect/154.16.67.95:30120") { UseShellExecute = true });
        }

        private void OpenDiscord()
        {
            Process.Start(new ProcessStartInfo("https://discord.gg/fzexZJ4") { UseShellExecute = true });
        }

        private void ViewSite()
        {
            Process.Start(new ProcessStartInfo("https://discord.gg/fzexZJ4") { UseShellExecute = true });
        }
    }
}
