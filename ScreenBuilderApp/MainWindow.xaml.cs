using ScreenBuilderApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenBuilderApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext =viewModel = new MainViewModel();
         
        }

    

        private void ToolboxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToolboxList.SelectedItem != null)
            {
              //  ListBoxItem selectedControl = (ListBoxItem)ToolboxList.SelectedItem;
                viewModel.SelectedControl= ToolboxList.SelectedItem as ToolboxItem;
                Point position = Mouse.GetPosition(AnimationCanvas);
                
                // Set initial position of the cloned control
                Canvas.SetLeft(AnimateElement, position.X);
                Canvas.SetTop(AnimateElement, position.Y);
                AnimateElement.Visibility = Visibility.Visible;

                UIElement container = VisualTreeHelper.GetParent(AddedControls) as UIElement;
                Point relativeLocation = AddedControls.TranslatePoint(new Point(0, 0), container);


                if (AddedControls.Items.Count > 0)
                {
                    ListBoxItem lastListboxItem = AddedControls.ItemContainerGenerator.ContainerFromItem(AddedControls.Items[AddedControls.Items.Count - 1]) as ListBoxItem;
                    relativeLocation.Y = (lastListboxItem.TranslatePoint(new Point(0, 0), AddedControls)).Y;                    
                }

                //TODO: Move the storyboard to the xaml instead of generating from the code
                // Animate control flying to the container
                DoubleAnimation flyingAnimation = new DoubleAnimation()
                {
                    From = position.X,
                    To = relativeLocation.X,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction=new CircleEase() { EasingMode=EasingMode.EaseIn }
                };

                DoubleAnimation flyingAnimationY = new DoubleAnimation()
                {
                    From = position.Y,
                    To = relativeLocation.Y,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseIn }
                };

                Storyboard.SetTarget(flyingAnimationY, AnimateElement);
                Storyboard.SetTargetProperty(flyingAnimationY, new PropertyPath(Canvas.TopProperty));

                Storyboard.SetTarget(flyingAnimation, AnimateElement);
                Storyboard.SetTargetProperty(flyingAnimation, new PropertyPath(Canvas.LeftProperty));

                var storyboard = new Storyboard();
                storyboard.Children.Add(flyingAnimation);
                storyboard.Children.Add(flyingAnimationY);
                storyboard.Completed += (s, _) =>
                {
                    AnimateElement.Visibility = Visibility.Collapsed;
                    viewModel.SelectedControls.Add(new ToolboxItem() { Name = viewModel.SelectedControl.Name });
                    // Clear the selection
                    ToolboxList.SelectedItem = null;
                };

                storyboard.Begin();
            }
        }


        private void AddedControls_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.AddedControls.SelectedItem == null) return;
            DragDrop.DoDragDrop(AddedControls, new DataObject(this.AddedControls.SelectedItem), DragDropEffects.Move);
        }

        private void AddedControls_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void AddedControls_Drop(object sender, DragEventArgs e)
        {
            var source  = e.Data.GetData(typeof(ToolboxItem)) as ToolboxItem;
            var targetIndex = viewModel.SelectedControls.IndexOf((sender as ListBoxItem).DataContext as ToolboxItem);
            viewModel.SelectedControls.Remove(source);
            viewModel.SelectedControls.Insert(targetIndex,source);
        }

        private void AddedControls_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }
    }
}
