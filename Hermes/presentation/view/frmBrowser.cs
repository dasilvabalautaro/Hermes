using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Hermes.tools;
using Hermes.presentation.presenter;

namespace Hermes.presentation.view
{
    public partial class frmBrowser : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();
        public ChromiumWebBrowser chromeBrowser;
        private HttpToolsPresenter httpToolsPresenter = new HttpToolsPresenter();
        private delegate void MessageResultDelegate(string resultInput, StatusStrip ssMain);
        private MessageResultDelegate delegateCatchResult;
        IDisposable subscriptionRequest;
        IDisposable subscriptionRequestError;
        private string _address;
        private string _consoleMessage;
        private StatusStrip status;
        
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
                updateUrlVideo(_address);
            }
        }

        public string ConsoleMessage
        {
            get
            {
                return _consoleMessage;
            }

            set
            {
                _consoleMessage = value;
                
            }
        }

        #endregion


        public frmBrowser()
        {
            InitializeComponent();           
            InitializeChromium();
        }

        public void InitializeChromium()
        {
            
            CefSettings settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment
                .SpecialFolder.LocalApplicationData) + @"\CEF";
            //settings.MultiThreadedMessageLoop = false;
            //settings.UserAgent = "CefSharp Browser";
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("no-proxy-server", "1");
            //settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream",
            //    "1");
            //settings.CefCommandLineArgs.Add("enable-speech-input", "1");
            //settings.CefCommandLineArgs.Add("enable-usermedia-screen-capture", "1");
            //settings.CefCommandLineArgs.Add("enable_webrtc", "1");
            //settings.PersistSessionCookies = true;
            // Initialize cef with the provided settings         
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }
            
            // Create a browser component
            //chromeBrowser = new ChromiumWebBrowser("https://dasilvabalautaro.github.io/WebRTC/");
            // Add it to the form and fill it to the form window.
            chromeBrowser = new ChromiumWebBrowser(" https://appr.tc/")
            {
                Dock = DockStyle.Fill,
            };
           
            this.Controls.Add(chromeBrowser);           
            chromeBrowser.AddressChanged += OnBrowserAddressChanged;
            chromeBrowser.ConsoleMessage += OnBrowserConsoleMessage;
            //BrowserSettings browserSettings = new BrowserSettings();
            //browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            //browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;           
            //chromeBrowser.BrowserSettings = browserSettings;
        }
      
        private void frmBrowser_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen
                .WorkingArea.Width - this.Width) / 2);
            initControls();
            subscriptionReactive();
        }

        
        private void initControls()
        {
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            this.delegateCatchResult = new MessageResultDelegate(addMessageResult);
        }

        private void addMessageResult(string resultInput, StatusStrip ssMain)
        {
            if (!string.IsNullOrEmpty(resultInput))
            {
                string operation = httpToolsPresenter
                    .getValueOfKeyResponseJson(resultInput, "operation");
                if (!string.IsNullOrEmpty(operation))
                {
                    controlManager.setValueTextStatusStrip(operation, 0, ssMain);
                }
            }
        }

        private void subscriptionReactive()
        {

            subscriptionRequest = httpToolsPresenter
                .subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRequestError = httpToolsPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
        }

        private void launchMessageStatus(string result)
        {
            this.Invoke(this.delegateCatchResult,
               new Object[] { result, status });
        }

        private void launchMessageError(string result)
        {
            MessageBox.Show(result);
        }

        private void OnBrowserAddressChanged(object sender, 
            AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Address = args.Address);
        }

        private void updateUrlVideo(string address)
        {
            httpToolsPresenter.setBodyForVideo(address);
            httpToolsPresenter.sendRequest();
        }

        private void OnBrowserConsoleMessage(object sender,
            ConsoleMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => ConsoleMessage = args.Message);
        }

        private void frmBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            chromeBrowser.Dispose();
            subscriptionRequest.Dispose();
            subscriptionRequestError.Dispose();
            controlManager.enabledOptionMenu(strNameMenu, mdiMain.NAME);
            controlManager.setValueTextStatusStrip("", 0, status);
        }


    }
}
