using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace csssceditor
{
    class Texture : ScObject
    {
        private byte m_vImageType;
        private ScImage m_vImage;
        private short m_vTextureId;
        private Dictionary<byte, Type> m_vScImageTypes;
        private Decoder m_vStorageObject;
        private long m_vOffset;

        public Texture(Decoder scs)
        {
            m_vStorageObject = scs;
            m_vScImageTypes = new Dictionary<byte, Type>();
            m_vScImageTypes.Add(0, typeof(ImageRgba8888));
            m_vScImageTypes.Add(2, typeof(ImageRgba4444));
            m_vScImageTypes.Add(4, typeof(ImageRgb565));
            m_vTextureId = (short)m_vStorageObject.GetTextures().Count();
        }

        public Texture(Texture t)
        {
            m_vImageType = t.GetImageType();
            m_vStorageObject = t.GetStorageObject();
            m_vTextureId = (short)m_vStorageObject.GetTextures().Count();
            m_vScImageTypes = new Dictionary<byte, Type>();
            m_vScImageTypes.Add(0, typeof(ImageRgba8888));
            m_vScImageTypes.Add(2, typeof(ImageRgba4444));
            m_vScImageTypes.Add(4, typeof(ImageRgb565));
            if (m_vScImageTypes.ContainsKey(m_vImageType))
            {
                m_vImage = (ScImage)Activator.CreateInstance(m_vScImageTypes[m_vImageType]);
            }
            else
            {
                m_vImage = new ScImage();
            }
            m_vImage.SetBitmap(new Bitmap(t.GetBitmap()));
            m_vOffset = t.GetOffset() > 0 ? -t.GetOffset() : t.GetOffset();
        }

        public override Bitmap GetBitmap()
        {
            return m_vImage.GetBitmap();
        }

        public override short GetId()
        {
            return m_vTextureId;
        }

        public override int GetDataType()
        {
            return 2;
        }

        public override string GetDataTypeName()
        {
            return "Textures";
        }

        public ScImage GetImage()
        {
            return m_vImage;
        }

        public byte GetImageType()
        {
            return m_vImageType;
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TextureId: " + m_vTextureId);
            sb.AppendLine("ImageType: " + m_vImageType.ToString());
            sb.AppendLine("ImageFormat: " + m_vImage.GetImageTypeName());
            sb.AppendLine("Width: " + m_vImage.GetWidth());
            sb.AppendLine("Height: " + m_vImage.GetHeight());
            return sb.ToString();
        }

        public long GetOffset()
        {
            return m_vOffset;
        }

        public Decoder GetStorageObject()
        {
            return m_vStorageObject;
        }

        public short GetTextureId()
        {
            return m_vTextureId;
        }

        public override bool IsImage()
        {
            return true;
        }

        public override void ParseData(BinaryReader br)
        {
            m_vImageType = br.ReadByte();

            if (m_vScImageTypes.ContainsKey(m_vImageType))
            {
                m_vImage = (ScImage)Activator.CreateInstance(m_vScImageTypes[m_vImageType]);
            }
            else
            {
                m_vImage = new ScImage();
            }
            m_vImage.ParseImage(br);
        }

        public override Bitmap Render(RenderingOptions options)
        {
            return GetBitmap();
        }

        public override void Save(FileStream input)
        {
            if(m_vOffset < 0)//new
            {
                //Get info
                input.Seek(Math.Abs(m_vOffset), SeekOrigin.Begin);
                byte[] dataType = new byte[1];
                input.Read(dataType, 0, 1);
                byte[] dataLength = new byte[4];
                input.Read(dataLength, 0, 4);
                //Then write
                input.Seek(m_vStorageObject.GetEofOffset(), SeekOrigin.Begin);
                input.Write(dataType, 0, 1);
                input.Write(dataLength, 0, 4);
                input.WriteByte(m_vImageType);
                m_vImage.WriteImage(input);
                m_vOffset = m_vStorageObject.GetEofOffset();
                m_vStorageObject.SetEofOffset(input.Position);
                input.Write(new byte[]{0,0,0,0,0},0,5);
            }
            else//existing
            {
                input.Seek(m_vOffset + 5, SeekOrigin.Current);
                input.WriteByte(m_vImageType);
                m_vImage.WriteImage(input);
            }
        }

        public void SetOffset(long position)
        {
            m_vOffset = position;
        }
    }
}
