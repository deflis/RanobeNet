using System.Text.RegularExpressions;

namespace RanobeNet.NovelParser
{
    public class Parser
    {
        private static readonly Regex regexpForSplit = new Regex("([|｜](?:《.+?》|《《.+?》》|（[あ-んア-ン]+?）)|(?:(?:[々〇〻\u3400-\u9FFF\uF900-\uFAFF]|[\uD840-\uD87F][\uDC00-\uDFFF])+?|[|｜].+?)《.+?》|《《.+?》》|(?:(?:[々〇〻\u3400-\u9FFF\uF900-\uFAFF]|[\uD840-\uD87F][\uDC00-\uDFFF])+?|[|｜].+?)（[あ-んア-ン]+?）)");

        private static readonly Regex regexpForParseRuby = new Regex("^[|｜]?(.+)[《（](.+)[》）]$");
        private static readonly Regex regexpForParseNotRuby = new Regex("^[|｜]([《（].+[》）]|《《(.+)》》)$");
        private static readonly Regex regexpForParseBouten = new Regex("^《《(.+)》》$");


        public List<List<dynamic>> parse(string novel)
        {
            return novel
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)
                .Select(line =>
                    parseLine(line.Trim())
                    .Select<INovelLineElement, dynamic>(element => element.type == Type.Text ? element.text : element)
                    .ToList()
                )
                .ToList();
        }

        private List<INovelLineElement> parseLine(string line)
        {
            return regexpForSplit.Split(line).Where(x => x != "").Select<string, INovelLineElement>((element) =>
            {
                var notRuby = regexpForParseNotRuby.Match(element);
                if (notRuby.Success)
                {
                    return new TextElement(notRuby.Groups[1].Value);
                }
                var bouten = regexpForParseBouten.Match(element);
                if (bouten.Success)
                {
                    return new BoutenElement(bouten.Groups[1].Value);
                }
                var ruby = regexpForParseRuby.Match(element);
                if (ruby.Success)
                {
                    return new RubyElement(ruby.Groups[1].Value, ruby.Groups[2].Value);
                }
                return new TextElement(element);
            }).Aggregate(new List<INovelLineElement>(), (list, element) =>
            {
                if (element.type == Type.Text && list.Count > 0 && list[list.Count - 1].type == Type.Text)
                {
                    var last = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    list.Add(new TextElement(last.text + element.text));
                }
                else
                {
                    list.Add(element);
                }
                return list;
            });
        }


        public interface INovelLineElement
        {
            public Type type { get; }
            public string text { get; }
        }

        public class TextElement : INovelLineElement
        {
            public Type type { get; } = Type.Text;
            public string text { get; }

            internal TextElement(string text)
            {
                this.text = text;
            }
        }

        public class BoutenElement : INovelLineElement
        {
            public Type type { get; } = Type.Bouten;
            public string text { get; }

            internal BoutenElement(string text)
            {
                this.text = text;
            }
        }
        public class RubyElement : INovelLineElement
        {
            public Type type { get; } = Type.Ruby;
            public string text { get; }
            public string yomi { get; }

            internal RubyElement(string text, string yomi)
            {
                this.text = text;
                this.yomi = yomi;
            }
        }

        public enum Type
        {
            Text,
            Ruby,
            Bouten
        }
    }

}
