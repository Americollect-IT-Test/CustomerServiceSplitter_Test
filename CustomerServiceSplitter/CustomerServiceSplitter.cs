using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;

namespace CustomerServiceSplitter
{
    class CustomerServiceSplitter
    {
        // variables

        private string connectionString_BH = CommonVariables.connectionString_BH;
        private string connectionString_BH_RO = CommonVariables.connectionString_BH_RO;
        private string connectionString_SQL = CommonVariables.connectionString;

        // data tables
        DataTable dt_AllActions = new DataTable();
        DataTable dt_AllData = new DataTable();
        DataTable dt_Importance = new DataTable();

        // end variables


        public CustomerServiceSplitter()
        {
            using (OdbcConnection con_BH_RO = new OdbcConnection(connectionString_BH_RO))
            using (OdbcConnection con_BH = new OdbcConnection(connectionString_BH))
            using (OdbcConnection con_SQL = new OdbcConnection(connectionString_SQL))
            {
                con_BH_RO.Open();
                con_BH.Open();

                QueryActions(con_BH_RO);

                if (dt_AllActions.Rows.Count >= 1)
                {
                    UpdateCSFlag(con_BH);
                }

                // now query for all of the data, and start to get the details for each
                QueryAllData(con_BH_RO, con_SQL);

                con_BH_RO.Close();
                con_BH.Close();
            }
        }

        /// <summary>
        /// get the PCS ran in the last day
        /// </summary>
        /// <param name="inCN">Read only connection to get the actions that were ran</param>
        private void QueryActions(OdbcConnection inCN)
        {
            Console.WriteLine("Querying for all PCS actions ran in the last day.");

            using (OdbcCommand SelectCMD = new OdbcCommand(CommonVariables.selectSQL_ODBC, inCN))
            using (OdbcDataAdapter adapter = new OdbcDataAdapter(SelectCMD))
            {
                adapter.Fill(dt_AllActions);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inCN"></param>
        private void UpdateCSFlag(OdbcConnection inCN)
        {
            Console.WriteLine("Updating any PCS actions with the SendToCS Flag, and clearing the Pend Date.");

            foreach (DataRow dr in dt_AllActions.Rows)
            {
                // take the data and make sure the CS flag is set to yes, as they ran an action and it should be (they're not worked the same day as they don't populate the same day)
                string[] newWindowData = new string[16];
                string[] previousData = AMC_Functions.GeneralFunctions.SplitAdata(dr["ADATA:A5"].ToString());

                // assign the fields, all previous fields except for the pend date (if there is one), and the flag being yes
                newWindowData[0] = previousData[0];
                newWindowData[1] = previousData[1];
                newWindowData[2] = previousData[2];
                newWindowData[3] = previousData[3];
                newWindowData[4] = previousData[4];
                newWindowData[5] = previousData[5];
                newWindowData[6] = previousData[6];
                newWindowData[7] = previousData[7];
                newWindowData[8] = previousData[8];
                newWindowData[9] = previousData[9];
                newWindowData[10] = previousData[10];
                newWindowData[11] = previousData[11];
                newWindowData[12] = previousData[12];
                newWindowData[13] = string.Empty;       // pend date
                newWindowData[14] = previousData[14];
                newWindowData[15] = "yes";              // CS flag

                // update all the datas
                AMC_Functions.UpdateBHWindows BHUpdate = new AMC_Functions.UpdateBHWindows(dr["amanumber"].ToString(), "AW", "5", inCN, newWindowData, CommonVariables.ProcessID);

            }
        }

        private struct importanceWindata
        {
            public importanceWindata(string inName, string inCode, string inType, string inNumber, string inAgency, string inElement, string inBloodhoundField)
            {
                windowName = inName;
                windowCode = inCode;
                windowType = inType;
                windowNumber = inNumber;
                windowAgency = inAgency;
                windowElement = inElement;
                BloodhoundField = inBloodhoundField;
            }

            public string windowName { get; }
            public string windowCode { get; }
            public string windowType { get; }
            public string windowNumber { get; }
            public string windowAgency { get; }
            public string windowElement { get; }
            public string BloodhoundField { get; }
        }

        /// <summary>
        /// Get all of the main data from the query,
        /// </summary>
        /// <param name="inCN"></param>
        private void QueryAllData(OdbcConnection inCN, OdbcConnection inSQL)
        {
            using (OdbcCommand SelectCMD = new OdbcCommand(CommonVariables.selectSQL_ODBC, inCN))
            using (OdbcDataAdapter adapter = new OdbcDataAdapter(SelectCMD))
            {
                adapter.Fill(dt_AllData);
            }

            // now, iterate through and get the window data elements to determine what the most important portion is, 
            string selectSQL_Importance = $@"SELECT * from `{CommonVariables.CSImportanceTable}`";

            using (OdbcCommand SelectCMD = new OdbcCommand(selectSQL_Importance, inCN))
            using (OdbcDataAdapter adapter = new OdbcDataAdapter(SelectCMD))
            {
                adapter.Fill(dt_Importance);
            }

            // add each in a dictionary to figure out how many joins we need, and each for a list as far as each element
            Dictionary<string, importanceWindata> dict_Joins = new Dictionary<string, importanceWindata>();
            List<importanceWindata> allElementQueries = new List<importanceWindata>();

            foreach (DataRow dr in dt_Importance.Rows)
            {
                if (!dict_Joins.ContainsKey(dr["windata_Name"].ToString().ToUpper()))
                {
                    // make sure it's not null or empty, since we don't care about those
                    dict_Joins.Add(dr["windata_Name"].ToString().ToUpper(), new importanceWindata(dr["windata_Name"].ToString(), dr["windata_Code"].ToString(), dr["windata_Type"].ToString(), dr["windata_Number"].ToString(), dr["windata_Agency"].ToString(), dr["windata_Element"].ToString(), dr["BloodhoundField"].ToString()));
                }

                allElementQueries.Add(new importanceWindata(dr["windata_Name"].ToString(), dr["windata_Code"].ToString(), dr["windata_Type"].ToString(), dr["windata_Number"].ToString(), dr["windata_Agency"].ToString(), dr["windata_Element"].ToString(), dr["BloodhoundField"].ToString()));
            }

            // build the select string for BH with the elements provided
            string selectSQL_Reason = "";

            foreach (importanceWindata element in allElementQueries)
            {
                if (selectSQL_Reason == "")
                {
                    selectSQL_Reason = $"SELECT {element.windowName}.wdtext[{element.windowElement}]";
                }
                else
                {
                    selectSQL_Reason += $", {element.windowName}.wdtext[{element.windowElement}]";
                }
            }

            // add the "PUB.acctmstr, just for a basis
            selectSQL_Reason += " FROM PUB.acctmstr ";

            // now, go through the dictionary and add each of the left joins in there
            foreach (importanceWindata element in dict_Joins.Values)
            {
                selectSQL_Reason += $" LEFT JOIN PUB.windata {element.windowName} ON {element.windowName}.wdtype = {element.windowType} AND {element.windowName}.wdcode = {element.windowCode} and {element.windowName}.wdnumber = {element.windowNumber} and {element.windowName}.wdagency = {element.windowAgency}";
            }

            // add the WHERE
            selectSQL_Reason += " WHERE acctwin_5.wdtext[16] != ''";


        }
    }
}
