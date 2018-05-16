using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduateSoftware.Models
{
    public class GraduateModel
    {
        public string StudentID { get; set; }
        public string GraduateName { get; set; }
        public string GraduateLastName { get; set; }
        public Nullable<int> GraduateYear { get; set; }
        public Nullable<int> WorkAreaID { get; set; }
        public Nullable<int> WorkAreaDetailID { get; set; }
        public string GraduateCompany { get; set; }
        public string GraduateTitle { get; set; }
        public string GraduateMail { get; set; }
        public string GraduatePhone { get; set; }
        public string StudentPassword { get; set; }
        public SelectList Alanlar { get; set; }
        public virtual AdminGraduateVerification adminGraduate { get; set; }
    }
}