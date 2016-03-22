using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using Microsoft.SharePoint.Client;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        protected System.Windows.Forms.NotifyIcon NotifyIcon;
        private bool _isOpen;
        private ClientContext _clientContext;
        private BindingList<TimeEntry> _timeEntryList;
        private BindingList<TimeEntry> _newTimeEntrieList;

        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();
            Title = Configuration.SharepointList;

            // Datagrid to display data from sharepoint
            _timeEntryList = GetTimeEntryList();
            TimeEntryDataGrid.ItemsSource = _timeEntryList;
            TimeEntryDataGrid.ScrollIntoView(TimeEntryDataGrid.Items[TimeEntryDataGrid.Items.Count - 1]);

            // Datagrid to add data into sharepoint
            _newTimeEntrieList = new BindingList<TimeEntry>();
            NewTimeEntryDataGrid.ItemsSource = _newTimeEntrieList;
            NewTimeEntryDataGrid.ScrollIntoView(NewTimeEntryDataGrid.Items[NewTimeEntryDataGrid.Items.Count - 1]);
        }

        protected BindingList<TimeEntry> GetTimeEntryList()
        {
            // Setpup SharePoint context
            _clientContext = new ClientContext(Configuration.SharepointUrl);
            
            List oList = _clientContext.Web.Lists.GetByTitle(Configuration.SharepointList);
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = "<View><Query><RowLimit>0</RowLimit></Query></View>"
            };
            ListItemCollection collListItem = oList.GetItems(camlQuery);
            _clientContext.Load(collListItem);
            _clientContext.ExecuteQuery();

            BindingList<TimeEntry> timeEntries = new BindingList<TimeEntry>();
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

        protected void AddNewTimeEntry()
        {
            TimeEntry newEntry = new TimeEntry();
            _newTimeEntrieList.Add(newEntry);
        }

        protected void UpdateTimeEntries()
        {

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
            _isOpen = false;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            timer.Tick += delegate
            {
                timer.Stop();
                if (_isOpen) _isOpen = true;
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
            AddNewTimeEntry();
        }

        private void Delete_Entry_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TimeEntryDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
