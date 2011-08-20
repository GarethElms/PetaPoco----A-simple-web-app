using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using MvcMiniProfiler;

namespace PetaPocoWebTest.Database
{
	/// <summary>
	/// To use the MVC Mini Profiler you need to wrap the database connection object with
	/// the ProfiledDbConnection type. In PetaPoco v4.0.3 a callback was added to make
	/// this very easy to do. Now just instantiate DatabaseWithMVCMiniProfiler() instead
	/// of PetaPoco.DataBase() and you get profiled SQL with PetaPoco
	/// </summary>
	public class DatabaseWithMVCMiniProfiler : PetaPoco.Database
	{
		public DatabaseWithMVCMiniProfiler(IDbConnection connection) : base(connection) { }
        public DatabaseWithMVCMiniProfiler(string connectionStringName) : base(connectionStringName) { }
        public DatabaseWithMVCMiniProfiler(string connectionString, string providerName) : base(connectionString, providerName) { }
        public DatabaseWithMVCMiniProfiler(string connectionString, DbProviderFactory dbProviderFactory) : base(connectionString, dbProviderFactory) { }

		public override IDbConnection OnConnectionOpened( IDbConnection connection)
		{
			// wrap the connection with a profiling connection that tracks timings 
			return MvcMiniProfiler.Data.ProfiledDbConnection.Get( connection as DbConnection, MiniProfiler.Current);
		}
	}

	/// <summary>
	/// If you want Glimpse profiling and MVC Mini Profiler at the same time you need to 
	/// instantiate this class insted of PetaPoco.Database
	/// </summary>
	public class DatabaseWithMVCMiniProfilerAndGlimpse : DatabaseWithGlimpseProfiling
	{
		public DatabaseWithMVCMiniProfilerAndGlimpse(IDbConnection connection) : base(connection) { }
        public DatabaseWithMVCMiniProfilerAndGlimpse(string connectionStringName) : base(connectionStringName) { }
        public DatabaseWithMVCMiniProfilerAndGlimpse(string connectionString, string providerName) : base(connectionString, providerName) { }
        public DatabaseWithMVCMiniProfilerAndGlimpse(string connectionString, DbProviderFactory dbProviderFactory) : base(connectionString, dbProviderFactory) { }

		public override IDbConnection OnConnectionOpened( IDbConnection connection)
		{
			// wrap the connection with a profiling connection that tracks timings 
			return MvcMiniProfiler.Data.ProfiledDbConnection.Get( connection as DbConnection, MiniProfiler.Current);
		}
	}

	/// <summary>
	/// Instantiate this class (instead of PetaPoco.Database) when you want to have Glimpse
	/// profiling on all SQL. See http://schotime.net/blog/index.php/2011/05/31/petapoco-glimpse-nuget-package/
	/// </summary>
    public class DatabaseWithGlimpseProfiling : PetaPoco.Database
    {
        public DatabaseWithGlimpseProfiling(IDbConnection connection) : base(connection) { }
        public DatabaseWithGlimpseProfiling(string connectionStringName) : base(connectionStringName) { }
        public DatabaseWithGlimpseProfiling(string connectionString, string providerName) : base(connectionString, providerName) { }
        public DatabaseWithGlimpseProfiling(string connectionString, DbProviderFactory dbProviderFactory) : base(connectionString, dbProviderFactory) { }

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
			tempData[DatabaseWithGlimpseProfiling.PetaKey] = HttpContext.Current.Items[DatabaseWithGlimpseProfiling.PetaKey];
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