using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace csssceditor
{
    class ImageRgba8888 : ScImage
    {
        public ImageRgba8888()
        {
        }

        public override string GetImageTypeName()
        {
            return "RGB8888";
        }

        public override void ParseImage(BinaryReader br)
        {
            base.ParseImage(br);
            m_vBitmap = new Bitmap(m_vWidth, m_vHeight, PixelFormat.Format32bppArgb);

            for (int column = 0; column < m_vHeight; column++)
            {
                for (int row = 0; row < m_vWidth; row++)
                {
                    byte r = br.ReadByte();
                    byte g = br.ReadByte();
                    byte b = br.ReadByte();
                    byte a = br.ReadByte();

                    m_vBitmap.SetPixel(row, column, Color.FromArgb((int)((a << 24) | (r << 16) | (g << 8) | b)));
                }
            }
        }

        public override void Print()
        {
            base.Print();
        }

        public override void WriteImage(FileStream input)
        {
            base.WriteImage(input);
            for (int column = 0; column < m_vBitmap.Height; column++)
            {
                for (int row = 0; row < m_vBitmap.Width; row++)
                {
                    input.WriteByte(m_vBitmap.GetPixel(row, column).R);
                    input.WriteByte(m_vBitmap.GetPixel(row, column).G);
                    input.WriteByte(m_vBitmap.GetPixel(row, column).B);
                    input.WriteByte(m_vBitmap.GetPixel(row, column).A);
                }
            }
        }
    }
}
