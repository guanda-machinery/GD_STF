#define Debug
using devDept.Eyeshot.Entities;
using System;
using System.Collections.Generic;
using WPFSTD105.ViewModel;

namespace WPFSTD105
{
    /// <summary>
    /// 還原動作列表
    /// </summary>
    public class ReductionList
    {
        private readonly devDept.Eyeshot.Model _Model;
        private readonly devDept.Eyeshot.Model _Drawing;
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ReductionList.ReductionList(Model, Model)' 的 XML 註解
        public ReductionList(devDept.Eyeshot.Model model, devDept.Eyeshot.Model drawing)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ReductionList.ReductionList(Model, Model)' 的 XML 註解
        {
            _Model = model;
            _Drawing = drawing;
        }
        /// <summary>
        /// 目前動作
        /// </summary>
        private int _Index { get; set; }
        /// <summary>
        /// 加入物件
        /// </summary>
        /// <param name="reduction3D"></param>
        /// <param name="reduction2D"></param>
        public void Add(Reduction reduction3D, Reduction reduction2D = null)
        {
            // 2022/06/24 呂宗霖 尚未解謎 為什麼要註解掉
            //return;
            Reductions3D.Insert(_Index == -1 ? 0 : _Index, reduction3D);
            if (reduction2D != null)
            {
                Reductions2D.Insert(_Index == -1 ? 0 : _Index, reduction2D);
            }
            _Index++;
        }
        /// <summary>
        /// 還原3D動作列表
        /// </summary>
        private List<Reduction> Reductions3D { get; set; } = new List<Reduction>();
        private List<Reduction> Reductions2D { get; set; } = new List<Reduction>();
        /// <summary>
        /// 前一個動作
        /// </summary>
        /// <returns>如果沒有上一個動作回傳 null</returns>
        public void Previous()
        {
            if (_Index - 1 < 0)
            {
#if DEBUG
                log4net.LogManager.GetLogger("前一個動作").Debug("找不到前一個動作");
#endif
                return;
            }
            Reduction(Reductions3D[_Index - 1], Reductions2D[_Index - 1]);//還原指定動作
            _Index--;
        }
        /// <summary>
        /// 目前動作變更為修改動作
        /// </summary>
        /// <param name="entities3D">變更過的物件列表</param>
        /// <param name="entities2D"></param>
        /// <remarks>
        /// 變更動作是用刪除與加入組合而成的,
        /// 使用虛擬鍵盤會自動加入刪除動作在列表內
        ///  </remarks>
        public void AddContinuous(List<Entity> entities3D, List<Entity> entities2D = null)
        {
            Reductions3D[_Index - 1].User.Add(ACTION_USER.Add);
            Reductions3D[_Index - 1].Recycle.Add(entities3D);
            if (entities2D != null)
            {
                Reductions2D[_Index - 1].User.Add(ACTION_USER.Add);
                Reductions2D[_Index - 1].Recycle.Add(entities2D);
            }
        }
        /// <summary>
        /// 下一個動作
        /// </summary>
        public void Next()
        {
            if (_Index >= Reductions3D.Count)
            {
#if DEBUG
                log4net.LogManager.GetLogger("下一個動作").Debug("找不到下一個動作");
#endif
                return;
            }

            Reduction result = Reductions3D[_Index];//取得目前動作

            Reduction(Reductions3D[_Index], Reductions2D[_Index]);
            _Index++;
            //return Reductions3D[_Index];
        }
        /// <summary>
        /// 還原動作
        /// </summary>
        /// <param name="reduction3D"></param>
        /// <param name="reduction2D"></param>
        private void Reduction(Reduction reduction3D, Reduction reduction2D)
        {
            try
            {
                ObSettingVM ViewModel = (ObSettingVM)_Model.DataContext;

                //記住用戶的編輯圖塊層
                BlockReference modelReference = _Model.CurrentBlockReference;

                BlockReference drawingReference = _Drawing.CurrentBlockReference;
                if (_Drawing != null)
                    drawingReference= _Drawing.CurrentBlockReference;

                //BlockReference blockr = (BlockReference)reduction3D.SelectReference;//要還原的圖塊層

                //BlockReference blockr = reduction.SelectReference;//要還原的圖塊層

                //if (_Model.CurrentBlockReference != blockr)//如果圖塊名稱不是空值
                if (_Model.CurrentBlockReference != reduction3D.SelectReference)//如果目前圖塊層不是在還原的圖塊層
                {
#if DEBUG
                    log4net.LogManager.GetLogger($"SetCurrent BlockReference").Debug(reduction3D.SelectReference.BlockName);
#endif
                    //層級 To 要編輯的BlockReference
                    _Model.SetCurrent(reduction3D.SelectReference);
                    if (_Drawing != null)
                        _Drawing.SetCurrent(reduction2D.SelectReference);
                }
                log4net.LogManager.GetLogger($"還原動作").Debug("開始");
                for (int i = 0; i < reduction3D.User.Count; i++)
                {

                    switch (reduction3D.User[i]) //判別動作
                    {
                        case ACTION_USER.Add:
                            //刪除物件
                            reduction3D.Recycle[i].ForEach(el =>
                            {
                                _Model.Entities.Remove(el);
                            });
                            reduction2D.Recycle[i].ForEach(el =>
                            {
                                if (_Drawing != null)
                                    _Drawing.Entities.Remove(el);
                            });
                            reduction3D.User[i] = reduction2D.User[i] = ACTION_USER.DELETE;
                            break;
                        case ACTION_USER.DELETE:
                            //插入模型
                            _Model.Entities.AddRange(reduction3D.Recycle[i]);
                            if (_Drawing != null)
                                _Drawing.Entities.AddRange(reduction2D.Recycle[i]);

                            reduction3D.User[i] = reduction2D.User[i] = ACTION_USER.Add;
                            break;
                        case ACTION_USER.MODIFY:
                            throw new Exception($"函式內沒有 {ACTION_USER.MODIFY} 動作處理方法");
                        default:
                            throw new Exception("找不到動作");
                    }
                }
                log4net.LogManager.GetLogger($"還原動作").Debug("結束");
                if (modelReference != reduction3D.SelectReference)//判斷是否要還原圖塊層
                    if (reduction3D.SelectReference == null)
                    {
#if DEBUG
                        log4net.LogManager.GetLogger("圖塊已刪除").Debug("開始");
#endif
                        //層級 To 要編輯的BlockReference
                        _Model.SetCurrent(null);
                        if (_Drawing != null)
                            _Drawing.SetCurrent(null);
                    }
                    else
                    {
#if DEBUG
                        log4net.LogManager.GetLogger("SetCurrent").Debug(modelReference);
#endif
                        //層級 To 要編輯的BlockReference
                        _Model.SetCurrent(modelReference);
                        _Drawing.SetCurrent(drawingReference);
                    }

                //_Model.ZoomFit();
                _Model.Refresh();
                _Drawing.Refresh();
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(e.Message, e.StackTrace);
                throw;
            }

        }
    }
}
