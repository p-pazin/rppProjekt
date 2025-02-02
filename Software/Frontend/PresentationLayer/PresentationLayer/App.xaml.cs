using Esri.ArcGISRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPTxy8BH1VEsoebNVZXo8HurKzNR8sv5E6DVdtNnOn9p5oukstUu7IvI-kxURMhNd6x5_xocfVNM-4y_opAGDycThcgkkHVHhxobbJYHRCoXlIFwLiIt-f2nmMJXEN23m5C00zrlsEV2wl6OPNpc3v0kxpg6Lw823EmLavyg3BBo7vqvN9cJcnEGprWnAh3ZRPMbIpjFRP5_0sSH6ZYbAYWajzpp2tovxNVb5PtR7aW1BM.AT1_I21a3IYX";
        }
    }
}
