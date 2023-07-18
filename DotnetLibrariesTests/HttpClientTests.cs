using DotnetLibraries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotnetLibrariesTests
{
    public class HttpClientTests
    {
        //private readonly string _url = "http://127.0.0.1/Imrc.FileStorage/api/";
        private readonly string _url = "https://localhost:44341/api/";

        [Test]
        public async Task Test1Async()
        {
            // Arrange
            var client = new HttpClient();
            client.BaseAddress = new Uri(_url);

            var expected = "";

            // Act
            var modelIds = Enumerable.Range(0, 10).Select(x => $"model_{x}").ToArray();
            var dt = CreateFakeDataTable();
            //var model = new DbTable("MODEL", dt.ToDynamic());
            //var detail = new DbTable("MODEL_DETAIL", dt.ToDynamic());
            DbTable model = null;
            DbTable detail = null;
            string json = GlobalFunctions.JsonSerialize(new SnapshotInfo(modelIds, model, detail));
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var response = await client.PostAsync("Snapshot", content);
            }
            var actual = "";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private DataTable CreateFakeDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Score", typeof(double));
            dt.Columns.Add("Birthday", typeof(DateTime));

            foreach (var index in Enumerable.Range(0, 10))
            {
                var row = dt.NewRow();
                row["Id"] = $"Id_{index}";
                row["Name"] = $"Name_{index}";
                row["Score"] = 1 + 0.1 * index;
                row["Birthday"] = new DateTime(2023, 01, 01 + index);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }

    public static class DataTableExtensions
    {
        //public static List<dynamic> ToDynamic(this DataTable dt)
        //{
        //    var dynamicDt = new List<dynamic>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        dynamic dyn = new ExpandoObject();
        //        dynamicDt.Add(dyn);
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            var dic = (IDictionary<string, object>)dyn;
        //            dic[column.ColumnName] = row[column];
        //        }
        //    }
        //    return dynamicDt;
        //}
        public static List<IDictionary<string, object>> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<IDictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
    }

    public class SnapshotInfo
    {
        public string[] ModelIds { get; set; }
        public DbTable ModelTable { get; set; }
        public DbTable ModelDetailTable { get; set; }

        public SnapshotInfo(string[] modelIds, DbTable modelTable, DbTable modelDetailTable)
        {
            ModelIds = modelIds;
            ModelTable = modelTable;
            ModelDetailTable = modelDetailTable;
        }
    }

    public class DbTable
    {
        public string Name { get; set; }
        public List<IDictionary<string, object>> Items { get; set; }

        public DbTable(string name, List<IDictionary<string, object>> items)
        {
            Name = name;
            Items = items;
        }
    }
}