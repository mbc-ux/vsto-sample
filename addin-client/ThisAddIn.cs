using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Vsto.Sample.Client.Models;
using Vsto.Sample.Client.Panes;
using Vsto.Sample.Client.ViewModels;

namespace Vsto.Sample.Client
{
    public partial class ThisAddIn
    { 
        private static class Builder
        {
            internal static UserControl BuildServicePane(IServiceClient dataPublisher)
            {
                var customTaskPane = new CustomTaskPane();
                var viewModel = new ContentViewModel(dataPublisher);
                customTaskPane.DataContext = viewModel;
                return customTaskPane;
            }
        }

        protected override void OnStartup()
        {
            base.OnStartup();

            var dataPublisher = new ServiceClient();
            var servicePane = Builder.BuildServicePane(dataPublisher);
            var customTaskPane = CustomTaskPanes.Add(servicePane, "HTTP GET");
            customTaskPane.Visible = true;
            Worksheet activeSheet = null;
            Application.WorkbookActivate += book =>
            {
                if (activeSheet != null) Marshal.ReleaseComObject(activeSheet);
                activeSheet = book.ActiveSheet as Worksheet;
            };
            var context = new SynchronizationContext();
            dataPublisher.DataPublished += data => context.Post(_ => activeSheet.Cells[1, 1] = data, null);
        }
    }
}
