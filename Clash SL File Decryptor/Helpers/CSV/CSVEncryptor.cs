using SevenZip;
using SevenZip.Compression.LZMA;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CSFD.Core.Helpers
{
    class CSVEncryptor
    {
        public static void Encrypt()
        {
            Encoder encoder = new Encoder();
            string[] files = Directory.GetFiles(@"In\Decrypted - CSV", "*.csv");
            string output = @"Out\Encrypted - CSV";

            int filesAmount = 0;
            foreach (string file in files)
            {
                filesAmount++;
            }
            if (filesAmount == 0)
            {
                MessageBox.Show("No CSV files were found in the folder \"In\\Decrypted - CSV.\""
                    + Environment.NewLine + "Place decrypted files in the folder and try again.",
                    "No Files Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            try
            {
                foreach (string str in files)
                {
                    FileInfo fileInfo = new FileInfo(str);
                    using (FileStream fileStream1 = new FileStream(str, FileMode.Open))
                    {
                        using (FileStream fileStream2 = new FileStream(Path.Combine(output, Path.GetFileName(str)), FileMode.Create))
                        {
                            CoderPropID[] propIDs = new CoderPropID[8]
                            {
                            CoderPropID.DictionarySize,
                            CoderPropID.PosStateBits,
                            CoderPropID.LitContextBits,
                            CoderPropID.LitPosBits,
                            CoderPropID.Algorithm,
                            CoderPropID.NumFastBytes,
                            CoderPropID.MatchFinder,
                            CoderPropID.EndMarker
                            };
                            object[] properties = new object[8]
                            {
                            (object) 262144,
                            (object) 2,
                            (object) 3,
                            (object) 0,
                            (object) 2,
                            (object) 32,
                            (object) "bt4",
                            (object) false
                            };
                            encoder.SetCoderProperties(propIDs, properties);
                            encoder.WriteCoderProperties((Stream)fileStream2);
                            fileStream2.Write(BitConverter.GetBytes(fileStream1.Length), 0, 4);
                            encoder.Code((Stream)fileStream1, (Stream)fileStream2, fileStream1.Length, -1L, (ICodeProgress)null);
                            fileStream2.Flush();
                            fileStream2.Close();
                        }
                        fileStream1.Close();
                    }
                }
                var success = MessageBox.Show("Successfully Encrypted CSV Files!" + Environment.NewLine +
                    "Select OK to open the folder, else select cancel!",
                    "Success",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (success == DialogResult.OK)
                {
                    Process.Start(output);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured, please try again or contact someone on the #DARK Network."
                    + Environment.NewLine + "Error: " + e,
                    "Error - Key:3",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}