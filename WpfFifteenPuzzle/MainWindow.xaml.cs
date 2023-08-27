using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace WpfFifteenPuzzle
{
    public partial class MainWindow : Window
    {

        private ObservableCollection<int> numbers;
        private Random random = new Random();

        public ICommand MoveCommand { get; }
        public ICommand ShuffleCommand { get; }

        public MainWindow()
        {
            InitializeComponent();

            numbers = new ObservableCollection<int>(Enumerable.Range(1, 15));
            numbers.Add(0); // Empty cell
            MoveCommand = new RelayCommand<object>(Move);
            ShuffleCommand = new RelayCommand<object>(Shuffle);
            board.ItemsSource = numbers;

            this.DataContext = this;
        }

        private void Move(object parameter)
        {
            int index = numbers.IndexOf((int)parameter);
            int emptyIndex = numbers.IndexOf(0);

            if (IsAdjacent(index, emptyIndex))
            {
                numbers[emptyIndex] = (int)parameter;
                numbers[index] = 0;
            }
        }

        private void Shuffle(object parameter)
        {
           
            int[] shuffledNumbers = numbers.OrderBy(n => random.Next()).ToArray();
            for (int i = 0; i < shuffledNumbers.Length; i++)
            {
                numbers[i] = shuffledNumbers[i];
            }
        }

        private bool IsAdjacent(int index1, int index2)
        {
            int row1 = index1 / 4;
            int col1 = index1 % 4;
            int row2 = index2 / 4;
            int col2 = index2 % 4;

            return Math.Abs(row1 - row2) + Math.Abs(col1 - col2) == 1;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}