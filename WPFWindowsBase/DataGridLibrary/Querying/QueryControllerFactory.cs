using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WPFWindowsBase.DataGridLibrary.Support;

namespace WPFWindowsBase.DataGridLibrary.Querying
{
    /// <summary>
    /// 查詢群組控制器
    /// </summary>
    public class QueryControllerFactory
    {
        public static QueryController
            GetQueryController(
            System.Windows.Controls.DataGrid dataGrid,
            FilterData filterData, IEnumerable itemsSource)
        {
            QueryController query;

            query = DataGridExtensions.GetDataGridFilterQueryController(dataGrid);

            if (query == null)
            {
                //清除過濾器（如果存在）開始
                System.ComponentModel.ICollectionView view
                    = System.Windows.Data.CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                if (view != null) view.Filter = null;
                //清除過濾器（如果存在）結束

                query = new QueryController();
                DataGridExtensions.SetDataGridFilterQueryController(dataGrid, query);
            }

            query.ColumnFilterData = filterData;
            query.ItemsSource = itemsSource;
            query.CallingThreadDispatcher = dataGrid.Dispatcher;
            query.UseBackgroundWorker = DataGridExtensions.GetUseBackgroundWorkerForFiltering(dataGrid);

            return query;
        }
    }
}
