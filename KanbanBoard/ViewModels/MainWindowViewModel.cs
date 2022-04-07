using Avalonia.Media;
using Avalonia;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KanbanBoard.Models;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace KanbanBoard.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        ObservableCollection<Note> ItemsPlanned { get; set; }
        ObservableCollection<Note> ItemsInProgress { get; set; }
        ObservableCollection<Note> ItemsCompleted { get; set; }
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            ItemsPlanned = new ObservableCollection<Note>();
            ItemsInProgress = new ObservableCollection<Note>();
            ItemsCompleted = new ObservableCollection<Note>();
            Content = new KanbanViewModel();
        }

        public void AddCompleted() => ItemsCompleted.Insert(0, new Note("New Note"));
        public void AddInProgress() => ItemsInProgress.Insert(0, new Note("New Note"));
        public void AddPlanned() => ItemsPlanned.Insert(0, new Note("New Note"));

        public void DeleteCompleted(Note item) => ItemsCompleted.Remove(item);
        public void DeleteInProgress(Note item) => ItemsInProgress.Remove(item);
        public void DeletePlanned(Note item) => ItemsPlanned.Remove(item);
        public void New()
        {
            ItemsPlanned.Clear();
            ItemsInProgress.Clear();
            ItemsCompleted.Clear();
        }

        public void WriteToBinaryFile(string filePath)
        {
            List<ObservableCollection<Note>> notes = new List<ObservableCollection<Note>>();
            notes.Add(ItemsPlanned);
            notes.Add(ItemsInProgress);
            notes.Add(ItemsCompleted);


            using (StreamWriter wr = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<ObservableCollection<Note>>));
                xs.Serialize(wr, notes);
            }
        }
        public void OpenWindowView() => Content = new KanbanViewModel();
        public void ReadFromBinaryFile(string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<ObservableCollection<Note>>));
            using (StreamReader sr = new StreamReader(filePath))
            {
                ItemsCompleted.Clear();
                ItemsInProgress.Clear();
                ItemsPlanned.Clear();
                List<ObservableCollection<Note>> notes = (List<ObservableCollection<Note>>)xs.Deserialize(sr);
                ItemsCompleted = notes[2];
                ItemsInProgress = notes[1];
                ItemsPlanned = notes[0];
            }
        }
    }
}