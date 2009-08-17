using System;
using System.Collections.Generic;
using System.Text;
using TwainDotNet.TwainNative;

namespace TwainDotNet
{
    public class Diagnostics
    {
        public Diagnostics(IWindowsMessageHook messageHook)
        {
            using (var dataSourceManager = new DataSourceManager(DataSourceManager.DefaultApplicationId, messageHook))
            {
                dataSourceManager.SelectSource();

                var dataSource = dataSourceManager.DataSource;
                dataSource.OpenSource();

                foreach (Capabilities capability in Enum.GetValues(typeof(Capabilities)))
                {
                    try
                    {
                        var result = Capability.GetBoolCapability(capability, dataSourceManager.ApplicationId, dataSource.SourceId);

                        Console.WriteLine("{0}: {1}", capability, result);
                    }
                    catch (TwainException e)
                    {
                        Console.WriteLine("{0}: {1} {2} {3}", capability, e.Message, e.ReturnCode, e.ConditionCode);
                    }
                }
            }
        }
    }
}
