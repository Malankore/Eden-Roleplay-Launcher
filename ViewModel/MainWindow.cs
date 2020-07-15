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
                //If _updateAvailable is True, change the visibility of Update Available Button
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
                //if version number is not the latest, display Update Available button next to version number
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
            //Grab the version number tag from github json and set the variable
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //Uncomment below to manually set version number
            //Version = "0.3b";
            LoadChangeLog();
            GetNextVersion();

        }

        private void Update()
        {
            Process.Start(new ProcessStartInfo("https://github.com/Malankore/Eden-Roleplay-Launcher/releases/latest") { UseShellExecute = true });
        }

        private void LoadChangeLog()
        {
            //Changelog json base website
            var client = new RestClient("http://tjeynon.us/");
            //Changelog json file and directory
            var request = new RestRequest("edenchanges.json", DataFormat.Json);
            //Request function for changlog json
            var response = client.Get(request);
            //Deserialize retrieved json
            var data = JsonConvert.DeserializeObject<List<Model.ChangeLog>>(response.Content);
            //Load deserialized json data to a collection
            ChangeLog = new ObservableCollection<Model.ChangeLog>(data.OrderByDescending(o => o.Date));
        }

        private void GetNextVersion()
        {
            //Version number json base website
            var client = new RestClient("https://github.com/Malankore/");
            //Version number json directory
            var request = new RestRequest("Eden-Roleplay-Launcher/releases/latest");
            //Request function for version json
            var response = client.Get(request);
            //Deserialize Version Json
            var gr = JsonConvert.DeserializeObject<Model.GitRelease>(response.Content);
            //If the version listed on github's latest does not match, display update available.
            if (gr.tag_name != Version)
            {
                _updateAvailable = true;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateVisible"));
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateText"));
            }
        }

        private void Quit()
        {
            //Close the application
            System.Windows.Application.Current.Shutdown();
        }

        private void LaunchEden()
        {
            //launch fivem and connect to the server
            Process.Start(new ProcessStartInfo("fivem://connect/154.16.67.95:30120") { UseShellExecute = true });
        }

        private void OpenDiscord()
        {
            //launch users default browser and launch the discord invite
            Process.Start(new ProcessStartInfo("https://discord.gg/fzexZJ4") { UseShellExecute = true });
        }

        private void ViewSite()
        {
            //launch users default browser and open the website
            Process.Start(new ProcessStartInfo("https://discord.gg/fzexZJ4") { UseShellExecute = true });
        }
    }
}
