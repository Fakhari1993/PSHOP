using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace PSALE.Classes
{
    class DeleteRelatedInfo
    {
        public void KolInSanad(string _KolCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column03) from Table_065_SanadDetail where Column03='" + _KolCode + "'", Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
             
        }
        public void MoeinInSanad(string _MoeinCode,string _KolCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column04) from Table_065_SanadDetail where Column04='" + _MoeinCode +
                "' and Column03='"+_KolCode+"'", Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
          
        }
        public void TafsiliInSanad(string _TafsiliCode,string _MoeinCode,string _KolCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column05) from Table_065_SanadDetail where Column05='" + _TafsiliCode +
                "' and Column04='"+_MoeinCode+"' and Column03='"+_KolCode+"'", Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
           
        }
        public void JozInSanad(string _JozCode,string _TafsiliCode,string _MoeinCode,string _KolCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column06) from Table_065_SanadDetail where Column06='" + _JozCode + "' and "
                +" Column05='"+_TafsiliCode+"' and Column04='"+_MoeinCode+"' and Column03='"+_KolCode+"'", Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این سرفصل در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
          
        }
        public void PersonInSanad(int _PersonCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column07) from Table_065_SanadDetail where Column07=" + _PersonCode, Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این شخص در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
           

        }
        public void CenterInSanad(Int16 _CenterCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column08) from Table_065_SanadDetail where Column08=" + _CenterCode, Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این مرکز هزینه در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
         
        }
        public void ProjInSanad(Int16 _ProjectCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column09) from Table_065_SanadDetail where Column09=" +_ProjectCode, Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این پروژه در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");
           
        }

        public void CurrencyInSanad(Int16 _CurrencyCode)
        {
            SqlConnection Con = new SqlConnection(Class_BasicOperation.ConStringM());
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Count(Column15) from Table_065_SanadDetail where Column15=" + _CurrencyCode, Con);
            if (Select.ExecuteScalar().ToString() != "0")
                throw new Exception("به علت استفاده از این ارز در اسناد ثبت شده، حذف آن امکانپذیر نمی باشد");

        }

    }
}
