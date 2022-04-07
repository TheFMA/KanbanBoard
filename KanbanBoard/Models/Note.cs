using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace KanbanBoard.Models
{
    [Serializable]
    public class Note : INotifyPropertyChanged
    {

        private string _header;
        private string _text;
        private Bitmap? _pathToFile;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                RaisePropertyChangedEvent("Header");
            }
        }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChangedEvent("Text");
            }
        }
        [XmlIgnore]
        public Bitmap? PathToFile
        {
            get => _pathToFile;
            set
            {
                _pathToFile = value;
                RaisePropertyChangedEvent("PathToFile");
            }
        }
        public Note(string Header)
        {
            this.Header = Header;
            Text = "";
            PathToFile = null;
        }
        public Note()
        {
            Header = "";
            Text = "";
            PathToFile = null;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
        public async void GetFile(Window parent)
        {
            var openFileDialog = new OpenFileDialog().ShowAsync(parent);
            string[]? path = await openFileDialog;
            if (path is not null)
            {
                string sourcePath = String.Join("/", path);
                FileInfo fileInfo = new FileInfo(sourcePath);
                using (FileStream fs = fileInfo.OpenRead())
                {
                    try
                    {
                        PathToFile = Bitmap.DecodeToWidth(fs, 100);
                    }
                    catch (Exception e)
                    {
                        PathToFile = null;
                    }
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("PathToFile")]
        public byte[] PathToFileSerialized
        {
            get
            {
                if (PathToFile == null) return null;
                using (MemoryStream s = new MemoryStream())
                {
                    PathToFile.Save(s);
                    return s.ToArray();
                }
            }
            set
            {
                if (value == null)
                {
                    PathToFile = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        PathToFile = new Bitmap(ms);
                    }
                }
            }
        }
    }
}