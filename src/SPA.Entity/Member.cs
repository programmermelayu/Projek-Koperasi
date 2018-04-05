using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.Core;
using SPA.Data;
using System.Data.SqlClient;
 
namespace SPA.Entity 
{
    public class Member : EntityBase, IEntity
    {
        public int ID { get; set; }
        public string NewIC { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Age { get; set; }
        public string Birthdate { get; set; }
        public int CategoryID  { get; set; }
        public int CitizenshipID { get; set; }
        public int StatusID { get; set; }
        public int RaceID { get; set; }
        public int ReligionID { get; set; }
        public int SexID { get; set; }
        public int MaritalStatusID { get; set; }

        public string PermanentAddress { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentPostcode { get; set; }
        public int PermanentStateID { get; set; }

        public string CurrentAddress { get; set; }
        public string CurrentDistrict { get; set; }
        public string CurrentPostcode { get; set; }
        public int CurrentStateID { get; set; }

        public string OfficePositionTitle { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeDistrict { get; set; }
        public string OfficePostcode { get; set; }
        public int OfficeStateID { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeEmail { get; set; }
        public string OfficeRetiredDate { get; set; }

        public string PersonalEmail { get; set; }
        public string PersonalHomePhone { get; set; }
        public string PersonalMobilePhone { get; set; }

        public string MemberStartDate { get; set; }
        public string MemberEndDate { get; set; }

        public bool MemberStart{ get; set; }
        public bool MemberEnd { get; set; }

        private List<MemberWasi> _memberWasis;
        public List<MemberWasi> MemberWasis
        {
            get
            {
                if (_memberWasis == null)
                {
                    _memberWasis = new List<MemberWasi>();
                }
                return _memberWasis;
            }
            set
            {
                _memberWasis = value;
            }

        }
        
        public string Sql { get; set; }
        public string ErrorMessage { get; set; }
        private string successMessage = "Maklumat berjaya disimpan!";
        public string SuccessMessage
        {
            get { 
                return successMessage; 
            }
            set { 
                successMessage = value;
            }
        }

        public Member() : base("Members")
        {
               
        }
        public bool Create()
        {
            if (!Validate())
            {
                return false;
            }
            else
            {
                if (ID > 0)
                {
                    return this.Update();
                }
                else
                {
                    return this.Insert();
                }
            }
        }

        private bool Insert()
        {
            BaseInsertBuilder.AddField("Name", Name);
            BaseInsertBuilder.AddField("Code", Code);
            BaseInsertBuilder.AddField("NewIC", NewIC);
            BaseInsertBuilder.AddField("Age", Age, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("Birthdate", Birthdate, Enums.Data.DataType.Date);

            BaseInsertBuilder.AddField("CitizenshipID", CitizenshipID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("RaceID", RaceID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("ReligionID", ReligionID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("SexID", SexID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("MaritalStatusID", MaritalStatusID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("StatusID", (int)Enums.MemberEnum.Status.NotActive, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("CategoryID", CategoryID, Enums.Data.DataType.Number);

            BaseInsertBuilder.AddField("PermanentAddress", PermanentAddress);
            BaseInsertBuilder.AddField("PermanentPostcode", PermanentPostcode);
            BaseInsertBuilder.AddField("PermanentDistrict", PermanentDistrict);
            BaseInsertBuilder.AddField("PermanentStateID", PermanentStateID, Enums.Data.DataType.Number);

            BaseInsertBuilder.AddField("CurrentAddress", CurrentAddress);
            BaseInsertBuilder.AddField("CurrentPostcode", CurrentPostcode);
            BaseInsertBuilder.AddField("CurrentDistrict", CurrentDistrict);
            BaseInsertBuilder.AddField("CurrentStateID", CurrentStateID, Enums.Data.DataType.Number);

            BaseInsertBuilder.AddField("OfficeAddress", OfficeAddress);
            BaseInsertBuilder.AddField("OfficePostcode", OfficePostcode);
            BaseInsertBuilder.AddField("OfficeDistrict", OfficeDistrict);
            BaseInsertBuilder.AddField("OfficeStateID", OfficeStateID, Enums.Data.DataType.Number);
            BaseInsertBuilder.AddField("OfficePhone", OfficePhone);
            BaseInsertBuilder.AddField("OfficeFax", OfficeFax);
            BaseInsertBuilder.AddField("OfficeEmail", OfficeEmail);
            BaseInsertBuilder.AddField("OfficePositionTitle", OfficePositionTitle);
            BaseInsertBuilder.AddField("OfficeRetiredDate", OfficeRetiredDate, Enums.Data.DataType.Date);

            BaseInsertBuilder.AddField("PersonalHomePhone", PersonalHomePhone);
            BaseInsertBuilder.AddField("PersonalMobilePhone", PersonalMobilePhone);
            BaseInsertBuilder.AddField("PersonalEmail", PersonalEmail);

            if (MemberStart)
            {
                BaseInsertBuilder.AddField("StartMembership", 1, Enums.Data.DataType.Boolean);
                BaseInsertBuilder.AddField("MembershipDate", MemberStartDate, Enums.Data.DataType.Date);
            }
            else
            {
                BaseInsertBuilder.AddField("StartMembership", 0, Enums.Data.DataType.Boolean);
            }
            if (MemberEnd)
            {
                BaseInsertBuilder.AddField("EndMembership", 1, Enums.Data.DataType.Boolean);
                BaseInsertBuilder.AddField("EndMembershipDate", MemberEndDate, Enums.Data.DataType.Date);
            }
            else
            {
                BaseInsertBuilder.AddField("EndMembership", 0, Enums.Data.DataType.Boolean);
            }

            base.ExecuteInsert();

            ID = GetID();
            if (MemberWasis.Count > 0)
            {
                foreach (var wasi in MemberWasis)
                {
                    wasi.MemberID = this.ID;
                    wasi.Create();                    
                }
            }

            return true; 
        }

        private bool Update()
        {
            BaseUpdateBuilder.AddField("Name", Name);
            BaseUpdateBuilder.AddField("Code", Code);
            BaseUpdateBuilder.AddField("NewIC", NewIC);
            BaseUpdateBuilder.AddField("Age", Age, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("Birthdate", Birthdate, Enums.Data.DataType.Date);

            BaseUpdateBuilder.AddField("CitizenshipID", CitizenshipID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("RaceID", RaceID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("ReligionID", ReligionID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("SexID", SexID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("MaritalStatusID", MaritalStatusID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("StatusID", StatusID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("CategoryID", CategoryID, Enums.Data.DataType.Number);

            BaseUpdateBuilder.AddField("PermanentAddress", PermanentAddress);
            BaseUpdateBuilder.AddField("PermanentPostcode", PermanentPostcode);
            BaseUpdateBuilder.AddField("PermanentDistrict", PermanentDistrict);
            BaseUpdateBuilder.AddField("PermanentStateID", PermanentStateID, Enums.Data.DataType.Number);


            BaseUpdateBuilder.AddField("CurrentAddress", CurrentAddress);
            BaseUpdateBuilder.AddField("CurrentPostcode", CurrentPostcode);
            BaseUpdateBuilder.AddField("CurrentDistrict", CurrentDistrict);
            BaseUpdateBuilder.AddField("CurrentStateID", CurrentStateID, Enums.Data.DataType.Number);

            BaseUpdateBuilder.AddField("OfficeAddress", OfficeAddress);
            BaseUpdateBuilder.AddField("OfficePostcode", OfficePostcode);
            BaseUpdateBuilder.AddField("OfficeDistrict", OfficeDistrict);
            BaseUpdateBuilder.AddField("OfficeStateID", OfficeStateID, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddField("OfficePhone", OfficePhone);
            BaseUpdateBuilder.AddField("OfficeFax", OfficeFax);
            BaseUpdateBuilder.AddField("OfficeEmail", OfficeEmail);
            BaseUpdateBuilder.AddField("OfficePositionTitle", OfficePositionTitle);
            BaseUpdateBuilder.AddField("OfficeRetiredDate", OfficeRetiredDate, Enums.Data.DataType.Date);

            BaseUpdateBuilder.AddField("PersonalHomePhone", PersonalHomePhone);
            BaseUpdateBuilder.AddField("PersonalMobilePhone", PersonalMobilePhone);
            BaseUpdateBuilder.AddField("PersonalEmail", PersonalEmail);

            if (MemberStart)
            {
                BaseUpdateBuilder.AddField("StartMembership", 1, Enums.Data.DataType.Boolean);
                BaseUpdateBuilder.AddField("MembershipDate", MemberStartDate, Enums.Data.DataType.Date);
            }
            else
            {
                BaseUpdateBuilder.AddField("StartMembership", 0, Enums.Data.DataType.Boolean);
            }
            if (MemberEnd)
            {
                BaseUpdateBuilder.AddField("EndMembership", 1, Enums.Data.DataType.Boolean);
                BaseUpdateBuilder.AddField("EndMembershipDate", MemberEndDate, Enums.Data.DataType.Date);
            }
            else
            {
                BaseUpdateBuilder.AddField("EndMembership", 0, Enums.Data.DataType.Boolean);
            }

            base.BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number, SPA.Enums.Data.AssignmentOperator.IsEqual);

            base.ExecuteUpdate();

            if (MemberWasis.Count > 0) MemberWasis.ForEach(x => x.Create());
            return true;

        }
        public bool Delete()
        {
            base.BaseDeleteBuilder.AddWhereField("ID", ID);

            base.ExecuteDelete();

            if (MemberWasis.Count > 0) MemberWasis.ForEach(x => x.Delete());

            return true;
        }

        private bool Validate()
        {
            if (String.IsNullOrEmpty(NewIC))
            {
                ErrorMessage  = "Nombor Kad Pengenalan tidak diisi.";
                return false;
            }
            if (CheckExist())
            {
                ErrorMessage = string.Format("Ahli dengan Nombor Kad Pengenalan {0} sudah wujud!", this.NewIC);
            }
            //check if the member already exist based on NoKPBaru
            return true;
        }

        public bool CheckExist()
        {
            return new MemberReader().ReadSingle(new FilterElement() { Key = Enums.Data.KeyElements.MemberKP, Value = this.NewIC });
        }

        public override int GetID()
        {
            var reader = new MemberReader();
            if (reader.ReadSingle(new FilterElement() { Key = Enums.Data.KeyElements.MemberKP, Value = this.NewIC }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        public override int GetID(Enums.Data.KeyElements key, object value)
        {
            var reader = new MemberReader();
            if (reader.ReadSingle(new FilterElement() { Key = key, Value = value }))
            {
                return reader.SingleRecord.ID;
            }
            else
            {
                return -1;
            }
        }

        public bool IsActive()
        {
            if (lsAlreadyActive()) return true;

            SelectBuilder sb = new SelectBuilder();
            var strHeader = "SELECT TOP 1 * FROM payments WHERE MemberID = " + this.ID + " AND AccountID = ";
            sb.Sql = strHeader + (int)Enums.PaymentEnum.AccountID.YuranBulanan;
            sb.Sql += " UNION " + strHeader + (int)Enums.PaymentEnum.AccountID.KebajikanDermasiswa;
            sb.Sql += " UNION " + strHeader + (int)Enums.PaymentEnum.AccountID.Saham;
            sb.Sql += " UNION " +  strHeader + (int)Enums.PaymentEnum.AccountID.FiMasuk;

            SqlDataReader dr = DbReader.Execute(sb.Sql);
            int countRow = 0;
            if (dr.HasRecord())
            {
                while (dr.Read())
                {
                    countRow++;
                }
            }
            return (countRow == 4) ? true : false;
        }

        private bool lsAlreadyActive()
        {
            var sql = "SELECT YuranBulanan, TabungKebajikan, Saham, FiMasuk FROM PaymentsTotal WHERE MemberID = " + this.ID;
            SqlDataReader dr = DbReader.Execute(sql);
            if(dr.HasRecord())
            {
                while (dr.Read())
                {
                    var yuranBulanan = dr.GetValueDouble("YuranBulanan");
                    var tabungKebajikan = dr.GetValueDouble("TabungKebajikan");
                    var saham = dr.GetValueDouble("Saham");
                    var FiMasuk = dr.GetValueDouble("FiMasuk");
                    if  ((yuranBulanan > 0) && (tabungKebajikan > 0) && (saham > 0) && (FiMasuk > 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateStatus()
        {
            int statusId = (int)Enums.MemberEnum.Status.NotActive;
            if (IsActive())
            {
                statusId = (int)Enums.MemberEnum.Status.Active;
            }
            BaseUpdateBuilder.AddField("StatusID", statusId, Enums.Data.DataType.Number);
            BaseUpdateBuilder.AddWhereField("ID", ID, Enums.Data.DataType.Number, SPA.Enums.Data.AssignmentOperator.IsEqual);
           
            base.ExecuteUpdate();
        }

    }
}