using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Asterisk.Utilities
{
  public class ImportDataFromCSV
  {
    private readonly HttpPostedFileBase _file;
    private readonly string _filePath;
    private readonly string _path;
    private readonly string _tempFolder;

    public ImportDataFromCSV(HttpPostedFileBase file, HttpServerUtilityBase server)
    {
      if (file == null) return;

      _file = file;
      _tempFolder = Guid.NewGuid().ToString();
      _filePath = Path.Combine(server.MapPath("~/Excel"), _tempFolder);
      Directory.CreateDirectory(_filePath);
      string currentFile = Path.GetFileName(file.FileName);
      if (currentFile != null) _path = Path.Combine(_filePath, currentFile);
    }

    public ImportDataFromCSV(HttpServerUtilityBase server)
    {
      throw new NotImplementedException();
    }

    public bool SaveCSVData(Action<DataRow> saveItem, int numberOfColumns)
    {
      if (IsFileCSV())
      {
        EnumerableRowCollection<DataRow> dataTable = ReadCSVToDataTable(_path).AsEnumerable();
        if (IsDataTableDataValid(dataTable, numberOfColumns))
        {
          foreach (DataRow dataRow in dataTable)
          {
            saveItem(dataRow);
          }
          CleanDownFiles();
          return true;
        }
        CleanDownFiles();
        return false;
      }
      return false;
    }

    private void CleanDownFiles()
    {
      File.Delete(_path);
      Thread.Sleep(200);
      Directory.Delete(_filePath);
    }


    private bool IsFileCSV()
    {
      if (_file == null || !_file.FileName.EndsWith(".csv"))
      {
        return false;
      }
      _file.SaveAs(_path);
      return true;
    }

    private bool IsDataTableDataValid(IEnumerable<DataRow> enumerableRowCollection, int numberOfColumns)
    {
      bool rtn = true;
      foreach (DataRow dataRow in enumerableRowCollection.Where(dataRow => dataRow.ItemArray.Length != numberOfColumns))
      {
        rtn = false;
      }
      return rtn;
    }

    private DataTable ReadCSVToDataTable(string filename)
    {
      string path = Path.GetDirectoryName(filename);
      string file = Path.GetFileName(filename);

      var dt = new DataTable();
      using (
        var conn =
          new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path +
                              "';Extended Properties='text;HDR=No;FMT=Delimited'"))
      {
        var da = new OleDbDataAdapter("SELECT * FROM " + file, conn);
        da.Fill(dt);
      }
      return dt;
    }
  }
}