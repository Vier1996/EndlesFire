using Codebase.Library.Network.Time;
using UnityEngine;

namespace Codebase.Library.Network
{
    public class NetworkService
    {
        public NetworkTime Time { get; private set; }

        public NetworkService()
        {
            Time = new NetworkTime();
        }

        public bool IsConnectedToNetwork() => 
            Application.internetReachability != NetworkReachability.NotReachable;
    }
}
