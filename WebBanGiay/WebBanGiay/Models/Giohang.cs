using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanGiay.Models
{
    public class Giohang
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
        public int iMagiay { get; set; }
        public string sTengiay { get; set; }
        public string sAnhbia { get; set; }
        public decimal dDongia { get; set; }
        public int iSoluong { get; set; }
        public decimal dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
       
        public Giohang (int MaGiay)
        {
            iMagiay = MaGiay;
            GIAY giay = data.GIAYs.Single(n => n.MaGIAY == iMagiay);
            sTengiay = giay.TenGIAY;
            sAnhbia = giay.Anhbia;
            dDongia = decimal.Parse(giay.Giaban.ToString());
            iSoluong = 1;
        }
    }
}