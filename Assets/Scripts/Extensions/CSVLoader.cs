using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Cysharp.Threading.Tasks;

using Extensions;

using UnityEngine;
using UnityEngine.Networking;

namespace MyFirstVisualNovel.Extensions
{
    public static class CSVLoader
    {
        /// <summary>
        /// Загружает CSV из Google Sheets по ссылке (gid берётся из ссылки)
        /// </summary>
        /// <param name="sheetURL">Ссылка типа https://docs.google.com/spreadsheets/d/ID/edit?gid=XXX</param>
        /// <returns>Массив строк и ячеек</returns>
        public static async UniTask<string[][]> DownloadGoogleSheetAsync(string sheetURL)
        {
            string sheetID = ExtractID(sheetURL);
            string gid = ExtractGid(sheetURL);

            if (string.IsNullOrEmpty(sheetID))
            {
                Debug.LogError("CSVLoader: Не удалось извлечь ID таблицы");
                return null;
            }

            if (string.IsNullOrEmpty(gid))
            {
                Debug.LogError("CSVLoader: Не удалось извлечь gid таблицы");
                gid = "0"; // по умолчанию первая вкладка
            }

            string csvURL = $"https://docs.google.com/spreadsheets/d/{sheetID}/export?format=csv&gid={gid}";

            string csvString = await DownloadCSV(csvURL);
            if (string.IsNullOrEmpty(csvString))
            {
                Debug.LogError("CSVLoader: не удалось скачать CSV");
                return null;
            }

            return ParseCSVString(csvString);
        }

        public static string[][] LoadCSVFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"CSVLoader: файл не найден {filePath}");
                return new string[0][];
            }

            string csvString = File.ReadAllText(filePath);
            return ParseCSVString(csvString);
        }

        /// <summary>
        /// Загружает текст по URL
        /// </summary>
        private static async UniTask<string> DownloadCSV(string csvURL)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(csvURL))
            {
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"CSVLoader download error: {request.error}");
                    return null;
                }

                if (request.downloadHandler.text.IsNullOrEmpty())
                    Debug.LogError($"CSVLoader downloaded file is empty");
                    
                return request.downloadHandler.text;
            }
        }

        /// <summary>
        /// Извлекает Spreadsheet ID из ссылки
        /// </summary>
        private static string ExtractID(string URL)
        {
            Match match = Regex.Match(URL, @"\/d\/([a-zA-Z0-9-_]+)");
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;
            return null;
        }

        /// <summary>
        /// Извлекает gid листа из ссылки
        /// </summary>
        private static string ExtractGid(string URL)
        {
            Match match = Regex.Match(URL, @"[?&]gid=(\d+)");
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;
            return null;
        }

        /// <summary>
        /// Парсит CSV в массив массивов строк
        /// </summary>
        private static string[][] ParseCSVString(string csvString)
        {
            if (string.IsNullOrEmpty(csvString))
                return new string[0][];

            var result = new List<List<string>>();
            var row = new List<string>();
            var stringBuilder = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < csvString.Length; i++)
            {
                char c = csvString[i];

                if (c == '"')
                {
                    if (i + 1 < csvString.Length && csvString[i + 1] == '"')
                    {
                        stringBuilder.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    row.Add(stringBuilder.ToString());
                    stringBuilder.Clear();
                }
                else if (c == '\n' && !inQuotes)
                {
                    row.Add(stringBuilder.ToString());
                    stringBuilder.Clear();
                    result.Add(new List<string>(row));
                    row.Clear();
                }
                else if (c == '\r')
                {
                    continue;
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            if (stringBuilder.Length > 0 || row.Count > 0)
            {
                row.Add(stringBuilder.ToString());
                result.Add(new List<string>(row));
            }

            var finalResult = new string[result.Count][];
            for (int i = 0; i < result.Count; i++)
                finalResult[i] = result[i].ToArray();

            return finalResult;
        }
    }
}
