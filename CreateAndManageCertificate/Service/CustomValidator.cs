using CreateAndManageCertificateServiceContract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace Service
{
    public class CustomValidator : X509CertificateValidator
    {

        public override void Validate(X509Certificate2 certificate)
        {
            NetTcpBinding bindingForValidateCertificate = new NetTcpBinding();
            string addressForValidateCertificate = "net.tcp://localhost:27016/CreateAndManageCertificateServiceContract";
            EndpointAddress epaForValidateCertificate = new EndpointAddress(new Uri(addressForValidateCertificate));
            using (ValidationCertificateClientSide proxyForValidateCertificate = new ValidationCertificateClientSide(bindingForValidateCertificate, new EndpointAddress(new Uri(addressForValidateCertificate))))
            {
                if(!proxyForValidateCertificate.IsValid(certificate))
                {
                    throw new SecurityTokenValidationException
                        ("Certificate is not valid");
                }
                proxyForValidateCertificate.Close();
            }
        }
    }
}
