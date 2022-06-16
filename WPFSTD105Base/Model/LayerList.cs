using devDept.Eyeshot;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WPFSTD105
{
    /// <summary>
    /// 圖層列表
    /// </summary>
    public class LayerList : ObservableCollection<Layer>, IdevDeptModel
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'LayerList.Model' 的 XML 註解
        public devDept.Eyeshot.Model Model { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'LayerList.Model' 的 XML 註解
        /// <summary>
        /// 當我添加或刪除一系列實體時，我只想在最後刷新模型。
        /// </summary>
        bool IdevDeptModel.stopInvalidate { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'LayerList.OnCollectionChanged(NotifyCollectionChangedEventArgs)' 的 XML 註解
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'LayerList.OnCollectionChanged(NotifyCollectionChangedEventArgs)' 的 XML 註解
        {
            base.OnCollectionChanged(e);

            if (Model == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Model.Layers.Add((Layer)e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Model.Layers.Remove((Layer)e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Model.Layers[e.NewStartingIndex] = (Layer)e.NewItems;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Model.Layers.Clear();
                    break;
                default:
                    break;
            }
        }
    }
}
