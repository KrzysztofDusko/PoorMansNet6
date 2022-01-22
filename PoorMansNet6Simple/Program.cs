
//PoorMansNet6Console
/*
Poor Man's T-SQL Formatter - a small free Transact-SQL formatting 
library for .Net 2.0 and JS, written in C#. 
Copyright (C) 2011-2013 Tao Klerks

Additional Contributors:
 * Timothy Klenke, 2012

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

//dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=true --self-contained false

using System;
using System.Collections.Generic;
using System.Text;
//using System.IO;
using System.Reflection;
using ExtraFormatter;


namespace ExtraFormatter
{
    public class Formatter
    {
        public string DoFormat(string inputSql)
        {
            //formatter engine option defaults
            var options = new PoorMansTSqlFormatterLib.Formatters.TSqlStandardFormatterOptions
            {
                KeywordStandardization = true,
                IndentString = "\t",
                SpacesPerTab = 4,
                MaxLineWidth = 999,
                NewStatementLineBreaks = 2,
                NewClauseLineBreaks = 1,
                TrailingCommas = false,
                SpaceAfterExpandedComma = true,
                ExpandBetweenConditions = true,
                ExpandBooleanExpressions = true,
                ExpandCaseStatements = true,
                ExpandCommaLists = true,
                BreakJoinOnSections = true,
                UppercaseKeywords = true,
                ExpandInLists = true
            };

            //bulk formatter options
            bool allowParsingErrors = false;
            List<string> extensions = new List<string>();
            bool backups = true;
            bool recursiveSearch = false;

            string uiLangCode = null;

            //flow/tracking switches
            bool showUsageFriendly = false;
            bool showUsageError = false;

            //OptionSet p = new OptionSet()
            //  .Add("is|indentString=", delegate (string v) { options.IndentString = v; })
            //  .Add("st|spacesPerTab=", delegate (string v) { options.SpacesPerTab = int.Parse(v); })
            //  .Add("mw|maxLineWidth=", delegate (string v) { options.MaxLineWidth = int.Parse(v); })
            //  .Add("sb|statementBreaks=", delegate (string v) { options.NewStatementLineBreaks = int.Parse(v); })
            //  .Add("cb|clauseBreaks=", delegate (string v) { options.NewClauseLineBreaks = int.Parse(v); })
            //  .Add("tc|trailingCommas", delegate (string v) { options.TrailingCommas = v != null; })
            //  .Add("sac|spaceAfterExpandedComma", delegate (string v) { options.SpaceAfterExpandedComma = v != null; })
            //  .Add("ebc|expandBetweenConditions", delegate (string v) { options.ExpandBetweenConditions = v != null; })
            //  .Add("ebe|expandBooleanExpressions", delegate (string v) { options.ExpandBooleanExpressions = v != null; })
            //  .Add("ecs|expandCaseStatements", delegate (string v) { options.ExpandCaseStatements = v != null; })
            //  .Add("ecl|expandCommaLists", delegate (string v) { options.ExpandCommaLists = v != null; })
            //  .Add("eil|expandInLists", delegate (string v) { options.ExpandInLists = v != null; })
            //  .Add("bjo|breakJoinOnSections", delegate (string v) { options.BreakJoinOnSections = v != null; })
            //  .Add("uk|uppercaseKeywords", delegate (string v) { options.UppercaseKeywords = v != null; })
            //  .Add("sk|standardizeKeywords", delegate (string v) { options.KeywordStandardization = v != null; })

            //  .Add("ae|allowParsingErrors", delegate (string v) { allowParsingErrors = v != null; })
            //  .Add("e|extensions=", delegate (string v) { extensions.Add((v.StartsWith(".") ? "" : ".") + v); })
            //  .Add("r|recursive", delegate (string v) { recursiveSearch = v != null; })
            //  .Add("b|backups", delegate (string v) { backups = v != null; })
            //  .Add("o|outputFileOrFolder=", delegate (string v) { outputFileOrFolder = v; })
            //  .Add("l|languageCode=", delegate (string v) { uiLangCode = v; })
            //  .Add("h|?|help", delegate (string v) { showUsageFriendly = v != null; })
            //      ;

            //first parse the args
            //List<string> remainingArgs = p.Parse(args);

            //then switch language if necessary

            string stdInput = inputSql;//File.ReadAllText(inputSql);

            if (extensions.Count == 0)
                extensions.Add(".sql");

            var formatter = new PoorMansTSqlFormatterLib.Formatters.TSqlStandardFormatter(options);
            formatter.ErrorOutputPrefix = Environment.NewLine;
            var formattingManager = new PoorMansTSqlFormatterLib.SqlFormattingManager(formatter);

            bool warningEncountered = false;
            if (!string.IsNullOrEmpty(stdInput))
            {
                string formattedOutput = null;
                bool parsingError = false;
                Exception parseException = null;
                try
                {
                    formattedOutput = formattingManager.Format(stdInput, ref parsingError);

                    //hide any handled parsing issues if they were requested to be allowed
                    if (allowParsingErrors) parsingError = false;
                }
                catch (Exception ex)
                {
                    parseException = ex;
                    parsingError = true;
                }
                return formattedOutput;

                //if (!string.IsNullOrEmpty(outputFileOrFolder))
                //{
                //WriteResultFile(outputFileOrFolder, null, null, ref warningEncountered, formattedOutput);
                //}
                //else
                //{
                //    Console.WriteLine(formattedOutput);
                //}
            }
            else
            {
                return stdInput;
            }
        }

        /*
        private static bool ProcessSearchResults(List<string> extensions, bool backups, bool allowParsingErrors, PoorMansTSqlFormatterLib.SqlFormattingManager formattingManager, FileSystemInfo[] matchingObjects, StreamWriter singleFileWriter, string replaceFromFolderPath, string replaceToFolderPath, ref bool warningEncountered)
        {
            bool fileFound = false;

            foreach (var fsEntry in matchingObjects)
            {
                if (fsEntry is FileInfo)
                {
                    if (extensions.Contains(fsEntry.Extension))
                    {
                        ReFormatFile((FileInfo)fsEntry, formattingManager, backups, allowParsingErrors, singleFileWriter, replaceFromFolderPath, replaceToFolderPath, ref warningEncountered);
                        fileFound = true;
                    }
                }
                else
                {
                    if (ProcessSearchResults(extensions, backups, allowParsingErrors, formattingManager, ((System.IO.DirectoryInfo)fsEntry).GetFileSystemInfos(), singleFileWriter, replaceFromFolderPath, replaceToFolderPath, ref warningEncountered))
                        fileFound = true;
                }
            }

            return fileFound;
        }

        private static void ReFormatFile(FileInfo fileInfo, PoorMansTSqlFormatterLib.SqlFormattingManager formattingManager, bool backups, bool allowParsingErrors, StreamWriter singleFileWriter, string replaceFromFolderPath, string replaceToFolderPath, ref bool warningEncountered)
        {
            bool failedBackup = false;
            string oldFileContents = "";
            string newFileContents = "";
            bool parsingError = false;
            bool failedFolder = false;
            Exception parseException = null;

            //TODO: play with / test encoding complexities
            //TODO: consider using auto-detection - read binary, autodetect, convert.
            //TODO: consider whether to keep same output encoding as source file, or always use same, and if so whether to make parameter-based.
            try
            {
                oldFileContents = System.IO.File.ReadAllText(fileInfo.FullName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format("Failed to read file contents (aborted): {0}", fileInfo.FullName));
                Console.Error.WriteLine(string.Format(" Error detail: {0}", ex.Message));
                warningEncountered = true;
            }
            if (oldFileContents.Length > 0)
            {
                try
                {
                    newFileContents = formattingManager.Format(oldFileContents, ref parsingError);

                    //hide any handled parsing issues if they were requested to be allowed
                    if (allowParsingErrors) parsingError = false;
                }
                catch (Exception ex)
                {
                    parseException = ex;
                    parsingError = true;
                }

                if (parsingError)
                {
                    Console.Error.WriteLine(string.Format("Encountered error when parsing or formatting file contents (aborted): {0}", fileInfo.FullName));
                    if (parseException != null)
                        Console.Error.WriteLine(string.Format(" Error detail: {0}", parseException.Message));
                    warningEncountered = true;
                }
            }
            if (!parsingError
                && (
                        (newFileContents.Length > 0
                        && !oldFileContents.Equals(newFileContents)
                        )
                        || singleFileWriter != null
                        || (replaceFromFolderPath != null && replaceToFolderPath != null)
                    )
                )

            {
                if (backups)
                {
                    try
                    {
                        fileInfo.CopyTo(fileInfo.FullName + ".bak", true);
                    }
                    catch (Exception)
                    {
                        failedBackup = true;
                        warningEncountered = true;
                        throw;
                    }
                }
                if (!failedBackup)
                {
                    if (singleFileWriter != null)
                    {
                        //we'll assume that running out of disk space, and other while-you-are-writing errors, and not worth worrying about
                        singleFileWriter.WriteLine(newFileContents);
                        singleFileWriter.WriteLine("GO");
                    }
                    else
                    {
                        string fullTargetPath = fileInfo.FullName;
                        if (replaceFromFolderPath != null && replaceToFolderPath != null)
                        {
                            fullTargetPath = fullTargetPath.Replace(replaceFromFolderPath, replaceToFolderPath);

                            string targetFolder = Path.GetDirectoryName(fullTargetPath);
                            try
                            {
                                if (!Directory.Exists(targetFolder))
                                    Directory.CreateDirectory(targetFolder);
                            }
                            catch (Exception)
                            {
                                failedFolder = true;
                                warningEncountered = true;
                                throw;
                            }
                        }

                        if (!failedFolder)
                        {
                            WriteResultFile(fullTargetPath, replaceFromFolderPath, replaceToFolderPath, ref warningEncountered, newFileContents);
                        }
                    }
                }
            }
        }
        */
        /*
        private static void WriteResultFile(string targetFilePath, string replaceFromFolderPath, string replaceToFolderPath, ref bool warningEncountered, string newFileContents)
        {
            try
            {
                File.WriteAllText(targetFilePath, newFileContents);
            }
            catch (Exception)
            {
                throw;
            }
        }
        */
    }
}
