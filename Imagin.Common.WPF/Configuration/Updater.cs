using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Web;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Updates <see cref="Application"/> using a local <see cref="AppManifest"/> and a remote <see cref="AppManifest"/>.
    /// </summary>
    public sealed class Updater : Base
    {
        #region Properties

        #region Events

        public event EventHandler<EventArgs<Result>> Checked;

        public event EventHandler<EventArgs<Error>> CheckFailed;
        
        public event EventHandler<EventArgs> Downloaded;

        public event EventHandler<EventArgs<Error>> DownloadFailed;

        public event EventHandler<EventArgs<Error>> InstallFailed;

        public event EventHandler<EventArgs> Installed;
        
        public event EventHandler<EventArgs> VersionCurrent;

        public event EventHandler<EventArgs> VersionNewer;

        public event EventHandler<EventArgs> VersionOlder;

        #endregion

        #region Private

        readonly string localManifestPath = string.Empty;

        readonly string localWorkPath;

        readonly Uri manifestPrototype = default;

        readonly string remoteManifestPath = string.Empty;

        readonly string remoteSourcePath;

        readonly Timer timer = new();

        #endregion

        #region Public

        public const double DefaultInterval = 60 * 60 * 1000;

        double interval = DefaultInterval;
        /// <summary>
        /// The interval (in milliseconds) between checking for updates (default = 60 * 60 seconds = 60 minutes = 1 hour).
        /// </summary>
        public double Interval
        {
            get => timer.Interval;
            set
            {
                this.Change(ref interval, value);

                if (isMonitoring)
                    StopMonitoring();

                timer.Interval = interval;

                if (isMonitoring)
                    StartMonitoring();
            }
        }

        bool isChecking = false;
        /// <summary>
        /// Whether or not we're currently checking for updates.
        /// </summary>
        public bool IsChecking
        {
            get => isChecking;
            private set => isChecking = value;
        }

        bool isMonitoring = false;
        /// <summary>
        /// Whether or not we can monitor (or check every so often) for updates.
        /// </summary>
        public bool IsMonitoring
        {
            get => isMonitoring;
            private set => this.Change(ref isMonitoring, value);
        }

        bool isMonitoringEnabled = false;
        /// <summary>
        /// Whether or not we can monitor (or check every so often) for updates.
        /// </summary>
        public bool IsMonitoringEnabled
        {
            get => isMonitoringEnabled;
            set
            {
                if (isMonitoring && !value)
                {
                    StopMonitoring();
                }
                else if (!isMonitoring && value)
                    StartMonitoring();

                this.Change(ref isMonitoringEnabled, value);
            }
        }

        #endregion

        #endregion

        #region Updater

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ManifestPrototype"></param>
        /// <param name="LocalPath">The path to the local manifest.</param>
        /// <param name="RemotePath">The path to the remote manifest.</param>
        /// <param name="RemoteSourcePath">The remote path to download files from.</param>
        public Updater(Uri manifestPrototype, string localManifestPath, string remoteManifestPath, string remoteSourcePath)
        {
            return;
            this.manifestPrototype = manifestPrototype;

            this.localManifestPath = localManifestPath;
            localWorkPath = $@"{Path.GetDirectoryName(this.localManifestPath)}\Update";

            this.remoteManifestPath = remoteManifestPath;
            this.remoteSourcePath = remoteSourcePath;

            timer.Elapsed += OnMonitoring;
        }

        #endregion

        #region Methods

        #region Private

        DialogWindow currentDialog = null;

        Result currentResult = null;

        Result Download()
        {
            var view = new DownloadControl();
            view.Downloaded += OnDownloaded;

            view.Source
                = remoteSourcePath;
            view.Destination 
                = localWorkPath;
            view.Start();

            currentDialog = null;
            var result = Dialog.Show(out currentDialog, "Update", view, DialogImage.Globe, Buttons.Cancel);
            switch (result)
            {
                case -1:
                    //The dialog was closed
                    OnDownloadFailed(new Error(new TaskCanceledException()));
                    break;
                case 0:
                    //The "Cancel" button was pressed
                    OnDownloadFailed(new Error(new TaskCanceledException()));
                    break;
                case 1:
                    //The download completed
                    if (currentResult is Error error)
                        OnDownloadFailed(error);

                    else OnDownloaded();
                    break;
            }

            return currentResult;
        }

        private void OnDownloaded(object sender, EventArgs<Result> e)
        {
            currentResult = e.Value;
            XWindow.SetResult(currentDialog, 1);
            currentDialog.Close();
        }

        async Task InstallAsync(AppManifestPair Manifests)
        {
            var Path = string.Concat(localWorkPath, @"\", System.IO.Path.GetFileName(remoteSourcePath));

            if (!File.Exists(Path))
            {
                OnInstallFailed(new Error(new FileNotFoundException("Installer file cannot be found.")));
            }
            else
            {
                await RestoreLocalManifestAsync(Manifests.Local.Path, manifestPrototype, Manifests.Remote.Version);

                try
                {
                    Storage.File.Long.Open(Path);
                }
                catch (Exception e)
                {
                    OnInstallFailed(new Error(new FileNotFoundException($"Unable to run installer file: {e.Message}")));
                    return;
                }

                OnInstalled();
                Environment.Exit(0);
            }
        }

        //...

        async Task RestoreLocalManifestAsync(AppManifest LocalManifest, string Message)
        {
            var result = Dialog.Show("Update", "{0} Would you like to restore the local manifest?".F(Message), DialogImage.Warning, Buttons.YesNo);

            if (result == 0)
            {
                if (await RestoreLocalManifestAsync(LocalManifest.Path, manifestPrototype))
                {
                    await CheckAsync();
                }
                else OnCheckFailed(new Error(new FileLoadException("Manifest creation failed.")));
            }
        }

        async Task<Result> RestoreLocalManifestAsync(string Path, Uri ResourceUri, Version NewVersion = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Storage.File.Long.Delete(Path);
                }
                catch { }
                SaveManifest(Path, ResourceUri, NewVersion);

                return new Success();
            });
        }

        void SaveManifest(string Path, Uri ResourceUri)
        {
            SaveManifest(Path, ResourceUri, default);
        }

        void SaveManifest(string Path, Uri ResourceUri, Version NewVersion)
        {
            using (var Resource = Application.GetResourceStream(ResourceUri).Stream)
            {
                if (Resource == null)
                    throw new InvalidDataException("Resource is null");

                if (NewVersion != null)
                {
                    using (var Reader = new StreamReader(Resource, Encoding.UTF8))
                    {
                        var Xml = XDocument.Parse(Reader.ReadToEnd());
                        if (Xml.Root.Name.LocalName != "App")
                            return;
                        Xml.Root.Element("Version").Value = NewVersion.ToString();
                        System.IO.File.WriteAllText(Path, Xml.ToString(), Encoding.ASCII);
                    }
                }
                else
                {
                    using (Stream Output = System.IO.File.OpenWrite(Path))
                        Resource.CopyTo(Output);
                }
            }
        }

        //...

        void OnChecked(Result result)
        {
            Checked?.Invoke(this, new EventArgs<Result>(result));

            if (result)
            {
                var success = result as Success<AppManifestPair>;
                Version a = success.Data.Local.Version, b = success.Data.Remote.Version;

                if (a == b)
                {
                    OnVersionCurrent(success.Data);
                }
                else if (a < b)
                {
                    OnVersionOlder(success.Data);
                }
                else if (a > b)
                    OnVersionNewer(success.Data);
            }
            else
            {
                var error = result as Error;
                if (error.FullName == typeof(ManifestNotFoundException).FullName)
                {
                    //var f = error.Exception as ManifestFormatException;

                    //if (f.IsRemote)
                    //{
                    //    OnCheckFailed(new Error(error.Exception));
                    //}
                    //else await RestoreLocalManifestAsync(result.Data.Local, f.Message);
                }
                else if (error.FullName == typeof(ManifestFormatException).FullName)
                {
                    //var f = error.Exception as ManifestFormatException;
                    //if (f.IsRemote)
                    //{
                        //OnCheckFailed(error);
                    //}
                    //else await RestoreLocalManifestAsync(result.Data.Local, f.Message);
                }
                //else OnCheckFailed(error);
            }
        }

        void OnCheckFailed(Error Error)
        {
            Dialog.Show("Update", Error.Text, DialogImage.Error, Buttons.Done);
            CheckFailed?.Invoke(this, new EventArgs<Error>(Error));
        }
        
        void OnDownloaded()
        {
            Downloaded?.Invoke(this, new EventArgs());
        }

        void OnDownloadFailed(Error Error)
        {
            Dialog.Show("Update", Error.Text, DialogImage.Error, Buttons.Done);
            DownloadFailed?.Invoke(this, new EventArgs<Error>(Error));
        }

        void OnInstallFailed(Error Error)
        {
            InstallFailed?.Invoke(this, new EventArgs<Error>(Error));
        }

        void OnInstalled()
        {
            Installed?.Invoke(this, new EventArgs());
        }

        async void OnMonitoring(object sender, ElapsedEventArgs e)
        {
            var result = await CheckAsync(null);
            OnChecked(result);
        }

        //...

        /// <summary>
        /// Occurs when the local version is newer than the remote version.
        /// </summary>
        void OnVersionNewer(AppManifestPair manifests)
        {
            Dialog.Show("Update", "Your version ({0}) is invalid.".F(manifests.Local.Version), DialogImage.Error, Buttons.Done);
            VersionNewer?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the local version is older than the remote version.
        /// </summary>
        async void OnVersionOlder(AppManifestPair manifests)
        {
            VersionOlder?.Invoke(this, new EventArgs());

            var result = Dialog.Show("Update", "A new version is available ({0}).".F(manifests.Remote.Version), default); //, new Button("Download & Install", 0), new ButtonTemplate("Download", 1), new ButtonTemplate("Cancel", 2)

            var download = default(Result);

            if (result == 0 || result == 1)
                download = Download();

            if (result == 1 && download is Success)
                await InstallAsync(manifests);
        }

        /// <summary>
        /// Occurs when the local version is the same as the remote version.
        /// </summary>
        void OnVersionCurrent(AppManifestPair Manifests)
        {
            Dialog.Show("Update", "You have the latest version ({0}).".F(Manifests.Local.Version), DialogImage.Success, Buttons.Done);
            VersionCurrent?.Invoke(this, new EventArgs());
        }

        //...

        void StartMonitoring()
        {
            IsMonitoring = true;
            timer.Start();
        }

        void StopMonitoring()
        {
            timer.Stop();
            IsMonitoring = false;
        }

        #endregion

        #region Public

        public async Task<Result> CheckAsync()
        {
            Result result = default;

            var text = new StackPanel();
            text.Children.Add(new TextBlock() { Text = "Checking for updates..." });
            text.Children.Add(new ProgressBar() { IsIndeterminate = true });

            await Dialog.ShowAsync("Update", text, null, async () =>
            {
                result = await CheckAsync(null);
                return 0;
            }, 
            null, Buttons.Cancel);
            OnChecked(result);
            return result;
        }

        public async Task<Result> CheckAsync(Action OnChecked)
        {
            Result result = default;
            IsChecking = true;

            var localManifest = new AppManifest(localManifestPath);
            var remoteManifest = new AppManifest(remoteManifestPath);

            await Task.Run(() =>
            {
                try
                {
                    if (!new FileInfo(localManifest.Path).Exists)
                        throw new ManifestNotFoundException(false, "Local manifest doesn't exist.");

                    if (!localManifest.Load(File.ReadAllText(localManifest.Path)))
                        throw new ManifestFormatException(false, "Local manifest is invalid.");

                    var remoteUri = new Uri(remoteManifest.Path);

                    var http = new Fetch
                    {
                        Retries = 5,
                        Timeout = 30000
                    };
                    http.Load(remoteUri.AbsoluteUri);

                    if (!http.Success)
                        throw new ManifestNotFoundException(true, "Remote app manifest is unavailable.");

                    if (!remoteManifest.Load(Encoding.UTF8.GetString(http.ResponseData)))
                        throw new ManifestFormatException(true, "Remote app manifest is unavailable.");

                    if (localManifest.SecurityToken != remoteManifest.SecurityToken)
                        throw new ManifestTokenException("Security tokens do not match.");
                }
                catch (Exception e)
                {
                    result = e;
                }

                result = new Success<AppManifestPair>(new AppManifestPair(localManifest, remoteManifest));
            });

            OnChecked?.Invoke();

            IsChecking = false;
            return result;
        }

        #endregion

        #endregion
    }
}