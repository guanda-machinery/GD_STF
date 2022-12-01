using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using DevExpress.Data.Extensions;
using WPFSTD105.Model;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using WPFSTD105.ViewModel;
using GD_STD.Enum;
using DevExpress.Xpf.Core;
using System.Threading;

namespace WPFSTD105
{
    public class ExcelBuyService
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcelBuyService()
        {
        }
        #region 私有屬性
        private devDept.Eyeshot.Model _BufferModel { get; set; }
        #endregion

        /// <summary>
        /// 啟動畫面管理器
        /// </summary>
        public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.CreateWaitIndicator();

        public static void CreateModelOverView(string path, ModelExt model) 
        {
            SpreadsheetControl spreadSheet = new SpreadsheetControl();
            List<int> rowList = new List<int>();
            IWorkbook book = spreadSheet.Document; //提供對控件中加載的工作簿的訪問
            #region  model              
            Worksheet sheet = book.Worksheets.Add("dm file"); //創建一個新工作表並將其附加到集合的末尾
            book.Worksheets.ActiveWorksheet = sheet;
            int row = 0;
            int column = 0;
            int zeroIndex = 0, firstIndex = 0, secondIndex = 0, thirdIndex = 0;
            #region Blocks
            List<object> modelBlockList = new List<object>();
            List<object> modelEntityList = new List<object>();
            Entity[] _entities = new Entity[model.Entities.Count];
            Block[] _blocks = new Block[model.Blocks.Count];
            model.Entities.CopyTo(_entities, 0);
            model.Blocks.CopyTo(_blocks, 0);
            modelBlockList.Add(_blocks);
            modelEntityList.Add(_entities);
            sheet.Cells[row++, column].Value = "model.Blocks";
            firstIndex = 0;
            
            for (int i = 0; i < modelBlockList.Count; i++)
            {
                var blocks = (Block[])modelBlockList[i];
                #region 第一層
                foreach (Block block in blocks)
                {
                    switch (block.GetType().Name)
                    {
                        case "Block":
                            Block b = (Block)block;
                            sheet.Cells[row, column++].Value = $"1-1-0model.Blocks[{firstIndex}]";
                            sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                            sheet.Cells[row++, column++].Value = $"{b.Name}";
                            column = 0;
                            break;
                        case "Steel3DBlock":
                            Steel3DBlock sdb = (Steel3DBlock)block;
                            sheet.Cells[row, column++].Value = $"1-1-0model.Blocks[{firstIndex}]";
                            sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                            sheet.Cells[row, column++].Value = $"{sdb.Name}";
                            sheet.Cells[row++, column++].Value = $"{sdb.Units}";
                            column = 0;
                            break;
                        default:
                            sheet.Cells[row, column++].Value = $"*1-1-0model.Blocks[{firstIndex}]";
                            sheet.Cells[row, column++].Value = $"例外型別";
                            sheet.Cells[row++, column++].Value = $"{block.GetType().Name}";
                            column = 0;
                            break;
                    }
                    #region 第二層
                    secondIndex = 0;
                    foreach (Entity entities in (EntityList)block.Entities)
                    {
                        switch (entities.GetType().Name)
                        {
                            case "BlockReference":
                                BlockReference br = (BlockReference)entities;
                                sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{br.BlockName}";
                                column = 0;
                                break;
                            case "Line":
                                Line line = (Line)entities;
                                sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                sheet.Cells[row, column++].Value = $"StartPoint:{line.StartPoint.X},{line.StartPoint.Y},{line.StartPoint.Z}";
                                sheet.Cells[row++, column++].Value = $"EndPoint:{line.EndPoint.X},{line.EndPoint.Y},{line.EndPoint.Z}";
                                column = 0;
                                break;
                            //case "Block":
                            //    Block block = (Block)entities;
                            //    break;
                            case "Circle":
                                Circle circle = (Circle)entities;
                                sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{circle.Diameter}";///Center
                                column = 0;
                                break;
                            case "Mesh":
                                Mesh mesh = (Mesh)entities;
                                sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                sheet.Cells[row, column++].Value = (mesh.Vertices.Count() > 0 ? "有Vertices" : "無Vertices");
                                sheet.Cells[row, column++].Value = $"{mesh.MaterialName}";
                                sheet.Cells[row, column++].Value = $"{mesh.Faces}";
                                sheet.Cells[row++, column++].Value = $"{mesh.LineTypeName}";
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"*1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                sheet.Cells[row, column++].Value = $"例外型別";
                                sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{entities.GetType().Name}";
                                column = 0;
                                break;
                        }
                        #region 第三層
                        thirdIndex = 0;
                        if (entities.EntityData == null)
                        {
                            switch (entities.GetType().Name)
                            {
                                case "Block":
                                case "Line":
                                case "SteelAttr":
                                case "BoltAttr":
                                case "BlockReference":
                                    sheet.Cells[row, column++].Value = $"1-3model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData = null";
                                    sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                    sheet.Cells[row += 2, column++].Value = $"{entities.GetType().Name}";
                                    column = 0;
                                    break;
                                default:
                                    sheet.Cells[row, column++].Value = $"*1-3model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData = null";
                                    sheet.Cells[row, column++].Value = $"例外型別";
                                    sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                    sheet.Cells[row += 2, column++].Value = $"{entities.GetType().Name}";
                                    column = 0;
                                    break;
                            }
                            //sheet.Cells[row++, column++].Value = $"{((BlockReference)entities).BlockName}無EntityData";
                            column = 0;
                        }
                        else
                        {
                            switch (entities.EntityData.GetType().Name)
                            {
                                case "BoltAttr":
                                    BoltAttr ba = (BoltAttr)entities.EntityData;//Dia Model Type Face
                                    sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = entities.EntityData.GetType().Name;
                                    sheet.Cells[row, column++].Value = ba.GUID.ToString();
                                    sheet.Cells[row, column++].Value = ba.BlockName;
                                    sheet.Cells[row, column++].Value = ba.X;
                                    sheet.Cells[row, column++].Value = ba.Y;
                                    sheet.Cells[row, column++].Value = ba.Z;
                                    sheet.Cells[row, column++].Value = ba.Dia;
                                    sheet.Cells[row, column++].Value = $"{ba.Mode}";
                                    sheet.Cells[row += 2, column++].Value = ba.Type.ToString();
                                    column = 0;
                                    break;
                                case "SteelAttr":
                                    SteelAttr sa = (SteelAttr)entities.EntityData;
                                    sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = sa.GetType().Name;
                                    sheet.Cells[row, column++].Value = sa.GUID.ToString();
                                    sheet.Cells[row, column++].Value = sa.AsseNumber;
                                    sheet.Cells[row, column++].Value = sa.PartNumber;
                                    sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                                    sheet.Cells[row, column++].Value = sa.Profile;
                                    sheet.Cells[row, column++].Value = sa.Material;
                                    sheet.Cells[row, column++].Value = sa.Number;
                                    sheet.Cells[row, column++].Value = sa.Creation;
                                    sheet.Cells[row, column++].Value = sa.Revise;
                                    sheet.Cells[row, column++].Value = sa.ExclamationMark;
                                    sheet.Cells[row, column++].Value = sa.Lock;
                                    sheet.Cells[row, column++].Value = sa.H;
                                    sheet.Cells[row, column++].Value = sa.W;
                                    sheet.Cells[row, column++].Value = sa.Kg;
                                    sheet.Cells[row, column++].Value = sa.Length;
                                    sheet.Cells[row, column++].Value = sa.Weight;
                                    sheet.Cells[row, column++].Value = sa.t1;
                                    sheet.Cells[row, column++].Value = sa.t2;
                                    sheet.Cells[row, column++].Value = sa.Phase;
                                    sheet.Cells[row, column++].Value = sa.ShippingNumber;
                                    sheet.Cells[row, column++].Value = sa.Title1;
                                    sheet.Cells[row += 2, column++].Value = sa.Title2;
                                    column = 0;
                                    break;
                                case "Mesh":
                                    Mesh mesh = (Mesh)entities.EntityData;
                                    break;
                                case "GroupBoltsAttr":
                                    GroupBoltsAttr gba = (GroupBoltsAttr)entities.EntityData;
                                    sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                    sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                    sheet.Cells[row, column++].Value = gba.X.ToString();
                                    sheet.Cells[row, column++].Value = gba.Y.ToString();
                                    sheet.Cells[row, column++].Value = gba.Z.ToString();
                                    sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                    sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                    sheet.Cells[row, column++].Value = ((AXIS_MODE)gba.Mode).ToString();
                                    sheet.Cells[row += 2, column++].Value = gba.Count.ToString();
                                    column = 0;
                                    break;
                                default:
                                    sheet.Cells[row, column++].Value = $"*1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = $"例外型別";
                                    sheet.Cells[row, column++].Value = $"{block.GetType().Name}+";
                                    sheet.Cells[row, column++].Value = $"{entities.GetType().Name}+";
                                    sheet.Cells[row += 2, column++].Value = $"{entities.EntityData.GetType().Name}";
                                    column = 0;
                                    break;
                            }
                        }
                        thirdIndex++;
                        #endregion
                        secondIndex++;
                    }
                    #endregion
                    firstIndex++;
                }
                #endregion
            }
            #endregion
            #region Entities
            firstIndex = 0; secondIndex = 0; thirdIndex = 0;
            sheet.Cells[row++, column].Value = "model.Entities";
            for (int i = 0; i < modelEntityList.Count; i++)
            {
                Entity[] entities = (Entity[])modelEntityList[i];
                #region 第一層
                firstIndex = 0;
                foreach (var entity in entities)
                {
                    switch (entity.GetType().Name)
                    {
                        case "BlockReference":
                            BlockReference br = (BlockReference)entity;
                            sheet.Cells[row, column++].Value = $"2-1-0model.Entities[{firstIndex}]";
                            sheet.Cells[row, column++].Value = $"{entity.GetType().Name}";
                            sheet.Cells[row++, column++].Value = $"{br.BlockName}";
                            column = 0;
                            break;
                        default:
                            sheet.Cells[row, column++].Value = $"*2-1-0model.Entities[{firstIndex}]";
                            sheet.Cells[row++, column++].Value = $"例外型別";
                            sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                            column = 0;
                            break;
                    }
                    #region 第二層
                    secondIndex = 0;
                    if (entity.EntityData == null)
                    {
                        switch (entity.GetType().Name)
                        {
                            case "BlockReference":
                                sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData = null";
                                sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"*2-2-0model.Entities[{firstIndex}].EntityData = null";
                                sheet.Cells[row++, column++].Value = $"例外型別";
                                sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                                column = 0;
                                break;
                        }
                        //sheet.Cells[row++, column++].Value = $"{((BlockReference)entities).BlockName}無EntityData";
                        column = 0;
                    }
                    else
                    {
                        switch (entity.EntityData.GetType().Name)
                        {
                            case "GroupBoltsAttr":
                                GroupBoltsAttr gba = (GroupBoltsAttr)entity.EntityData;
                                sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData";
                                sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                sheet.Cells[row, column++].Value = $"{gba.Face}";
                                sheet.Cells[row, column++].Value = $"{gba.Type}";
                                sheet.Cells[row, column++].Value = gba.X.ToString();
                                sheet.Cells[row, column++].Value = gba.Y.ToString();
                                sheet.Cells[row, column++].Value = gba.Z.ToString();
                                sheet.Cells[row, column++].Value = gba.dX;
                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dXs.ToArray());
                                sheet.Cells[row, column++].Value = gba.dY;
                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dYs.ToArray());
                                sheet.Cells[row, column++].Value = $"{gba.Count}";
                                sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                sheet.Cells[row += 2, column++].Value = ((AXIS_MODE)gba.Mode).ToString();//Count
                                column = 0;
                                break;
                            case "BlockReference":
                                break;
                            case "SteelAttr":
                                SteelAttr sa = (SteelAttr)entity.EntityData;
                                sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData";
                                sheet.Cells[row, column++].Value = sa.GetType().Name;
                                sheet.Cells[row, column++].Value = sa.GUID.ToString();
                                sheet.Cells[row, column++].Value = sa.AsseNumber;
                                sheet.Cells[row, column++].Value = sa.PartNumber;
                                sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                                sheet.Cells[row, column++].Value = sa.Profile;
                                sheet.Cells[row, column++].Value = sa.Material;
                                sheet.Cells[row, column++].Value = sa.Number;
                                sheet.Cells[row, column++].Value = sa.Creation;
                                sheet.Cells[row, column++].Value = sa.Revise;
                                sheet.Cells[row, column++].Value = sa.ExclamationMark;
                                sheet.Cells[row, column++].Value = sa.Lock;
                                sheet.Cells[row, column++].Value = sa.H;
                                sheet.Cells[row, column++].Value = sa.W;
                                sheet.Cells[row, column++].Value = sa.Kg;
                                sheet.Cells[row, column++].Value = sa.Length;
                                sheet.Cells[row, column++].Value = sa.Weight;
                                sheet.Cells[row, column++].Value = sa.t1;
                                sheet.Cells[row, column++].Value = sa.t2;
                                sheet.Cells[row, column++].Value = sa.Phase;
                                sheet.Cells[row, column++].Value = sa.ShippingNumber;
                                sheet.Cells[row, column++].Value = sa.Title1;
                                sheet.Cells[row += 2, column++].Value = sa.Title2;
                                column = 0;
                                break;
                            default://BlockReference
                                sheet.Cells[row, column++].Value = $"*2-2-0model.Entities[{firstIndex}].EntityData";
                                sheet.Cells[row, column++].Value = $"例外型別";
                                sheet.Cells[row, column++].Value = entity.GetType().Name;
                                sheet.Cells[row += 2, column++].Value = entity.EntityData.GetType().Name;
                                column = 0;
                                break;
                        }
                    }
                    #endregion
                    secondIndex++;
                }
                #endregion
                firstIndex++;
            }
            #endregion
            #endregion

            book.BeginUpdate();
            book.SaveDocument(path, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
        }
        
        /// <summary>
        /// 檔案總覽
        /// </summary>
        /// <param name="path"></param>
        public void CreateFileOverView(string path,
            List<object> modelBlockList,
            List<object> modelEntityList,
            List<string> errorGUID)
        {
            //List<Block> blockList = (List<Block>)modelBlockList;
            //List<Entity> entityList = (List<Entity>)modelEntityList;
            ScreenManager.Show(inputBlock: InputBlockMode.None, timeout: 100);

            #region 資料取得
            ScreenManager.ViewModel.Status = $"取得資訊中 ...";
            _BufferModel = new devDept.Eyeshot.Model();
            _BufferModel.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            _BufferModel.InitializeViewports();
            _BufferModel.renderContext = new devDept.Graphics.D3DRenderContextWPF(new System.Drawing.Size(100, 100), new devDept.Graphics.ControlData());
            //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
            ApplicationVM appVM = new ApplicationVM();
            STDSerialization ser = new STDSerialization();
            TypeSettingVM tsVM = new TypeSettingVM();
            // 取得dm檔與零件之對應
            ObservableCollection<DataCorrespond> dataCorrespond = ser.GetDataCorrespond();
            // 取得構件資訊
            ObservableCollection<SteelAssembly> assemblies = ser.GetGZipAssemblies();

            //取得零件資訊
            Dictionary<string, ObservableCollection<SteelPart>> steelPart = ser.GetPart();

            // 取得孔群資訊
            Dictionary<string, ObservableCollection<SteelBolts>> steelBolts = ser.GeBolts();

            // 取得排版資訊
            ObservableCollection<MaterialDataView> materialDataViews = ser.GetMaterialDataView();

            // 取得排版零件資訊           
            ObservableCollection<TypeSettingDataView> DataViews = tsVM.LoadDataViews();

            // 取得切割線設定資訊
            ObservableCollection<SteelCutSetting> cutSettingList = ser.GetCutSettingList();

            // 排版DM



            ScreenManager.ViewModel.Status = $"資訊已取得 ...";
            //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
            #endregion

            SpreadsheetControl spreadSheet = new SpreadsheetControl();
            List<int> rowList = new List<int>();
            IWorkbook book = spreadSheet.Document; //提供對控件中加載的工作簿的訪問
            try
            {
                #region MyRegion
                ScreenManager.ViewModel.Status = $"產生文件 ...DataCorrespond";
                //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                #region DataCorrespond
                Worksheet sheet = book.Worksheets.Add("DataCorrespond"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                ReadOnlyCollection<DataCorrespond> dataCorrespondDataViews = new ReadOnlyCollection<DataCorrespond>(dataCorrespond);
                int row = 0;
                int column = 0;
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "編號";
                sheet.Cells[row, column++].Value = "DataName";
                sheet.Cells[row, column++].Value = "鋼材類型";
                sheet.Cells[row, column++].Value = "oPoint";
                sheet.Cells[row, column++].Value = "vPoint";
                sheet.Cells[row++, column++].Value = "uPoint";
                #endregion
                #region 欄位塞值
                foreach (DataCorrespond item in dataCorrespondDataViews)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = item.Number;
                    sheet.Cells[row, column++].Value = item.DataName;
                    sheet.Cells[row, column++].Value = item.Type.ToString();
                    int tempRow = row;
                    foreach (NcPoint3D op in item.oPoint)
                    {
                        sheet.Cells[row++, column].Value = $"{op.X}\t{op.Y}\t{op.Z}";
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (NcPoint3D vp in item.vPoint)
                    {
                        sheet.Cells[row++, column].Value = $"{vp.X}\t{vp.Y}\t{vp.Z}";
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (NcPoint3D up in item.uPoint)
                    {
                        sheet.Cells[row++, column].Value = $"{up.X}\t{up.Y}\t{up.Z}";
                    }
                    rowList.Add(row);
                    row = rowList.Max() + 1;
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                ScreenManager.ViewModel.Status = $"產生文件 ...Assemblies";
                //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                #region Assemblies
                sheet = book.Worksheets.Add("Assemblies"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                ReadOnlyCollection<SteelAssembly> assembliesDataViews = new ReadOnlyCollection<SteelAssembly>(assemblies);
                row = 0;
                column = 0;
                rowList = new List<int>();
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "GUID";
                sheet.Cells[row, column++].Value = "構件編號";
                sheet.Cells[row, column++].Value = "數量";
                sheet.Cells[row, column++].Value = "建立日期";
                sheet.Cells[row, column++].Value = "修改日期";
                sheet.Cells[row, column++].Value = "名稱";
                sheet.Cells[row, column++].Value = "是否為Tekla文件";
                sheet.Cells[row, column++].Value = "長";
                sheet.Cells[row, column++].Value = "高度";
                sheet.Cells[row, column++].Value = "寬度";
                sheet.Cells[row, column++].Value = "圖面狀態";
                sheet.Cells[row, column++].Value = "構件ID";
                sheet.Cells[row, column++].Value = "構件Phase";
                sheet.Cells[row, column++].Value = "構件位置";
                sheet.Cells[row, column++].Value = "運輸說明";
                sheet.Cells[row++, column++].Value = "運輸號碼";
                #endregion
                #region 欄位塞值
                foreach (SteelAssembly item in assembliesDataViews)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = item.GUID.ToString();
                    sheet.Cells[row, column++].Value = item.Number;
                    sheet.Cells[row, column++].Value = item.Count;
                    sheet.Cells[row, column++].Value = item.Creation;
                    sheet.Cells[row, column++].Value = item.Revise;
                    sheet.Cells[row, column++].Value = item.DrawingName;
                    sheet.Cells[row, column++].Value = item.IsTekla;
                    sheet.Cells[row, column++].Value = item.Length;
                    sheet.Cells[row, column++].Value = item.H;
                    sheet.Cells[row, column++].Value = item.W;
                    sheet.Cells[row, column++].Value = $"{item.State}";
                    int tempRow = row;
                    foreach (var id in item.ID)
                    {
                        sheet.Cells[row++, column].Value = $"{id}";
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (var phase in item.Phase)
                    {
                        sheet.Cells[row++, column].Value = $"{phase}";
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (var position in item.Position)
                    {
                        sheet.Cells[row++, column].Value = $"{position}";
                    }
                    row = tempRow;
                    column++;
                    foreach (var shippingDescription in item.ShippingDescription)
                    {
                        sheet.Cells[row++, column].Value = $"{shippingDescription}";
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (var shippingNumber in item.ShippingNumber)
                    {
                        sheet.Cells[row++, column].Value = $"{shippingNumber}";
                    }
                    rowList.Add(row);
                    row = rowList.Max() + 1;
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                ScreenManager.ViewModel.Status = $"產生文件 ...SteelPart";
                //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                #region SteelPart
                sheet = book.Worksheets.Add("SteelPart"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                rowList = new List<int>();
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "GUID";
                sheet.Cells[row, column++].Value = "零件狀況";
                sheet.Cells[row, column++].Value = "零件編號";
                sheet.Cells[row, column++].Value = "名稱";
                sheet.Cells[row, column++].Value = "斷面規格";
                sheet.Cells[row, column++].Value = "數量";
                sheet.Cells[row, column++].Value = "材質";
                sheet.Cells[row, column++].Value = "長度";
                sheet.Cells[row, column++].Value = "Phase";
                sheet.Cells[row, column++].Value = "車次";
                sheet.Cells[row, column++].Value = "標題1";
                sheet.Cells[row, column++].Value = "標題2";
                sheet.Cells[row, column++].Value = "高度";
                sheet.Cells[row, column++].Value = "寬度";
                sheet.Cells[row, column++].Value = "腹板厚度";
                sheet.Cells[row, column++].Value = "翼板厚度";
                sheet.Cells[row, column++].Value = "型鋼類型";
                sheet.Cells[row, column++].Value = "排版";
                sheet.Cells[row, column++].Value = "零件ID";
                sheet.Cells[row++, column++].Value = "構件ID";
                #endregion
                #region 欄位塞值
                foreach (KeyValuePair<string, ObservableCollection<SteelPart>> item in steelPart)
                {
                    column = 0;
                    foreach (SteelPart part in item.Value)
                    {
                        column = 0;
                        sheet.Cells[row, column++].Value = part.GUID.ToString();
                        sheet.Cells[row, column++].Value = part.ExclamationMark;
                        sheet.Cells[row, column++].Value = part.Number;
                        sheet.Cells[row, column++].Value = part.DrawingName;
                        sheet.Cells[row, column++].Value = part.Profile;
                        sheet.Cells[row, column++].Value = part.Count.ToString();
                        sheet.Cells[row, column++].Value = part.Material;
                        sheet.Cells[row, column++].Value = part.Length;
                        sheet.Cells[row, column++].Value = part.Phase;
                        sheet.Cells[row, column++].Value = part.ShippingNumber;
                        sheet.Cells[row, column++].Value = part.Title1;
                        sheet.Cells[row, column++].Value = part.Title2;
                        sheet.Cells[row, column++].Value = part.H;
                        sheet.Cells[row, column++].Value = part.W;
                        sheet.Cells[row, column++].Value = part.t1;
                        sheet.Cells[row, column++].Value = part.t2;
                        sheet.Cells[row, column++].Value = part.Type.ToString();
                        int tempRow = row;
                        foreach (bool match in part.Match)
                        {
                            sheet.Cells[row++, column].Value = match;
                        }
                        rowList.Add(row);
                        row = tempRow;
                        column++;
                        foreach (int id in part.ID)
                        {
                            sheet.Cells[row++, column].Value = id;
                        }
                        rowList.Add(row);
                        row = tempRow;
                        column++;
                        foreach (int father in part.Father)
                        {
                            sheet.Cells[row++, column].Value = father;
                        }
                        rowList.Add(row);
                        row = rowList.Max() + 1;
                    }
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                ScreenManager.ViewModel.Status = $"產生文件 ...SteelBolt";
                //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                #region SteelBolt
                sheet = book.Worksheets.Add("SteelBolt"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                rowList = new List<int>();
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "是否為Tekla文件";
                sheet.Cells[row, column++].Value = "數量";
                sheet.Cells[row, column++].Value = "材質";
                sheet.Cells[row, column++].Value = "斷面規格";
                sheet.Cells[row, column++].Value = "圖面狀態";
                sheet.Cells[row, column++].Value = "螺栓類型";
                sheet.Cells[row, column++].Value = "建立日期";
                sheet.Cells[row, column++].Value = "修改日期";
                sheet.Cells[row++, column++].Value = "歸屬購件";
                #endregion
                #region 欄位塞值
                foreach (KeyValuePair<string, ObservableCollection<SteelBolts>> item in steelBolts)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = item.Value[0].IsTekla;
                    sheet.Cells[row, column++].Value = item.Value[0].Count.ToString();
                    sheet.Cells[row, column++].Value = item.Value[0].Material;
                    sheet.Cells[row, column++].Value = item.Value[0].Profile;
                    sheet.Cells[row, column++].Value = item.Value[0].State.ToString();
                    sheet.Cells[row, column++].Value = item.Value[0].Type.ToString();
                    sheet.Cells[row, column++].Value = item.Value[0].Creation;
                    sheet.Cells[row, column++].Value = item.Value[0].Revise;
                    foreach (int father in item.Value[0].Father)
                    {
                        sheet.Cells[row++, column].Value = father;
                    }
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region SteelAttrCutSetting
                sheet = book.Worksheets.Add("SteelAttrCutSetting"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                rowList = new List<int>();
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "零件GUID";
                sheet.Cells[row, column++].Value = "Face";
                sheet.Cells[row, column++].Value = "左上X";
                sheet.Cells[row, column++].Value = "左上Y";
                sheet.Cells[row, column++].Value = "右上X";
                sheet.Cells[row, column++].Value = "右上Y";
                sheet.Cells[row, column++].Value = "右下X";
                sheet.Cells[row, column++].Value = "右下Y";
                sheet.Cells[row, column++].Value = "左下X";
                sheet.Cells[row++, column++].Value = "左下Y";
                #endregion
                #region 欄位塞值
                foreach (SteelCutSetting item in cutSettingList)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = $"{item.GUID}";
                    sheet.Cells[row, column++].Value = $"{item.face}";
                    sheet.Cells[row, column++].Value = $"{item.ULX}";
                    sheet.Cells[row, column++].Value = $"{item.ULY}";
                    sheet.Cells[row, column++].Value = $"{item.URX}";
                    sheet.Cells[row, column++].Value = $"{item.URY}";
                    sheet.Cells[row, column++].Value = $"{item.DRX}";
                    sheet.Cells[row, column++].Value = $"{item.DRY}";
                    sheet.Cells[row, column++].Value = $"{item.DLX}";
                    sheet.Cells[row++, column++].Value = $"{item.DLY}";
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region ErrorDataName
                sheet = book.Worksheets.Add("Wrong DM File"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                foreach (var item in errorGUID)
                {
                    sheet.Cells[row++, column].Value = $"{item}]";
                }

                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region 製品設定-零件清單
                sheet = book.Worksheets.Add("製品設定-零件清單"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                rowList = new List<int>();
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "dm檔名";
                sheet.Cells[row, column++].Value = "構件編號";
                sheet.Cells[row, column++].Value = "零件編號";
                sheet.Cells[row, column++].Value = "名稱";
                sheet.Cells[row, column++].Value = "鋼材類型";
                sheet.Cells[row, column++].Value = "斷面規格";
                sheet.Cells[row, column++].Value = "鋼材類型(Index)";
                sheet.Cells[row, column++].Value = "材質";
                sheet.Cells[row, column++].Value = "數量";
                sheet.Cells[row, column++].Value = "零件長";
                sheet.Cells[row, column++].Value = "零件重";
                sheet.Cells[row, column++].Value = "高度";
                sheet.Cells[row, column++].Value = "寬度";
                sheet.Cells[row, column++].Value = "腹板厚度";
                sheet.Cells[row, column++].Value = "Phase";
                sheet.Cells[row, column++].Value = "拆運";
                sheet.Cells[row, column++].Value = "標題一";
                sheet.Cells[row++, column++].Value = "標題二";
                #endregion
                #region 欄位塞值
                foreach (ProductSettingsPageViewModel item in ObSettingVM.GetData())
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = item.steelAttr.GUID.Value.ToString();
                    sheet.Cells[row, column++].Value = item.steelAttr.AsseNumber;
                    sheet.Cells[row, column++].Value = item.steelAttr.PartNumber;
                    sheet.Cells[row, column++].Value = item.steelAttr.Name;
                    sheet.Cells[row, column++].Value = item.TypeDesc;
                    sheet.Cells[row, column++].Value = item.steelAttr.Profile;
                    sheet.Cells[row, column++].Value = item.SteelType;
                    sheet.Cells[row, column++].Value = item.steelAttr.Material;
                    sheet.Cells[row, column++].Value = item.Count;
                    sheet.Cells[row, column++].Value = item.steelAttr.Length;
                    sheet.Cells[row, column++].Value = item.steelAttr.Weight;
                    sheet.Cells[row, column++].Value = item.steelAttr.H;
                    sheet.Cells[row, column++].Value = item.steelAttr.W;
                    sheet.Cells[row, column++].Value = item.steelAttr.t1;
                    sheet.Cells[row, column++].Value = item.steelAttr.t2;
                    sheet.Cells[row, column++].Value = item.steelAttr.Phase;
                    sheet.Cells[row, column++].Value = item.steelAttr.ShippingNumber;
                    sheet.Cells[row, column++].Value = item.steelAttr.Title1;
                    sheet.Cells[row++, column++].Value = item.steelAttr.Title2;
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region  零件DM檔   
                ScreenManager.ViewModel.Status = $"產生文件 ...model";
                //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                sheet = book.Worksheets.Add("零件DM檔"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                int zeroIndex = 0, firstIndex = 0, secondIndex = 0, thirdIndex = 0;
                #region Blocks
                sheet.Cells[row++, column].Value = "model.Blocks";
                firstIndex = 0;
                for (int i = 0; i < modelBlockList.Count; i++)
                {
                    var blocks = (Block[])modelBlockList[i];
                    #region 第一層
                    foreach (Block block in blocks)
                    {
                        switch (block.GetType().Name)
                        {
                            case "Block":
                                Block b = (Block)block;
                                sheet.Cells[row, column++].Value = $"1-1-0model.Blocks[{firstIndex}]";
                                sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{b.Name}";
                                column = 0;
                                break;
                            case "Steel3DBlock":
                                Steel3DBlock sdb = (Steel3DBlock)block;
                                sheet.Cells[row, column++].Value = $"1-1-0model.Blocks[{firstIndex}]";
                                sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                sheet.Cells[row, column++].Value = $"{sdb.Name}";
                                sheet.Cells[row++, column++].Value = $"{sdb.Units}";
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"*1-1-0model.Blocks[{firstIndex}]";
                                sheet.Cells[row, column++].Value = $"例外型別";
                                sheet.Cells[row++, column++].Value = $"{block.GetType().Name}";
                                column = 0;
                                break;
                        }
                        #region 第二層
                        secondIndex = 0;
                        foreach (Entity entities in (EntityList)block.Entities)
                        {
                            switch (entities.GetType().Name)
                            {
                                case "BlockReference":
                                    BlockReference br = (BlockReference)entities;
                                    sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                    sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                    sheet.Cells[row++, column++].Value = $"{br.BlockName}";
                                    column = 0;
                                    break;
                                case "Line":
                                    Line line = (Line)entities;
                                    sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                    sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                    sheet.Cells[row, column++].Value = $"StartPoint:{line.StartPoint.X},{line.StartPoint.Y},{line.StartPoint.Z}";
                                    sheet.Cells[row++, column++].Value = $"EndPoint:{line.EndPoint.X},{line.EndPoint.Y},{line.EndPoint.Z}";
                                    column = 0;
                                    break;
                                //case "Block":
                                //    Block block = (Block)entities;
                                //    break;
                                case "Circle":
                                    Circle circle = (Circle)entities;
                                    sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                    sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                    sheet.Cells[row++, column++].Value = $"{circle.Diameter}";///Center
                                    column = 0;
                                    break;
                                case "Mesh":
                                    Mesh mesh = (Mesh)entities;
                                    sheet.Cells[row, column++].Value = $"1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                    sheet.Cells[row, column++].Value = $"{entities.GetType().Name}";
                                    sheet.Cells[row, column++].Value = (mesh.Vertices.Count() > 0 ? "有Vertices" : "無Vertices");
                                    sheet.Cells[row, column++].Value = $"{mesh.MaterialName}";
                                    sheet.Cells[row, column++].Value = $"{mesh.Faces}";
                                    sheet.Cells[row++, column++].Value = $"{mesh.LineTypeName}";
                                    column = 0;
                                    break;
                                default:
                                    sheet.Cells[row, column++].Value = $"*1-2model.Blocks[{firstIndex}].Entities[{secondIndex}]";
                                    sheet.Cells[row, column++].Value = $"例外型別";
                                    sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                    sheet.Cells[row++, column++].Value = $"{entities.GetType().Name}";
                                    column = 0;
                                    break;
                            }
                            #region 第三層
                            thirdIndex = 0;
                            if (entities.EntityData == null)
                            {
                                switch (entities.GetType().Name)
                                {
                                    case "Block":
                                    case "Line":
                                    case "SteelAttr":
                                    case "BoltAttr":
                                    case "BlockReference":
                                        sheet.Cells[row, column++].Value = $"1-3model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData = null";
                                        sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                        sheet.Cells[row += 2, column++].Value = $"{entities.GetType().Name}";
                                        column = 0;
                                        break;
                                    default:
                                        sheet.Cells[row, column++].Value = $"*1-3model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData = null";
                                        sheet.Cells[row, column++].Value = $"例外型別";
                                        sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                        sheet.Cells[row += 2, column++].Value = $"{entities.GetType().Name}";
                                        column = 0;
                                        break;
                                }
                                //sheet.Cells[row++, column++].Value = $"{((BlockReference)entities).BlockName}無EntityData";
                                column = 0;
                            }
                            else
                            {
                                switch (entities.EntityData.GetType().Name)
                                {
                                    case "BoltAttr":
                                        BoltAttr ba = (BoltAttr)entities.EntityData;//Dia Model Type Face
                                        sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                        sheet.Cells[row, column++].Value = entities.EntityData.GetType().Name;
                                        sheet.Cells[row, column++].Value = ba.GUID.ToString();
                                        sheet.Cells[row, column++].Value = ba.BlockName;
                                        sheet.Cells[row, column++].Value = ba.X;
                                        sheet.Cells[row, column++].Value = ba.Y;
                                        sheet.Cells[row, column++].Value = ba.Z;
                                        sheet.Cells[row, column++].Value = ba.Dia;
                                        sheet.Cells[row, column++].Value = $"{ba.Mode}";
                                        sheet.Cells[row += 2, column++].Value = ba.Type.ToString();
                                        column = 0;
                                        break;
                                    case "SteelAttr":
                                        SteelAttr sa = (SteelAttr)entities.EntityData;
                                        sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                        sheet.Cells[row, column++].Value = sa.GetType().Name;
                                        sheet.Cells[row, column++].Value = sa.GUID.ToString();
                                        sheet.Cells[row, column++].Value = sa.AsseNumber;
                                        sheet.Cells[row, column++].Value = sa.PartNumber;
                                        sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                                        sheet.Cells[row, column++].Value = sa.Profile;
                                        sheet.Cells[row, column++].Value = sa.Material;
                                        sheet.Cells[row, column++].Value = sa.Number;
                                        sheet.Cells[row, column++].Value = sa.Creation;
                                        sheet.Cells[row, column++].Value = sa.Revise;
                                        sheet.Cells[row, column++].Value = sa.ExclamationMark;
                                        sheet.Cells[row, column++].Value = sa.Lock;
                                        sheet.Cells[row, column++].Value = sa.H;
                                        sheet.Cells[row, column++].Value = sa.W;
                                        sheet.Cells[row, column++].Value = sa.Kg;
                                        sheet.Cells[row, column++].Value = sa.Length;
                                        sheet.Cells[row, column++].Value = sa.Weight;
                                        sheet.Cells[row, column++].Value = sa.t1;
                                        sheet.Cells[row, column++].Value = sa.t2;
                                        sheet.Cells[row, column++].Value = sa.Phase;
                                        sheet.Cells[row, column++].Value = sa.ShippingNumber;
                                        sheet.Cells[row, column++].Value = sa.Title1;
                                        sheet.Cells[row += 2, column++].Value = sa.Title2;
                                        column = 0;
                                        break;
                                    case "Mesh":
                                        Mesh mesh = (Mesh)entities.EntityData;
                                        break;
                                    case "GroupBoltsAttr":
                                        GroupBoltsAttr gba = (GroupBoltsAttr)entities.EntityData;
                                        sheet.Cells[row, column++].Value = $"1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                        sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                        sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                        sheet.Cells[row, column++].Value = gba.X.ToString();
                                        sheet.Cells[row, column++].Value = gba.Y.ToString();
                                        sheet.Cells[row, column++].Value = gba.Z.ToString();
                                        sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                        sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                        sheet.Cells[row, column++].Value = ((AXIS_MODE)gba.Mode).ToString();
                                        sheet.Cells[row += 2, column++].Value = gba.Count.ToString();
                                        column = 0;
                                        break;
                                    default:
                                        sheet.Cells[row, column++].Value = $"*1-4model.Blocks[{firstIndex}].Entities[{secondIndex}].EntityData";
                                        sheet.Cells[row, column++].Value = $"例外型別";
                                        sheet.Cells[row, column++].Value = $"{block.GetType().Name}+";
                                        sheet.Cells[row, column++].Value = $"{entities.GetType().Name}+";
                                        sheet.Cells[row += 2, column++].Value = $"{entities.EntityData.GetType().Name}";
                                        column = 0;
                                        break;
                                }
                            }
                            thirdIndex++;
                            #endregion
                            secondIndex++;
                        }
                        #endregion
                        firstIndex++;
                    }
                    #endregion
                }
                #endregion
                #region Entities
                firstIndex = 0; secondIndex = 0; thirdIndex = 0;
                sheet.Cells[row++, column].Value = "model.Entities";
                for (int i = 0; i < modelEntityList.Count; i++)
                {
                    Entity[] entities = (Entity[])modelEntityList[i];
                    #region 第一層
                    firstIndex = 0;
                    foreach (var entity in entities)
                    {
                        switch (entity.GetType().Name)
                        {
                            case "BlockReference":
                                BlockReference br = (BlockReference)entity;
                                sheet.Cells[row, column++].Value = $"2-1-0model.Entities[{firstIndex}]";
                                sheet.Cells[row, column++].Value = $"{entity.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{br.BlockName}";
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"*2-1-0model.Entities[{firstIndex}]";
                                sheet.Cells[row++, column++].Value = $"例外型別";
                                sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                                column = 0;
                                break;
                        }
                        #region 第二層
                        secondIndex = 0;
                        if (entity.EntityData == null)
                        {
                            switch (entity.GetType().Name)
                            {
                                case "BlockReference":
                                    sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData = null";
                                    sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                                    column = 0;
                                    break;
                                default:
                                    sheet.Cells[row, column++].Value = $"*2-2-0model.Entities[{firstIndex}].EntityData = null";
                                    sheet.Cells[row++, column++].Value = $"例外型別";
                                    sheet.Cells[row++, column++].Value = $"{entity.GetType().Name}";
                                    column = 0;
                                    break;
                            }
                            //sheet.Cells[row++, column++].Value = $"{((BlockReference)entities).BlockName}無EntityData";
                            column = 0;
                        }
                        else
                        {
                            switch (entity.EntityData.GetType().Name)
                            {
                                case "GroupBoltsAttr":
                                    GroupBoltsAttr gba = (GroupBoltsAttr)entity.EntityData;
                                    sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                    sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                    sheet.Cells[row, column++].Value = $"{gba.Face}";
                                    sheet.Cells[row, column++].Value = $"{gba.Type}";
                                    sheet.Cells[row, column++].Value = gba.X.ToString();
                                    sheet.Cells[row, column++].Value = gba.Y.ToString();
                                    sheet.Cells[row, column++].Value = gba.Z.ToString();
                                    sheet.Cells[row, column++].Value = gba.dX;
                                    sheet.Cells[row, column++].Value = String.Join(" ", gba.dXs.ToArray());
                                    sheet.Cells[row, column++].Value = gba.dY;
                                    sheet.Cells[row, column++].Value = String.Join(" ", gba.dYs.ToArray());
                                    sheet.Cells[row, column++].Value = $"{gba.Count}";
                                    sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                    sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                    sheet.Cells[row += 2, column++].Value = ((AXIS_MODE)gba.Mode).ToString();//Count
                                    column = 0;
                                    break;
                                case "BlockReference":
                                    break;
                                case "SteelAttr":
                                    SteelAttr sa = (SteelAttr)entity.EntityData;
                                    sheet.Cells[row, column++].Value = $"2-2-0model.Entities[{firstIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = sa.GetType().Name;
                                    sheet.Cells[row, column++].Value = sa.GUID.ToString();
                                    sheet.Cells[row, column++].Value = sa.AsseNumber;
                                    sheet.Cells[row, column++].Value = sa.PartNumber;
                                    sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                                    sheet.Cells[row, column++].Value = sa.Profile;
                                    sheet.Cells[row, column++].Value = sa.Material;
                                    sheet.Cells[row, column++].Value = sa.Number;
                                    sheet.Cells[row, column++].Value = sa.Creation;
                                    sheet.Cells[row, column++].Value = sa.Revise;
                                    sheet.Cells[row, column++].Value = sa.ExclamationMark;
                                    sheet.Cells[row, column++].Value = sa.Lock;
                                    sheet.Cells[row, column++].Value = sa.H;
                                    sheet.Cells[row, column++].Value = sa.W;
                                    sheet.Cells[row, column++].Value = sa.Kg;
                                    sheet.Cells[row, column++].Value = sa.Length;
                                    sheet.Cells[row, column++].Value = sa.Weight;
                                    sheet.Cells[row, column++].Value = sa.t1;
                                    sheet.Cells[row, column++].Value = sa.t2;
                                    sheet.Cells[row, column++].Value = sa.Phase;
                                    sheet.Cells[row, column++].Value = sa.ShippingNumber;
                                    sheet.Cells[row, column++].Value = sa.Title1;
                                    sheet.Cells[row += 2, column++].Value = sa.Title2;
                                    column = 0;
                                    break;
                                default://BlockReference
                                    sheet.Cells[row, column++].Value = $"*2-2-0model.Entities[{firstIndex}].EntityData";
                                    sheet.Cells[row, column++].Value = $"例外型別";
                                    sheet.Cells[row, column++].Value = entity.GetType().Name;
                                    sheet.Cells[row += 2, column++].Value = entity.EntityData.GetType().Name;
                                    column = 0;
                                    break;
                            }
                        }
                        #endregion
                        secondIndex++;
                        #region 第三層(X)
                        //switch (entity.EntityData.GetType().Name)
                        //{
                        //    default:
                        //        sheet.Cells[row, column++].Value = $"2-1-0model.Entities[{firstIndex}]";
                        //        sheet.Cells[row, column++].Value = entities.GetType().Name;
                        //        sheet.Cells[row++, column++].Value = entity.GetType().Name;
                        //        sheet.Cells[row++, column++].Value = entity.EntityData.GetType().Name;
                        //        column = 0;
                        //        break;
                        //}
                        #endregion
                    }
                    #endregion
                    firstIndex++;
                }
                //#region SteelAttr
                //    //case "SteelAttr":
                //    #region 第一層
                //    //secondIndex = 0;
                //    //    SteelAttr sa = (SteelAttr)entity.EntityData;
                //    //    sheet.Cells[row, column++].Value = $"model.Entities[{i}].EntityData";
                //    //    sheet.Cells[row, column++].Value = entity.EntityData.GetType().Name;
                //    //    sheet.Cells[row, column++].Value = sa.GUID.ToString();
                //    //    sheet.Cells[row, column++].Value = sa.AsseNumber;
                //    //    sheet.Cells[row, column++].Value = sa.PartNumber;
                //    //    sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                //    //    sheet.Cells[row, column++].Value = sa.Profile;
                //    //    sheet.Cells[row, column++].Value = sa.Material;
                //    //    sheet.Cells[row, column++].Value = sa.Number;
                //    //    sheet.Cells[row, column++].Value = sa.Creation;
                //    //    sheet.Cells[row, column++].Value = sa.Revise;
                //    //    sheet.Cells[row, column++].Value = sa.ExclamationMark;
                //    //    sheet.Cells[row, column++].Value = sa.Lock;
                //    //    sheet.Cells[row, column++].Value = sa.H;
                //    //    sheet.Cells[row, column++].Value = sa.W;
                //    //    sheet.Cells[row, column++].Value = sa.Kg;
                //    //    sheet.Cells[row, column++].Value = sa.Length;
                //    //    sheet.Cells[row, column++].Value = sa.Weight;
                //    //    sheet.Cells[row, column++].Value = sa.t1;
                //    //    sheet.Cells[row, column++].Value = sa.t2;
                //    //    sheet.Cells[row, column++].Value = sa.Phase;
                //    //    sheet.Cells[row, column++].Value = sa.ShippingNumber;
                //    //    sheet.Cells[row, column++].Value = sa.Title1;
                //    //    sheet.Cells[row++, column++].Value = sa.Title2;
                //    //    column = 0;
                //    //    break; 
                //    #endregion
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region 排版零件
                sheet = book.Worksheets.Add("排版零件檔"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "零件編號";
                sheet.Cells[row, column++].Value = "構件編號";
                sheet.Cells[row, column++].Value = "建立日期";
                sheet.Cells[row, column++].Value = "長度";
                sheet.Cells[row, column++].Value = "零件鎖定";
                sheet.Cells[row, column++].Value = "材質";
                sheet.Cells[row, column++].Value = "斷面規格";
                sheet.Cells[row, column++].Value = "修改日期";
                sheet.Cells[row, column++].Value = "圖面狀態";
                sheet.Cells[row, column++].Value = "型鋼類型";
                sheet.Cells[row, column++].Value = "高";
                sheet.Cells[row, column++].Value = "寬";
                sheet.Cells[row, column++].Value = "腹板厚度";
                sheet.Cells[row, column++].Value = "重量";
                sheet.Cells[row, column++].Value = "數量";

                sheet.Cells[row, column++].Value = "零件ID";
                sheet.Cells[row++, column++].Value = "可批配料單";
                #endregion
                #region 塞值
                foreach (TypeSettingDataView mdv in DataViews)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = mdv.PartNumber;
                    sheet.Cells[row, column++].Value = mdv.AssemblyNumber;
                    sheet.Cells[row, column++].Value = mdv.Creation;
                    sheet.Cells[row, column++].Value = mdv.Length;
                    sheet.Cells[row, column++].Value = mdv.Lock;
                    sheet.Cells[row, column++].Value = mdv.Material;
                    sheet.Cells[row, column++].Value = mdv.Profile;
                    sheet.Cells[row, column++].Value = mdv.Revise;
                    sheet.Cells[row, column++].Value = mdv.State.ToString();
                    sheet.Cells[row, column++].Value = mdv.SteelType;
                    sheet.Cells[row, column++].Value = mdv.H;
                    sheet.Cells[row, column++].Value = mdv.W;
                    sheet.Cells[row, column++].Value = mdv.t1;
                    sheet.Cells[row, column++].Value = mdv.t2;
                    sheet.Cells[row, column++].Value = mdv.Weigth;
                    int tempRow = row;
                    foreach (double id in mdv.ID)
                    {
                        sheet.Cells[row++, column].Value = id;
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                    foreach (bool match in mdv.Match)
                    {
                        sheet.Cells[row++, column].Value = match;
                    }
                    rowList.Add(row);
                    column = 0;
                    row = rowList.Max() + 1;
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion

                #region Material
                sheet = book.Worksheets.Add("排版 file"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                #region 欄位名稱
                sheet.Cells[row, column++].Value = "來源列表";
                sheet.Cells[row, column++].Value = "素材編號";
                sheet.Cells[row, column++].Value = "斷面規格";
                sheet.Cells[row, column++].Value = "索引值";
                sheet.Cells[row, column++].Value = "購料長";
                sheet.Cells[row, column++].Value = "材質";
                sheet.Cells[row, column++].Value = "廠商";
                sheet.Cells[row, column++].Value = "修改日期";
                sheet.Cells[row++, column++].Value = "歸屬購件";
                #endregion
                #region 塞值
                foreach (MaterialDataView mdv in materialDataViews)
                {
                    column = 0;
                    sheet.Cells[row, column++].Value = mdv.Sources;
                    sheet.Cells[row, column++].Value = mdv.MaterialNumber;
                    sheet.Cells[row, column++].Value = mdv.Profile;
                    sheet.Cells[row, column++].Value = mdv.LengthIndex;
                    sheet.Cells[row, column++].Value = mdv.LengthStr;
                    sheet.Cells[row, column++].Value = mdv.Material;
                    int tempRow = row;
                    foreach (double length in mdv.LengthList)
                    {
                        sheet.Cells[row++, column].Value = length;
                    }
                    rowList.Add(row);
                    row = tempRow;
                    column++;
                }
                #endregion
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion 
                #endregion

                #region 排版DM
                sheet = book.Worksheets.Add("Mateial dm file"); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                row = 0;
                column = 0;
                var mateialList = appVM.GetAllDevMateialPart();
                foreach (string materialNumber in mateialList)
                {
                    ReadFile readFile = ser.ReadMaterialModel(materialNumber);
                    _BufferModel.Clear();
                    readFile.DoWork();
                    readFile.AddToScene(_BufferModel);
                    int index_Block = 0, index_Entities = 0, index_EntityData = 0;
                    #region Block
                    _BufferModel.Blocks.ForEach(block =>
                                {
                                    column = 0;
                                    sheet.Cells[row, column++].Value = $"model.Block[{index_Block}]";
                                    switch (block.GetType().Name)
                                    {
                                        case "Block":
                                            Block item = (Block)block;
                                            sheet.Cells[row, column++].Value = $"{item.GetType().Name}";
                                            sheet.Cells[row++, column].Value = $"{item.Name}";
                                            column = 0;
                                            break;
                                        case "Steel3DBlock":
                                            Steel3DBlock sdb = (Steel3DBlock)block;
                                            sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                            sheet.Cells[row, column++].Value = $"{sdb.Name}";
                                            sheet.Cells[row++, column++].Value = $"{sdb.Units}";
                                            column = 0;
                                            break;
                                        default:
                                            sheet.Cells[row, column++].Value = $"{block.GetType().Name}";
                                            sheet.Cells[row++, column++].Value = $"未歸類";
                                            column = 0;
                                            break;
                                    }
                                    #region Block.Entities
                                    index_Entities = 0;
                                    block.Entities.ForEach(e =>
                                    {
                                        column = 0;
                                        sheet.Cells[row, column++].Value = $"model.Block[{index_Block}].Entities[{index_Entities}]";
                                        switch (e.GetType().Name)
                                        {
                                            case "BlockReference":
                                                BlockReference item = (BlockReference)e;
                                                sheet.Cells[row, column++].Value = item.GetType().Name;
                                                sheet.Cells[row++, column++].Value = $"{item.BlockName}";
                                                column = 0;
                                                break;
                                            case "Line":
                                                Line line = (Line)e;
                                                sheet.Cells[row, column++].Value = line.GetType().Name;
                                                sheet.Cells[row, column++].Value = $"StartPoint:{line.StartPoint.X},{line.StartPoint.Y},{line.StartPoint.Z}";
                                                sheet.Cells[row++, column++].Value = $"EndPoint:{line.EndPoint.X},{line.EndPoint.Y},{line.EndPoint.Z}";
                                                column = 0;
                                                break;
                                            case "Circle":
                                                Circle circle = (Circle)e;
                                                sheet.Cells[row, column++].Value = circle.GetType().Name;
                                                sheet.Cells[row++, column++].Value = $"{circle.Diameter}";
                                                column = 0;
                                                break;
                                            case "Mesh":
                                                Mesh mesh = (Mesh)e;
                                                sheet.Cells[row, column++].Value = $"{mesh.GetType().Name}";
                                                sheet.Cells[row, column++].Value = (mesh.Vertices.Count() > 0 ? "有Vertices" : "無Vertices");
                                                sheet.Cells[row, column++].Value = $"{mesh.MaterialName}";
                                                sheet.Cells[row, column++].Value = $"{mesh.Faces}";
                                                sheet.Cells[row++, column++].Value = $"{mesh.LineTypeName}";
                                                column = 0;
                                                break;
                                            default:
                                                sheet.Cells[row, column++].Value = $"{e.GetType().Name}";
                                                sheet.Cells[row++, column++].Value = $"未歸類";
                                                column = 0;
                                                break;
                                        }
                                        column = 0;
                                        #region Block.Entities.EntityData
                                        sheet.Cells[row, column++].Value = $"model.Block[{index_Block}].Entities[{index_Entities}].EntityData";
                                        switch ((e.EntityData).GetType().Name)
                                        {
                                            #region SteelAttr
                                            case "SteelAttr":
                                                SteelAttr item = (SteelAttr)(e.EntityData);
                                                sheet.Cells[row, column++].Value = item.GetType().Name;
                                                sheet.Cells[row, column++].Value = item.GUID.ToString();
                                                sheet.Cells[row, column++].Value = item.AsseNumber;
                                                sheet.Cells[row, column++].Value = item.PartNumber;
                                                sheet.Cells[row, column++].Value = ((OBJECT_TYPE)item.Type).ToString();
                                                sheet.Cells[row, column++].Value = item.Profile;
                                                sheet.Cells[row, column++].Value = item.Material;
                                                sheet.Cells[row, column++].Value = item.Number;
                                                sheet.Cells[row, column++].Value = item.Creation;
                                                sheet.Cells[row, column++].Value = item.Revise;
                                                sheet.Cells[row, column++].Value = item.ExclamationMark;
                                                sheet.Cells[row, column++].Value = item.Lock;
                                                sheet.Cells[row, column++].Value = item.H;
                                                sheet.Cells[row, column++].Value = item.W;
                                                sheet.Cells[row, column++].Value = item.Kg;
                                                sheet.Cells[row, column++].Value = item.Length;
                                                sheet.Cells[row, column++].Value = item.Weight;
                                                sheet.Cells[row, column++].Value = item.t1;
                                                sheet.Cells[row, column++].Value = item.t2;
                                                sheet.Cells[row, column++].Value = item.Phase;
                                                sheet.Cells[row, column++].Value = item.ShippingNumber;
                                                sheet.Cells[row, column++].Value = item.Title1;
                                                sheet.Cells[row++, column++].Value = item.Title2;
                                                break;
                                            #endregion
                                            #region BoltAttr
                                            case "BoltAttr":
                                                BoltAttr bolt = (BoltAttr)(e.EntityData);
                                                sheet.Cells[row, column++].Value = bolt.GetType().Name;
                                                sheet.Cells[row, column++].Value = bolt.GUID.ToString();
                                                sheet.Cells[row, column++].Value = bolt.BlockName;
                                                sheet.Cells[row, column++].Value = bolt.X;
                                                sheet.Cells[row, column++].Value = bolt.Y;
                                                sheet.Cells[row, column++].Value = bolt.Z;
                                                sheet.Cells[row, column++].Value = bolt.Dia;
                                                sheet.Cells[row, column++].Value = $"{bolt.Mode}";
                                                sheet.Cells[row++, column++].Value = bolt.Type.ToString();
                                                column = 0;
                                                break;
                                            #endregion
                                            #region GroupBoltsAttr
                                            case "GroupBoltsAttr":
                                                GroupBoltsAttr gba = (GroupBoltsAttr)(e.EntityData);
                                                sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                                sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                                sheet.Cells[row, column++].Value = $"{gba.Face}";
                                                sheet.Cells[row, column++].Value = $"{gba.Type}";
                                                sheet.Cells[row, column++].Value = gba.X.ToString();
                                                sheet.Cells[row, column++].Value = gba.Y.ToString();
                                                sheet.Cells[row, column++].Value = gba.Z.ToString();
                                                sheet.Cells[row, column++].Value = gba.dX;
                                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dXs.ToArray());
                                                sheet.Cells[row, column++].Value = gba.dY;
                                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dYs.ToArray());
                                                sheet.Cells[row, column++].Value = $"{gba.Count}";
                                                sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                                sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                                sheet.Cells[row++, column++].Value = ((AXIS_MODE)gba.Mode).ToString();//Count
                                                column = 0;
                                                break;
                                            #endregion
                                            default:
                                                sheet.Cells[row, column++].Value = $"{(e.EntityData).GetType().Name}";
                                                sheet.Cells[row++, column++].Value = $"未歸類";
                                                column = 0;
                                                break;
                                        }
                                        index_Entities++;
                                        #endregion
                                    });
                                    #endregion
                                    index_Block++;
                                });
                    #endregion
                    index_Entities = 0;
                    _BufferModel.Entities.ForEach(e =>
                    {
                        column = 0;
                        sheet.Cells[row, column++].Value = $"model.Entities[{index_Entities++}]";
                        switch (e.GetType().Name)
                        {
                            case "BlockReference":
                                BlockReference br = (BlockReference)e;
                                sheet.Cells[row, column++].Value = $"{br.GetType().Name}";
                                sheet.Cells[row++, column++].Value = $"{br.BlockName}";
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"例外型別";
                                sheet.Cells[row++, column++].Value = $"{e.GetType().Name}";
                                column = 0;
                                break;
                        }
                        column = 0;
                        sheet.Cells[row, column++].Value = $"model.Entities[{index_Entities}].EntityData";
                        switch (e.EntityData.GetType().Name)
                        {
                            case "GroupBoltsAttr":
                                GroupBoltsAttr gba = (GroupBoltsAttr)e.EntityData;
                                sheet.Cells[row, column++].Value = $"{gba.GetType().Name}";
                                sheet.Cells[row, column++].Value = gba.GUID.ToString();
                                sheet.Cells[row, column++].Value = $"{gba.Face}";
                                sheet.Cells[row, column++].Value = $"{gba.Type}";
                                sheet.Cells[row, column++].Value = gba.X.ToString();
                                sheet.Cells[row, column++].Value = gba.Y.ToString();
                                sheet.Cells[row, column++].Value = gba.Z.ToString();
                                sheet.Cells[row, column++].Value = gba.dX;
                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dXs.ToArray());
                                sheet.Cells[row, column++].Value = gba.dY;
                                sheet.Cells[row, column++].Value = String.Join(" ", gba.dYs.ToArray());
                                sheet.Cells[row, column++].Value = $"{gba.Count}";
                                sheet.Cells[row, column++].Value = gba.Dia.ToString();
                                sheet.Cells[row, column++].Value = gba.StartHole.ToString();
                                sheet.Cells[row++, column++].Value = ((AXIS_MODE)gba.Mode).ToString();//Count
                                column = 0;
                                break;
                            case "BlockReference":
                                break;
                            case "SteelAttr":
                                SteelAttr sa = (SteelAttr)e.EntityData;
                                sheet.Cells[row, column++].Value = sa.GetType().Name;
                                sheet.Cells[row, column++].Value = sa.GUID.ToString();
                                sheet.Cells[row, column++].Value = sa.AsseNumber;
                                sheet.Cells[row, column++].Value = sa.PartNumber;
                                sheet.Cells[row, column++].Value = ((OBJECT_TYPE)sa.Type).ToString();
                                sheet.Cells[row, column++].Value = sa.Profile;
                                sheet.Cells[row, column++].Value = sa.Material;
                                sheet.Cells[row, column++].Value = sa.Number;
                                sheet.Cells[row, column++].Value = sa.Creation;
                                sheet.Cells[row, column++].Value = sa.Revise;
                                sheet.Cells[row, column++].Value = sa.ExclamationMark;
                                sheet.Cells[row, column++].Value = sa.Lock;
                                sheet.Cells[row, column++].Value = sa.H;
                                sheet.Cells[row, column++].Value = sa.W;
                                sheet.Cells[row, column++].Value = sa.Kg;
                                sheet.Cells[row, column++].Value = sa.Length;
                                sheet.Cells[row, column++].Value = sa.Weight;
                                sheet.Cells[row, column++].Value = sa.t1;
                                sheet.Cells[row, column++].Value = sa.t2;
                                sheet.Cells[row, column++].Value = sa.Phase;
                                sheet.Cells[row, column++].Value = sa.ShippingNumber;
                                sheet.Cells[row, column++].Value = sa.Title1;
                                sheet.Cells[row++, column++].Value = sa.Title2;
                                column = 0;
                                break;
                            default:
                                sheet.Cells[row, column++].Value = $"例外型別";
                                sheet.Cells[row, column++].Value = e.GetType().Name;
                                sheet.Cells[row++, column++].Value = e.EntityData.GetType().Name;
                                column = 0;
                                break;
                        }
                        row++;
                        index_Entities++;
                    });
                    row++;
                    column = 0;
                }
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                #endregion



                book.BeginUpdate();
                book.SaveDocument(path, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }
            catch (Exception ex)
            {
                string subPath1 = path.Substring(0, path.LastIndexOf('.'));
                string subPath2 = path.Substring(path.LastIndexOf('.') + 1);
                book.BeginUpdate();
                book.SaveDocument(subPath1+"Error."+subPath2, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }
            ScreenManager.Close();
        }

        /// <summary>
        /// 創建文件
        /// </summary>
        /// <param name="path">儲存路徑</param>
        public void CreateFile(string path, ObservableCollection<MaterialDataView> dataViews, ObservableCollection<SteelAttr> steelAttrs = null)
        {
            SpreadsheetControl spreadSheet = new SpreadsheetControl();
            try
            {
                int row = 0;
                IWorkbook book = spreadSheet.Document; //提供對控件中加載的工作簿的訪問
                Worksheet sheet = book.Worksheets.Add(); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                ReadOnlyCollection<MaterialDataView> materialDataViews = new ReadOnlyCollection<MaterialDataView>(dataViews);
                steelAttrs = steelAttrs == null ? SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp") : steelAttrs;
                materialDataViews.GroupBy(el => el.Profile).ForEach(el =>
                {
                    sheet.Cells[row, 0].Value = $"斷面規格 : {el.Key}";
                    row++;
                    sheet.Range[$"A{row}:H{row}"].Merge();
                    sheet.Cells[row, 0].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                    sheet.Cells[row, 0].Borders.TopBorder.Color = Color.Black;
                    sheet.Cells[row, 0].Value = "購料長度";
                    sheet.Cells[row, 1].Value = "切割長度組合";
                    sheet.Cells[row, 2].Value = "材質";
                    sheet.Cells[row, 3].Value = "加工長度";
                    sheet.Cells[row, 4].Value = "損耗";
                    sheet.Cells[row, 5].Value = "購料重量";
                    sheet.Cells[row, 6].Value = "來源";
                    sheet.Cells[row, 7].Value = "狀態";

                    {
                        sheet.Cells[row, 0].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.TopBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.RightBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.LeftBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.BottomBorder.Color = Color.Black;
                    }

                    row++;
                    foreach (var item in el)
                    {
                        int index = steelAttrs.FindIndex(steel => steel.Profile == item.Profile);
                        sheet.Cells[row, 0].Value = item.LengthStr;
                        sheet.Cells[row, 1].Value = item.Parts.Select(part => $"{part.Length}").Aggregate((str1, str2) => $"{str1},{str2}");
                        sheet.Cells[row, 2].Value = item.Parts[0].Material;
                        sheet.Cells[row, 3].Value = item.Loss;
                        sheet.Cells[row, 4].Value = item.LengthStr - item.Loss;
                        sheet.Cells[row, 5].Value = (index == -1 ? 0 : item.LengthStr / 1000 * steelAttrs[index].Kg).ToString("#0.00");
                        sheet.Cells[row, 6].Value = item.Sources;
                        sheet.Cells[row, 0].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.TopBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.TopBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.TopBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.RightBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.RightBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.RightBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.LeftBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.LeftBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.LeftBorder.Color = Color.Black;

                        sheet.Cells[row, 0].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 1].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 2].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 3].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 4].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 5].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 6].Borders.BottomBorder.Color = Color.Black;
                        sheet.Cells[row, 7].Borders.BottomBorder.Color = Color.Black;
                        row++;
                    }
                });
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                book.BeginUpdate();
                book.SaveDocument(path, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}