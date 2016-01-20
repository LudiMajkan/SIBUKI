using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace CommunicationContract
{
    [ServiceContract]
    public interface IContract
    {

        [OperationContract]
        void EstablishConnection();

        [OperationContract]
        void PingServer(TimeSpan t);
    }
}
