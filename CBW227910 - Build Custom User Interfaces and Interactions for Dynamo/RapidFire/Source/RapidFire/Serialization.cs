using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidFire
{
    public static class Serialization
    {
        //Thisvariable returns the default shortcut file path.
        public static string ShortcutFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                        "RapidFire", 
                                                        "shortcuts.txt");

        /// <summary>
        /// Read the saved shortcuts file;
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static List<Shortcut> ReadFile(string filePath)
        {
            List<Shortcut> shortcuts = new List<Shortcut>();
            try
            {
                using (StreamReader stream = new StreamReader(filePath))
                {
                    while (!stream.EndOfStream)
                    {
                        string[] shortcutParts = stream.ReadLine().Split(new char[1] { '|' });
                        string nodeName = shortcutParts[1];
                        var thisCut = new Shortcut(shortcutParts[0].ToUpper(), shortcutParts[1]);

                        shortcuts.Add(thisCut);
                    }
                }
                return shortcuts;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal static void WriteShortcutsToFile(string shortcutPath, List<Shortcut> shortcuts)
        {
            //write a blank file
            DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(shortcutPath));
            using (File.Create(shortcutPath));

            // Then write all shortcuts with non-empty keys to the File
            using (StreamWriter shortcutsFile = new StreamWriter(shortcutPath, false))
            {
                foreach (Shortcut shortcut in shortcuts)
                {
                    if (!shortcut.Keys.Equals(""))
                    {
                        string shcut = shortcut.Keys.ToUpper() + "|" + shortcut.NodeName + "|";
                        shortcutsFile.WriteLine(shcut);
                    }
                }
            }
        }
    }
}
