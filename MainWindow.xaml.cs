using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
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
        protected bool IsOpen;
        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();
            Title = Configuration.SharepointList;
            List<TimeEntry> timeEntrieList = GetTimeEntryList();
            TimeEntryDataGrid.ItemsSource = timeEntrieList;
            TimeEntryDataGrid.ScrollIntoView(TimeEntryDataGrid.Items[TimeEntryDataGrid.Items.Count - 1]);
        }

        protected List<TimeEntry> GetTimeEntryList()
        {
            // Setpup SharePoint context
            ClientContext clientContext = new ClientContext(Configuration.SharepointUrl);
            
            List oList = clientContext.Web.Lists.GetByTitle(Configuration.SharepointList);
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = "<View><Query><RowLimit>0</RowLimit></Query></View>"
            };
            ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();

            List<TimeEntry> timeEntries = new List<TimeEntry>();
            foreach (ListItem oListItem in collListItem)
            {
                Dictionary<string, object> fieldValues = oListItem.FieldValues;
                TimeEntry timeEntry = new TimeEntry();
                object value;
                FieldUserValue employeeObject = null;
                if (fieldValues.TryGetValue("Employee", out value))
                {
                    if (value != null)
                    {
                        employeeObject = (FieldUserValue) value;
                    }
                }
                if (employeeObject != null && employeeObject.LookupValue == Configuration.RealName)
                {
                    if (fieldValues.TryGetValue("Week_x0020_Ending", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.WeekEnding = value.ToString();
                        }
                    }
                    if (fieldValues.TryGetValue("Day", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.Day = value.ToString();
                        }
                    }
                    if (fieldValues.TryGetValue("Type_x0020_of_x0020_Work", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.TypeOfWork = value.ToString();
                        }
                    }
                    if (fieldValues.TryGetValue("Customer", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.Customer = value.ToString();
                        }
                    }
                    if (fieldValues.TryGetValue("Hours", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.Hours = value.ToString();
                        }
                    }
                    if (fieldValues.TryGetValue("Title", out value))
                    {
                        if (value != null)
                        {
                            timeEntry.Description = value.ToString();
                        }
                    }
                    timeEntries.Add(timeEntry);
                }
            }
            return timeEntries;
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

        public void OpenWindowOnTimer()
        {
            IsOpen = false;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            timer.Tick += delegate
            {
                timer.Stop();
                if (IsOpen) IsOpen = true;
                Activate();
                Topmost = true;
                Show();
                WindowState = WindowState.Normal;
            };

            timer.Start();

        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                OpenWindowOnTimer();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            NotifyIcon.Dispose();
            base.OnClosing(e);
        }

        private void Configure_Button_Click(object sender, RoutedEventArgs e)
        {
            ConfigureWindow configureWindow = new ConfigureWindow();
            configureWindow.Show();
            Close();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void Save_Now_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Later_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Entry_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Entry_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
