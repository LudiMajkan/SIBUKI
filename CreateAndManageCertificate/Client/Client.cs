using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationContract;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;

namespace Client
{
    public class Client : ChannelFactory<IContract>, IContract, IDisposable
    {

        IContract factory;
        public Client(NetTcpBinding binding, EndpointAddress address, string validationType)
            : base(binding, address)
        {
            if (validationType.Equals("chain"))
            {
                this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                this.Credentials.ClientCertificate.SetCertificate("CN=client", StoreLocation.LocalMachine, StoreName.My);
                this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
                //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomValidator();
                factory = this.CreateChannel();
            }
            else
            {
                this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
                this.Credentials.ClientCertificate.SetCertificate("CN=custom", StoreLocation.LocalMachine, StoreName.My);
                this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
                this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new CustomValidator();
                factory = this.CreateChannel();
            }
        }

        public void PingServer(TimeSpan t)
        {
            try
            {
                factory.PingServer(t);
            }
            catch (SecurityTokenValidationException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void EstablishConnection()
        {
            try
            {
                factory.EstablishConnection();
            }
            catch (SecurityTokenValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
