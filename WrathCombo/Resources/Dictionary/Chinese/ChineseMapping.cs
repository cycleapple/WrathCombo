using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECommons.Logging;
using System;
using WrathCombo.Resources.Dictionary.Chinese.Skill;
using WrathCombo.Resources.Dictionary.Chinese.Description;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WrathCombo.Resources.Dictionary.Chinese
{
    // 統一替換文字的擴充套件方法
    public static class TextReplacer
    {
        public static string ReplaceWithChinese(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Most preset names and descriptions are complete dictionary keys.
            // Avoid scanning the full fallback dictionary for those exact matches.
            if (ReplacementsDictionary.Replacements.TryGetValue(text, out var exactReplacement))
                return exactReplacement;

#if DEBUG
            DictionaryDebugger.StartReplacementSession(text);
#endif

            var lines = text.Split(['\n'], StringSplitOptions.None);
            bool anyReplacement = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string originalLine = lines[i];
                string replacedLine = originalLine;

                foreach (var pair in ReplacementsDictionary.Replacements)
                {
                    if (replacedLine.Contains(pair.Key))
                    {
                        string beforeReplace = replacedLine;
                        replacedLine = replacedLine.Replace(pair.Key, pair.Value);

                        if (beforeReplace != replacedLine)
                        {
#if DEBUG
                            DictionaryDebugger.RecordUsedKeyInSession(pair.Key, text);
#endif
                            anyReplacement = true;
                        }
                    }
                }

                lines[i] = Regex.Replace(replacedLine, @"\s*([\u4e00-\u9fa5])\s*", "$1");

#if DEBUG
                if (originalLine == replacedLine && !string.IsNullOrWhiteSpace(originalLine) &&
                    !Regex.IsMatch(originalLine, @"^\s*$"))
                {
                    DictionaryDebugger.RecordUnreplacedText(originalLine);
                }
#endif
            }

            return string.Join("\n", lines);
        }
    }

    public static class ReplacementsDictionary
    {
        private static readonly Dictionary<string, string> _replacements = new();
        private static readonly StringBuilder _duplicateKeysLog = new();

        static ReplacementsDictionary()
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                InitializeDictionary();
                stopwatch.Stop();

                PluginLog.Information($"中文替換字典初始化完成，共載入 {_replacements.Count} 個鍵值對，耗時 {stopwatch.ElapsedMilliseconds} 毫秒");

                if (_duplicateKeysLog.Length > 0)
                {
                    PluginLog.Warning($"初始化過程中檢測到重複鍵值對，{_duplicateKeysLog}");
                }
            }
            catch (System.Exception ex)
            {
                PluginLog.Error($"初始化中文替換字典時發生錯誤: {ex.Message}\n{ex.StackTrace}");
                _replacements.Clear();
            }

            #if DEBUG
            DictionaryDebugger.Initialize();
            #endif
        }

        private static void InitializeDictionary()
        {
            // 收集所有鍵值對
            var allKeyValuePairs = new List<KeyValuePair<string, string>>();

            // Prefer the newer translations when the API13 source text matches exactly.
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions1.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions2.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions3.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions4.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions5.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions6.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions7.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions8.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions9.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions10.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions11.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions12.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions13.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions14.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions15.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions16.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions17.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions18.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions19.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions20.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions21.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions22.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions23.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions24.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions25.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions26.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions27.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions28.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions29.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions30.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions31.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions32.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions33.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions34.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions35.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions36.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions37.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions38.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions39.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions40.GetDescriptions());
            allKeyValuePairs.AddRange(LatestTraditionalDescriptions41.GetDescriptions());

            allKeyValuePairs.AddRange(Genesis.GetDescriptions());
            allKeyValuePairs.AddRange(ASTDesc.GetDescriptions());
            allKeyValuePairs.AddRange(PCTDesc.GetDescriptions());
            allKeyValuePairs.AddRange(BLMDesc.GetDescriptions());
            allKeyValuePairs.AddRange(BLUDesc.GetDescriptions());
            allKeyValuePairs.AddRange(BRDDesc.GetDescriptions());
            allKeyValuePairs.AddRange(DNCDesc.GetDescriptions());
            allKeyValuePairs.AddRange(DRGDesc.GetDescriptions());
            allKeyValuePairs.AddRange(DRKDesc.GetDescriptions());
            allKeyValuePairs.AddRange(GNBDesc.GetDescriptions());
            allKeyValuePairs.AddRange(MCHDesc.GetDescriptions());
            allKeyValuePairs.AddRange(MNKDesc.GetDescriptions());
            allKeyValuePairs.AddRange(NINDesc.GetDescriptions());
            allKeyValuePairs.AddRange(PLDDesc.GetDescriptions());
            allKeyValuePairs.AddRange(RDMDesc.GetDescriptions());
            allKeyValuePairs.AddRange(RPRDesc.GetDescriptions());
            allKeyValuePairs.AddRange(SAMDesc.GetDescriptions());
            allKeyValuePairs.AddRange(SCHDesc.GetDescriptions());
            allKeyValuePairs.AddRange(SGEDesc.GetDescriptions());
            allKeyValuePairs.AddRange(SMNDesc.GetDescriptions());
            allKeyValuePairs.AddRange(VPRDesc.GetDescriptions());
            allKeyValuePairs.AddRange(WARDesc.GetDescriptions());
            allKeyValuePairs.AddRange(WHMDesc.GetDescriptions());
            allKeyValuePairs.AddRange(DOLDesc.GetDescriptions());
            allKeyValuePairs.AddRange(DOHDesc.GetDescriptions());
            allKeyValuePairs.AddRange(CommonSkills.GetSkills());            
            allKeyValuePairs.AddRange(VariantSkills.GetSkills());
            allKeyValuePairs.AddRange(DOLSkills.GetSkills());
            allKeyValuePairs.AddRange(Apocalypse.GetDescriptions());

            // 檢測並記錄重複鍵
            var duplicates = allKeyValuePairs
                .GroupBy(kvp => kvp.Key)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicates.Count != 0)
            {
                _duplicateKeysLog.AppendLine($"共檢測到 {duplicates.Count} 個重複鍵:");

                foreach (var group in duplicates)
                {
                    _duplicateKeysLog.AppendLine($"鍵: \"{group.Key}\"，出現 {group.Count()} 次，值:");
                    foreach (var kvp in group)
                    {
                        _duplicateKeysLog.AppendLine($"  - \"{kvp.Value}\"");
                    }
                }
            }

            // 過濾掉重複鍵並填充最終字典
            // 只保留每個鍵的第一個出現
            var uniqueKeyValues = allKeyValuePairs
                .GroupBy(kvp => kvp.Key)
                .Select(g => g.First())
                .Where(kvp => !string.IsNullOrEmpty(kvp.Key));

            foreach (var kvp in uniqueKeyValues)
            {
                _replacements.Add(kvp.Key, kvp.Value);
            }
        }

        public static IReadOnlyDictionary<string, string> Replacements => _replacements;

        public static string GetDuplicateKeysLog()
        {
            return _duplicateKeysLog.ToString();
        }
    }
}
