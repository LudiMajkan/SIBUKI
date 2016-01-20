using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using CreateAndManageCertificateServiceContract;

namespace CreateAndManageCertificate
{
    class Program
    {
        static void Main(string[] args)
        {

            InitializeCertificateList.ImportCertificates();

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
            string address = "net.tcp://localhost:27016/CreateAndManageCertificateServiceContract";
            ServiceHost host = new ServiceHost(typeof(CreateAndManageCertificateServiceContract));
            host.AddServiceEndpoint(typeof(ICreateAndManageCertificateServiceContract), binding, address);

            host.Open();
            Console.WriteLine("WCFService is opened.");

            bool shouldIExitApp = false;
            Console.WriteLine("Welcome to infrastructure for creating certificates, installing and using permission giving!");
            Console.WriteLine("\nJust follow our program and you will have no problems creating any number of certificates!");
            do
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Create certificate .cer and .pvk files");
                Console.WriteLine("2. Create certificate .pfx file");
                Console.WriteLine("3. Install certificate (BETA!)");
                Console.WriteLine("4. Create and install certificate (BETA!)");
                Console.WriteLine("5. Grant access to the private key");
                Console.WriteLine("6. Deny access to the private key");
                Console.WriteLine("7. Revoke certificate");
                Console.WriteLine("8. Exit our precious application. :(");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        MakeCert.CreateCerts(1);
                        break;

                    case "2":
                        MakeCert.CreatePfxFile();
                        break;

                    case "3":
                        Console.WriteLine("Enter certificate name that you want to install");
                        string name = Console.ReadLine();
                        Console.WriteLine("Does certificate have private key? (Y/N)");
                        string hasPrivateKeyS = Console.ReadLine();
                        bool hasPrivateKey = false;
                        hasPrivateKey = hasPrivateKeyS.ToUpper().Equals("Y");

                        MakeCert.InstallCert(null, name, hasPrivateKey);
                        
                        break;

                    case "4":
                        MakeCert.CreateCerts(1);
                        MakeCert.CreatePfxFile();
                        MakeCert.InstallCert(MakeCert.certificates[MakeCert.certificates.Count - 1]);

                        break;

                    case "5":
                        MakeCert.GiveRights();

                        break;
                    
                    case "6":
                        MakeCert.DenyRights();

                        break;
                    
                    case "7":
                        Console.WriteLine("Enter name for certificate that you want to revoke");
                        string nameForRevocation = Console.ReadLine();
                        MakeCert.RevokeCert(nameForRevocation);

                        break;
                    
                    case "8":
                        shouldIExitApp = true;

                        break;
                    
                    default:
                        break;
                }
            } while (!shouldIExitApp);
            host.Close();
        }
    }
}
