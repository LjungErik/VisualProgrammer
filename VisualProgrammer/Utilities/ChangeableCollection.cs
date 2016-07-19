using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VisualProgrammer.Utilities
{
    public class ChangeableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private List<T> content = new List<T>();

        public ChangeableCollection()
        { }

        public ChangeableCollection(IEnumerable<T> collection) :
            base(collection)
        { }

        public ChangeableCollection(List<T> list) :
            base(list)
        { }

        public event PropertyChangedEventHandler ContentChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnAdd(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnRemove(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    OnReset();
                    break;
            }
        }

        private void OnAdd(IList newItems)
        {
            foreach(T item in newItems)
            {
                content.Add(item);
                item.PropertyChanged += new PropertyChangedEventHandler(OnContentChanged);
            }

            OnContentChanged(this, new PropertyChangedEventArgs("Add"));
        }

        private void OnRemove(IList oldItems)
        {
            foreach(T item in oldItems)
            {
                content.Remove(item);
                item.PropertyChanged -= new PropertyChangedEventHandler(OnContentChanged);
            }

            OnContentChanged(this, new PropertyChangedEventArgs("Remove"));
        }

        private void OnReset()
        {
            foreach(T item in content)
            {
                item.PropertyChanged -= new PropertyChangedEventHandler(OnContentChanged);
            }
            content.Clear();

            OnContentChanged(this, new PropertyChangedEventArgs("Reset"));
        }

        private void OnContentChanged(object sender, PropertyChangedEventArgs e)
        {
            if(ContentChanged != null)
                ContentChanged(this, e);
        }
    }
}
