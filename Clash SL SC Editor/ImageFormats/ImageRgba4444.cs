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
    class ImageRgba4444 : ScImage
    {
        public ImageRgba4444()
        {
        }

        public override string GetImageTypeName()
        {
            return "RGB4444";
        }

        public override void ParseImage(BinaryReader br)
        {
            base.ParseImage(br);
            m_vBitmap = new Bitmap(m_vWidth, m_vHeight, PixelFormat.Format32bppArgb);

            for (int column = 0; column < m_vHeight; column++)
            {
                for (int row = 0; row < m_vWidth; row++)
                {
                    ushort color = br.ReadUInt16();

                    int red = (int)((color >> 12) & 0xF) << 4;
                    int green = (int)((color >> 8) & 0xF) << 4;
                    int blue = (int)((color >> 4) & 0xF) << 4;
                    int alpha = (int)(color & 0xF) << 4;

                    m_vBitmap.SetPixel(row, column, Color.FromArgb(alpha, red, green, blue));
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
                    byte red = m_vBitmap.GetPixel(row, column).R;
                    byte green = m_vBitmap.GetPixel(row, column).G;
                    byte blue = m_vBitmap.GetPixel(row, column).B;
                    byte alpha = m_vBitmap.GetPixel(row, column).A;

                    ushort color = (ushort)(((((red >> 4)) & 0xF) << 12) | ((((green >> 4)) & 0xF) << 8) | ((((blue >> 4)) & 0xF) << 4) | ((alpha >> 4) & 0xF));

                    input.Write(BitConverter.GetBytes(color), 0, 2);
                }
            }
        }
    }
}
