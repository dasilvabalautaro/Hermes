using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.repository
{
    class RepositoryDisk
    {
        #region methods
        public RepositoryDisk() { }

        public string readTextFile(string strPath)
        {
            StreamReader objStream;
            string strReturn = string.Empty;

            try
            {
                if (File.Exists(strPath))
                {
                    objStream = new StreamReader(strPath, Encoding.Default);
                    strReturn = objStream.ReadToEnd();
                    objStream.Close();
                }

            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new FileNotFoundException("Read File", e);
                
            }
            return strReturn;
        }

        #endregion


    }
}
