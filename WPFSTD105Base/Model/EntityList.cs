//using devDept.Eyeshot;
//using devDept.Eyeshot.Entities;
//using WPFSTD105.Model;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WPFSTD105
//{
//    /// <summary>
//    /// Eyeshot實體列表
//    /// </summary>
//    public class EntityList : ObservableCollection<Entity>, IdevDeptModel
//    {
//        #region 公開屬性
//        public devDept.Eyeshot.Model Model { get; set; }

//        #endregion


//        #region 公開方法
//        /// <summary>
//        /// 加入範圍
//        /// </summary>
//        /// <param name="entities"></param>
//        public void AddRange(IEnumerable<Entity> entities)
//        {
//            //暫停更新
//            ((IdevDeptModel)this).stopInvalidate = true;

//            foreach (var entity in entities)
//            {
//                Add(entity);
//            }
//            Model.Entities.AddRange(entities);
//            Model.Entities.Regen();
//            Model.Refresh();
//            //啟動更新
//            ((IdevDeptModel)this).stopInvalidate = false;
//        }

//        ///// <summary>
//        ///// 加入物件
//        ///// </summary>
//        ///// <param name="entity">物件</param>
//        ///// <param name="color">物件顏色</param>
//        //public void Add(Entity entity, System.Drawing.Color color)
//        //{
//        //    this.Add(entity);
//        //    Model.Entities.Add(entity, color);
//        //    Model.Entities.Regen();
//        //}
//        /// <summary>
//        /// 刪除範圍物件
//        /// </summary>
//        /// <param name="entities"></param>
//        public void RemoveRange(IEnumerable<Entity> entities)
//        {
//            //暫停更新
//            ((IdevDeptModel)this).stopInvalidate = true;

//            foreach (var entity in entities)
//            {
//                Remove(entity);
//            }

//            /*只修改和編譯需要它的實體。 每個實體都是自動的
//             * 添加到devDept.Eyeshot.Environment.Blocks後重新生成並編譯
//             * 或devDept.Eyeshot.Block.Entities集合。 您需要調用此函數
//             * 僅當您更改/轉換這些集合中已有的實體時*/
//            Model.Entities.Regen(); //更新實體
//            Model.Refresh(); //強制刷新視圖

//            ((IdevDeptModel)this).stopInvalidate = false;
//        }
//        #endregion

//        #region 受保護的方法
//        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
//        {
//            base.OnCollectionChanged(e);

//            if (Model == null)
//                return;

//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    Model.Entities.Add((Entity)e.NewItems[0]);
//                    break;
//                case NotifyCollectionChangedAction.Remove:
//                    Model.Entities.Remove((Entity)e.OldItems[0]);
//                    break;
//                case NotifyCollectionChangedAction.Replace:
//                    Model.Entities[e.NewStartingIndex] = (Entity)e.NewItems;
//                    break;
//                case NotifyCollectionChangedAction.Reset:
//                    Model.Entities.Clear();
//                    break;
//                default:
//                    break;
//            }
//            //刷新視圖
//            if (!((IdevDeptModel)this).stopInvalidate)
//            {
//                Model.Entities.Regen();
//                Model.Refresh();
//            }
//        }
//        #endregion

//        #region 私有屬性
//        /// <summary>
//        /// 當我添加或刪除一系列實體時，我只想在最後刷新模型。
//        /// </summary>
//        bool IdevDeptModel.stopInvalidate { get; set; }
//        #endregion
//    }
//}
