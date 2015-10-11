using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
namespace Zgke.MyImage
{
    /// <summary> 
    /// GIF操作类 
    /// zgke@sina.com 
    /// QQ:116149 
    /// </summary> 
    public class ImageGif
    {    
        private Header m_Header;
        private LogicalScreen m_LogicalScreen;
        private ColorTable m_GlobalColorTable;
        private IList<ExtensionIntroducer> m_ExtensionIntroducer = new List<ExtensionIntroducer>();
        private ApplicationExtension m_ApplicationExtension;
        private IList<GraphicControlExtension> m_GraphicControlExtension = new List<GraphicControlExtension>();
        /// <summary> 
        /// 当前位置 
        /// </summary> 
        private int m_Index = 0;
        private bool m_Open = false;
        /// <summary> 
        /// 是否正常打开 
        /// </summary> 
        public bool OpenOK { get { return m_Open; } }
        /// <summary> 
        /// 图形数量 
        /// </summary> 
        public int ImageCount { get { return m_GraphicControlExtension.Count; } }
        /// <summary> 
        /// 获取GIF图形 
        /// </summary> 
        public Image Image
        {
            get 
            {
                if (m_GraphicControlExtension.Count == 0) return null;
                MemoryStream _MemoryImage = new MemoryStream();
                _MemoryImage.Position = 0;
                byte[] _Temp = m_Header.GetByte();
                _MemoryImage.Write(_Temp, 0, _Temp.Length);
                _Temp = m_LogicalScreen.GetByte();
                _MemoryImage.Write(_Temp, 0, _Temp.Length);
                _Temp = m_GlobalColorTable.GetByte();
                _MemoryImage.Write(_Temp, 0, _Temp.Length);
                _Temp = m_ApplicationExtension.GetByte();
                _MemoryImage.Write(_Temp, 0, _Temp.Length);
                for (int i = 0; i != m_GraphicControlExtension.Count; i++)  //保存图形 
                {
                    _Temp = m_GraphicControlExtension[i].GetByte();
                    _MemoryImage.Write(_Temp, 0, _Temp.Length);
                }
                for (int i = 0; i != m_ExtensionIntroducer.Count; i++)  //保存描述 
                {
                    _Temp = m_ExtensionIntroducer[i].GetByte();
                    _MemoryImage.Write(_Temp, 0, _Temp.Length);
                }
                _MemoryImage.WriteByte(0x3B);
                return Image.FromStream(_MemoryImage);
            }            
        }
        /// <summary> 
        /// 由文件打开 
        /// </summary> 
        /// <param name="p_GifFileName"></param> 
        public ImageGif(string p_GifFileName)
        {
            System.IO.FileStream _FileStream = new FileStream(p_GifFileName, FileMode.Open);
            byte[] _GifByte = new byte[_FileStream.Length];
            _FileStream.Read(_GifByte, 0, _GifByte.Length);
            _FileStream.Close();
            m_Header = new Header(_GifByte, ref m_Index, ref m_Open);
            if (m_Open == false) return;
            m_LogicalScreen = new LogicalScreen(_GifByte, ref m_Index, ref m_Open);
            if (m_Open == false) return;
            m_GlobalColorTable = new ColorTable(m_LogicalScreen.GlobalPal, m_LogicalScreen.GlobalColorSize, _GifByte, ref m_Index, ref m_Open);
            if (m_Open == false) return;
            //固定位置信息读取完成 
            m_Open = false;
            while (true)   //不知道有多少个模块一直循环吧 
            { 
                #region 判断位置 
                switch (_GifByte[m_Index])
                {
                    case 0x21:
                        m_Index += 2;   //坐标移动到类别后面  取类类别用-1方式 少一行代码 
                        switch (_GifByte[m_Index - 1])
                        {
                            case 0xFE:
                                m_ExtensionIntroducer.Add(new ExtensionIntroducer(_GifByte, ref m_Index, ref m_Open));
                                if (m_Open == false) return;   //获取失败每必要继续了直接返回 
                                break;
                            case 0xFF:
                                m_ApplicationExtension = new ApplicationExtension(_GifByte, ref m_Index, ref m_Open);
                                if (m_Open == false) return;
                                break;
                            case 0xF9:         //图形在这个区 LZW数据 
                                m_GraphicControlExtension.Add(new GraphicControlExtension(_GifByte, ref m_Index, ref m_Open));
                                if (m_Open == false) return;
                                break;
                            default:         //找不到类别 直接结束  
                                //System.Windows.Forms.MessageBox.Show(_GifByte[m_Index - 1].ToString("X02"));
                                return;
                        }
                        break;
                    case 0x3B:
                        m_Open = true;
                        return;
                    default:
                        m_Open = false;
                       // System.Windows.Forms.MessageBox.Show(_GifByte[m_Index].ToString("X02"), m_Index.ToString());
                        return;
                } 
                #endregion 
            }
        }
        /// <summary> 
        /// 建立新的图形 
        /// </summary> 
        /// <param name="p_Width"></param> 
        /// <param name="p_Height"></param> 
        public ImageGif(ushort p_Width, ushort p_Height)
        {
            m_Header = new Header();
            m_LogicalScreen = new LogicalScreen(p_Width, p_Height);
            m_ApplicationExtension = new ApplicationExtension();
            m_ExtensionIntroducer.Add(new ExtensionIntroducer("http://blog.csdn.net/zgke"));  //这东西可以很多个 描述就是描述 没发现什么作用
            
        } 
        #region 添加图形 删除 插入 
        /// <summary> 
        /// 添加一个图形   
        /// </summary> 
        /// <param name="Image">图形</param> 
        /// <param name="p_DelayTime">显示时间</param> 
        /// <param name="p_UserColorTable">是否使用全局色彩表 true使用 false 不使用</param> 
        private GraphicControlExtension GetGraphicsControl(Image p_Image, ushort p_DelayTime, bool p_UserColorTable, DisposalMethod p_Disposal)
        {
            
            MemoryStream _MemOry = new MemoryStream();
            //Image _Image = ToImageGif((Bitmap)p_Image, Color.White);   //透明的方法可以加到这里 
            p_Image.Save(_MemOry, ImageFormat.Gif);        //保存成GIF图形 
            int _Index = 783;   //开始位置781 0x21 0xF9 已经计算 
            bool _Open = false;
            GraphicControlExtension _Graphics = new GraphicControlExtension(_MemOry.GetBuffer(), ref _Index, ref _Open);
            _Graphics.DelayTime = p_DelayTime;
            _Graphics.TransparentColorFlag = true;
            _Graphics.TransparentColorIndex = 16;
            ColorTable _Table = new ColorTable();
            Image _AddImage = Image.FromStream(_MemOry);  //获取压缩后的图形 
            for (int i = 0; i != _AddImage.Palette.Entries.Length; i++)      //获取图形颜色表 
            {
                _Table.ColorTableList.Add(_AddImage.Palette.Entries[i]);
            }
            if (m_GlobalColorTable == null) m_GlobalColorTable = _Table;     //不管怎么样都要全局色彩表 
            if (p_UserColorTable == false)
            {
                _Graphics.ColorTable = _Table;      //根据需要设置全局颜色表               
                _Graphics.UserColorTable = !p_UserColorTable;
            }
            _Graphics.Disposal = p_Disposal;
            return _Graphics;
        }
        /// <summary> 
        /// 添加一个图形 图形可以大于虚拟屏幕 就是显示不出来而已........ 
        /// </summary> 
        /// <param name="Image">图形</param> 
        /// <param name="p_DelayTime">显示时间</param> 
        /// <param name="p_UserColorTable">是否使用全局色彩表 true使用 false 不使用</param> 
        public void AddImage(Image p_Image, ushort p_DelayTime, bool p_UserColorTable, DisposalMethod p_Disposal)
        {
            GraphicControlExtension _Info = GetGraphicsControl(p_Image, p_DelayTime, p_UserColorTable, p_Disposal);
            if (_Info != null) m_GraphicControlExtension.Add(_Info);
        }
        /// <summary> 
        /// 删除一个图形 
        /// </summary> 
        /// <param name="p_Index"></param> 
        public void RemoveImage(int p_Index)
        {
            if (p_Index > m_GraphicControlExtension.Count - 1) return;
            m_GraphicControlExtension.RemoveAt(p_Index);
        }
        /// <summary> 
        /// 插入一个图形 
        /// </summary> 
        /// <param name="Image">图形</param> 
        /// <param name="p_DelayTime">显示时间</param> 
        /// <param name="p_UserColorTable">是否使用全局色彩表 true使用 false 不使用</param> 
        public void InsertImage(Image p_Image, ushort p_DelayTime, bool p_UserColorTable, int p_Index, DisposalMethod p_Disposal)
        {
            if (p_Index > m_GraphicControlExtension.Count - 1) return;
            GraphicControlExtension _Info = GetGraphicsControl(p_Image, p_DelayTime, p_UserColorTable, p_Disposal);
            if (_Info != null) m_GraphicControlExtension.Insert(p_Index, _Info);
        } 
        #endregion 
        /// <summary> 
        /// 设置一个图形播放的时间 
        /// </summary> 
        /// <param name="p_Index">索引</param> 
        /// <param name="p_DelayTime">播放时间</param> 
        public void SetImageTime(int p_Index, ushort p_DelayTime)
        {
            if (p_Index > m_GraphicControlExtension.Count - 1) return;
            m_GraphicControlExtension[p_Index].DelayTime = p_DelayTime;
        }
        /// <summary> 
        /// 设置一个图形开始位置 
        /// </summary> 
        /// <param name="p_Index">索引</param> 
        /// <param name="p_X">X坐标</param> 
        /// <param name="p_Y">Y坐标</param> 
        public void SetImageLocatch(int p_Index, Point p_Point)
        {
            if (p_Index > m_GraphicControlExtension.Count - 1) return;
            m_GraphicControlExtension[p_Index].Point = p_Point;
        }
        /// <summary> 
        /// 设置处理模式 
        /// </summary> 
        /// <param name="p_Index">索引</param> 
        /// <param name="p_DisposalMethod">处理模式</param> 
        public void SetImageDisposalMethod(int p_Index, DisposalMethod p_DisposalMethod)
        {
            if (p_Index > m_GraphicControlExtension.Count - 1) return;
            m_GraphicControlExtension[p_Index].Disposal = p_DisposalMethod;
        }
        /// <summary> 
        /// 保存文件 
        /// </summary> 
        /// <param name="p_FileName"></param> 
        public void SaveFile(string p_FileName)
        {
            if (m_GraphicControlExtension.Count == 0) return ;
            FileStream _File = new FileStream(p_FileName, FileMode.Create);
            _File.Position = 0;
            byte[] _Temp = m_Header.GetByte();
            _File.Write(_Temp, 0, _Temp.Length);
            _Temp = m_LogicalScreen.GetByte();
            _File.Write(_Temp, 0, _Temp.Length);
            _Temp = m_GlobalColorTable.GetByte();
            _File.Write(_Temp, 0, _Temp.Length);
            _Temp = m_ApplicationExtension.GetByte();
            _File.Write(_Temp, 0, _Temp.Length);
            
            for (int i = 0; i != m_GraphicControlExtension.Count; i++)  //保存图形 
            {
                _Temp = m_GraphicControlExtension[i].GetByte();
                _File.Write(_Temp, 0, _Temp.Length);
            }
            for (int i = 0; i != m_ExtensionIntroducer.Count; i++)  //保存描述 
            {
                _Temp = m_ExtensionIntroducer[i].GetByte();
                _File.Write(_Temp, 0, _Temp.Length);
            }
            _File.WriteByte(0x3B);
            _File.Close();
        } 
        #region 获取图形 
        /// <summary> 
        /// 内部方法 保存一个图形 
        /// </summary> 
        /// <param name="m_Stream">数据流</param> 
        /// <param name="p_Index">图形索引</param> 
        /// <returns>图形</returns> 
        private Stream GetImage(Stream m_Stream, int p_Index)
        {
            if (p_Index < 0 || p_Index > ImageCount) return null;
            m_Stream.Position = 0;
            byte[] _Temp = m_Header.GetByte();
            m_Stream.Write(_Temp, 0, _Temp.Length);
            _Temp = m_LogicalScreen.GetByte();
            m_Stream.Write(_Temp, 0, _Temp.Length);
            _Temp = m_GlobalColorTable.GetByte();
            m_Stream.Write(_Temp, 0, _Temp.Length);
            _Temp = m_ApplicationExtension.GetByte();
            m_Stream.Write(_Temp, 0, _Temp.Length);
            _Temp = m_GraphicControlExtension[p_Index].GetByte();
            m_Stream.Write(_Temp, 0, _Temp.Length);
            for (int i = 0; i != m_ExtensionIntroducer.Count; i++)  //保存描述 
            {
                _Temp = m_ExtensionIntroducer[i].GetByte();
                m_Stream.Write(_Temp, 0, _Temp.Length);
            }
            m_Stream.WriteByte(0x3B);
            return m_Stream;
        }
        /// <summary> 
        /// 根据索引获取图形 
        /// </summary> 
        /// <param name="p_Index">索引</param> 
        /// <returns>图形</returns> 
        public Image GetImage(int p_Index)
        {
            MemoryStream _Memory = new MemoryStream();
            return Image.FromStream(GetImage(_Memory, p_Index));
        }
        /// <summary> 
        /// 根据索引获取图形 
        /// </summary> 
        /// <param name="p_Index">索引</param> 
        /// <returns>图形</returns> 
        public void GetImage(int p_Index, string p_FileName)
        {
            FileStream _File = new FileStream(p_FileName, FileMode.Create);
            GetImage(_File, p_Index);
            _File.Close();
        } 
        #endregion 
        /// <summary> 
        /// 处理模式 
        /// </summary> 
        public enum DisposalMethod
        {
            /// <summary> 
            /// 不使用处置方法 一般的GIF都是这个 
            /// </summary> 
            NoDisposalMethod = 0,
            /// <summary> 
            /// 不处置图形，把图形从当前位置移去 
            /// </summary> 
            NoDisposalImage = 1,
            /// <summary> 
            /// 回复到背景色 
            /// </summary> 
            RestoreBackgroundColor = 2,
            /// <summary> 
            /// 回复到先前状态 
            /// </summary> 
            RestoreFrontState = 3,
            /// <summary> 
            /// 为定义4-7 
            /// </summary> 
            Null
        } 



        #region 类定义 
        /// <summary> 
        /// GIF文件头 
        /// </summary> 
        private class Header
        {
            /// <summary> 
            ///文件头 必须是GIF 
            /// </summary> 
            private byte[] _Signature = new byte[] { 0x47, 0x49, 0x46 };
            /// <summary> 
            /// 版本信息  
            /// </summary> 
            private byte[] _Version = new byte[] { 0x38, 0x39, 0x61 };
            public Header(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                if (p_Byte[0] != _Signature[0] || p_Byte[1] != _Signature[1] || p_Byte[2] != _Signature[2])
                {
                    return;
                }
                _Version[0] = p_Byte[3];
                _Version[1] = p_Byte[4];
                _Version[2] = p_Byte[5];
                p_Index += 6;
                p_Open = true;
            }
            public Header()
            {
            }
            /// <summary> 
            /// 返回版本号  
            /// </summary> 
            public string Version
            {
                get
                { return Encoding.ASCII.GetString(_Version); }
                set
                {
                    if (value == "87a") _Version[1] = 0x37;
                }
            }
            public byte[] GetByte()
            {
                byte[] _Temp = new byte[6];
                _Temp[0] = _Signature[0];
                _Temp[1] = _Signature[1];
                _Temp[2] = _Signature[2];
                _Temp[3] = _Version[0];
                _Temp[4] = _Version[1];
                _Temp[5] = _Version[2];
                return _Temp;
            }
        }
        /// <summary> 
        /// 逻辑屏幕标识符 
        /// </summary> 
        private class LogicalScreen
        {
            /// <summary> 
            /// 图形宽 
            /// </summary> 
            private byte[] _Width = new byte[2];
            /// <summary> 
            /// 图形高 
            /// </summary> 
            private byte[] _Height = new byte[2];
            /// <summary> 
            /// 第8位 - 全局颜色列表标志(Global Color Table Flag)，当置位时表示有全局颜色列表，pixel值有意义. 
            /// 第5位 第6位 第7位 颜色深度(Color ResoluTion)，cr+1确定图象的颜色深度. 
            /// 第4位 分类标记 如果置位表示全局颜色列表分类排列. 
            /// 第1位 第2位 第3位 全局颜色列表大小，pixel+1确定颜色列表的索引数（2的pixel+1次方）. 
            /// </summary> 
            private byte[] _BitArray = new byte[1];
            /// <summary> 
            /// 背景色索引 
            /// </summary> 
            private byte _Blackground = 0;
            /// <summary> 
            /// 像素宽高比 
            /// </summary> 
            private byte _PixelAspectRadio = 0;
            public LogicalScreen(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                _Width[0] = p_Byte[p_Index];
                _Width[1] = p_Byte[p_Index + 1];
                _Height[0] = p_Byte[p_Index + 2];
                _Height[1] = p_Byte[p_Index + 3];
                _BitArray[0] = p_Byte[p_Index + 4];
                _Blackground = p_Byte[p_Index + 5];
                _PixelAspectRadio = p_Byte[p_Index + 6];
                p_Index += 7;
                p_Open = true;
            }
            public LogicalScreen(ushort p_Width, ushort p_Height)
            {
                Width = p_Width;
                Height = p_Height;
                _Blackground = 0;
                _PixelAspectRadio = 0;
                _BitArray[0] = 135;
            }
            /// <summary> 
            /// 高 
            /// </summary> 
            public ushort Width { get { return BitConverter.ToUInt16(_Width, 0); } set { _Width = BitConverter.GetBytes(value); } }
            /// <summary> 
            /// 宽 
            /// </summary> 
            public ushort Height { get { return BitConverter.ToUInt16(_Height, 0); } set { _Height = BitConverter.GetBytes(value); } }
            /// <summary> 
            /// 背景索引 
            /// </summary> 
            public byte Blackground { get { return _Blackground; } set { _Blackground = value; } }
            /// <summary> 
            /// 像素宽高比 
            /// </summary> 
            public byte PixelAspectRadio { get { return _PixelAspectRadio; } set { _PixelAspectRadio = value; } }
            /// <summary> 
            /// 全局颜色列表标志 
            /// </summary> 
            public bool GlobalPal
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[7];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[7] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// RGB颜色值是否按照使用率进行从高到底的 次序排序的 
            /// </summary> 
            public bool SortFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[3];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[3] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// 色彩表数量 
            /// </summary> 
            public int GlobalColorSize
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    BitArray _Bit = new BitArray(3);
                    _Bit[0] = _BitList[0];
                    _Bit[1] = _BitList[1];
                    _Bit[2] = _BitList[2];
                    byte[] _Count = new byte[1];
                    _Bit.CopyTo(_Count, 0);
                    return (int)Math.Pow(2, _Count[0] + 1);
                }
            }
            /// <summary> 
            /// 彩色分辨率 
            /// </summary> 
            public byte ColorResolution
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    BitArray _Bit = new BitArray(3);
                    _Bit[0] = _BitList[4];
                    _Bit[1] = _BitList[5];
                    _Bit[2] = _BitList[6];
                    byte[] _Count = new byte[1];
                    _Bit.CopyTo(_Count, 0);
                    return _Count[0];
                }
            }
            public byte[] GetByte()
            {
                byte[] _Temp = new byte[7];
                _Temp[0] = _Width[0];
                _Temp[1] = _Width[1];
                _Temp[2] = _Height[0];
                _Temp[3] = _Height[1];
                _Temp[4] = _BitArray[0];
                _Temp[5] = _Blackground;
                _Temp[6] = _PixelAspectRadio;
                return _Temp;
            }
        }
        /// <summary> 
        /// 色彩表 
        /// </summary> 
        private class ColorTable
        {
            private IList<Color> _ColorTable = new List<Color>();
            public ColorTable(bool p_GlobalPal, int p_ColorNumb, byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                if (p_GlobalPal)
                {
                    for (int i = 0; i != p_ColorNumb; i++)
                    {
                        _ColorTable.Add(Color.FromArgb((int)p_Byte[p_Index], (int)p_Byte[p_Index + 1], (int)p_Byte[p_Index + 2]));
                        p_Index += 3;
                    }
                }
                p_Open = true;
            }
            public ColorTable()
            {
            }
            /// <summary> 
            /// 获取全局色彩表 
            /// </summary> 
            /// <returns></returns> 
            public IList<Color> ColorTableList { get { return _ColorTable; } set { _ColorTable = value; } }
            public byte[] GetByte()
            {
                byte[] _Temp = new byte[_ColorTable.Count * 3];
                for (int i = 0; i != _ColorTable.Count; i++)
                {
                    _Temp[(i * 3)] = (byte)_ColorTable[i].R;
                    _Temp[(i * 3) + 1] = (byte)_ColorTable[i].G;
                    _Temp[(i * 3) + 2] = (byte)_ColorTable[i].B;
                }
                return _Temp;
            }
        }
        /// <summary> 
        /// 注释~~~ 
        /// </summary> 
        private class ExtensionIntroducer
        {
            private byte[] _Header = new byte[] { 0x21, 0xFE };
            private MemoryStream _Text = new MemoryStream();
            public ExtensionIntroducer(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                while (true)
                {
                    if (p_Index > p_Byte.Length - 1) return;
                    if (p_Byte[p_Index] == 0) break;
                    _Text.WriteByte(p_Byte[p_Index]);
                    p_Index++;
                }
                _Text.WriteByte(0);
                p_Index++;
                p_Open = true;
            }
            public ExtensionIntroducer(string p_Text)
            {
                Text = p_Text;
            }
            public string Text
            {
                get { return Encoding.ASCII.GetString(_Text.GetBuffer()); }
                set { _Text = new MemoryStream(Encoding.ASCII.GetBytes(value)); _Text.WriteByte(0); }
                //设置数据的时候一定要再写个0 
            }
            public byte[] GetByte()
            {
                byte[] _Temp = new byte[2 + _Text.Length];
                _Temp[0] = _Header[0];
                _Temp[1] = _Header[1];
                _Text.Position = 0;
                for (int i = 0; i != _Text.Length - 1; i++)
                {
                    _Temp[2 + i] = (byte)_Text.ReadByte();
                }
                return _Temp;
            }
        }
        /// <summary> 
        /// 应用程序扩展 
        /// </summary> 
        private class ApplicationExtension
        {
            private byte[] _Header = new byte[] { 0x21, 0xFF };
            private byte _BlockSize = 0x0B;  //11大小 
            private byte[] _Identifier = new byte[8]; //8个ASCII 
            private byte[] _AuthenticationCode = new byte[3]; //3个ASCII 
            private MemoryStream _Data = new MemoryStream();
            public ApplicationExtension(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                _BlockSize = p_Byte[p_Index];
                _Identifier[0] = p_Byte[p_Index + 1];
                _Identifier[1] = p_Byte[p_Index + 2];
                _Identifier[2] = p_Byte[p_Index + 3];
                _Identifier[3] = p_Byte[p_Index + 4];
                _Identifier[4] = p_Byte[p_Index + 5];
                _Identifier[5] = p_Byte[p_Index + 6];
                _Identifier[6] = p_Byte[p_Index + 7];
                _Identifier[7] = p_Byte[p_Index + 8];
                _AuthenticationCode[0] = p_Byte[p_Index + 9];
                _AuthenticationCode[1] = p_Byte[p_Index + 10];
                _AuthenticationCode[2] = p_Byte[p_Index + 11];
                p_Index += 12;
                while (true)
                {
                    if (p_Index + 1 > p_Byte.Length - 1) return;
                    if (p_Byte[p_Index] == 0 && p_Byte[p_Index + 1] != 0) break;
                    _Data.WriteByte(p_Byte[p_Index]);
                    p_Index++;
                }
                _Data.WriteByte(0);
                p_Index++;
                p_Open = true;
            }
            public ApplicationExtension()
            {
                _Identifier[0] = 0x4E;
                _Identifier[1] = 0x45;
                _Identifier[2] = 0x54;
                _Identifier[3] = 0x53;
                _Identifier[4] = 0x43;
                _Identifier[5] = 0x41;
                _Identifier[6] = 0x50;
                _Identifier[7] = 0x45;
                _AuthenticationCode[0] = 0x32;
                _AuthenticationCode[1] = 0x2E;
                _AuthenticationCode[2] = 0x30;
                _Data.Position = 0;
                _Data.WriteByte(3);
                _Data.WriteByte(1);
                _Data.WriteByte(232);
                _Data.WriteByte(3);
                _Data.WriteByte(0);
            }
            public byte[] GetByte()
            {
                byte[] _Temp = new byte[14 + _Data.Length];
                _Temp[0] = _Header[0];
                _Temp[1] = _Header[1];
                _Temp[2] = _BlockSize;
                _Temp[3] = _Identifier[0];
                _Temp[4] = _Identifier[1];
                _Temp[5] = _Identifier[2];
                _Temp[6] = _Identifier[3];
                _Temp[7] = _Identifier[4];
                _Temp[8] = _Identifier[5];
                _Temp[9] = _Identifier[6];
                _Temp[10] = _Identifier[7];
                _Temp[11] = _AuthenticationCode[0];
                _Temp[12] = _AuthenticationCode[1];
                _Temp[13] = _AuthenticationCode[2];
                _Data.Position = 0;
                for (int i = 0; i != _Data.Length - 1; i++)
                {
                    _Temp[14 + i] = (byte)_Data.ReadByte();
                }
                return _Temp;
            }
        }
        /// <summary> 
        /// 图形控制扩展 
        /// </summary> 
        private class GraphicControlExtension
        {
            private byte[] _Header = new byte[] { 0x21, 0xF9 };
            private byte _BlockSize = 0x4; //4大小 
            private byte[] _BitArray = new byte[1];
            private byte[] _DelayTime = new byte[2];
            private byte _TransparentColorIndex = 0;
            private byte _BlockEnd = 0;
            private ImageDescriptor _Image;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="p_Byte">图片数据的字节数组</param>
            /// <param name="p_Index"></param>
            /// <param name="p_Open"></param>
            public GraphicControlExtension(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                _BlockSize = p_Byte[p_Index];
                _BitArray[0] = p_Byte[p_Index + 1];
                _DelayTime[0] = p_Byte[p_Index + 2];
                _DelayTime[1] = p_Byte[p_Index + 3];
                _TransparentColorIndex = p_Byte[p_Index + 4];
                _BlockEnd = p_Byte[p_Index + 5];
                p_Index += 6;
                _Image = new ImageDescriptor(p_Byte, ref p_Index, ref p_Open);
            }
            /// <summary> 
            /// 设置颜色索引表 
            /// </summary> 
            public ColorTable ColorTable { get { return _Image.ColorList; } set { _Image.ColorList = value; } }
            /// <summary> 
            /// 设置颜色索引表 
            /// </summary> 
            public bool UserColorTable { get { return _Image.LocalColorTableFlag; } set { _Image.LocalColorTableFlag = value; } }
            /// <summary> 
            /// 设置颜色索引表 
            /// </summary> 
            public int UserColorTableNumb { get { return _Image.PixelNumb; } set { _Image.PixelNumb = value; } }
            /// <summary> 
            /// 播放时间 
            /// </summary> 
            public ushort DelayTime { get { return BitConverter.ToUInt16(_DelayTime, 0); } set { _DelayTime = BitConverter.GetBytes(value); } }
            /// <summary> 
            /// 设置透明色 
            /// </summary> 
            public byte TransparentColorIndex { get { return _TransparentColorIndex; } set { _TransparentColorIndex = value; } }
            /// <summary> 
            /// 置位表示使用透明颜色 
            /// </summary> 
            public bool TransparentColorFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[0];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[0] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// 指出是否期待用户有输入之后才继续进行下去 
            /// </summary> 
            public bool UseInputFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[1];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[1] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// 处理模式 
            /// </summary> 
            public DisposalMethod Disposal
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    BitArray _Bit = new BitArray(3);
                    _Bit[0] = _BitList[2];
                    _Bit[1] = _BitList[3];
                    _Bit[2] = _BitList[4];
                    byte[] _Count = new byte[1];
                    _Bit.CopyTo(_Count, 0);
                    switch (_Count[0])
                    {
                        case 1:
                            return DisposalMethod.NoDisposalMethod;
                        case 2:
                            return DisposalMethod.NoDisposalImage;
                        case 3:
                            return DisposalMethod.RestoreBackgroundColor;
                        case 4:
                            return DisposalMethod.RestoreFrontState;
                        default:
                            return DisposalMethod.Null;
                    }
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    bool[] _Value = new bool[3];
                    switch (value)
                    {
                        case DisposalMethod.NoDisposalMethod:
                            _Value[0] = true;
                            break;
                        case DisposalMethod.NoDisposalImage:
                            _Value[1] = true;
                            break;
                        case DisposalMethod.RestoreBackgroundColor:
                            _Value[0] = true;
                            _Value[1] = true;
                            break;
                        case DisposalMethod.RestoreFrontState:
                            _Value[2] = true;
                            break;
                        case DisposalMethod.Null:
                            break;
                    }
                    BitArray _Bit = new BitArray((int)value);
                    _BitList[2] = _Value[0];
                    _BitList[3] = _Value[1];
                    _BitList[4] = _Value[2];
                    byte[] _Count = new byte[1];
                    _BitList.CopyTo(_Count, 0);
                    _BitArray[0] = _Count[0];
                }
            }
            /// <summary> 
            /// 获取数据 
            /// </summary> 
            /// <returns></returns> 
            public byte[] GetByte()
            {
                byte[] _ImageByte = _Image.GetByte();
                byte[] _Bytes = new byte[8 + _ImageByte.Length];
                _Bytes[0] = _Header[0];
                _Bytes[1] = _Header[1];
                _Bytes[2] = _BlockSize;
                _Bytes[3] = _BitArray[0];
                _Bytes[4] = _DelayTime[0];
                _Bytes[5] = _DelayTime[1];
                _Bytes[6] = _TransparentColorIndex;
                _Bytes[7] = _BlockEnd;
                _Image.PixelNumb = 256;
                Array.Copy(_ImageByte, 0, _Bytes, 8, _ImageByte.Length);
                return _Bytes;
            }
            public Point Point
            {
                get
                {
                    return new Point(_Image.Left, _Image.Top);
                }
                set
                {
                    _Image.Left = (ushort)value.X;
                    _Image.Top = (ushort)value.Y;
                }
            }
        }
        /// <summary> 
        /// 图形区域 
        /// </summary> 
        private class ImageDescriptor
        {
            private byte[] _Header = new byte[] { 0x2C };
            private byte[] _Left = new byte[2];
            private byte[] _Top = new byte[2];
            private byte[] _Width = new byte[2];
            private byte[] _Height = new byte[2];
            private byte[] _BitArray = new byte[1];
            private ColorTable _ColorTable = new ColorTable();
            private MemoryStream _Lzw = new MemoryStream();
            public ImageDescriptor(byte[] p_Byte, ref int p_Index, ref bool p_Open)
            {
                p_Open = false;
                if (p_Byte[p_Index] != _Header[0]) return;
                _Left[0] = p_Byte[p_Index + 1];   //790 
                _Left[1] = p_Byte[p_Index + 2];
                _Top[0] = p_Byte[p_Index + 3];
                _Top[1] = p_Byte[p_Index + 4];
                _Width[0] = p_Byte[p_Index + 5];
                _Width[1] = p_Byte[p_Index + 6];
                _Height[0] = p_Byte[p_Index + 7];
                _Height[1] = p_Byte[p_Index + 8];
                _BitArray[0] = p_Byte[p_Index + 9];  //798 
                p_Index += 10;
                _ColorTable = new ColorTable(LocalColorTableFlag, PixelNumb, p_Byte, ref p_Index, ref p_Open);
                _Lzw.WriteByte(p_Byte[p_Index]);
                p_Index++;
                while (true)
                {
                    int _BlockByteSize = p_Byte[p_Index];  //判断后面的块大小 
                    _Lzw.WriteByte(p_Byte[p_Index]);  //不管是什么先写入 
                    p_Index++;
                    if (_BlockByteSize > 0)
                    {
                        _Lzw.Write(p_Byte, p_Index, _BlockByteSize);
                        p_Index += _BlockByteSize;
                    }
                    else
                    {
                        break;
                    }
                }
                p_Open = true;
            }
            /// <summary> 
            /// 局部颜色列表标志 
            /// </summary> 
            public bool LocalColorTableFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[7];
                }
                set
                {
                    if (value)
                    {
                        PixelNumb = 256;
                    }
                    else
                    {
                        PixelNumb = 0;
                    }
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[7] = value;
                    byte[] _Count = new byte[1];
                    _BitList.CopyTo(_Count, 0);
                    _BitArray[0] = _Count[0];
                }
            }
            /// <summary> 
            /// 交织标志 
            /// </summary> 
            public bool InterlaceFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[6];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[6] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// 表示紧跟着的局部颜色列表分类排列 
            /// </summary> 
            public bool SortFlag
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    return _BitList[5];
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    _BitList[5] = value;
                    _BitList.CopyTo(_BitArray, 0);
                }
            }
            /// <summary> 
            /// 颜色标数量 
            /// </summary> 
            public int PixelNumb
            {
                get
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    BitArray _Bit = new BitArray(3);
                    _Bit[0] = _BitList[0];
                    _Bit[1] = _BitList[1];
                    _Bit[2] = _BitList[2];
                    byte[] _Count = new byte[1];
                    _Bit.CopyTo(_Count, 0);
                    return (int)Math.Pow(2, _Count[0] + 1);
                }
                set
                {
                    BitArray _BitList = new BitArray(_BitArray);
                    byte[] _Count = new byte[1];
                    switch (value)
                    {
                        case 0:
                            _BitList[0] = false;
                            _BitList[1] = false;
                            _BitList[2] = false;
                            _BitList.CopyTo(_Count, 0);
                            _BitArray[0] = _Count[0];
                            break;
                        default:
                            _BitList[0] = true;
                            _BitList[1] = true;
                            _BitList[2] = true;
                            _BitList.CopyTo(_Count, 0);
                            _BitArray[0] = _Count[0];
                            break;
                    }
                }
            }
            /// <summary> 
            /// 开始位置X 
            /// </summary> 
            public ushort Left
            {
                get { return BitConverter.ToUInt16(_Left, 0); }
                set { _Left = BitConverter.GetBytes(value); }
            }
            /// <summary> 
            /// 结束位置Y 
            /// </summary> 
            public ushort Top
            {
                get { return BitConverter.ToUInt16(_Top, 0); }
                set { _Top = BitConverter.GetBytes(value); }
            }
            /// <summary> 
            /// 宽 
            /// </summary> 
            public short Width
            {
                get { return BitConverter.ToInt16(_Width, 0); }
                set { _Width = BitConverter.GetBytes(value); }
            }
            /// <summary> 
            /// 高 
            /// </summary> 
            public short Height
            {
                get { return BitConverter.ToInt16(_Height, 0); }
                set { _Height = BitConverter.GetBytes(value); }
            }
            /// <summary> 
            /// 色彩表 
            /// </summary> 
            public ColorTable ColorList { get { return _ColorTable; } set { _ColorTable = value; } }
            /// <summary> 
            /// 色彩表 
            /// </summary> 
            public MemoryStream LzwData { get { return _Lzw; } set { _Lzw = value; } }
            public byte[] GetByte()
            {
                int _ColorCount = _ColorTable.ColorTableList.Count * 3;
                byte[] _Temp = new byte[10 + _ColorCount + _Lzw.Length];
//这里2009-05-04修改为10  原来的11 图形在IE和QQ的EDIT里不会动. 
                _Temp[0] = _Header[0];
                _Temp[1] = _Left[0];
                _Temp[2] = _Left[1];
                _Temp[3] = _Top[0];
                _Temp[4] = _Top[1];
                _Temp[5] = _Width[0];
                _Temp[6] = _Width[1];
                _Temp[7] = _Height[0];
                _Temp[8] = _Height[1];
                _Temp[9] = _BitArray[0];
                int _Index = 10;
                if (PixelNumb != 0)
                {
                    for (int i = 0; i != _ColorTable.ColorTableList.Count; i++)
                    {
                        _Temp[_Index] = (byte)_ColorTable.ColorTableList[i].R;
                        _Temp[_Index + 1] = (byte)_ColorTable.ColorTableList[i].G;
                        _Temp[_Index + 2] = (byte)_ColorTable.ColorTableList[i].B;
                        _Index += 3;
                    }
                }
                _Lzw.Position = 0;
                for (int i = 0; i != _Lzw.Length - 1; i++)
                {
                    _Temp[_Index] = (byte)_Lzw.ReadByte();
                    _Index++;
                }
                return _Temp;
            }
        } 
        #endregion 


        #region GIF透明处理 
        /// <summary> 
        /// 转换为GIF透明  保存 Image.Save(@"c:/1.GIF", ImageFormat.Gif); ALPHA 0为透明 
        /// </summary> 
        /// <param name="p_MyBitMap">原始图形</param> 
        /// <param name="p_Transparent">透明色如果默认为黑色</param> 
        /// <returns>GIF格式的IMAGE</returns> 
        public static Bitmap ToImageGif(Bitmap p_MyBitMap, Color p_Transparent)
        {
        
            int _Width = p_MyBitMap.Width;
            int _Height = p_MyBitMap.Height;
            System.IO.MemoryStream _MemoryStream = new MemoryStream();
            p_MyBitMap.Save(_MemoryStream, ImageFormat.Gif);//吧我的图片以gif格式保存到内存
            Bitmap _SaveImage = (Bitmap)Image.FromStream(_MemoryStream);  //保存成GIF 256色
            _MemoryStream.Dispose();
            //获取颜色索引
            System.Drawing.Imaging.ColorPalette _ColorPalette = _SaveImage.Palette;           
            _ColorPalette.Entries[16] = Color.FromArgb(_ColorPalette.Entries[16].A, p_Transparent.R, p_Transparent.G, p_Transparent.B);
            BitmapData _SaveData = _SaveImage.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            byte[] _SaveBytes = new byte[_SaveData.Stride * _SaveData.Height];  //获取保存后的颜色
            Marshal.Copy(_SaveData.Scan0, _SaveBytes, 0, _SaveData.Stride * _SaveData.Height);

            Bitmap _LoadBitmap = new Bitmap(_Width, _Height, PixelFormat.Format32bppArgb);
            Graphics _Graphcis = Graphics.FromImage(_LoadBitmap);
            _Graphcis.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            _Graphcis.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            _Graphcis.DrawImage(p_MyBitMap, new Rectangle(0, 0, _Width, _Height));
            _Graphcis.Dispose();
            BitmapData _LoadData = _LoadBitmap.LockBits(new Rectangle(0, 0, _Width, _Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] _LoadByte = new byte[_LoadData.Stride * _SaveData.Height]; //获取土星的原始颜色
            Marshal.Copy(_LoadData.Scan0, _LoadByte, 0, _LoadData.Stride * _LoadData.Height);
            int _ReadIndex = 0;
            int _WriteIndex = 0;
            int _ReadWidthIndex = 0;
            Color _ReadColor = Color.Transparent;//透明色
            int _Transparent = p_Transparent.ToArgb();//获取自己设置的透明色rgb
            for (int i = 0; i != _Height; i++)
            {
                _ReadIndex = i * _LoadData.Stride;
                _WriteIndex = i * _SaveData.Stride;
                for (int z = 0; z != _Width; z++)
                {
                    _ReadWidthIndex = z * 4;
                    _ReadColor = Color.FromArgb(_LoadByte[_ReadIndex + _ReadWidthIndex + 3], _LoadByte[_ReadIndex + _ReadWidthIndex + 2], _LoadByte[_ReadIndex + _ReadWidthIndex + 1], _LoadByte[_ReadIndex + _ReadWidthIndex]);
                    if (_ReadColor.A == 0 || _ReadColor.ToArgb() == _Transparent)
                    {
                        _SaveBytes[_WriteIndex + z] = (byte)16;
                    }
                }
            }
            _LoadBitmap.UnlockBits(_LoadData);
            _LoadBitmap.Dispose();
            Marshal.Copy(_SaveBytes, 0, _SaveData.Scan0, _SaveBytes.Length);
            _SaveImage.UnlockBits(_SaveData);
            _SaveImage.Palette = _ColorPalette;
            return _SaveImage;
   
        } 
        #endregion 
    }
}

