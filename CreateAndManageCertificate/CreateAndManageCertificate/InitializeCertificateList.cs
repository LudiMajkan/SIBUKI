using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CreateAndManageCertificate
{
    public static class InitializeCertificateList
    {
        public static void ImportCertificates()
        {
            string[] lines = System.IO.File.ReadAllLines(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString())
                + @"\CreateAndManageCertificate\CertiicatePathsAndPasswords.txt");
            foreach(string line in lines)
            {
                if (!line.Equals(""))
                {
                    string[] splittedLine = line.Split(',');
                    if (splittedLine[1].Equals(""))
                    {
                        MakeCert.certificates.Add(new X509Certificate2(splittedLine[0]));
                    }
                    else
                    {
                        MakeCert.certificates.Add(new X509Certificate2(splittedLine[0], splittedLine[1]));
                    }
                }
            }
        }

        public static void WriteCertificatePathAndPassword(string path, string password)
        {
            string[] lines = {path + "," + password};
            System.IO.File.AppendAllLines(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString())
                + @"\CreateAndManageCertificate\CertiicatePathsAndPasswords.txt", lines);
        }
    }
}
