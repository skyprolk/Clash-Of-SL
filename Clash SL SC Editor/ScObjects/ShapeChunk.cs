using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using MathNet.Numerics.LinearAlgebra;

namespace CSSCE
{
    class ShapeChunk : ScObject
    {
        private short m_vChunkId;
        private short m_vShapeId;
        private byte m_vTextureId;
        private byte m_vChunkType;
        private List<PointF> m_vPointsXY;
        private List<PointF> m_vPointsUV;
        private Decoder m_vStorageObject;
        private long m_vOffset;

        public ShapeChunk(Decoder scs)
        {
            m_vStorageObject = scs;
            m_vPointsXY = new List<PointF>();
            m_vPointsUV = new List<PointF>();
        }

        public byte GetChunkType()
        {
            return m_vChunkType;
        }

        public override int GetDataType()
        {
            return 99;
        }

        public override string GetDataTypeName()
        {
            return "ShapeChunks";
        }

        public override string GetName()
        {
            return "Chunk " + GetId().ToString();
        }

        public List<PointF> GetPointsUV()
        {
            return m_vPointsUV;
        }

        public List<PointF> GetPointsXY()
        {
            return m_vPointsXY;
        }

        public override short GetId()
        {
            return m_vChunkId;
        }

        public override string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ChunkId: " + m_vChunkId);
            sb.AppendLine("ShapeId (ref): " + m_vShapeId);
            sb.AppendLine("TextureId (ref): " + m_vTextureId);
            return sb.ToString();
        }

        public long GetOffset()
        {
            return m_vOffset;
        }

        public short GetShapeId()
        {
            return m_vShapeId;
        }

        public byte GetTextureId()
        {
            return m_vTextureId;
        }

        public override bool IsImage()
        {
            return true;
        }

        public override void ParseData(BinaryReader br)
        {
            Debug.WriteLine("Parsing chunk data from shape " + m_vShapeId);
            m_vOffset = br.BaseStream.Position;
            m_vTextureId = br.ReadByte();//00
            byte shapePointCount = br.ReadByte();//04
            var texture = (Texture)m_vStorageObject.GetTextures()[m_vTextureId];

            for (int i = 0; i < shapePointCount; i++)
            {
                float x = (float)(br.ReadInt32() * 0.05);//* 0.05);
                float y = (float)(br.ReadInt32() * 0.05);//* 0.05);
                m_vPointsXY.Add(new PointF(x,y));
                Debug.WriteLine("x: " + x + ", y: " + y);
            }
            if (m_vChunkType == 22)
            {
                for (int i = 0; i < shapePointCount; i++)
                {
                    float u = (float)(br.ReadUInt16() / 65535.0)*texture.GetImage().GetWidth();
                    float v = (float)(br.ReadUInt16() / 65535.0)*texture.GetImage().GetHeight();
                    m_vPointsUV.Add(new PointF(u, v));
                    Debug.WriteLine("u: " + u + ", v: " + v);
                }
            }
            else
            {
                for (int i = 0; i < shapePointCount; i++)
                {
                    ushort u = br.ReadUInt16();// image.Width);
                    ushort v = br.ReadUInt16();// image.Height);//(short) (65535 * br.ReadInt16() / image.Height);
                    m_vPointsUV.Add(new Point(u, v));
                    Debug.WriteLine("u: " + u + ", v: " + v);
                }
            }
        }

        public override Bitmap Render(RenderingOptions options)
        {
            Debug.WriteLine("Rendering chunk from shape " + m_vShapeId);
            Bitmap result = null;
            var texture = (Texture)m_vStorageObject.GetTextures()[m_vTextureId];
            if (texture != null)
            {
                Bitmap bitmap = texture.GetBitmap();

                Debug.WriteLine("Rendering polygon image of " + GetPointsUV().Count.ToString() + " points");
                foreach(PointF uv in GetPointsUV())
                {
                    Debug.WriteLine("u: " + uv.X + ", v: " + uv.Y);
                }
          
                GraphicsPath gpuv = new GraphicsPath();
                gpuv.AddPolygon(GetPointsUV().ToArray());

                int gpuvWidth = Rectangle.Round(gpuv.GetBounds()).Width;
                gpuvWidth = gpuvWidth > 0 ? gpuvWidth : 1;
                Debug.WriteLine("gpuvWidth: " + gpuvWidth);
                int gpuvHeight = Rectangle.Round(gpuv.GetBounds()).Height;
                gpuvHeight = gpuvHeight > 0 ? gpuvHeight : 1;
                Debug.WriteLine("gpuvHeight: " + gpuvHeight);
                var shapeChunk = new Bitmap(gpuvWidth, gpuvHeight);

                int chunkX = Rectangle.Round(gpuv.GetBounds()).X;
                int chunkY = Rectangle.Round(gpuv.GetBounds()).Y;
 
                //bufferizing shape
                using (Graphics g = Graphics.FromImage(shapeChunk))
                {
                    //On conserve la qualité de l'image intacte
                    gpuv.Transform(new Matrix(1, 0, 0, 1, -chunkX, -chunkY));
                    g.SetClip(gpuv);
                    g.DrawImage(bitmap, -chunkX, -chunkY);
                    if (options.ViewPolygons)
                        g.DrawPath(new Pen(Color.DarkGray, 2), gpuv); 
                }

                result = shapeChunk;
            }
            return result;
        }

        public void Replace(Bitmap chunk)
        {
            var texture = (Texture)m_vStorageObject.GetTextures()[m_vTextureId];
            if (texture != null)
            {
                Bitmap bitmap = texture.GetBitmap();

                GraphicsPath gpuv = new GraphicsPath();
                gpuv.AddPolygon(GetPointsUV().ToArray());
                int x = Rectangle.Round(gpuv.GetBounds()).X;
                int y = Rectangle.Round(gpuv.GetBounds()).Y;
                int width = Rectangle.Round(gpuv.GetBounds()).Width;
                int height = Rectangle.Round(gpuv.GetBounds()).Height;

                GraphicsPath gpChunk = new GraphicsPath();
                gpChunk.AddRectangle(new Rectangle(0, 0, width, height));

                using (Graphics g = Graphics.FromImage(bitmap))
                {   
                    gpChunk.Transform(new Matrix(1, 0, 0, 1, x, y));
                    g.SetClip(gpuv);
                    g.Clear(Color.Transparent);
                    g.DrawImage(chunk, x, y);
                }
            }
        }

        public override void Save(FileStream input)
        {
            if(m_vOffset < 0)
            {
                m_vOffset = input.Position;
                input.WriteByte(m_vTextureId);
                input.WriteByte((byte)m_vPointsUV.Count);
                foreach (var pointXY in m_vPointsXY)
                {
                    input.Write(BitConverter.GetBytes((int)(pointXY.X * 20)), 0, 4);
                    input.Write(BitConverter.GetBytes((int)(pointXY.Y * 20)), 0, 4);
                }

                var texture = (Texture)m_vStorageObject.GetTextures()[m_vTextureId];

                if (m_vChunkType == 22)
                {
                    foreach (var pointUV in m_vPointsUV)
                    {
                        input.Write(BitConverter.GetBytes((ushort)((pointUV.X / texture.GetImage().GetWidth()) * 65535)), 0, 2);
                        input.Write(BitConverter.GetBytes((ushort)((pointUV.Y / texture.GetImage().GetHeight()) * 65535)), 0, 2);
                    }
                }
                else
                {
                    foreach (var pointUV in m_vPointsUV)
                    {
                        input.Write(BitConverter.GetBytes((ushort)(pointUV.X)), 0, 2);
                        input.Write(BitConverter.GetBytes((ushort)(pointUV.Y)), 0, 2);
                    }
                }  
            }
            else
            {
                input.Seek(m_vOffset, SeekOrigin.Begin);
                input.WriteByte(m_vTextureId);
            }
        }
     
        public void SetChunkId(short id)
        {
            m_vChunkId = id;
        }

        public void SetChunkType(byte type)
        {
            m_vChunkType = type;
        }

        public void SetOffset(long offset)
        {
            m_vOffset = offset;
        }

        public void SetShapeId(short id)
        {
            m_vShapeId = id;
        }

        public void SetTextureId(byte id)
        {
            m_vTextureId = id;
        }
    }
}
