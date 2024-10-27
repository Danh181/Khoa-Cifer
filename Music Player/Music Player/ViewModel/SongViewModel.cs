using Music_Player.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Music_Player.ViewModel
{
    public class SongViewModel : INotifyPropertyChanged
    {
        // Observable collection of songs
        public ObservableCollection<Song> Songs { get; set; }

        // The active song
        private Song _activeSong;
        public Song ActiveSong
        {
            get { return _activeSong; }
            set
            {
                _activeSong = value;
                OnPropertyChanged();
            }
        }

        // Command for setting active song
        public ICommand SetActiveSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        public ICommand PreviousSongCommand { get; private set; }

        // Action to play the selected song (set by MainWindow.xaml.cs)
        public Action<string> PlaySongAction { get; set; }

        public SongViewModel()
        {
            // Initialize the songs collection
            Songs = new ObservableCollection<Song>();
            LoadSongs();

            SetActiveSongCommand = new RelayCommand<Song>(SetActiveSong);
            NextSongCommand = new RelayCommand(PlayNextSong);
            PreviousSongCommand = new RelayCommand(PlayPreviousSong);
        }

        private void PlayNextSong()
        {
            if (ActiveSong == null) return;

            int currentIndex = Songs.IndexOf(ActiveSong);
            int nextIndex = (currentIndex + 1) % Songs.Count;

            SetActiveSong(Songs[nextIndex]);
        }

        private void PlayPreviousSong()
        {
            if (ActiveSong == null) return;

            int currentIndex = Songs.IndexOf(ActiveSong);
            int previousIndex = (currentIndex - 1 < 0) ? Songs.Count - 1 : currentIndex - 1;
            SetActiveSong(Songs[previousIndex]);
        }

        private void LoadSongs()
        {
            string mp3FolderPath = @"C:\Users\danht\Music";  // Replace with your MP3 folder path
            var files = Directory.GetFiles(mp3FolderPath, "*.mp3");

            int songNumber = 1;
            foreach (var file in files)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                // For this example, I'll just use a dummy time and isActive logic.
                // You can modify this to extract actual duration and set active logic as needed.
                Songs.Add(new Song
                {
                    Number = songNumber.ToString("D2"),
                    Title = fileNameWithoutExtension,
                    FilePath = file,
                    Time = "03:30",  // Dummy time
                    isActive = songNumber == 4  // Activate the 4th song
                });

                songNumber++;
            }
        }

        private void SetActiveSong(Song selectedSong)
        {
            foreach (var song in Songs)
            {
                song.isActive = song == selectedSong;
            }

            ActiveSong = selectedSong;
            PlaySongAction?.Invoke(selectedSong.FilePath);
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // RelayCommand class for command handling
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
