using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DashBoard.Models
{

    [DataContract]
    [Serializable()]
    public class TaskModel
    {
        [DataMember(Name="Date")]
        public string Date { get; set; }

        public DateTime DateTime
        {
            get
            {
                if (this.Date != null)
                    try
                    {
                        return DateTime.ParseExact(this.Date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                         return DateTime.ParseExact(this.Date, "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture);
                    }
                else return new DateTime();
            }
            set { }
        }

        [DataMember(Name="Type")]
        public string Type { get; set; }

        [DataMember(Name ="Status")]
        public string Status { get; set; }

        [DataMember(Name ="Number")]
        public string Number { get; set; }

        [DataMember(Name = "BasicNumber")]
        public string BasicNumber { get; set; }

        [DataMember(Name= "Name")]
        public string Name { get; set; }

        [DataMember(Name="Contact")]
        public string Contact { get; set; }

        [DataMember(Name = "DateAccept")]
        public string DateAccept { get; set; }

        [DataMember(Name = "Content")]
        public string Content { get; set; }

        [DataMember(Name = "Result")]
        public string Result { get; set; }

        [DataMember(Name = "Error")]
        public string Error { get; set; }

        [DataMember(Name = "Payment")]
        public string Payment { get; set; }

        [DataMember(Name = "InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [DataMember(Name = "Messages")]
        public List<MessageModel> Messages { get; set; }

        [DataMember(Name = "AttachedFiles")]
        public List<AttachedFiles> AttachedFiles { get; set; }

        [DataMember(Name = "ExpectedTime")]
        public string ExpDate { get; set; }

        //[DataMember(Name = "ExpectedTime")]
        public string ExpectedTime
        {
            get
            {
                if (this.ExpDate != "01.01.0001 0:00:00")
                    try
                    {
                        return DateTime.ParseExact(this.ExpDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
                    }
                    catch

                    {
                        try
                        {
                            return DateTime.ParseExact(this.ExpDate, "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
                        }
                        catch
                        {
                            return this.ExpDate;
                        }
                    }
                else return "-";
            }
            set { }
        }

        public string PaymentImage
        {
            get
            {
                if (this.Payment == "1")
                {
                    return "<image src=\"/Content/images/icons/money1.png\">";
                }
                else return "<div style=\"height: 20px;\"></div>";
            }
            set
            {

            }
        }


    }
    public class AttachedFiles
    {

        public string FileName { get; set; }
        public string URL { get; set; }
    }
}