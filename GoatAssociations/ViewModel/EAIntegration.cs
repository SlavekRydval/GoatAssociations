using GoatAssociations.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace GoatAssociations.ViewModel
{
    public class EAIntegration
    {
        private ViewModel.GoatAssociationAddin goatAssociationAddin = null;

        private MainViewModel mainVieModel = null; 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public String EA_Connect(EA.Repository Repository)
        {
            
            //MessageBox.Show("GoatAssociation: Append debugger, if needed.");

            goatAssociationAddin = new ViewModel.GoatAssociationAddin
            {
                Repository = Repository
            };

            mainVieModel = new MainViewModel
                (
                    null, 
                    new DialogService()
                );



            return "";
        }


        /// <summary>
        /// Lets tidy up the mess we have created
        /// It is called just once when EA is about to end
        /// </summary>
        public void EA_Disconnect()
        {
            goatAssociationAddin.Repository = null; 
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        public void EA_FileNew(EA.Repository Repository) => goatAssociationAddin.Repository = Repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        public void EA_FileOpen(EA.Repository Repository) => goatAssociationAddin.Repository = Repository;


        const string menuHeader = "-&Goat Associations";
        const string menuItemAbout = "&About Goat Associations…";

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Location"></param>
        /// <param name="MenuName"></param>
        /// <param name="ItemName"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="IsChecked"></param>
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            switch (ItemName)
            {
                case menuItemAbout:
                    IsEnabled = mainVieModel.AboutCommand.CanExecute(null);// goatAssociationAddin.AboutCommand.CanExecute(null);
                    break;
                default:
                    IsEnabled = false;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Location"></param>
        /// <param name="MenuName"></param>
        /// <param name="ItemName"></param>
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                case menuItemAbout:
                    mainVieModel.AboutCommand.Execute(null);
                    //goatAssociationAddin.AboutCommand.Execute(null);
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
                    EA.Connector conn = Repository.GetConnectorByID(int.Parse (EventProperties.Get("ConnectorID").Value));

                    if (goatAssociationAddin.EditAssociationCommand.CanExecute (conn)) 
                    {
                        goatAssociationAddin.EditAssociationCommand.Execute(conn);
                        return goatAssociationAddin.EditAssociationCommand.Result;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);    
                }

            }
            return false;
        }

        public bool EA_OnContextItemDoubleClicked(EA.Repository Repository, string GUID, EA.ObjectType ObjectType)
        {
            if (ObjectType == EA.ObjectType.otConnector)// && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                EA.Connector conn = Repository.GetConnectorByGuid(GUID);

                if (goatAssociationAddin.EditAssociationCommand.CanExecute(conn))
                {
                    goatAssociationAddin.EditAssociationCommand.Execute(conn);

                    if (goatAssociationAddin.EditAssociationCommand.Result && 
                        (Repository.GetCurrentDiagram()?.SelectedConnector.ConnectorGUID == conn.ConnectorGUID))
                    {
                        Repository.SaveDiagram (Repository.GetCurrentDiagram().DiagramID);
                        Repository.ReloadDiagram(Repository.GetCurrentDiagram().DiagramID);
                    }

                    return true; 
                }
            }

            return false; 
        }

    }
}
