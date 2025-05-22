using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

public class PersistenceScriptReadme
{
    public string readme => ToString();
    public string readmeTitle = string.Empty;
    public int readmeSeparatorLineLength = 120;
    private readonly List<string> readmeLines = new List<string>();

    public void AddReadmeLines(string[] readmeLinesContent)
    {
        foreach (var readmeLine in readmeLinesContent)
        {
            AddReadmeLine(readmeLine);
        }
    }

    public void AddReadmeLine(string readmeLine) => readmeLines.Add(readmeLine);

    public override string ToString()
    {
        var builder = new StringBuilder();
        if (readmeLines.Count > 0)
            builder.AppendLine(SeparatorLine(readmeSeparatorLineLength, $"{readmeTitle} Readme Title"));
        foreach (var readmeLine in readmeLines)
        {
            builder.AppendLine(readmeLine);
        }

        if (readmeLines.Count > 0)
            builder.AppendLine(SeparatorLine(readmeSeparatorLineLength + 7, String.Empty));
        return builder.ToString();
    }

    private string SeparatorLine(int totalLength, string centerText)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<");
        var halfSeparatorCount = (int)((totalLength - centerText.Length) * 0.5f);
        for (int i = 0; i < totalLength - centerText.Length; i++)
        {
            if (i == halfSeparatorCount)
            {
                builder.Append(centerText);
                continue;
            }

            builder.Append("-");
        }

        builder.Append(">");
        return builder.ToString();
    }
}