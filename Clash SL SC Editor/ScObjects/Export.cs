using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
//using System.Windows.Shapes;
//using System.Windows.Media;

namespace csssceditor
{
    class Export : ScObject
    {
        private short m_vExportId;
        private string m_vExportName;
        private MovieClip m_vDataObject;
        private Decoder m_vStorageObject;

        public Export(Decoder scs)
        {
            m_vStorageObject = scs;
        }

        public override List<ScObject> GetChildren()
        {
            return m_vDataObject.GetChildren();
        }

        public ScObject GetDataObject()
        {
            return m_vDataObject;
        }

        public override int GetDataType()
        {
            return 7;
        }

        public override string GetDataTypeName()
        {
            return "Exports";
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ExportId: " + m_vExportId);
            return sb.ToString();
        }

        public override string GetName()
        {
            return m_vExportName;
        }

        public override short GetId()
        {
            return m_vExportId;
        }

        public override void Save(FileStream input)
        {
            input.Seek(0,SeekOrigin.Begin);
            byte[] file = new byte[input.Length];
            input.Read(file, 0, file.Length);

            int cursor = (int)m_vStorageObject.GetStartExportsOffset();

            input.Seek(m_vStorageObject.GetStartExportsOffset(), SeekOrigin.Begin);

            ushort exportCount = BitConverter.ToUInt16(file, cursor);
            input.Write(BitConverter.GetBytes((ushort)(exportCount + 1)),0,2);
            cursor += 2;

            input.Seek(exportCount * 2, SeekOrigin.Current);
            cursor += exportCount * 2;
            input.Write(BitConverter.GetBytes(m_vExportId), 0, 2);

            for (int i = 0; i < exportCount;i++)
            {
                byte nameLength = file[cursor];
                cursor++;
                byte[] exportName = new byte[nameLength];
                Array.Copy(file,cursor,exportName,0,nameLength);
                input.WriteByte(nameLength);
                input.Write(exportName,0,nameLength);
                cursor += nameLength;
            }

            input.WriteByte((byte)m_vExportName.Length);
            input.Write(Encoding.UTF8.GetBytes(m_vExportName),0,(byte)m_vExportName.Length);

            while(cursor<file.Length)
            {
                input.WriteByte(file[cursor]);
                cursor++;
            }

            //refresh all offsets
            foreach(Texture t in m_vStorageObject.GetTextures())
            {
                long offset = t.GetOffset();
                if(offset > 0)
                    offset += 2 + 1 + m_vExportName.Length;
                else
                    offset = offset - 2 - 1 - m_vExportName.Length;
                t.SetOffset(offset);
            }
            foreach (Shape s in m_vStorageObject.GetShapes())
            {
                long offset = s.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + m_vExportName.Length;
                else
                    offset = offset - 2 - 1 - m_vExportName.Length;
                s.SetOffset(offset);
                foreach (ShapeChunk sc in s.GetChunks())
                {
                    long chunkOffset = sc.GetOffset();
                    if (chunkOffset > 0)
                        chunkOffset += 2 + 1 + m_vExportName.Length;
                    else
                        chunkOffset = chunkOffset - 2 - 1 - m_vExportName.Length;
                    sc.SetOffset(chunkOffset);
                }
            }
            foreach (MovieClip mc in m_vStorageObject.GetMovieClips())
            {
                long offset = mc.GetOffset();
                if (offset > 0)
                    offset += 2 + 1 + m_vExportName.Length;
                else
                    offset = offset - 2 - 1 - m_vExportName.Length;
                mc.SetOffset(offset);
            }
            m_vStorageObject.SetEofOffset(m_vStorageObject.GetEofOffset() + 2 + 1 + m_vExportName.Length);
            //ne pas oublier eofoffset
        }

        public void SetDataObject(MovieClip sd)
        {
            m_vDataObject = sd;
        }

        public void SetId(short id)
        {
            m_vExportId = id;
        }

        public void SetExportName(string name)
        {
            m_vExportName = name;
        }
    }
}
