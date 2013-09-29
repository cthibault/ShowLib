using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;

namespace ShowLib.Client.WPF.Model
{
    public class LinkWithItem<T> : Link
    {
        private T _item;
        public T Item
        {
            get { return this._item; }
            set
            {
                if (!this._item.Equals(value))
                {
                    this._item = value;
                    this.OnPropertyChanged("Item");
                }
            }
        }
    }
}
