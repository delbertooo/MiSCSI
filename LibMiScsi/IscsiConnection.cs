using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMiScsi
{
    public class IscsiConnection
    {
        public string NodeAddress { get; }
        public bool IsConnected { get; }

        internal IscsiConnection(string nodeAddress, bool isConnected)
        {
            NodeAddress = nodeAddress ?? throw new ArgumentNullException(nameof(nodeAddress));
            IsConnected = isConnected;
        }

        public override string ToString()
        {
            return "IscsiConnection(" + NodeAddress + ", connected=" + IsConnected + ")";
        }
    }

}
