using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLib.Client.WPF.Model
{
    public class Link<T> : Link
    {
        private T _item;
        public T Item
        {
            get { return this._item; }
            set
            {
                if (this._item == null || !this._item.Equals(value))
                {
                    this._item = value;
                    this.OnPropertyChanged("Item");
                }
            }
        }
    }
}
