using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MarkAndFill.Base.Utils;
using MarkAndFill.Model;

namespace MarkAndFill.Base.Managers;

public class WordManager
{
    public List<Mark>? GetUniqueTags(string filePath)
    {
        var uniqueTags = new List<Mark>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл шаблона не найден.", filePath);

        using (var doc = WordprocessingDocument.Open(filePath, false))
        {
            if (GetBodyDocument(doc) is Body body)
            {
                var tagPattern = new Regex(@"\{\{[^}]+\}\}");

                foreach (var paragraph in body.Descendants<Paragraph>())
                {
                    var paragraphText = paragraph.InnerText;

                    var matches = tagPattern.Matches(paragraphText);

                    foreach (Match match in matches) uniqueTags.Add(new Mark(match.Value));
                }
            }
        }

        return uniqueTags;
    }

    public void ReplaceTags(string filePath, Dictionary<string, string> tagValues)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл шаблона не найден.", filePath);

        // Открываем документ для редактирования (isEditable: true)
        using (var doc = WordprocessingDocument.Open(filePath, true))
        {
            if (GetBodyDocument(doc) is Body body)
            {
                // Получаем все абзацы один раз
                var paragraphs = body.Descendants<Paragraph>().ToList();

                foreach (var tag in tagValues)
                {
                    // Экранируем фигурные скобки, так как в Regex они являются спецсимволами группировки
                    var pattern = Regex.Escape(tag.Key);
                    var regex = new Regex(pattern, RegexOptions.Compiled);

                    //OpenXmlRegex: заменяет текст, сохраняя <w:rPr> (форматирование)
                    OpenXmlRegex.Replace(paragraphs, regex, tag.Value);
                }

                // Сохраняем изменения
                doc.MainDocumentPart!.Document!.Save();
            }
        }
    }

    private Body? GetBodyDocument(WordprocessingDocument document)
    {
        return document?.MainDocumentPart?.Document?.Body;
    }
}