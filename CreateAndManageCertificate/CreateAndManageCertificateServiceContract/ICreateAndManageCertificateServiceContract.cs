using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndManageCertificateServiceContract
{
    [ServiceContract]
    public interface ICreateAndManageCertificateServiceContract
    {
        [OperationContract]
        byte[] GetCertificateName(string name, string accountName);

        [OperationContract]
        bool IsValid(X509Certificate2 certificate);
    }
}
