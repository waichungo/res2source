using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace res2source.viewmodels
{
    public class ViewModelBase : ObservableObject { }
    partial class FileObject : ViewModelBase
    {
        [ObservableProperty]
        private string objectName = "";
        [ObservableProperty]
        private string path = "";
        [ObservableProperty]
        private long size = 0;
        [ObservableProperty]
        private bool useStatic = false;
    }
    partial class MainWindowViewModel:ViewModelBase
    {
        [ObservableProperty]
        private string className = "";
        [ObservableProperty]
        private ObservableCollection<FileObject> files = new();
    }

}
