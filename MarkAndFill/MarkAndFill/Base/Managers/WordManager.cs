using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MarkAndFill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkAndFill.Base.Managers
{
    public class WordManager
    {
        public List<Mark>? GetUniqueTags(string filePath) 
        {
            var uniqueTags = new List<Mark>();

            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false)) 
            {
                if (GetBodyDocument(doc) is Body body)
                {
                    var tagPattern = new Regex(@"\{\{[^}]+\}\}");

                    foreach (Paragraph paragraph in body.Descendants<Paragraph>()) 
                    {
                        string paragraphText = paragraph.InnerText;

                        var matches = tagPattern.Matches(paragraphText);

                        foreach (Match match in matches) 
                        {
                            uniqueTags.Add(new Mark(match.Value));
                        }

                    }
                }
            }

            return uniqueTags;
        }

        public void ReplaceTags(string filePath, Dictionary<string, string> tagValues) 
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true)) 
            {
                if (GetBodyDocument(doc) is Body body)
                {
                    foreach (Paragraph paragraph in body.Descendants<Paragraph>()) 
                    {
                        string paragraphText = paragraph.InnerText;
                        string updatedText = paragraphText;
                        foreach (var tag in tagValues)
                        {
                            updatedText = updatedText.Replace(tag.Key, tag.Value);
                        }

                        RunProperties? firstRunProps = null;
                        var firstRun = paragraph.Descendants<Run>().FirstOrDefault();
                        if (firstRun?.RunProperties != null)
                        {
                            firstRunProps = firstRun.RunProperties.CloneNode(true) as RunProperties;
                        }

                        // Очищаем старые раны
                        paragraph.RemoveAllChildren<Run>();

                        // Создаем новый ран с замененным текстом
                        Run newRun = new Run();

                        // Восстанавливаем форматирование
                        if (firstRunProps != null)
                        {
                            newRun.AppendChild(firstRunProps);
                        }

                        newRun.AppendChild(new Text(updatedText) { Space = SpaceProcessingModeValues.Preserve });
                        paragraph.AppendChild(newRun);
                    }

                    doc.MainDocumentPart!.Document!.Save();
                }

            }
        }

        private Body? GetBodyDocument(WordprocessingDocument document) 
        {
            return document?.MainDocumentPart?.Document?.Body;
        }
    }
}
