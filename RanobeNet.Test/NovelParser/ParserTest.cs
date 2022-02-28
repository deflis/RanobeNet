using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using RanobeNet.NovelParser;
using MockQueryable.NSubstitute;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RanobeNet.Test.NovelParser
{

    [TestFixture]
    public class ParserTest
    {
        private Parser parser = new Parser();
        private JsonSerializerOptions options = new JsonSerializerOptions();

        [SetUp]
        public void Setup()
        {
            options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }
        [Test]
        public void ルビを変換できる()
        {
            var parsed = parser.parse(@"カクヨム互換

冴えない彼女《ヒロイン》の育てかた
あいつの｜etc《えとせとら》
この際｜紅蓮の炎《ヘルフレイム》に焼かれて果てろ！

ノベルアップ互換
わたし、ノベラといいます。女神（めがみ）やってます！
わたし、ノベラといいます。女神｜（めがみ）やってます！
");

            var expected = new List<List<dynamic>>
            {
                new List<dynamic>
                {
                    "カクヨム互換"
                },
                new List<dynamic>
                {
                },
                new List<dynamic>
                {
                    "冴えない",
                    new { type= "ruby", text= "彼女", yomi= "ヒロイン" },
                    "の育てかた"
                },
                new List<dynamic>
                {
                    "あいつの",
                    new { type= "ruby", text= "etc", yomi= "えとせとら" },
                },
                new List<dynamic>
                {
                    "この際",
                    new { type= "ruby", text= "紅蓮の炎", yomi= "ヘルフレイム" },
                    "に焼かれて果てろ！",
                },
                new List<dynamic>
                {
                },
                new List<dynamic>
                {
                    "ノベルアップ互換"
                },
                new List<dynamic>
                {
                    "わたし、ノベラといいます。",
                    new { type= "ruby", text= "女神", yomi= "めがみ" },
                    "やってます！"
                },
                new List<dynamic>
                {
                    "わたし、ノベラといいます。女神（めがみ）やってます！"
                },
                new List<dynamic>
                {
                },
            };

            var parsedJson = JsonSerializer.Serialize(parsed, options);

            var expectedJson = JsonSerializer.Serialize(expected, options);

            Assert.AreEqual(expectedJson, parsedJson);
        }
        [Test]
        public async Task 傍点を変換できる()
        {
            var parsed = parser.parse(@"おじいさんは山へ《《柴刈り》》に出かけました。");

            var expected = new List<List<dynamic>>
            {
                new List<dynamic>
                {
                    "おじいさんは山へ",
                    new { type= "bouten", text= "柴刈り"},
                    "に出かけました。"
                },
            };

            var parsedJson = JsonSerializer.Serialize(parsed, options);

            var expectedJson = JsonSerializer.Serialize(expected, options);

            Assert.AreEqual(expectedJson, parsedJson);
        }
        [Test]
        public async Task 空行を変換できる()
        {
            var parsed = parser.parse(@"
おじいさんは山へ《《柴刈り》》に出かけました。
");

            var expected = new List<List<dynamic>>
            {
                new List<dynamic>
                {
                },
                new List<dynamic>
                {
                    "おじいさんは山へ",
                    new { type= "bouten", text= "柴刈り"},
                    "に出かけました。"
                },
                new List<dynamic>
                {
                },
            };

            var parsedJson = JsonSerializer.Serialize(parsed, options);

            var expectedJson = JsonSerializer.Serialize(expected, options);

            Assert.AreEqual(expectedJson, parsedJson);
        }

    }
}
