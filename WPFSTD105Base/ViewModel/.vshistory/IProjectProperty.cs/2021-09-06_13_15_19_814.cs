using GD_STD.Data;
using System;
using System.Collections.ObjectModel;

namespace WPFSTD105.ViewModel
{
    public interface IProjectProperty
    {
        ObservableCollection<BomProperty> BomProperties { get; set; }
        DateTime Create { get; set; }
        string Design { get; set; }
        bool Load { get; set; }
        string Location { get; set; }
        string Name { get; set; }
        string Number { get; set; }
    }
}