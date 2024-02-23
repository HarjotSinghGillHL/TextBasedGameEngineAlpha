using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class HL_FileSystem
    {
        void ReadFile(string FilePath)
        {
            string filePath = "@"+FilePath;
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine("[ERROR] The file [" + FilePath+ "] could not be read");
            }
        }

        public static List<string> ReadFileLineByLine(string szMapFileName)
        {
            List<string> map = new List<string>();

            FileInfo FileInfo = new FileInfo(szMapFileName);

            if (!FileInfo.Exists)
            {
                System.Console.WriteLine("File not found [Filename : " + szMapFileName + "]");
                return map;
            }

            if (FileInfo.Length == 0)
                return map;

            StreamReader FileReader = new StreamReader(szMapFileName);

            string Line;
            while ((Line = FileReader.ReadLine()) != null)
                map.Add(Line);

            return map;
        }

        public static int GetLengthOfTheLongestRow(ref List<string> FileData)
        {
            int Size = 0;

            for (int iCurrent = 0; iCurrent < FileData.Count; iCurrent++)
            {
                if (FileData[iCurrent].Length > Size)
                {
                    Size = FileData[iCurrent].Length;
                }
            }

            return Size;
        }

    }
}
