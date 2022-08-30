using System.Windows.Forms;

namespace Vsto.Sample.Client.Panes
{
    public partial class CustomTaskPane : UserControl
    {
        public CustomTaskPane()
        {
            InitializeComponent();
        }

        public object DataContext 
        {  
            get { return _taskPaneContent.DataContext; } 
            set { _taskPaneContent.DataContext = value; }
        }
    }
}
