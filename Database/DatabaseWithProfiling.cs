using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace PetaPocoWebTest.Database
{
    public class DatabaseWithProfiling : PetaPoco.Database
    {
        public DatabaseWithProfiling(IDbConnection connection) : base(connection) { }
        public DatabaseWithProfiling(string connectionStringName) : base(connectionStringName) { }
        public DatabaseWithProfiling(string connectionString, string providerName) : base(connectionString, providerName) { }
        public DatabaseWithProfiling(string connectionString, DbProviderFactory dbProviderFactory) : base(connectionString, dbProviderFactory) { }

        Stopwatch sw = new Stopwatch();
        private IDbCommand currentCommand;

        public bool ForceLogging { get; set; }

        public override void OnExecutingCommand(IDbCommand cmd)
        {
            if (ForceLogging || HttpContext.Current.IsDebuggingEnabled)
            {
                currentCommand = cmd;
                sw.Reset();
                sw.Start();
            }
        }

        public override void OnExecutedCommand(IDbCommand cmd)
        {
            if (ForceLogging || HttpContext.Current.IsDebuggingEnabled)
            {
                sw.Stop();
                if (currentCommand == cmd)
                {
                    CurrentRequestInfo.Add(new PetaPocoSqlInfo
                    {
                        Time = sw.ElapsedTicks / 10000d,
                        FormattedSql = FormatCommand(cmd),
                        Sql = cmd.CommandText,
                        Parameters = cmd.Parameters
                    });
                }
            }
        }

        public static string PetaKey = "__petaPocoSqlInfo";
        public static List<PetaPocoSqlInfo> CurrentRequestInfo
        {
            get
            {
                if (HttpContext.Current.Items[PetaKey] == null)
				{
                    HttpContext.Current.Items[PetaKey] = new List<PetaPocoSqlInfo>();
				}

                return (List<PetaPocoSqlInfo>)HttpContext.Current.Items[PetaKey];
            }
        }

		public static void PersistGlimpseAfterRedirect( TempDataDictionary tempData)
		{
			tempData[DatabaseWithProfiling.PetaKey] = HttpContext.Current.Items[DatabaseWithProfiling.PetaKey];
		}

        public class PetaPocoSqlInfo
        {
            public double Time { get; set; }
            public string FormattedSql { get; set; }
            public string Sql { get; set; }
            public IDataParameterCollection Parameters { get; set; }
        }

    }
}