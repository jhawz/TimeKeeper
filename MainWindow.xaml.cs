using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        protected System.Windows.Forms.NotifyIcon NotifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();

            // Setpup SharePoint context
            ClientContext context = new ClientContext(Configuration.SharepointUrl);
            Web sharePointSite = context.Web;
            context.Load(sharePointSite, osite => osite.Title);
            context.ExecuteQuery();

            Title = sharePointSite.Title;

            ListCollection lists = sharePointSite.Lists;
            IEnumerable<List> listsCollection =
                context.LoadQuery(lists.Include(l => l.Title, l => l.Id));
            context.ExecuteQuery();

            TimeEntriesListBox.ItemsSource = listsCollection;
            TimeEntriesListBox.DisplayMemberPath = "Title";
        }

        private void SetNotifyIcon()
        {
            NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("Resources/Clock.ico"),
                Text =  Properties.Resources.Time_Keeper_String,
                Visible = true
            };
            NotifyIcon.DoubleClick +=
                delegate
                {
                    Show();
                    WindowState = WindowState.Normal;
                };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            NotifyIcon.Dispose();
            base.OnClosing(e);
        }
    }
}
