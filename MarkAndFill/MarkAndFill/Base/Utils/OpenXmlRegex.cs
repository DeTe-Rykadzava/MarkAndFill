using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MarkAndFill.Base.Utils;

public static class OpenXmlRegex
{
    public static void Replace(IEnumerable<Paragraph> paragraphs, Regex regex, string replacement)
    {
        foreach (var paragraph in paragraphs) Replace(paragraph, regex, replacement);
    }

    public static void Replace(Paragraph paragraph, Regex regex, string replacement)
    {
        if (paragraph == null) return;

        // Собираем все текстовые элементы внутри абзаца
        var textElements = paragraph.Descendants<Text>().ToList();
        if (!textElements.Any()) return;

        // Склеиваем текст для поиска совпадений
        var fullText = string.Join("", textElements.Select(t => t.Text ?? ""));
        var matches = regex.Matches(fullText);

        if (matches.Count == 0) return;

        // ВАЖНО: Идем по совпадениям С КОНЦА. 
        // Это нужно, чтобы при изменении длины текста в конце абзаца не сбивались 
        // индексы (match.Index) для тегов, находящихся в начале абзаца.
        for (var i = matches.Count - 1; i >= 0; i--)
        {
            var match = matches[i];
            var matchStart = match.Index;
            var matchEnd = match.Index + match.Length;

            var replacementInserted = false;
            var currentPos = 0;

            foreach (var textElement in textElements)
            {
                var elementLength = textElement.Text?.Length ?? 0;

                // Пустые элементы не занимают места в строке
                if (elementLength == 0) continue;

                var elementStart = currentPos;
                var elementEnd = currentPos + elementLength;

                // Элемент находится строго ДО или строго ПОСЛЕ тега
                var isBeforeMatch = elementEnd <= matchStart;
                var isAfterMatch = elementStart >= matchEnd;

                if (isBeforeMatch || isAfterMatch)
                {
                    currentPos = elementEnd;
                    continue;
                }

                // Элемент ПЕРЕСЕКАЕТСЯ с тегом. Вычисляем точные границы среза.
                var startCut = Math.Max(elementStart, matchStart);
                var endCut = Math.Min(elementEnd, matchEnd);

                // Текст до тега внутри этого элемента
                var textBefore = textElement.Text.Substring(0, startCut - elementStart);
                // Текст после тега внутри этого элемента
                var textAfter = textElement.Text.Substring(endCut - elementStart);

                var newText = textBefore;

                // Вставляем замену только в ПЕРВЫЙ элемент, который затронул тег
                if (!replacementInserted)
                {
                    newText += replacement;
                    replacementInserted = true;
                }

                newText += textAfter;
                textElement.Text = newText;

                currentPos = elementEnd;
            }
        }
    }
}