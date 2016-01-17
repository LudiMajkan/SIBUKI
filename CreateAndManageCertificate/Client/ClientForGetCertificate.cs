using CreateAndManageCertificateServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientForGetCertificate : ChannelFactory<ICreateAndManageCertificateServiceContract>, ICreateAndManageCertificateServiceContract, IDisposable
    {

        ICreateAndManageCertificateServiceContract factory;
        public ClientForGetCertificate(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public byte[] GetCertificateName(string name, string accountName)
        {
            return factory.GetCertificateName(name, accountName);
        }


        public bool IsValid(X509Certificate2 certificate)
        {
            return factory.IsValid(certificate);
        }
    }
}
