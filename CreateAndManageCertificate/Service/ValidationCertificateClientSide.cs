using CreateAndManageCertificateServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Service
{
    class ValidationCertificateClientSide : ChannelFactory<ICreateAndManageCertificateServiceContract>, ICreateAndManageCertificateServiceContract, IDisposable
    {
        ICreateAndManageCertificateServiceContract factory;
        public ValidationCertificateClientSide(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }


        public byte[] GetCertificateName(string name, string accountName)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            return factory.IsValid(certificate);
        }
    }
}
