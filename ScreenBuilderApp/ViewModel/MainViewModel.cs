using ScreenBuilderApp.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;


namespace ScreenBuilderApp.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private ObservableCollection<ToolboxItem> controls;
        private ObservableCollection<ToolboxItem> selectedControls;
        private ToolboxItem selectedControl;

        public ObservableCollection<ToolboxItem> Toolbox
        {
            get { return controls; }
            set { controls = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ToolboxItem> SelectedControls
        {
            get { return selectedControls; }
            set { selectedControls = value; OnPropertyChanged(); }
        }

        public ToolboxItem SelectedControl
        {
            get { return selectedControl; }
            set { selectedControl = value; OnPropertyChanged(); }
        }

        public ICommand AddControlCommand { get; }

        public MainViewModel()
        {
            // Initialize the control lists
            Toolbox = new ObservableCollection<ToolboxItem>();
            //{ "Textbox", "Label", "Checkbox", "Button" };
            Toolbox.Add(new ToolboxItem() { Name = "Textbox"});
            Toolbox.Add(new ToolboxItem() { Name = "Label" });
            Toolbox.Add(new ToolboxItem() { Name = "Checkbox" });
            Toolbox.Add(new ToolboxItem() { Name = "Button" });
            SelectedControls = new ObservableCollection<ToolboxItem>();

            // Initialize the command
            AddControlCommand = new RelayCommand(AddControl);
        }

        private void AddControl(object parameter)
        {
            if (parameter is string selectedControl)
            {
                // Add the control to the container
                //if (!string.IsNullOrEmpty(selectedControl))
                //{
                //    SelectedControls.Add(selectedControl);
                //    SelectedControl = null;
                //}
            }
        }

     
    }
}