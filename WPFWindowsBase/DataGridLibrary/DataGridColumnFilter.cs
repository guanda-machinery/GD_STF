using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections;
using WPFWindowsBase.DataGridLibrary.Support;
using System.Reflection;
using WPFWindowsBase.DataGridLibrary.Querying;
using System.Windows.Controls.Primitives;

namespace WPFWindowsBase.DataGridLibrary
{
    /// <summary>
    /// DataGrid列過濾器
    /// </summary>
    public class DataGridColumnFilter : Control
    {
        static DataGridColumnFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnFilter), new FrameworkPropertyMetadata(typeof(DataGridColumnFilter)));
        }

        #region 覆寫
        protected override void OnPropertyChanged(
            DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataGridItemsSourceProperty
                && e.OldValue != e.NewValue
                && AssignedDataGridColumn != null && DataGrid != null && AssignedDataGridColumn is DataGridColumn)
            {
                initialize();

                FilterCurrentData.IsRefresh = true;

                filterCurrentData_FilterChangedEvent(this, EventArgs.Empty);

                FilterCurrentData.FilterChangedEvent -= new EventHandler<EventArgs>(filterCurrentData_FilterChangedEvent);
                FilterCurrentData.FilterChangedEvent += new EventHandler<EventArgs>(filterCurrentData_FilterChangedEvent);
            }

            base.OnPropertyChanged(e);
        }
        #endregion

        #region 屬性
        /// <summary>
        /// 篩選當前數據
        /// </summary>
        public FilterData FilterCurrentData
        {
            get { return (FilterData)GetValue(FilterCurrentDataProperty); }
            set { SetValue(FilterCurrentDataProperty, value); }
        }

        public static readonly DependencyProperty FilterCurrentDataProperty =
            DependencyProperty.Register("FilterCurrentData", typeof(FilterData), typeof(DataGridColumnFilter));
        /// <summary>
        /// 分配的DataGridColumn標題
        /// </summary>
        public DataGridColumnHeader AssignedDataGridColumnHeader
        {
            get { return (DataGridColumnHeader)GetValue(AssignedDataGridColumnHeaderProperty); }
            set { SetValue(AssignedDataGridColumnHeaderProperty, value); }
        }

        public static readonly DependencyProperty AssignedDataGridColumnHeaderProperty =
            DependencyProperty.Register(nameof(AssignedDataGridColumnHeader), typeof(DataGridColumnHeader), typeof(DataGridColumnFilter));
        /// <summary>
        /// 分配的DataGrid列
        /// </summary>
        public DataGridColumn AssignedDataGridColumn
        {
            get { return (DataGridColumn)GetValue(AssignedDataGridColumnProperty); }
            set { SetValue(AssignedDataGridColumnProperty, value); }
        }

        public static readonly DependencyProperty AssignedDataGridColumnProperty =
            DependencyProperty.Register(nameof(AssignedDataGridColumn), typeof(DataGridColumn), typeof(DataGridColumnFilter));

        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        public static readonly DependencyProperty DataGridProperty =
            DependencyProperty.Register(nameof(DataGrid), typeof(DataGrid), typeof(DataGridColumnFilter));

        public IEnumerable DataGridItemsSource
        {
            get { return (IEnumerable)GetValue(DataGridItemsSourceProperty); }
            set { SetValue(DataGridItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty DataGridItemsSourceProperty =
            DependencyProperty.Register(nameof(DataGridItemsSource), typeof(IEnumerable), typeof(DataGridColumnFilter));
        /// <summary>
        /// 正在過濾
        /// </summary>
        public bool IsFilteringInProgress
        {
            get { return (bool)GetValue(IsFilteringInProgressProperty); }
            set { SetValue(IsFilteringInProgressProperty, value); }
        }

        public static readonly DependencyProperty IsFilteringInProgressProperty =
            DependencyProperty.Register(nameof(IsFilteringInProgress), typeof(bool), typeof(DataGridColumnFilter));
        /// <summary>
        /// 篩選器類型
        /// </summary>
        public FilterType FilterType { get { return FilterCurrentData != null ? FilterCurrentData.Type : FilterType.Text; } }

        /// <summary>
        /// IsText篩選器控件
        /// </summary>
        public bool IsTextFilterControl
        {
            get { return (bool)GetValue(IsTextFilterControlProperty); }
            set { SetValue(IsTextFilterControlProperty, value); }
        }

        public static readonly DependencyProperty IsTextFilterControlProperty =
            DependencyProperty.Register(nameof(IsTextFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是數字過濾器控件
        /// </summary>
        public bool IsNumericFilterControl
        {
            get { return (bool)GetValue(IsNumericFilterControlProperty); }
            set { SetValue(IsNumericFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsNumericFilterControlProperty =
            DependencyProperty.Register(nameof(IsNumericFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 過濾器控制之間是數字
        /// </summary>
        public bool IsNumericBetweenFilterControl
        {
            get { return (bool)GetValue(IsNumericBetweenFilterControlProperty); }
            set { SetValue(IsNumericBetweenFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsNumericBetweenFilterControlProperty =
            DependencyProperty.Register(nameof(IsNumericBetweenFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是<see cref="bool"/>過濾器控制
        /// </summary>
        public bool IsBooleanFilterControl
        {
            get { return (bool)GetValue(IsBooleanFilterControlProperty); }
            set { SetValue(IsBooleanFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsBooleanFilterControlProperty =
            DependencyProperty.Register(nameof(IsBooleanFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是列表過濾器控件
        /// </summary>
        public bool IsListFilterControl
        {
            get { return (bool)GetValue(IsListFilterControlProperty); }
            set { SetValue(IsListFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsListFilterControlProperty =
            DependencyProperty.Register(nameof(IsListFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是日期時間過濾器控件
        /// </summary>
        public bool IsDateTimeFilterControl
        {
            get { return (bool)GetValue(IsDateTimeFilterControlProperty); }
            set { SetValue(IsDateTimeFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsDateTimeFilterControlProperty =
            DependencyProperty.Register(nameof(IsDateTimeFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是過濾器控件之間的日期時間
        /// </summary>
        public bool IsDateTimeBetweenFilterControl
        {
            get { return (bool)GetValue(IsDateTimeBetweenFilterControlProperty); }
            set { SetValue(IsDateTimeBetweenFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsDateTimeBetweenFilterControlProperty =
            DependencyProperty.Register("IsDateTimeBetweenFilterControl", typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// 是第一個過濾器控制
        /// </summary>
        public bool IsFirstFilterControl
        {
            get { return (bool)GetValue(IsFirstFilterControlProperty); }
            set { SetValue(IsFirstFilterControlProperty, value); }
        }
        public static readonly DependencyProperty IsFirstFilterControlProperty =
            DependencyProperty.Register(nameof(IsFirstFilterControl), typeof(bool), typeof(DataGridColumnFilter));

        /// <summary>
        /// IsControl已初始化
        /// </summary>
        public bool IsControlInitialized
        {
            get { return (bool)GetValue(IsControlInitializedProperty); }
            set { SetValue(IsControlInitializedProperty, value); }
        }
        public static readonly DependencyProperty IsControlInitializedProperty =
            DependencyProperty.Register(nameof(IsControlInitialized), typeof(bool), typeof(DataGridColumnFilter));
        #endregion

        #region 初始化
        private void initialize()
        {
            if (DataGridItemsSource != null && AssignedDataGridColumn != null && DataGrid != null)
            {
                initFilterData();

                initControlType();

                handleListFilterType();

                hookUpCommands();

                IsControlInitialized = true;
            }
        }
        /// <summary>
        /// 初始化過濾器數據
        /// </summary>
        private void initFilterData()
        {
            if (FilterCurrentData == null || !FilterCurrentData.IsTypeInitialized)
            {
                string valuePropertyBindingPath = getValuePropertyBindingPath(AssignedDataGridColumn);

                bool typeInitialized;

                Type valuePropertyType = getValuePropertyType(
                    valuePropertyBindingPath, getItemSourceElementType(out typeInitialized));

                FilterType filterType = getFilterType(
                    valuePropertyType,
                    isComboDataGridColumn(),
                    isBetweenType());

                FilterOperator filterOperator = FilterOperator.Undefined;

                string queryString = String.Empty;
                string queryStringTo = String.Empty;

                FilterCurrentData = new FilterData(
                    filterOperator,
                    filterType,
                    valuePropertyBindingPath,
                    valuePropertyType,
                    queryString,
                    queryStringTo,
                    typeInitialized,
                    DataGridColumnExtensions.GetIsCaseSensitiveSearch(AssignedDataGridColumn));
            }
        }
        /// <summary>
        /// 初始化ControlType
        /// </summary>
        private void initControlType()
        {
            IsFirstFilterControl = false;

            IsTextFilterControl = false;
            IsNumericFilterControl = false;
            IsBooleanFilterControl = false;
            IsListFilterControl = false;
            IsDateTimeFilterControl = false;

            IsNumericBetweenFilterControl = false;
            IsDateTimeBetweenFilterControl = false;

            if (FilterType == FilterType.Text)
            {
                IsTextFilterControl = true;
            }
            else if (FilterType == FilterType.Numeric)
            {
                IsNumericFilterControl = true;
            }
            else if (FilterType == FilterType.Boolean)
            {
                IsBooleanFilterControl = true;
            }
            else if (FilterType == FilterType.List)
            {
                IsListFilterControl = true;
            }
            else if (FilterType == FilterType.DateTime)
            {
                IsDateTimeFilterControl = true;
            }
            else if (FilterType == FilterType.NumericBetween)
            {
                IsNumericBetweenFilterControl = true;
            }
            else if (FilterType == FilterType.DateTimeBetween)
            {
                IsDateTimeBetweenFilterControl = true;
            }
        }
        /// <summary>
        /// 處理列表過濾器類型
        /// </summary>
        private void handleListFilterType()
        {
            if (FilterCurrentData.Type == FilterType.List)
            {
                ComboBox comboBox = this.Template.FindName("PART_ComboBoxFilter", this) as ComboBox;
                DataGridComboBoxColumn column = AssignedDataGridColumn as DataGridComboBoxColumn;

                if (comboBox != null && column != null)
                {

                    if (DataGridComboBoxExtensions.GetIsTextFilter(column))
                    {
                        FilterCurrentData.Type = FilterType.Text;
                        initControlType();
                    }
                    else //list filter type
                    {
                        Binding columnItemsSourceBinding = null;
                        columnItemsSourceBinding = BindingOperations.GetBinding(column, DataGridComboBoxColumn.ItemsSourceProperty);

                        if (columnItemsSourceBinding == null)
                        {
                            System.Windows.Setter styleSetter = column.EditingElementStyle.Setters.First(s => ((System.Windows.Setter)s).Property == DataGridComboBoxColumn.ItemsSourceProperty) as System.Windows.Setter;
                            if (styleSetter != null)
                                columnItemsSourceBinding = styleSetter.Value as Binding;
                        }

                        comboBox.DisplayMemberPath = column.DisplayMemberPath;
                        comboBox.SelectedValuePath = column.SelectedValuePath;

                        if (columnItemsSourceBinding != null)
                        {
                            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, columnItemsSourceBinding);
                        }

                        comboBox.RequestBringIntoView
                            += new RequestBringIntoViewEventHandler(setComboBindingAndHanldeUnsetValue);
                    }
                }
            }
        }
        /// <summary>
        /// 設置組合綁定並處理未設置的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setComboBindingAndHanldeUnsetValue(object sender, RequestBringIntoViewEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            DataGridComboBoxColumn column = AssignedDataGridColumn as DataGridComboBoxColumn;

            if (column.ItemsSource == null)
            {
                if (combo.ItemsSource != null)
                {
                    IList list = combo.ItemsSource.Cast<object>().ToList();

                    if (list.Count > 0 && list[0] != DependencyProperty.UnsetValue)
                    {
                        combo.RequestBringIntoView -=
                            new RequestBringIntoViewEventHandler(setComboBindingAndHanldeUnsetValue);

                        list.Insert(0, DependencyProperty.UnsetValue);

                        combo.DisplayMemberPath = column.DisplayMemberPath;
                        combo.SelectedValuePath = column.SelectedValuePath;

                        combo.ItemsSource = list;
                    }
                }
            }
            else
            {
                combo.RequestBringIntoView -=
                    new RequestBringIntoViewEventHandler(setComboBindingAndHanldeUnsetValue);

                IList comboList = null;
                IList columnList = null;

                if (combo.ItemsSource != null)
                {
                    comboList = combo.ItemsSource.Cast<object>().ToList();
                }

                columnList = column.ItemsSource.Cast<object>().ToList();

                if (comboList == null ||
                    (columnList.Count > 0 && columnList.Count + 1 != comboList.Count))
                {
                    columnList = column.ItemsSource.Cast<object>().ToList();
                    columnList.Insert(0, DependencyProperty.UnsetValue);

                    combo.ItemsSource = columnList;
                }

                combo.RequestBringIntoView +=
                    new RequestBringIntoViewEventHandler(setComboBindingAndHanldeUnsetValue);
            }
        }
        /// <summary>
        /// 獲取值屬性綁定路徑
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string getValuePropertyBindingPath(DataGridColumn column)
        {
            string path = String.Empty;

            if (column is DataGridBoundColumn)
            {
                DataGridBoundColumn bc = column as DataGridBoundColumn;
                path = (bc.Binding as Binding).Path.Path;
            }
            else if (column is DataGridTemplateColumn)
            {
                DataGridTemplateColumn tc = column as DataGridTemplateColumn;

                object templateContent = tc.CellTemplate.LoadContent();

                if (templateContent != null && templateContent is TextBlock)
                {
                    TextBlock block = templateContent as TextBlock;

                    BindingExpression binding = block.GetBindingExpression(TextBlock.TextProperty);

                    path = binding.ParentBinding.Path.Path;
                }
            }
            else if (column is DataGridComboBoxColumn)
            {
                DataGridComboBoxColumn comboColumn = column as DataGridComboBoxColumn;

                path = null;

                Binding binding = ((comboColumn.SelectedValueBinding) as Binding);

                if (binding == null)
                {
                    binding = ((comboColumn.SelectedItemBinding) as Binding);
                }

                if (binding == null)
                {
                    binding = comboColumn.SelectedValueBinding as Binding;
                }

                if (binding != null)
                {
                    path = binding.Path.Path;
                }

                if (comboColumn.SelectedItemBinding != null && comboColumn.SelectedValueBinding == null)
                {
                    if (path != null && path.Trim().Length > 0)
                    {
                        if (DataGridComboBoxExtensions.GetIsTextFilter(comboColumn))
                        {
                            path += "." + comboColumn.DisplayMemberPath;
                        }
                        else
                        {
                            path += "." + comboColumn.SelectedValuePath;
                        }
                    }
                }
            }

            return path;
        }
        /// <summary>
        /// 獲取值屬性類型
        /// </summary>
        /// <param name="path"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        private Type getValuePropertyType(string path, Type elementType)
        {
            Type type = typeof(object);

            if (elementType != null)
            {
                string[] properties = path.Split(".".ToCharArray()[0]);

                PropertyInfo pi = null;

                if (properties.Length == 1)
                {
                    pi = elementType.GetProperty(path);
                }
                else
                {
                    pi = elementType.GetProperty(properties[0]);

                    for (int i = 1; i < properties.Length; i++)
                    {
                        if (pi != null)
                        {
                            pi = pi.PropertyType.GetProperty(properties[i]);
                        }
                    }
                }


                if (pi != null)
                {
                    type = pi.PropertyType;
                }
            }

            return type;
        }
        /// <summary>
        /// 獲取項目來源元素類型
        /// </summary>
        /// <param name="typeInitialized"></param>
        /// <returns></returns>
        private Type getItemSourceElementType(out bool typeInitialized)
        {
            typeInitialized = false;

            Type elementType = null;

            IList l = (DataGridItemsSource as IList);

            if (l != null && l.Count > 0)
            {
                object obj = l[0];

                if (obj != null)
                {
                    elementType = l[0].GetType();
                    typeInitialized = true;
                }
                else
                {
                    elementType = typeof(object);
                }
            }
            if (l == null)
            {
                ListCollectionView lw = (DataGridItemsSource as ListCollectionView);

                if (lw != null && lw.Count > 0)
                {
                    object obj = lw.CurrentItem;

                    if (obj != null)
                    {
                        elementType = lw.CurrentItem.GetType();
                        typeInitialized = true;
                    }
                    else
                    {
                        elementType = typeof(object);
                    }
                }
            }

            return elementType;
        }
        /// <summary>
        /// 取得過濾類型
        /// </summary>
        /// <param name="valuePropertyType"></param>
        /// <param name="isAssignedDataGridColumnComboDataGridColumn"></param>
        /// <param name="isBetweenType"></param>
        /// <returns></returns>
        private FilterType getFilterType(
            Type valuePropertyType,
            bool isAssignedDataGridColumnComboDataGridColumn,
            bool isBetweenType)
        {
            FilterType filterType;

            if (isAssignedDataGridColumnComboDataGridColumn)
            {
                filterType = FilterType.List;
            }
            else if (valuePropertyType == typeof(Boolean) || valuePropertyType == typeof(Nullable<Boolean>))
            {
                filterType = FilterType.Boolean;
            }
            else if (valuePropertyType == typeof(SByte) || valuePropertyType == typeof(Nullable<SByte>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Byte) || valuePropertyType == typeof(Nullable<Byte>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Int16) || valuePropertyType == typeof(Nullable<Int16>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(UInt16) || valuePropertyType == typeof(Nullable<UInt16>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Int32) || valuePropertyType == typeof(Nullable<Int32>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(UInt32) || valuePropertyType == typeof(Nullable<UInt32>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Int64) || valuePropertyType == typeof(Nullable<Int64>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Single) || valuePropertyType == typeof(Nullable<Single>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Int64) || valuePropertyType == typeof(Nullable<Int64>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Decimal) || valuePropertyType == typeof(Nullable<Decimal>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(float) || valuePropertyType == typeof(Nullable<float>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Double) || valuePropertyType == typeof(Nullable<Double>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(Int64) || valuePropertyType == typeof(Nullable<Int64>))
            {
                filterType = FilterType.Numeric;
            }
            else if (valuePropertyType == typeof(DateTime) || valuePropertyType == typeof(Nullable<DateTime>))
            {
                filterType = FilterType.DateTime;
            }
            else
            {
                filterType = FilterType.Text;
            }

            if (filterType == FilterType.Numeric && isBetweenType)
            {
                filterType = FilterType.NumericBetween;
            }
            else if (filterType == FilterType.DateTime && isBetweenType)
            {
                filterType = FilterType.DateTimeBetween;
            }

            return filterType;
        }
        /// <summary>
        /// 是<see cref="DataGridComboBoxColumn"/>
        /// </summary>
        /// <returns></returns>
        private bool isComboDataGridColumn()
        {
            return AssignedDataGridColumn is DataGridComboBoxColumn;
        }
        /// <summary>
        /// 是之間類型
        /// </summary>
        /// <returns></returns>
        private bool isBetweenType()
        {
            return DataGridColumnExtensions.GetIsBetweenFilterControl(AssignedDataGridColumn);
        }
        /// <summary>
        /// 連接命令
        /// </summary>
        private void hookUpCommands()
        {
            if (DataGridExtensions.GetClearFilterCommand(DataGrid) == null)
            {
                DataGridExtensions.SetClearFilterCommand(
                    DataGrid, new DataGridFilterCommand(clearQuery));
            }
        }
        #endregion

        #region 查詢方式
        /// <summary>
        /// 過濾當前數據FilterChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void filterCurrentData_FilterChangedEvent(object sender, EventArgs e)
        {
            if (DataGrid != null)
            {
                QueryController query = QueryControllerFactory.GetQueryController(
                    DataGrid, FilterCurrentData, DataGridItemsSource);

                addFilterStateHandlers(query);

                query.DoQuery();

                IsFirstFilterControl = query.IsCurentControlFirstControl;
            }
        }
        /// <summary>
        /// 清除過濾器
        /// </summary>
        /// <param name="parameter"></param>
        private void clearQuery(object parameter)
        {
            if (DataGrid != null)
            {
                QueryController query = QueryControllerFactory.GetQueryController(
                    DataGrid, FilterCurrentData, DataGridItemsSource);

                query.ClearFilter();
            }
        }
        /// <summary>
        /// 加入過濾器狀態處理程序
        /// </summary>
        /// <param name="query"></param>
        private void addFilterStateHandlers(QueryController query)
        {
            query.FilteringStarted -= new EventHandler<EventArgs>(query_FilteringStarted);
            query.FilteringFinished -= new EventHandler<EventArgs>(query_FilteringFinished);

            query.FilteringStarted += new EventHandler<EventArgs>(query_FilteringStarted);
            query.FilteringFinished += new EventHandler<EventArgs>(query_FilteringFinished);
        }
        /// <summary>
        /// 查詢過濾完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void query_FilteringFinished(object sender, EventArgs e)
        {
            if (FilterCurrentData.Equals((sender as QueryController).ColumnFilterData))
            {
                this.IsFilteringInProgress = false;
            }
        }
        /// <summary>
        /// 查詢過濾完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void query_FilteringStarted(object sender, EventArgs e)
        {
            if (FilterCurrentData.Equals((sender as QueryController).ColumnFilterData))
            {
                this.IsFilteringInProgress = true;
            }
        }
        #endregion
    }
}