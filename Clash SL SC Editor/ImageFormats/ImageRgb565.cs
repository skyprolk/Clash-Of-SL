using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace CSSCE
{
    class ImageRgb565 : ScImage
    {
        public ImageRgb565()
        {
        }

        public override string GetImageTypeName()
        {
            return "RGB565";
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

                    int red = (int)((color >> 11) & 0x1F) << 3;
                    int green = (int)((color >> 5) & 0x3F) << 2;
                    int blue = (int)(color & 0X1F) << 3;

                    m_vBitmap.SetPixel(row, column, Color.FromArgb(red, green, blue));
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

                    ushort color = (ushort)(((((red >> 3)) & 0x1F) << 11) | ((((green >> 2)) & 0x3F) << 5) | ((blue >> 3) & 0x1F));

                    input.Write(BitConverter.GetBytes(color), 0, 2);
                }
            }
        }
    }
}
