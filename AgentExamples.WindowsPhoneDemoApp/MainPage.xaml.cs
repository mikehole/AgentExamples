using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AgentExamples.WindowsPhoneDemoApp.Resources;

namespace AgentExamples.WindowsPhoneDemoApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }


        private void CmdSimple_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Examples/SimpleComms.xaml", UriKind.Relative));
        }

        private void CmdObject_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Examples/Objects.xaml", UriKind.Relative));
        }
    }
}