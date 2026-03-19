using System.Text;

namespace AirPressure.Services
{
    /// <summary>
    /// Minimal CSV exporter: single static method ExportAsync
    /// Writes visible columns and rows to CSV (UTF8 with BOM) using a temp file then atomic move.
    /// </summary>
    public static class CsvExporter
    {
        public static async Task ExportAsync(DataGridView grid, string filePath)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            string tempPath = Path.GetTempFileName();
            try
            {
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var sw = new StreamWriter(fs, new UTF8Encoding(true))) // BOM
                {
                    var visibleCols = grid.Columns
                        .Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .OrderBy(c => c.DisplayIndex)
                        .ToArray();

                    // header
                    var headers = visibleCols.Select(c => Escape(c.HeaderText));
                    await sw.WriteLineAsync(string.Join(',', headers));

                    // rows
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        if (row.IsNewRow) continue;
                        var cells = new List<string>(visibleCols.Length);
                        foreach (var col in visibleCols)
                        {
                            var v = row.Cells[col.Index].Value;
                            cells.Add(Escape(v?.ToString() ?? string.Empty));
                        }
                        await sw.WriteLineAsync(string.Join(',', cells));
                    }

                    await sw.FlushAsync();
                }

                if (File.Exists(filePath)) File.Delete(filePath);
                File.Move(tempPath, filePath);
            }
            catch
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
                throw;
            }
        }

        private static string Escape(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            if (input.Contains(',') || input.Contains('"') || input.Contains('\r') || input.Contains('\n'))
            {
                input = input.Replace("\"", "\"\"");
                return '"' + input + '"';
            }
            return input;
        }
    }
}
