using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

namespace Extensions
{
    public static class CSVReader
    {
        public static List<string[]> ReadCSV(string CSVFile, bool isFirstRowTitles = true)
        {
            string[] rawCSV = CSVFile.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> CSVlines = new List<string[]>();

            foreach (string row in rawCSV)
            {
                string[] line = row.Trim().Split(',');
                if (line.Length > 0) CSVlines.Add(line);
            }

            if (isFirstRowTitles) CSVlines.RemoveAt(0);

            return CSVlines;
        }

        public static ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> ReadCSVToObjects(string CSVFile)
        {
            List<string[]> CSVlines = ReadCSV(CSVFile);
            Dictionary<string, ReadOnlyDictionary<string, string>> parsedCSV = new Dictionary<string, ReadOnlyDictionary<string, string>>();
            for (int i = 0; i < CSVlines.Count;)
            {
                KeyValuePair<string, ReadOnlyDictionary<string, string>> CSVObject = ReadObject(CSVlines, i, out i);
                parsedCSV.Add(CSVObject.Key, CSVObject.Value);
            }

            return new ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>(parsedCSV);
        }

        public static string ObjectsDataToCSharp(ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> objectsData, string classNamespace, string className)
        {
            string fileContent = "";
            fileContent += $"namespace {classNamespace}\n";
            fileContent += $"{{\n";
            fileContent += $"public class {className}\n";
            fileContent += $"{{\n";

            foreach (string objectName in objectsData.Keys)
            {
                fileContent += $"public class {objectName}\n";
                fileContent += $"{{\n";
                foreach (KeyValuePair<string, string> objectVariable in objectsData[objectName])
                {
                    string variableName = objectVariable.Key;
                    string variableType = objectVariable.Value.Split(' ')[0];
                    string variableValue = objectVariable.Value.Split(' ')[1];

                    fileContent += $"public const {variableType} {variableName} = {variableValue};\n";
                }
                fileContent += $"}}\n";
            }

            fileContent += $"}}\n";
            fileContent += $"}}\n";

            return fileContent;
        }

        private static KeyValuePair<string, ReadOnlyDictionary<string, string>> ReadObject(List<string[]> CSVlines, int objectStartIndex, out int objectEndIndex)
        {
            objectEndIndex = objectStartIndex;

            if (String.IsNullOrEmpty(CSVlines[objectStartIndex][0]))
            {
                Debug.LogError($"ReadObject Error: Unable to find object name");
                return new KeyValuePair<string, ReadOnlyDictionary<string, string>>("Error", null);
            }

            string objectName = CSVlines[objectStartIndex][0];
            string objectValueType;
            string objectValueName;
            string objectValue;
            Dictionary<string, string> objectValues = new Dictionary<string, string>();

            do
            {
                objectValueType = CSVlines[objectEndIndex][1];
                objectValueName = CSVlines[objectEndIndex][2];
                objectValue = CSVlines[objectEndIndex][3];
                objectValues.Add(objectValueName, $"{objectValueType} {objectValue}");
                objectEndIndex++;
            }
            while (objectEndIndex < CSVlines.Count && String.IsNullOrEmpty(CSVlines[objectEndIndex][0]));

            return new KeyValuePair<string, ReadOnlyDictionary<string, string>>(objectName, new ReadOnlyDictionary<string, string>(objectValues));
        }
    }
}
