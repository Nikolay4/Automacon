using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashBoard.Models
{
    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "ID")]
        public string Id { get; set; }

        [DataMember(Name = "ParentID")]
        public string ParentId { get; set; }
        //
        [DataMember(Name = "Text")]//на самом деле text
        public string Question { get; set; }

        [DataMember(Name = "Answer")]
        public string Answer { get; set; }

        [DataMember(Name = "Type")]//на самом деле type
        public string Type { get; set; }

        [DataMember(Name = "User")]
        public string UserName { get; set; }

        [DataMember(Name = "IsProcessed")]
        public bool IsProcessed { get; set; }

        [DataMember(Name = "IsRead")]
        public bool IsRead { get; set; }

        [DataMember(Name = "Date")]
        public string DateTime { get; set; }

        [DataMember(Name = "AnswerDateTime")]
        public string AnswerDateTime { get; set; }

        public string children { get { return "{}"; } set { } }
    }


    [DataContract]
    [Serializable()]
    public class Types
    {
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string TypePresent { get; set; }
    }


    [DataContract]
    [Serializable()]
    public class Contacts
    {
        [DataMember]
        public string Name { get; set; }
    }
}



//{
//"ID": "ИП-00013339.01.002.00001",
//"ParentID": "",
//"Type": "Выполнить основную работу по заданию",
//"Text": "",
//"Date": "20160815093938",
//"User": "Беляк Николай",
//"TaskID": "ИП-00013339.01",
//"TaskStatus": "Выдано"
//},