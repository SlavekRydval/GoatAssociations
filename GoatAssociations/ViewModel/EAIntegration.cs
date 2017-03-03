using System;
using System.Windows.Input;

namespace GoatAssociations.ViewModel
{
    public class EAIntegration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public String EA_Connect(EA.Repository Repository)
        {
            return "";
        }


        /// <summary>
        /// Lets tidy up the mess we have created
        /// It is called just once when EA is about to end
        /// </summary>
        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        const string menuHeader = "-&Goat Associations";
        const string menuItemAbout = "&About...";

        /// <summary>
        /// It is called when EA needs to create menu items for our addin
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Location"></param>
        /// <param name="MenuName"></param>
        /// <returns></returns>
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {

            switch (MenuName)
            {
                // main menu
                case "":
                    return menuHeader;
                // return all items for main menu
                case menuHeader:
                    string[] subMenus = { menuItemAbout };
                    return subMenus;
            }

            return "";
        }


        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            switch (ItemName)
            {
                case menuItemAbout:
                    IsEnabled = true;
                    break;
                default:
                    IsEnabled = false;
                    break;
            }
        }

        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                case menuItemAbout:
                    ///JTS, must be coverted into Action execution calling
                    (new View.About()).ShowDialog();
                    break;
                default:
                    throw new NotImplementedException($"Operation: {nameof (EA_MenuClick)} {nameof (ItemName)}:{ItemName}");
            }
        }


        /// <summary>
        /// EA_OnPostNewConnector notifies Add-Ins that a new
        ///connector has been created on a diagram.It enables Add-Ins
        ///to modify the connector upon creation.
        ///
        ///This event occurs after a user has dragged a new
        ///connector from the Toolbox or Resources window onto a
        ///diagram.The notification is provided immediately after the
        ///connector is added to the model.
        /// </summary>
        /// <param name="Repository">An EA.Repository object representing the currently open Enterprise Architect model.</param>
        /// <param name="EventProperties">Contains the following EventProperty objects for the new connector:
        /// ConnectorID: Along value corresponding to Connector. ConnectorID</param>
        /// <returns>Return True if the connector has been updated during this notification. Return False otherwise.</returns>
        public bool EA_OnPostNewConnector(EA.Repository Repository, EA.EventProperties EventProperties)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                try
                {
                    string strConnectorID = EventProperties.Get("ConnectorID").Value;
                    int intConnectorID = int.Parse(strConnectorID);

                    EA.Connector conn = Repository.GetConnectorByID(intConnectorID);

                    if (conn.MetaType == "Association" || conn.MetaType == "Aggregation") 
                    {
                        ViewModel.GoatAssociation GoatAssociation = new ViewModel.GoatAssociation(new Model.GoatAssociation(), conn);
                        View.GoatAssociation dlg = new View.GoatAssociation();
                        dlg.DataContext = GoatAssociation;
                        if (dlg.ShowDialog() == true)
                        {
                            return true;
                        }
                        else
                            return false; 
                    }
                }
                catch (Exception)
                {
                    
                }

            }
            return false;
        }

    }
}
