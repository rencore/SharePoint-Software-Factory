namespace $AppName$.Silverlight.$SilverlightApplicationName$
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Microsoft.SharePoint.Client;

    public partial class MainPage : UserControl
    {
        ClientContext ctx;
        ListItemCollection contactsListItems;

        public MainPage()
        {
            InitializeComponent();

            //Get SharePoint List after page loads
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Load client OM assemblies
            LoadClientOM loadClientOM = new LoadClientOM(delegate { LoadData(); });
            loadClientOM.Run();
        }

        void LoadData(){
            //Get Client Context
            ctx = ClientContext.Current;

            if (ctx != null) //null if we are not in SharePoint
            {
                //Get data from SharePoint client OM
                //List dataList = ctx.Web.Lists.GetByTitle("[listname]");                
            }
        }
    }
}
