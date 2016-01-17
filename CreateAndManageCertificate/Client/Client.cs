using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationContract;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;

namespace Client
{
    public class Client : ChannelFactory<IContract>, IContract, IDisposable
    {

        IContract factory;
        public Client(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.SetCertificate("CN=client", StoreLocation.LocalMachine, StoreName.My);
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomValidator();
            factory = this.CreateChannel();
        }

        public void PingServer(TimeSpan t)
        {
            factory.PingServer(t);
        }
    }
}
