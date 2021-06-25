

namespace Roland.WPF.Controls.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    ///     Provide set a functionnalities about the Datagrid
    /// </summary>
    public static class DataGridExtensions
    {
        #region Methods
        /// <summary>
        ///     Retrieve specific cell of the datagrid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int columnIndex)
        {
            if (row == null)
            {
                return null;
            }

            var presenter = VisualHelper.FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null)
            {
                return null;
            }

            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell != null)
            {
                return cell;
            }

            // now try to bring into view and retreive the cell
            grid.ScrollIntoView(row, grid.Columns[columnIndex]);
            cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

            return cell;
        }

        /// <summary>
        ///     Return all rows of the grid
        ///     NOTE : I'm not sure about the method if the datagrid is virtualized and the row not "directly" displayed
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static IEnumerable<DataGridRow> GetDataGridRows(this DataGrid grid)
        {
            IEnumerable itemsSource = grid.ItemsSource;
            if (null == itemsSource)
            {
                yield return null;
            }
            foreach (var item in itemsSource)
            {
                DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row)
                {
                    yield return row;
                }
            }
        }
        #endregion
    }
}
