using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using PetaPocoWebTest.Database;

namespace PetaPoco.Profiling
{
    [GlimpsePlugin]
    public class PetaPocoGlimpsePlugin : IGlimpsePlugin
    {
        public object GetData(HttpApplication application)
        {
            if (application.Context.Items[DatabaseWithGlimpseProfiling.PetaKey] == null)
                return new List<object[]> { new[] { "Log" }, new[] { "No database requests or database logging not switched on (Compilation debug='true' or ForceLogging='true' on PetaPoco.DatabaseWithLogging)", "warn" } };

            var sqls = new List<object[]> {new[] {"#", "Time(ms)", "Sql", "Parameters"}};

            var i = 1;
            foreach (var item in DatabaseWithGlimpseProfiling.CurrentRequestInfo)
            {
                var parameterHeadings = new List<object[]> {new[] {"Name", "Type", "Value"}};
                parameterHeadings.AddRange(item.Parameters.Cast<IDataParameter>().Select(pm => new[] { pm.ParameterName, pm.Value.GetType().Name, pm.Value }));

                object parameters = parameterHeadings.Count > 1 ? parameterHeadings : null;
                
                var formattedTime = item.Time.ToString("0.#0");
                var time = item.Time > 100 ? "*"+formattedTime+"*" : formattedTime;

                sqls.Add(new[] { i++, time, item.Sql, parameters });
            }

			HttpContext.Current.Items[PetaPocoWebTest.Database.DatabaseWithGlimpseProfiling.PetaKey] = null; //ELMS: Clear it out

            return sqls;
        }

        public void SetupInit(HttpApplication application)
        {
        }

        public string Name
        {
            get { return "PetaPoco"; }
        }
    }
}