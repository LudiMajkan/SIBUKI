using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CreateAndManageCertificate
{
    public static class MakeCert
    {
        public static List<X509Certificate2> certificates = new List<X509Certificate2>();
        public static List<X509Certificate2> revocationList = new List<X509Certificate2>();

        /// <summary>
        /// Creates numOfCerts certificates.
        /// </summary>
        /// <param name="numOfCerts">Number of certificates to create.</param>
        public static void CreateCerts(int numOfCerts)
        {
            for (int i = 0; i < numOfCerts; i++)
            {
                Console.WriteLine("Enter name for .cer and .pvk files");
                string cerAndpvkName = Console.ReadLine();
                Console.WriteLine("Is certificate self signed? (Y/N)");
                string isSelfSigned = Console.ReadLine();
                string selfSigned = "";
                string issuerCerFileName = "";
                string issuerPvkFileName = "";
                string skyExchange = " -sky exchange ";
                if (isSelfSigned.ToUpper().Equals("Y"))
                {
                    selfSigned = "-r ";
                    skyExchange = " -sky exchange ";
                }
                else
                {
                    Console.WriteLine("Enter issuer .cer file");
                    issuerCerFileName = " -ic " + Console.ReadLine() + ".cer";
                    Console.WriteLine("Enter issuer .pvk file");
                    issuerPvkFileName = " -iv " + Console.ReadLine() + ".pvk";
                }
                string makecertCMD = "makecert.exe " + " -n \"CN=" + cerAndpvkName + "\" " + selfSigned + "-pe " + "-cy authority " + issuerCerFileName + issuerPvkFileName +
                                     skyExchange + "-sv " + cerAndpvkName + ".pvk " + cerAndpvkName + ".cer";
                Console.WriteLine(makecertCMD);
                Process p = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.RedirectStandardInput = true;
                info.UseShellExecute = false;
                p.StartInfo = info;
                p.Start();
                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd..");
                        sw.WriteLine("cd..");
                        sw.WriteLine(makecertCMD);
                        sw.WriteLine("cls");
                        sw.WriteLine("exit");
                    }
                }
                p.WaitForExit();
                certificates.Add(MakeCert.CreateCertObj(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + cerAndpvkName + ".cer", null));
                InitializeCertificateList.WriteCertificatePathAndPassword(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + cerAndpvkName + ".cer", "");
            }
        }

        public static void CreatePfxFile()
        {
            Console.WriteLine("Enter name for .pvk and .cer files");
            string namePvkAndCer = Console.ReadLine();
            Console.WriteLine("Enter name for .pfx file");
            string namePfx = Console.ReadLine();
            Console.WriteLine("Enter password");
            ConsoleKeyInfo key;
            string pass = "";
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            string pvk2pfxCMD = "pvk2pfx.exe -pvk " + namePvkAndCer + ".pvk" + " -pi " + pass + " -spc " + namePvkAndCer + ".cer " +
                                "-pfx " + namePfx + ".pfx" + " -f";
            Console.WriteLine(pvk2pfxCMD);
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            p.StartInfo = info;
            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd..");
                    sw.WriteLine("cd..");
                    sw.WriteLine(pvk2pfxCMD);
                    sw.WriteLine("cls");
                    sw.WriteLine("exit");
                }
            }
            p.WaitForExit();
            certificates.Add(MakeCert.CreateCertObj(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + namePvkAndCer + ".pfx", pass));
            InitializeCertificateList.WriteCertificatePathAndPassword(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + namePvkAndCer + ".pfx", pass);
        }

        public static void InstallCert(X509Certificate2 cert = null, string name = null, bool hasPrivateKey = true)
        {
            if (cert != null)
            {
                if (!cert.Issuer.Equals(cert.SubjectName.Name))
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
                else
                {
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(cert);
                    store.Close();
                }
            }
            else if (name != null && cert == null)
            {
                cert = FindCertByName(name, hasPrivateKey);
                if (cert != null)
                {
                    if (!cert.Issuer.Equals(cert.SubjectName.Name))
                    {
                        if(!cert.HasPrivateKey)
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
                    else
                    {
                        X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                        store.Open(OpenFlags.ReadWrite);
                        store.Add(cert);
                        store.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Certificate with given name does not exist in evidence!");
                }
            }
        }

        public static X509Certificate2 FindCertByName(string name, bool hasPrivateKey)
        {
            foreach (X509Certificate2 cert in certificates)
            {
                if (name.Contains("CN="))
                {
                    if (cert.SubjectName.Name.Equals(name))
                    {
                        if (cert.HasPrivateKey.Equals(hasPrivateKey))
                        {
                            return cert;
                        }
                    }
                }
                else
                {
                    if (cert.SubjectName.Name.Equals("CN=" + name))
                    {
                        if (cert.HasPrivateKey.Equals(hasPrivateKey))
                        {
                            return cert;
                        }
                    }
                }
            }
            return null;
        }

        public static void RemoveCert(X509Certificate2 cert)
        {
            if (!cert.Issuer.Equals(cert.SubjectName.Name))
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(cert);
                store.Close();
            }
            else
            {
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(cert);
                store.Close();
            }
        }

        public static void SendCertificateToRevocationList(X509Certificate2 cert)
        {
            RemoveCert(cert);
            certificates.Remove(cert);
            revocationList.Add(cert);
        }

        public static X509Certificate2 CreateCertObj(string path, string password)
        {
            X509Certificate2 cert;
            if (password != null)
            {
                cert = new X509Certificate2(path, password, X509KeyStorageFlags.MachineKeySet);
            }
            else
            {
                cert = new X509Certificate2(path);
            }
            return cert;
        }

        public static void GiveRights()
        {

            Console.WriteLine("Enter user account name you wish to grant private key accesss");
            string accName = Console.ReadLine();
            Console.WriteLine("Enter certificate name");
            string certName = Console.ReadLine();
            Console.WriteLine("Is certificate root or  personal? (R/P)");
            string certPlace = Console.ReadLine();
            string certPath = "My";
            if(certPlace.ToUpper().Equals("R"))
            {
                certPath = "Root";
            }
            string grantAccessCommand = "winhttpcertcfg.exe -g -c LOCAL_MACHINE\\" + certPath + " -s " + certName + " -a " + accName;

            Console.WriteLine(grantAccessCommand);

            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            p.StartInfo = info;
            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd..");
                    sw.WriteLine("cd..");
                    sw.WriteLine(grantAccessCommand);
                    //sw.WriteLine("cls");
                    sw.WriteLine("exit");
                }
            }
            p.WaitForExit();
        }

        public static void DenyRights()
        {
            Console.WriteLine("Enter user account name you wish to deny private key accesss");
            string accName = Console.ReadLine();
            Console.WriteLine("Enter certificate name");
            string certName = Console.ReadLine();
            Console.WriteLine("Is certificate root or  personal? (R/P)");
            string certPlace = Console.ReadLine();
            string certPath = "My";
            if (certPlace.ToUpper().Equals("R"))
            {
                certPath = "Root";
            }
            string denyAccessCommand = "winhttpcertcfg.exe -r -c LOCAL_MACHINE\\" + certPath + " -s " + certName + " -a " + accName;

            Console.WriteLine(denyAccessCommand);

            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            p.StartInfo = info;
            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd..");
                    sw.WriteLine("cd..");
                    sw.WriteLine(denyAccessCommand);
                    //sw.WriteLine("cls");
                    sw.WriteLine("exit");
                }
            }
            p.WaitForExit();
        }
        public static void RevokeCert(string name)
        {
            X509Certificate2 certWithoutPrivateKey = FindCertByName(name, false);
            X509Certificate2 certWithPrivateKey = FindCertByName(name, true);
            revocationList.Add(certWithoutPrivateKey);
            revocationList.Add(certWithPrivateKey);
            certificates.Remove(certWithoutPrivateKey);
            certificates.Remove(certWithPrivateKey);
            string[] strs = new string[certificates.Count];
            string[] strsFromFile = System.IO.File.ReadAllLines(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString())
                + @"\CreateAndManageCertificate\CertiicatePathsAndPasswords.txt");
            int writeTostrs = 0;
            for(int i = 0; i < certificates.Count + 2; i++)
            {
                if(!strsFromFile[i].Contains(name + ".cer") && !strsFromFile[i].Contains(name + ".pfx"))
                {
                    strs[writeTostrs] = strsFromFile[i];
                    writeTostrs++;
                }
                else
                {
                    if(File.Exists(strsFromFile[i].Split(',')[0]))
                    {
                        File.Delete(strsFromFile[i].Split(',')[0]);
                        if(strsFromFile[i].Contains(name + ".cer"))
                        {
                            File.Delete(strsFromFile[i].Split(',')[0].Replace(name + ".cer", name + ".pvk"));
                        }
                        else if(strsFromFile[i].Contains(name + ".pfx"))
                        {
                            File.Delete(strsFromFile[i].Split(',')[0].Replace(name + ".pfx", name + ".pvk"));
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString())
                + @"\CreateAndManageCertificate\CertiicatePathsAndPasswords.txt", strs);
        }
    }
}

