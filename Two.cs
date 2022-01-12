using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;

namespace _2048
{
    public class Two
    {
        public static int block_num = 4;
        public static int block_size = 45;
        public static int margin = 5;
        public static int padding = 15;
        public static int form_height = margin * (block_num + 1) + block_size * block_num + margin * 2 + block_size + padding * 3 + 14;
        public static int form_length = margin * (block_num + 1) + block_size * block_num + padding * 2 + 14;
        public static int interval_num = 10;
        public static Rank rank;
        private static string file_name = block_num + @"database.txt";
        public static void load()
        {
            DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(Rank));
            using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                rank = dc.ReadObject(fs) as Rank;
            }
            Console.WriteLine("load");
        }

        public static int Main()
        {
            if (File.Exists(file_name))
            {
                load();
            }
            else
            {
                rank = new Rank();
                save();
            }
            Console.WriteLine(form_height + "," + form_length);
            Application.Run(new View());
            return 0;
        }
        public static void save()
        {
            DataContractJsonSerializer dc = new DataContractJsonSerializer(typeof(Rank));
            using (FileStream fs = new FileStream(file_name, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                dc.WriteObject(fs, rank);
            }
            Console.WriteLine("save");
        }
    }
}