using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace CSSCE
{
    class ScImage
    {
        protected ushort m_vWidth;
        protected ushort m_vHeight;
        protected Bitmap m_vBitmap; 

        public ScImage()
        {
        }

        public ScImage(ScImage im)
        {
            m_vWidth = im.GetWidth();
            m_vHeight = im.GetHeight();
            m_vBitmap = new Bitmap(im.GetBitmap());
        }

        public Bitmap GetBitmap()
        {
            return m_vBitmap;
        }

        public virtual string GetImageTypeName()
        {
            return "unknown";
        }

        public ushort GetHeight()
        {
            return m_vHeight;
        }


        public ushort GetWidth()
        {
            return m_vWidth;
        }

        public virtual void ParseImage(BinaryReader br)
        {
            m_vWidth = br.ReadUInt16();
            m_vHeight = br.ReadUInt16();
        }

        public virtual void Print()
        {
            Debug.WriteLine("Width: " + m_vWidth.ToString());
            Debug.WriteLine("Height: " + m_vHeight.ToString());
        }

        public void SetBitmap(Bitmap b)
        {
            m_vBitmap = b;
            m_vWidth = (ushort)b.Width;
            m_vHeight = (ushort)b.Height;
        }

        public virtual void WriteImage(FileStream input)
        {
            input.Write(BitConverter.GetBytes(m_vWidth), 0, 2);
            input.Write(BitConverter.GetBytes(m_vHeight), 0, 2);
        }
    }
}
