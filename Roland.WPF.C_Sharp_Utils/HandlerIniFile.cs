using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.C_Sharp_Utils
{
    public class HandlerIniFileWriter : IDisposable
    {
        readonly HandlerIniFile _file = new HandlerIniFile();

        public HandlerIniFileWriter(string filename)
        {
            _file.Load(filename);
        }

        public void SetValue(string section, string key, string value)
        {
            _file.SetValue(section, key, value);
        }

        public void Dispose()
        {
            _file.Save();
        }
    }


    public class HandlerIniFile
    {
        //private Hashtable Sections = new Hashtable();
        private OrderedDictionary Sections = new OrderedDictionary();
        private string sFileName;
        private const string newline = "\r\n";

        private Encoding m_currentEncoding = Encoding.Default;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public HandlerIniFile() { }

        /// <summary>
        ///     Default consturctor that load ini file
        /// </summary>
        /// <param name="fileName">name of the section</param>
        public HandlerIniFile(string fileName)
        {
            sFileName = fileName;
            if (File.Exists(fileName))
                Load(fileName);
        }

        /// <summary>
        ///     add a new section [section] to the ini file
        /// </summary>
        /// <param name="section">name of the section</param>
        public void AddSection(string section)
        {
            if (!Sections.Contains(section))
                Sections.Add(section, new Section());
        }

        /// <summary>
        ///     add a new section [section] to the ini file with key and value
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <param name="key">name of the key</param>
        /// <param name="value">value of the key</param>
        public void AddSection(string section, string key, string value)
        {
            AddSection(section);
            ((Section)Sections[section]).SetKey(key, value);
        }

        /// <summary>
        ///     Remove section from ini file
        /// </summary>
        /// <param name="section">section to remove</param>
        public void RemoveSection(string section)
        {
            if (Sections.Contains(section))
                Sections.Remove(section);
        }

        /// <summary>
        /// Modifie ou crée une valeur d'une clef dans une section
        /// </summary>
        /// <param name="section">Nom de la section</param>
        /// <param name="key">Nom de la clef</param>
        /// <param name="value">Valeur de la clef</param>
        public void SetValue(string section, string key, string value)
        {
            this[section].SetKey(key, value);
        }

        /// <summary>
        /// Retourne la valeur d'une clef dans une section
        /// </summary>
        /// <param name="section">Nom de la section</param>
        /// <param name="key">Nom de la clef</param>
        /// <param name="defaut">Valeur par défaut si la clef/section n'existe pas</param>
        /// <returns>Valeur de la clef, ou la valeur entrée par défaut</returns>
        public string GetValue(string section, string key, object defaut)
        {
            string val = this[section][key];
            if (val == "")
            {
                this[section][key] = defaut.ToString();
                return defaut.ToString();
            }
            else
                return val;
        }

        /// <summary>
        ///     Return the value of a key from section
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <param name="key">name of the key</param>
        /// <returns>Key's value or string empty of not found</returns>
        public string GetValue(string section, string key)
        {
            return GetValue(section, key, "");
        }

        /// <summary>
        ///     Section's indexors
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private Section this[string section]
        {
            get
            {
                if (!Sections.Contains(section))
                    AddSection(section);
                return (Section)Sections[section];
            }
            set
            {
                if (!Sections.Contains(section))
                    AddSection(section);
                Sections[section] = value;
            }
        }

        /// <summary>
        ///     Save current ini file
        /// </summary>
        public void Save()
        {
            if (sFileName != "")
                Save(sFileName);
        }

        /// <summary>
        ///     Save .ini file with new name
        /// </summary>
        /// <param name="fileName">Nom de fichier</param>
        public void Save(string fileName)
        {
            using (StreamWriter str = new StreamWriter(fileName, false, m_currentEncoding))
            {
                foreach (object okey in Sections.Keys)
                {
                    str.Write("[" + okey.ToString() + "]" + newline);
                    Section sct = (Section)Sections[okey.ToString()];
                    foreach (string key in (sct.Keys))
                    {
                        str.Write(key + "=" + sct[key] + newline);
                    }
                }
                str.Flush();
            }
        }

        /// <summary>
        ///     Load .ini file
        /// </summary>
        /// <param name="fileName">filepath to load</param>
        public void Load(string fileName)
        {
            Sections = new OrderedDictionary();
            using (StreamReader str = new StreamReader(fileName, Encoding.Default, true))
            {
                string fichier = str.ReadToEnd();
                string[] lignes = fichier.Split('\r', '\n');
                string currentSection = "";
                //Keep current encoding
                m_currentEncoding = str.CurrentEncoding;
                foreach (string ligne in lignes)
                {
                    string line = ligne.TrimStart();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = ligne.Substring(1, line.Length - 2);
                        AddSection(currentSection);
                    }
                    else if (line != "" && !line.StartsWith("#") && !line.StartsWith("//"))
                    {
                        string[] scts = ligne.Split(new char[1] { '=' }, 2);
                        this[currentSection].SetKey(scts[0].Trim(), scts[1].Trim());
                    }
                }
                this.sFileName = fileName;
            }
        }

        /// <summary>
        ///     Datastruct of the sections
        /// </summary>
        private class Section
        {
            private readonly OrderedDictionary clefs = new OrderedDictionary();
            public Section() { }

            /// <summary>
            ///     Set a value to a key and create it if not exist 
            /// </summary>
            /// <param name="key">Name of the key</param>
            /// <param name="value">Value of the key</param>
            public void SetKey(string key, string value)
            {
                if (key.IndexOf("=") > 0)
                    throw new Exception("char '=' forbidden");
                if (clefs.Contains(key))
                    clefs[key] = value;
                else
                    clefs.Add(key, value);
            }
            /// <summary>
            ///     Delete a key
            /// </summary>
            /// <param name="key">key to delete</param>
            public void DeleteKey(string key)
            {
                if (clefs.Contains(key))
                    clefs.Remove(key);
            }

            /// <summary>
            /// Contained keys of the section
            /// </summary>
            public ICollection Keys
            {
                get
                {
                    return clefs.Keys;
                }
            }

            /// <summary>
            ///     keys indexer
            /// </summary>
            public string this[string key]
            {
                get
                {
                    if (clefs.Contains(key))
                        return clefs[key].ToString();
                    else
                    {
                        SetKey(key, "");
                        return "";
                    }
                }
                set
                {
                    SetKey(key, value);
                }
            }
        }
    }
}
