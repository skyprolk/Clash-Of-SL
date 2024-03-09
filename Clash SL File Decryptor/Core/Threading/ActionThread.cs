using System;
using System.Threading;
using System.Windows.Forms;
using CSFD.Core;
using CSFD.Core.Helpers;

namespace CSFD.Core.Threading
{
    internal class ActionThread
    {
        public static void Start(Types action)
        {
            if (action == Types.CsvDecrypt)
            {
                Thread T = new Thread(() =>
                {
                    CSVDecryptor.Decrypt();
                }); T.Start();
            }
            else if (action == Types.CsvEncrypt)
            {
                Thread T = new Thread(() =>
                {
                    CSVEncryptor.Encrypt();
                }); T.Start();
            }
            else if (!Settings.ScVersion.HasValue)
            {
                MessageBox.Show("Please Choose a Version of the SC Files", "No Chosen Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Settings.ScVersion.HasValue)
            {
                if (action == Types.ScDecrypt & Settings.ScVersion == true)
                {
                    var warn = MessageBox.Show("Warning: The SC Decryptor/Encryptor is only compatible" +
                        Environment.NewLine + "with version 7.XXX gamefiles or lower. If you wish to proceed hit \"OK\", otherwise hit canel.",
                        "Warning: Compatible Gamefiles Version",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (warn == DialogResult.OK)
                    {

                        Thread T = new Thread(() =>
                        {
                            SCDecryptor.Decrypt();
                        }); T.Start();
                    }
                }
                else if (action == Types.ScDecrypt & Settings.ScVersion == false)
                {
                    //NO DECRYPTION FOR 8.X YET
                }
                else if (action == Types.ScEncrypt & Settings.ScVersion == true)
                {
                    var warn1 = MessageBox.Show("Warning: The SC Decryptor/Encryptor is only compatible" +
                        Environment.NewLine + "with version 7.XXX gamefiles or lower. If you wish to proceed hit \"OK\", otherwise hit canel.",
                        "Warning: Compatible Gamefiles Version",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (warn1 == DialogResult.OK)
                    {
                        Thread T = new Thread(() =>
                        {
                            SCEncryptor.Encrypt();
                        }); T.Start();
                    }
                }
                else if (action == Types.ScEncrypt & Settings.ScVersion == false)
                {
                    //NO DECRYPTION FOR 8.X YET
                }
                
            }
        }

        public enum Types
        {
            CsvEncrypt,
            CsvDecrypt,
            ScEncrypt,
            ScDecrypt
        }
    }
}
