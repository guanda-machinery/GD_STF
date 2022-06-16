using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Collections;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace WPFWindowsBase
{
    /// <summary>
    /// 查詢控制器
    /// </summary>
    public class QueryController
    {
        /// <summary>
        /// 列過濾器數據
        /// </summary>
        public FilterData ColumnFilterData { get; set; }
        /// <summary>
        /// 資料來源
        /// </summary>
        public IEnumerable ItemsSource { get; set; }
        /// <summary>
        /// 列過濾器
        /// </summary>
        private readonly Dictionary<string, FilterData> filtersForColumns;
        /// <summary>
        /// 查詢
        /// </summary>
        Query query;
        /// <summary>
        /// 調用線程分派器
        /// </summary>
        public Dispatcher CallingThreadDispatcher { get; set; }
        /// <summary>
        /// 使用後台工作者
        /// </summary>
        public bool UseBackgroundWorker { get; set; }
        /// <summary>
        /// 鎖住物件
        /// </summary>
        private readonly object lockObject;
        /// <summary>
        /// 查詢控制器
        /// </summary>
        public QueryController()
        {
            lockObject = new object();

            filtersForColumns = new Dictionary<string, FilterData>();
            query = new Query();
        }
        /// <summary>
        /// 查詢
        /// </summary>
        public void DoQuery()
        {
            DoQuery(false);
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="force"></param>
        public void DoQuery(bool force)
        {
            ColumnFilterData.IsSearchPerformed = false;

            if (!filtersForColumns.ContainsKey(ColumnFilterData.ValuePropertyBindingPath))
            {
                filtersForColumns.Add(ColumnFilterData.ValuePropertyBindingPath, ColumnFilterData);
            }
            else
            {
                filtersForColumns[ColumnFilterData.ValuePropertyBindingPath] = ColumnFilterData;
            }

            if (isRefresh)
            {
                if (filtersForColumns.ElementAt(filtersForColumns.Count - 1).Value.ValuePropertyBindingPath
                    == ColumnFilterData.ValuePropertyBindingPath)
                {
                    runFiltering(force);
                }
            }
            else if (filteringNeeded)
            {
                runFiltering(force);
            }

            ColumnFilterData.IsSearchPerformed = true;
            ColumnFilterData.IsRefresh = false;
        }
        /// <summary>
        /// 是當前控制優先控制
        /// </summary>
        public bool IsCurentControlFirstControl
        {
            get
            {
                return filtersForColumns.Count > 0
                    ? filtersForColumns.ElementAt(0).Value.ValuePropertyBindingPath == ColumnFilterData.ValuePropertyBindingPath : false;
            }
        }
        /// <summary>
        /// 清除篩選
        /// </summary>
        public void ClearFilter()
        {
            int count = filtersForColumns.Count;
            for (int i = 0; i < count; i++)
            {
                FilterData data = filtersForColumns.ElementAt(i).Value;

                data.ClearData();
            }

            DoQuery();
        }

        #region 內部
        /// <summary>
        /// 刷新
        /// </summary>
        private bool isRefresh
        {
            get { return (from f in filtersForColumns where f.Value.IsRefresh == true select f).Count() > 0; }
        }
        /// <summary>
        /// 需要過濾
        /// </summary>
        private bool filteringNeeded
        {
            get { return (from f in filtersForColumns where f.Value.IsSearchPerformed == false select f).Count() == 1; }
        }
        /// <summary>
        /// 執行過濾
        /// </summary>
        /// <param name="force">強行</param>
        private void runFiltering(bool force)
        {
            bool filterChanged;

            createFilterExpressionsAndFilteredCollection(out filterChanged, force);

            if (filterChanged || force)
            {
                OnFilteringStarted(this, EventArgs.Empty);

                applayFilter();
            }
        }
        /// <summary>
        /// 創建過濾器表達式和過濾後的集合
        /// </summary>
        /// <param name="filterChanged">過濾條件已更改</param>
        /// <param name="force">強行</param>
        private void createFilterExpressionsAndFilteredCollection(out bool filterChanged, bool force)
        {
            QueryCreator queryCreator = new QueryCreator(filtersForColumns);

            queryCreator.CreateFilter(ref query);

            filterChanged = (query.IsQueryChanged || (query.FilterString != String.Empty && isRefresh));

            if ((force && query.FilterString != String.Empty) || (query.FilterString != String.Empty && filterChanged))
            {
                IEnumerable collection = ItemsSource as IEnumerable;

                if (ItemsSource is ListCollectionView)
                {
                    collection = (ItemsSource as ListCollectionView).SourceCollection as IEnumerable;
                }

                var observable = ItemsSource as System.Collections.Specialized.INotifyCollectionChanged;
                if (observable != null)
                {
                    observable.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(observable_CollectionChanged);
                    observable.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(observable_CollectionChanged);

                }

                #region Debug
#if DEBUG
                System.Diagnostics.Debug.WriteLine("QUERY STATEMENT: " + query.FilterString);

                string debugParameters = String.Empty;
                query.QueryParameters.ForEach(p =>
                {
                    if (debugParameters.Length > 0) debugParameters += ",";
                    debugParameters += p.ToString();
                });

                System.Diagnostics.Debug.WriteLine("QUERY PARAMETRS: " + debugParameters);
#endif
                #endregion

                if (query.FilterString != String.Empty)
                {
                    var result = collection.AsQueryable().Where(query.FilterString, query.QueryParameters.ToArray<object>());

                    filteredCollection = result.Cast<object>().ToList();
                }
            }
            else
            {
                filteredCollection = null;
            }

            query.StoreLastUsedValues();
        }

        private void observable_CollectionChanged(
            object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DoQuery(true);
        }

        #region 内部过滤

        /// <summary>
        /// 過濾後的收藏
        /// </summary>
        private IList filteredCollection;
        /// <summary>
        /// 過濾的集合<see cref="HashSet{T}"/> 
        /// </summary>
        HashSet<object> filteredCollectionHashSet;
        /// <summary>
        /// 應用過濾器
        /// </summary>
        void applayFilter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);

            if (filteredCollection != null)
            {
                executeFilterAction(
                    new Action(() =>
                    {
                        filteredCollectionHashSet = initLookupDictionary(filteredCollection);

                        view.Filter = new Predicate<object>(itemPassesFilter);

                        OnFilteringFinished(this, EventArgs.Empty);
                    })
                );
            }
            else
            {
                executeFilterAction(
                    new Action(() =>
                    {
                        if (view.Filter != null)
                        {
                            view.Filter = null;
                        }

                        OnFilteringFinished(this, EventArgs.Empty);
                    })
                );
            }
        }
        /// <summary>
        /// 執行過濾器動作
        /// </summary>
        /// <param name="action"></param>
        private void executeFilterAction(Action action)
        {
            if (UseBackgroundWorker)
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += delegate (object sender, DoWorkEventArgs e)
                {
                    lock (lockObject)
                    {
                        executeActionUsingDispatcher(action);
                    }
                };

                worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        OnFilteringError(this, new FilteringEventArgs(e.Error));
                    }
                };

                worker.RunWorkerAsync();
            }
            else
            {
                try
                {
                    executeActionUsingDispatcher(action);
                }
                catch (Exception e)
                {
                    OnFilteringError(this, new FilteringEventArgs(e));
                }
            }
        }
        /// <summary>
        /// 使用分派器執行操作
        /// </summary>
        /// <param name="action"></param>
        private void executeActionUsingDispatcher(Action action)
        {
            if (this.CallingThreadDispatcher != null && !this.CallingThreadDispatcher.CheckAccess())
            {
                this.CallingThreadDispatcher.Invoke
                    (
                        new Action(() =>
                        {
                            invoke(action);
                        })
                    );
            }
            else
            {
                invoke(action);
            }
        }
        /// <summary>
        /// 調用
        /// </summary>
        /// <param name="action"></param>
        private static void invoke(Action action)
        {
            System.Diagnostics.Trace.WriteLine("------------------ START APPLAY FILTER ------------------------------");
            Stopwatch sw = Stopwatch.StartNew();

            action.Invoke();

            sw.Stop();
            System.Diagnostics.Trace.WriteLine("TIME: " + sw.ElapsedMilliseconds);
            System.Diagnostics.Trace.WriteLine("------------------ STOP APPLAY FILTER ------------------------------");
        }

        private bool itemPassesFilter(object item)
        {
            return filteredCollectionHashSet.Contains(item);
        }

        #region 幫手
        /// <summary>
        /// 初始化查詢字典
        /// </summary>
        /// <param name="collection">集合</param>
        /// <returns></returns>
        private HashSet<object> initLookupDictionary(IList collection)
        {
            HashSet<object> dictionary;

            if (collection != null)
            {
                dictionary = new HashSet<object>(collection.Cast<object>()/*.ToList()*/);
            }
            else
            {
                dictionary = new HashSet<object>();
            }

            return dictionary;
        }
        #endregion

        #endregion
        #endregion

        #region 進度通知
        /// <summary>
        /// 開始篩選
        /// </summary>
        public event EventHandler<EventArgs> FilteringStarted;
        /// <summary>
        /// 完成篩選
        /// </summary>
        public event EventHandler<EventArgs> FilteringFinished;
        /// <summary>
        /// 篩選錯誤
        /// </summary>
        public event EventHandler<FilteringEventArgs> FilteringError;
        /// <summary>
        /// 在開始過濾時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilteringStarted(object sender, EventArgs e)
        {
            EventHandler<EventArgs> localEvent = FilteringStarted;

            if (localEvent != null) localEvent(sender, e);
        }
        /// <summary>
        /// 過濾完成時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilteringFinished(object sender, EventArgs e)
        {
            EventHandler<EventArgs> localEvent = FilteringFinished;

            if (localEvent != null) localEvent(sender, e);
        }
        /// <summary>
        /// 篩選錯誤時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilteringError(object sender, FilteringEventArgs e)
        {
            EventHandler<FilteringEventArgs> localEvent = FilteringError;

            if (localEvent != null) localEvent(sender, e);
        }
        #endregion
    }
}
