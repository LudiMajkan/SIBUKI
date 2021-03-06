﻿using CommunicationContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Security.Principal;
using System.IdentityModel.Tokens;


namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            string address = "net.tcp://localhost:9999/WCFService";
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost host = new ServiceHost(typeof(Service));
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            host.AddServiceEndpoint(typeof(IContract), binding, address);
            host.Credentials.ServiceCertificate.SetCertificate("CN=srv", StoreLocation.LocalMachine, StoreName.My);
            host.Open();

            NetTcpBinding bindingForCustomValidation = new NetTcpBinding(SecurityMode.Transport);
            string addressForCustomValidation = "net.tcp://localhost:10000/WCFService";
            bindingForCustomValidation.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost hostForCustomValidation = new ServiceHost(typeof(Service));
            hostForCustomValidation.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            hostForCustomValidation.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new CustomValidator();
            hostForCustomValidation.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostForCustomValidation.AddServiceEndpoint(typeof(IContract), bindingForCustomValidation, addressForCustomValidation);
            hostForCustomValidation.Credentials.ServiceCertificate.SetCertificate("CN=srv", StoreLocation.LocalMachine, StoreName.My);
            hostForCustomValidation.Open();

            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();
        }
    }
}
