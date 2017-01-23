using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading;
using System.Web;

namespace Asterisk.Utilities
{
  public class ImportDataFromCSV
  {
    private readonly string _path;
    private readonly string _tempFolder;
    private readonly string _filePath;
    private readonly HttpPostedFileBase _file;

    public ImportDataFromCSV(HttpPostedFileBase file, HttpServerUtilityBase server)
    {
      if (file == null) return;

      _file = file;
      _tempFolder = Guid.NewGuid().ToString();
      _filePath = System.IO.Path.Combine(server.MapPath("~/Excel"), _tempFolder);
      System.IO.Directory.CreateDirectory(_filePath);
      var currentFile = System.IO.Path.GetFileName(file.FileName);
      if (currentFile != null) _path = System.IO.Path.Combine(_filePath, currentFile);
    }

    public ImportDataFromCSV(HttpServerUtilityBase server)
    {
      throw new NotImplementedException();
    }

    public bool SaveCSVData(Action<DataRow> saveItem, int numberOfColumns)
    {
      if (IsFileCSV())
      {
        var dataTable = ReadCSVToDataTable(_path).AsEnumerable();
        if (IsDataTableDataValid(dataTable, numberOfColumns))
        {
          foreach (var dataRow in dataTable)
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
      System.IO.File.Delete(_path);
      Thread.Sleep(200);
      System.IO.Directory.Delete(_filePath);
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
      var rtn = true;
      foreach (var dataRow in enumerableRowCollection.Where(dataRow => dataRow.ItemArray.Length != numberOfColumns))
      {
        rtn = false;
      }
      return rtn;
    }

    private DataTable ReadCSVToDataTable(string filename)
    {
      var path = System.IO.Path.GetDirectoryName(filename);
      var file = System.IO.Path.GetFileName(filename);

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