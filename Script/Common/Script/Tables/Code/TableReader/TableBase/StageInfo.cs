﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class StageInfoRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Level { get; set; }
        public STAGE_TYPE StageType { get; set; }
        public string FightLogicPath { get; set; }
        public string ScenePath { get; set; }
        public string Audio { get; set; }
        public string Icon { get; set; }
        public string BG { get; set; }
        public List<string> AwardType { get; set; }
        public List<string> AwardValue { get; set; }
        public StageInfoRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            AwardType = new List<string>();
            AwardValue = new List<string>();
        }
        public string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Level));
            recordStrList.Add(((int)StageType).ToString());
            recordStrList.Add(TableWriteBase.GetWriteStr(FightLogicPath));
            recordStrList.Add(TableWriteBase.GetWriteStr(ScenePath));
            recordStrList.Add(TableWriteBase.GetWriteStr(Audio));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(BG));
            foreach (var testTableItem in AwardType)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in AwardValue)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class StageInfo : TableFileBase
    {
        public Dictionary<string, StageInfoRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public StageInfoRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("StageInfo" + ": " + id, ex);
            }
        }

        public StageInfo(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, StageInfoRecord>();
            if(isPath)
            {
                string[] lines = File.ReadAllLines(pathOrContent, Encoding.Default);
                lines[0] = lines[0].Replace("\r\n", "\n");
                ParserTableStr(string.Join("\n", lines));
            }
            else
            {
                ParserTableStr(pathOrContent.Replace("\r\n", "\n"));
            }
        }

        private void ParserTableStr(string content)
        {
            StringReader rdr = new StringReader(content);
            using (var reader = new CsvReader(rdr))
            {
                HeaderRecord header = reader.ReadHeaderRecord();
                while (reader.HasMoreRecords)
                {
                    DataRecord data = reader.ReadDataRecord();
                    if (data[0].StartsWith("#"))
                        continue;

                    StageInfoRecord record = new StageInfoRecord(data);
                    Records.Add(record.Id, record);
                }
            }
        }

        public void CoverTableContent()
        {
            foreach (var pair in Records)
            {
                pair.Value.Name = TableReadBase.ParseString(pair.Value.ValueStr[1]);
                pair.Value.Desc = TableReadBase.ParseString(pair.Value.ValueStr[2]);
                pair.Value.Level = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.StageType =  (STAGE_TYPE)TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.FightLogicPath = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.ScenePath = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.Audio = TableReadBase.ParseString(pair.Value.ValueStr[7]);
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[8]);
                pair.Value.BG = TableReadBase.ParseString(pair.Value.ValueStr[9]);
                pair.Value.AwardType.Add(TableReadBase.ParseString(pair.Value.ValueStr[10]));
                pair.Value.AwardType.Add(TableReadBase.ParseString(pair.Value.ValueStr[11]));
                pair.Value.AwardType.Add(TableReadBase.ParseString(pair.Value.ValueStr[12]));
                pair.Value.AwardType.Add(TableReadBase.ParseString(pair.Value.ValueStr[13]));
                pair.Value.AwardValue.Add(TableReadBase.ParseString(pair.Value.ValueStr[14]));
                pair.Value.AwardValue.Add(TableReadBase.ParseString(pair.Value.ValueStr[15]));
                pair.Value.AwardValue.Add(TableReadBase.ParseString(pair.Value.ValueStr[16]));
                pair.Value.AwardValue.Add(TableReadBase.ParseString(pair.Value.ValueStr[17]));
            }
        }
    }

}