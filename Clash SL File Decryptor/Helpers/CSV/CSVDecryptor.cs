using SevenZip;
using SevenZip.Compression.LZMA;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CSFD.Core.Helpers
{
    class CSVDecryptor
    {
        public static void Decrypt()
        {
            Decoder decoder = new Decoder();
            string[] files = Directory.GetFiles(@"In\Encrypted - CSV", "*.csv");
            string output = @"Out\Decrypted - CSV";

            
            int filesAmount = 0;
            foreach (string file in files)
            {
                filesAmount++;
            }
            if (filesAmount == 0)
            {
                MessageBox.Show("No CSV files were found in the folder \"In\\Encrypted - CSV.\"" 
                    + Environment.NewLine + "Place encrypted files in the folder and try again.",
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
                            byte[] numArray = new byte[5];
                            fileStream1.Read(numArray, 0, 5);
                            byte[] buffer = new byte[4];
                            fileStream1.Read(buffer, 0, 4);
                            int int32 = BitConverter.ToInt32(buffer, 0);
                            decoder.SetDecoderProperties(numArray);
                            decoder.Code((Stream)fileStream1, (Stream)fileStream2, fileStream1.Length, (long)int32, (ICodeProgress)null);
                            fileStream2.Flush();
                            fileStream2.Close();
                        }
                        fileStream1.Close();
                    }
                }
                var success = MessageBox.Show("Successfully Decrypted CSV Files!" + Environment.NewLine +
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
                    "Error - Key:1",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}