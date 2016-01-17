using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        public static bool ExitThreadMethod()
        {
            string a = "";
            do
            {
                a = Console.ReadLine();

            } while (!a.ToUpper().Equals("E"));
            return true;
        }

        public static void InstalCertificate(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
            {
                X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();
            }
            else
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();
            }
        }

        static void Main(string[] args)
        {
            /*Console.WriteLine("Do you want to get cerificate?(Y/N)");
            string getCertificate = Console.ReadLine();
            if (getCertificate.ToUpper().Equals("Y"))
            {
                Console.WriteLine("Give me name for certificate!");
                string name = Console.ReadLine();
                NetTcpBinding bindingForGetCertificate = new NetTcpBinding();
                string addressForGetCertificate = "net.tcp://localhost:27016/CreateAndManageCertificateServiceContract";
                EndpointAddress epaForGetCertificate = new EndpointAddress(new Uri(addressForGetCertificate));
                using(ClientForGetCertificate proxyForGetCertificate = new ClientForGetCertificate(bindingForGetCertificate, new EndpointAddress(new Uri(addressForGetCertificate))))
                {
                    string accountName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    MemoryStream memStream = new MemoryStream();
                    BinaryFormatter binForm = new BinaryFormatter();
                    byte[] byteArray = proxyForGetCertificate.GetCertificateName(name, accountName);
                    memStream.Write(byteArray, 0, byteArray.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    X509Certificate2 cert = (X509Certificate2)binForm.Deserialize(memStream);
                    
                    InstalCertificate(cert);
                }
                
            }*/
            bool exit = false;
            System.Threading.Thread exitThread = new System.Threading.Thread(() => { exit = ExitThreadMethod(); });
            exitThread.Start();
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:9999/WCFService";
            EndpointAddress epa = new EndpointAddress(new Uri(address));

            Console.WriteLine("Client started.\nPress \'e\' to exit.");
            using (Client proxy = new Client(binding, new EndpointAddress(new Uri(address),new X509CertificateEndpointIdentity(
                                  new X509Certificate2(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()) + @"\CreateAndManageCertificate\srv.cer")))))
            {
                Random rnd = new Random();
                int sleepInterval = 0;
                while (!exit)
                {
                    sleepInterval = rnd.Next(1, 10);
                    System.Threading.Thread.Sleep(sleepInterval * 1000);
                    proxy.PingServer(DateTime.Now.TimeOfDay);
                }
            }
        }
    }
}
