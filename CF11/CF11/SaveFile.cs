using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CF11
{
    class SaveFile
    {
        public SaveFile()
        {
        }

        private string[] lines;
        public void CombineContentText(string barcode, string date_time_12, string led_1_x, string led_1_y, string led_2_x, string led_2_y)
        {
            lines = new string[30];

            lines[0] = "S" + barcode;
            lines[1] = "CVALEO";
            lines[2] = "BXBD";
            lines[3] = "NHuavaleoME01";
            lines[4] = "PAssembly";
            lines[5] = "s1";
            lines[6] = "DN/A";
            lines[7] = "RN/A";
            lines[8] = "nN/A";
            lines[9] = "rN/A";
            lines[10] = "WN/A";
            lines[11] = "TP";
            lines[12] = "OVALEO";
            lines[13] = "L31";
            lines[14] = "p11";
            lines[15] = "[" + date_time_12;
            lines[16] = "]" + date_time_12;
            lines[17] = "MLED1-X:";
            lines[18] = "d" + led_1_x;
            lines[19] = ">";
            lines[20] = "MLED1-Y:";
            lines[21] = "d" + led_1_y;
            lines[22] = ">";
            lines[23] = "MLED2-X:";
            lines[24] = "d" + led_2_x;
            lines[25] = ">";
            lines[26] = "MLED2-Y:";
            lines[27] = "d" + led_2_y;
            lines[28] = ">";
            lines[29] = "";
        }

       public void WriteMES(string barcode, string date_time_24, string num)
       {
           string file_name = barcode + date_time_24 + num + ".log";
           string file_mes_path = @"C:\Tars\";
           string file_backup_path= @"D:\MES\";
               
           // for MES directory
           string combination_path = Path.Combine(file_mes_path, file_name);
           System.IO.File.WriteAllLines(combination_path, this.lines);

           // for MES backup directory
           combination_path = Path.Combine(file_backup_path, file_name);
           if (false == Directory.Exists(file_backup_path))
           {
               Directory.CreateDirectory(file_backup_path);
           }
           System.IO.File.WriteAllLines(combination_path, this.lines);
       }

    }
}
