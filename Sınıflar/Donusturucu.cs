using System.IO;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace Image2ICO_Converter.Sınıflar
{
    public class Donusturucu
    {
        #region SingletonPattern
        private static          Donusturucu?    instance                                    ;
        private static readonly object          lockObject                  = new object()  ;

        private Donusturucu()
        {
            // Constructor is private to prevent instantiation.
        }

        public static Donusturucu GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        // Double-check locking to ensure thread safety.
                        instance ??= new Donusturucu();
                    }
                }
                return instance;
            }
        }
        #endregion

        public BOOL PNG2ICO(Stream  input_stream                , 
                            Stream  output_stream               , 
                            INT32   size_s32                    , 
                            BOOL    keep_aspect_ratio_b = false )
        {
            Bitmap input_bit = (Bitmap)Image.FromStream(input_stream);

            if (input_bit != null)
            {
                INT32 width, height;

                if (true == keep_aspect_ratio_b)
                {
                    width   = size_s32;
                    height  = input_bit.Height / input_bit.Width * size_s32;
                }
                else
                {
                    width = height = size_s32;
                }

                Bitmap new_bit = new Bitmap(input_bit, new Size(width, height));

                if (new_bit != null)
                {
                    // yeniden boyutlandirilan png, ileride kullanmak uzere bir bellek akisina kaydedilir
                    MemoryStream mem_data = new MemoryStream();
                    new_bit.Save(mem_data, System.Drawing.Imaging.ImageFormat.Png);

                    BinaryWriter icon_writer = new BinaryWriter(output_stream);

                    if (output_stream != null && icon_writer != null)
                    {
                        icon_writer = Ikon_Yaz(icon_writer, width, height, mem_data);
                        icon_writer.Flush();

                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        public BOOL PNG2ICO(string  input_image_str             , 
                            string  output_icon_str             , 
                            INT32   size_s32                    , 
                            BOOL    keep_aspect_ratio_b = false )
        {
            FileStream  input_stream    = new FileStream(input_image_str, FileMode.Open);
            FileStream  output_stream   = new FileStream(output_icon_str, FileMode.OpenOrCreate);

            BOOL        result          = PNG2ICO(input_stream, output_stream, size_s32, keep_aspect_ratio_b);

            input_stream.Close();
            output_stream.Close();

            return result;
        }

        public BinaryWriter Ikon_Yaz(BinaryWriter   icon_writer , 
                                     INT32          width       , 
                                     INT32          height      , 
                                     MemoryStream   mem_data    )
        {
            // 0-1 reserved, 0
            icon_writer.Write((INT8)0);
            icon_writer.Write((INT8)0);

            // 2-3 image type, 1 = icon, 2 = cursor
            icon_writer.Write((INT16)1);

            // 4-5 number of images
            icon_writer.Write((INT16)1);

            // image entry 1
            // 0 image width
            icon_writer.Write((INT8)width);
            // 1 image height
            icon_writer.Write((INT8)height);

            // 2 number of colors
            icon_writer.Write((INT8)0);

            // 3 reserved
            icon_writer.Write((INT8)0);

            // 4-5 color planes
            icon_writer.Write((INT16)0);

            // 6-7 bits per pixel
            icon_writer.Write((INT16)32);

            // 8-11 size of image data
            icon_writer.Write((INT32)mem_data.Length);

            // 12-15 offset of image data
            icon_writer.Write(6 + 16);

            // write image data
            // png data must contain the whole png data file
            icon_writer.Write(mem_data.ToArray());

            return icon_writer;
        }
    }
}
