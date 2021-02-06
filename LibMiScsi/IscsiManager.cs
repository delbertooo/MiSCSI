using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace LibMiScsi
{
    public class IscsiManager
    {

        public void Foo()
        {
            PowerShell ps = PowerShell.Create();
            //ps.AddCommand("Import-Module").AddParameter("Name", "iscsi").Invoke();
            Debug.WriteLine("import {0}", ps.HadErrors);
            var output = ps.AddCommand("Get-IscsiTarget").Invoke();
            Debug.WriteLine("get {0}", ps.HadErrors);
            foreach (var o in output)
            {
                Debug.WriteLine("Output: {0}", o);
            }
        }

        public List<IscsiConnection> ListConnections()
        {
            var output = PowerShell.Create().AddCommand("Get-IscsiTarget").Invoke();
            return output
                .Select(o => { return new IscsiConnection((string)o.Members["NodeAddress"].Value, (bool)o.Members["IsConnected"].Value); })
                .ToList();
        }

        public bool Disconnect(IscsiConnection connection)
        {
            if (!connection.IsConnected) return true;
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Disconnect-IscsiTarget").AddParameter("NodeAddress", connection.NodeAddress).AddParameter("Confirm", false).Invoke();
            return !ps.HadErrors;
        }
        public bool Connect(IscsiConnection connection)
        {
            if (connection.IsConnected) return true;
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Connect-IscsiTarget").AddParameter("NodeAddress", connection.NodeAddress).Invoke();
            return !ps.HadErrors;
        }
    }
}
