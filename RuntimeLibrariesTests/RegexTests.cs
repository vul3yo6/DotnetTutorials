using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RuntimeLibrariesTests
{
    public class RegexTests
    {
        // ref: https://stackoverflow.com/questions/6542996/how-to-split-csv-whose-columns-may-contain-comma
        [Test]
        public void Csv_Test_Case1()
        {
            // Arrange
            var expected = new string[]
            {
                "\"Eko S. Wibowo\"", "\"Tamanan, Banguntapan, Bantul, DIY\"", "\"6/27/1979\"", "",
                "Database.Type", "PGSQL", "Mirle.EDMApi", "6/27/2024", "",
                "Database.ConnectionConfig", "\"{\"IP\":\"{edm-database-ip}\",\"Port\":\"5432\",\"UID\":\"mirle\",\"PWD\":\"Ml22099478!\",\"Database\":\"mirle_edm\"}\"", "Mirle.EDMApi", "\"2024/06/27\""
            };

            // Act
            var target = @"""Eko S. Wibowo"",""Tamanan, Banguntapan, Bantul, DIY"",""6/27/1979"",
Database.Type,PGSQL,Mirle.EDMApi,6/27/2024,
Database.ConnectionConfig,""{""IP"":""{edm-database-ip}"",""Port"":""5432"",""UID"":""mirle"",""PWD"":""Ml22099478!"",""Database"":""mirle_edm""}"",Mirle.EDMApi,""2024/06/27""";
            
            var rows = target.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Regex parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var tokens = new List<string>();
            foreach (var row in rows)
            {
                var subTokens = parser.Split(row);
                tokens.AddRange(subTokens);
            }
            string[] actual = tokens.ToArray();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Csv_Test_Case2()
        {
            // Arrange
            var expected = new string[]
            {
                "IPMAliasSetting", 
                "\"{  \"Item\": {    \"motor\": \"馬達\",    \"motor1\": \"馬達1\",    \"motor2\": \"馬達2\"  },  \"Category\": {    \"lifting_l1\": \"升降模組\",    \"traveling\": \"走行模組\",    \"rotating_r1\": \"迴轉模組\",    \"shift\": \"側移模組\",    \"move\": \"走行模組\",    \"lift\": \"捲揚模組\",    \"Straight\": \"走行模組：直向\",    \"fork\": \"叉臂模組：取貨\"  }}\"", 
                "Mirle.EHMApi",
                "6/27/2024",
                ""
            };

            // Act
            var target = "IPMAliasSetting,\"{  \"Item\": {    \"motor\": \"馬達\",    \"motor1\": \"馬達1\",    \"motor2\": \"馬達2\"  },  \"Category\": {    \"lifting_l1\": \"升降模組\",    \"traveling\": \"走行模組\",    \"rotating_r1\": \"迴轉模組\",    \"shift\": \"側移模組\",    \"move\": \"走行模組\",    \"lift\": \"捲揚模組\",    \"Straight\": \"走行模組：直向\",    \"fork\": \"叉臂模組：取貨\"  }}\",Mirle.EHMApi,6/27/2024,";
            Regex parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            string[] actual = parser.Split(target);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
