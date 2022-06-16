using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    /// <summary>
    /// Eyeshot實體列表
    /// </summary>
    public class EntityList : ObservableCollection<Entity>
    {
        #region 公開屬性
        public devDept.Eyeshot.Model Model { get; set; }
        #endregion

        #region 公開方法
        /// <summary>
        /// 加入範圍
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<Entity> entities)
        {
            stopInvalidate = true;

            foreach (var entity in entities)
            {
                Add(entity);
            }
            Model.Entities.AddRange(entities);
            Model.Refresh();

            stopInvalidate = false;
        }
        /// <summary>
        /// 刪除範圍物件
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<Entity> entities)
        {
            stopInvalidate = true;

            foreach (var entity in entities)
            {
                Remove(entity);
            }
            Model.Refresh();

            stopInvalidate = false;
        }
        #endregion

        #region 受保護的方法
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (Model == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Model.Entities.Add((Entity)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Model.Entities.Remove((Entity)e.OldItems[0]);
                    break;
                default:
                    break;
            }

            if (!stopInvalidate)
                Model.Refresh();
        }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 當我添加或刪除一系列實體時，我只想在最後刷新模型。
        /// </summary>
        private bool stopInvalidate { get; set; }
        #endregion
    }
}
