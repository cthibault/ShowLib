using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLib.Client.WPF.ViewModel
{
    public interface IPageViewModel
    {
        string Name { get; }
        Uri Uri { get; }
    }
}
