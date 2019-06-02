using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SongsParser.Interfaces;
using SongsParser.Services;
using Splat;

namespace SongsParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterConstant(new BillboardSongsParser(), typeof(ISongsParser));
        }
    }
}
