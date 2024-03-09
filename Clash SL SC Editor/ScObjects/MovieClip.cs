using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace csssceditor
{
    class MovieClip : ScObject
    {
        private short m_vDataType;
        private short m_vClipId;
        private short m_vFrameCount;
        private List<ScObject> m_vShapes;
        private Decoder m_vStorageObject;
        private long m_vOffset;

        public MovieClip(Decoder scs, short dataType)
        {
            m_vStorageObject = scs;
            m_vDataType = dataType;
            m_vShapes = new List<ScObject>();
        }

        public MovieClip(MovieClip mv)
        {
            m_vStorageObject = mv.GetStorageObject();
            m_vDataType = mv.GetMovieClipDataType();
            m_vShapes = new List<ScObject>();

            this.SetOffset(-Math.Abs(mv.GetOffset()));
            
            //Duplicate MovieClip
            using (FileStream input = new FileStream(m_vStorageObject.GetFileName(), FileMode.Open))
            {
                input.Seek(Math.Abs(mv.GetOffset()) + 5, SeekOrigin.Begin);
                using (var br = new BinaryReader(input))
                {
                    this.ParseData(br);
                }
            }

            //Set new clip id
            short maxMovieClipId = this.GetId();
            foreach(MovieClip clip in m_vStorageObject.GetMovieClips())
            {
                if (clip.GetId() > maxMovieClipId)
                    maxMovieClipId = clip.GetId();
            }
            maxMovieClipId++;
            this.SetId(maxMovieClipId);

            //Get max shape id
            short maxShapeId = 20000;//avoid collision with other objects in MovieClips
            foreach(Shape shape in m_vStorageObject.GetShapes())
            {
                if (shape.GetId() > maxShapeId)
                    maxShapeId = shape.GetId();
            }
            maxShapeId++;

            //Duplicate shapes associated to clip
            List<ScObject> newShapes = new List<ScObject>();
            foreach(Shape s in m_vShapes)
            {
                Shape newShape = new Shape(s);
                newShape.SetId(maxShapeId);
                maxShapeId++;
                newShapes.Add(newShape);
                m_vStorageObject.AddShape(newShape);//Add to global shapelist
                m_vStorageObject.AddChange(newShape);
            }
            this.m_vShapes = newShapes;
        }

        public override List<ScObject> GetChildren()
        {
            return m_vShapes;
        }

        public override int GetDataType()
        {
            return 1;
        }

        public override string GetDataTypeName()
        {
            return "MovieClips";
        }

        public override short GetId()
        {
            return m_vClipId;
        }

        public short GetMovieClipDataType()
        {
            return m_vDataType;
        }

        public long GetOffset()
        {
            return m_vOffset;
        }

        public List<ScObject> GetShapes()
        {
            return m_vShapes;
        }

        public Decoder GetStorageObject()
        {
            return m_vStorageObject;
        }

        public override void ParseData(BinaryReader br)
        {
            
            Debug.WriteLine("MovieClip data type: " + m_vDataType);
            /*StringBuilder hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2}", b);
            Debug.WriteLine(hex.ToString());*/

            m_vClipId = br.ReadInt16();
            br.ReadByte();//a1 + 34
            m_vFrameCount = br.ReadInt16();
            if(m_vDataType == 14)
            {
                    
            }
            else
            {
                int cnt1 = br.ReadInt32();
                short[] sa1 = new short[cnt1 * 3];
                for(int i=0;i<cnt1;i++)
                {
                    sa1[3 * i] = br.ReadInt16();
                    sa1[3 * i + 1] = br.ReadInt16();
                    sa1[3 * i + 2] = br.ReadInt16();
                }
            }
            int cnt2 = br.ReadInt16();
            short[] sa2 = new short[cnt2];//a1 + 8
            for (int i = 0; i < cnt2; i++)
            {
                sa2[i] = br.ReadInt16();
                int index = m_vStorageObject.GetShapes().FindIndex(shape => shape.GetId() == sa2[i]);
                if (index != -1)
                    m_vShapes.Add(m_vStorageObject.GetShapes()[index]);
            }
            if (m_vDataType == 12)
                br.ReadBytes(cnt2);//a1 + 12

            //read ascii
            for(int i=0;i<cnt2;i++)
            {
                byte stringLength = br.ReadByte();
                if (stringLength < 255)
                    br.ReadBytes(stringLength);
            }
                
            byte v26;
            while(true)
            {
                while(true)
                {
                    while(true)
                    {
                        v26 = br.ReadByte();
                        br.ReadInt32();
                        if (v26 != 5)
                            break;
                    }
                    if (v26 == 11)
                    {
                        short frameId = br.ReadInt16();
                        byte frameNameLength = br.ReadByte();
                        if (frameNameLength < 255)
                            br.ReadBytes(frameNameLength);
                    }
                    else
                        break;
                }
                if (v26 == 0)
                    break;
                Debug.WriteLine("Unknown tag " + v26.ToString());
            }
        }

        public override void Save(FileStream input)
        {
            if (m_vOffset < 0)//new
            {
                using (FileStream readInput = new FileStream(m_vStorageObject.GetFileName(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //Positionnement des curseurs
                    readInput.Seek(Math.Abs(m_vOffset), SeekOrigin.Begin);
                    input.Seek(m_vStorageObject.GetEofOffset(), SeekOrigin.Begin);

                    //type and length
                    byte[] dataType = new byte[1];
                    readInput.Read(dataType, 0, 1);
                    byte[] dataLength = new byte[4];
                    readInput.Read(dataLength, 0, 4);
                    input.Write(dataType, 0, 1);
                    input.Write(dataLength, 0, 4);

                    //movieclip
                    readInput.Seek(2, SeekOrigin.Current);
                    input.Write(BitConverter.GetBytes(m_vClipId), 0, 2);

                    input.WriteByte((byte)readInput.ReadByte());
                    readInput.Seek(2, SeekOrigin.Current);
                    input.Write(BitConverter.GetBytes(m_vFrameCount), 0, 2);

                    if (m_vDataType == 14)
                    {

                    }
                    else
                    {
                        //int cnt1 = br.ReadInt32();
                        byte[] cnt1 = new byte[4];
                        readInput.Read(cnt1, 0, 4);
                        input.Write(cnt1, 0, 4);

                        for (int i = 0; i < BitConverter.ToInt32(cnt1, 0); i++)
                        {
                            byte[] uk1 = new byte[2];
                            readInput.Read(uk1, 0, 2);
                            input.Write(uk1, 0, 2);

                            byte[] uk2 = new byte[2];
                            readInput.Read(uk2, 0, 2);
                            input.Write(uk2, 0, 2);

                            byte[] uk3 = new byte[2];
                            readInput.Read(uk3, 0, 2);
                            input.Write(uk3, 0, 2);
                        }
                    }
                    //int cnt2 = br.ReadInt16();
                    byte[] cnt2 = new byte[2];
                    readInput.Read(cnt2, 0, 2);
                    input.Write(cnt2, 0, 2);

                    int cptShape = 0;
                    for (int i = 0; i < BitConverter.ToInt16(cnt2,0); i++)
                    {
                        byte[] id = new byte[2];
                        readInput.Read(id, 0, 2);

                        int index = m_vStorageObject.GetShapes().FindIndex(shape => shape.GetId() == BitConverter.ToInt16(id,0));
                        if (index != -1)
                        {
                            input.Write(BitConverter.GetBytes(m_vShapes[cptShape].GetId()), 0, 2);
                            cptShape++;
                        }
                        else
                        {
                            input.Write(id, 0, 2);
                        }
                    }
                    if (m_vDataType == 12)
                    {
                        for (int i = 0; i < BitConverter.ToInt16(cnt2, 0); i++)
                        {
                            input.WriteByte((byte)readInput.ReadByte());
                        }
                    }
                        
                    //read ascii
                    for (int i = 0; i < BitConverter.ToInt16(cnt2, 0); i++)
                    {
                        byte stringLength = (byte)readInput.ReadByte();
                        input.WriteByte(stringLength);
                        if (stringLength < 255)
                        {
                            for(int j=0;j<stringLength;j++)
                                input.WriteByte((byte)readInput.ReadByte());
                        }
                    }

                    byte v26;
                    while (true)
                    {
                        while (true)
                        {
                            while (true)
                            {
                                v26 = (byte)readInput.ReadByte();
                                input.WriteByte(v26);

                                //br.ReadInt32();
                                byte[] uk4 = new byte[4];
                                readInput.Read(uk4, 0, 4);
                                input.Write(uk4, 0, 4);

                                if (v26 != 5)
                                    break;
                            }
                            if (v26 == 11)
                            {
                                //short frameId = br.ReadInt16();
                                byte[] frameId = new byte[2];
                                readInput.Read(frameId, 0, 2);
                                input.Write(frameId, 0, 2);

                                byte frameNameLength = (byte)readInput.ReadByte();
                                input.WriteByte(frameNameLength);

                                if (frameNameLength < 255)
                                {
                                    for(int i=0;i<frameNameLength;i++)
                                    {
                                        input.WriteByte((byte)readInput.ReadByte());
                                    }
                                }
                            }
                            else
                                break;
                        }
                        if (v26 == 0)
                            break;
                        Debug.WriteLine("Unknown tag " + v26.ToString());
                    }        
                }
                m_vOffset = m_vStorageObject.GetEofOffset();
                m_vStorageObject.SetEofOffset(input.Position);
                input.Write(new byte[] { 0, 0, 0, 0, 0 }, 0, 5);
            }
        }

        public void SetId(short id)
        {
            m_vClipId = id;
        }

        public void SetOffset(long position)
        {
            m_vOffset = position;
        }
    }
}
