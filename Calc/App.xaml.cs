namespace Calc
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        /// <summary>
        /// Window creation override
        /// </summary>
        /// <param name="activationState"></param>
        /// <returns></returns>
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            // Set size
            window.Width = 600;
            window.Height = 800;

            // Placement happens after activation
            window.Activated += Window_Activated;            

            return window;
        }

        /// <summary>
        /// Window activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Activated(object sender, EventArgs e)
        {
#if WINDOWS
            var window = sender as Window;

            // give it some time to complete window resizing task.
            await window.Dispatcher.DispatchAsync(() => { });

            var disp = DeviceDisplay.Current.MainDisplayInfo;

            // move to screen center
            window.X = (disp.Width / disp.Density - window.Width) / 2;
            window.Y = (disp.Height / disp.Density - window.Height) / 2;
#endif
        }
    }
}