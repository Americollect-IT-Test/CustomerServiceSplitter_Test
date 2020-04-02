using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CustomerServiceSplitter
{
    public static class CommonVariables
    {
        public static double DayDifference = 1;

        public static string ProcessID
        {
            get { return "0552"; }
        }

        /// <summary>
        /// Flag for test mode - will change the BH query but not the tables.  ****update this if needed to flag from test or true, if changing to test need to modify the tables below as well****
        /// </summary>
        public static bool TestMode
        {
            get { return true; }
        }

        /// <summary>
        /// Flag for whether this is on reporting - shouldn't be as it does updates as well
        /// </summary>
        public static bool isReporting
        { 
            get { return false; }
        }

        /// <summary>
        /// connection string for the database, stored in connectionStrings.Config
        /// </summary>
        public static string connectionString
        {
            get { return ConfigurationManager.ConnectionStrings["CSConnectionString"].ConnectionString; }
        }

        /// <summary>
        /// get the uncommitted version for BH
        /// </summary>
        public static string connectionString_BH_RO
        {
            get { return AMC_Functions.DetermineDSNFile.getDSN("jerrodr", "DB1", false) ; }
        }

        /// <summary>
        /// get the committed version for BH
        /// </summary>
        public static string connectionString_BH
        {
            get { return AMC_Functions.DetermineDSNFile.getDSN("jerrodr", "DB1", true); }
        }

        /// <summary>
        /// Table for the worker data in itself
        /// </summary>
        public static string WorkerTable
        {
            //get { return "WorkerData"; }
            get { return "TEST_WorkerData"; }
        }

        /// <summary>
        /// Table for the splitter assignments
        /// </summary>
        public static string SplitterTable
        {
            //get { return "CS_Assignments"; }
            get { return "Test_CS_Assignments"; }
        }

        /// <summary>
        /// Table for the accounts that were added new since the last run
        /// </summary>
        public static string AddedDailyTable
        {
            //get { return "CS_Added_Daily"; }
            get { return "Test_CS_Added_Daily"; }
        }

        /// <summary>
        /// Table that stores the importance for the options chose
        /// </summary>
        public static string CSImportanceTable
        {
            //get { return "CS_Importance"; }
            get { return "Test_CS_Importance"; }
        }

        /// <summary>
        /// main query for adding the data into the database
        /// </summary>
        public static string selectSQL_ODBC
        {
            get { return $@"SELECT amanumber as 'Account Num', amstatus as 'Status', amcnumber as 'Cred_Num', dmfname as 'First Name', dmlname as 'Last name', amdnumber as 'Debtor Num', '' as 'Highest Importance', '' as 'Reason' '' as 'WORKER#', '' as 'Worker_Timestamp', {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} as 'date_inserted', credwin_L.wdtext[11] as 'Last Contact Date', credwin_L.wdtext[12] as 'Last Contact Time', cmalphcode as 'Alpha', acctwin_5.wdtext[14] as 'Pend Date' , '' as 'WrittenDispute', '' as 'Client Access', '' as 'Export Type', '' as 'Comment', amclacctno as 'Client Account #', amdatelstd as 'List Date', amlstactdt as 'Last Activity Date', ambalance as 'Current Balance', amlastdate as 'Last Pay Date', acctwin_C.wdtext[1] as 'Account', acctwin_C.wdtext[2] as 'Ticket', acctwin_C.wdtext[3] as 'Encounter', acctwin_C.wdtext[4] as 'PatientID', acctwin_C.wdtext[5] as 'Guarantor', acctwin_C.wdtext[6] as 'Visit #', acctwin_C.wdtext[7] as 'Charge ID', acctwin_C.wdtext[8] as 'MRN', acctwin_C.wdtext[9] as 'Order #', acctwin_C.wdtext[10] as 'Invoice #', acctwin_C.wdtext[11] as 'NPI #', acctwin_C.wdtext[12] as 'Customer #', acctwin_C.wdtext[13] as 'Clnt Tx ID', acctwin_C.wdtext[14] as 'Bill #', acctwin_C.wdtext[15] as 'CASE #', acctwin_C.wdtext[16] as 'Comp ID', acctwin_X4.wdtext[1] as 'Pharm NM', acctwin_X4.wdtext[2] as 'Address', acctwin_X4.wdtext[3] as 'Non-Abv-NM', acctwin_X4.wdtext[4] as 'Billing NM', acctwin_H.wdtext[1] as 'PT First Name', acctwin_H.wdtext[1] as 'PT Last Name', acctwin_H.wdtext[2] as 'PT SS#', acctwin_H.wdtext[3] as 'PT DOB', acctwin_H.wdtext[11] as 'DOC NAME', acctwin_H.wdtext[12] as 'CLINIC', dbtrwin_P.wdtext[9] as 'CHPT-CASE#', dbtrwin_P.wdtext[10] as 'FILE DATE', dbtrwin_P.wdtext[6] as 'DOD', dbtrwin_W.wdtext[1] as 'H-PAY FREQ', dbtrwin_W.wdtext[1] as '', dbtrwin_W.wdtext[2] as 'DEPARTMENT', dbtrwin_W.wdtext[3] as 'HRS WORKED', dbtrwin_W.wdtext[4] as '$/HR/SAL', dbtrwin_W.wdtext[5] as 'S-PAY FREQ', dbtrwin_W.wdtext[6] as 'S-DEPARTMENT', dbtrwin_W.wdtext[7] as 'S-HRS WORKED', dbtrwin_W.wdtext[8] as 'S - $/HR/SAL', dbtrwin_W.wdtext[9] as '# OF KIDS', dbtrwin_W.wdtext[10] as 'H-PKS/GKS$', dbtrwin_W.wdtext[11] as 'S-PKS/GKS', dbtrwin_W.wdtext[12] as 'MTG/RENT$', dbtrwin_W.wdtext[13] as 'RECV AID', dbtrwin_W.wdtext[14] as 'PP OFFERED', dbtrwin_W.wdtext[15] as 'OTHER:', dbtrwin_W.wdtext[16] as 'UPDATED', '' as 'Flagged', '' as 'IS Flagged', cmcname as 'Cred Name', '' as 'Account Status', amamtlstd as 'List Amount', dmssn as 'Guar SSN', '' as 'Interest', ambalance as 'Prin Bal', dmdob as 'Guar DOB', '' as 'Amount Paid', '' as 'Date Paid', dacolcode as 'Queue', acctwin_XA.wdtext[1] as 'Claims Paid Amount1', acctwin_XA.wdtext[2] as 'Claims Paid Date1', acctwin_XA.wdtext[3] as 'Claims Paid Amount2', acctwin_XA.wdtext[4] as 'Claims Paid Date2', acctwin_XA.wdtext[5] as 'Claims Paid Amount3', acctwin_XA.wdtext[6] as 'Claims Paid Date3', acctwin_XA.wdtext[7] as 'Claims Paid Amount4', acctwin_XA.wdtext[8] as 'Claims Paid Date4', akaname as 'AKAName', '' as 'NoticeType', 0 as 'DaysInWorker'
            FROM PUB.acctmstr
            JOIN PUB.damstr on PUB.damstr.dadnumber = acctmstr.amdnumber
            JOIN PUB.dbtrmstr on PUB.dbtrmstr.dmdnumber = acctmstr.amdnumber
            JOIN PUB.credmstr on PUB.credmstr.cmcnumber = acctmstr.amcnumber
            LEFT JOIN PUB.windata acctwin_C on acctwin_C.wdtype = 'A' and acctwin_C.wdcode = 'C' and acctwin_C.wdnumber = acctmstr.amanumber and acctwin_C.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata acctwin_H on acctwin_H.wdtype = 'A' and acctwin_H.wdcode = 'H' and acctwin_C.wdnumber = acctmstr.amanumber and acctwin_H.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata acctwin_X4 on acctwin_X4.wdtype = 'A' and acctwin_X4.wdcode = 'X4' and acctwin_X4.wdnumber = acctmstr.amanumber and acctwin_X4.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata acctwin_XC on acctwin_XC.wdtype = 'A' and acctwin_XC.wdcode = 'XC' and acctwin_XC.wdnumber = acctmstr.amanumber and acctwin_XC.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata acctwin_XD on acctwin_XD.wdtype = 'A' and acctwin_XD.wdcode = 'XD' and acctwin_XD.wdnumber = acctmstr.amanumber and acctwin_XD.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata acctwin_5 on acctwin_5.wdtype = 'A' and acctwin_5.wdcode = '5' and acctwin_5.wdnumber = acctmstr.amanumber and acctwin_5.wdagency = acctmstr.amagency
            LEFT JOIN PUB.windata credwin_L on credwin_L.wdtype = 'C' and credwin_L.wdcode = 'L' and credwin_L.wdnumber = acctmstr.amcnumber and credwin_L.wdagency = ''
            LEFT JOIN PUB.windata dbtrwin_W on dbtrwin_W.wdtype = 'D' and dbtrwin_W.wdcode = 'W' and dbtrwin_W.wdnumber = acctmstr.amdnumber and dbtrwin_W.wdagency = ''
            LEFT JOIN PUB.windata dbtrwin_P on dbtrwin_P.wdtype = 'D' and dbtrwin_P.wdcode = 'P' and dbtrwin_P.wdnumber = acctmstr.amdnumber and dbtrwin_P.wdagency = ''
            WHERE acctwin_5.wdtext[16] != ''"; }

            // TODO - update for current after running with test
            //get { return $@"SELECT amanumber as 'Account Num', amstatus as 'Status', amcnumber as 'Cred_Num', dmfname as 'First Name', dmlname as 'Last name', amdnumber as 'Debtor Num', '' as 'Highest Importance', '' as 'Reason' '' as 'WORKER#', '' as 'Worker_Timestamp', {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} as 'date_inserted', credwin_L.wdtext[11] as 'Last Contact Date', credwin_L.wdtext[12] as 'Last Contact Time', cmalphcode as 'Alpha', acctwin_5.wdtext[14] as 'Pend Date' , '' as 'WrittenDispute', '' as 'Client Access', '' as 'Export Type', '' as 'Comment', amclacctno as 'Client Account #', amdatelstd as 'List Date', amlstactdt as 'Last Activity Date', ambalance as 'Current Balance', amlastdate as 'Last Pay Date', acctwin_C.wdtext[1] as 'Account', acctwin_C.wdtext[2] as 'Ticket', acctwin_C.wdtext[3] as 'Encounter', acctwin_C.wdtext[4] as 'PatientID', acctwin_C.wdtext[5] as 'Guarantor', acctwin_C.wdtext[6] as 'Visit #', acctwin_C.wdtext[7] as 'Charge ID', acctwin_C.wdtext[8] as 'MRN', acctwin_C.wdtext[9] as 'Order #', acctwin_C.wdtext[10] as 'Invoice #', acctwin_C.wdtext[11] as 'NPI #', acctwin_C.wdtext[12] as 'Customer #', acctwin_C.wdtext[13] as 'Clnt Tx ID', acctwin_C.wdtext[14] as 'Bill #', acctwin_C.wdtext[15] as 'CASE #', acctwin_C.wdtext[16] as 'Comp ID', acctwin_X4.wdtext[1] as 'Pharm NM', acctwin_X4.wdtext[2] as 'Address', acctwin_X4.wdtext[3] as 'Non-Abv-NM', acctwin_X4.wdtext[4] as 'Billing NM', acctwin_H.wdtext[1] as 'PT First Name', acctwin_H.wdtext[1] as 'PT Last Name', acctwin_H.wdtext[2] as 'PT SS#', acctwin_H.wdtext[3] as 'PT DOB', acctwin_H.wdtext[11] as 'DOC NAME', acctwin_H.wdtext[12] as 'CLINIC', dbtrwin_P.wdtext[9] as 'CHPT-CASE#', dbtrwin_P.wdtext[10] as 'FILE DATE', dbtrwin_P.wdtext[6] as 'DOD', dbtrwin_W.wdtext[1] as 'H-PAY FREQ', dbtrwin_W.wdtext[1] as '', dbtrwin_W.wdtext[2] as 'DEPARTMENT', dbtrwin_W.wdtext[3] as 'HRS WORKED', dbtrwin_W.wdtext[4] as '$/HR/SAL', dbtrwin_W.wdtext[5] as 'S-PAY FREQ', dbtrwin_W.wdtext[6] as 'S-DEPARTMENT', dbtrwin_W.wdtext[7] as 'S-HRS WORKED', dbtrwin_W.wdtext[8] as 'S - $/HR/SAL', dbtrwin_W.wdtext[9] as '# OF KIDS', dbtrwin_W.wdtext[10] as 'H-PKS/GKS$', dbtrwin_W.wdtext[11] as 'S-PKS/GKS', dbtrwin_W.wdtext[12] as 'MTG/RENT$', dbtrwin_W.wdtext[13] as 'RECV AID', dbtrwin_W.wdtext[14] as 'PP OFFERED', dbtrwin_W.wdtext[15] as 'OTHER:', dbtrwin_W.wdtext[16] as 'UPDATED', '' as 'Flagged', '' as 'IS Flagged', cmcname as 'Cred Name', '' as 'Account Status', amamtlstd as 'List Amount', dmssn as 'Guar SSN', '' as 'Interest', ambalance as 'Prin Bal', dmdob as 'Guar DOB', '' as 'Amount Paid', '' as 'Date Paid', dacolcode as 'Queue', acctwin_XA.wdtext[1] as 'Claims Paid Amount1', acctwin_XA.wdtext[2] as 'Claims Paid Date1', acctwin_XA.wdtext[3] as 'Claims Paid Amount2', acctwin_XA.wdtext[4] as 'Claims Paid Date2', acctwin_XA.wdtext[5] as 'Claims Paid Amount3', acctwin_XA.wdtext[6] as 'Claims Paid Date3', acctwin_XA.wdtext[7] as 'Claims Paid Amount4', acctwin_XA.wdtext[8] as 'Claims Paid Date4', akaname as 'AKAName', '' as 'NoticeType', 0 as 'DaysInWorker'
            //FROM PUB.acctmstr
            //JOIN PUB.damstr on PUB.damstr.dadnumber = acctmstr.amdnumber
            //JOIN PUB.dbtrmstr on PUB.dbtrmstr.dmdnumber = acctmstr.amdnumber
            //JOIN PUB.credmstr on PUB.credmstr.cmcnumber = acctmstr.amcnumber
            //LEFT JOIN PUB.windata acctwin_C on acctwin_C.wdtype = 'A' and acctwin_C.wdcode = 'C' and acctwin_C.wdnumber = acctmstr.amanumber and acctwin_C.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata acctwin_H on acctwin_H.wdtype = 'A' and acctwin_H.wdcode = 'H' and acctwin_C.wdnumber = acctmstr.amanumber and acctwin_H.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata acctwin_X4 on acctwin_X4.wdtype = 'A' and acctwin_X4.wdcode = 'X4' and acctwin_X4.wdnumber = acctmstr.amanumber and acctwin_X4.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata acctwin_XC on acctwin_XC.wdtype = 'A' and acctwin_XC.wdcode = 'XC' and acctwin_XC.wdnumber = acctmstr.amanumber and acctwin_XC.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata acctwin_XD on acctwin_XD.wdtype = 'A' and acctwin_XD.wdcode = 'XD' and acctwin_XD.wdnumber = acctmstr.amanumber and acctwin_XD.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata acctwin_5 on acctwin_5.wdtype = 'A' and acctwin_5.wdcode = '5' and acctwin_5.wdnumber = acctmstr.amanumber and acctwin_5.wdagency = acctmstr.amagency
            //LEFT JOIN PUB.windata credwin_L on credwin_L.wdtype = 'C' and credwin_L.wdcode = 'L' and credwin_L.wdnumber = acctmstr.amcnumber and credwin_L.wdagency = ''
            //LEFT JOIN PUB.windata dbtrwin_W on dbtrwin_W.wdtype = 'D' and dbtrwin_W.wdcode = 'W' and dbtrwin_W.wdnumber = acctmstr.amdnumber and dbtrwin_W.wdagency = ''
            //LEFT JOIN PUB.windata dbtrwin_P on dbtrwin_P.wdtype = 'D' and dbtrwin_P.wdcode = 'P' and dbtrwin_P.wdnumber = acctmstr.amdnumber and dbtrwin_P.wdagency = ''
            //WHERE acctwin_5.wdtext[16] != ''"; }
        }

        /// <summary>
        /// Get the actions of the PCS that were ran
        /// </summary>
        public static string selectSQL_ODBC_Actions
        {
            get { return $@"SELECT ahagency, ahdnumber, ahactcode, ahdate, ahtime, ahappend, ahperson, amanumber, amcnumber, acctwin_5.wdtext as 'ADATA:A5'
                                    from PUB.actnacct
                                    JOIN PUB.actnhist on CAST(PUB.actnhist.ahtemp2 as INT) = PUB.actnacct.aahistid
                                    JOIN PUB.acctmstr on PUB.acctmstr.amaserial = PUB.actnacct.aaaserial
                                    LEFT JOIN PUB.windata acctwin_5 on acctwin_5.wdtype = 'A' and acctwin_5.wdcode = '5' and acctwin_5.wdnumber = amanumber and acctwin_5.wdagency = acctmstr.amagency
                                    WHERE ahactcode IN ('PCS') AND ahdate >= {DateTime.Today.AddDays(DayDifference)} WITH (NOLOCK)"; 
            }
        }

        /// <summary>
        /// reporting query for the main query - not currently used
        /// </summary>
        public static string selectSQL_SQL
        {
            get { return ""; }
        }

        /// <summary>
        /// reporting query for the actions query - not currently used
        /// </summary>
        public static string selectSQL_SQL_Actions
        {
            get { return ""; }
        }

    }
}
