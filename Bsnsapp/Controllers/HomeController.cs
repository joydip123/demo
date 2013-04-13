using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Bsnsapp.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Bsnsapp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Welcome/
        private readonly IDocumentSession _documentSession;
        public HomeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;

        }
        [ActionName("Index")]
        public ActionResult Index()
        {
            Vendor vd = new Vendor();
            vd.Manufacturers = GetManufacturerList();

            vd.StateModel = new List<State1>();
            vd.StateModel = GetAllState1();
            //////////////////////////////////////////////////////
            var addrs = new List<String>
                    {
                       "No Data"
                    };
            ViewData["addrs"] = addrs;
            ///////////////////////////////////////////////////////
            var addrs2 = new List<String>
                    {
                       "."
                    };
            ViewData["lat_lon"] = addrs2;
            ///////////////////////////////////////////////////////
            var addrs3 = new List<String>
                    {
                       "."
                    };
            ViewData["city_zip"] = addrs3;
            return View(vd);
        }
        [HttpPost]
        public ActionResult Index(Vendor vndr)
        {
            //Vendor vd = new Vendor();
            if (!ModelState.IsValid)

                vndr.Country = "";
            ////////////////////get country name from View/////////////////////////////////////////////////////////
            List<State1> objCountry = new List<State1>();
            objCountry = GetAllState1().Where(m => m.Id.ToString() == Request.Form["Country"]).ToList();
            var objCoun = objCountry[0].StateName;
            vndr.Country = objCoun;
            /////////////////////////////////////Checking category exist or not//////////////////////////////////////////////////////////
            var retrv_qry = _documentSession.Query<Vendor>().Where(x => x.Country == objCoun && x.State == vndr.State && x.City == vndr.City && x.Catgry == vndr.Catgry).Take(50).ToList();
            //////////////////////////////////////take the view in same condition---Part-1--///////////////////////////////////////////
            vndr.StateModel = new List<State1>();
            vndr.StateModel = GetAllState1();
            vndr.Manufacturers = GetManufacturerList();
            /////////////////////////////////////////////////////////////////--Part-2--//////////////////////////////////////
            string address = "", LtLn = "", zip = "";

            for (int i = 0; i < retrv_qry.Count; i++)
            {
                address = address + retrv_qry[i].Address1 + " " + retrv_qry[i].Pin + " " + retrv_qry[i].Phone + "~";
                //  address = address + retrv_qry[i].Address1 + " " + retrv_qry[i].Pin + " " + retrv_qry[i].Phone+Environment.NewLine;
                zip = retrv_qry[i].Pin;
                LtLn = LtLn + retrv_qry[i].Latitude + "," + retrv_qry[i].Longitude + "~";
            }

            if (retrv_qry != null)
            {
                var addrs = new List<String>
                    {
                       address
                       //retrv_qry[0].Address1,
                       //retrv_qry[1].Address1
                    };
                ViewData["addrs"] = addrs;
                /////////////////////////////////////////////////////////
                var addrs2 = new List<String>
                    {
                      LtLn
                    };
                ViewData["lat_lon"] = addrs2;
                /////////////////////////////////////////////////////////
                var addrs3 = new List<String>
                    {
                      zip
                    };
                ViewData["city_zip"] = addrs3;
            }
            else
            {
                var addrs = new List<String>
                    {
                       "No Data"
                    };
                ViewData["addrs"] = addrs;
                ////////////////////////////////////////////////////////
                var addrs2 = new List<String>
                    {
                       "."
                    };
                ViewData["lat_lon"] = addrs2;
                ////////////////////////////////////////////////////////
                var addrs3 = new List<String>
                    {
                       "."
                    };
                ViewData["city_zip"] = addrs3;
            }
            return View(vndr);
        }
        private List<SelectListItem> GetManufacturerList()
        {
            List<SelectListItem> manuList = new List<SelectListItem>();
            manuList = (from categories in _documentSession.Query<Category>()
                        select
                            new SelectListItem
                            {
                                //Value = categories.Id.ToString(),
                                Text = categories.Categories
                            }
                      ).Distinct().ToList();
            return manuList;
        }
        public List<State1> GetAllState1()
        {

            List<State1> objstate = new List<State1>();
            objstate.Add(new State1 { Id = 0, StateName = "Select Country" });
            objstate.Add(new State1 { Id = 1, StateName = "AFGHANISTAN" });
            objstate.Add(new State1 { Id = 2, StateName = "ALAND ISLANDS" });
            objstate.Add(new State1 { Id = 3, StateName = "ALBANIA" });
            objstate.Add(new State1 { Id = 4, StateName = "ALGERIA" });
            objstate.Add(new State1 { Id = 5, StateName = "AMERICAN SAMOA" });
            objstate.Add(new State1 { Id = 6, StateName = "ANDORRA" });
            objstate.Add(new State1 { Id = 7, StateName = "AMERICAN SAMOA" });
            objstate.Add(new State1 { Id = 8, StateName = "ANGOLA" });
            objstate.Add(new State1 { Id = 9, StateName = "ANGUILLA" });
            objstate.Add(new State1 { Id = 10, StateName = "ANTARCTICA," });
            objstate.Add(new State1 { Id = 11, StateName = "ANTIGUA AND BARBUDA" });
            objstate.Add(new State1 { Id = 12, StateName = "ARGENTINA" });
            objstate.Add(new State1 { Id = 13, StateName = "ARMENIA" });
            objstate.Add(new State1 { Id = 14, StateName = "ARUBA" });
            objstate.Add(new State1 { Id = 15, StateName = "AUSTRALIA" });
            objstate.Add(new State1 { Id = 16, StateName = "AZERBAIJAN" });
            objstate.Add(new State1 { Id = 17, StateName = "BAHAMAS" });
            objstate.Add(new State1 { Id = 18, StateName = "BAHRAIN" });
            objstate.Add(new State1 { Id = 19, StateName = "BANGLADESH" });
            objstate.Add(new State1 { Id = 20, StateName = "BARBADOS" });
            objstate.Add(new State1 { Id = 21, StateName = "BELARUS" });
            objstate.Add(new State1 { Id = 22, StateName = "BELGIUM" });
            objstate.Add(new State1 { Id = 23, StateName = "BELIZE" });
            objstate.Add(new State1 { Id = 24, StateName = "BENIN" });
            objstate.Add(new State1 { Id = 25, StateName = "BERMUDA" });
            objstate.Add(new State1 { Id = 26, StateName = "BHUTAN" });
            objstate.Add(new State1 { Id = 27, StateName = "BONAIRE" });
            objstate.Add(new State1 { Id = 28, StateName = "BOLIVIA" });
            objstate.Add(new State1 { Id = 29, StateName = "BOSNIA AND HERZEGOVINA" });
            objstate.Add(new State1 { Id = 30, StateName = "BOTSWANA" });
            objstate.Add(new State1 { Id = 31, StateName = "BOUVET ISLAND" });
            objstate.Add(new State1 { Id = 32, StateName = "BRAZIL" });
            objstate.Add(new State1 { Id = 33, StateName = "BRITISH INDIAN OCEAN TERRITORY" });
            objstate.Add(new State1 { Id = 34, StateName = "BRUNEI DARUSSALAM" });
            objstate.Add(new State1 { Id = 35, StateName = "BULGARIA" });
            objstate.Add(new State1 { Id = 36, StateName = "BURKINA FASO" });
            objstate.Add(new State1 { Id = 37, StateName = "BURUNDI" });
            objstate.Add(new State1 { Id = 38, StateName = "CAMBODIA" });
            objstate.Add(new State1 { Id = 39, StateName = "CAMEROON" });
            objstate.Add(new State1 { Id = 40, StateName = "CANADA" });
            objstate.Add(new State1 { Id = 41, StateName = "CAPE VERDE" });
            objstate.Add(new State1 { Id = 42, StateName = "CAYMAN ISLANDS" });
            objstate.Add(new State1 { Id = 43, StateName = "CENTRAL AFRICAN REPUBLIC" });
            objstate.Add(new State1 { Id = 44, StateName = "CHAD" });
            objstate.Add(new State1 { Id = 45, StateName = "CHILE" });
            objstate.Add(new State1 { Id = 46, StateName = "CHINA" });
            objstate.Add(new State1 { Id = 47, StateName = "CHRISTMAS ISLAND" });
            objstate.Add(new State1 { Id = 48, StateName = "COCOS (KEELING) ISLANDS" });
            objstate.Add(new State1 { Id = 49, StateName = "COLOMBIA" });
            objstate.Add(new State1 { Id = 50, StateName = "COMOROS" });
            objstate.Add(new State1 { Id = 51, StateName = "CONGO" });
            objstate.Add(new State1 { Id = 52, StateName = "COOK ISLANDS" });
            objstate.Add(new State1 { Id = 53, StateName = "COSTA RICA" });
            objstate.Add(new State1 { Id = 54, StateName = "COTE D IVOIRE" });
            objstate.Add(new State1 { Id = 55, StateName = "CROATIA" });
            objstate.Add(new State1 { Id = 56, StateName = "CUBA" });
            objstate.Add(new State1 { Id = 57, StateName = "CURAÇAO" });
            objstate.Add(new State1 { Id = 58, StateName = "CYPRUS" });
            objstate.Add(new State1 { Id = 59, StateName = "CZECH REPUBLIC" });
            objstate.Add(new State1 { Id = 60, StateName = "DENMARK" });
            objstate.Add(new State1 { Id = 61, StateName = "DJIBOUTI" });
            objstate.Add(new State1 { Id = 62, StateName = "DOMINICA" });
            objstate.Add(new State1 { Id = 63, StateName = "DOMINICAN REPUBLIC" });
            objstate.Add(new State1 { Id = 64, StateName = "ECUADOR" });
            objstate.Add(new State1 { Id = 65, StateName = "EGYPT" });
            objstate.Add(new State1 { Id = 66, StateName = "EL SALVADOR" });
            objstate.Add(new State1 { Id = 67, StateName = "EQUATORIAL GUINEA" });
            objstate.Add(new State1 { Id = 68, StateName = "ERITREA" });
            objstate.Add(new State1 { Id = 69, StateName = "ESTONIA" });
            objstate.Add(new State1 { Id = 70, StateName = "ETHIOPIA" });
            objstate.Add(new State1 { Id = 71, StateName = "FALKLAND ISLANDS (MALVINAS)" });
            objstate.Add(new State1 { Id = 72, StateName = "FAROE ISLANDS" });
            objstate.Add(new State1 { Id = 73, StateName = "FIJI" });
            objstate.Add(new State1 { Id = 74, StateName = "FINLAND" });
            objstate.Add(new State1 { Id = 75, StateName = "FRANCE" });
            objstate.Add(new State1 { Id = 76, StateName = "FRENCH GUIANA" });
            objstate.Add(new State1 { Id = 77, StateName = "FRENCH POLYNESIA" });
            objstate.Add(new State1 { Id = 78, StateName = "FRENCH SOUTHERN TERRITORIES" });
            objstate.Add(new State1 { Id = 79, StateName = "GABON" });
            objstate.Add(new State1 { Id = 80, StateName = "GAMBIA" });
            objstate.Add(new State1 { Id = 81, StateName = "GEORGIA" });
            objstate.Add(new State1 { Id = 82, StateName = "GERMANY" });
            objstate.Add(new State1 { Id = 83, StateName = "GHANA" });
            objstate.Add(new State1 { Id = 84, StateName = "GIBRALTAR" });
            objstate.Add(new State1 { Id = 85, StateName = "GREECE" });
            objstate.Add(new State1 { Id = 86, StateName = "GREENLAND" });
            objstate.Add(new State1 { Id = 87, StateName = "GRENADA" });
            objstate.Add(new State1 { Id = 88, StateName = "GUADELOUPE" });
            objstate.Add(new State1 { Id = 89, StateName = "GUAM" });
            objstate.Add(new State1 { Id = 90, StateName = "GUATEMALA" });
            objstate.Add(new State1 { Id = 91, StateName = "GUERNSEY" });
            objstate.Add(new State1 { Id = 92, StateName = "GUINEA" });
            objstate.Add(new State1 { Id = 93, StateName = "GUINEA-BISSAU" });
            objstate.Add(new State1 { Id = 94, StateName = "GUYANA" });
            objstate.Add(new State1 { Id = 95, StateName = "HAITI" });
            objstate.Add(new State1 { Id = 96, StateName = "HEARD ISLAND AND MCDONALD ISLANDS" });
            objstate.Add(new State1 { Id = 97, StateName = "HOLY SEE (VATICAN CITY STATE)" });
            objstate.Add(new State1 { Id = 98, StateName = "HONDURAS" });
            objstate.Add(new State1 { Id = 99, StateName = "HONG KONG" });
            objstate.Add(new State1 { Id = 100, StateName = "HUNGARY" });
            objstate.Add(new State1 { Id = 101, StateName = "ICELAND" });
            objstate.Add(new State1 { Id = 102, StateName = "INDIA" });
            objstate.Add(new State1 { Id = 103, StateName = "INDONESIA" });
            objstate.Add(new State1 { Id = 104, StateName = "IRAN" });
            objstate.Add(new State1 { Id = 105, StateName = "ISLAMIC REPUBLIC OF" });
            objstate.Add(new State1 { Id = 106, StateName = "IRAQ" });
            objstate.Add(new State1 { Id = 107, StateName = "IRELAND" });
            objstate.Add(new State1 { Id = 108, StateName = "ISLE OF MAN" });
            objstate.Add(new State1 { Id = 109, StateName = "ISRAEL" });
            objstate.Add(new State1 { Id = 110, StateName = "ITALY" });
            objstate.Add(new State1 { Id = 111, StateName = "JAMAICA" });
            objstate.Add(new State1 { Id = 112, StateName = "JAPAN" });
            objstate.Add(new State1 { Id = 113, StateName = "JERSEY" });
            objstate.Add(new State1 { Id = 114, StateName = "JORDAN" });
            objstate.Add(new State1 { Id = 115, StateName = "KAZAKHSTAN" });
            objstate.Add(new State1 { Id = 116, StateName = "KENYA" });
            objstate.Add(new State1 { Id = 117, StateName = "KIRIBATI" });
            objstate.Add(new State1 { Id = 118, StateName = "KOREA" });
            objstate.Add(new State1 { Id = 119, StateName = "DEMOCRATIC PEOPLE'S REPUBLIC OF" });
            objstate.Add(new State1 { Id = 120, StateName = "KOREA, REPUBLIC OF" });
            objstate.Add(new State1 { Id = 121, StateName = "KUWAIT" });
            objstate.Add(new State1 { Id = 123, StateName = "KYRGYZSTAN" });
            objstate.Add(new State1 { Id = 124, StateName = "LAO PEOPLE'S DEMOCRATIC REPUBLIC" });
            objstate.Add(new State1 { Id = 125, StateName = "LATVIA" });
            objstate.Add(new State1 { Id = 126, StateName = "LEBANON" });
            objstate.Add(new State1 { Id = 127, StateName = "LESOTHO" });
            objstate.Add(new State1 { Id = 128, StateName = "LIBERIA" });
            objstate.Add(new State1 { Id = 129, StateName = "LIBYA" });
            objstate.Add(new State1 { Id = 130, StateName = "LIECHTENSTEIN" });
            objstate.Add(new State1 { Id = 131, StateName = "LITHUANIA" });
            objstate.Add(new State1 { Id = 132, StateName = "LUXEMBOURG" });
            objstate.Add(new State1 { Id = 133, StateName = "MACAO" });
            objstate.Add(new State1 { Id = 134, StateName = "MACEDONIA" });
            objstate.Add(new State1 { Id = 135, StateName = "MADAGASCAR" });
            objstate.Add(new State1 { Id = 136, StateName = "MALAWI" });
            objstate.Add(new State1 { Id = 137, StateName = "MALAYSIA" });
            objstate.Add(new State1 { Id = 138, StateName = "MALDIVES" });
            objstate.Add(new State1 { Id = 139, StateName = "MALI" });
            objstate.Add(new State1 { Id = 140, StateName = "MALTA" });
            objstate.Add(new State1 { Id = 141, StateName = "MARSHALL ISLANDS" });
            objstate.Add(new State1 { Id = 142, StateName = "MARTINIQUE" });
            objstate.Add(new State1 { Id = 143, StateName = "MAURITANIA" });
            objstate.Add(new State1 { Id = 144, StateName = "MAURITIUS" });
            objstate.Add(new State1 { Id = 145, StateName = "MAYOTTE" });
            objstate.Add(new State1 { Id = 146, StateName = "MEXICO" });
            objstate.Add(new State1 { Id = 147, StateName = "MICRONESIA" });
            objstate.Add(new State1 { Id = 148, StateName = "FEDERATED STATES OF" });
            objstate.Add(new State1 { Id = 149, StateName = "MOLDOVA, REPUBLIC OF" });
            objstate.Add(new State1 { Id = 150, StateName = "MONACO" });
            objstate.Add(new State1 { Id = 151, StateName = "MONGOLIA" });
            objstate.Add(new State1 { Id = 152, StateName = "MONTENEGRO" });
            objstate.Add(new State1 { Id = 153, StateName = "MONTSERRAT" });
            objstate.Add(new State1 { Id = 154, StateName = "MOROCCO" });
            objstate.Add(new State1 { Id = 155, StateName = "MOZAMBIQUE" });
            objstate.Add(new State1 { Id = 156, StateName = "MYANMAR" });
            objstate.Add(new State1 { Id = 157, StateName = "NAMIBIA" });
            objstate.Add(new State1 { Id = 158, StateName = "NAURU" });
            objstate.Add(new State1 { Id = 159, StateName = "NEPAL" });
            objstate.Add(new State1 { Id = 160, StateName = "NETHERLANDS" });
            objstate.Add(new State1 { Id = 161, StateName = "NEW CALEDONIA" });
            objstate.Add(new State1 { Id = 162, StateName = "NEW ZEALAND" });
            objstate.Add(new State1 { Id = 163, StateName = "NICARAGUA" });
            objstate.Add(new State1 { Id = 164, StateName = "NIGER" });
            objstate.Add(new State1 { Id = 165, StateName = "NIGERIA" });
            objstate.Add(new State1 { Id = 166, StateName = "NIUE" });
            objstate.Add(new State1 { Id = 167, StateName = "NORFOLK ISLAND" });
            objstate.Add(new State1 { Id = 168, StateName = "NORTHERN MARIANA ISLANDS" });
            objstate.Add(new State1 { Id = 169, StateName = "NORWAY" });
            objstate.Add(new State1 { Id = 170, StateName = "OMAN" });
            objstate.Add(new State1 { Id = 171, StateName = "PAKISTAN" });
            objstate.Add(new State1 { Id = 172, StateName = "PALAU" });
            objstate.Add(new State1 { Id = 173, StateName = "PALESTINE" });
            objstate.Add(new State1 { Id = 174, StateName = "PANAMA" });
            objstate.Add(new State1 { Id = 175, StateName = "PAPUA NEW GUINEA" });
            objstate.Add(new State1 { Id = 176, StateName = "PARAGUAY" });
            objstate.Add(new State1 { Id = 177, StateName = "PERU" });
            objstate.Add(new State1 { Id = 178, StateName = "PHILIPPINES" });
            objstate.Add(new State1 { Id = 179, StateName = "PITCAIRN" });
            objstate.Add(new State1 { Id = 180, StateName = "POLAND" });
            objstate.Add(new State1 { Id = 181, StateName = "PORTUGAL" });
            objstate.Add(new State1 { Id = 182, StateName = "PUERTO RICO" });
            objstate.Add(new State1 { Id = 183, StateName = "QATAR" });
            objstate.Add(new State1 { Id = 184, StateName = "REUNION" });
            objstate.Add(new State1 { Id = 185, StateName = "ROMANIA" });
            objstate.Add(new State1 { Id = 186, StateName = "RUSSIAN FEDERATION" });
            objstate.Add(new State1 { Id = 187, StateName = "RWANDA" });
            objstate.Add(new State1 { Id = 188, StateName = "SAINT BARTHELEMY" });
            objstate.Add(new State1 { Id = 189, StateName = "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA" });
            objstate.Add(new State1 { Id = 190, StateName = "SAINT KITTS AND NEVIS" });
            objstate.Add(new State1 { Id = 191, StateName = "SAINT LUCIA" });
            objstate.Add(new State1 { Id = 192, StateName = "SAINT MARTIN (FRENCH PART)" });
            objstate.Add(new State1 { Id = 193, StateName = "SAINT PIERRE AND MIQUELON" });
            objstate.Add(new State1 { Id = 194, StateName = "SAINT VINCENT AND THE GRENADINES" });
            objstate.Add(new State1 { Id = 195, StateName = "SAMOA" });
            objstate.Add(new State1 { Id = 196, StateName = "SAN MARINO" });
            objstate.Add(new State1 { Id = 197, StateName = "SAO TOME AND PRINCIPE" });
            objstate.Add(new State1 { Id = 198, StateName = "SAUDI ARABIA" });
            objstate.Add(new State1 { Id = 199, StateName = "SENEGAL" });
            objstate.Add(new State1 { Id = 200, StateName = "SERBIA" });
            objstate.Add(new State1 { Id = 201, StateName = "SEYCHELLES" });
            objstate.Add(new State1 { Id = 202, StateName = "SIERRA LEONE" });
            objstate.Add(new State1 { Id = 203, StateName = "SINGAPORE" });
            objstate.Add(new State1 { Id = 204, StateName = "SINT MAARTEN (DUTCH PART)" });
            objstate.Add(new State1 { Id = 205, StateName = "SLOVAKIA" });
            objstate.Add(new State1 { Id = 206, StateName = "SOLOMON ISLANDS" });
            objstate.Add(new State1 { Id = 207, StateName = "SOMALIA" });
            objstate.Add(new State1 { Id = 208, StateName = "SOUTH AFRICA" });
            objstate.Add(new State1 { Id = 209, StateName = "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS" });
            objstate.Add(new State1 { Id = 210, StateName = "SOUTH SUDAN" });
            objstate.Add(new State1 { Id = 211, StateName = "SPAIN" });
            objstate.Add(new State1 { Id = 212, StateName = "SRI LANKA" });
            objstate.Add(new State1 { Id = 213, StateName = "SUDAN" });
            objstate.Add(new State1 { Id = 214, StateName = "SURINAME" });
            objstate.Add(new State1 { Id = 215, StateName = "SVALBARD AND JAN MAYEN" });
            objstate.Add(new State1 { Id = 216, StateName = "SWAZILAND" });
            objstate.Add(new State1 { Id = 217, StateName = "SWEDEN" });
            objstate.Add(new State1 { Id = 218, StateName = "SWITZERLAND" });
            objstate.Add(new State1 { Id = 219, StateName = "SYRIAN ARAB REPUBLIC" });
            objstate.Add(new State1 { Id = 220, StateName = "TAIWAN, PROVINCE OF CHINA" });
            objstate.Add(new State1 { Id = 221, StateName = "TAJIKISTAN" });
            objstate.Add(new State1 { Id = 222, StateName = "TANZANIA" });
            objstate.Add(new State1 { Id = 223, StateName = "THAILAND" });
            objstate.Add(new State1 { Id = 224, StateName = "TIMOR-LESTE" });
            objstate.Add(new State1 { Id = 225, StateName = "TOGO" });
            objstate.Add(new State1 { Id = 226, StateName = "TOKELAU" });
            objstate.Add(new State1 { Id = 227, StateName = "TONGA" });
            objstate.Add(new State1 { Id = 228, StateName = "TRINIDAD AND TOBAGO" });
            objstate.Add(new State1 { Id = 229, StateName = "TUNISIA" });
            objstate.Add(new State1 { Id = 230, StateName = "TURKEY" });
            objstate.Add(new State1 { Id = 231, StateName = "TURKMENISTAN" });
            objstate.Add(new State1 { Id = 232, StateName = "TURKS AND CAICOS ISLANDS" });
            objstate.Add(new State1 { Id = 233, StateName = "TUVALU" });
            objstate.Add(new State1 { Id = 234, StateName = "UGANDA" });
            objstate.Add(new State1 { Id = 235, StateName = "UKRAINE" });
            objstate.Add(new State1 { Id = 236, StateName = "UNITED ARAB EMIRATES" });
            objstate.Add(new State1 { Id = 237, StateName = "UNITED KINGDOM" });
            objstate.Add(new State1 { Id = 238, StateName = "UNITED STATES" });
            objstate.Add(new State1 { Id = 239, StateName = "UNITED STATES MINOR OUTLYING ISLANDS" });
            objstate.Add(new State1 { Id = 240, StateName = "URUGUAY" });
            objstate.Add(new State1 { Id = 241, StateName = "UZBEKISTAN" });
            objstate.Add(new State1 { Id = 242, StateName = "VANUATU" });
            objstate.Add(new State1 { Id = 243, StateName = "VENEZUELA" });
            objstate.Add(new State1 { Id = 244, StateName = "VIET NAM" });
            objstate.Add(new State1 { Id = 245, StateName = "VIRGIN ISLANDS" });
            objstate.Add(new State1 { Id = 246, StateName = "WALLIS AND FUTUNA" });
            objstate.Add(new State1 { Id = 247, StateName = "WESTERN SAHARA" });
            objstate.Add(new State1 { Id = 248, StateName = "YEMEN" });
            objstate.Add(new State1 { Id = 249, StateName = "ZAMBIA" });
            objstate.Add(new State1 { Id = 250, StateName = "ZIMBABWE" });

            return objstate;
        }
    }
}
