using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;
using TheDebtBook.ViewModels;
using TheDebtBook.Views;


namespace TheDebtBook.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<Person> persons;
        private string filename = "";

        public MainWindowViewModel()
        {
            persons = new ObservableCollection<Person>
            {
            #if DEBUG
                new Person("Victor Kildahl", new Debt(-10)),
                new Person("Lasse Mosel", new Debt(-100)),
                new Person("Marc Warming", new Debt(-200)),
                new Person("Brian Stjernholm", new Debt(250))
            #endif  
            };
            CurrentPerson = null;
        }

        Person currentPerson = null;

        public Person CurrentPerson
        {
            get { return currentPerson; }
            set
            {
                SetProperty(ref currentPerson, value);
            }
        }


        int currentIndex = -1;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                SetProperty(ref currentIndex, value);
            }
        }

        public ObservableCollection<Person> Persons
        {
            get { return persons; }
            set { SetProperty(ref persons, value); }
        }


        ICommand _PreviusCommand;
        public ICommand PreviusCommand
        {
            get
            {
                return _PreviusCommand ??
                (_PreviusCommand = new DelegateCommand(
                 PreviusCommandExecute, PreviusCommandCanExecute).ObservesProperty(() => CurrentIndex));
            }
        }

        private void PreviusCommandExecute()
        {
            if (CurrentIndex > 0)
                --CurrentIndex;
        }

        private bool PreviusCommandCanExecute()
        {
            if (CurrentIndex > 0)
                return true;
            else
                return false;
        }

        ICommand _nextCommand;
        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new DelegateCommand(
                       () => ++CurrentIndex,
                       () => CurrentIndex < (Persons.Count - 1)).ObservesProperty(() => CurrentIndex));
            }
        }

        ICommand _newCommand;
        public ICommand AddNewPersonCommand
        {
            get
            {
                return _newCommand ?? (_newCommand = new DelegateCommand(() =>
                {
                    var newPerson = new Person();
                    Persons.Add(newPerson);
                    CurrentPerson = newPerson;
                }));
            }
        }

        ICommand _deleteCommand;
        public ICommand DeletePersonCommand => _deleteCommand ?? (_deleteCommand =
            new DelegateCommand(DeletePerson, DeletePerson_CanExecute)
                    .ObservesProperty(() => CurrentIndex));

        private void DeletePerson()
        {
            Persons.Remove(CurrentPerson);
            // RaisePropertyChanged("Count");
        }

        private bool DeletePerson_CanExecute()
        {
            if (Persons.Count > 0 && CurrentIndex >= 0)
                return true;
            else
                return false;
        }

        ICommand _closeAppCommand;
        public ICommand CloseAppCommand
        {
            get
            {
                return _closeAppCommand ?? (_closeAppCommand = new DelegateCommand(() =>
                {
                    App.Current.MainWindow.Close();
                }));
            }
        }

        ICommand _SaveAsCommand;
        public ICommand SaveAsCommand
        {
            get { return _SaveAsCommand ?? (_SaveAsCommand = new DelegateCommand<string>(SaveAsCommand_Execute)); }
        }

        private void SaveAsCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {
                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                filename = argFilename;
                SaveFileCommand_Execute();
            }
        }

        ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new DelegateCommand(SaveFileCommand_Execute, SaveFileCommand_CanExecute)
                  .ObservesProperty(() => Persons.Count));
            }
        }

        private void SaveFileCommand_Execute()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, Persons);
            writer.Close();
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (filename != "") && (Persons.Count > 0);
        }

        ICommand _NewFileCommand;
        public ICommand NewFileCommand
        {
            get { return _NewFileCommand ?? (_NewFileCommand = new DelegateCommand(NewFileCommand_Execute)); }
        }

        private void NewFileCommand_Execute()
        {
            MessageBoxResult res = MessageBox.Show("Any unsaved data will be lost. Are you sure you want to initiate a new file?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Persons.Clear();
                filename = "";
            }
        }


        ICommand _OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _OpenFileCommand ?? (_OpenFileCommand = new DelegateCommand<string>(OpenFileCommand_Execute)); }
        }

        private void OpenFileCommand_Execute(string argFilename)
        {
            if (argFilename == "")
            {

                MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                filename = argFilename;
                var tempPersons = new ObservableCollection<Person>();

                // Create an instance of the XmlSerializer class and specify the type of object to deserialize.
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
                try
                {
                    TextReader reader = new StreamReader(filename);
                    tempPersons = (ObservableCollection<Person>)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Persons = tempPersons;
            }
        }

        ICommand _history;
        public ICommand HistoryCommand
        {
            get
            {
                return _history ?? (_history = new DelegateCommand(() =>
                {
                    var vm = new PersonViewModel(CurrentPerson);
                    var dlg = new PersonView
                    {
                        DataContext = vm
                    };
                    if (dlg.ShowDialog() == true)
                    {
                    }
                }));
            }
        }
    }
}





