using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sarcastifer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public 
        StringBuilder sbEntry;
        public SBinding binding; 
        public MainWindow()
        {
            sbEntry = new StringBuilder(4096);
            binding = new SBinding();
            binding.DisplayText = "";

            InitializeComponent();
            
            Entry.TextChanged += Entry_TextChanged;            
            DataContext = binding; 
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {  
            foreach(TextChange t in e.Changes)
            {
                if (t.RemovedLength > 0)
                    sbEntry.Remove(t.Offset, t.RemovedLength);

                if (t.AddedLength > 0)
                    sbEntry.Insert(t.Offset, Entry.Text.Substring(t.Offset, t.AddedLength)); 
            }
            Sarcastify(sbEntry);
            binding.DisplayText = sbEntry.ToString(); 
        }

        private void Sarcastify(StringBuilder builder)
        {
            bool makeSmall = true; 
            for(int i = 0; i < builder.Length; i++)
            {
                byte x = (byte)builder[i];  
                if (x >= 0x41 && x <= 0x5A && makeSmall)
                {
                    builder[i] = (char)(x + 0x20); 
                }
                else if (x >= 0x61 && x <= 0x7A && !makeSmall)
                {
                    builder[i] = (char)(x - 0x20);
                }

                if(x != 32)
                makeSmall = !makeSmall; 
            }
        }
    }

    public class SBinding : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        //// Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        string _value = "";
        public string DisplayText
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value; 
                    OnPropertyChanged("DisplayText"); 
                }
            }
        }
        
        
    }
}
