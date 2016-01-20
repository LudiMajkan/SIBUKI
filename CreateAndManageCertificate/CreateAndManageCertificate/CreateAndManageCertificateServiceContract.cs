using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CreateAndManageCertificateServiceContract;
using System.Runtime.Serialization.Formatters.Binary;

namespace CreateAndManageCertificate
{
    public class CreateAndManageCertificateServiceContract : ICreateAndManageCertificateServiceContract
    {
        #region GetCertificateName currently not in use and returns null
        public byte[] GetCertificateName(string name, string accountName)
        {
            /*#region Create .cer and .pvk

            string cerAndpvkName = name;
            string selfSigned = "";
            string issuerCerFileName = "";
            string issuerPvkFileName = "";
            string skyExchange = " -sky exchange ";

            issuerCerFileName = " -ic issuer.cer";
            issuerPvkFileName = " -iv issuer.pvk";

            string makecertCMD = "makecert.exe " + " -n \"CN=" + cerAndpvkName + "\" " + selfSigned + "-pe " + "-cy authority " +
                                    issuerCerFileName + issuerPvkFileName + skyExchange + "-sv " + cerAndpvkName + ".pvk " + cerAndpvkName + ".cer";

            Process p1 = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.FileName = "cmd.exe";
            info1.RedirectStandardInput = true;
            info1.UseShellExecute = false;
            p1.StartInfo = info1;
            p1.Start();
            using (StreamWriter sw = p1.StandardInput)
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
            p1.WaitForExit();
            MakeCert.certificates.Add(MakeCert.CreateCertObj(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + cerAndpvkName + ".cer", null));

            #endregion

            #region Create .pfx

            string namePvkAndCer = name;
            string namePfx = name;
            Console.WriteLine("Enter password");
            ConsoleKeyInfo key;
            string pass = Console.ReadLine();
           /* do
            {
                key = Console.ReadKey();
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
            } while (key.Key != ConsoleKey.Enter);*/
            /*string pvk2pfxCMD = "pvk2pfx.exe -pvk " + namePvkAndCer + ".pvk" + " -pi " + pass + " -spc " + namePvkAndCer + ".cer " +
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
            X509Certificate2 cert = MakeCert.CreateCertObj(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + @"\" + namePvkAndCer + ".pfx", pass);
            MakeCert.certificates.Add(cert);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, cert);
                return ms.ToArray();
            }
            #endregion*/
            return null;
        }
        #endregion

        public bool IsValid(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                return false;
            }
            if (certificate.NotAfter < DateTime.Now)
            {
                return false;
            }

            X509Certificate2 issuer = MakeCert.FindCertByName(certificate.IssuerName.Name, certificate.HasPrivateKey);

            if (issuer == null)
            {
                return false;
            }

            return !MakeCert.revocationList.Contains(certificate);
        }
    }
}
