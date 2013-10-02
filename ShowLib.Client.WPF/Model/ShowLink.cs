using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;
using ShowLib.Data.Entities;

namespace ShowLib.Client.WPF.Model
{
    public class ShowLink : Link<Show>
    {
        public ShowLink(Show show)
        {
            this.PopulateWith(show);
        }

        public void PopulateWith(Show show)
        {
            this.Item = show;
            this.DisplayName = show != null ? show.Title : "--No Title--";
            this.Source = show != null ? new Uri("/view/ShowDetailPage.xaml#" + show.Id, UriKind.Relative) : null;
        }
    }
}
