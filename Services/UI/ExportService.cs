﻿using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiveChartPlay.Services.UI
{
    public interface IExportService
    {
        void ExportTo(ExportType fileType, string fileName);
        void ShowPreview();
    }

    public class ExportService : ServiceBase, IExportService
    {
        public TableView View
        {
            get { return (TableView)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(TableView), typeof(ExportService), new PropertyMetadata(null));

        public void ExportTo(ExportType fileType, string fileName)
        {
            if (View == null) return;
            switch (fileType)
            {
                case ExportType.XLSX:
                    View.ExportToXlsx(fileName);
                    break;
                case ExportType.PDF:
                    View.ExportToPdf(fileName);
                    break;
            }
        }

        public void ShowPreview()
        {
            if (View == null) return;

            var rootWindow = LayoutTreeHelper.GetVisualParents(AssociatedObject).FirstOrDefault(x => x is Window) as Window;
            if (rootWindow != null)
                View.ShowPrintPreviewDialog(rootWindow);
        }
    }

    public enum ExportType
    {
        XLSX,
        PDF
    }
}
