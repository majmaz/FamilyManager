using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using MBox;
using PerCalendar;
using System.Globalization;
using System.Drawing;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Controls.DataVisualization.Charting;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Net;
using System.Management;

namespace FamilyManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        DateTime DDate = new DateTime();
        TakhsisVamTbl _TakhsisVamTbl = new TakhsisVamTbl();
        PardakhtVamTbl _PardakhtVamTbl = new PardakhtVamTbl();
        TakhsisDaramadKhoshe _TakhsisDaramadKhoshe = new TakhsisDaramadKhoshe();
        OnvanVamTbl _OnvanVamTbl = new OnvanVamTbl();
        OnvanVamNafarTbl _OnvanVamNafarTbl = new OnvanVamNafarTbl();
        SabteHazinehSakhtemanTbl _SabteHazinehSakhtemanTbl = new SabteHazinehSakhtemanTbl();
        HamsaieTbl _HamsaieTbl = new HamsaieTbl();
        MavadGhzaNameTbl _MavadGhzaNameTbl = new MavadGhzaNameTbl();
        FamilyManaerDBEntities _FamilyManaerDBEntities = new FamilyManaerDBEntities();
        IadAvarTbl _IadAvarTbl = new IadAvarTbl();
        VAMTbl _VAMTbl = new VAMTbl();
        FinancialTbl _FinancialTbl = new FinancialTbl();
        ComboBoxTbl _ComboBoxTbl = new ComboBoxTbl();
        TreeKala _TreeKala = new TreeKala();
        GhzaNameTbl _GhzaNameTbl = new GhzaNameTbl();
        DepositTbl _DepositTbl = new DepositTbl();
        TanzimZamanIadAvar _TanzimZamanIadAvar = new TanzimZamanIadAvar();
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        MojodiKalaTbl _MojodiKalaTbl = new MojodiKalaTbl();
        public void SaveError(Exception error) //دخیره ارور
        {
            System.Windows.Forms.MessageBox.Show(error.ToString());
        }
        public void EmptyPar()//خالی کردن پارامترها
        {
            Par.FoodName = dlg.FileName = Par.Year = Par.ImagePath = Par.string3 = Par.string2 = Par.string1 = Par.ActveRightPanel = Par.TreeKala1 = Par.TreeKala2 = Par.TreeKala3 = string.Empty; Par.IDIadAvar = Par.ID =  -1; Par.Tarikh = null;
            Par._DateTimeVariable = null;
            Par.TreeExpand = false;
             Par.Nafarat = Par.MojodiMavadGhazaiy = 0;
        }
        private string GetHardwarSerial()
        {
          //  try
        //    {
                string cpuSerial = string.Empty;
            string hardSerial = string.Empty;
            string mainBoardSerial = string.Empty;
            //----------------------

            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection objcol = mgmt.GetInstances();
            foreach (ManagementObject obj in objcol)
            {
                if (obj.Properties["ProcessorId"] != null)
                    cpuSerial = obj.Properties["ProcessorId"].Value.ToString().Trim();
            }

            //---------------------

            ManagementObjectSearcher sercher = new ManagementObjectSearcher("select * from Win32_PhysicalMedia");
            foreach (ManagementObject wmi_Hd in sercher.Get())
            {
                if (wmi_Hd["SerialNumber"] != null)
                    hardSerial = wmi_Hd["SerialNumber"].ToString().Trim();
            }

            //---------------------

            ManagementObjectSearcher sercher2 = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject wmi_Board in sercher2.Get())
            {
                if (wmi_Board["SerialNumber"] != null)
                    mainBoardSerial = wmi_Board["SerialNumber"].ToString().Trim();
            }

            //-----------

            string systemId = cpuSerial + hardSerial + mainBoardSerial;
                // char[] arr = systemId.ToCharArray();
                // Array.Reverse(arr);
                //  return new string("maj"+arr);
                return ("maj" + systemId);

       //     }
       //     catch (Exception error)
       //     {
       //         SaveError(error);
       //     }
        }


        public void selectDaramadGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(SabteDaramadTextBox1.Text))
                {
                    var Fin1 = from p in _FamilyManaerDBEntities.OnvanDaramadTbls
                               orderby p.ID descending
                               select p;
                    if (Fin1 != null)
                    {
                        IncomeGrid2.ItemsSource = Fin1.Select(s => new
                        {

                            عنوان = s.Onvan

                        }).ToList();
                    }
                }
                else
                {
                    var Fin = from p in _FamilyManaerDBEntities.OnvanDaramadTbls
                              where p.Onvan.Contains(SabteDaramadTextBox1.Text)
                              orderby p.ID descending
                              select p;
                    if (Fin != null)
                    {
                        IncomeGrid2.ItemsSource = Fin.Select(s => new
                        {

                            عنوان = s.Onvan

                        }).ToList();
                    }
                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        public void depositGrid()
        {

            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                gridSabteSeporde.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.Deposit
                }).ToList();
                gridSabteSeporde.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateOnvanDaramadGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(SabteOnvanDaramadPTextBox1.Text))
                {
                    var Fin1 = from p in _FamilyManaerDBEntities.OnvanDaramadTbls
                               orderby p.ID descending
                               select p;
                    if (Fin1 != null)
                    {
                        gridSabteOnvanDaramadP.ItemsSource = Fin1.Select(s => new
                        {

                            عنوان = s.Onvan

                        }).ToList();
                    }
                }
                else
                {
                    var Fin = from p in _FamilyManaerDBEntities.OnvanDaramadTbls
                              where p.Onvan.Contains(SabteOnvanDaramadPTextBox1.Text)
                              orderby p.ID descending
                              select p;
                    if (Fin != null)
                    {
                        gridSabteOnvanDaramadP.ItemsSource = Fin.Select(s => new
                        {

                            عنوان = s.Onvan

                        }).ToList();
                    }
                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }

        }
        public void createSabtecheckGrid()
        {

            var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                      where p.FinancialCategory == "check"
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                ModiriatChckGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.Title,
                    تاریخ = s.PersianDate,
                    مبلغ = s.Cost,
                    سپرده = s.Deposite,
                    گیرنده = s.girandeh,
                    پاس = s.Pas,
                    توضیحات = s.Description

                }).ToList();
                ModiriatChckGrid.Columns[0].Visibility = Visibility.Hidden;

            }
        }
        public void CreateGheimatGhaza(string nameghaza) // 
        {

            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameGhaza == nameghaza
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                EntekhabGheimatGhazaGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameMavad,
                    مقدار = s.Meghdar,
                    واحد = s.Vahed
                }).ToList();
                EntekhabGheimatGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
            }

        }
        public void CreateMavad2(string nameghaza) // 
        {

            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameGhaza == nameghaza
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                ntekhabCalleryGhazaGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameMavad,
                    مقدار = s.Meghdar,
                    واحد = s.Vahed
                }).ToList();
                ntekhabCalleryGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
            }

        }
        public void CreateMavad() // 
        {
            if (EntekhabNameGhazaTextBox1.Text != "")
            {
                var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                          where p.NameGhaza.Contains(EntekhabNameGhazaTextBox1.Text)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    EntekhabNameGhazaGrid2.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.NameMavad,
                        مقدار = s.Meghdar,
                        واحد = s.Vahed
                    }).ToList();
                    EntekhabNameGhazaGrid2.Columns[0].Visibility = Visibility.Hidden;
                }
            }
        }
        public void PardakhtVamGrid()
        {

            if ((PardakhtVamCombo1.Text != "") && (PardakhtVamCombo2.Text != ""))
            {
                var Fin = from p in _FamilyManaerDBEntities.PardakhtVamTbls
                          where p.OnvanVam == PardakhtVamCombo1.Text && p.NameVamGirandeh == PardakhtVamCombo2.Text
                          orderby p.Tarikh descending
                          select p;
                if (Fin != null)
                {
                    gridPardakhtVam.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.OnvanVam,
                        نفر = s.NameVamGirandeh,
                        پرداختی = s.MablaghPardakhti,
                        تاریخ = s.Tarikh,
                        توضیحات = s.Tozihat
                    }).ToList();
                    gridPardakhtVam.Columns[0].Visibility = Visibility.Hidden;
                }
            }
        }
        public void CreateVamNobatGrid()
        {
            var Fin = from p in _FamilyManaerDBEntities.TakhsisVamTbls
                      where p.Onvan == NobatVamGirandehCombo1.Text
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                gridNobatVamGirandeh.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.Onvan,
                    نفر = s.NameVamGirandeh,
                    سال = s.NobatVam_Sal,
                    ماه = s.NobatVam_Mah,
                    پرداخت = s.Tarikh,
                    توضیحات = s.Tozihat
                }).ToList();
                gridNobatVamGirandeh.Columns[0].Visibility = Visibility.Hidden;
            }

        }
        public void CreateHazinehVahedGrid() /// ساخت جدول هزینه واحد
        {

            if (SbteKharjkardVahedTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                          where p.IDVahed != null && p.Income == null
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SbteKharjkardVahedGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        واحد = s.VahedName,
                        عنوان = s.TitleCost,
                        تاریخ = s.PersianDate,
                        مبلغ = s.Cost,
                        ماه = s.mmonth,
                        توضیحات = s.Description
                    }).ToList();
                    SbteKharjkardVahedGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                          where ((p.TitleCost.Contains(SbteKharjkardVahedTextBox1.Text)) && (p.VahedName != null) && p.Income == null)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SbteKharjkardVahedGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        واحد = s.VahedName,
                        عنوان = s.TitleCost,
                        تاریخ = s.PersianDate,
                        مبلغ = s.Cost,
                        ماه = s.mmonth,
                        توضیحات = s.Description
                    }).ToList();
                    SbteKharjkardVahedGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
        }
        public void CreateGhaza3() // جدول غذاها
        {
            if (EntekhabNameGhazaTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    EntekhabNameGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    EntekhabNameGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          where p.Name.Contains(EntekhabNameGhazaTextBox1.Text)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    EntekhabNameGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    EntekhabNameGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }

        }
        public void EntekhabMojodiGhaza()
        {
            int Mojodi = 0;
            EntekhabMojodiGhazaGrid1.Columns.Clear();
            EntekhabMojodiGhazaGrid1.Items.Clear();
            EntekhabMojodiGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("ID") });
            EntekhabMojodiGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "نام", Binding = new System.Windows.Data.Binding("نام") });
            EntekhabMojodiGhazaGrid1.Columns[0].Visibility = Visibility.Hidden;
            var FFin1 = from p in _FamilyManaerDBEntities.GhzaNameTbls
                        orderby p.ID
                        select p;
            if (FFin1 != null)
            {
                foreach (var F1 in FFin1)
                {
                    EntekhabMojodiGhazaGrid1.Items.Add(new { ID = F1.ID, نام = F1.Name });
                }
            }
            var Fin1 = from p in _FamilyManaerDBEntities.GhzaNameTbls
                       orderby p.ID
                       select p;
            if (Fin1 != null)
            {
                foreach (var F1 in Fin1)
                {

                    var Fin2 = from pp in _FamilyManaerDBEntities.MavadGhzaNameTbls
                               where pp.NameGhaza == F1.Name
                               orderby pp.ID
                               select pp;
                    if (Fin2 != null)
                    {
                        foreach (var F2 in Fin2)
                        {
                            var Fin3 = from ppp in _FamilyManaerDBEntities.FinancialTbls
                                       where ppp.Title == F2.NameMavad
                                       orderby ppp.ID descending
                                       select ppp;
                            if (Fin3 != null)
                            {
                                foreach (var F3 in Fin3)
                                {

                                    Mojodi = Mojodi + int.Parse(F3.Meghdar.Value.ToString());

                                }
                                if (Mojodi <= 0)
                                {
                                    EntekhabMojodiGhazaGrid1.Items.Remove(new { ID = F1.ID, نام = F1.Name });
                                }
                                Mojodi = 0;

                            }

                        }


                    }

                }
            }
        }

        public void CreateGhaza2() // جدول غذاها
        {
            if (SabteMavadGhazaTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SabteMavadGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    SabteMavadGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          where p.Name.Contains(SabteMavadGhazaTextBox1.Text)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SabteMavadGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    SabteMavadGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }

        }
        public void EntekhabMavadGhaza()
        {
            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameMavad == (EntekhabMavadGhazaTextBox1.Text)
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                EntekhabMavadGhazaGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameGhaza
                }).ToList();
                EntekhabMavadGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateGhaza() // جدول غذاها
        {
            if (SabTeNameGhazaTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SabTeNameGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    SabTeNameGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.GhzaNameTbls
                          where p.Name.Contains(SabTeNameGhazaTextBox1.Text)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SabTeNameGhazaGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Name
                    }).ToList();
                    SabTeNameGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }

        }
        public void CreateMavadGhaza()
        {

            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameGhaza.Contains(SabteMavadGhazaTextBox1.Text)
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                SabteMavadGhazaGrid1.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameMavad,
                    مقدار = s.Meghdar
                }).ToList();
                SabteMavadGhazaGrid1.Columns[0].Visibility = Visibility.Hidden;
            }

        }
        public void createSabteVamPanelGrid()
        {

            var Fin = from p in _FamilyManaerDBEntities.VAMTbls
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                SabteVamPanelGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.Title,
                    تاریخ = s.PersianDate,
                    مبلغ = s.ssum,
                    تعداد = s.NumberGhest,
                    توضیحات = s.description
                }).ToList();
                SabteVamPanelGrid.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateSabteKharjkardKamel()
        {
            if (SabetKharjKardKamelTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          where ((p.FinancialCategory == "CompleteCost") && (p.Datee >= Par._DateTimeVariable.Value))
                          orderby p.EnterDate descending
                          select p;
                if (Fin != null)
                {
                    SabetKharjKardKamelGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Title,
                        تاریخ = s.PersianDate,
                        هزینه = s.Cost,
                        سپرده = s.Deposite,
                        توضیحات = s.Description
                    }).ToList();
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          where ((p.FinancialCategory == "CompleteCost") && (p.Datee >= Par._DateTimeVariable.Value) && (p.Title.Contains(SabetKharjKardKamelTextBox1.Text)))
                          orderby p.EnterDate descending
                          select p;
                if (Fin != null)
                {
                    SabetKharjKardKamelGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Title,
                        تاریخ = s.PersianDate,
                        هزینه = s.Cost,
                        سپرده = s.Deposite,
                        توضیحات = s.Description
                    }).ToList();
                }
            }

            SabetKharjKardKamelGrid.Columns[0].Visibility = Visibility.Hidden;

        }// ساخت جدول خرجکرد کامل

        public void EntekhabMavadGhazaGrid2()
        {
            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameMavad == EntekhabMavadGhazaTextBox1.Text
                      orderby p.NameGhaza descending
                      select p;
            if (Fin != null)
            {
                EntekhabMavadGhazaGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameGhaza

                }).ToList();
                EntekhabMavadGhazaGrid.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateSabteKharjkardSade() //ساخت جدول خرجکرد ساده
        {
            if (SabetKharjKardsadeTextBox1.Text == "")
            {
                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          where ((p.FinancialCategory == "SimpleCost") && (p.Datee >= Par._DateTimeVariable.Value))
                          orderby p.EnterDate descending
                          select p;
                if (Fin != null)
                {
                    SabetKharjKardsadeGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Title,
                        تاریخ = s.PersianDate,
                        هزینه = s.Cost,
                        سپرده = s.Deposite,
                        توضیحات = s.Description
                    }).ToList();
                }
            }
            else
            {
                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          where ((p.FinancialCategory == "SimpleCost") && (p.Datee >= Par._DateTimeVariable.Value) && (p.Title.Contains(SabetKharjKardsadeTextBox1.Text)))
                          orderby p.EnterDate descending
                          select p;
                if (Fin != null)
                {
                    SabetKharjKardsadeGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Title,
                        تاریخ = s.PersianDate,
                        هزینه = s.Cost,
                        سپرده = s.Deposite,
                        توضیحات = s.Description
                    }).ToList();
                }
            }

            //SabetKharjKardsadeGrid.Columns[0].Visibility = Visibility.Hidden;

        }
        public void CreateGhimatGhazaGrid()
        {
            int Gheimat = 0;
            EntekhabGheimatGhazaGrid1.Columns.Clear();
            EntekhabGheimatGhazaGrid1.Items.Clear();
            EntekhabGheimatGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("ID") });
            EntekhabGheimatGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "نام", Binding = new System.Windows.Data.Binding("نام") });
            EntekhabGheimatGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "قیمت", Binding = new System.Windows.Data.Binding("قیمت") });
            EntekhabGheimatGhazaGrid1.Columns[0].Visibility = Visibility.Hidden;

            var Fin1 = from p in _FamilyManaerDBEntities.GhzaNameTbls
                       orderby p.ID
                       select p;
            if (Fin1 != null)
            {
                foreach (var F1 in Fin1)
                {

                    var Fin2 = from pp in _FamilyManaerDBEntities.MavadGhzaNameTbls
                               where pp.NameGhaza == F1.Name
                               orderby pp.ID
                               select pp;
                    if (Fin2 != null)
                    {
                        foreach (var F2 in Fin2)
                        {
                            var Fin3 = from ppp in _FamilyManaerDBEntities.FinancialTbls
                                       where ppp.Title == F2.NameMavad
                                       orderby ppp.ID descending
                                       select ppp;
                            if (Fin3 != null)
                            {
                                string x = "";
                                foreach (var F3 in Fin3)
                                {
                                    if (x != F2.NameMavad)
                                    {
                                        Gheimat = Gheimat + ((int.Parse(F3.Cost.Value.ToString()) / int.Parse(F3.Meghdar.Value.ToString()) * F2.Meghdar.Value));
                                        x = F2.NameMavad;

                                    }
                                }
                            }

                        }
                        int.TryParse(EntekhabGheimatGhazaTextBox1.Text, out int outnumber);
                        if (Gheimat <= outnumber)
                        {
                            EntekhabGheimatGhazaGrid1.Items.Add(new { ID = F1.ID, نام = F1.Name, قیمت = Gheimat });
                        }
                        Gheimat = 0;
                    }

                }
            }
        }
        public void CreateHamsaieGrid() // ساخت جدول همسایه ها
        {
            SabteHamsaieGrid.Items.Clear();
            var Fin = from p in _FamilyManaerDBEntities.HamsaieTbls
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                foreach (var item in Fin)
                {
                    SabteHamsaieGrid.Items.Add(new { A1 = item.ID, A2 = item.NameVahed, A3 = item.TedadNafarat, A4 = item.Metrazh, A5 = item.startPersianDate, A6 = item.FinishPersianDate, A7 = item.Tasfieh, A8 = item.Description });
                }
            }

        }
        public void CreateVamNafarGrid() // گرید نام وام گیرندگان
        {
            gridNameVamGirandeh.Items.Clear();
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                      where p.VamTitle == NameVamGirandehCombo1.Text
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                foreach (var item in Fin)
                {
                    gridNameVamGirandeh.Items.Add(new { ID = item.ID, A1 = item.Nafar, A2 = item.Mobile });
                }
            }
        }
        public void CreateOnvanVamGrid() //عنوان وام گرید
        {
            gridOnvanVam.Items.Clear();
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamTbls
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                foreach (var item in Fin)
                {
                    gridOnvanVam.Items.Add(new { ID = item.ID, A1 = item.Title, A2 = item.StartPersianDate, A3 = item.TedadAghsat, A4 = item.MablaghVam.Value.ToString("N0"), A5 = item.MablaghGhest.Value.ToString("N0"), A6 = item.FAAL, A7 = item.Description });

                }
            }
        }
        public void CreateHazinehSakhtemanGrid() // ساخت جدول هزینه ساختمان
        {
            var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                      where p.VahedName == null
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                SabetKharjKardsadeSakhtemanGrid.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.TitleCost,
                    تاریخ = s.PersianDate,
                    مبلغ = s.Cost,
                    ماه = s.mmonth,
                    شیوه = s.ShiveTaghsim,
                    شروع = s.startPersianDate,
                    پایان = s.FinishPersianDate,
                    توضیحات = s.Description
                }).ToList();
                SabetKharjKardsadeSakhtemanGrid.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateMojodiGhazaGrid(string nameghaza)
        {
            var Fin = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                      where p.NameGhaza == nameghaza
                      orderby p.ID descending
                      select p;
            if (Fin != null)
            {
                EntekhabMojodiGhazaGrid2.ItemsSource = Fin.Select(s => new
                {
                    ID = s.ID,
                    عنوان = s.NameMavad,
                    مقدار = s.Meghdar
                }).ToList();
                EntekhabMojodiGhazaGrid2.Columns[0].Visibility = Visibility.Hidden;
            }
        }
        public void CreateGhazaCallery()
        {
            decimal callery = 0;
            ntekhabCalleryGhazaGrid1.Columns.Clear();
            ntekhabCalleryGhazaGrid1.Items.Clear();
            ntekhabCalleryGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("ID") });
            ntekhabCalleryGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "نام", Binding = new System.Windows.Data.Binding("نام") });
            ntekhabCalleryGhazaGrid1.Columns.Add(new DataGridTextColumn { Header = "کالری", Binding = new System.Windows.Data.Binding("کالری") });
            ntekhabCalleryGhazaGrid1.Columns[0].Visibility = Visibility.Hidden;

            var Fin1 = from p in _FamilyManaerDBEntities.GhzaNameTbls
                       orderby p.ID
                       select p;
            if (Fin1 != null)
            {
                foreach (var F1 in Fin1)
                {

                    var Fin2 = from pp in _FamilyManaerDBEntities.MavadGhzaNameTbls
                               where pp.NameGhaza == F1.Name
                               orderby pp.ID
                               select pp;
                    if (Fin2 != null)
                    {
                        foreach (var F2 in Fin2)
                        {
                            var Fin3 = from ppp in _FamilyManaerDBEntities.TreeKalas
                                       where ((ppp.Header == F2.NameMavad) || (ppp.SubHeader == F2.NameMavad) || (ppp.SubSubHeader == F2.NameMavad))
                                       orderby ppp.ID
                                       select ppp;
                            if (Fin3 != null)
                            {
                                foreach (var F3 in Fin3)
                                {
                                    callery = callery + (F3.IekCallery.Value / 100 * F2.Meghdar.Value);
                                }
                            }

                        }
                        int.TryParse(ntekhabCalleryGhazaTextBox1.Text, out int outnumber);
                        if (callery <= outnumber)
                        {
                            ntekhabCalleryGhazaGrid1.Items.Add(new { ID = F1.ID, نام = F1.Name, کالری = callery });
                        }
                        callery = 0;
                    }

                }
            }
        }
        public void CreatMojodiGrid()
        {
            var Fin = from p in _FamilyManaerDBEntities.MojodiKalaTbls
                      select p;
            if (MojodiTextBox1.Text != "")
            {
                Fin = from p in _FamilyManaerDBEntities.MojodiKalaTbls
                      where p.Onvan.Contains(MojodiTextBox1.Text)
                      select p;
            }

            if (Fin != null)
            {
                MojodiGrid.ItemsSource = Fin.Select(d => new
                {
                    عنوان = d.Onvan,
                    مقدار = d.Meghdar
                }).ToList();
            }


        }
        public void CreateSabteKalaTreeView() // نمودار درختی کالا
        {
            string x = "";
            if (MojodiPanel.Visibility == Visibility.Visible)
            {
                x = MojodiTextBox1.Text;
            }
            else if (SabteMavadGhazaPanel.Visibility == Visibility.Visible)
            {
                x = SabteMavadGhazaTextBox2.Text;
            }
            else if (EntekhabMavadGhazaPanel.Visibility == Visibility.Visible)
            {
                x = EntekhabMavadGhazaTextBox1.Text;
            }
            else
            {
                x = SabetKharjKardKamelTextBox1.Text;

            }
            EntekhabMavadGhazaTree.Items.Clear();
            SabteKalaTreeView.Items.Clear();
            MojodiTreeView.Items.Clear();
            SabetKharjKardKamelTreeView.Items.Clear();
            SabteMavadGhazaTree.Items.Clear();


            var tree1 = from p in _FamilyManaerDBEntities.TreeKalas
                        group p by p.Header into g
                        orderby g.Key
                        select g;


            if (x != "")
            {
                tree1 = from p in _FamilyManaerDBEntities.TreeKalas
                        where p.Header.Contains(x) || p.SubHeader.Contains(x) || p.SubSubHeader.Contains(x)
                        group p by p.Header into g
                        orderby g.Key
                        select g;
            }


            if (tree1 != null)
            {
                foreach (var T1 in tree1)
                {
                    TreeViewItem _treeItem1 = new TreeViewItem();
                    _treeItem1.Header = T1.Key;
                    if (Par.TreeExpand)
                    {
                        _treeItem1.IsExpanded = true;
                    }

                    var tree2 = from pp in _FamilyManaerDBEntities.TreeKalas
                                where pp.Header == T1.Key
                                group pp by pp.SubHeader into gg

                                orderby gg.Key
                                select gg;


                    if (tree2 != null)
                    {
                        foreach (var T2 in tree2)
                        {

                            TreeViewItem _treeItem2 = new TreeViewItem();
                            if (T2.Key != "")
                            {
                                _treeItem2.Header = T2.Key;
                                _treeItem1.Items.Add(_treeItem2);
                                if (Par.TreeExpand)
                                {
                                    _treeItem1.IsExpanded = true;
                                    _treeItem2.IsExpanded = true;
                                }
                            }
                            var tree3 = from ppp in _FamilyManaerDBEntities.TreeKalas
                                        where ((ppp.Header == T1.Key) && (ppp.SubHeader == T2.Key))
                                        orderby ppp.SubSubHeader
                                        select ppp;
                            if (tree3 != null)
                            {
                                foreach (var T3 in tree3)
                                {
                                    TreeViewItem _treeItem3 = new TreeViewItem();
                                    if (T3.SubSubHeader != "")
                                    {
                                        _treeItem3.Header = T3.SubSubHeader;
                                        _treeItem2.Items.Add(_treeItem3);
                                        if (Par.TreeExpand)
                                        {
                                            _treeItem1.IsExpanded = true;
                                            _treeItem2.IsExpanded = true;
                                            _treeItem3.IsExpanded = true;
                                        }


                                    }
                                }
                            }
                        }
                    }
                    if (SabetKharjKardKamelPanel.Visibility == Visibility.Visible)
                    {
                        SabetKharjKardKamelTreeView.Items.Add(_treeItem1);

                    }
                    else if (EntekhabMavadGhazaPanel.Visibility == Visibility.Visible)
                    {
                        EntekhabMavadGhazaTree.Items.Add(_treeItem1);
                    }
                    else if (MojodiPanel.Visibility == Visibility.Visible)
                    {
                        MojodiTreeView.Items.Add(_treeItem1);
                    }
                    else if (SabteMavadGhazaPanel.Visibility == Visibility.Visible)
                    {
                        SabteMavadGhazaTree.Items.Add(_treeItem1);
                    }
                    else
                    {
                        SabteKalaTreeView.Items.Add(_treeItem1);

                    }

                }

            }
        }

        public void SpecifyTime(string canvasname, DateTime CanvasDateTime)//مشخص کردن تاریخ شروع کشیدن جدول زمانی
        {

            int colorconter = 0, RePsubtractwidth = 0, CanvasHeight = 0;
            double CanvasSetTopHeight = 0, ZaribEslahCanvasHeight = 0;
            if (SabteIadAvarPanel.IsEnabled == true)
            {
                ZaribEslahCanvasHeight = CanvasTimeTable1.ActualHeight /5500;
                CanvasHeight = Convert.ToInt32(CanvasTimeTable1.ActualHeight) / 8 + Convert.ToInt32(CanvasTimeTable1.ActualHeight) / 800;
            }
            else if (GozaresfSabteIadAvarPanel.IsEnabled == true)
            {
                ZaribEslahCanvasHeight = GozareshSabteIadAvarCanvasTimeTable1.ActualHeight / 2000;
                CanvasHeight = Convert.ToInt32(GozareshSabteIadAvarCanvasTimeTable1.ActualHeight) / 4 + Convert.ToInt32(GozareshSabteIadAvarCanvasTimeTable1.ActualHeight) / 1600;
            }


            var starttime = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 0, 0, 0);
            var finishtime = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 23, 59, 59);
            var iad = from p in _FamilyManaerDBEntities.IadAvarTbls
                      where (p.Periodic == false && p.StartDateTime >= starttime && p.StartDateTime <= finishtime) || (p.Periodic == false && p.EndDateTime >= starttime && p.EndDateTime <= finishtime) || (p.Periodic == true && p.PeriodicEndTime >= starttime && p.PeriodicEndTime <= finishtime) || (p.Periodic == true && p.StartDateTime >= starttime && p.StartDateTime <= finishtime) || (p.Periodic == true && p.PeriodicEndTime >= finishtime && p.StartDateTime <= starttime)
                      orderby p.StartDateTime ascending
                      select p;


            if (iad != null)
            {

                foreach (var Time in iad)
                {

                    bool isrealtime = true;
                    int rep = 0;
                    colorconter++;
                    var Time0 = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 00, 00, 00);
                    var Time6 = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 06, 00, 00);
                    var Time12 = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 12, 0, 0);
                    var Time18 = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 18, 0, 0);
                    var Time24 = new DateTime(CanvasDateTime.Year, CanvasDateTime.Month, CanvasDateTime.Day, 23, 59, 59);
                    DateTime startposition = Time.StartDateTime.Value;
                    DateTime endposition = Time.EndDateTime.Value;
                    if (Time.Periodic == true) //خارج کردن دور های غیر مرتبط
                    {
                        isrealtime = false;

                        for (int i = 0; i <= Time.PeriodNumBer.Value; i++)
                        {
                            switch (Time.PeriodocKind)
                            {
                                case "روز":
                                    startposition = Time.StartDateTime.Value.AddDays(Time.MeasurePeriodic.Value * i);
                                    endposition = Time.EndDateTime.Value.AddDays(Time.MeasurePeriodic.Value * i);
                                    break;
                                case "ماه":
                                    startposition = Time.StartDateTime.Value.AddMonths(Time.MeasurePeriodic.Value * i);
                                    endposition = Time.EndDateTime.Value.AddMonths(Time.MeasurePeriodic.Value * i);
                                    break;
                                case "سال":
                                    startposition = Time.StartDateTime.Value.AddYears(Time.MeasurePeriodic.Value * i);
                                    endposition = Time.EndDateTime.Value.AddYears(Time.MeasurePeriodic.Value * i);
                                    break;
                            }

                            DateTime finishtimeAddDays = finishtime;
                            DateTime starttimeAddDays = starttime;
                            if ((new DateTime(startposition.Year, startposition.Month, startposition.Day) == new DateTime(starttime.Year, starttime.Month, starttime.Day))
                                || (new DateTime(endposition.Year, endposition.Month, endposition.Day) == new DateTime(finishtime.Year, finishtime.Month, finishtime.Day)))
                            {
                                isrealtime = true;
                                break;
                            }
                        }
                    }
                    if (isrealtime == false)
                    {
                        break;
                    }
                    int repp = 1;// این متغیر برای حل مشکل پریودهای تاریخی تعریف شده است
                    if ((Time.Periodic == true) && (startposition.Day != endposition.Day) && (CanvasDateTime.Day != Time.PeriodicEndTime.Value.Day))
                    {
                        repp = 2;
                    }

                    DateTime Fixstartposition = Time0, Fixendposition = Time0;
                    if (startposition < Time0) { startposition = Time0; }
                    if (endposition > Time24) { endposition = Time24; }

                    string StartTime = Time.StartDateTime.Value.Minute.ToString().PadLeft(2, '0') + " : " + Time.StartDateTime.Value.Hour.ToString().PadLeft(2, '0');
                    string EndTime = Time.EndDateTime.Value.Minute.ToString().PadLeft(2, '0') + " : " + Time.EndDateTime.Value.Hour.ToString().PadLeft(2, '0');
                    if (SabteIadAvarPanel.IsEnabled == true)
                    {
                        for (int i = 0; i < repp; i++)
                        {
                            while (rep == 0)
                            {
                                if (Time.MeasurePeriodic==2 && endposition == Time24 && Time.PeriodocKind=="روز") // زمانی که شرایط رو به رو رخ می دهد با توجه به اینکه شروع در یک روز و پایان فردا هست از فیلتر امروز رد می شود اما فیلتری وجود ندارد که فردا را رسم نکند
                                {
                                    Boolean braek = true;
                                    for (int iii = 0; iii <= Time.PeriodNumBer; iii+=2)
                                    {
                                        DateTime sss = Time.StartDateTime.Value.AddDays(iii);
                                        if (sss.ToShortDateString() == startposition.ToShortDateString())
                                        {
                                            braek = false;
                                            break;                                         
                                        }
                                    }
                                    if (braek)
                                    {
                                        return;
                                    }

                                }
                                if (startposition > Time18)
                                {
                                    rep = 1; CanvasSetTopHeight = (7 * CanvasHeight) + (10 * ZaribEslahCanvasHeight); RePsubtractwidth = 3;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition > Time24) { Fixendposition = Time24; }
                                }
                                else if (startposition > Time12)
                                {
                                    CanvasSetTopHeight = 5 * CanvasHeight + 6 * ZaribEslahCanvasHeight; RePsubtractwidth = 2;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition <= Time18) { rep = 1; }
                                    else if (endposition <= Time24) { Fixendposition = Time18; startposition = Time18.AddSeconds(1); }
                                }
                                else if (startposition > Time6)
                                {
                                    CanvasSetTopHeight = 3 * CanvasHeight + 2 * ZaribEslahCanvasHeight; RePsubtractwidth = 1;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition <= Time12) { rep = 1; }
                                    else { Fixendposition = Time12; startposition = Time12.AddSeconds(1); }
                                }
                                else if (startposition >= Time0)
                                {
                                    CanvasSetTopHeight = 1 * CanvasHeight + ZaribEslahCanvasHeight; RePsubtractwidth = 0;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition <= Time6) { rep = 1; }
                                    else { Fixendposition = Time6; startposition = Time6.AddSeconds(1); }
                                }
                                var xxx = function.CreatTimeTable(RePsubtractwidth, Convert.ToInt32(CanvasTimeTable1.ActualHeight), Convert.ToInt32(CanvasTimeTable1.ActualWidth), Fixstartposition, Fixendposition, "CanvasTimeTable1");
                                CreateTable(colorconter, CanvasSetTopHeight, xxx.Item1, xxx.Item2, xxx.Item3, Time.TitleActivity, Time.ID.ToString(), xxx.Item4, StartTime, EndTime, Time.TitleActivity, Time.description, Fixendposition, canvasname);
                                if (colorconter == 4) { colorconter = 0; }
                            }
                            startposition = new DateTime(starttime.Year, starttime.Month, starttime.Day, Time.StartDateTime.Value.Hour, Time.StartDateTime.Value.Minute, 0);
                            endposition = Time24;
                            rep = 0;
                        }
                    }
                    else if (GozaresfSabteIadAvarPanel.IsEnabled == true)
                    {
                        for (int i = 0; i < repp; i++)
                        {
                            while (rep == 0)
                            {
                                if (startposition > Time12)
                                {
                                    rep = 1; CanvasSetTopHeight = 3 * CanvasHeight + 3 * ZaribEslahCanvasHeight; RePsubtractwidth = 1;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition > Time24) { Fixendposition = Time24; }
                                }
                                else if (startposition >= Time0)
                                {
                                    CanvasSetTopHeight = 1 * CanvasHeight + ZaribEslahCanvasHeight; RePsubtractwidth = 0;
                                    Fixstartposition = startposition; Fixendposition = endposition;
                                    if (endposition <= Time12) { rep = 1; }
                                    else { Fixendposition = Time12; startposition = Time12.AddSeconds(1); }

                                }
                                var xxx = function.CreatTimeTable(RePsubtractwidth, Convert.ToInt32(GozareshSabteIadAvarCanvasTimeTable1.ActualHeight), Convert.ToInt32(GozareshSabteIadAvarCanvasTimeTable1.ActualWidth), Fixstartposition, Fixendposition, "GozareshSabteIadAvarCanvasTimeTable1");
                                CreateTable(colorconter, CanvasSetTopHeight, xxx.Item1, xxx.Item2, xxx.Item3, Time.TitleActivity, Time.ID.ToString(), xxx.Item4, StartTime, EndTime, Time.TitleActivity, Time.description, Fixendposition, canvasname);
                                if (colorconter == 4) { colorconter = 0; }
                            }
                            startposition = new DateTime(starttime.Year, starttime.Month, starttime.Day, Time.StartDateTime.Value.Hour, Time.StartDateTime.Value.Minute, 0);
                            endposition = Time24;
                            rep = 0;
                        }
                    }
                }
            }
        }
        //کشیدن مستطیل برای تایم تیبل
        public void CreateTable(int colorconter, double CanvasSetTopHeight, int HeightObj, int StartWidth1Obj, int EndWidth1Obj, string content, string name, DateTime endtime, string StartTime, string EndTime, string ActivityName, string description, DateTime Fixendposition, string canvasname)
        {

            TextBlock _Rectangle = new TextBlock();
            SolidColorBrush blueBrush = new SolidColorBrush();
            Viewbox _Viewbox = new Viewbox();
            if (endtime < DateTime.Now)
            { _Rectangle.TextDecorations = TextDecorations.Strikethrough; }
            _Viewbox.Stretch = Stretch.Fill;
            _Viewbox.StretchDirection = StretchDirection.Both;
            _Viewbox.MaxHeight = HeightObj;
            _Viewbox.MaxWidth = EndWidth1Obj - StartWidth1Obj;
            _Viewbox.Child = _Rectangle;
            switch (colorconter)
            {
                case 1:
                    blueBrush.Color = Colors.Purple;
                    _Rectangle.Background = blueBrush;
                    break;
                case 2:
                    blueBrush.Color = Colors.Magenta;
                    _Rectangle.Background = blueBrush;
                    break;
                case 3:
                    blueBrush.Color = Colors.Olive;
                    _Rectangle.Background = blueBrush;
                    break;
                case 4:
                    blueBrush.Color = Colors.Tomato;
                    _Rectangle.Background = blueBrush;
                    break;

            }

            _Rectangle.TextAlignment = TextAlignment.Center;
            _Rectangle.FontWeight = FontWeights.UltraBold;
            _Rectangle.LineHeight = Double.NaN;
            _Rectangle.Width = EndWidth1Obj - StartWidth1Obj;
            _Rectangle.ToolTip = ActivityName + Environment.NewLine + "ساعت شروع  " + StartTime + Environment.NewLine + "ساعت پایان  " + EndTime + Environment.NewLine + description;
            if ((Fixendposition.Hour == 23) && (Fixendposition.Minute == 59) && (Fixendposition.Second == 59))
            {
                _Rectangle.Name = "Z" + name; //نشان می دهد ادامه کار به فردا منتقل می شود
            }
            else
            {
                _Rectangle.Name = "A" + name;
            }

            _Rectangle.Text = content;
            _Rectangle.Cursor = System.Windows.Input.Cursors.Hand;
            _Rectangle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.task_Click);
            //////////////////////////////////////////////////////////////////////////////////////////////////
            if (SabteIadAvarPanel.IsEnabled == true)
            {
                Canvas.SetTop(_Viewbox, CanvasSetTopHeight + Convert.ToInt32(CanvasTimeTable1.ActualHeight) / 90);
                Canvas.SetRight(_Viewbox, StartWidth1Obj + Convert.ToInt32(CanvasTimeTable1.ActualWidth) / 1000);
            }
            else if (GozaresfSabteIadAvarPanel.IsEnabled == true)
            {
                Canvas.SetTop(_Viewbox, CanvasSetTopHeight);// + Convert.ToInt32(GozaresfSabteIadAvarPanel.ActualHeight) / 90);
                Canvas.SetRight(_Viewbox, StartWidth1Obj + Convert.ToInt32(GozaresfSabteIadAvarPanel.ActualWidth) / 10000);
            }



            switch (canvasname)
            {
                case "CanvasTimeTable1":
                    CanvasTimeTable1.Children.Add(_Viewbox);
                    break;
                case "GozareshSabteIadAvarCanvasTimeTable1":
                    GozareshSabteIadAvarCanvasTimeTable1.Children.Add(_Viewbox);
                    break;
                case "GozareshSabteIadAvarCanvasTimeTable2":
                    GozareshSabteIadAvarCanvasTimeTable2.Children.Add(_Viewbox);
                    break;
                case "GozareshSabteIadAvarCanvasTimeTable3":
                    GozareshSabteIadAvarCanvasTimeTable3.Children.Add(_Viewbox);
                    break;
                case "GozareshSabteIadAvarCanvasTimeTable4":
                    GozareshSabteIadAvarCanvasTimeTable4.Children.Add(_Viewbox);
                    break;
                case "GozareshSabteIadAvarCanvasTimeTable5":
                    GozareshSabteIadAvarCanvasTimeTable5.Children.Add(_Viewbox);
                    break;

            }

            if (SabteIadAvarPanel.IsEnabled == true)
            {
                EndWidth1Obj = EndWidth1Obj - Convert.ToInt32(CanvasTimeTable1.ActualWidth);
            }
            else if (GozaresfSabteIadAvarPanel.IsEnabled == true)
            {
                EndWidth1Obj = EndWidth1Obj - Convert.ToInt32(GozareshSabteIadAvarCanvasTimeTable1.ActualWidth);
            }


            StartWidth1Obj = 0;

        }
        private void task_Click(object sender, MouseButtonEventArgs e)
        {

            int i = 0;
            string ID = ((TextBlock)sender).Name;
            StringBuilder sb = new StringBuilder(ID);
            sb.Remove(0, 1);
         Par.IDIadAvarAvalie= Par.IDIadAvar = int.Parse(sb.ToString());
            var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == Par.IDIadAvar).FirstOrDefault();
            if (ispresent != null)
            {
                long IDD = 0;
                TimeSpan _TimeSpan = ispresent.EndDateTime.Value - ispresent.StartDateTime.Value;
                SabteIadAvarTextBox1.Text = ispresent.TitleActivity.ToString();
                SabteIadAvarTimePicker1.Value = ispresent.StartDateTime;
                SabteIadAvarTimePicker2.Value = ispresent.EndDateTime;
                SabteIadAvarTextBox3.Text = ispresent.description;
                SabteIadAvarCombo1.Text = ispresent.MeasurePeriodic.ToString();
                SabteIadAvarCombo2.Text = ispresent.PeriodocKind;
                for (i = 0; i < ispresent.PeriodNumBer; i++)
                {
                    switch (SabteIadAvarCombo2.Text)
                    {
                        case "روز":
                            DDate = ispresent.StartDateTime.Value.AddDays((ispresent.MeasurePeriodic.Value * i) - ispresent.MeasurePeriodic.Value);
                            break;
                        case "ماه":
                            DDate = ispresent.StartDateTime.Value.AddMonths((ispresent.MeasurePeriodic.Value * i) - ispresent.MeasurePeriodic.Value);
                            break;
                        case "سال":
                            DDate = ispresent.StartDateTime.Value.AddYears((ispresent.MeasurePeriodic.Value * i) - ispresent.MeasurePeriodic.Value);
                            break;
                    }
                    if (IDD != ispresent.ID)
                    {
                        IDD = ispresent.ID;
                        i--;
                    }
                    if (new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day) == new DateTime(DDate.Year, DDate.Month, DDate.Day))
                    {
                        break;
                    }
                }
                string istomo = ((TextBlock)sender).Name;
                istomo = istomo[0].ToString();

                try
                {

                    if ((istomo == "A") && (new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day) != new DateTime(ispresent.PeriodicEndTime.Value.Year, ispresent.PeriodicEndTime.Value.Month, ispresent.PeriodicEndTime.Value.Day)))
                    {
                        SabteIadAvarTextBox4.Text = (ispresent.PeriodNumBer.Value - i + 1).ToString();

                    }
                    else
                    {
                        SabteIadAvarTextBox4.Text = (ispresent.PeriodNumBer.Value - i).ToString();


                    }
                }
                catch { }
                toggleButton.IsChecked = ispresent.Periodic.Value;
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject  //پیدا کرد ن کنترل های خاص 
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public void CleanOldDataEnteredTXT()
        {
            foreach (System.Windows.Controls.TextBox tb in FindVisualChildren<System.Windows.Controls.TextBox>(this))
            { tb.Text = string.Empty; }
            foreach (System.Windows.Controls.ComboBox cb in FindVisualChildren<System.Windows.Controls.ComboBox>(this))
            { cb.Text = string.Empty; }

        }        // خالی کردن کلیه تکس باکسها
        public void Toolbarinvisible()
        {

            TakhsisRightToolbar.Visibility = DarooLeftToolbarPanel.Visibility = VamRightToolbar.Visibility = ModiriatSakhtemanRightToolbar.Visibility = CoockRightToolbar.Visibility = KharjKardRightToolbar.Visibility = ProfileLeftToolbarPanel.Visibility = HesabmaliRightToolbar.Visibility = IadAvarRightToolbar.Visibility = Visibility.Collapsed;
            TakhsisRightToolbar.IsEnabled = DarooLeftToolbarPanel.IsEnabled = VamRightToolbar.IsEnabled = ModiriatSakhtemanRightToolbar.IsEnabled = CoockRightToolbar.IsEnabled = KharjKardRightToolbar.IsEnabled = ProfileLeftToolbarPanel.IsEnabled = HesabmaliRightToolbar.IsEnabled = IadAvarRightToolbar.IsEnabled = false;
            Par.ActveRightPanel = "";

        }
        public void Panelinvisible()
        {
            RegisterPanel.Visibility = GozareshTakhsisBodjePanel.Visibility= UpdatePanel.Visibility = GzareshTakhsisHazinehPanel.Visibility = ekhtesaskhsisHazinehPanel.Visibility = TakhsisDaramadPanel.Visibility = TakhsisPanel.Visibility = GozareshOnvanKarKardPanel.Visibility = GozareshNoeKarkardPanel.Visibility = GozareshSaatKarkardPanel.Visibility = NemodarDaramadSakhteman.Visibility = NemodarHazinehSakhteman.Visibility = GozareshSbteSharjPanel.Visibility = MohasebeHazinehSakhtemanTab.Visibility = SabteOnvanHazinehSakhteman.Visibility = SbteSharjPanel.Visibility = GozareshDaroPanel.Visibility = MasrafeDaroPanel.Visibility = GozareshGhazaGheimatPanel.Visibility = GozareshGhazaCaleryPanel.Visibility = GozareshGhazaTdadPanel.Visibility = BarnamehGhazaPanel.Visibility = GozareshHazinehDaramad2Panel.Visibility = GozareshHazinehDaramadPanel.Visibility = GozareshHazinehPanel.Visibility = GozareshDaramadPanel.Visibility = SabteOnvanDaramadPanel.Visibility = TanzimZaherPanel.Visibility = IadAvarDaroPanel.Visibility = GozaeshVamPanel.Visibility = PardakhtVamPanel.Visibility = NobatVamGirandehPanel.Visibility = NameVamGirandehPanel.Visibility = OnvanVamPanel.Visibility = TakhsisHazinehPanel.Visibility = KhomsPanel.Visibility = MohasebehHazinehSakhtemanPanel.Visibility = SbteKharjkardVahedPanel.Visibility = SabetKharjKardsadeSakhtemanPanel.Visibility = SabteHamsaiePanel.Visibility = EntekhabMojodiGhazaPanel.Visibility = EntekhabGheimatGhazaPanel.Visibility = EntekhabCalleryGhazaPanel.Visibility = EntekhabMavadGhazaPanel.Visibility = EntekhabNameGhazaPanel.Visibility = SabteMavadGhazaPanel.Visibility = SabTeNameGhazaPanel.Visibility = MojodiPanel.Visibility = SabetKharjKardKamelPanel.Visibility = SabetKharjKardsadePanel.Visibility = SabetKharjKardPanel.Visibility = SabteKalaPanel.Visibility = SabteKalaPanel.Visibility = textgozareshSabteIadAvarPanel.Visibility = RepeatSabteIadAvarPanel.Visibility = SabteIadAvarTanzimat.Visibility = GozaresfSabteIadAvarPanel.Visibility = SabteIadAvarPanel.Visibility = SabteIadAvarPanel.Visibility = ModiriatChckPanel.Visibility = SabteVamPanel.Visibility = SabteDaramadPanel.Visibility = MoshakhasatManPanel.Visibility = TaghirRamzPanel.Visibility = MohasebeSodBankiPanel.Visibility = MohasebeGarVamPanel.Visibility = SabteSepordePanel.Visibility = Visibility.Collapsed;
            RegisterPanel.IsEnabled = GozareshTakhsisBodjePanel.IsEnabled= UpdatePanel.IsEnabled = GzareshTakhsisHazinehPanel.IsEnabled = ekhtesaskhsisHazinehPanel.IsEnabled = TakhsisDaramadPanel.IsEnabled = TakhsisPanel.IsEnabled = GozareshOnvanKarKardPanel.IsEnabled = GozareshNoeKarkardPanel.IsEnabled = GozareshSaatKarkardPanel.IsEnabled = NemodarDaramadSakhteman.IsEnabled = NemodarHazinehSakhteman.IsEnabled = GozareshSbteSharjPanel.IsEnabled = SabteOnvanHazinehSakhteman.IsEnabled = SbteSharjPanel.IsEnabled = GozareshDaroPanel.IsEnabled = MasrafeDaroPanel.IsEnabled = GozareshGhazaGheimatPanel.IsEnabled = GozareshGhazaCaleryPanel.IsEnabled = GozareshGhazaTdadPanel.IsEnabled = BarnamehGhazaPanel.IsEnabled = GozareshHazinehDaramad2Panel.IsEnabled = GozareshHazinehDaramadPanel.IsEnabled = GozareshHazinehPanel.IsEnabled = GozareshDaramadPanel.IsEnabled = SabteOnvanDaramadPanel.IsEnabled = TanzimZaherPanel.IsEnabled = IadAvarDaroPanel.IsEnabled = GozaeshVamPanel.IsEnabled = PardakhtVamPanel.IsEnabled = NobatVamGirandehPanel.IsEnabled = NameVamGirandehPanel.IsEnabled = OnvanVamPanel.IsEnabled = TakhsisHazinehPanel.IsEnabled = KhomsPanel.IsEnabled = MohasebehHazinehSakhtemanPanel.IsEnabled = SbteKharjkardVahedPanel.IsEnabled = SabetKharjKardsadeSakhtemanPanel.IsEnabled = SabteHamsaiePanel.IsEnabled = EntekhabMojodiGhazaPanel.IsEnabled = EntekhabGheimatGhazaPanel.IsEnabled = EntekhabCalleryGhazaPanel.IsEnabled = EntekhabMavadGhazaPanel.IsEnabled = EntekhabNameGhazaPanel.IsEnabled = SabteMavadGhazaPanel.IsEnabled = SabTeNameGhazaPanel.IsEnabled = MojodiPanel.IsEnabled = SabetKharjKardKamelPanel.IsEnabled = SabetKharjKardsadePanel.IsEnabled = SabetKharjKardPanel.IsEnabled = SabteKalaPanel.IsEnabled = SabteKalaPanel.IsEnabled = textgozareshSabteIadAvarPanel.IsEnabled = RepeatSabteIadAvarPanel.IsEnabled = SabteIadAvarTanzimat.IsEnabled = GozaresfSabteIadAvarPanel.IsEnabled = SabteIadAvarPanel.IsEnabled = ModiriatChckPanel.IsEnabled = SabteVamPanel.IsEnabled = SabteDaramadPanel.IsEnabled = MoshakhasatManPanel.IsEnabled = TaghirRamzPanel.IsEnabled = MohasebeSodBankiPanel.IsEnabled = MohasebeGarVamPanel.IsEnabled = SabteSepordePanel.IsEnabled = false;

        }
        public void ProfileLeftToolbarPanelVisible()
        {
            ProfileLeftToolbarPanel.Visibility = Visibility.Visible; ProfileLeftToolbarPanel.IsEnabled = true;
        }
        public void IadAvarToolbarProfileVisible()
        {
            IadAvarRightToolbar.Visibility = Visibility.Visible; IadAvarRightToolbar.IsEnabled = true;
        }
        public void KharjKardLeftToolbarProfileVisible()
        {
            KharjKardRightToolbar.Visibility = Visibility.Visible; KharjKardRightToolbar.IsEnabled = true;
        }
        public void CoockRightToolbarVisible()
        {
            CoockRightToolbar.Visibility = Visibility.Visible; CoockRightToolbar.IsEnabled = true;
        }
        public void ModiriatSakhtemanRightToolbarVisibile()
        {
            ModiriatSakhtemanRightToolbar.Visibility = Visibility.Visible; ModiriatSakhtemanRightToolbar.IsEnabled = true;
        }
        public void VamToolbarProfileVisible()
        {
            VamRightToolbar.Visibility = Visibility.Visible; VamRightToolbar.IsEnabled = true;
        }
        public void HesabMaliLeftToolbarProfileVisible()
        {
            HesabmaliRightToolbar.Visibility = Visibility.Visible; HesabmaliRightToolbar.IsEnabled = true;
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            //دکمه پروفایل
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ProfileLeftToolbarPanelVisible();
        }

        public byte [] HashMe (string code)
        {
            HashAlgorithm hash = new SHA1Cng();
            byte[] HashByte = hash.ComputeHash(Encoding.UTF8.GetBytes(code));
            return HashByte;
        }
        public void CheckReg()
        {

               var ispresentHardwareCode = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.Description == "HardwareCode");
            var ispresentGUID = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.Description == "GUID");
            if (ispresentGUID == null)
            {
                _ComboBoxTbl.Description = "GUID";
                _ComboBoxTbl.SpecialCode = Guid.NewGuid().ToString();
                _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                _FamilyManaerDBEntities.SaveChanges();
            }
            if (ispresentHardwareCode == null)
            {
                Toolbarinvisible();
                Panelinvisible();
                ProfileUpToolBar.Visibility = HesabMaliupToolbar.Visibility = KharjKardUPToolBar.Visibility = SandoghVamUPToolBar.Visibility = KhomsUPToolBar.Visibility = IadAvarUPToolBar.Visibility = IadavarDaroUPToolBar.Visibility = AshpaziUPToolBar.Visibility = ModiratSakhtemanUPToolBar.Visibility = AmozeshUPToolBar.Visibility = Visibility.Hidden;
                RegisterPanel.Visibility = Visibility.Visible;
                RegisterPanel.IsEnabled = true;
            }
            else
            {
                // if (ispresentHardwareCode.SpecialCode != Convert.ToBase64String(HashMe(GetHardwarSerial()))) 
                if (HashMe(ispresentGUID.SpecialCode + GetHardwarSerial()) == ispresentHardwareCode.File)
                {
                    MajMessageBox.show("نرم افزار شما نامعتبر می باشد", MajMessageBox.MajMessageBoxBut.OK);
                    RegisterPanel.Visibility = Visibility.Visible;
                    RegisterPanel.IsEnabled = true;
                }

                

            }


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "AksPasZamineh");

            using (var ms = new System.IO.MemoryStream(ispresent.aks))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                BackGroundd.ImageSource = image;
            }

            var ispresentFont = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Font");
            if ((ispresentFont.Passage != string.Empty) && (ispresentFont.Passage != null))
            {
                MMainWindow.FontFamily = new System.Windows.Media.FontFamily(ispresentFont.Passage);
                TanzimZaherPanelCombo2.Text = ispresentFont.Passage;
            }

            if (CheckInternetConnection())
            {
                if (update())
            {
                var result = MajMessageBox.show("نسخه جدید موجود می باشد آیا مایل به روز رسانی می باشید?." , MajMessageBox.MajMessageBoxBut.IadAvar);
                if (result.ToString() == "Yes")
                {
                    Panelinvisible();
                    ProfileLeftToolbarPanelVisible();
                    UpdatePanel.Visibility = Visibility.Visible; UpdatePanel.IsEnabled = true;
                }

            }
            }
            CheckReg();
        }

        private void MoshakhasatManBut_Click_1(object sender, RoutedEventArgs e)
        {

            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT(); CleanOldDataEnteredTXT();
            ProfileLeftToolbarPanelVisible();
            MoshakhasatManPanel.Visibility = Visibility.Visible; MoshakhasatManPanel.IsEnabled = true;
        }        //دکمه تولبار مشخصات من
        private void TaghirRamzBut_Click(object sender, RoutedEventArgs e)
        {

            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ProfileLeftToolbarPanelVisible();
            TaghirRamzPanel.Visibility = Visibility.Visible; TaghirRamzPanel.IsEnabled = true;
        }        //دکمه تغییر رمز
        private void HesabMaliupToolbar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            HesabMaliLeftToolbarProfileVisible();
        }
        private void SodButHesabmali_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            HesabMaliLeftToolbarProfileVisible();
            MohasebeSodBankiPanel.Visibility = Visibility.Visible; MohasebeSodBankiPanel.IsEnabled = true;
        }
        private void SodVamBut_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            MohasebeGarVamPanel.Visibility = Visibility.Visible; MohasebeGarVamPanel.IsEnabled = true;
        }
        private void ijadsepordehButHesabmali_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteSepordePanel.Visibility = Visibility.Visible; SabteSepordePanel.IsEnabled = true;

            depositGrid();

        }
        private void SabtDaramadButHesabmali_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteDaramadPanel.Visibility = Visibility.Visible; SabteDaramadPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      select p.Deposit;
            if (Fin != null)
            {
                SabteDaramadCombo1.ItemsSource = Fin.ToList();
            }
            CreateDataGridForIncome();
            selectDaramadGrid();
        }
        private void SabtVamButHesabmali_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteVamPanel.Visibility = Visibility.Visible; SabteVamPanel.IsEnabled = true;
            createSabteVamPanelGrid();
            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      select p.Deposit;
            if (Fin != null)
            {
                SabteVamPanelCombo1.ItemsSource = Fin.ToList();
            }
        }
        private void ModiriatCheckButHesabmali_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatChckPanel.Visibility = Visibility.Visible; ModiriatChckPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      select p.Deposit;
            if (Fin != null)
            {
                ModiriatChckCombo1.ItemsSource = Fin.ToList();
            }
            createSabtecheckGrid();
        }
        private void IadAvarUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            Toolbarinvisible();
            IadAvarRightToolbar.Visibility = Visibility.Visible; IadAvarRightToolbar.IsEnabled = true;
            Par.ActveRightPanel = "IadAvarRightToolbar";
        }
        public void CreaeListBoxKar()
        {
            ListBox4.Items.Clear();
            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.OnvanKar != null
                      orderby _.OnvanKar descending
                      select _;
            foreach (var item in Fin)
            {
                ListBox4.Items.Add(item.OnvanKar);
            }
            //  ListBox4.ItemsSource = Fin.ToList();
        }
        private void SabtefaaliatIadAvarBut_Click(object sender, RoutedEventArgs e)//دکمه تولبار سمت چپ ثبت فعالیت روزانه
        {


            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteIadAvarPanel.Visibility = Visibility.Visible; SabteIadAvarPanel.IsEnabled = true;
            CanvasTimeTable1.Children.Clear();
            CreaeListBoxKar();

        }
        public Boolean CheckDonotRepeatIadAvar(DateTime Start , DateTime Finish , Boolean periodic , int PeriodNumberrrr , string PeriodKind , int MeasurePeriodic)
        {
           // Par.ID = 0;
            Boolean Save = true;
            if (Start == Finish)
            {
                MajMessageBox.show("زمان شروع و پایان با هم یکسان هستند", MajMessageBox.MajMessageBoxBut.OK);
                return Save;

            }
            List<SaatOnvanKar> saatOnvanKarsList = new List<SaatOnvanKar>();
            DateTime startdate = Start;
            DateTime FinishDate = Finish;

            SaatOnvanKar AddSaat(DateTime start, DateTime finish, long ID, string OnvanKar)
            {
                SaatOnvanKar _SaatOnvanKar = new SaatOnvanKar();
                _SaatOnvanKar.Start = start;
                _SaatOnvanKar.Finish = Finish;
                _SaatOnvanKar.ID = ID;
                _SaatOnvanKar.OnvanKar = OnvanKar;
                return (_SaatOnvanKar);
            }

            for ( int PeriodNumber = 0; PeriodNumber < PeriodNumberrrr; PeriodNumber++)
            {
                if (periodic == true)
                {
                    switch (PeriodKind)
                    {
                        case "روز":
                            startdate = Start.AddDays(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            FinishDate = Finish.AddDays(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            break;
                        case "ماه":
                            startdate = Start.AddMonths(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            FinishDate = Finish.AddMonths(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            break;
                        case "سال":
                            startdate = Start.AddYears(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            FinishDate = Finish.AddYears(int.Parse(SabteIadAvarCombo1.Text) * PeriodNumber);
                            break;
                    }
                }
                var FinOnvanKar = from _ in _FamilyManaerDBEntities.IadAvarTbls
                                  where (
                                         ((_.StartDateTime < startdate && (startdate < _.EndDateTime) && (_.Periodic == false))
                                        || ((_.StartDateTime < FinishDate) && (FinishDate < _.EndDateTime) && (_.Periodic == false))
                                        || ((_.StartDateTime >= startdate) && (FinishDate >= _.EndDateTime) && (_.Periodic == false)))
                                        || ((_.StartDateTime < startdate && (startdate < _.PeriodicEndTime) && (_.Periodic == true))
                                        || ((_.StartDateTime < FinishDate) && (FinishDate < _.PeriodicEndTime) && (_.Periodic == true))
                                        || ((_.StartDateTime >= startdate) && (FinishDate >= _.PeriodicEndTime) && (_.Periodic == true))))
                                  select _;


                foreach (var itemOnvanKar in FinOnvanKar)
                {

                    if (itemOnvanKar.Periodic == true && itemOnvanKar.ID != Par.IDIadAvarAvalie) // کار دوره ایی است 
                    {
                        DateTime? NextStartPeriodTime = null, NexEndPeriodTime = null;
                        for (int i = 0; i <= itemOnvanKar.PeriodNumBer; i++) // پیدا کردن شماره شروع دوره تلاقی یافته با بازه زمانی
                        {

                            switch (itemOnvanKar.PeriodocKind )
                            {
                                case "روز":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "ماه":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "سال":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                            }
                            saatOnvanKarsList.Add(AddSaat(NextStartPeriodTime.Value, NexEndPeriodTime.Value, itemOnvanKar.ID, itemOnvanKar.TitleActivity));
                            if (FinishDate < NextStartPeriodTime)
                            {
                                break;
                            }
                        }
                    }
                    else  // کار دوره ایی نیست 
                    {
                        saatOnvanKarsList.Add(AddSaat(itemOnvanKar.StartDateTime.Value, itemOnvanKar.EndDateTime.Value, itemOnvanKar.ID, itemOnvanKar.TitleActivity));

                    }
                }

                var FinExist = from _ in saatOnvanKarsList
                               where (
                               (_.Start <= startdate && startdate <= _.Finish)
                                        || (_.Start <= FinishDate && FinishDate <= _.Finish)
                                        || (_.Start >= startdate && FinishDate >= _.Finish)
                                        || (_.Start == startdate && FinishDate == _.Finish)
                                        )
                               select _;
                foreach (var item in FinExist)
                 {
                    var Time = _FamilyManaerDBEntities.IadAvarTbls.FirstOrDefault(_ => _.ID == item.ID);

                    // if ((SabteIadAvarPanel.IsEnabled == true && Par.ID != item.ID && Viraiesh) || (SabteIadAvarPanel.IsEnabled == true && Viraiesh == false)) /// تکراری ها
                    if (SabteIadAvarPanel.IsEnabled == true && Par.IDIadAvar != item.ID && Par.IDIadAvarAvalie!=item.ID ) /// تکراری ها
                    {

                        var result = MajMessageBox.show("بازه زمانی انتخابی با فعالیت زیر تداخل دارد آیا مایل هستید هر دوفعالیت با هم ذخیره شوند." + Environment.NewLine + Time.TitleActivity, MajMessageBox.MajMessageBoxBut.IadAvar);
                        if (result.ToString() == "OK")
                        {
                            Save = false;
                            break;
                        }
                        else if (result.ToString() == "No")
                        {
                            Save = false;
                            Panelinvisible();
                            RepeatSabteIadAvarPanel.Visibility = Visibility.Visible;
                            RepeatSabteIadAvarPanel.IsEnabled = true;
                            TimeSpan _TimeSpan = Time.EndDateTime.Value - Time.StartDateTime.Value;
                            RepeatSabteIadAvarTextBox1.Text = Time.TitleActivity.ToString();
                            RepeatSabteIadAvarTimePicker1.Value = Time.StartDateTime;
                            RepeatSabteIadAvarTimePicker2.Value = Time.EndDateTime;
                            RepeatSabteIadAvarTextBox3.Text = Time.description;
                            RepeatSabteIadAvarCombo1.Text = Time.MeasurePeriodic.ToString();
                            RepeatSabteIadAvarCombo2.Text = Time.PeriodocKind;
                            RepeatSabteIadAvarPanelTextBox2.Text = Time.PersianStartDate;
                            RepeatSabteIadAvarPaneltoggleButton.IsChecked = Time.Periodic;
                            Par.IDIadAvar = Time.ID;
                            Par._DateTimeVariableStart = Time.StartDateTime;
                            if (Time.PeriodNumBer != null)
                            {
                                RepeatSabteIadAvarTextBox4.Text = (Time.PeriodNumBer.Value).ToString();
                            }
                        }
                        else if (result.ToString() == "Yes")
                        {
                            Par._DateTimeVariable = Time.StartDateTime;
                            Save = true;

                        }
                        break;
                    }
                    else  if (RepeatSabteIadAvarPanel.IsEnabled == true && Par.IDIadAvar != item.ID && Par.IDIadAvarAvalie != item.ID) 
                    {
                        Save = false;
                        MajMessageBox.show("بازه زمانی انتخابی با فعالیت زیر تداخل دارد."
                            + Environment.NewLine + "عنوان فعالیت :" + Time.TitleActivity
                            + Environment.NewLine + "ساعت شروع :" + Time.PersianStartTime
                            + Environment.NewLine + "ساعت پایان :" + Time.PersianEndTime

                            , MajMessageBox.MajMessageBoxBut.OK);
                        break;
                    }

                    
                }
            }
            return Save;
        }
        public void sabteIadAvar(Boolean Viraiesh, string PanelName , long ID) // تابع ثبت یادآور
        {
            
            if (PanelName == "RepeatSabteIadAvarPanel")
            {
                
                var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == ID).FirstOrDefault();
              //  var starttime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0);
             //   var finishtime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0);
               var StartTimePicker = RepeatSabteIadAvarTimePicker1.Value;
               var EndTimePicker = RepeatSabteIadAvarTimePicker2.Value;
               // DateTime SSStart = new DateTime(ispresent.StartDateTime.Value.Year, ispresent.StartDateTime.Value.Month, ispresent.StartDateTime.Value.Day, StartTimePicker.Value.Hour, StartTimePicker.Value.Minute, 0, 0, 0);
              //  DateTime FFFinish = new DateTime(ispresent.StartDateTime.Value.Year, ispresent.StartDateTime.Value.Month, ispresent.StartDateTime.Value.Day, EndTimePicker.Value.Hour, EndTimePicker.Value.Minute, 0, 0, 0);
                DateTime SSStart = new DateTime(Par._DateTimeVariableStart.Value.Year, Par._DateTimeVariableStart.Value.Month, Par._DateTimeVariableStart.Value.Day, StartTimePicker.Value.Hour, StartTimePicker.Value.Minute, 0, 0, 0);
                DateTime FFFinish = new DateTime(Par._DateTimeVariableStart.Value.Year, Par._DateTimeVariableStart.Value.Month, Par._DateTimeVariableStart.Value.Day, EndTimePicker.Value.Hour, EndTimePicker.Value.Minute, 0, 0, 0);
                if (RepeatSabteIadAvarTimePicker1.Value > RepeatSabteIadAvarTimePicker2.Value)
                {
                    FFFinish = FFFinish.AddDays(1);
                }
                ispresent.TitleActivity = RepeatSabteIadAvarTextBox1.Text;
                ispresent.PersianStartDate = RepeatSabteIadAvarPanelTextBox2.Text;
                ispresent.PersianStartTime = StartTimePicker.Value.Hour + " : " + StartTimePicker.Value.Minute;
                ispresent.StartDateTime = SSStart;
                ispresent.description = RepeatSabteIadAvarTextBox3.Text;
                ispresent.PersianEndTime = EndTimePicker.Value.Hour + " : " + EndTimePicker.Value.Minute;
                ispresent.EndDateTime = FFFinish;
                if (RepeatSabteIadAvarPaneltoggleButton.IsChecked == true)
                {
                    int PeriodNumBer = int.Parse(RepeatSabteIadAvarTextBox4.Text);
                    int MeasurePeriodic = int.Parse(RepeatSabteIadAvarCombo1.Text);
                    switch (SabteIadAvarCombo2.Text)
                    {
                        case "روز":
                            ispresent.PeriodicEndTime = SSStart.AddDays((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "ماه":
                            ispresent.PeriodicEndTime = SSStart.AddMonths((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "سال":
                            ispresent.PeriodicEndTime = SSStart.AddYears((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                    }
                    ispresent.PeriodNumBer = PeriodNumBer;
                    ispresent.PeriodocKind = RepeatSabteIadAvarCombo2.Text;
                    ispresent.MeasurePeriodic = MeasurePeriodic;
                    
                }
                else
                {
                    ispresent.PeriodNumBer = null;
                    ispresent.PeriodocKind = null;
                    ispresent.MeasurePeriodic = null;
                }
                _FamilyManaerDBEntities.SaveChanges();

                
               // MajMessageBox.show("اطلاعات با موفقیت تغییر یافت.", MajMessageBox.MajMessageBoxBut.OK);
                Panelinvisible();
                SabteIadAvarPanel.Visibility = Visibility.Visible;
                SabteIadAvarPanel.IsEnabled = true;
               
                if (Par.viraieshIadavar)
                {
                    var starttime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0);
                    var finishtime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0);
                    if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
                    {
                        finishtime = finishtime.AddDays(1);
                    }
                    if (toggleButton.IsChecked.Value == true)
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, int.Parse(SabteIadAvarTextBox4.Text), SabteIadAvarCombo2.Text, int.Parse(SabteIadAvarCombo1.Text)))
                        {
                            sabteIadAvar(true, "ViraeshSabteFaaliat", Par.IDIadAvarAvalie);
                        }
                    }
                    else
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, 1, "Nothing", 1))
                        {
                            //Par.viraiesh = true;
                            //  Par.IDD = Par.ID;
                            sabteIadAvar(true, "ViraeshSabteFaaliat", Par.IDIadAvar);
                        }
                    }

                }
                else 
                {
                    var starttime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0);
                    var finishtime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0);
                    if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
                    {
                        finishtime = finishtime.AddDays(1);
                    }
                    if (toggleButton.IsChecked == true)
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, int.Parse(SabteIadAvarTextBox4.Text), SabteIadAvarCombo2.Text, int.Parse(SabteIadAvarCombo1.Text)))
                        {
                            //  Par.viraiesh = false;
                            sabteIadAvar(false, "SabteJadid", 0);
                        }
                    }
                    else
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, 1, "", 1))
                        {
                            // Par.viraiesh = false;
                            sabteIadAvar(false, "SabteJadid", 0);
                        }
                    }
                }

            }
          else  if (Viraiesh && PanelName == "ViraeshSabteFaaliat")  // ویرایش یادآور
            {
                var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == ID).FirstOrDefault();
                var StartTimePicker = SabteIadAvarTimePicker1.Value;
                var EndTimePicker = SabteIadAvarTimePicker2.Value;
                DateTime SSStart = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month , Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute,0,0,0);
                DateTime FFFinish = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0, 0, 0);
                if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
                {
                    FFFinish = FFFinish.AddDays(1);
                }
                ispresent.TitleActivity = SabteIadAvarTextBox1.Text;
                ispresent.PersianStartDate = SabteIadAvarTextBox2.Text;
                ispresent.PersianStartTime = StartTimePicker.Value.Hour + " : " + StartTimePicker.Value.Minute;
                ispresent.StartDateTime = SSStart;
                ispresent.description = SabteIadAvarTextBox3.Text;


                ispresent.PersianEndTime = EndTimePicker.Value.Hour + " : " + EndTimePicker.Value.Minute;
                ispresent.EndDateTime = FFFinish;
                ispresent.Periodic = toggleButton.IsChecked;
                if (toggleButton.IsChecked == true)
                {
                    int PeriodNumBer = int.Parse(SabteIadAvarTextBox4.Text);
                    int MeasurePeriodic = int.Parse(SabteIadAvarCombo1.Text);
                    switch (SabteIadAvarCombo2.Text)
                    {
                        case "روز":
                            ispresent.PeriodicEndTime = SSStart.AddDays((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "ماه":
                            ispresent.PeriodicEndTime = SSStart.AddMonths((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "سال":
                            ispresent.PeriodicEndTime = SSStart.AddYears((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                    }
                    ispresent.PeriodNumBer = PeriodNumBer;
                    ispresent.PeriodocKind = SabteIadAvarCombo2.Text;
                    ispresent.MeasurePeriodic = MeasurePeriodic;
                }
                else
                {
                    ispresent.PeriodNumBer = null;
                    ispresent.PeriodocKind = null;
                    ispresent.MeasurePeriodic = null;
                }
                _FamilyManaerDBEntities.SaveChanges();
                  Par.ID = -1;

                MajMessageBox.show("اطلاعات با موفقیت تغییر یافت.", MajMessageBox.MajMessageBoxBut.OK);
                SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                toggleButton.IsChecked = false;
                CanvasTimeTable1.Children.Clear();
                SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
                CreaeListBoxKar();
            }
           else if (!Viraiesh  && PanelName == "SabteJadid")    // ذخیره بادآور
            {

                var StartTimePicker = SabteIadAvarTimePicker1.Value;
                var EndTimePicker = SabteIadAvarTimePicker2.Value;
                DateTime SSStart = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0, 0, 0);
                DateTime FFFinish = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0, 0, 0);
                if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
                {
                    FFFinish = FFFinish.AddDays(1);
                }
                _IadAvarTbl.TitleActivity = SabteIadAvarTextBox1.Text;
                _IadAvarTbl.PersianStartDate = SabteIadAvarTextBox2.Text;
                _IadAvarTbl.PersianStartTime = StartTimePicker.Value.Hour.ToString().PadLeft(2, '0') + " : " + StartTimePicker.Value.Minute.ToString().PadLeft(2, '0');
                _IadAvarTbl.StartDateTime = SSStart;
                _IadAvarTbl.PersianEndTime = EndTimePicker.Value.Hour.ToString().PadLeft(2, '0') + " : " + EndTimePicker.Value.Minute.ToString().PadLeft(2, '0');
                _IadAvarTbl.EndDateTime = FFFinish;
                _IadAvarTbl.ReminderCategory = "یادآور کارها";
                _IadAvarTbl.description = SabteIadAvarTextBox3.Text;
                _IadAvarTbl.Periodic = toggleButton.IsChecked;
                if (toggleButton.IsChecked == true)
                {
                    int PeriodNumBer = int.Parse(SabteIadAvarTextBox4.Text);
                    int MeasurePeriodic = int.Parse(SabteIadAvarCombo1.Text);
                    switch (SabteIadAvarCombo2.Text)
                    {
                        case "روز":
                            _IadAvarTbl.PeriodicEndTime = SSStart.AddDays((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "ماه":
                            _IadAvarTbl.PeriodicEndTime = SSStart.AddMonths((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                        case "سال":
                            _IadAvarTbl.PeriodicEndTime = SSStart.AddYears((MeasurePeriodic * PeriodNumBer) - MeasurePeriodic);
                            break;
                    }
                    _IadAvarTbl.PeriodNumBer = PeriodNumBer;
                    _IadAvarTbl.PeriodocKind = SabteIadAvarCombo2.Text;
                    _IadAvarTbl.MeasurePeriodic = MeasurePeriodic;
                }
                _FamilyManaerDBEntities.IadAvarTbls.Add(_IadAvarTbl);
                _FamilyManaerDBEntities.SaveChanges();
                //Save = false;
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                toggleButton.IsChecked = false;
                CanvasTimeTable1.Children.Clear();
                SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
                CreaeListBoxKar();
            }
            //CanvasTimeTable1.Children.Clear();
            //SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
            //CreaeListBoxKar();
            //SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
            //SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
            //toggleButton.IsChecked = false;
        }
        public void SabteIadAvarBut1_Click(object sender, RoutedEventArgs e) // دکمه ثبت یاد آور 
        {
            if ((SabteIadAvarTextBox1.Text == "") || (SabteIadAvarTextBox2.Text == "") || (SabteIadAvarTimePicker1.Value == null) || (SabteIadAvarTimePicker2.Value == null))
            { MajMessageBox.show("لطفاً تمامی فیلدها را تکمیل نمایید.", MajMessageBox.MajMessageBoxBut.OK); return; }

            var ispresent = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.OnvanKar == SabteIadAvarTextBox1.Text);
            if (ispresent == null)
            {
                var result = MajMessageBox.show(" عنوان فعالیت زیر قبلاً در لیست فعالیت ها ذخیره نشده است آیا مایل به ذخیره آن می باشید" + Environment.NewLine + SabteIadAvarTextBox1.Text, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result.ToString() == "Yes")
                {
                    _ComboBoxTbl.OnvanKar = SabteIadAvarTextBox1.Text;
                    _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                }
            }
            Par.viraieshIadavar = false;
            var starttime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0);
            var finishtime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0);
            if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
            {
                finishtime = finishtime.AddDays(1);
            }
            if (toggleButton.IsChecked==true)
            {
                if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, int.Parse(SabteIadAvarTextBox4.Text), SabteIadAvarCombo2.Text, int.Parse(SabteIadAvarCombo1.Text)))
                {
                  //  Par.viraiesh = false;
                    sabteIadAvar(false, "SabteJadid",0);
                }
            }
            else
            {
                if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, 1, "", 1))
                {
                   // Par.viraiesh = false;
                    sabteIadAvar(false, "SabteJadid",0);
                }
            }

            if (RepeatSabteIadAvarPanel.Visibility==Visibility.Hidden)
            {
                SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                toggleButton.IsChecked = false;
                CreaeListBoxKar();
            }





        }
        private void MoshahedefaaliatIadAvarBut_Click(object sender, RoutedEventArgs e)
        {

            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            GozaresfSabteIadAvarPanel.Visibility = Visibility.Visible; GozaresfSabteIadAvarPanel.IsEnabled = true;
            GozareshSabteIadAvarCanvasTimeTable1.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable2.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable3.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable4.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable5.Children.Clear();
            GozareshSabteIadAvarlabel1.Content = GozareshSabteIadAvarlabel2.Content = GozareshSabteIadAvarlabel3.Content = GozareshSabteIadAvarlabel4.Content = string.Empty;
        }

        private void PersianCalendarBut_Click(object sender, RoutedEventArgs e)
        {

            Par._DateTimeVariable = PerCalendar.Date.start();
            Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
            Par.Year = Date.PYear.ToString();
            SabteIadAvarTextBox2.Text = SabteDaramadTextBox2.Text = Par.Tarikh;
            if (SabteIadAvarPanel.Visibility == Visibility.Visible)
            {
                CanvasTimeTable1.Children.Clear();
                SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
                //   SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                // SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                //toggleButton.IsChecked = false;
            }
            else if (SabteDaramadPanel.Visibility == Visibility.Visible) { }
            else if (SbteKharjkardVahedPanel.Visibility == Visibility.Visible)
            {

                SbteKharjkardVahedTextBox2.Text = Par.Tarikh;
                switch (Date.PMonth)
                {
                    case 1:
                        SbteKharjkardVahedCombo1.Text = "فروردین";
                        break;
                    case 2:
                        SbteKharjkardVahedCombo1.Text = "اردیبهشت";
                        break;
                    case 3:
                        SbteKharjkardVahedCombo1.Text = "خرداد";
                        break;
                    case 4:
                        SbteKharjkardVahedCombo1.Text = "تیر";
                        break;
                    case 5:
                        SbteKharjkardVahedCombo1.Text = "مرداد";
                        break;
                    case 6:
                        SbteKharjkardVahedCombo1.Text = "شهریور";
                        break;
                    case 7:
                        SbteKharjkardVahedCombo1.Text = "مهر";
                        break;
                    case 8:
                        SbteKharjkardVahedCombo1.Text = "آبان";
                        break;
                    case 9:
                        SbteKharjkardVahedCombo1.Text = "آذر";
                        break;
                    case 10:
                        SbteKharjkardVahedCombo1.Text = "دی";
                        break;
                    case 11:
                        SbteKharjkardVahedCombo1.Text = "بهمن";
                        break;
                    case 12:
                        SbteKharjkardVahedCombo1.Text = "اسفند";
                        break;
                }
            }
            else if (SabetKharjKardsadeSakhtemanPanel.Visibility == Visibility.Visible)
            {
                SabetKharjKardsadeSakhtemanTextBox2.Text = Par.Tarikh;
                switch (Date.PMonth)
                {
                    case 1:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "فروردین";
                        break;
                    case 2:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "اردیبهشت";
                        break;
                    case 3:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "خرداد";
                        break;
                    case 4:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "تیر";
                        break;
                    case 5:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "مرداد";
                        break;
                    case 6:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "شهریور";
                        break;
                    case 7:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "مهر";
                        break;
                    case 8:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "آبان";
                        break;
                    case 9:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "آذر";
                        break;
                    case 10:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "دی";
                        break;
                    case 11:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "بهمن";
                        break;
                    case 12:
                        SabetKharjKardsadeSakhtemanCombo1.Text = "اسفند";
                        break;
                }
            }
            else if (SabetKharjKardKamelPanel.Visibility == Visibility.Visible) { SabetKharjKardKamelTextBox2.Text = Par.Tarikh; CreateSabteKharjkardKamel(); }
            else if (PardakhtVamPanel.Visibility == Visibility.Visible) { PardakhtVamTextBox5.Text = Par.Tarikh; }
            else if (NobatVamGirandehPanel.Visibility == Visibility.Visible) { NobatVamGirandehTextBox5.Text = Par.Tarikh; }
            else if (SabteVamPanel.Visibility == Visibility.Visible) { SabteVamPanelTextBox6.Text = Par.Tarikh; }
            else if (SabetKharjKardsadePanel.Visibility == Visibility.Visible) { SabetKharjKardsadeTextBox2.Text = Par.Tarikh; CreateSabteKharjkardSade(); }
            else if (ModiriatChckPanel.Visibility == Visibility.Visible) { ModiriatChckTextBox2.Text = Par.Tarikh; }
            else if (GozaresfSabteIadAvarPanel.IsEnabled == true)
            {
                GozareshSabteIadAvarTextBox1.Text = Par.Tarikh;
                PersianCalendar p = new PersianCalendar();
                var d = Par._DateTimeVariable.Value.AddDays(1);
                GozareshSabteIadAvarlabel1.Content = p.GetYear(d) + "/" + p.GetMonth(d) + "/" + p.GetDayOfMonth(d);
                d = Par._DateTimeVariable.Value.AddDays(2);
                GozareshSabteIadAvarlabel2.Content = p.GetYear(d) + "/" + p.GetMonth(d) + "/" + p.GetDayOfMonth(d);
                d = Par._DateTimeVariable.Value.AddDays(3);
                GozareshSabteIadAvarlabel3.Content = p.GetYear(d) + "/" + p.GetMonth(d) + "/" + p.GetDayOfMonth(d);
                d = Par._DateTimeVariable.Value.AddDays(4);
                GozareshSabteIadAvarlabel4.Content = p.GetYear(d) + "/" + p.GetMonth(d) + "/" + p.GetDayOfMonth(d);

                GozareshSabteIadAvarCanvasTimeTable1.Children.Clear();
                GozareshSabteIadAvarCanvasTimeTable2.Children.Clear();
                GozareshSabteIadAvarCanvasTimeTable3.Children.Clear();
                GozareshSabteIadAvarCanvasTimeTable4.Children.Clear();
                GozareshSabteIadAvarCanvasTimeTable5.Children.Clear();
                SpecifyTime("GozareshSabteIadAvarCanvasTimeTable1", Par._DateTimeVariable.Value);
                SpecifyTime("GozareshSabteIadAvarCanvasTimeTable2", Par._DateTimeVariable.Value.AddDays(1));
                SpecifyTime("GozareshSabteIadAvarCanvasTimeTable3", Par._DateTimeVariable.Value.AddDays(2));
                SpecifyTime("GozareshSabteIadAvarCanvasTimeTable4", Par._DateTimeVariable.Value.AddDays(3));
                SpecifyTime("GozareshSabteIadAvarCanvasTimeTable5", Par._DateTimeVariable.Value.AddDays(4));

            }
        }
        private void TanzimatIadAvarBut_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteIadAvarTanzimat.Visibility = Visibility.Visible; SabteIadAvarTanzimat.IsEnabled = true;
        }        //دکمه سمت راست تنظیمات پنل یادآور
        private void SabteIadAvarTanzimatBut1_Click(object sender, RoutedEventArgs e)
        {

        }        //دکمه ثبت تنظیمات پنل یادآور
        private void CanvasTimeTable1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasTimeTable1.Children.Clear();
        }
        private void SabteIadAvarBut2_Click(object sender, RoutedEventArgs e) // حذف یادآور
        {
            if (Par.IDIadAvar == -1) { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == Par.IDIadAvar).FirstOrDefault();
            if (ispresent != null)
            {
                var result = MajMessageBox.show("آیا از حذف فعالیت زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleActivity, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    _FamilyManaerDBEntities.IadAvarTbls.Remove(ispresent);
                    _FamilyManaerDBEntities.SaveChanges();
                    Par.IDIadAvar = -1;
                    SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                    SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                    toggleButton.IsChecked = false;
                    CanvasTimeTable1.Children.Clear();
                    SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
                    MajMessageBox.show("اطلاعات با موفقیت پاک شد.", MajMessageBox.MajMessageBoxBut.OK);

                }

            }
            else { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); }
        }         //حذف یادآور کارها
        private void SabteIadAvarBut3_Click(object sender, RoutedEventArgs e) // ویرایش یادآور
        {

            if (Par.IDIadAvar == -1) { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            if ((SabteIadAvarTextBox1.Text == "") || (SabteIadAvarTextBox2.Text == "") || (SabteIadAvarTimePicker1.Value == null) || (SabteIadAvarTimePicker2.Value == null))
            { MajMessageBox.show("لطفاً تمامی فیلدها را تکمیل نمایید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == Par.IDIadAvar).FirstOrDefault();
            if (ispresent != null)
            {
                var result = MajMessageBox.show("آیا از تغییر فعالیت زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleActivity, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    Par.viraieshIadavar = true;
                    var starttime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker1.Value.Value.Hour, SabteIadAvarTimePicker1.Value.Value.Minute, 0);
                    var finishtime = new DateTime(Par._DateTimeVariable.Value.Year, Par._DateTimeVariable.Value.Month, Par._DateTimeVariable.Value.Day, SabteIadAvarTimePicker2.Value.Value.Hour, SabteIadAvarTimePicker2.Value.Value.Minute, 0);
                    if (SabteIadAvarTimePicker1.Value.Value.Hour > SabteIadAvarTimePicker2.Value.Value.Hour)
                    {
                        finishtime = finishtime.AddDays(1);
                    }
                    if (toggleButton.IsChecked.Value == true)
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, int.Parse(SabteIadAvarTextBox4.Text), SabteIadAvarCombo2.Text, int.Parse(SabteIadAvarCombo1.Text)))
                        {
                            //Par.viraiesh = true;
                            // Par.IDD = Par.ID;
                            sabteIadAvar(true, "ViraeshSabteFaaliat", Par.IDIadAvarAvalie);
                        }
                    }
                    else
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, toggleButton.IsChecked.Value, 1, "Nothing", 1))
                        {
                            //Par.viraiesh = true;
                            //  Par.IDD = Par.ID;
                            sabteIadAvar(true, "ViraeshSabteFaaliat", Par.IDIadAvarAvalie);
                        }
                    }

                }
                if (RepeatSabteIadAvarPanel.Visibility == Visibility.Hidden)
                {
                    SabteIadAvarTextBox4.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTextBox1.Text = "";
                    SabteIadAvarCombo2.Text = SabteIadAvarCombo1.Text = SabteIadAvarTextBox3.Text = SabteIadAvarTimePicker2.Text = SabteIadAvarTimePicker1.Text = "";
                    toggleButton.IsChecked = false;
                    CreaeListBoxKar();
                }

            }
            else { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); }

        }        
        public void MohasebehSod()
        {
            try
            {
                string x1 = "0", x2 = "0", x3 = "0";
                if (MohasebeSodBankiTextBox1.Text == "") { x1 = "0"; } else { x1 = MohasebeSodBankiTextBox1.Text; }
                if (MohasebeSodBankiTextBox2.Text == "") { x2 = "0"; } else { x2 = MohasebeSodBankiTextBox2.Text; }
                if (MohasebeSodBankiTextBox3.Text == "") { x3 = "0"; } else { x3 = MohasebeSodBankiTextBox3.Text; }
                MohasebeSodBankiTextBox4.Text = (decimal.Parse(x1) * decimal.Parse(x2) * decimal.Parse(x3) / 1200).ToString("0");
                MohasebeSodBankiTextBox5.Text = (decimal.Parse(x1) * decimal.Parse(x2) / 100).ToString("0");
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeSodBankiTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeSodBankiTextBox1.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeSodBankiTextBox1.Text = string.Format("{0:N0}", number);
                    MohasebeSodBankiTextBox1.SelectionStart = MohasebeSodBankiTextBox1.Text.Length;
                    MohasebehSod();
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeSodBankiTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeSodBankiTextBox2.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeSodBankiTextBox2.Text = string.Format("{0}", number);
                    MohasebeSodBankiTextBox2.SelectionStart = MohasebeSodBankiTextBox1.Text.Length;
                    if (int.Parse(MohasebeSodBankiTextBox2.Text) > 100) { MohasebeSodBankiTextBox2.Text = "100"; }
                    MohasebehSod();
                }

            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeSodBankiTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeSodBankiTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeSodBankiTextBox3.Text = string.Format("{0:N0}", number);
                    MohasebeSodBankiTextBox3.SelectionStart = MohasebeSodBankiTextBox3.Text.Length;
                    MohasebehSod();
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeSodBankiTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number = 0;
                if (decimal.TryParse(MohasebeSodBankiTextBox4.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeSodBankiTextBox4.Text = string.Format("{0:N0}", number);
                    MohasebeSodBankiTextBox4.SelectionStart = MohasebeSodBankiTextBox4.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeSodBankiTextBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeSodBankiTextBox5.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeSodBankiTextBox5.Text = string.Format("{0:N0}", number);
                    MohasebeSodBankiTextBox5.SelectionStart = MohasebeSodBankiTextBox5.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }

        }
        public void MohasebehVam()
        {
            try
            {
                string x1 = "0", x2 = "0", x3 = "0";
                if (MohasebeGarVamPanelTextBox1.Text == "") { x1 = "0"; } else { x1 = MohasebeGarVamPanelTextBox1.Text; }
                if (MohasebeGarVamPanelTextBox2.Text == "") { x2 = "0"; } else { x2 = MohasebeGarVamPanelTextBox2.Text; }
                if (MohasebeGarVamPanelTextBox3.Text == "") { x3 = "0"; } else { x3 = MohasebeGarVamPanelTextBox3.Text; }
                string chandmah = x2;
                if (chandmah == "0") { chandmah = "1"; }
                string SodeKol = (decimal.Parse(x1) * decimal.Parse(x2) * decimal.Parse(x3) / 2400).ToString("0");
                MohasebeGarVamPanelTextBox4.Text = SodeKol;
                MohasebeGarVamPanelTextBox5.Text = ((decimal.Parse(SodeKol) + (decimal.Parse(x1))) / decimal.Parse(chandmah)).ToString("0");
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeGarVamPanelTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeGarVamPanelTextBox1.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeGarVamPanelTextBox1.Text = string.Format("{0:N0}", number);
                    MohasebeGarVamPanelTextBox1.SelectionStart = MohasebeGarVamPanelTextBox1.Text.Length;
                    MohasebehVam();
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeGarVamPanelTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeGarVamPanelTextBox2.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeGarVamPanelTextBox2.Text = string.Format("{0:N0}", number);
                    MohasebeGarVamPanelTextBox2.SelectionStart = MohasebeGarVamPanelTextBox2.Text.Length;
                    MohasebehVam();
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeGarVamPanelTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeGarVamPanelTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeGarVamPanelTextBox3.Text = string.Format("{0}", number);
                    MohasebeGarVamPanelTextBox3.SelectionStart = MohasebeGarVamPanelTextBox3.Text.Length;
                    if (int.Parse(MohasebeGarVamPanelTextBox3.Text) > 100) { MohasebeGarVamPanelTextBox3.Text = "100"; }
                    MohasebehVam();
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeGarVamPanelTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(MohasebeGarVamPanelTextBox4.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeGarVamPanelTextBox4.Text = string.Format("{0:N0}", number);
                    MohasebeGarVamPanelTextBox4.SelectionStart = MohasebeGarVamPanelTextBox4.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void MohasebeGarVamPanelTextBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(MohasebeGarVamPanelTextBox5.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MohasebeGarVamPanelTextBox5.Text = string.Format("{0:N0}", number);
                    MohasebeGarVamPanelTextBox5.SelectionStart = MohasebeGarVamPanelTextBox5.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void SabteDaramadTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabteDaramadTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SabteDaramadTextBox3.Text = string.Format("{0:N0}", number);
                    SabteDaramadTextBox3.SelectionStart = SabteDaramadTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        private void SabteDaramadTBut2_Click(object sender, RoutedEventArgs e) // دکمه ثبت درآمد
        {
            try
            {
                if (SabteDaramadTextBox1.Text == "")
                {
                    MajMessageBox.show("لطفاً عنوان درآمد را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (SabteDaramadTextBox2.Text == "")
                {
                    MajMessageBox.show("لطفاً تاریخ درآمد را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (SabteDaramadTextBox3.Text == "")
                {
                    MajMessageBox.show("لطفاً مبلغ درآمد را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _FinancialTbl.Title = SabteDaramadTextBox1.Text;
                _FinancialTbl.PersianDate = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                _FinancialTbl.Datee = Par._DateTimeVariable.Value;
                _FinancialTbl.Income = decimal.Parse(SabteDaramadTextBox3.Text);
                _FinancialTbl.Cost = 0;
                _FinancialTbl.Description = SabteDaramadTextBox4.Text;
                _FinancialTbl.Deposite = SabteDaramadCombo1.Text;
                _FamilyManaerDBEntities.FinancialTbls.Add(_FinancialTbl);
                _FamilyManaerDBEntities.SaveChanges();
                SabteDaramadTextBox1.Text = ""; SabteDaramadTextBox3.Text = ""; SabteDaramadTextBox4.Text = "";
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateDataGridForIncome();
            }
            catch (Exception error) { SaveError(error); }
        }
        private void SabteDaramadTextBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)// Numreric Only For All TextBlock فقط عدد
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public void CreateDataGridForIncome()
        {
            try
            {
                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          where p.Cost == 0
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    IncomeGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        عنوان = s.Title,
                        تاریخ = s.PersianDate,
                        مبلغ = s.Income,
                        سپرده = s.Deposite,
                        توضیحات = s.Description

                    }).ToList();
                    IncomeGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception error) { SaveError(error); }

        }
        private void SabteDaramadTBut1_Click(object sender, RoutedEventArgs e)//دکمه ویرایش درآمدها
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.Title = SabteDaramadTextBox1.Text;
                    ispresent.PersianDate = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                    ispresent.Datee = Par._DateTimeVariable.Value;
                    ispresent.Income = decimal.Parse(SabteDaramadTextBox3.Text);
                    ispresent.Description = SabteDaramadTextBox4.Text;
                    ispresent.Deposite = SabteDaramadCombo1.Text;
                    _FamilyManaerDBEntities.SaveChanges();
                    SabteDaramadTextBox1.Text = ""; SabteDaramadTextBox3.Text = ""; SabteDaramadTextBox4.Text = "";
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreateDataGridForIncome();

                    EmptyPar();
                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

            }
            catch (Exception error) { SaveError(error); }

        }
        private void IncomeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)IncomeGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        SabteDaramadTextBox1.Text = ispresent.Title;
                        SabteDaramadTextBox2.Text = ispresent.PersianDate;
                        SabteDaramadCombo1.Text = ispresent.Deposite;
                        SabteDaramadTextBox3.Text = ispresent.Income.ToString();
                        SabteDaramadTextBox4.Text = ispresent.Description;
                    }

                }
            }
            catch (Exception error) { SaveError(error); }

        }
        private void SabteIadAvarTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabteIadAvarTextBox4.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SabteIadAvarTextBox4.Text = string.Format("{0}", number);
                    SabteIadAvarTextBox4.SelectionStart = SabteIadAvarTextBox4.Text.Length;
                    if (int.Parse(SabteIadAvarTextBox4.Text) > 99999) { SabteIadAvarTextBox4.Text = "99999"; }
                }
            }
            catch (Exception error) { SaveError(error); }
        }        // در تغییر تکست باکس عدد باشد تا مقدار درخواست شده
        private void GozareshSabteIadAvarlabel2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GozareshSabteIadAvarCanvasTimeTable1.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable2.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable3.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable4.Children.Clear();
            GozareshSabteIadAvarCanvasTimeTable5.Children.Clear();
        }
        private void RepeatSabteIadAvarBut3_Click(object sender, RoutedEventArgs e)
        {

            var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == Par.IDIadAvar).FirstOrDefault();
            if (ispresent != null)
            {
                var result = MajMessageBox.show("آیا از تغییر فعالیت زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleActivity, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    var starttime = new DateTime(Par._DateTimeVariableStart.Value.Year, Par._DateTimeVariableStart.Value.Month, Par._DateTimeVariableStart.Value.Day, RepeatSabteIadAvarTimePicker1.Value.Value.Hour, RepeatSabteIadAvarTimePicker1.Value.Value.Minute, 0);
                    var finishtime = new DateTime(Par._DateTimeVariableStart.Value.Year, Par._DateTimeVariableStart.Value.Month, Par._DateTimeVariableStart.Value.Day, RepeatSabteIadAvarTimePicker2.Value.Value.Hour, RepeatSabteIadAvarTimePicker2.Value.Value.Minute, 0);
                    if (RepeatSabteIadAvarTimePicker1.Value.Value.Hour > RepeatSabteIadAvarTimePicker2.Value.Value.Hour)
                    {
                        finishtime = finishtime.AddDays(1);
                    }
                    if (RepeatSabteIadAvarPaneltoggleButton.IsChecked==true)
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, RepeatSabteIadAvarPaneltoggleButton.IsChecked.Value, int.Parse(RepeatSabteIadAvarTextBox4.Text), RepeatSabteIadAvarCombo2.Text, int.Parse(RepeatSabteIadAvarCombo1.Text)))
                        {
                            sabteIadAvar(false, "RepeatSabteIadAvarPanel", Par.IDIadAvar);

                        }
                    }
                    else
                    {
                        if (CheckDonotRepeatIadAvar(starttime, finishtime, RepeatSabteIadAvarPaneltoggleButton.IsChecked.Value, 1, "", 1))
                        {
                            sabteIadAvar(false, "RepeatSabteIadAvarPanel", Par.IDIadAvar);

                        }
                    }
                    //RepeatSabteIadAvarPanel.IsEnabled = false;
                    //RepeatSabteIadAvarPanel.Visibility = Visibility.Hidden;
                    //SabteIadAvarPanel.IsEnabled = true;
                    //SabteIadAvarPanel.Visibility = Visibility.Visible;
                }

            }
            else { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); }

        }        // دکمه ویرایش وقتی فعالیت ها تداخل دارد
        private void textgozareshPersianCalendarBut2_Click(object sender, RoutedEventArgs e) // دکمه شروع تاریخ گزارش
        {
            Par._DateTimeVariableStart = PerCalendar.Date.start();
            Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0') + "    " + Date.PDayOfWeek;
            textgozareshSabteIadAvarTextBox3.Text = Par.Tarikh;
        }
        private void textgozareshPersianCalendarBut_Click(object sender, RoutedEventArgs e) // دکمه پایان تاریخ گزارش
        {
            Par._DateTimeVariableFinish = PerCalendar.Date.start();
            Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0') + "    " + Date.PDayOfWeek;
            textgozareshSabteIadAvarTextBox2.Text = Par.Tarikh;
        }
        public void GozareshSabteIadavar()
        {
            try
            {
                var Fin = from p in _FamilyManaerDBEntities.IadAvarTbls
                          where (p.StartDateTime >= Par._DateTimeVariableStart && p.StartDateTime <= Par._DateTimeVariableFinish)
                          || (p.PeriodicEndTime >= Par._DateTimeVariableStart && p.PeriodicEndTime <= Par._DateTimeVariableFinish)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    GridGozareshSabteIadavar.ItemsSource = Fin.Select(s => new
                    {
                        عنوان = s.TitleActivity,
                        تاریخ = s.PersianStartDate,
                        شروع = s.PersianStartTime,
                        پایان = s.PersianEndTime.ToString(),
                        توضیحات = s.description,
                        دوره = " هر " + s.MeasurePeriodic + s.PeriodocKind + " یکبار در " + s.PeriodNumBer + " دوره "

                    }).ToList();
                }

            }


            catch (Exception error) { SaveError(error); }
        }
        private void textgozareshSabteIadAvarTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            GozareshSabteIadavar();
        }
        private void textgozareshSabteIadAvarTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            GozareshSabteIadavar();
        }
        private void textgozareshSabteIadAvarTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                var Fin = from p in _FamilyManaerDBEntities.IadAvarTbls
                       .Where(p => p.TitleActivity.Contains(textgozareshSabteIadAvarTextBox4.Text))

                          select p;

                if (Fin != null)
                {
                    GridGozareshSabteIadavar.ItemsSource = Fin.Select(s => new
                    {
                        عنوان = s.TitleActivity,
                        تاریخ = s.PersianStartDate,
                        ساعت = s.PersianStartTime,
                        مدت = s.PersianEndTime,
                        توضیحات = s.description,
                        دوره = " هر " + s.MeasurePeriodic + s.PeriodocKind + " یکبار در " + s.PeriodNumBer + " دوره "

                    }).ToList();
                }

            }


            catch (Exception error) { SaveError(error); }
        }
        private void MoshahedefaaliatIadAvarBut2_Click(object sender, RoutedEventArgs e) // پنل سمت جپ - گزارش فعالیت ها
        {

            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                textgozareshSabteIadAvarPanel.Visibility = Visibility.Visible;
                textgozareshSabteIadAvarPanel.IsEnabled = true;

            }
            catch (Exception error) { SaveError(error); }
        }

        private void RepeatSabteIadAvarBut4_Click(object sender, RoutedEventArgs e) // دکمه بی خیال در ثبت تکرار یادآوری
        {
            Panelinvisible();
            SabteIadAvarPanel.Visibility = Visibility.Visible;
            SabteIadAvarPanel.IsEnabled = true;
        }

        private void RepeatSabteIadAvarBut2_Click(object sender, RoutedEventArgs e) // حذف فعالیت های همزمانی 
        {
            if (Par.IDIadAvar == -1) { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            var ispresent = _FamilyManaerDBEntities.IadAvarTbls.Where(check => check.ID == Par.IDIadAvar).FirstOrDefault();
            if (ispresent != null)
            {
                var result = MajMessageBox.show("آیا از حذف فعالیت زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleActivity, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    _FamilyManaerDBEntities.IadAvarTbls.Remove(ispresent);
                    _FamilyManaerDBEntities.SaveChanges();
                    Par.IDIadAvar = -1;
                    CanvasTimeTable1.Children.Clear();
                    SpecifyTime("CanvasTimeTable1", Par._DateTimeVariable.Value);
                    MajMessageBox.show("اطلاعات با موفقیت پاک شد.", MajMessageBox.MajMessageBoxBut.OK);

                }

            }
            else { MajMessageBox.show("لطفاً فعالیت مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); }
            Panelinvisible();
            SabteIadAvarPanel.Visibility = Visibility.Visible;
            SabteIadAvarPanel.IsEnabled = true;
            SabteIadAvarBut1_Click(sender, e);
        }

        private void SabteSepordeDownBut1_Click(object sender, RoutedEventArgs e)// دکمه ثبت نام سپرده
        {
            _ComboBoxTbl.Deposit = SabteSepordeTextBox1.Text + " " + SabteSepordeTextBox2.Text;
            _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
            _FamilyManaerDBEntities.SaveChanges();
            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
            depositGrid();
            EmptyPar();
        }

        private void SabteSepordeDownBut3_Click(object sender, RoutedEventArgs e) // حذف نام سپرده
        {
            try
            {
                object item = (object)gridSabteSeporde.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً سپرده مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.ComboBoxTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridSabteSeporde.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف سپرده زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Deposit, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.ComboBoxTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                depositGrid();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteDaramadTBut3_Click(object sender, RoutedEventArgs e)// حذف ثبت درآمد
        {
            try
            {
                object item = (object)IncomeGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً درآمد مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                IncomeGrid.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف سپرده زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.FinancialTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                CreateDataGridForIncome();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteVamPanelTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {


            try
            {
                decimal number;
                if (decimal.TryParse(SabteVamPanelTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SabteVamPanelTextBox3.Text = string.Format("{0:N0}", number);
                    SabteVamPanelTextBox3.SelectionStart = SabteVamPanelTextBox3.Text.Length;
                }

            }
            catch (Exception error) { SaveError(error); }

        }

        private void SabteVamPanelTextBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SabteVamPanelTextBox5.Text != "")
            {
                if (int.Parse(SabteVamPanelTextBox5.Text) >= 9999)
                {
                    SabteVamPanelTextBox5.Text = "9999";
                }
            }
        }

        private void SabteVamPanelBut2_Click(object sender, RoutedEventArgs e) //دکمه ثبت وام
        {
            if ((SabteVamPanelTextBox1.Text == "") || (SabteVamPanelTextBox6.Text == "") || (SabteVamPanelTextBox3.Text == "") || (SabteVamPanelTextBox5.Text == ""))
            {
                MajMessageBox.show("لطفاً تمامی فیلدها را تکمیل نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                return;
            }
            _VAMTbl.Title = SabteVamPanelTextBox1.Text;
            _VAMTbl.PersianDate = SabteVamPanelTextBox6.Text;
            _VAMTbl.GDate = Par._DateTimeVariable.Value;
            _VAMTbl.ssum = decimal.Parse(SabteVamPanelTextBox3.Text);
            _VAMTbl.NumberGhest = int.Parse(SabteVamPanelTextBox5.Text);
            _VAMTbl.Deposite = SabteVamPanelCombo1.Text;
            _VAMTbl.description = SabteVamPanelTextBox4.Text;
            _FamilyManaerDBEntities.VAMTbls.Add(_VAMTbl);
            _FamilyManaerDBEntities.SaveChanges();
            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
            EmptyPar();
            CleanOldDataEnteredTXT();
            createSabteVamPanelGrid();



        }

        private void SabteVamPanelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)SabteVamPanelGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.VAMTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        SabteVamPanelTextBox1.Text = ispresent.Title;
                        SabteVamPanelTextBox6.Text = ispresent.PersianDate;
                        SabteVamPanelCombo1.Text = ispresent.Deposite;
                        SabteVamPanelTextBox3.Text = ispresent.ssum.ToString();
                        SabteVamPanelTextBox5.Text = ispresent.NumberGhest.ToString();
                        SabteVamPanelTextBox4.Text = ispresent.description.ToString();
                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteVamPanelTBut1_Click(object sender, RoutedEventArgs e) // دکمه ویرایش  وام
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.VAMTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.Title = SabteVamPanelTextBox1.Text;
                    ispresent.PersianDate = SabteVamPanelTextBox6.Text;
                    ispresent.GDate = Par._DateTimeVariable.Value;
                    ispresent.ssum = decimal.Parse(SabteVamPanelTextBox3.Text);
                    ispresent.NumberGhest = int.Parse(SabteVamPanelTextBox5.Text);
                    ispresent.description = SabteVamPanelTextBox4.Text;
                    ispresent.Deposite = SabteVamPanelCombo1.Text;
                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    createSabteVamPanelGrid();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteVamPanelBut3_Click(object sender, RoutedEventArgs e) // حذف ثبت وام
        {
            try
            {
                object item = (object)SabteVamPanelGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.VAMTbls.Where(check => check.ID == ID).FirstOrDefault();
                SabteVamPanelGrid.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف وام زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.VAMTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                createSabteVamPanelGrid();
                CleanOldDataEnteredTXT();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckBut2_Click(object sender, RoutedEventArgs e) // ثبت چک
        {
            try
            {
                if (ModiriatChckTextBox1.Text == "")
                {
                    MajMessageBox.show("لطفاً عنوان چک را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (ModiriatChckTextBox2.Text == "")
                {
                    MajMessageBox.show("لطفاً تاریخ سررسید چک را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (ModiriatChckTextBox3.Text == "")
                {
                    MajMessageBox.show("لطفاً مبلغ درآمد را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _FinancialTbl.Title = ModiriatChckTextBox1.Text;
                _FinancialTbl.PersianDate = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                _FinancialTbl.Datee = Par._DateTimeVariable.Value;
                _FinancialTbl.Cost = decimal.Parse(ModiriatChckTextBox3.Text);
                _FinancialTbl.Description = ModiriatChckTextBox4.Text;
                _FinancialTbl.Deposite = ModiriatChckCombo1.Text;
                _FinancialTbl.FinancialCategory = "check";
                _FinancialTbl.girandeh = ModiriatChckTextBox5.Text;
                if (toggleButtonModiriatChck.IsChecked == true)
                {
                    _FinancialTbl.Pas = true;
                }
                _FamilyManaerDBEntities.FinancialTbls.Add(_FinancialTbl);
                _FamilyManaerDBEntities.SaveChanges();
                SabteDaramadTextBox1.Text = ""; SabteDaramadTextBox3.Text = ""; SabteDaramadTextBox4.Text = "";
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                createSabtecheckGrid();
                CleanOldDataEnteredTXT();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // گرید چک
        {
            try
            {
                object item = (object)ModiriatChckGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        ModiriatChckTextBox1.Text = ispresent.Title;
                        _FinancialTbl.PersianDate = ispresent.PersianDate;
                        ModiriatChckTextBox3.Text = ispresent.Cost.ToString();
                        ModiriatChckTextBox4.Text = ispresent.Description;
                        ModiriatChckCombo1.Text = ispresent.Deposite;
                        ModiriatChckTextBox5.Text = ispresent.girandeh;
                        toggleButtonModiriatChck.IsChecked = ispresent.Pas;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckBut1_Click(object sender, RoutedEventArgs e) // ویرایش چک
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.Title = ModiriatChckTextBox1.Text;
                    ispresent.PersianDate = Par.Tarikh;
                    ispresent.Datee = Par._DateTimeVariable.Value;
                    ispresent.Cost = decimal.Parse(ModiriatChckTextBox3.Text);
                    ispresent.Description = ModiriatChckTextBox4.Text;
                    ispresent.Deposite = ModiriatChckCombo1.Text;
                    ispresent.girandeh = ModiriatChckTextBox5.Text;
                    if (toggleButtonModiriatChck.IsChecked == true)
                    {
                        ispresent.Pas = true;
                    }
                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    createSabtecheckGrid();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckBut3_Click(object sender, RoutedEventArgs e) /// حدف چک
        {
            try
            {
                object item = (object)ModiriatChckGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً درآمد مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                ModiriatChckGrid.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف چک زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.FinancialTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                createSabtecheckGrid();
                CleanOldDataEnteredTXT();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox3.Text = string.Format("{0:N0}", number);
                    ModiriatChckTextBox3.SelectionStart = ModiriatChckTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void KharjKardUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            KharjKardLeftToolbarProfileVisible();
        }
        private void KharjKardBut2_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabetKharjKardsadePanel.Visibility = Visibility.Visible; SabetKharjKardsadePanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      select p.Deposit;
            if (Fin != null)
            {
                SabetKharjKardsadeCombo1.ItemsSource = Fin.ToList();
            }
            KharjKardLeftToolbarProfileVisible();
            CreateSabteKharjkardSade();
        }

        private void KharjKardBut1_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteKalaTextBox1.IsEnabled = true;
            SabteKalaTextBox2.IsEnabled = false;
            SabteKalaTextBox3.IsEnabled = false;
            KharjKardLeftToolbarProfileVisible();
            SabteKalaPanel.Visibility = Visibility.Visible; SabteKalaPanel.IsEnabled = true;
            CreateSabteKalaTreeView();
        }

        private void SabteKalaTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) // انتخاب از نمودار درختی کالا
        {
            object item = null;
            try
            {
                //   CleanOldDataEnteredTXT();
                if (EntekhabMavadGhazaPanel.Visibility == Visibility.Visible)
                {
                    item = (object)EntekhabMavadGhazaTree.SelectedItem;
                }

                else if (SabetKharjKardKamelPanel.Visibility == Visibility.Visible)
                {
                    item = (object)SabetKharjKardKamelTreeView.SelectedItem;
                }
                else if (SabteMavadGhazaPanel.Visibility == Visibility.Visible)
                {
                    item = (object)SabteMavadGhazaTree.SelectedItem;
                }
                else if (MojodiPanel.Visibility == Visibility.Visible)
                {
                    item = (object)MojodiTreeView.SelectedItem;
                }

                else
                {
                    item = (object)SabteKalaTreeView.SelectedItem;
                }


                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf(":");
                    int foundS2 = name.IndexOf("Items.Count", foundS1);
                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 2);

                    var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == name).FirstOrDefault();
                    if (ispresent3 != null)
                    {
                        Par.TreeKala1 = SabteKalaTextBox1.Text = ispresent3.Header;
                        Par.TreeKala2 = SabteKalaTextBox2.Text = ispresent3.SubHeader;
                        Par.TreeKala3 = SabteKalaTextBox3.Text = MojodiTextBox1.Text = SabetKharjKardKamelTextBox1.Text = EntekhabMavadGhazaTextBox1.Text = SabteMavadGhazaTextBox2.Text = EntekhabMavadGhazaTextBox1.Text = ispresent3.SubSubHeader;
                        Par.string1 = SabteKalaCombo1.Text = SabetKharjKardKamelTextBox5.Text = SabteMavadGhazaTextBox4.Text = MojodiTextBox3.Text = ispresent3.Vahed;
                        Par.string2 = SabteKalaTextBox4.Text = (ispresent3.IekCallery * 100).ToString();
                        Par.string3 = SabteKalaTextBox5.Text = ispresent3.description;
                        MojodiTextBox3.Text = ispresent3.Vahed;
                    }


                    var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == name).FirstOrDefault();
                    if (ispresent2 != null)
                    {
                        Par.TreeKala1 = SabteKalaTextBox1.Text = ispresent2.Header;
                        Par.TreeKala2 = SabteKalaTextBox2.Text = MojodiTextBox1.Text = SabetKharjKardKamelTextBox1.Text = EntekhabMavadGhazaTextBox1.Text = SabteMavadGhazaTextBox2.Text = EntekhabMavadGhazaTextBox1.Text = ispresent2.SubHeader;
                        Par.string1 = SabteKalaCombo1.Text = MojodiTextBox2.Text = SabetKharjKardKamelTextBox5.Text = SabteMavadGhazaTextBox4.Text = MojodiTextBox3.Text = ispresent2.Vahed;
                        Par.string2 = SabteKalaTextBox4.Text = (ispresent2.IekCallery * 100).ToString();
                        Par.string3 = SabteKalaTextBox5.Text = ispresent2.description;
                        MojodiTextBox3.Text = ispresent2.Vahed;


                    }

                    var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == name).FirstOrDefault();
                    if (ispresent1 != null)
                    {
                        Par.TreeKala1 = SabteKalaTextBox1.Text = MojodiTextBox1.Text = SabetKharjKardKamelTextBox1.Text = EntekhabMavadGhazaTextBox1.Text = SabteMavadGhazaTextBox2.Text = EntekhabMavadGhazaTextBox1.Text = ispresent1.Header;
                        Par.string1 = SabteKalaCombo1.Text = MojodiTextBox2.Text = SabetKharjKardKamelTextBox5.Text = SabteMavadGhazaTextBox4.Text = MojodiTextBox3.Text = ispresent1.Vahed;
                        Par.string2 = SabteKalaTextBox4.Text = (ispresent1.IekCallery * 100).ToString();
                        Par.string3 = SabteKalaTextBox5.Text = ispresent1.description;
                        MojodiTextBox3.Text = ispresent1.Vahed;

                    }


                }
            }
            catch (Exception error) { SaveError(error); }
        }


        private void SabteKalaBut2_Click(object sender, RoutedEventArgs e) // ثبت نمودار درختی سطح یک 
        {
            if (SabteKalaTextBox1.IsEnabled == true)
            {
                if (SabteKalaTextBox1.Text == "")

                {
                    MajMessageBox.show("لطفاً اطلاعات مد نظر را وارد نمایید", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


                var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox1.Text).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox1.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox1.Text).FirstOrDefault();
                if ((ispresent1 != null) || (ispresent2 != null) || (ispresent3 != null))
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox1.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


            }
            if (SabteKalaTextBox2.IsEnabled == true)
            {
                if (SabteKalaTextBox2.Text == "")

                {
                    MajMessageBox.show("لطفاً اطلاعات مد نظر را وارد نمایید", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if (SabteKalaTextBox1.Text == "")
                {
                    MajMessageBox.show("لطفاً گروه کالایی را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox2.Text).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox2.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox2.Text).FirstOrDefault();
                if ((ispresent1 != null) || (ispresent2 != null) || (ispresent3 != null))
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox2.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }




            }
            if (SabteKalaTextBox3.IsEnabled == true)
            {
                if (SabteKalaTextBox3.Text == "")

                {
                    MajMessageBox.show("لطفاً اطلاعات مد نظر را وارد نمایید", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if ((SabteKalaTextBox2.Text == "") || (SabteKalaTextBox1.Text == ""))
                {
                    MajMessageBox.show("لطفاً گروه کالایی را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                var ispresent7 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox3.Text).FirstOrDefault();
                var ispresent8 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox3.Text).FirstOrDefault();
                var ispresent9 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox3.Text).FirstOrDefault();
                if ((ispresent7 != null) || (ispresent8 != null) || (ispresent9 != null))
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox3.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }



            }

            _TreeKala.Header = SabteKalaTextBox1.Text;
            _TreeKala.SubHeader = SabteKalaTextBox2.Text;
            _TreeKala.SubSubHeader = SabteKalaTextBox3.Text;
            _TreeKala.Vahed = SabteKalaCombo1.Text;
            if (!string.IsNullOrEmpty(SabteKalaTextBox4.Text))
            {
                _TreeKala.IekCallery = decimal.Parse(SabteKalaTextBox4.Text) / 100;

            }
            else
            {
                _TreeKala.IekCallery = 0;
            }
            _TreeKala.description = SabteKalaTextBox5.Text;
            _FamilyManaerDBEntities.TreeKalas.Add(_TreeKala);
            _FamilyManaerDBEntities.SaveChanges();
            CleanOldDataEnteredTXT();
            EmptyPar();
            CreateSabteKalaTreeView();
            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);

        }
        private void SabteKalaBut1_Click(object sender, RoutedEventArgs e) //ویرایش نمودار درختی
        {
            var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox1.Text).FirstOrDefault();
            var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox1.Text).FirstOrDefault();
            var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox1.Text).FirstOrDefault();

            var ispresent4 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox2.Text).FirstOrDefault();
            var ispresent5 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox2.Text).FirstOrDefault();
            var ispresent6 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox2.Text).FirstOrDefault();

            var ispresent7 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteKalaTextBox3.Text).FirstOrDefault();
            var ispresent8 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteKalaTextBox3.Text).FirstOrDefault();
            var ispresent9 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteKalaTextBox3.Text).FirstOrDefault();

            bool FaghatKamiat = false;
            if ((SabteKalaTextBox1.Text == Par.TreeKala1) && (SabteKalaTextBox2.Text == Par.TreeKala2) && (SabteKalaTextBox3.Text == Par.TreeKala3))
            {
                if ((Par.string1 != SabteKalaCombo1.Text) || (Par.string2 != SabteKalaTextBox4.Text) || (Par.string3 != SabteKalaTextBox5.Text))
                {
                    FaghatKamiat = true;
                }

            }

            if (SabteKalaTextBox1.IsEnabled == true)
            {
                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if (((ispresent1 != null) || (ispresent2 != null) || (ispresent3 != null)) && FaghatKamiat == false)
                {

                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox1.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var result22 = MajMessageBox.show("آیا از  تغییر اطمینان دارید؟", MajMessageBox.MajMessageBoxBut.YESNO);
                if (result22 == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    bool exist = true;
                    while (exist)
                    {
                        ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();
                        if ((ispresent == null) || (Par.TreeKala1 == SabteKalaTextBox1.Text))
                        {
                            exist = false;
                        }
                        ispresent.Header = SabteKalaTextBox1.Text;
                        ispresent.Vahed = SabteKalaCombo1.Text;
                        ispresent.IekCallery = decimal.Parse(SabteKalaTextBox4.Text) / 100;
                        ispresent.description = SabteKalaTextBox5.Text;
                        _FamilyManaerDBEntities.SaveChanges();

                    }

                }
            }


            if (SabteKalaTextBox2.IsEnabled == true)
            {
                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == Par.TreeKala2).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if (SabteKalaTextBox1.Text == "")
                {
                    MajMessageBox.show("لطفاً گروه کالایی را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


                if (((ispresent4 != null) || (ispresent5 != null) || (ispresent6 != null)) && FaghatKamiat == false)
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox2.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var result2 = MajMessageBox.show("آیا از  تغییر اطمینان دارید؟", MajMessageBox.MajMessageBoxBut.YESNO);
                if (result2 == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    bool exist = true;
                    while (exist)
                    {
                        ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == Par.TreeKala2).FirstOrDefault();
                        if ((ispresent == null) || (Par.TreeKala2 == SabteKalaTextBox2.Text))
                        {
                            exist = false;
                        }
                        ispresent.SubHeader = SabteKalaTextBox2.Text;
                        ispresent.Vahed = SabteKalaCombo1.Text;
                        ispresent.IekCallery = decimal.Parse(SabteKalaTextBox4.Text) / 100;
                        ispresent.description = SabteKalaTextBox5.Text;
                        _FamilyManaerDBEntities.SaveChanges();

                    }

                }




            }
            if (SabteKalaTextBox3.IsEnabled == true)
            {
                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == Par.TreeKala3).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if ((SabteKalaTextBox2.Text == "") || (SabteKalaTextBox1.Text == ""))
                {
                    MajMessageBox.show("لطفاً گروه کالایی را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


                if (((ispresent7 != null) || (ispresent8 != null) || (ispresent9 != null)) && FaghatKamiat == false)
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است." + Environment.NewLine + SabteKalaTextBox3.Text, MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var result3 = MajMessageBox.show("آیا از  تغییر اطمینان دارید؟", MajMessageBox.MajMessageBoxBut.YESNO);
                if (result3 == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == Par.TreeKala3).FirstOrDefault();
                    ispresent.SubSubHeader = SabteKalaTextBox3.Text;
                    ispresent.Vahed = SabteKalaCombo1.Text;
                    ispresent.IekCallery = decimal.Parse(SabteKalaTextBox4.Text) / 100;
                    ispresent.description = SabteKalaTextBox5.Text;
                    _FamilyManaerDBEntities.SaveChanges();

                }


            }
            CleanOldDataEnteredTXT();
            EmptyPar();
            CreateSabteKalaTreeView();
            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);

        }


        private void KharjKardBut5_Click(object sender, RoutedEventArgs e)// ثبت نمودار درختی سطح 2
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteKalaTextBox1.IsEnabled = false;
            SabteKalaTextBox2.IsEnabled = true;
            SabteKalaTextBox3.IsEnabled = false;
            KharjKardLeftToolbarProfileVisible();
            SabteKalaPanel.Visibility = Visibility.Visible; SabteKalaPanel.IsEnabled = true;
            CreateSabteKalaTreeView();
        }

        private void KharjKardBut6_Click(object sender, RoutedEventArgs e) //ثبت نمودار درختی سطح 3
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            SabteKalaTextBox1.IsEnabled = false;
            SabteKalaTextBox2.IsEnabled = false;
            SabteKalaTextBox3.IsEnabled = true;
            KharjKardLeftToolbarProfileVisible();
            SabteKalaPanel.Visibility = Visibility.Visible; SabteKalaPanel.IsEnabled = true;
            CreateSabteKalaTreeView();
        }

        private void SabteKalaBut3_Click(object sender, RoutedEventArgs e) // حذف نمودار درختی
        {
            //         try
            //         {
            if (SabteKalaTextBox1.IsEnabled == true)
            {
                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var result = MajMessageBox.show("آیا از  حذف کالای زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Header, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    bool exist = true;
                    while (exist)
                    {
                        ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();

                        if (ispresent == null)
                        {
                            exist = false;
                            break;
                        }
                        _FamilyManaerDBEntities.TreeKalas.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();


                    }
                }
            }

            if (SabteKalaTextBox2.IsEnabled == true)
            {

                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == Par.TreeKala2).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


                var result = MajMessageBox.show("آیا از  حذف کالای زیر اطمینان دارید؟" + Environment.NewLine + ispresent.SubHeader, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    bool exist = true;
                    while (exist)
                    {
                        ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();

                        if (ispresent == null)
                        {
                            exist = false;
                            break;
                        }
                        _FamilyManaerDBEntities.TreeKalas.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();


                    }
                }


            }


            if (SabteKalaTextBox3.IsEnabled == true)
            {

                var ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == Par.TreeKala3).FirstOrDefault();
                if (ispresent == null)
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var result = MajMessageBox.show("آیا از  حذف کالای زیر اطمینان دارید؟" + Environment.NewLine + ispresent.SubSubHeader, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                {
                    bool exist = true;
                    while (exist)
                    {
                        ispresent = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == Par.TreeKala1).FirstOrDefault();

                        if (ispresent == null)
                        {
                            exist = false;
                            break;

                        }
                        _FamilyManaerDBEntities.TreeKalas.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                        CleanOldDataEnteredTXT();


                    }
                }

            }
            CleanOldDataEnteredTXT();
            EmptyPar();
            CreateSabteKalaTreeView();
            MajMessageBox.show("اطلاعات با موفقیت حذف شد.", MajMessageBox.MajMessageBoxBut.OK);

            //     }




            //            catch (Exception error) { SaveError(error); }
            //
        }

        private void SabteKalaTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                float number;
                if (float.TryParse(SabteKalaTextBox4.Text, out number))
                {
                    if (number >= 9999)
                    {
                        SabteKalaTextBox4.Text = "9999";
                    }
                    //  SabteKalaTextBox4.Text = string.Format("{0:N0}", number);
                    //   SabteKalaTextBox4.SelectionStart = SabteKalaTextBox4.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }

        }

        private void SabetKharjKardsadeTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabetKharjKardsadeTextBox3.Text, out number))
                {
                    if (number >= 9999999999)
                    {
                        number = 9999999999;
                    }
                    SabetKharjKardsadeTextBox3.Text = string.Format("{0:N0}", number);
                    SabetKharjKardsadeTextBox3.SelectionStart = SabetKharjKardsadeTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeBut2_Click(object sender, RoutedEventArgs e) // ثبت ساده خرجکرد
        {
            try
            {
                if ((SabetKharjKardsadeTextBox1.Text == "") || (SabetKharjKardsadeTextBox2.Text == "") || (SabetKharjKardsadeTextBox3.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _FinancialTbl.EnterDate = DateTime.Now;
                _FinancialTbl.Title = SabetKharjKardsadeTextBox1.Text;
                _FinancialTbl.PersianDate = Par.Tarikh;
                _FinancialTbl.Datee = Par._DateTimeVariable.Value;
                _FinancialTbl.Cost = decimal.Parse(SabetKharjKardsadeTextBox3.Text);
                _FinancialTbl.Income = 0;
                _FinancialTbl.Description = SabetKharjKardsadeTextBox4.Text;
                _FinancialTbl.Deposite = SabetKharjKardsadeCombo1.Text;
                _FinancialTbl.FinancialCategory = "SimpleCost";

                _FamilyManaerDBEntities.FinancialTbls.Add(_FinancialTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateSabteKharjkardSade();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeBut1_Click(object sender, RoutedEventArgs e) // ویرایش ثبت خرجکرد ساده
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.EnterDate = DateTime.Now;
                    ispresent.Title = SabetKharjKardsadeTextBox1.Text;
                    ispresent.PersianDate = Par.Tarikh;
                    ispresent.Datee = Par._DateTimeVariable.Value;
                    ispresent.Cost = decimal.Parse(SabetKharjKardsadeTextBox3.Text);
                    ispresent.Description = SabetKharjKardsadeTextBox4.Text;
                    ispresent.Deposite = SabetKharjKardsadeCombo1.Text;
                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    CreateSabteKharjkardSade();

                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)SabetKharjKardsadeGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        SabetKharjKardsadeTextBox1.Text = ispresent.Title;
                        SabetKharjKardsadeTextBox2.Text = Par.Tarikh = ispresent.PersianDate;
                        Par._DateTimeVariable = ispresent.Datee.Value;
                        SabetKharjKardsadeTextBox3.Text = ispresent.Cost.ToString();
                        SabetKharjKardsadeTextBox4.Text = ispresent.Description;
                        SabetKharjKardsadeCombo1.Text = ispresent.Deposite;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeBut3_Click(object sender, RoutedEventArgs e) //حذف خرجکرد ساده
        {
            try
            {
                object item = (object)SabetKharjKardsadeGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                SabetKharjKardsadeGrid.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.FinancialTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateSabteKharjkardSade();


            }
            catch (Exception error) { SaveError(error); }
        }


        private void SabetKharjKardKamelBut2_Click(object sender, RoutedEventArgs e)//ثبت کامل خرجکرد
        {
            try
            {
                if ((SabetKharjKardKamelTextBox1.Text == "") || (SabetKharjKardKamelTextBox2.Text == "") || (SabetKharjKardKamelTextBox3.Text == "") || (SabetKharjKardKamelTextBox4.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
                if ((ispresent1 == null) && (ispresent2 == null) && (ispresent3 == null))
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را از نمودار درختی انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var ispresent4 = _FamilyManaerDBEntities.MojodiKalaTbls.FirstOrDefault(check => check.Onvan == SabetKharjKardKamelTextBox1.Text);
                if (ispresent4 != null)
                {
                    ispresent4.Meghdar = ispresent4.Meghdar + decimal.Parse(SabetKharjKardKamelTextBox4.Text);
                }
                else
                {
                    _MojodiKalaTbl.Meghdar = decimal.Parse(SabetKharjKardKamelTextBox4.Text);
                    _MojodiKalaTbl.Onvan = SabetKharjKardKamelTextBox1.Text;
                    _MojodiKalaTbl.Vahed = SabetKharjKardKamelTextBox5.Text;
                    _FamilyManaerDBEntities.MojodiKalaTbls.Add(_MojodiKalaTbl);
                }


                _FinancialTbl.EnterDate = DateTime.Now;
                _FinancialTbl.Title = SabetKharjKardKamelTextBox1.Text;
                _FinancialTbl.PersianDate = Par.Tarikh;
                _FinancialTbl.Datee = Par._DateTimeVariable.Value;
                _FinancialTbl.Cost = decimal.Parse(SabetKharjKardKamelTextBox3.Text);
                _FinancialTbl.Income = 0;
                _FinancialTbl.Description = SabetKharjKardKamelTextBox6.Text;
                _FinancialTbl.Deposite = SabetKharjKardKamelCombo1.Text;
                _FinancialTbl.FinancialCategory = "CompleteCost";
                _FinancialTbl.Meghdar = decimal.Parse(SabetKharjKardKamelTextBox4.Text);
                _FamilyManaerDBEntities.FinancialTbls.Add(_FinancialTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateSabteKharjkardKamel();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardKamelTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Par.TreeExpand = true;
            CreateSabteKalaTreeView();
            CreatMojodiGrid();
            CreateSabteKharjkardKamel();
            CreateSabteKalaTreeView();
            Par.TreeExpand = false;


        }

        private void KharjKardBut3_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            KharjKardLeftToolbarProfileVisible();
            SabetKharjKardKamelPanel.Visibility = Visibility.Visible; SabetKharjKardKamelPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.ComboBoxTbls
                      where p.Deposit != null
                      select p.Deposit;
            if (Fin != null)
            {
                SabetKharjKardKamelCombo1.ItemsSource = Fin.ToList();
            }
            CreateSabteKalaTreeView();
            CreateSabteKharjkardKamel();
        }

        private void SabetKharjKardKamelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)SabetKharjKardKamelGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        SabetKharjKardKamelTextBox1.Text = ispresent.Title;
                        SabetKharjKardKamelTextBox2.Text = Par.Tarikh = ispresent.PersianDate;
                        Par._DateTimeVariable = ispresent.Datee.Value;
                        SabetKharjKardKamelTextBox3.Text = ispresent.Cost.ToString();
                        SabetKharjKardKamelTextBox6.Text = ispresent.Description;
                        SabetKharjKardKamelCombo1.Text = ispresent.Deposite;
                        SabetKharjKardKamelTextBox4.Text = ispresent.Meghdar.ToString();

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        //private void SabetKharjKardKamelBut1_Click(object sender, RoutedEventArgs e) // ویرایش خرج کرد کامل
        //{
        //    try
        //    {
        //        if ((SabetKharjKardKamelTextBox1.Text == "") || (SabetKharjKardKamelTextBox2.Text == "") || (SabetKharjKardKamelTextBox3.Text == "") || (SabetKharjKardKamelTextBox4.Text == ""))
        //        {
        //            MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
        //            return;

        //        }
        //        var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
        //        var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
        //        var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabetKharjKardKamelTextBox1.Text).FirstOrDefault();
        //        if ((ispresent1 == null) && (ispresent2 == null) && (ispresent3 == null))
        //        {
        //            MajMessageBox.show("لطفاً کالای مد نظر خود را از نمودار درختی انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
        //            return;
        //        }
        //        var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
        //        if (ispresent != null)
        //        {
        //            ispresent.EnterDate = DateTime.Now;
        //            ispresent.Title = SabetKharjKardKamelTextBox1.Text;
        //            ispresent.PersianDate = Par.Tarikh;
        //            ispresent.Datee = Par._DateTimeVariable.Value;
        //            ispresent.Cost = decimal.Parse(SabetKharjKardKamelTextBox3.Text);
        //            ispresent.Description = SabetKharjKardKamelTextBox6.Text;
        //            ispresent.Deposite = SabetKharjKardKamelCombo1.Text;
        //            ispresent.Meghdar = long.Parse(SabetKharjKardKamelTextBox4.Text);
        //            _FamilyManaerDBEntities.SaveChanges();
        //            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
        //            CleanOldDataEnteredTXT();
        //            EmptyPar();
        //            CreateSabteKharjkardSade();
        //            SabetKharjKardKamelGrid.SelectedItem = null;


        //        }
        //        else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
        //    }
        //    catch (Exception error) { SaveError(error); }
        //}

        private void SabetKharjKardKamelBut3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal Mojodi = 0;
                object item = (object)SabetKharjKardKamelGrid.SelectedItem;

                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.MojodiKalaTbls.FirstOrDefault(_ => _.Onvan == ispresent.Title);
                if (ispresent != null)
                {

                    var result = MajMessageBox.show("آیا از حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        if (ispresent2 != null)
                        {
                            Mojodi = ispresent2.Meghdar.Value - ispresent.Meghdar.Value;
                            if (Mojodi < 0)
                            {
                                ispresent2.Meghdar = 0;
                            }
                            else
                            {
                                ispresent2.Meghdar = Mojodi;
                            }

                        }
                        _FamilyManaerDBEntities.FinancialTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }

                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateSabteKharjkardSade();
                SabetKharjKardKamelGrid.SelectedItem = null;

                MajMessageBox.show("اطلاعات با موفقیت حذف شد." + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.OK);

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardKamelTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabetKharjKardKamelTextBox4.Text, out number))
                {
                    if (number >= 999999)
                    {
                        number = 999999;
                    }
                    SabetKharjKardKamelTextBox4.Text = string.Format("{0:N0}", number);
                    SabetKharjKardKamelTextBox4.SelectionStart = SabetKharjKardKamelTextBox4.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardKamelTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabetKharjKardKamelTextBox3.Text, out number))
                {
                    if (number >= 999999999)
                    {
                        number = 999999999;
                    }
                    SabetKharjKardKamelTextBox3.Text = string.Format("{0:N0}", number);
                    SabetKharjKardKamelTextBox3.SelectionStart = SabetKharjKardKamelTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardKamelTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateSabteKharjkardKamel();
        }

        private void AshpaziUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
        }

        private void CoockRightToolbarBut1_Click(object sender, RoutedEventArgs e) //موجودی  - دکمه سمت راست
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            CoockRightToolbarVisible();
            MojodiPanel.Visibility = Visibility.Visible; MojodiPanel.IsEnabled = true;
            CreateSabteKalaTreeView();
            CreatMojodiGrid();
        }

        private void MojodiTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Par.TreeExpand = true;
            CreateSabteKalaTreeView();
            CreatMojodiGrid();

            Par.TreeExpand = false;
        }

        private void MojodiBut2_Click(object sender, RoutedEventArgs e)// اضافه نکودن موجودی
        {
            try
            {
                if ((MojodiTextBox1.Text == "") || (MojodiTextBox2.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == MojodiTextBox1.Text).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == MojodiTextBox1.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == MojodiTextBox1.Text).FirstOrDefault();
                if ((ispresent1 == null) && (ispresent2 == null) && (ispresent3 == null))
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را از نمودار درختی انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                var ispresent4 = _FamilyManaerDBEntities.MojodiKalaTbls.Where(check => check.Onvan == MojodiTextBox1.Text).FirstOrDefault();


                if (ispresent4 != null)
                {
                    ispresent4.Meghdar = decimal.Parse(MojodiTextBox2.Text) + ispresent4.Meghdar;
                    _FamilyManaerDBEntities.SaveChanges();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreatMojodiGrid();

                }
                else if (ispresent4 == null)
                {
                    _MojodiKalaTbl.Onvan = MojodiTextBox1.Text;
                    _MojodiKalaTbl.Meghdar = decimal.Parse(MojodiTextBox2.Text);
                    _MojodiKalaTbl.Vahed = MojodiTextBox3.Text;
                    _FamilyManaerDBEntities.MojodiKalaTbls.Add(_MojodiKalaTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreatMojodiGrid();
                }


            }
            catch (Exception error) { SaveError(error); }
        }

        private void MojodiBut1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((MojodiTextBox1.Text == "") || (MojodiTextBox2.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var ispresent1 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == MojodiTextBox1.Text).FirstOrDefault();
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == MojodiTextBox1.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == MojodiTextBox1.Text).FirstOrDefault();
                if ((ispresent1 == null) && (ispresent2 == null) && (ispresent3 == null))
                {
                    MajMessageBox.show("لطفاً کالای مد نظر خود را از نمودار درختی انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                var ispresent4 = _FamilyManaerDBEntities.MojodiKalaTbls.Where(check => check.Onvan == MojodiTextBox1.Text).FirstOrDefault();


                if (ispresent4 != null)
                {
                    ispresent4.Meghdar = -1 * (decimal.Parse(MojodiTextBox2.Text)) + ispresent4.Meghdar;
                    _FamilyManaerDBEntities.SaveChanges();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreatMojodiGrid();
                }
                else if (ispresent4 == null)
                {
                    _MojodiKalaTbl.Onvan = MojodiTextBox1.Text;
                    _MojodiKalaTbl.Meghdar = decimal.Parse(MojodiTextBox2.Text);
                    _MojodiKalaTbl.Vahed = MojodiTextBox3.Text;
                    _FamilyManaerDBEntities.MojodiKalaTbls.Add(_MojodiKalaTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreatMojodiGrid();
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaBut5_Click(object sender, RoutedEventArgs e) // انتخاب عکس
        {

            dlg.ShowDialog();
            ImageSource imgsource = new BitmapImage(new Uri(dlg.FileName));
            SabTeNameGhazaImae.Source = imgsource;


        }

        private void CoockRightToolbarBut5_Click(object sender, RoutedEventArgs e) // پنل سمت چپ : انتخاب غذا
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            SabTeNameGhazaPanel.Visibility = Visibility.Visible; SabTeNameGhazaPanel.IsEnabled = true;
            CreateGhaza();
        }

        private void SabTeNameGhazaBut2_Click(object sender, RoutedEventArgs e) // ثبت نام غذا
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == SabTeNameGhazaTextBox1.Text).FirstOrDefault();
                if (ispresent != null)
                {
                    MajMessageBox.show("این عنوان قبلاً انتخاب شده است.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                string richTextSabTeNameGhazaTextBox4 = new TextRange(SabTeNameGhazaTextBox4.Document.ContentStart, SabTeNameGhazaTextBox4.Document.ContentEnd).Text;

                if ((SabTeNameGhazaTextBox1.Text == "") || (SabTeNameGhazaTextBox3.Text == "") || (richTextSabTeNameGhazaTextBox4 == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _GhzaNameTbl.Name = SabTeNameGhazaTextBox1.Text;
                _GhzaNameTbl.Nafarat = int.Parse(SabTeNameGhazaTextBox3.Text);
                _GhzaNameTbl.description = richTextSabTeNameGhazaTextBox4;
                if (Par.ImagePath != "")
                {

                    var bitmap = SabTeNameGhazaImae.Source as BitmapSource;
                    var encoder = new PngBitmapEncoder(); // or one of the other encoders
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));

                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        _GhzaNameTbl.Aks = stream.ToArray();
                    }


                }
                Par.ImagePath = "";


                _FamilyManaerDBEntities.GhzaNameTbls.Add(_GhzaNameTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateGhaza();
                ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaImae_MouseDown(object sender, MouseButtonEventArgs e) // انتخاب عکس غذا
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
     "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
     "Portable Network Graphic (*.png)|*.png";
                if (dlg.ShowDialog() == true)
                {
                    ImageSource imgsource = new BitmapImage(new Uri(dlg.FileName));
                    SabTeNameGhazaImae.Source = imgsource;
                    Par.ImagePath = dlg.FileName;
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) //انتخاب کرید غذا
        {
            try
            {
                object item = (object)SabTeNameGhazaGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        string x = ispresent.description;

                        SabTeNameGhazaTextBox1.Text = ispresent.Name;
                        SabTeNameGhazaTextBox3.Text = ispresent.Nafarat.ToString();

                        SabTeNameGhazaTextBox4.AppendText(ispresent.description);

                        if (ispresent.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                SabTeNameGhazaImae.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            SabTeNameGhazaImae.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }


                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaBut1_Click(object sender, RoutedEventArgs e) // ویرایش غذا
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    string richTextSabTeNameGhazaTextBox4 = new TextRange(SabTeNameGhazaTextBox4.Document.ContentStart, SabTeNameGhazaTextBox4.Document.ContentEnd).Text;

                    ispresent.Name = SabTeNameGhazaTextBox1.Text;
                    ispresent.Nafarat = int.Parse(SabTeNameGhazaTextBox3.Text);
                    ispresent.description = richTextSabTeNameGhazaTextBox4;
                    if (Par.ImagePath != "")
                    {

                        var bitmap = SabTeNameGhazaImae.Source as BitmapSource;
                        var encoder = new PngBitmapEncoder(); // or one of the other encoders
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            ispresent.Aks = stream.ToArray();
                        }


                    }
                    Par.ImagePath = "";

                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    CreateGhaza();
                    ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaBut3_Click(object sender, RoutedEventArgs e) //حذف عنوان غذا
        {
            try
            {


                object item = (object)SabTeNameGhazaGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Name, MajMessageBox.MajMessageBoxBut.YESNO);
                        if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                        {
                            _FamilyManaerDBEntities.GhzaNameTbls.Remove(ispresent);
                            _FamilyManaerDBEntities.SaveChanges();
                        }
                    }
                    EmptyPar();
                    CleanOldDataEnteredTXT();
                    CreateGhaza();

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabTeNameGhazaTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateGhaza3();
            CreateMavad();
        }

        private void CoockRightToolbarBut6_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            CoockRightToolbarVisible();
            SabteMavadGhazaPanel.Visibility = Visibility.Visible; SabteMavadGhazaPanel.IsEnabled = true;
            CreateGhaza2();
            CreateSabteKalaTreeView();
        }

        private void SabteMavadGhazaGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e) // انتخاب غذا برای انتخاب مواد اولیه
        {
            try
            {
                object item = (object)SabteMavadGhazaGrid1.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.MavadGhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    var ispresent2 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == ispresent.NameGhaza).FirstOrDefault();
                    if (ispresent != null)
                    {

                        SabteMavadGhazaTextBox2.Text = ispresent.NameMavad;
                        SabteMavadGhazaTextBox3.Text = ((ispresent.Meghdar) * (ispresent2.Nafarat)).ToString();



                    }

                }
            }
            catch (Exception error) { SaveError(error); }

        }

        private void SabteMavadGhazaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)SabteMavadGhazaGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        SabteMavadGhazaTextBox1.Text = ispresent.Name;
                        SabteMavadGhazaTextBox5.Text = "برای " + ispresent.Nafarat.ToString() + " نفر";



                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteMavadGhazaTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateGhaza2();
            CreateMavadGhaza();
        }



        private void SabteMavadGhazaTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Par.TreeExpand = true;
            CreateSabteKalaTreeView();
        }

        private void SabteMavadGhazaBut2_Click(object sender, RoutedEventArgs e) // ثبت مواد غذایی
        {
            try
            {
                if ((SabteMavadGhazaTextBox1.Text == "") || (SabteMavadGhazaTextBox2.Text == "") || (SabteMavadGhazaTextBox3.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var ispresent1 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == SabteMavadGhazaTextBox1.Text).FirstOrDefault();
                if (ispresent1 == null)
                {
                    MajMessageBox.show("این عنوان غذایی وجود ندارد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var Fin = from n in _FamilyManaerDBEntities.MavadGhzaNameTbls
                          where n.NameGhaza == SabteMavadGhazaTextBox1.Text
                          select n;
                foreach (var item in Fin)
                {
                    if (item.NameMavad == SabteMavadGhazaTextBox2.Text)
                    {
                        MajMessageBox.show("این عنوان ماده اولیه برای غذای انتخابی قبلاً انتخاب شده است", MajMessageBox.MajMessageBoxBut.OK);
                        return;
                    }
                }
                var ispresent2 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.Header == SabteMavadGhazaTextBox2.Text).FirstOrDefault();
                var ispresent3 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubHeader == SabteMavadGhazaTextBox2.Text).FirstOrDefault();
                var ispresent4 = _FamilyManaerDBEntities.TreeKalas.Where(check => check.SubSubHeader == SabteMavadGhazaTextBox2.Text).FirstOrDefault();

                if ((ispresent2 == null) && (ispresent3 == null) && (ispresent4 == null))
                {
                    MajMessageBox.show("این عنوان مواد اولیه وجود ندارد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                _MavadGhzaNameTbl.NameGhaza = SabteMavadGhazaTextBox1.Text;
                _MavadGhzaNameTbl.NameMavad = SabteMavadGhazaTextBox2.Text;

                if (ispresent2 != null)
                {
                    _MavadGhzaNameTbl.Vahed = ispresent2.Vahed;

                }
                else if (ispresent3 != null)
                {
                    _MavadGhzaNameTbl.Vahed = ispresent3.Vahed;

                }
                else if (ispresent4 != null)
                {
                    _MavadGhzaNameTbl.Vahed = ispresent4.Vahed;

                }
                _MavadGhzaNameTbl.Meghdar = (int.Parse(SabteMavadGhazaTextBox3.Text) / ispresent1.Nafarat);


                var ispresent5 = _FamilyManaerDBEntities.MojodiKalaTbls.FirstOrDefault(check => check.Onvan == SabteMavadGhazaTextBox2.Text);
                if (ispresent5 == null)
                {
                    _MojodiKalaTbl.Vahed = SabteMavadGhazaTextBox4.Text;
                    _MojodiKalaTbl.Onvan = SabteMavadGhazaTextBox2.Text;
                    _MojodiKalaTbl.Meghdar = 0;
                    _FamilyManaerDBEntities.MojodiKalaTbls.Add(_MojodiKalaTbl);
                }

                _FamilyManaerDBEntities.MavadGhzaNameTbls.Add(_MavadGhzaNameTbl);
                _FamilyManaerDBEntities.SaveChanges();
                //  CleanOldDataEnteredTXT();
                EmptyPar();
                //  CreateSabteKalaTreeView();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateMavadGhaza();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteMavadGhazaBut3_Click(object sender, RoutedEventArgs e) // حذف مواد غذایی
        {
            try
            {


                object item = (object)SabteMavadGhazaGrid1.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.MavadGhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.NameMavad, MajMessageBox.MajMessageBoxBut.YESNO);
                        if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                        {
                            _FamilyManaerDBEntities.MavadGhzaNameTbls.Remove(ispresent);
                            _FamilyManaerDBEntities.SaveChanges();
                        }
                    }
                    EmptyPar();
                    CleanOldDataEnteredTXT();
                    CreateGhaza();

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void CoockRightToolbarBut2_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            EntekhabNameGhazaPanel.Visibility = Visibility.Visible; EntekhabNameGhazaPanel.IsEnabled = true;
            CreateGhaza3();
        }

        private void EntekhabNameGhazaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)EntekhabNameGhazaGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {

                        EntekhabNameGhazaTextBox1.Text = ispresent.Name;
                        EntekhabNameGhazaTextBox4.Text = ispresent.description;

                        if (ispresent.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                EntekhabNameGhazaImae.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            EntekhabNameGhazaImae.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }


                    }

                }
            }
            catch (Exception error) { SaveError(error); }

        }

        private void CoockRightToolbarBut7_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            EntekhabMavadGhazaPanel.Visibility = Visibility.Visible; EntekhabMavadGhazaPanel.IsEnabled = true;
            CreateSabteKalaTreeView();
            EntekhabMavadGhaza();
        }

        private void EntekhabMavadGhazaTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntekhabMavadGhaza();
        }

        private void EntekhabMavadGhazaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)EntekhabMavadGhazaGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.MavadGhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    var ispresent2 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == ispresent.NameGhaza).FirstOrDefault();
                    if (ispresent != null)
                    {


                        EntekhabMavadGhazaTextBox4.Text = ispresent2.description;

                        if (ispresent2.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent2.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                EntekhabMavadGhazaImae.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            EntekhabMavadGhazaImae.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }

                        EntekhabMavadGhazaGrid.SelectedItem = false;
                    }

                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void CoockRightToolbarBut3_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            EntekhabCalleryGhazaPanel.Visibility = Visibility.Visible; EntekhabCalleryGhazaPanel.IsEnabled = true;
            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));
            ntekhabCalleryGhazaImae.Source = imgsource;
            ntekhabCalleryGhazaGrid.ItemsSource = null;

        }

        private void ntekhabCalleryGhazaGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)ntekhabCalleryGhazaGrid1.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    //   var ispresent2 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == ispresent.NameGhaza).FirstOrDefault();
                    if (ispresent != null)
                    {


                        ntekhabCalleryGhazaTextBox4.Text = ispresent.description;

                        if (ispresent.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                ntekhabCalleryGhazaImae.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            ntekhabCalleryGhazaImae.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }
                        CreateMavad2(ispresent.Name);
                        ntekhabCalleryGhazaGrid1.SelectedItem = false;
                    }

                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void CoockRightToolbarBut4_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            EntekhabGheimatGhazaPanel.Visibility = Visibility.Visible; EntekhabGheimatGhazaPanel.IsEnabled = true;
            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));
            EntekhabGheimatGhazaImae.Source = imgsource;
            EntekhabGheimatGhazaGrid.ItemsSource = null;
        }

        private void EntekhabGheimatGhazaTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateGhimatGhazaGrid();
        }

        private void EntekhabGheimatGhazaGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)EntekhabGheimatGhazaGrid1.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    //   var ispresent2 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == ispresent.NameGhaza).FirstOrDefault();
                    if (ispresent != null)
                    {


                        EntekhabGheimatGhazaTextBox4.Text = ispresent.description;

                        if (ispresent.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                EntekhabGheimatGhazaImae.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            EntekhabGheimatGhazaImae.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }
                        CreateGheimatGhaza(ispresent.Name);
                        EntekhabGheimatGhazaGrid1.SelectedItem = false;
                    }

                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void CoockRightToolbarBut8_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            EntekhabMojodiGhazaPanel.Visibility = Visibility.Visible; EntekhabMojodiGhazaPanel.IsEnabled = true;
            EntekhabMojodiGhaza();
        }

        private void EntekhabMojodiGhazaGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)EntekhabMojodiGhazaGrid1.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    //   var ispresent2 = _FamilyManaerDBEntities.GhzaNameTbls.Where(check => check.Name == ispresent.NameGhaza).FirstOrDefault();
                    if (ispresent != null)
                    {


                        EntekhabMojodiGhazaTextBox2.Text = ispresent.description;

                        if (ispresent.Aks != null)
                        {
                            using (var ms = new System.IO.MemoryStream(ispresent.Aks))
                            {
                                var image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                                EntekhabMojodiGhazaImge.Source = image;
                            }
                        }
                        else
                        {
                            ImageSource imgsource = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\pic\EmptyImage.png"));

                            EntekhabMojodiGhazaImge.Source = imgsource;
                            Par.ImagePath = dlg.FileName;
                        }
                        CreateMojodiGhazaGrid(ispresent.Name);
                        EntekhabMojodiGhazaGrid1.SelectedItem = false;
                    }

                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiratSakhtemanUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
        }

        private void ModiriatSakhtemanRightToolbarBut1_Click(object sender, RoutedEventArgs e) // دکمه سمت راست معرفی همسایه ها
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            ModiriatSakhtemanRightToolbarVisibile();
            SabteHamsaiePanel.Visibility = Visibility.Visible; SabteHamsaiePanel.IsEnabled = true;
            CreateHamsaieGrid();
            SabteHamsaieTextBox1.IsEnabled = true;

        }

        private void SabteHamsaieBut2_Click(object sender, RoutedEventArgs e) // ثبت همسایه ها
        {
            try
            {
                if ((SabteHamsaieTextBox1.Text == "") || (SabteHamsaieTextBox2.Text == "") || (SabteHamsaieTextBox3.Text == "") || (SabteffHamsaieTextBox4.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var existname = _FamilyManaerDBEntities.HamsaieTbls.FirstOrDefault(_ => _.NameVahed == SabteHamsaieTextBox1.Text);
                if (existname != null)
                {
                    MajMessageBox.show("این عنوان قبلاً انخاب شده است.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var IDVahesd = from _ in _FamilyManaerDBEntities.HamsaieTbls
                               where (string.IsNullOrEmpty(_.FinishPersianDate) && _.NameVahed == SabteHamsaieTextBox1.Text)
                               select _;

                foreach (var item in IDVahesd)
                {
                    MajMessageBox.show("این عنوان قبلاً انخاب شده است.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _HamsaieTbl.NameVahed = SabteHamsaieTextBox1.Text;
                _HamsaieTbl.TedadNafarat = int.Parse(SabteHamsaieTextBox2.Text);
                _HamsaieTbl.Metrazh = int.Parse(SabteHamsaieTextBox3.Text);
                _HamsaieTbl.Tasfieh = toggleButddton.IsChecked;
                _HamsaieTbl.StartGdate = Par._DateTimeVariableStart;
                _HamsaieTbl.PhineNumber = SabteffHamsaieTextBox4.Text;
                _HamsaieTbl.startPersianDate = SabteHamsaieTextBox22.Text.ToString();
                if (!string.IsNullOrEmpty(SabteHamsaieTextBox522.Text))
                {
                    _HamsaieTbl.FinishGdate = Par._DateTimeVariableFinish;
                    _HamsaieTbl.FinishPersianDate = SabteHamsaieTextBox522.Text.ToString();
                }
                else
                {
                    _HamsaieTbl.FinishGdate = null;
                    _HamsaieTbl.FinishPersianDate = string.Empty;
                }

                _HamsaieTbl.Description = SabteHamsaieTextBox4.Text;
                _HamsaieTbl.NameVahed = SabteHamsaieTextBox1.Text;

                if (string.IsNullOrEmpty(SabteHamsaieTextBox522.Text) && toggleButddton.IsChecked == true)
                {
                    MajMessageBox.show("تصفیه زمانی انجام می شود که واحد مد نظر خارج شده باشد..", MajMessageBox.MajMessageBoxBut.OK);
                    toggleButddton.IsChecked = false;
                    return;
                }

                _FamilyManaerDBEntities.HamsaieTbls.Add(_HamsaieTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateHamsaieGrid();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteHamsaieBut1_Click(object sender, RoutedEventArgs e)// ویرایش همسایه ها
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.HamsaieTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                var ExistName = _FamilyManaerDBEntities.HamsaieTbls.FirstOrDefault(check => check.NameVahed == SabteHamsaieTextBox1.Text);
                if (ispresent != null)
                {
                    if ((Par.NameVahed != SabteHamsaieTextBox1.Text) && (ExistName != null))
                    {
                        MajMessageBox.show("نام واحد قبلاً انتخاب شده است", MajMessageBox.MajMessageBoxBut.OK);
                        return;
                    }
                    ispresent.TedadNafarat = int.Parse(SabteHamsaieTextBox2.Text);
                    ispresent.Metrazh = int.Parse(SabteHamsaieTextBox3.Text);
                    ispresent.Description = SabteHamsaieTextBox4.Text;
                    ispresent.StartGdate = Par._DateTimeVariableStart;
                    ispresent.startPersianDate = SabteHamsaieTextBox22.Text.ToString();
                    ispresent.FinishGdate = Par._DateTimeVariableFinish;
                    ispresent.FinishPersianDate = SabteHamsaieTextBox522.Text.ToString();
                    ispresent.Tasfieh = toggleButddton.IsChecked; ;
                    ispresent.PhineNumber = SabteffHamsaieTextBox4.Text;

                    if (string.IsNullOrEmpty(SabteHamsaieTextBox522.Text) && toggleButddton.IsChecked == true)
                    {
                        MajMessageBox.show("تصفیه زمانی انجام می شود که واحد مد نظر خارج شده باشد..", MajMessageBox.MajMessageBoxBut.OK);
                        toggleButddton.IsChecked = false;
                        return;
                    }

                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    CreateHamsaieGrid();
                    SabteHamsaieTextBox1.IsEnabled = true;

                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteHamsaieGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)// انتخاب از گرید همسایه ها
        {
            try
            {
                SabteHamsaieTextBox1.IsEnabled = false;
                object item = (object)SabteHamsaieGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.HamsaieTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        Par._DateTimeVariableStart = ispresent.StartGdate.Value;
                        SabteHamsaieTextBox22.Text = ispresent.startPersianDate;
                        if (!string.IsNullOrEmpty(ispresent.FinishPersianDate))
                        {
                            Par._DateTimeVariableFinish = ispresent.FinishGdate.Value;

                        }
                        SabteHamsaieTextBox522.Text = ispresent.FinishPersianDate;
                        Par.NameVahed = SabteHamsaieTextBox1.Text = ispresent.NameVahed;
                        SabteHamsaieTextBox2.Text = ispresent.TedadNafarat.ToString();
                        SabteHamsaieTextBox3.Text = ispresent.Metrazh.ToString();
                        SabteHamsaieTextBox4.Text = ispresent.Description;
                        SabteffHamsaieTextBox4.Text = ispresent.PhineNumber;
                        toggleButddton.IsChecked = ispresent.Tasfieh;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteHamsaieBut3_Click(object sender, RoutedEventArgs e) // حذف از جدول همسایه
        {
            try
            {
                SabteHamsaieTextBox1.IsEnabled = true;
                object item = (object)SabteHamsaieGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.HamsaieTbls.Where(check => check.ID == ID).FirstOrDefault();
                SabetKharjKardsadeGrid.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.NameVahed, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.HamsaieTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateHamsaieGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatSakhtemanRightToolbarBut5_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : ثبت هزینه ساختمان
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            ModiriatSakhtemanRightToolbarVisibile();
            SabetKharjKardsadeSakhtemanPanel.Visibility = Visibility.Visible; SabetKharjKardsadeSakhtemanPanel.IsEnabled = true;
            CreateHazinehSakhtemanGrid();
            var Fin2 = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                       where _.NoeHazinehIaDaramadSakhteman == "هزینه"
                       select _.OnvanHazinehIaDaramadSakhteman;
            SabetKharjKardsadeSakhtemanTextBox1.ItemsSource = Fin2.ToList();
            SabetKharjKardsadeSakhtemanTextBox1.SelectedIndex = 0;
        }

        private void SabetKharjKardsadeSakhtemanBut2_Click(object sender, RoutedEventArgs e) // ثبت هزینه ساختمان
        {
            try
            {
                if ((SabetKharjKardsadeSakhtemanTextBox1.Text == "") || (SabetKharjKardsadeSakhtemanTextBox2.Text == "") || (SabetKharjKardsadeSakhtemanTextBox3.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (Par._DateTimeVariableFinish < Par._DateTimeVariableStart)
                {
                    MajMessageBox.show("تاریخ شروع نمی تواند بزرگتر از تاریخ پایان باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }


                _SabteHazinehSakhtemanTbl.TitleCost = SabetKharjKardsadeSakhtemanTextBox1.Text;
                _SabteHazinehSakhtemanTbl.PersianDate = Par.Tarikh;
                _SabteHazinehSakhtemanTbl.Year = Par.Year;
                _SabteHazinehSakhtemanTbl.GDate = Par._DateTimeVariable.Value;
                _SabteHazinehSakhtemanTbl.Cost = decimal.Parse(SabetKharjKardsadeSakhtemanTextBox3.Text);
                _SabteHazinehSakhtemanTbl.mmonth = SabetKharjKardsadeSakhtemanCombo1.Text;
                _SabteHazinehSakhtemanTbl.ShiveTaghsim = SabetKharjKardsadeSakhtemanCombo2.Text;
                _SabteHazinehSakhtemanTbl.Description = SabetKharjKardsadeSakhtemanTextBox4.Text;
                _SabteHazinehSakhtemanTbl.Enteghali = false;

                _SabteHazinehSakhtemanTbl.VahedName = null;
                _SabteHazinehSakhtemanTbl.Income = 0;
                if (!string.IsNullOrEmpty(SbteKharjkardVahedTextBلox8.Text))
                {
                    _SabteHazinehSakhtemanTbl.StartGdat = Par._DateTimeVariableStart;
                    _SabteHazinehSakhtemanTbl.startPersianDate = SbteKharjkardVahedTextBلox8.Text;
                }
                if (!string.IsNullOrEmpty(SbteKharjkardVahedTextBoلx6.Text))
                {

                    _SabteHazinehSakhtemanTbl.FinishGdate = Par._DateTimeVariableFinish;
                    _SabteHazinehSakhtemanTbl.FinishPersianDate = SbteKharjkardVahedTextBoلx6.Text;
                }

                _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateHazinehSakhtemanGrid();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeSakhtemanGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // انتخاب از  جدول هزینه ساختمان
        {
            try
            {
                object item = (object)SabetKharjKardsadeSakhtemanGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        SabetKharjKardsadeSakhtemanTextBox1.Text = ispresent.TitleCost;
                        SabetKharjKardsadeSakhtemanTextBox2.Text = Par.Tarikh = ispresent.PersianDate;
                        Par.Year = ispresent.Year;
                        Par._DateTimeVariable = ispresent.GDate.Value;
                        SabetKharjKardsadeSakhtemanTextBox3.Text = ispresent.Cost.ToString();
                        SabetKharjKardsadeSakhtemanCombo1.Text = ispresent.mmonth;
                        SabetKharjKardsadeSakhtemanCombo2.Text = ispresent.ShiveTaghsim;
                        SabetKharjKardsadeTextBox4.Text = ispresent.Description;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeSakhtemanBut1_Click(object sender, RoutedEventArgs e) // ویرایش هزینه ساختمان
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.TitleCost = SabetKharjKardsadeSakhtemanTextBox1.Text;
                    ispresent.PersianDate = Par.Tarikh;
                    ispresent.Year = Par.Year;
                    ispresent.GDate = Par._DateTimeVariable.Value;
                    ispresent.Cost = decimal.Parse(SabetKharjKardsadeSakhtemanTextBox3.Text);
                    ispresent.mmonth = SabetKharjKardsadeSakhtemanCombo1.Text;
                    ispresent.ShiveTaghsim = SabetKharjKardsadeSakhtemanCombo2.Text;
                    ispresent.Description = SabetKharjKardsadeTextBox4.Text;
                    ispresent.VahedName = null;

                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    CreateHazinehSakhtemanGrid();

                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeSakhtemanBut3_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleCost, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateHazinehSakhtemanGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabetKharjKardsadeSakhtemanTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(SabetKharjKardsadeSakhtemanTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SabetKharjKardsadeSakhtemanTextBox3.Text = string.Format("{0:N0}", number);
                    SabetKharjKardsadeSakhtemanTextBox3.SelectionStart = SabetKharjKardsadeSakhtemanTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatSakhtemanRightToolbarBut6_Click(object sender, RoutedEventArgs e) // دکمه سمت راست ثبت هزینه واحد
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            var Fin = from p in _FamilyManaerDBEntities.HamsaieTbls
                      where p.Tasfieh == false
                      select p.NameVahed;
            if (Fin != null)
            {
                SbteKharjkardVahedCombo2.ItemsSource = Fin.ToList();
            }
            ModiriatSakhtemanRightToolbarVisibile();
            SbteKharjkardVahedPanel.Visibility = Visibility.Visible; SbteKharjkardVahedPanel.IsEnabled = true;
            var Fin2 = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                       where _.NoeHazinehIaDaramadSakhteman == "هزینه"
                       select _.OnvanHazinehIaDaramadSakhteman;
            SbteKharjkardVahedTextBox1.ItemsSource = Fin2.ToList();
            SbteKharjkardVahedTextBox1.SelectedIndex = 0;
        }

        private void SbteKharjkardVahedBut2_Click(object sender, RoutedEventArgs e) // ثبت هزینه واحد
        {
            try
            {
                if ((SbteKharjkardVahedTextBox1.Text == "") || (SbteKharjkardVahedTextBox2.Text == "") || (SbteKharjkardVahedTextBox3.Text == "") || (SbteKharjkardVahedCombo1.Text == "") || (SbteKharjkardVahedCombo2.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var IDVahesd = from _ in _FamilyManaerDBEntities.HamsaieTbls
                               where (string.IsNullOrEmpty(_.FinishPersianDate) && _.NameVahed == SbteKharjkardVahedCombo2.Text)
                               select _;

                foreach (var item in IDVahesd)
                {


                    _SabteHazinehSakhtemanTbl.TitleCost = SbteKharjkardVahedTextBox1.Text;
                    _SabteHazinehSakhtemanTbl.PersianDate = Par.Tarikh;
                    _SabteHazinehSakhtemanTbl.Income = 0;
                    _SabteHazinehSakhtemanTbl.GDate = Par._DateTimeVariable.Value;
                    _SabteHazinehSakhtemanTbl.Cost = decimal.Parse(SbteKharjkardVahedTextBox3.Text);
                    _SabteHazinehSakhtemanTbl.mmonth = SbteKharjkardVahedCombo1.Text;
                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(item.ID.ToString());
                    _SabteHazinehSakhtemanTbl.VahedName = SbteKharjkardVahedCombo2.Text;
                    _SabteHazinehSakhtemanTbl.Description = SbteKharjkardVahedTextBox4.Text;
                    _SabteHazinehSakhtemanTbl.Year = Par.Year;
                    _SabteHazinehSakhtemanTbl.Enteghali = false;

                    _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CreateHazinehVahedGrid();
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedBut1_Click(object sender, RoutedEventArgs e) //ویرایش هزینه واحد
        {
            try
            {
                var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    ispresent.TitleCost = SbteKharjkardVahedTextBox1.Text;
                    ispresent.PersianDate = Par.Tarikh;
                    ispresent.Year = Par.Year;
                    ispresent.GDate = Par._DateTimeVariable.Value;
                    ispresent.Cost = decimal.Parse(SbteKharjkardVahedTextBox3.Text);
                    ispresent.mmonth = SbteKharjkardVahedCombo1.Text;
                    ispresent.VahedName = SbteKharjkardVahedCombo2.Text;
                    ispresent.Description = SbteKharjkardVahedTextBox4.Text;

                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    CreateHazinehVahedGrid();
                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedBut3_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleCost, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateHazinehVahedGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(SbteKharjkardVahedTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SbteKharjkardVahedTextBox3.Text = string.Format("{0:N0}", number);
                    SbteKharjkardVahedTextBox3.SelectionStart = SbteKharjkardVahedTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) //انتخاب هزینه واحد
        {
            try
            {
                object item = (object)SbteKharjkardVahedGrid.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        SbteKharjkardVahedTextBox1.Text = ispresent.TitleCost;
                        SbteKharjkardVahedTextBox2.Text = Par.Tarikh = ispresent.PersianDate;
                        Par.Year = ispresent.Year;
                        Par._DateTimeVariable = ispresent.GDate.Value;
                        SbteKharjkardVahedTextBox3.Text = ispresent.Cost.ToString();
                        SbteKharjkardVahedCombo1.Text = ispresent.mmonth;
                        SbteKharjkardVahedCombo2.Text = ispresent.VahedName;
                        SbteKharjkardVahedTextBox4.Text = ispresent.Description;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatSakhtemanRightToolbarBut2_Click(object sender, RoutedEventArgs e)// دکمه سمت راست : محاسبه هزینه ساختمان
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
            MohasebehHazinehSakhtemanPanel.Visibility = Visibility.Visible; MohasebehHazinehSakhtemanPanel.IsEnabled = true;
            //var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
            //          select p.Year;
            //if (Fin != null)
            //{
            //    MohasebehHazinehSakhtemanPanelCombo2.ItemsSource = Fin.Distinct().ToList();
            //}



        }

        private void MohasebehHazinehSakhtemanPanelBut1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                          where !string.IsNullOrEmpty(p.VahedName)
                          orderby p.ID descending
                          select p;
                if (Fin != null)
                {
                    SbteKharjkardVahedGrid.ItemsSource = Fin.Select(s => new
                    {
                        ID = s.ID,
                        واحد = s.VahedName,
                        عنوان = s.TitleCost,
                        تاریخ = s.PersianDate,
                        مبلغ = s.Cost,
                        ماه = s.mmonth,
                        توضیحات = s.Description
                    }).ToList();
                    SbteKharjkardVahedGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        public void MohasebehSakhteman(bool Tasfieh) // ساخت جدول هزینه ساختمان
        {
            //try
            //{
            MohasebeHazinehSakhtemanTab.Visibility = Visibility.Visible;
            int vahed = 0;
            decimal poolSandogh = 0;
            DateTime StartEskan = DateTime.Now;
            DateTime FinishEskan = DateTime.Now;
            string ShiveeTaghsim = "";
            int metrazh = 0;
            int nafarat = 0;

            MohasebeHazinehSakhtemanTab.Items.Clear();
            IadAdvarDaroDataaGrid.Items.Clear();

            var fin = from _ in _FamilyManaerDBEntities.HamsaieTbls
                      select _;
            if (fin != null)
            {


                foreach (var itemNahaiyHamsaie in fin)
                {
                    System.Windows.Controls.DataGrid _DataGrid = new System.Windows.Controls.DataGrid();
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "عنوان", Binding = new System.Windows.Data.Binding("عنوان") });
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "تاریخ", Binding = new System.Windows.Data.Binding("تاریخ") });
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "هزینه کل", Binding = new System.Windows.Data.Binding("Cost") });
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "سهم هزینه واحد", Binding = new System.Windows.Data.Binding("Vahed") });
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "شیوه محاسبه", Binding = new System.Windows.Data.Binding("Method") });
                    _DataGrid.Columns.Add(new DataGridTextColumn { Header = "پرداختی واحد", Binding = new System.Windows.Data.Binding("pardakhti") });

                    _DataGrid.IsReadOnly = true;
                    _DataGrid.Name = "Grid" + itemNahaiyHamsaie.ID;
                    TabItem _TabItem = new TabItem();
                    _TabItem.Header = itemNahaiyHamsaie.NameVahed;

                    var FinHazineSakhteman = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                                             where _.Year == MohasebehHazinehSakhsdfgtemanPanelcombo.Text && _.mmonth == MohasebehHazinehSakhtemanPansdfgelComBo.Text
                                             orderby _.GDate ascending
                                             select _;
                    if (FinHazineSakhteman != null)
                    {
                        decimal Hazineh = 0;
                        decimal Sharj = 0;
                        foreach (var itemHazineSakhteman in FinHazineSakhteman)
                        {
                            // قبض نیست
                            if (string.IsNullOrEmpty(itemHazineSakhteman.FinishPersianDate) && itemHazineSakhteman.Cost > 0 && !string.IsNullOrEmpty(itemHazineSakhteman.PersianDate))// -قبض نیست-شارژ نیست-انتقال از ماه قبل نیست
                            {
                                var FinHamsaieDargir = from _ in _FamilyManaerDBEntities.HamsaieTbls //// مشخص کردن همسایه های درگیر در محاسبه
                                                       where
                                                       (string.IsNullOrEmpty(itemHazineSakhteman.FinishPersianDate) && _.StartGdate <= itemHazineSakhteman.GDate && string.IsNullOrEmpty(_.FinishPersianDate))   //قبض نیست و  از قبل از هزینه آمده و نرفته
                                                      || (string.IsNullOrEmpty(itemHazineSakhteman.FinishPersianDate) && _.StartGdate <= itemHazineSakhteman.GDate && _.FinishGdate >= itemHazineSakhteman.GDate)  //قبض نیست و  از قبل از هزینه آمده و رفته  

                                                       orderby _.ID descending
                                                       select _;

                                vahed = 0;
                                metrazh = 0;
                                nafarat = 0;
                                bool Hozor = false;
                                foreach (var F11 in FinHamsaieDargir)
                                {
                                    if (itemNahaiyHamsaie.NameVahed == F11.NameVahed)
                                    {
                                        Hozor = true;
                                    }
                                    vahed++;
                                    metrazh = metrazh + F11.Metrazh.Value;
                                    nafarat = nafarat + F11.TedadNafarat.Value;
                                }
                                ShiveeTaghsim = itemHazineSakhteman.ShiveTaghsim;
                                decimal cost = 0;
                                if (!Hozor)
                                {
                                    break;
                                }
                                if (string.IsNullOrEmpty(itemHazineSakhteman.startPersianDate) && !string.IsNullOrEmpty(itemHazineSakhteman.VahedName) && itemHazineSakhteman.VahedName == itemNahaiyHamsaie.NameVahed && itemHazineSakhteman.Cost > 0)  //قبض نیست و سهم واحد است
                                {
                                    Hazineh = Hazineh + itemHazineSakhteman.Cost.Value;
                                    _DataGrid.Items.Add(new { عنوان = itemHazineSakhteman.TitleCost, تاریخ = itemHazineSakhteman.PersianDate, Cost = itemHazineSakhteman.Cost.Value.ToString("N0"), Vahed = itemHazineSakhteman.Cost.Value.ToString("N0"), Method = "سهم واحد", pardakhti = 0 });

                                }
                                else if (string.IsNullOrEmpty(itemHazineSakhteman.startPersianDate) && string.IsNullOrEmpty(itemHazineSakhteman.VahedName))  //قبض نیست و سهم واحد نیست
                                {

                                    switch (itemHazineSakhteman.ShiveTaghsim)
                                    {

                                        case "مساوی":
                                            cost = Math.Round(itemHazineSakhteman.Cost.Value / vahed, 0);
                                            break;
                                        case "متراژ":
                                            cost = Math.Round((itemHazineSakhteman.Cost.Value * itemNahaiyHamsaie.Metrazh.Value) / metrazh, 0);
                                            break;
                                        case "نفرات":
                                            cost = Math.Round((itemHazineSakhteman.Cost.Value * itemNahaiyHamsaie.TedadNafarat.Value) / nafarat, 0);
                                            break;
                                        case "متراژ و نفرات":
                                            cost = Math.Round((((itemHazineSakhteman.Cost.Value / 2) * itemNahaiyHamsaie.TedadNafarat.Value) / nafarat) + (((itemHazineSakhteman.Cost.Value / 2) * itemNahaiyHamsaie.Metrazh.Value) / metrazh), 0);
                                            break;
                                    }
                                    Hazineh = Hazineh + cost;
                                    _DataGrid.Items.Add(new { عنوان = itemHazineSakhteman.TitleCost, تاریخ = itemHazineSakhteman.PersianDate, Cost = itemHazineSakhteman.Cost.Value.ToString("N0"), Vahed = cost.ToString("N0"), Method = ShiveeTaghsim, pardakhti = 0 });

                                }


                            }
                            // قبض است
                            else if (!string.IsNullOrEmpty(itemHazineSakhteman.FinishPersianDate) && itemHazineSakhteman.Cost > 0) // قبض است-  و شارژ نیست
                            {
                                bool Hozor = false;

                                decimal cost = 0;
                                TimeSpan GhabzDuration = itemHazineSakhteman.FinishGdate.Value - itemHazineSakhteman.StartGdat.Value;

                                for (int i = 0; i < GhabzDuration.Days; i++)
                                {
                                    Hozor = false;
                                    vahed = 0;
                                    metrazh = 0;
                                    nafarat = 0;
                                    var TodayGhabz = itemHazineSakhteman.StartGdat.Value.AddDays(i);
                                    var FinHamsaieDargir = from _ in _FamilyManaerDBEntities.HamsaieTbls //// مشخص کردن همسایه های درگیر در محاسبه
                                                           where


                                                              (string.IsNullOrEmpty(_.FinishPersianDate) && _.StartGdate < TodayGhabz)// همسایه نرفته
                                                             || (!string.IsNullOrEmpty(_.FinishPersianDate) && _.FinishGdate > TodayGhabz)// همسایه رفته

                                                           orderby _.ID descending
                                                           select _;

                                    foreach (var F11 in FinHamsaieDargir)
                                    {
                                        if (itemNahaiyHamsaie.NameVahed == F11.NameVahed)
                                        {
                                            Hozor = true;
                                        }
                                        vahed++;
                                        metrazh = metrazh + F11.Metrazh.Value;
                                        nafarat = nafarat + F11.TedadNafarat.Value;
                                    }
                                    ShiveeTaghsim = itemHazineSakhteman.ShiveTaghsim;
                                    //  cost = 0;

                                    if (Hozor)
                                    {
                                        decimal PleGhabzeEmrooz = itemHazineSakhteman.Cost.Value / GhabzDuration.Days;
                                        switch (itemHazineSakhteman.ShiveTaghsim)
                                        {

                                            case "مساوی":
                                                cost += PleGhabzeEmrooz / vahed;
                                                break;
                                            case "متراژ":
                                                cost += (PleGhabzeEmrooz * itemNahaiyHamsaie.Metrazh.Value) / metrazh;
                                                break;
                                            case "نفرات":
                                                cost += (PleGhabzeEmrooz * itemNahaiyHamsaie.TedadNafarat.Value) / nafarat;
                                                break;
                                            case "متراژ و نفرات":
                                                cost += (((PleGhabzeEmrooz / 2) * itemNahaiyHamsaie.TedadNafarat.Value) / nafarat) + (((PleGhabzeEmrooz / 2) * itemNahaiyHamsaie.Metrazh.Value) / metrazh);
                                                break;
                                        }

                                    }
                                }
                                if (cost > 0)
                                {
                                    Hazineh = Hazineh + Math.Round(cost, 0);
                                    _DataGrid.Items.Add(new { عنوان = itemHazineSakhteman.TitleCost, تاریخ = itemHazineSakhteman.PersianDate, Cost = itemHazineSakhteman.Cost.Value.ToString("N0"), Vahed = Math.Round(cost, 0).ToString("N0"), Method = ShiveeTaghsim, pardakhti = 0 });
                                }
                            }
                            else if ((itemHazineSakhteman.Income > 0) && (itemNahaiyHamsaie.NameVahed == itemHazineSakhteman.VahedName)) // شارژ است
                            {
                                Sharj = Sharj + itemHazineSakhteman.Income.Value;
                                _DataGrid.Items.Add(new { عنوان = itemHazineSakhteman.TitleCost, تاریخ = itemHazineSakhteman.PersianDate, Cost = 0, Vahed = 0, Method = "*", pardakhti = itemHazineSakhteman.Income.Value.ToString("N0") });
                            }
                            else if ((itemHazineSakhteman.Cost > 0) && (itemNahaiyHamsaie.NameVahed == itemHazineSakhteman.VahedName)) // انتقال هزینه از ماه قبل است
                            {
                                Hazineh = Hazineh + itemHazineSakhteman.Cost.Value;
                                _DataGrid.Items.Add(new { عنوان = itemHazineSakhteman.TitleCost, تاریخ = itemHazineSakhteman.PersianDate, Cost = itemHazineSakhteman.Cost.Value, Vahed = 0, Method = "*", pardakhti = "0" });
                            }
                        }
                        if (_DataGrid.Items.Count > 0)
                        {
                            poolSandogh = poolSandogh + Sharj - Hazineh;
                            IadAdvarDaroDataaGrid.Items.Add(new { A1 = itemNahaiyHamsaie.NameVahed, A2 = Hazineh.ToString("N0"), A3 = Sharj.ToString("N0"), A4 = (Sharj - Hazineh).ToString("N0") });
                            if (Tasfieh)
                            {
                                string NextMount = string.Empty;
                                string year = string.Empty;
                                if (MohasebehHazinehSakhtemanPansdfgelComBo.Text == "اسفند")
                                {
                                    NextMount = "فروردین";
                                    year = (int.Parse(MohasebehHazinehSakhsdfgtemanPanelcombo.Text) + 1).ToString();
                                }
                                else
                                {
                                    int INTNextMount = (MohasebehHazinehSakhtemanPansdfgelComBo.SelectedIndex) + 1;
                                    year = MohasebehHazinehSakhsdfgtemanPanelcombo.Text;
                                    MohasebehHazinehSakhtemanPansdfgelComBo.SelectedIndex = INTNextMount;
                                    NextMount = MohasebehHazinehSakhtemanPansdfgelComBo.Text;
                                    MohasebehHazinehSakhtemanPansdfgelComBo.SelectedIndex = INTNextMount - 1;

                                }

                                if (Sharj > Hazineh)
                                {
                                    _SabteHazinehSakhtemanTbl.TitleCost = "انتقال پرداختی به ماه بعد";
                                    _SabteHazinehSakhtemanTbl.mmonth = MohasebehHazinehSakhtemanPansdfgelComBo.Text;
                                    _SabteHazinehSakhtemanTbl.Year = MohasebehHazinehSakhsdfgtemanPanelcombo.Text;
                                    _SabteHazinehSakhtemanTbl.Cost = Sharj - Hazineh;
                                    _SabteHazinehSakhtemanTbl.Income = 0;
                                    _SabteHazinehSakhtemanTbl.Enteghali = true;
                                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(itemNahaiyHamsaie.ID.ToString());
                                    _SabteHazinehSakhtemanTbl.VahedName = itemNahaiyHamsaie.NameVahed;
                                    _FamilyManaerDBEntities.SaveChanges();


                                    _SabteHazinehSakhtemanTbl.TitleCost = "انتقال پرداختی از ماه قبل";
                                    _SabteHazinehSakhtemanTbl.mmonth = NextMount;
                                    _SabteHazinehSakhtemanTbl.Year = year;
                                    _SabteHazinehSakhtemanTbl.Cost = 0;
                                    _SabteHazinehSakhtemanTbl.Enteghali = true;
                                    _SabteHazinehSakhtemanTbl.Income = Sharj - Hazineh;
                                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(itemNahaiyHamsaie.ID.ToString());
                                    _SabteHazinehSakhtemanTbl.VahedName = itemNahaiyHamsaie.NameVahed;
                                    _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                                    _FamilyManaerDBEntities.SaveChanges();

                                }
                                else if (Sharj < Hazineh)
                                {
                                    _SabteHazinehSakhtemanTbl.TitleCost = "انتقال  بدهی به ماه بعد";
                                    _SabteHazinehSakhtemanTbl.mmonth = MohasebehHazinehSakhtemanPansdfgelComBo.Text;
                                    _SabteHazinehSakhtemanTbl.Year = MohasebehHazinehSakhsdfgtemanPanelcombo.Text;
                                    _SabteHazinehSakhtemanTbl.Cost = 0;
                                    _SabteHazinehSakhtemanTbl.Enteghali = true;
                                    _SabteHazinehSakhtemanTbl.Income = Hazineh - Sharj;
                                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(itemNahaiyHamsaie.ID.ToString());
                                    _SabteHazinehSakhtemanTbl.VahedName = itemNahaiyHamsaie.NameVahed;
                                    _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                                    _FamilyManaerDBEntities.SaveChanges();


                                    _SabteHazinehSakhtemanTbl.TitleCost = "انتقال بدهی از ماه قبل";
                                    _SabteHazinehSakhtemanTbl.mmonth = NextMount;
                                    _SabteHazinehSakhtemanTbl.Year = year;
                                    _SabteHazinehSakhtemanTbl.Enteghali = true;
                                    _SabteHazinehSakhtemanTbl.Cost = Hazineh - Sharj;
                                    _SabteHazinehSakhtemanTbl.Income = 0;
                                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(itemNahaiyHamsaie.ID.ToString());
                                    _SabteHazinehSakhtemanTbl.VahedName = itemNahaiyHamsaie.NameVahed;
                                    _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                                    _FamilyManaerDBEntities.SaveChanges();
                                }

                                MohasebehSakhteman(false);
                            }
                        }
                    }
                    if (_DataGrid.Items.Count > 0)
                    {
                        _TabItem.Content = _DataGrid;
                        MohasebeHazinehSakhtemanTab.Items.Add(_TabItem);
                    }
                }
                IadAdvarDaroDataaGrid.Items.Add(new { A1 = "موجودی صندوق", A2 = "*", A3 = "*", A4 = poolSandogh.ToString("N0") });

            }
            //catch (Exception error) { SaveError(error); }
        }
        private void MohasebehHazinehSakhtemanPanelBut1_Click_1(object sender, RoutedEventArgs e) // ساخت جدول هزینه ساختمان
        {
            MohasebehSakhteman(false);
        }

        private void MohasebehHazinehSakhtemanPanelTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(KhomsTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    KhomsTextBox3.Text = string.Format("{0:N0}", number);
                    KhomsTextBox3.SelectionStart = KhomsTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void MohasebehHazinehSakhtemanPanelBut2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MohasebehHazinehSakhtemanPanelBut3_Click(object sender, RoutedEventArgs e) // دکمه رند کردن
        {
            var result = MajMessageBox.show("آیا از انجام تصفیه اطمینان دارید؟", MajMessageBox.MajMessageBoxBut.IadAvar);
            if (result.ToString() == "Yes")
            {
                MohasebehSakhteman(true);

            }
        }

        private void KhomsUPToolBar_Click(object sender, RoutedEventArgs e) // تولبار خمس
        {
            string salNew = "", SAlOld = "";
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            KhomsPanel.Visibility = Visibility.Visible; KhomsPanel.IsEnabled = true;

            var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                       orderby p.ID ascending
                       select p;
            if (Fin1 != null)
            {
                foreach (var F1 in Fin1)
                {
                    salNew = F1.PersianDate.Substring(0, 4);
                    if (salNew != SAlOld)
                    {
                        KhomsCombo2.Items.Add(salNew);
                        SAlOld = salNew;

                    }
                }
            }
        }

        private void KhomsBut3_Click(object sender, RoutedEventArgs e) // محاسبه خمس
        {
            try
            {
                if ((KhomsCombo2.Text == "") || (KhomsCombo1.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را انتخاب  نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                int Mah = 0;
                decimal Hazineh = 0, Daramad = 0;
                switch (KhomsCombo1.Text)
                {
                    case "فروردین":
                        Mah = 1;
                        break;
                    case "اردیبهشت":
                        Mah = 2;
                        break;
                    case "خرداد":
                        Mah = 3;
                        break;
                    case "تیر":
                        Mah = 4;
                        break;
                    case "مرداد":
                        Mah = 6;
                        break;
                    case "شهریور":
                        Mah = 6;
                        break;
                    case "مهر":
                        Mah = 7;
                        break;
                    case "آبان":
                        Mah = 8;
                        break;
                    case "آذر":
                        Mah = 9;
                        break;
                    case "دی":
                        Mah = 10;
                        break;
                    case "بهمن":
                        Mah = 11;
                        break;
                    case "اسفند":
                        Mah = 12;
                        break;
                }

                PersianCalendar pc = new PersianCalendar();
                DateTime startDate = DateTime.Now;
                DateTime finishtDate = DateTime.Now;


                startDate = pc.ToDateTime(int.Parse(KhomsCombo2.Text), Mah, 1, 12, 30, 0, 0);
                finishtDate = startDate.AddYears(1);

                KhomsGrid.Columns.Clear();
                KhomsGrid.Items.Clear();
                KhomsGrid.Columns.Add(new DataGridTextColumn { Header = "درآمد", Binding = new System.Windows.Data.Binding("I") });
                KhomsGrid.Columns.Add(new DataGridTextColumn { Header = "هزینه", Binding = new System.Windows.Data.Binding("H") });


                var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                            where startDate <= p.Datee.Value && finishtDate >= p.Datee.Value
                            orderby p.ID descending
                            select p;
                if (Fin11 != null)
                {
                    foreach (var F1 in Fin11)
                    {
                        Hazineh = Hazineh + F1.Cost.Value;
                        Daramad = Daramad + F1.Income.Value;

                    }

                    KhomsGrid.Items.Add(new { H = Hazineh.ToString("N0"), I = Daramad.ToString("N0") });
                    if (Daramad > Hazineh)
                    {
                        KhomsTextBox3.Text = ((Daramad - Hazineh) / 5).ToString("N0");
                    }
                    else
                    {
                        KhomsTextBox3.Text = " 0";
                    }
                }



            }
            catch (Exception error) { SaveError(error); }
        }




        private void KharjKardBut7_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : تخصیص هزینه
        {




            TakhsisHazinehCombo2.Items.Clear();
            string salNew = "", SAlOld = "";
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            TakhsisHazinehPanel.Visibility = Visibility.Visible; TakhsisHazinehPanel.IsEnabled = true;
            KharjKardLeftToolbarProfileVisible();


            var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                       orderby p.ID ascending
                       select p;
            if (Fin1 != null)
            {
                foreach (var F1 in Fin1)
                {
                    salNew = F1.PersianDate.Substring(0, 4);
                    if (salNew != SAlOld)
                    {
                        TakhsisHazinehCombo2.Items.Add(salNew);
                        SAlOld = salNew;

                    }
                }
            }
        }

        private void TakhsisHazinehBut3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((TakhsisHazinehCombo2.Text == "") || (TakhsisHazinehCombo1.Text == "") || (TakhsisHazinehTextBox3.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را انتخاب  نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                int Mah = 0;
                decimal Hazineh = 0, KolHazineh = 0;
                string Tarikh = "";
                switch (TakhsisHazinehCombo1.Text)
                {
                    case "فروردین":
                        Mah = 1;
                        break;
                    case "اردیبهشت":
                        Mah = 2;
                        break;
                    case "خرداد":
                        Mah = 3;
                        break;
                    case "تیر":
                        Mah = 4;
                        break;
                    case "مرداد":
                        Mah = 6;
                        break;
                    case "شهریور":
                        Mah = 6;
                        break;
                    case "مهر":
                        Mah = 7;
                        break;
                    case "آبان":
                        Mah = 8;
                        break;
                    case "آذر":
                        Mah = 9;
                        break;
                    case "دی":
                        Mah = 10;
                        break;
                    case "بهمن":
                        Mah = 11;
                        break;
                    case "اسفند":
                        Mah = 12;
                        break;
                }

                SolidColorBrush RowBrush = new SolidColorBrush();


                PersianCalendar pc = new PersianCalendar();
                PersianCalendar pc2 = new PersianCalendar();
                DateTime startDate = DateTime.Now;
                DateTime today = DateTime.Now;
                DateTime PersianstartDate = DateTime.Now;
                DateTime startstartDate = DateTime.Now;
                DateTime finishtDate = DateTime.Now;
                DateTime shamsi = DateTime.Now;



                startstartDate = pc.ToDateTime(int.Parse(TakhsisHazinehCombo2.Text), Mah, 1, 12, 30, 0, 0);
                startDate = startstartDate;

                if (today < startstartDate)
                {
                    MajMessageBox.show("تاریخ مذکور فراتر از تاریخ امروز است.", MajMessageBox.MajMessageBoxBut.OK);

                    return;

                }
                finishtDate = startDate.AddDays(1);

                TakhsisHazinehGrid.Columns.Clear();
                TakhsisHazinehGrid.Items.Clear();
                TakhsisHazinehGrid.Columns.Add(new DataGridTextColumn { Header = "تاریخ", Binding = new System.Windows.Data.Binding("تاریخ") });
                TakhsisHazinehGrid.Columns.Add(new DataGridTextColumn { Header = "خرج کرد", Binding = new System.Windows.Data.Binding("Cost") });
                TakhsisHazinehGrid.Columns.Add(new DataGridTextColumn { Header = "سهمیه روزانه", Binding = new System.Windows.Data.Binding("سهمیه") });
                TakhsisHazinehGrid.Columns.Add(new DataGridTextColumn { Header = "مانده", Binding = new System.Windows.Data.Binding("مانده") });

                int DayOfMonth = 0;
                while (pc.GetMonth(startstartDate) == pc.GetMonth(startDate))
                {
                    DayOfMonth++;
                    startDate = startDate.AddDays(1);
                }
                startDate = startstartDate;
                while (pc.GetMonth(startstartDate) == pc.GetMonth(startDate))
                {


                    var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                where startDate.Year == p.Datee.Value.Year && startDate.Month == p.Datee.Value.Month && startDate.Day == p.Datee.Value.Day
                                orderby p.ID descending
                                select p;
                    if (Fin11 != null)
                    {
                        foreach (var F1 in Fin11)
                        {
                            Hazineh = Hazineh + F1.Cost.Value;
                            KolHazineh = KolHazineh + Hazineh;
                            Tarikh = F1.PersianDate;

                        }
                    }
                    string sahm;
                    int NumDay = pc2.GetDayOfMonth(shamsi);
                    shamsi = new DateTime(startDate.Year, startDate.Month, startDate.Day, 10, 35, 0);
                    string tarikh = pc2.GetYear(shamsi) + "/" + pc2.GetMonth(shamsi) + "/" + pc2.GetDayOfMonth(shamsi);
                    decimal Dessahm = (decimal.Parse(TakhsisHazinehTextBox3.Text) - KolHazineh) / DayOfMonth;
                    if (Dessahm <= 0)
                    {
                        sahm = "0";
                    }
                    else
                    {
                        sahm = Dessahm.ToString("N0");
                    }
                    DayOfMonth--;

                    if (Dessahm < Hazineh)
                    {
                        RowBrush.Color = Colors.Red;

                    }
                    else
                    {
                        RowBrush.Color = Colors.Green;

                    }


                    TakhsisHazinehGrid.RowBackground = RowBrush;

                    TakhsisHazinehGrid.Items.Add(new { تاریخ = tarikh, Cost = Hazineh.ToString("N0"), سهمیه = sahm, مانده = (decimal.Parse(TakhsisHazinehTextBox3.Text) - KolHazineh).ToString("N0") });

                    Hazineh = 0;

                    if ((today.Year == startDate.Year) && (today.Month == startDate.Month) && (today.Day == startDate.Day))
                    {
                        break;
                    }
                    startDate = startDate.AddDays(1);
                }







            }
            catch (Exception error) { SaveError(error); }
        }

        private void TakhsisHazinehTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(TakhsisHazinehTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    TakhsisHazinehTextBox3.Text = string.Format("{0:N0}", number);
                    TakhsisHazinehTextBox3.SelectionStart = TakhsisHazinehTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void OnvanVamBut1_Click(object sender, RoutedEventArgs e) //ثبت عنوان وام
        {
            try
            {

                if ((string.IsNullOrEmpty(OnvanVamTextBox1.Text)) || (string.IsNullOrEmpty(OnvanVamTextssBox1.Text)) || (string.IsNullOrEmpty(TakhsiddsHazinehTextBox3.Text)) || (string.IsNullOrEmpty(MablaghVamTextBox.Text)) || (string.IsNullOrEmpty(TakhddsiddsHazinehTextBdox3.Text)))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var ispersent = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(x => x.Title == OnvanVamTextBox1.Text);
                if (ispersent != null)
                {
                    MajMessageBox.show("این عنوان قبلاً ا نتخاب شده است لطفاً نام دیگری انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان نباید قبل از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                _OnvanVamTbl.Title = OnvanVamTextBox1.Text;
                _OnvanVamTbl.StarGdate = Par._DateTimeVariableStart;
                _OnvanVamTbl.StartPersianDate = OnvanVamTextssBox1.Text;
                _OnvanVamTbl.TedadAghsat = int.Parse(TakhsiddsHazinehTextBox3.Text);
                _OnvanVamTbl.MablaghVam = decimal.Parse(MablaghVamTextBox.Text);
                _OnvanVamTbl.MablaghGhest = decimal.Parse(TakhddsiddsHazinehTextBdox3.Text);
                _OnvanVamTbl.FAAL = toggleBddutton.IsChecked;
                _OnvanVamTbl.Description = OnvanVamTeddxtBox1.Text;


                _FamilyManaerDBEntities.OnvanVamTbls.Add(_OnvanVamTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                CreateOnvanVamGrid();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void OnvanVamBut3_Click(object sender, RoutedEventArgs e) //حذف عنوان وام
        {
            try
            {
                object item = (object)gridOnvanVam.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.OnvanVamTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridOnvanVam.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Title, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.OnvanVamTbls.Remove(ispresent);
                        var Fin1 = from _ in _FamilyManaerDBEntities.TakhsisVamTbls
                                   where _.Onvan == ispresent.Title
                                   select _;
                        var Fin2 = from _ in _FamilyManaerDBEntities.PardakhtVamTbls
                                   where _.OnvanVam == ispresent.Title
                                   select _;
                        var Fin3 = from _ in _FamilyManaerDBEntities.OnvanVamNafarTbls
                                   where _.VamTitle == ispresent.Title
                                   select _;
                        foreach (var item1 in Fin1)
                        {
                            _FamilyManaerDBEntities.TakhsisVamTbls.Remove(item1);
                        }
                        foreach (var item2 in Fin2)
                        {
                            _FamilyManaerDBEntities.PardakhtVamTbls.Remove(item2);
                        }
                        foreach (var item3 in Fin3)
                        {
                            _FamilyManaerDBEntities.OnvanVamNafarTbls.Remove(item3);
                        }


                        _FamilyManaerDBEntities.SaveChanges();


                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateOnvanVamGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void VamRightToolbarBut1_Click(object sender, RoutedEventArgs e) //دکمه سمت چپ : عنوان وام
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            OnvanVamPanel.Visibility = Visibility.Visible; OnvanVamPanel.IsEnabled = true;
            CreateOnvanVamGrid();
        }

        private void SandoghVamUPToolBar_Click(object sender, RoutedEventArgs e) //دکمه تولبار : وام
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            VamToolbarProfileVisible();
        }

        private void NameVamGirandehBut1_Click(object sender, RoutedEventArgs e)//ثبت نام وام گیرنده
        {
            try
            {
                if ((NameVamGirandehTextBox1.Text == "") || (NameVamGirandehCombo1.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _OnvanVamNafarTbl.Nafar = NameVamGirandehTextBox1.Text;
                _OnvanVamNafarTbl.VamTitle = NameVamGirandehCombo1.Text;
                _OnvanVamNafarTbl.Mobile = SabteIafdAvarTextBox2.Text;


                _FamilyManaerDBEntities.OnvanVamNafarTbls.Add(_OnvanVamNafarTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CreateVamNafarGrid();
                CleanOldDataEnteredTXT();
                EmptyPar();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void VamRightToolbarBut2_Click(object sender, RoutedEventArgs e) // دکمه سمت چپ:نام نفرات وام گیرنده
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            NameVamGirandehPanel.Visibility = Visibility.Visible; NameVamGirandehPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamTbls
                      where p.FAAL == true
                      select p.Title;
            if (Fin != null)
            {
                NameVamGirandehCombo1.ItemsSource = Fin.ToList();
            }
            CreateVamNafarGrid();
            NameVamGirandehCombo1.IsReadOnly = false;
            NameVamGirandehTextBox1.IsReadOnly = false;
            NameVamGirandehCombo1.IsEnabled = true;

        }

        private void NameVamGirandehBut3_Click(object sender, RoutedEventArgs e) //حذف نام وام گیرنده
        {
            try
            {
                object item = (object)gridNameVamGirandeh.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.OnvanVamNafarTbls.Where(check => check.ID == ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Nafar, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.OnvanVamNafarTbls.Remove(ispresent);

                        var Fin1 = from _ in _FamilyManaerDBEntities.TakhsisVamTbls
                                   where _.Onvan == ispresent.VamTitle && _.NameVamGirandeh == ispresent.Nafar
                                   select _;
                        var Fin2 = from _ in _FamilyManaerDBEntities.PardakhtVamTbls
                                   where _.OnvanVam == ispresent.VamTitle && _.NameVamGirandeh == ispresent.Nafar
                                   select _;

                        foreach (var item1 in Fin1)
                        {
                            _FamilyManaerDBEntities.TakhsisVamTbls.Remove(item1);
                        }
                        foreach (var item2 in Fin2)
                        {
                            _FamilyManaerDBEntities.PardakhtVamTbls.Remove(item2);
                        }


                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                NameVamGirandehCombo1.IsReadOnly = false;
                NameVamGirandehTextBox1.IsReadOnly = false;
                NameVamGirandehCombo1.IsEnabled = true;

                // CreateVamNafarGrid();
                gridNameVamGirandeh.SelectedItem = null;
                EmptyPar();
                CleanOldDataEnteredTXT();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void VamRightToolbarBut3_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : تخصیص وام
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            NobatVamGirandehPanel.Visibility = Visibility.Visible; NobatVamGirandehPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamTbls
                      where p.FAAL == true
                      select p.Title;
            if (Fin != null)
            {
                NobatVamGirandehCombo1.ItemsSource = Fin.ToList();
            }
            GiveMePersianYear(DateTime.Now, NobatVamGirandehCombo4);
            NobatVamGirandehCombo1.SelectedIndex = 0;
            NobatVamGirandehCombo2.SelectedIndex = 0;
            NobatVamGirandehCombo3.SelectedIndex = 0;
            var Fin1 = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                       where NobatVamGirandehCombo1.Text == p.VamTitle
                       select p.Nafar;
            if (Fin1 != null)
            {
                NobatVamGirandehCombo2.ItemsSource = Fin1.ToList();
            }
            int year = int.Parse(NobatVamGirandehCombo4.Text);
            NobatVamGirandehCombo4.Items.Add(year + 3);
            NobatVamGirandehCombo4.Items.Add(year + 4);
            CreateVamNobatGrid();
            MojodiSandoghVam();
        }
        private void NobatVamGirandehBut1_Click(object sender, RoutedEventArgs e) // ثبت تخصیص وام
        {
            try
            {
                int num = NobatVamGirandehCombo1.SelectedIndex;
                if ((NobatVamGirandehCombo1.Text == "") || (NobatVamGirandehCombo2.Text == "") || (NobatVamGirandehCombo3.Text == "") || (NobatVamGirandehCombo4.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var Fin = from _ in _FamilyManaerDBEntities.TakhsisVamTbls
                          where _.Onvan == NobatVamGirandehCombo1.Text && _.NameVamGirandeh == NobatVamGirandehCombo2.Text
                          select _;
                foreach (var item in Fin)
                {
                    MajMessageBox.show("این شخص قبلاً انتخاب شده است", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                _TakhsisVamTbl.NobatVam_Mah = NobatVamGirandehCombo3.Text;
                _TakhsisVamTbl.Onvan = NobatVamGirandehCombo1.Text;
                _TakhsisVamTbl.NameVamGirandeh = NobatVamGirandehCombo2.Text;
                _TakhsisVamTbl.NobatVam_Sal = NobatVamGirandehCombo4.Text;
                _TakhsisVamTbl.Tozihat = NobatVamGirandehTextBox4.Text;
                _TakhsisVamTbl.Tarikh = NobatVamGirandehTextBox5.Text;
                _TakhsisVamTbl.GTarikh = Par._DateTimeVariable;

                _FamilyManaerDBEntities.TakhsisVamTbls.Add(_TakhsisVamTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                NobatVamGirandehCombo1.SelectedIndex = num;
                CreateVamNobatGrid();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                MojodiSandoghVam();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void gridNobatVamGirandeh_SelectionChanged(object sender, SelectionChangedEventArgs e) // گرید تخصیص وام
        {
            try
            {
                object item = (object)gridNobatVamGirandeh.SelectedItem;
                if (item != null)
                {
                    string name = item.ToString();
                    int foundS1 = name.IndexOf("=");
                    int foundS2 = name.IndexOf(",", foundS1 + 1);

                    name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                    Par.ID = Convert.ToInt32(name);
                    var ispresent = _FamilyManaerDBEntities.TakhsisVamTbls.Where(check => check.ID == Par.ID).FirstOrDefault();
                    if (ispresent != null)
                    {
                        Par._DateTimeVariable = ispresent.GTarikh;
                        NobatVamGirandehCombo1.Text = ispresent.Onvan;
                        NobatVamGirandehCombo3.Text = ispresent.NobatVam_Mah;
                        NobatVamGirandehCombo4.Text = ispresent.NobatVam_Sal;
                        NobatVamGirandehTextBox4.Text = ispresent.Tozihat;
                        NobatVamGirandehTextBox5.Text = ispresent.Tarikh;
                        NobatVamGirandehCombo2.Text = ispresent.NameVamGirandeh;

                    }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void NobatVamGirandehBut2_Click(object sender, RoutedEventArgs e) // ویرایش تخصیص وام
        {
            try
            {
                int num = NobatVamGirandehCombo1.SelectedIndex;
                if ((NobatVamGirandehCombo1.Text == "") || (NobatVamGirandehCombo2.Text == "") || (NobatVamGirandehCombo3.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }



                object item = (object)gridNobatVamGirandeh.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.TakhsisVamTbls.Where(check => check.ID == ID).FirstOrDefault();
                if (ispresent != null)
                {

                    ispresent.NobatVam_Sal = NobatVamGirandehCombo4.Text;
                    ispresent.Onvan = NobatVamGirandehCombo1.Text;
                    ispresent.NameVamGirandeh = NobatVamGirandehCombo2.Text;
                    ispresent.NobatVam_Mah = NobatVamGirandehCombo3.Text;
                    ispresent.Tozihat = NobatVamGirandehTextBox4.Text;
                    ispresent.Tarikh = NobatVamGirandehTextBox5.Text;
                    ispresent.GTarikh = Par._DateTimeVariable.Value;


                    _FamilyManaerDBEntities.SaveChanges(); CreateVamNobatGrid();
                    MajMessageBox.show("اطلاعات با موفقیت تغییر یافت.", MajMessageBox.MajMessageBoxBut.OK);

                    CleanOldDataEnteredTXT();
                    EmptyPar();
                    NobatVamGirandehCombo1.SelectedIndex = num;
                    CreateVamNobatGrid();

                }
                else { MajMessageBox.show("لطافاً ابتدا ردیف مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void NobatVamGirandehCombo1_DropDownClosed(object sender, EventArgs e)
        {
            CreateVamNobatGrid();
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                      where NobatVamGirandehCombo1.Text == p.VamTitle
                      select p.Nafar;
            if (Fin != null)
            {
                NobatVamGirandehCombo2.ItemsSource = Fin.ToList();
            }
            MojodiSandoghVam();
        }

        private void NobatVamGirandehBut3_Click(object sender, RoutedEventArgs e) // حذف تخصیص وام
        {
            try
            {
                object item = (object)gridNobatVamGirandeh.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.TakhsisVamTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridNameVamGirandeh.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.NameVamGirandeh, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.TakhsisVamTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateVamNobatGrid();

            }
            catch (Exception error) { SaveError(error); }
        }


        private void VamRightToolbarBut4_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            PardakhtVamPanel.Visibility = Visibility.Visible; PardakhtVamPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamTbls
                      where p.FAAL == true
                      select p.Title;
            if (Fin != null)
            {
                PardakhtVamCombo1.ItemsSource = Fin.Distinct().ToList();
            }
        }

        private void PardakhtVamCombo2_DropDownClosed(object sender, EventArgs e)
        {

            PardakhtVamGrid();
        }

        private void PardakhtVamBut1_Click(object sender, RoutedEventArgs e) // ثبت پرداخت وام
        {
            int num1 = 0, num2 = 0;
            try
            {
                if ((PardakhtVamCombo1.Text == "") || (PardakhtVamCombo2.Text == "") || (PardakhtVamTextBox2.Text == "") || (PardakhtVamTextBox5.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }

                _PardakhtVamTbl.OnvanVam = PardakhtVamCombo1.Text;
                _PardakhtVamTbl.NameVamGirandeh = PardakhtVamCombo2.Text;
                _PardakhtVamTbl.MablaghPardakhti = decimal.Parse(PardakhtVamTextBox2.Text);
                _PardakhtVamTbl.Tozihat = PardakhtVamTextBox4.Text;
                _PardakhtVamTbl.Tarikh = Par.Tarikh;
                _PardakhtVamTbl.GTarikh = Par._DateTimeVariable.Value;

                num1 = PardakhtVamCombo1.SelectedIndex;
                num2 = PardakhtVamCombo2.SelectedIndex;
                _FamilyManaerDBEntities.PardakhtVamTbls.Add(_PardakhtVamTbl);
                _FamilyManaerDBEntities.SaveChanges();
                CleanOldDataEnteredTXT();
                EmptyPar();
                PardakhtVamCombo1.SelectedIndex = num1;
                PardakhtVamCombo2.SelectedIndex = num2;
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                PardakhtVamGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void PardakhtVamBut3_Click(object sender, RoutedEventArgs e) //حذف وام پرداختی
        {
            try
            {
                int num1 = PardakhtVamCombo1.SelectedIndex;
                int num2 = PardakhtVamCombo2.SelectedIndex;
                object item = (object)gridPardakhtVam.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string name = item.ToString();
                int foundS1 = name.IndexOf("=");
                int foundS2 = name.IndexOf(",", foundS1 + 1);

                name = name.Substring(foundS1 + 1, foundS2 - foundS1 - 1);
                int ID = Convert.ToInt32(name);
                var ispresent = _FamilyManaerDBEntities.PardakhtVamTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridPardakhtVam.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حدف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.NameVamGirandeh, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.PardakhtVamTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                PardakhtVamCombo1.SelectedIndex = num1;
                PardakhtVamCombo2.SelectedIndex = num2;
                PardakhtVamGrid();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void VamRightToolbarBut5_Click(object sender, RoutedEventArgs e)
        {
            gridNameVacmGirandeh.Items.Clear();
            ListBox2.Items.Clear();
            gridGozaeshVam.Items.Clear();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            GozaeshVamPanel.Visibility = Visibility.Visible; GozaeshVamPanel.IsEnabled = true;
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamTbls
                      where p.FAAL == true
                      select p.Title;
            if (Fin != null)
            {
                GozaeshVamCombo1.ItemsSource = Fin.Distinct().ToList();
            }
        }

        private void GozaeshVamCombo1_DropDownClosed(object sender, EventArgs e)
        {
            gridNameVacmGirandeh.Items.Clear();
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                      where p.VamTitle == GozaeshVamCombo1.Text
                      select p.Nafar;
            if (Fin != null)
            {
                GozaeshVamCombo2.ItemsSource = Fin.Distinct().ToList();
            }
            var ispersentVam = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.Title == GozaeshVamCombo1.Text);




            var FinOnvanVam = from _ in _FamilyManaerDBEntities.OnvanVamNafarTbls
                              where _.VamTitle == GozaeshVamCombo1.Text
                              select _;

            foreach (var itemOnvanVam2 in FinOnvanVam)
            {
                decimal TafavotPardakhtBishtar = 0;
                decimal TafavotPardakhtKamtar = 0;
                bool PardakhtBish = false;
                bool PardakhtKam = false;
                bool Ok = true;
                PersianCalendar pc = new PersianCalendar();
                DateTime NextMonthDate = DateTime.Now;
                int ShomarehGhest = 1;
                int DayTakhir = 0;

                DateTime today = DateTime.Now;
                // تبدیل میلادی به هجری شمسی
                int yearStart = pc.GetYear(ispersentVam.StarGdate.Value);
                int MonthStart = pc.GetMonth(ispersentVam.StarGdate.Value);
                int DayStart = pc.GetDayOfMonth(ispersentVam.StarGdate.Value);
                int YearNow = pc.GetYear(DateTime.Now);
                int MonthtNow = pc.GetMonth(DateTime.Now);
                int DayNow = pc.GetDayOfMonth(DateTime.Now);
                int TedadAghsat = ((YearNow - yearStart) * 12 + MonthtNow - MonthStart);
                decimal AghsatPardakhti = 0;
                decimal AghsateMandeh = 0;
                var OnvanVam = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.Title == GozaeshVamCombo1.Text);

                var Fin2 = from p in _FamilyManaerDBEntities.PardakhtVamTbls
                           where (p.OnvanVam == GozaeshVamCombo1.Text) && (p.NameVamGirandeh == itemOnvanVam2.Nafar)
                           orderby p.Tarikh ascending
                           select p;

                if (Fin2 != null)
                {
                    decimal PardakhtiIekGhest = 0;
                    decimal MablaghkGhest = 0;
                    foreach (var F2 in Fin2)
                    {

                        while (Ok)
                        {
                            if (PardakhtBish)
                            {
                                PardakhtiIekGhest = TafavotPardakhtBishtar;
                                MablaghkGhest = OnvanVam.MablaghGhest.Value;
                                PardakhtBish = false;
                            }
                            else if (PardakhtKam)
                            {
                                PardakhtiIekGhest = F2.MablaghPardakhti.Value;
                                MablaghkGhest = TafavotPardakhtKamtar;
                                PardakhtKam = false;
                            }
                            else
                            {
                                PardakhtiIekGhest = F2.MablaghPardakhti.Value;
                                MablaghkGhest = OnvanVam.MablaghGhest.Value;
                            }

                            TafavotPardakhtKamtar = 0;
                            TafavotPardakhtBishtar = 0;

                            var SarresidDate = pc.AddMonths(OnvanVam.StarGdate.Value, ShomarehGhest);
                            if (SarresidDate > F2.GTarikh)
                            {

                            }
                            else
                            {
                                TimeSpan timespan = SarresidDate - F2.GTarikh.Value;
                                DayTakhir += timespan.Days;
                            }
                            //یک = ShomarehGhest.ToString(),                                      //شماره قسط
                            //دو = TarikhSarresid,                                                //سررسید قسط
                            //سه = F2.Tarikh,                                                     //تاریخ پرداخت
                            //هفت = MablaghkGhest.ToString("N0"),                                  //مبلغ قست  
                            //چهار = PardakhtiIekGhest.ToString("N0"),                     //مبلغ پرداختی
                            //پنج = (PardakhtiIekGhest - MablaghkGhest).ToString("N0"),          //ما به تفاوت
                            //شش = OnvanVam.TedadAghsat - ShomarehGhest                                 //اقساط باقی مانده

                            if (MablaghkGhest == PardakhtiIekGhest)
                            {
                                ShomarehGhest++;
                                PardakhtiIekGhest = 0;
                                TafavotPardakhtKamtar = 0;
                                TafavotPardakhtBishtar = 0;
                                Ok = false;
                            }
                            else if (MablaghkGhest < PardakhtiIekGhest) // مبلغ پرداختی بیشتر
                            {
                                ShomarehGhest++;
                                PardakhtBish = true;
                                PardakhtKam = false;
                                TafavotPardakhtBishtar = PardakhtiIekGhest - MablaghkGhest;
                                PardakhtiIekGhest = 0;


                            }
                            else if (MablaghkGhest > PardakhtiIekGhest) /// مبلغ پرداختی کمتر
                            {
                                TafavotPardakhtKamtar = MablaghkGhest - PardakhtiIekGhest;
                                PardakhtiIekGhest = 0;
                                PardakhtKam = true;
                                PardakhtBish = false;
                                Ok = false;
                            }
                        }
                        Ok = true;

                    }
                }

                var FinPardakhti = from _ in _FamilyManaerDBEntities.PardakhtVamTbls
                                   where _.OnvanVam == GozaeshVamCombo1.Text && _.NameVamGirandeh == itemOnvanVam2.Nafar
                                   group _ by _.MablaghPardakhti into g
                                   select new
                                   {
                                       Pardakhti = g.Sum(_ => _.MablaghPardakhti)
                                   };
                foreach (var itemPardakhti in FinPardakhti)
                {
                    AghsatPardakhti = itemPardakhti.Pardakhti.Value / ispersentVam.MablaghGhest.Value;
                }
                AghsateMandeh = TedadAghsat - AghsatPardakhti;
                if (AghsateMandeh >= 1)
                {
                    if (DayNow < DayStart)
                    {
                        AghsateMandeh += -1;
                    }
                    gridNameVacmGirandeh.Items.Add(new { A1 = itemOnvanVam2.Nafar, A2 = AghsateMandeh, A3 = DayTakhir });
                }
            }
        }

        private void GozaeshVamCombo2_DropDownClosed(object sender, EventArgs e) // جدول اقساط باقی مانده
        {
            try
            {
                gridGozaeshVam.Columns.Clear();
                gridGozaeshVam.Items.Clear();
                decimal TafavotPardakhtBishtar = 0;
                decimal TafavotPardakhtKamtar = 0;
                bool PardakhtBish = false;
                bool PardakhtKam = false;
                bool Ok = true;
                PersianCalendar pc = new PersianCalendar();
                DateTime NextMonthDate = DateTime.Now;
                int ShomarehGhest = 1;
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "شماره قسط", Binding = new System.Windows.Data.Binding("یک") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "سررسید قسط", Binding = new System.Windows.Data.Binding("دو") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "تاریخ پرداخت", Binding = new System.Windows.Data.Binding("سه") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "مبلغ قسط", Binding = new System.Windows.Data.Binding("هفت") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "مبلغ پرداختی", Binding = new System.Windows.Data.Binding("چهار") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "ما به تفاوت", Binding = new System.Windows.Data.Binding("پنج") });
                gridGozaeshVam.Columns.Add(new DataGridTextColumn { Header = "اقساط باقی مانده", Binding = new System.Windows.Data.Binding("شش") });

                var OnvanVam = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.Title == GozaeshVamCombo1.Text);
                var Fin1 = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                           where (p.VamTitle == GozaeshVamCombo1.Text) && (p.Nafar == GozaeshVamCombo2.Text)
                           orderby p.ID descending
                           select p;

                if (Fin1 != null)
                {
                    foreach (var F1 in Fin1)
                    {
                        var Fin2 = from p in _FamilyManaerDBEntities.PardakhtVamTbls
                                   where (p.OnvanVam == GozaeshVamCombo1.Text) && (p.NameVamGirandeh == GozaeshVamCombo2.Text)
                                   orderby p.Tarikh ascending
                                   select p;

                        if (Fin2 != null)
                        {
                            decimal PardakhtiIekGhest = 0;
                            decimal MablaghkGhest = 0;
                            foreach (var F2 in Fin2)
                            {

                                while (Ok)
                                {
                                    if (PardakhtBish)
                                    {
                                        PardakhtiIekGhest = TafavotPardakhtBishtar;
                                        MablaghkGhest = OnvanVam.MablaghGhest.Value;
                                        PardakhtBish = false;
                                    }
                                    else if (PardakhtKam)
                                    {
                                        PardakhtiIekGhest = F2.MablaghPardakhti.Value;
                                        MablaghkGhest = TafavotPardakhtKamtar;
                                        PardakhtKam = false;
                                    }
                                    else
                                    {
                                        PardakhtiIekGhest = F2.MablaghPardakhti.Value;
                                        MablaghkGhest = OnvanVam.MablaghGhest.Value;
                                    }

                                    TafavotPardakhtKamtar = 0;
                                    TafavotPardakhtBishtar = 0;

                                    string TarikhSarresid = pc.GetYear(pc.AddMonths(OnvanVam.StarGdate.Value, ShomarehGhest)) + "/" + pc.GetMonth(pc.AddMonths(OnvanVam.StarGdate.Value, ShomarehGhest)) + "/" + pc.GetDayOfMonth(pc.AddMonths(OnvanVam.StarGdate.Value, ShomarehGhest));
                                    gridGozaeshVam.Items.Add(new
                                    {
                                        یک = ShomarehGhest.ToString(),                                      //شماره قسط
                                        دو = TarikhSarresid,                                                //سررسید قسط
                                        سه = F2.Tarikh,                                                     //تاریخ پرداخت
                                        هفت = MablaghkGhest.ToString("N0"),                                  //مبلغ قست  
                                        چهار = PardakhtiIekGhest.ToString("N0"),                     //مبلغ پرداختی
                                        پنج = (PardakhtiIekGhest - MablaghkGhest).ToString("N0"),          //ما به تفاوت
                                        شش = OnvanVam.TedadAghsat - ShomarehGhest                                 //اقساط باقی مانده
                                    });
                                    if (MablaghkGhest == PardakhtiIekGhest)
                                    {
                                        ShomarehGhest++;
                                        PardakhtiIekGhest = 0;
                                        TafavotPardakhtKamtar = 0;
                                        TafavotPardakhtBishtar = 0;
                                        Ok = false;
                                    }
                                    else if (MablaghkGhest < PardakhtiIekGhest) // مبلغ پرداختی بیشتر
                                    {
                                        ShomarehGhest++;
                                        PardakhtBish = true;
                                        PardakhtKam = false;
                                        TafavotPardakhtBishtar = PardakhtiIekGhest - MablaghkGhest;
                                        PardakhtiIekGhest = 0;


                                    }
                                    else if (MablaghkGhest > PardakhtiIekGhest) /// مبلغ پرداختی کمتر
                                    {
                                        TafavotPardakhtKamtar = MablaghkGhest - PardakhtiIekGhest;
                                        PardakhtiIekGhest = 0;
                                        PardakhtKam = true;
                                        PardakhtBish = false;
                                        Ok = false;
                                    }
                                }
                                Ok = true;

                            }
                        }
                    }






                }
            }

            catch (Exception error) { SaveError(error); }

        }

        private void PardakhtVamCombo1_DropDownClosed(object sender, EventArgs e)
        {
            var Fin = from p in _FamilyManaerDBEntities.OnvanVamNafarTbls
                      where p.VamTitle == PardakhtVamCombo1.Text
                      select p.Nafar;
            if (Fin != null)
            {
                PardakhtVamCombo2.ItemsSource = Fin.Distinct().ToList();
            }
        }

        private void PardakhtVamTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(PardakhtVamTextBox2.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    PardakhtVamTextBox2.Text = string.Format("{0:N0}", number);
                    PardakhtVamTextBox2.SelectionStart = PardakhtVamTextBox2.Text.Length;
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void IadavarDaroUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            DarooLeftToolbarPanel.Visibility = Visibility.Visible; DarooLeftToolbarPanel.IsEnabled = true;
        }

        private void ProfileLeftToolbarPanelBut2_Click(object sender, RoutedEventArgs e) // دکمه سمت راست تنظیمات ساعت
        {
            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                ProfileLeftToolbarPanelVisible();
                ProfileLeftToolbarPanel.Visibility = Visibility.Visible; ProfileLeftToolbarPanel.IsEnabled = true;
                SabteIadAvarTanzimat.Visibility = Visibility.Visible; SabteIadAvarTanzimat.IsEnabled = true;
                var ispresent1 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "check");
                SabteIadAvarTanzimatCombo3.Text = ispresent1.Hafteh.ToString();
                SabteIadAvarTanzimatCombo6.Text = ispresent1.rooz.ToString();
                SabteIadAvarTanzimatTimePicker3.Value = ispresent1.saat;
                var ispresent2 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "kar");
                SabteIadAvarTanzimatCombo2.Text = ispresent2.Hafteh.ToString();
                SabteIadAvarTanzimatCombo5.Text = ispresent2.rooz.ToString();
                SabteIadAvarTanzimatTimePicker2.Value = ispresent2.saat;
                var ispresent3 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Daro");
                SabteIadAvarTanzimatCombo1.Text = ispresent3.Hafteh.ToString();
                SabteIadAvarTanzimatCombo4.Text = ispresent3.rooz.ToString();
                SabteIadAvarTanzimatTimePicker1.Value = ispresent3.saat;
            }
            catch (Exception error)
            {
                SaveError(error);

            }
        }

        private void SabteIadAvarTanzimatBut1_Click_1(object sender, RoutedEventArgs e) // دکمه ثبت ساعت هشدارها
        {
            try
            {


                var ispresent1 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "check");
                ispresent1.Hafteh = int.Parse(SabteIadAvarTanzimatCombo3.Text);
                ispresent1.rooz = int.Parse(SabteIadAvarTanzimatCombo6.Text);
                ispresent1.saat = SabteIadAvarTanzimatTimePicker3.Value;
                var ispresent2 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "kar");
                ispresent2.Hafteh = int.Parse(SabteIadAvarTanzimatCombo2.Text);
                ispresent2.rooz = int.Parse(SabteIadAvarTanzimatCombo5.Text);
                ispresent2.saat = SabteIadAvarTanzimatTimePicker2.Value;
                var ispresent3 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Daro");
                ispresent3.Hafteh = int.Parse(SabteIadAvarTanzimatCombo1.Text);
                ispresent3.rooz = int.Parse(SabteIadAvarTanzimatCombo4.Text);
                ispresent3.saat = SabteIadAvarTanzimatTimePicker1.Value;
                _FamilyManaerDBEntities.SaveChanges();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void TanzimZaherBut1_Click(object sender, RoutedEventArgs e) // انتخاب عکس پس زمیته
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
 "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
 "Portable Network Graphic (*.png)|*.png";
            if (dlg.ShowDialog() == true)
            {
                var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "AksPasZamineh");
                ImageSource imgsource = new BitmapImage(new Uri(dlg.FileName));


                var bitmap = imgsource as BitmapSource;
                var encoder = new PngBitmapEncoder(); // or one of the other encoders
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    ispresent.aks = stream.ToArray();
                    _FamilyManaerDBEntities.SaveChanges();
                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);

                    BackGroundd.ImageSource = bitmap;
                }


            }
        }

        private void TanzimZaherBut2_Click(object sender, RoutedEventArgs e)// دکمه ثبت پیش فرض عکس پس زمینه
        {
            var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "AksPasZamineh");
            var ispresent1 = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "AksPasZaminehGhadimi");

            ispresent.aks = ispresent1.aks;
            _FamilyManaerDBEntities.SaveChanges();
            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
            using (var ms = new System.IO.MemoryStream(ispresent.aks))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                BackGroundd.ImageSource = image;
            }

        }

        private void TanzimZaherPanelCombo2_DropDownClosed(object sender, EventArgs e)// انتخاب فونت
        {
            if (TanzimZaherPanelCombo2.Text != string.Empty)
            {
                var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Font");
                ispresent.Passage = TanzimZaherPanelCombo2.Text;
                _FamilyManaerDBEntities.SaveChanges();
                var ispresentFont = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Font");

                MMainWindow.FontFamily = new System.Windows.Media.FontFamily(ispresentFont.Passage);
            }



        }

        private void TanzimZaherBut3_Click(object sender, RoutedEventArgs e)
        {

            var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Font");
            ispresent.Passage = string.Empty;
            _FamilyManaerDBEntities.SaveChanges();
            System.Windows.Forms.Application.Restart();
            this.Close();

        }

        private void ProfileLeftToolbarPanelBut3_Click(object sender, RoutedEventArgs e)
        {

            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ProfileLeftToolbarPanelVisible();
            TanzimZaherPanel.Visibility = Visibility.Visible; TanzimZaherPanel.IsEnabled = true;
            var ispresent = _FamilyManaerDBEntities.TanzimZamanIadAvars.FirstOrDefault(x => x.Name == "Font");
            if ((ispresent.Passage != null) && (ispresent.Passage != string.Empty))
            {
                TanzimZaherPanelCombo2.Text = ispresent.Passage;
            }
        }

        private void SabteOnvanDaramadPBut1_Click(object sender, RoutedEventArgs e) // ثبت عنوان درآمد
        {
            try
            {
                OnvanDaramadTbl _OnvanDaramadTbl = new OnvanDaramadTbl();
                if (string.IsNullOrEmpty(SabteOnvanDaramadPTextBox1.Text))
                {
                    MajMessageBox.show("لطفاً عنوان درآمد را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var ispresent = _FamilyManaerDBEntities.OnvanDaramadTbls.FirstOrDefault(x => x.Onvan == SabteOnvanDaramadPTextBox1.Text);
                if (ispresent != null)
                {
                    MajMessageBox.show("این عنوان قبلاً وارد شده است", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                _OnvanDaramadTbl.Onvan = SabteOnvanDaramadPTextBox1.Text;
                _FamilyManaerDBEntities.OnvanDaramadTbls.Add(_OnvanDaramadTbl);
                _FamilyManaerDBEntities.SaveChanges();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                EmptyPar();
                CreateOnvanDaramadGrid();
                SabteOnvanDaramadPTextBox1.Text = string.Empty;
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void SabteOnvanDaramadPTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateOnvanDaramadGrid();
        }

        private void SabteOnvanDaramadPBut3_Click(object sender, RoutedEventArgs e)//دکمه حذف عنوان درآمد
        {
            try
            {
                object item = (object)gridSabteOnvanDaramadP.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string onvan = (gridSabteOnvanDaramadP.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var ispresent = _FamilyManaerDBEntities.OnvanDaramadTbls.Where(check => check.Onvan == onvan).FirstOrDefault();
                gridSabteOnvanDaramadP.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.Onvan, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.OnvanDaramadTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                CreateOnvanDaramadGrid();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteOnvanDaramad_Click(object sender, RoutedEventArgs e) // دکمه سمت راست: عنوان درآمد
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            HesabMaliLeftToolbarProfileVisible();
            SabteOnvanDaramadPanel.Visibility = Visibility.Visible; SabteOnvanDaramadPanel.IsEnabled = true;
            CreateOnvanDaramadGrid();

        }

        private void SabteDaramadTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectDaramadGrid();
        }

        private void IncomeGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)IncomeGrid2.SelectedItem;
                string onvan = (IncomeGrid2.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                SabteDaramadTextBox1.Text = onvan;
            }
            catch (Exception error) { SaveError(error); }
        }

        private void GozareshHesabButHesabmali_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : گزارش درآمد
        {
            GozareshDaramadCombo1.Items.Clear();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            GozareshDaramadPanel.Visibility = Visibility.Visible; GozareshDaramadPanel.IsEnabled = true;

            PersianCalendar pc = new PersianCalendar();
            DateTime startDate = DateTime.Now;

            // تبدیل میلادی به هجری شمسی
            GozareshDaramadLBL2.Content = GozareshDaramadLBL1.Content = pc.GetYear(startDate).ToString() + "/" + pc.GetMonth(startDate).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(startDate).ToString().PadLeft(2, '0');
            Par._DateTimeVariableStart= Par._DateTimeVariableFinish = startDate;

            var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                      where p.Income != 0
                      group p by p.Deposite into g
                      select new
                      {
                          sepordeh = g.FirstOrDefault().Deposite
                      };
            foreach (var item in Fin)
            {
                GozareshDaramadCombo1.Items.Add(item.sepordeh);
            }
            if (GozareshDaramadCombo1.Items.Count > 0)
            {
                GozareshDaramadCombo1.SelectedIndex = 0;

            }
        }

        private void GozareshDarAmda() // دکمه ایجاد گزارش درآمد
        {
            try
            {
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (GozareshDaramadCombo1.Text == "همه حساب ها")
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                               where p.Income != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish
                               group p by p.Title into g
                               select new
                               {
                                   onvan = g.FirstOrDefault().Title,
                                   Meghdar = g.Sum(x => x.Income
                                   )
                               };
                    foreach (var item in Fin1)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin2 = from a in _FamilyManaerDBEntities.FinancialTbls
                               where a.Income != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish
                               group a by a.Title into g
                               select new
                               {
                                   onvan = g.FirstOrDefault().Title,
                                   Tedad = g.Count()
                               };
                    foreach (var item1 in Fin2)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshDaramad2.DataContext = chartvalue2;
                }
                else if (GozareshDaramadCombo1.Text == "فاقد نام")
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                where p.Income != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish && p.Deposite == null
                                group p by p.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Meghdar = g.Sum(x => x.Income
                                    )
                                };
                    foreach (var item in Fin11)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin22 = from a in _FamilyManaerDBEntities.FinancialTbls
                                where a.Income != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish && a.Deposite == null
                                group a by a.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Tedad = g.Count()
                                };
                    foreach (var item1 in Fin22)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshDaramad2.DataContext = chartvalue2;
                }
                else
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                where p.Income != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish && p.Deposite == GozareshDaramadCombo1.Text
                                group p by p.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Meghdar = g.Sum(x => x.Income
                                    )
                                };
                    foreach (var item in Fin11)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin22 = from a in _FamilyManaerDBEntities.FinancialTbls
                                where a.Income != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish && a.Deposite == GozareshDaramadCombo1.Text
                                group a by a.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Tedad = g.Count()
                                };
                    foreach (var item1 in Fin22)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshDaramad2.DataContext = chartvalue2;
                }










            }
            catch (Exception error)
            {
                SaveError(error);
            }

        }

        private void PieSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPie.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان درآمد:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarGozareshDaramad1_Click(object sender, RoutedEventArgs e) // دکمه انتخاب شروع تاریخ گزارش درآمد
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshDaramadLBL1.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                GozareshDarAmda();
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshDaramad2_Click(object sender, RoutedEventArgs e) // دکمه انتخاب تاریخ پایان گزارش درآمد
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshDaramadLBL2.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                GozareshDarAmda();
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPieGozareshDaramad2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPieGozareshDaramad2.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان درآمد:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void KharjKardBut4_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : گزارش خرج کرد
        {

            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            KharjKardLeftToolbarProfileVisible();

            GozareshHazinehPanel.Visibility = Visibility.Visible; GozareshHazinehPanel.IsEnabled = true;
            PersianCalendar pc = new PersianCalendar();
            DateTime startDate = DateTime.Now;

            // تبدیل میلادی به هجری شمسی
            GozareshHazinehLBL4.Content = GozareshHazinehLBL3.Content = pc.GetYear(startDate).ToString() + "/" + pc.GetMonth(startDate).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(startDate).ToString().PadLeft(2, '0');

            GozareshHazinehCombo2.SelectedIndex = 0;

            var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                      where p.Cost != 0
                      group p by p.Deposite into g
                      select new
                      {
                          sepordeh = g.FirstOrDefault().Deposite
                      };
            foreach (var item in Fin)
            {
                GozareshHazinehCombo2.Items.Add(item.sepordeh);
            }
        }

        private void GozareshHazinehBut1_Click(object sender, RoutedEventArgs e) // دکمه ایجاد گزارش های خرج کرد 
        {
            try
            {
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                if (GozareshHazinehCombo2.Text == "همه حساب ها")
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                               where p.Cost != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish
                               group p by p.Title into g
                               select new
                               {
                                   onvan = g.FirstOrDefault().Title,
                                   Meghdar = g.Sum(x => x.Cost
                                   )
                               };
                    foreach (var item in Fin1)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart1.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin2 = from a in _FamilyManaerDBEntities.FinancialTbls
                               where a.Cost != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish
                               group a by a.Title into g
                               select new
                               {
                                   onvan = g.FirstOrDefault().Title,
                                   Tedad = g.Count()
                               };
                    foreach (var item1 in Fin2)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshHazineh1.DataContext = chartvalue2;
                }
                else if (GozareshHazinehCombo2.Text == "فاقد نام")
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                where p.Cost != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish && p.Deposite == null
                                group p by p.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Meghdar = g.Sum(x => x.Cost
                                    )
                                };
                    foreach (var item in Fin11)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart1.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin22 = from a in _FamilyManaerDBEntities.FinancialTbls
                                where a.Cost != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish && a.Deposite == null
                                group a by a.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Tedad = g.Count()
                                };
                    foreach (var item1 in Fin22)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshHazineh1.DataContext = chartvalue2;
                }
                else
                {
                    List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                    var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                where p.Cost != 0 && p.Datee > Par._DateTimeVariableStart && p.Datee < Par._DateTimeVariableFinish && p.Deposite == GozareshHazinehCombo2.Text
                                group p by p.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Meghdar = g.Sum(x => x.Cost
                                    )
                                };
                    foreach (var item in Fin11)
                    {
                        chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    }
                    pieChart1.DataContext = chartvalue;


                    List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();
                    var Fin22 = from a in _FamilyManaerDBEntities.FinancialTbls
                                where a.Cost != 0 && a.Datee > Par._DateTimeVariableStart && a.Datee < Par._DateTimeVariableFinish && a.Deposite == GozareshHazinehCombo2.Text
                                group a by a.Title into g
                                select new
                                {
                                    onvan = g.FirstOrDefault().Title,
                                    Tedad = g.Count()
                                };
                    foreach (var item1 in Fin22)
                    {
                        chartvalue2.Add(new KeyValuePair<string, int>(item1.onvan, item1.Tedad));
                    }
                    PPieGozareshHazineh1.DataContext = chartvalue2;
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarGozareshHazineh_Click(object sender, RoutedEventArgs e) // دکمه شروع تاریخ گزارش خرجکرد
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL3.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh1_Click(object sender, RoutedEventArgs e) // دکمه پایان تاریخ گزارش خرجکرد
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL4.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPie1_SelectionChanged(object sender, SelectionChangedEventArgs e)// کلیک بر روی گزارش پای هزینه
        {
            try
            {
                object item = (object)PPie1.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان هزینه:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPieGozareshHazineh1_SelectionChanged(object sender, SelectionChangedEventArgs e) // کلیک ر روی گزارش پای هزینه و تعداد
        {
            try
            {
                object item = (object)PPieGozareshHazineh1.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان هزینه:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshHazinehBut2_Click(object sender, RoutedEventArgs e) // دکمه ایجاد گزارش هزینه و درآمد 
        {
            try
            {
                decimal Hazinehhh = 0, Daramaddd = 0;

                DarAmadClusteredChart.DataContext = null;
                HazinehClusteredChart.DataContext = null;
                List<HazinehDaramadClusteredChat> ListHazinehDaramadClusteredChat = new List<HazinehDaramadClusteredChat>();

                List<KeyValuePair<string, decimal>> chartvalueHazineh = new List<KeyValuePair<string, decimal>>();
                List<KeyValuePair<string, decimal>> chartvalueDarAmad = new List<KeyValuePair<string, decimal>>();

                if (GozareshHazinehCombo5.Text == "همه حساب ها")
                {
                    var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                               where Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                               group p by p.PersianDate into g
                               select new
                               {
                                   DateDate = g.FirstOrDefault().PersianDate,
                                   Hazineh = g.Sum(x => x.Cost),
                                   Daramad = g.Sum(x => x.Income)
                               };
                    foreach (var item in Fin1)
                    {
                        HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                        _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                        _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                        _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                        _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                        _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                        ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                    }
                }
                else if (GozareshHazinehCombo5.Text == "فاقد نام")
                {
                    var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                               where p.Deposite == null && Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                               group p by p.PersianDate into g
                               select new
                               {
                                   DateDate = g.FirstOrDefault().PersianDate,
                                   Hazineh = g.Sum(x => x.Cost),
                                   Daramad = g.Sum(x => x.Income)
                               };
                    foreach (var item in Fin1)
                    {
                        HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                        _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                        _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                        _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                        _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                        _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                        ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                    }
                }
                else
                {
                    var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                               where p.Deposite == GozareshHazinehCombo5.Text && Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                               group p by p.PersianDate into g
                               select new
                               {
                                   DateDate = g.FirstOrDefault().PersianDate,
                                   Hazineh = g.Sum(x => x.Cost),
                                   Daramad = g.Sum(x => x.Income)
                               };
                    foreach (var item in Fin1)
                    {
                        HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                        _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                        _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                        _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                        _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                        _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                        ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                    }
                }

                if (GozareshHazinehCombo6.Text == "روزانه")
                {

                    var Fin2 = from p in ListHazinehDaramadClusteredChat
                               group p by p.Day into gg

                               select new
                               {
                                   DateDate = gg.FirstOrDefault().Day,
                                   Hazineh = gg.Sum(x => x.Hazineh),
                                   Daramad = gg.Sum(x => x.Daramad)
                               };
                    foreach (var item2 in Fin2)
                    {
                        Hazinehhh = Hazinehhh + item2.Hazineh;
                        Daramaddd = Daramaddd + item2.Daramad;
                        chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                        chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                    }
                }
                else if (GozareshHazinehCombo6.Text == "ماهانه")
                {

                    var Fin2 = from p in ListHazinehDaramadClusteredChat
                               group p by p.month into gg

                               select new
                               {
                                   DateDate = gg.FirstOrDefault().month,
                                   Hazineh = gg.Sum(x => x.Hazineh),
                                   Daramad = gg.Sum(x => x.Daramad)
                               };
                    foreach (var item2 in Fin2)
                    {
                        Hazinehhh = Hazinehhh + item2.Hazineh;
                        Daramaddd = Daramaddd + item2.Daramad;
                        chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                        chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                    }
                }
                else if (GozareshHazinehCombo6.Text == "سالانه")
                {

                    var Fin2 = from p in ListHazinehDaramadClusteredChat
                               group p by p.year into gg

                               select new
                               {
                                   DateDate = gg.FirstOrDefault().year,
                                   Hazineh = gg.Sum(x => x.Hazineh),
                                   Daramad = gg.Sum(x => x.Daramad)
                               };
                    foreach (var item2 in Fin2)
                    {
                        Hazinehhh = Hazinehhh + item2.Hazineh;
                        Daramaddd = Daramaddd + item2.Daramad;
                        chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                        chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                    }
                }

                DarAmadClusteredChart.DataContext = chartvalueDarAmad;
                HazinehClusteredChart.DataContext = chartvalueHazineh;
                GozareshHazinehLBL5.Content = Daramaddd.ToString("N0");
                GozareshHazinehLBL6.Content = (Hazinehhh).ToString("N0");
                GozareshHazinehLBL7.Content = (Daramaddd - Hazinehhh).ToString("N0");
            }

            catch (Exception error)
            {
                SaveError(error);
            }

        }

        private void PersianCalendarGozareshHazineh1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL1.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL2.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void KharjKardBut8_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : گزارش خرجکرد درآمد
        {
            try
            {

                Toolbarinvisible();
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                GozareshHazinehDaramadPanel.Visibility = Visibility.Visible; GozareshHazinehDaramadPanel.IsEnabled = true;
                KharjKardLeftToolbarProfileVisible();
                PersianCalendar pc = new PersianCalendar();
                DateTime startDate = DateTime.Now;

                // تبدیل میلادی به هجری شمسی
                GozareshHazinehLBL1.Content = GozareshHazinehLBL2.Content = pc.GetYear(startDate).ToString() + "/" + pc.GetMonth(startDate).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(startDate).ToString().PadLeft(2, '0');

                GozareshHazinehCombo5.SelectedIndex = 0;
                GozareshHazinehCombo6.SelectedIndex = 0;

                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          group p by p.Deposite into g
                          select new
                          {
                              sepordeh = g.FirstOrDefault().Deposite
                          };
                foreach (var item in Fin)
                {
                    GozareshHazinehCombo5.Items.Add(item.sepordeh);
                }
            }

            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshHazinehBut3_Click(object sender, RoutedEventArgs e) // دکمه ایجاد نمودار مانده
        {
            //try { 


            decimal Hazinehhh = 0, Daramaddd = 0;

            DarAmadClusteredChart1.DataContext = null;
            HazinehClusteredChart.DataContext = null;
            List<HazinehDaramadClusteredChat> ListHazinehDaramadClusteredChat = new List<HazinehDaramadClusteredChat>();

            List<KeyValuePair<string, decimal>> chartvalueHazineh = new List<KeyValuePair<string, decimal>>();
            List<KeyValuePair<string, decimal>> chartvalueDarAmad = new List<KeyValuePair<string, decimal>>();

            if (GozareshHazinehCombo1.Text == "همه حساب ها")
            {
                var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                           where Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                           group p by p.PersianDate into g
                           select new
                           {
                               DateDate = g.FirstOrDefault().PersianDate,
                               Hazineh = g.Sum(x => x.Cost),
                               Daramad = g.Sum(x => x.Income)
                           };
                foreach (var item in Fin1)
                {
                    HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                    _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                    _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                    _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                    _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                    _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                    ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                }
            }
            else if (GozareshHazinehCombo1.Text == "فاقد نام")
            {
                var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                           where p.Deposite == null && Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                           group p by p.PersianDate into g
                           select new
                           {
                               DateDate = g.FirstOrDefault().PersianDate,
                               Hazineh = g.Sum(x => x.Cost),
                               Daramad = g.Sum(x => x.Income)
                           };
                foreach (var item in Fin1)
                {
                    HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                    _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                    _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                    _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                    _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                    _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                    ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                }
            }
            else
            {
                var Fin1 = from p in _FamilyManaerDBEntities.FinancialTbls
                           where p.Deposite == GozareshHazinehCombo1.Text && Par._DateTimeVariableStart < p.Datee && Par._DateTimeVariableFinish > p.Datee
                           group p by p.PersianDate into g
                           select new
                           {
                               DateDate = g.FirstOrDefault().PersianDate,
                               Hazineh = g.Sum(x => x.Cost),
                               Daramad = g.Sum(x => x.Income)
                           };
                foreach (var item in Fin1)
                {
                    HazinehDaramadClusteredChat _HazinehDaramadClusteredChat = new HazinehDaramadClusteredChat();
                    _HazinehDaramadClusteredChat.year = item.DateDate.Substring(0, 4);
                    _HazinehDaramadClusteredChat.month = item.DateDate.ToString().Substring(5, 2);
                    _HazinehDaramadClusteredChat.Day = item.DateDate.Substring(8, 2);
                    _HazinehDaramadClusteredChat.Daramad = item.Daramad.Value;
                    _HazinehDaramadClusteredChat.Hazineh = item.Hazineh.Value;
                    ListHazinehDaramadClusteredChat.Add(_HazinehDaramadClusteredChat);
                }
            }

            if (GozareshHazinehCombo3.Text == "روزانه")
            {

                var Fin2 = from p in ListHazinehDaramadClusteredChat
                           group p by p.Day into gg

                           select new
                           {
                               DateDate = gg.FirstOrDefault().Day,
                               Hazineh = gg.Sum(x => x.Hazineh),
                               Daramad = gg.Sum(x => x.Daramad)
                           };
                foreach (var item2 in Fin2)
                {
                    Hazinehhh = Hazinehhh + item2.Hazineh;
                    Daramaddd = Daramaddd + item2.Daramad;
                    chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                    chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                }
            }
            else if (GozareshHazinehCombo3.Text == "ماهانه")
            {

                var Fin2 = from p in ListHazinehDaramadClusteredChat
                           group p by p.month into gg

                           select new
                           {
                               DateDate = gg.FirstOrDefault().month,
                               Hazineh = gg.Sum(x => x.Hazineh),
                               Daramad = gg.Sum(x => x.Daramad)
                           };
                foreach (var item2 in Fin2)
                {
                    Hazinehhh = Hazinehhh + item2.Hazineh;
                    Daramaddd = Daramaddd + item2.Daramad;
                    chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                    chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                }
            }
            else if (GozareshHazinehCombo3.Text == "سالانه")
            {

                var Fin2 = from p in ListHazinehDaramadClusteredChat
                           group p by p.year into gg

                           select new
                           {
                               DateDate = gg.FirstOrDefault().year,
                               Hazineh = gg.Sum(x => x.Hazineh),
                               Daramad = gg.Sum(x => x.Daramad)
                           };
                foreach (var item2 in Fin2)
                {
                    Hazinehhh = Hazinehhh + item2.Hazineh;
                    Daramaddd = Daramaddd + item2.Daramad;
                    chartvalueDarAmad.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Daramad));
                    chartvalueHazineh.Add(new KeyValuePair<string, decimal>(item2.DateDate, item2.Hazineh));
                }
            }

            DarAmadClusteredChart1.DataContext = chartvalueDarAmad;
            GozareshHazinehLBL10.Content = Daramaddd.ToString("N0");
            GozareshHazinehLBL11.Content = (Hazinehhh).ToString("N0");
            GozareshHazinehLBL12.Content = (Daramaddd - Hazinehhh).ToString("N0");




            //            catch (Exception error)
            //{
            //    SaveError(error);
            //}
        }

        private void PersianCalendarGozareshHazineh2_Click(object sender, RoutedEventArgs e)// دکمه شروع تاریخ گزارش مانده
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL8.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh3_Click(object sender, RoutedEventArgs e) // دکمه تاریخ پایان گزارش
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL9.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void KharjKardBut9_Click(object sender, RoutedEventArgs e) // دکمه سمت راست:گزارش موجودی
        {
            try
            {

                Toolbarinvisible();
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                GozareshHazinehDaramad2Panel.Visibility = Visibility.Visible; GozareshHazinehDaramad2Panel.IsEnabled = true;
                KharjKardLeftToolbarProfileVisible();
                PersianCalendar pc = new PersianCalendar();
                DateTime startDate = DateTime.Now;

                // تبدیل میلادی به هجری شمسی
                GozareshHazinehLBL8.Content = GozareshHazinehLBL9.Content = pc.GetYear(startDate).ToString() + "/" + pc.GetMonth(startDate).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(startDate).ToString().PadLeft(2, '0');

                GozareshHazinehCombo1.SelectedIndex = 0;
                GozareshHazinehCombo3.SelectedIndex = 0;

                var Fin = from p in _FamilyManaerDBEntities.FinancialTbls
                          group p by p.Deposite into g
                          select new
                          {
                              sepordeh = g.FirstOrDefault().Deposite
                          };
                foreach (var item in Fin)
                {
                    GozareshHazinehCombo1.Items.Add(item.sepordeh);
                }
            }

            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarModiriatChckBut2_Click(object sender, RoutedEventArgs e) //انتخاب تاریخ برنامه غذایی
        {
            try
            {

                LBL44.Content = null;
                LBL22.Content = null;
                GozareshHazinehCombo10.Visibility = Visibility.Hidden;

                PersianCalendarModiriatChckBut2.Visibility = Visibility.Hidden;
                PersianCalendar PC = new PersianCalendar();
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                LBL1.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                LBL2.Content = PC.GetYear(Date.GDate.AddDays(1)) + "/" + PC.GetMonth(Date.GDate.AddDays(1)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(1))).ToString().PadLeft(2, '0');
                LBL3.Content = PC.GetYear(Date.GDate.AddDays(2)) + "/" + PC.GetMonth(Date.GDate.AddDays(2)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(2))).ToString().PadLeft(2, '0');
                LBL4.Content = PC.GetYear(Date.GDate.AddDays(3)) + "/" + PC.GetMonth(Date.GDate.AddDays(3)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(3))).ToString().PadLeft(2, '0');
                LBL5.Content = PC.GetYear(Date.GDate.AddDays(4)) + "/" + PC.GetMonth(Date.GDate.AddDays(4)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(4))).ToString().PadLeft(2, '0');
                LBL6.Content = PC.GetYear(Date.GDate.AddDays(5)) + "/" + PC.GetMonth(Date.GDate.AddDays(5)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(5))).ToString().PadLeft(2, '0');
                LBL7.Content = PC.GetYear(Date.GDate.AddDays(6)) + "/" + PC.GetMonth(Date.GDate.AddDays(6)).ToString().PadLeft(2, '0') + "/" + PC.GetDayOfMonth((Date.GDate.AddDays(6))).ToString().PadLeft(2, '0');

                toggleButtonModiriatChck1.IsEnabled = true;
                toggleButtonModiriatChck2.IsEnabled = true;
                toggleButtonModiriatChck3.IsEnabled = true;
                toggleButtonModiriatChck4.IsEnabled = true;
                toggleButtonModiriatChck5.IsEnabled = true;
                toggleButtonModiriatChck6.IsEnabled = true;
                toggleButtonModiriatChck7.IsEnabled = true;
                toggleButtonModiriatChck1.Visibility = Visibility.Visible;
                toggleButtonModiriatChck2.Visibility = Visibility.Visible;
                toggleButtonModiriatChck3.Visibility = Visibility.Visible;
                toggleButtonModiriatChck4.Visibility = Visibility.Visible;
                toggleButtonModiriatChck5.Visibility = Visibility.Visible;
                toggleButtonModiriatChck6.Visibility = Visibility.Visible;
                toggleButtonModiriatChck7.Visibility = Visibility.Visible;
                ModiriatChckTextBox10.Visibility = Visibility.Visible;
                ModiriatChckTextBox11.Visibility = Visibility.Visible;
                ModiriatChckTextBox16.Visibility = Visibility.Visible;
                ModiriatChckTextBox12.Visibility = Visibility.Visible;
                ModiriatChckTextBox13.Visibility = Visibility.Visible;
                ModiriatChckTextBox14.Visibility = Visibility.Visible;
                ModiriatChckTextBox15.Visibility = Visibility.Visible;
                ModiriatChckTextBox10.IsEnabled = false;
                ModiriatChckTextBox11.IsEnabled = false;
                ModiriatChckTextBox16.IsEnabled = false;
                ModiriatChckTextBox12.IsEnabled = false;
                ModiriatChckTextBox13.IsEnabled = false;
                ModiriatChckTextBox14.IsEnabled = false;
                ModiriatChckTextBox15.IsEnabled = false;

                var date1 = Par._DateTimeVariableStart.Value.AddDays(1);
                var date2 = Par._DateTimeVariableStart.Value.AddDays(2);
                var date3 = Par._DateTimeVariableStart.Value.AddDays(3);
                var date4 = Par._DateTimeVariableStart.Value.AddDays(4);
                var date5 = Par._DateTimeVariableStart.Value.AddDays(5);
                var date6 = Par._DateTimeVariableStart.Value.AddDays(6);

                var Fin = from a in _FamilyManaerDBEntities.GhazaBarnamehTbls
                          where a.Vade == GozareshHazinehCombo10.Text
                          select a;

                var ispersent1 = Fin.FirstOrDefault(x => x.Gdate == Par._DateTimeVariableStart);
                var ispersent2 = Fin.FirstOrDefault(x => x.Gdate == date1);
                var ispersent3 = Fin.FirstOrDefault(x => x.Gdate == date2);
                var ispersent4 = Fin.FirstOrDefault(x => x.Gdate == date3);
                var ispersent5 = Fin.FirstOrDefault(x => x.Gdate == date4);
                var ispersent6 = Fin.FirstOrDefault(x => x.Gdate == date5);
                var ispersent7 = Fin.FirstOrDefault(x => x.Gdate == date6);

                if (ispersent1 != null)
                {
                    ModiriatChckTextBox10.Text = ispersent1.Nafar.ToString();
                    LBL11.Content = ispersent1.Onvan;
                }
                if (ispersent2 != null)
                {
                    ModiriatChckTextBox11.Text = ispersent2.Nafar.ToString();
                    LBL22.Content = ispersent2.Onvan;
                }
                if (ispersent3 != null)
                {
                    ModiriatChckTextBox16.Text = ispersent3.Nafar.ToString();
                    LBL33.Content = ispersent3.Onvan;
                }
                if (ispersent4 != null)
                {
                    ModiriatChckTextBox12.Text = ispersent4.Nafar.ToString();
                    LBL44.Content = ispersent4.Onvan;
                }
                if (ispersent5 != null)
                {
                    ModiriatChckTextBox13.Text = ispersent5.Nafar.ToString();
                    LBL55.Content = ispersent5.Onvan;
                }
                if (ispersent6 != null)
                {
                    ModiriatChckTextBox14.Text = ispersent6.Nafar.ToString();
                    LBL66.Content = ispersent6.Onvan;
                }
                if (ispersent7 != null)
                {
                    ModiriatChckTextBox15.Text = ispersent7.Nafar.ToString();
                    LBL77.Content = ispersent7.Onvan;
                }


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        public string AyaMavadGhazaKafie(string itemFinGhazaName, int Nafarat)
        {
            decimal MojodiNafar = 10000;
            var FinMavadGhaza = from MavadGhaza in _FamilyManaerDBEntities.MavadGhzaNameTbls
                                where MavadGhaza.NameGhaza == itemFinGhazaName

                                select MavadGhaza;
            foreach (var itemFinMavadGhaza in FinMavadGhaza)
            {
                var FinGheimatGhaza = from GheimatGhaza in _FamilyManaerDBEntities.FinancialTbls
                                      where GheimatGhaza.Title == itemFinMavadGhaza.NameMavad
                                      orderby GheimatGhaza.Datee descending
                                      select GheimatGhaza;



                var FinMojodiNafar = from m in _FamilyManaerDBEntities.MojodiKalaTbls
                                     where m.Onvan == itemFinMavadGhaza.NameMavad
                                     select m;
                foreach (var itemFinMojodiNafar in FinMojodiNafar)
                {
                    if (MojodiNafar > itemFinMojodiNafar.Meghdar.Value / itemFinMavadGhaza.Meghdar.Value)
                    {
                        MojodiNafar = itemFinMojodiNafar.Meghdar.Value / itemFinMavadGhaza.Meghdar.Value;

                    }
                }

            }
            if (MojodiNafar < Nafarat)
            {
                var result = MajMessageBox.show("موجودی مواد اولیه برای این تعداد نفرات کفایت نمی کند." + Environment.NewLine + "آیا این برنامه غذایی بدون کسر مواد اولیه ذخیره گردد؟", MajMessageBox.MajMessageBoxBut.YESNO);
                return result.ToString();

            }
            else
            {
                return "Ok";
            }

        }
        private void toggleButtonModiriatChck1_Checked(object sender, RoutedEventArgs e)
        {



            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox10.IsEnabled = true;
            ModiriatChckTextBox11.IsEnabled = false;
            ModiriatChckTextBox16.IsEnabled = false;
            ModiriatChckTextBox12.IsEnabled = false;
            ModiriatChckTextBox13.IsEnabled = false;
            ModiriatChckTextBox14.IsEnabled = false;
            ModiriatChckTextBox15.IsEnabled = false;

        }

        private void CoockRightToolbarBut9_Click(object sender, RoutedEventArgs e) // دکمه سمت راست : برنامه غذا
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            BarnamehGhazaPanel.Visibility = Visibility.Visible; BarnamehGhazaPanel.IsEnabled = true;

            ModiriatChckTextBox8.Text = string.Empty;
            EmptyPar();
            LBL1.Content = null;
            LBL2.Content = null;
            LBL3.Content = null;
            LBL4.Content = null;
            LBL5.Content = null;
            LBL6.Content = null;
            LBL7.Content = null;
            LBL11.Content = null;
            LBL22.Content = null;
            LBL33.Content = null;
            LBL44.Content = null;
            LBL55.Content = null;
            LBL66.Content = null;
            LBL77.Content = null;
            ModiriatChckTextBox10.Text = ModiriatChckTextBox11.Text = ModiriatChckTextBox12.Text = ModiriatChckTextBox13.Text = ModiriatChckTextBox14.Text = ModiriatChckTextBox15.Text = ModiriatChckTextBox16.Text = string.Empty;


            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            toggleButtonModiriatChck1.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck2.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck3.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck4.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck5.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck6.Visibility = Visibility.Hidden;
            toggleButtonModiriatChck7.Visibility = Visibility.Hidden;
            ModiriatChckTextBox10.Visibility = Visibility.Hidden;
            ModiriatChckTextBox11.Visibility = Visibility.Hidden;
            ModiriatChckTextBox16.Visibility = Visibility.Hidden;
            ModiriatChckTextBox12.Visibility = Visibility.Hidden;
            ModiriatChckTextBox13.Visibility = Visibility.Hidden;
            ModiriatChckTextBox14.Visibility = Visibility.Hidden;
            ModiriatChckTextBox15.Visibility = Visibility.Hidden;
            ModiriatChckTextBox10.IsEnabled = false;
            ModiriatChckTextBox11.IsEnabled = false;
            ModiriatChckTextBox16.IsEnabled = false;
            ModiriatChckTextBox12.IsEnabled = false;
            ModiriatChckTextBox13.IsEnabled = false;
            ModiriatChckTextBox14.IsEnabled = false;
            ModiriatChckTextBox15.IsEnabled = false;
            PersianCalendarModiriatChckBut2.Visibility = Visibility.Visible;
            LBL44.Content = "لطفاً تاریخ را انتخاب کنید";
            LBL22.Content = "وعده غذایی : ";
            GozareshHazinehCombo10.Visibility = Visibility.Visible;
            GozareshHazinehCombo10.SelectedIndex = 0;


            CreateEnTekhabGhazaTbl();
        }
        public void TaghirMojodiKala(string NoAmaliat)
        {

            var FinMavadGhaza = from MavadGhaza in _FamilyManaerDBEntities.MavadGhzaNameTbls
                                where MavadGhaza.NameGhaza == Par.FoodName
                                select MavadGhaza;
            foreach (var itemFinMavadGhaza in FinMavadGhaza)
            {
                var isperesent = _FamilyManaerDBEntities.MojodiKalaTbls.FirstOrDefault(_ => _.Onvan == itemFinMavadGhaza.NameMavad);
                if (NoAmaliat == "اضافه")
                {
                    isperesent.Meghdar = isperesent.Meghdar + Par.Nafarat * itemFinMavadGhaza.Meghdar;
                    _FamilyManaerDBEntities.SaveChanges();
                }
                else if (NoAmaliat == "کسر")
                {
                    if (isperesent.Meghdar - Par.Nafarat * itemFinMavadGhaza.Meghdar >= 0)
                    {
                        isperesent.Meghdar = isperesent.Meghdar - Par.Nafarat * itemFinMavadGhaza.Meghdar;

                    }
                    else
                    {
                        isperesent.Meghdar = 0;
                    }
                    _FamilyManaerDBEntities.SaveChanges();

                }
            }
        }

        private void toggleButtonModiriatChck1_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL11, LBL1, ModiriatChckTextBox10, 0);

        }

        private void toggleButtonModiriatChck2_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL22, LBL2, ModiriatChckTextBox11, 1);
        }

        private void toggleButtonModiriatChck3_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL33, LBL3, ModiriatChckTextBox16, 2);
        }

        private void toggleButtonModiriatChck4_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL44, LBL4, ModiriatChckTextBox12, 3);
        }

        private void toggleButtonModiriatChck5_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL55, LBL5, ModiriatChckTextBox13, 4);
        }

        private void toggleButtonModiriatChck6_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL66, LBL6, ModiriatChckTextBox14, 5);
        }

        private void toggleButtonModiriatChck7_Unchecked(object sender, RoutedEventArgs e)
        {
            BarnamehGhaza(LBL77, LBL7, ModiriatChckTextBox15, 6);
        }

        private void toggleButtonModiriatChck2_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox11.IsEnabled = true;
        }

        private void toggleButtonModiriatChck3_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox16.IsEnabled = true;
        }

        private void toggleButtonModiriatChck4_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox12.IsEnabled = true;
        }

        private void toggleButtonModiriatChck5_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox13.IsEnabled = true;
        }

        private void toggleButtonModiriatChck6_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck7.IsChecked = false;
            ModiriatChckTextBox14.IsEnabled = true;
        }

        private void toggleButtonModiriatChck7_Checked(object sender, RoutedEventArgs e)
        {
            toggleButtonModiriatChck1.IsChecked = false;
            toggleButtonModiriatChck2.IsChecked = false;
            toggleButtonModiriatChck3.IsChecked = false;
            toggleButtonModiriatChck4.IsChecked = false;
            toggleButtonModiriatChck5.IsChecked = false;
            toggleButtonModiriatChck6.IsChecked = false;
            ModiriatChckTextBox15.IsEnabled = true;
        }
        public decimal GheimatGhaza(string ghazaname)
        {
            decimal FoodCostt = 0;

            var FinGhazaName = from GhazaName in _FamilyManaerDBEntities.GhzaNameTbls
                               where GhazaName.Name == ghazaname
                               select GhazaName;
            foreach (var itemFinGhazaName in FinGhazaName)
            {
                var FinMavadGhaza = from MavadGhaza in _FamilyManaerDBEntities.MavadGhzaNameTbls
                                    where MavadGhaza.NameGhaza == itemFinGhazaName.Name

                                    select MavadGhaza;
                foreach (var itemFinMavadGhaza in FinMavadGhaza)
                {
                    var FinGheimatGhaza = from GheimatGhaza in _FamilyManaerDBEntities.FinancialTbls
                                          where GheimatGhaza.Title == itemFinMavadGhaza.NameMavad
                                          orderby GheimatGhaza.Datee descending
                                          select GheimatGhaza;
                    //var FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.Header == itemFinMavadGhaza.NameMavad);

                    //if (FinCaleyGhaza == null)
                    //{
                    //    FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubHeader == itemFinMavadGhaza.NameMavad);
                    //    if (FinCaleyGhaza == null)
                    //    {
                    //        FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubSubHeader == itemFinMavadGhaza.NameMavad);
                    //    }
                    //}
                    //FoodCall += (FinCaleyGhaza.IekCallery).Value * itemFinMavadGhaza.Meghdar.Value;
                    int i = 0;
                    foreach (var itemFinGheimatGhaza in FinGheimatGhaza)
                    {


                        if (i == 0)
                        {
                            FoodCostt += (itemFinGheimatGhaza.Cost).Value * itemFinMavadGhaza.Meghdar.Value;
                        }
                        i++;
                    }

                }
            }
            return FoodCostt;

        }
        public void CreateEnTekhabGhazaTbl()
        {
            ModiriatChckGrid1.Items.Clear();
            //if (string.IsNullOrEmpty(ModiriatChckTextBox8.Text))
            //{


            //    var FinGhazaName = from GhazaName in _FamilyManaerDBEntities.GhzaNameTbls
            //                       select GhazaName;
            //    foreach (var itemFinGhazaName in FinGhazaName)
            //    {
            //        decimal FoodCall = 0, FoodCostt = 0;
            //        var FinMavadGhaza = from MavadGhaza in _FamilyManaerDBEntities.MavadGhzaNameTbls
            //                            where MavadGhaza.NameGhaza == itemFinGhazaName.Name

            //                            select MavadGhaza;
            //        foreach (var itemFinMavadGhaza in FinMavadGhaza)
            //        {
            //            var FinGheimatGhaza = from GheimatGhaza in _FamilyManaerDBEntities.FinancialTbls
            //                                  where GheimatGhaza.Title == itemFinMavadGhaza.NameMavad
            //                                  orderby GheimatGhaza.Datee descending
            //                                  select GheimatGhaza;
            //            var FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.Header == itemFinMavadGhaza.NameMavad);
            //            if (FinCaleyGhaza == null)
            //            {
            //                FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubHeader == itemFinMavadGhaza.NameMavad);
            //                if (FinCaleyGhaza == null)
            //                {
            //                    FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubSubHeader == itemFinMavadGhaza.NameMavad);
            //                }
            //            }
            //            FoodCall += (FinCaleyGhaza.IekCallery).Value * itemFinMavadGhaza.Meghdar.Value;
            //            int i = 0;
            //            foreach (var itemFinGheimatGhaza in FinGheimatGhaza)
            //            {


            //                if (i == 0)
            //                {
            //                    FoodCostt += (itemFinGheimatGhaza.Cost).Value * itemFinMavadGhaza.Meghdar.Value;
            //                }
            //                i++;
            //            }

            //        }
            //        ModiriatChckGrid1.Items.Add(new
            //        {
            //            Foodname = itemFinGhazaName.Name,
            //            FoodCal = FoodCall.ToString("N0"),
            //            FoodCost = FoodCostt.ToString("N0")
            //        });
            //    }
            //}
            //else
            //{
            var FinGhazaName = from GhazaName in _FamilyManaerDBEntities.GhzaNameTbls
                                   //where GhazaName.Name.Contains(ModiriatChckTextBox8.Text) || GhazaName.Name.Contains(string.Empty)
                                   // where GhazaName.Name.Contains()

                               select GhazaName;
            foreach (var itemFinGhazaName in FinGhazaName)
            {
                decimal FoodCall = 0, FoodCostt = 0, MojodiNafar = 10000;
                var FinMavadGhaza = from MavadGhaza in _FamilyManaerDBEntities.MavadGhzaNameTbls
                                    where MavadGhaza.NameGhaza == itemFinGhazaName.Name

                                    select MavadGhaza;
                foreach (var itemFinMavadGhaza in FinMavadGhaza)
                {
                    var FinGheimatGhaza = from GheimatGhaza in _FamilyManaerDBEntities.FinancialTbls
                                          where GheimatGhaza.Title == itemFinMavadGhaza.NameMavad
                                          orderby GheimatGhaza.Datee descending
                                          select GheimatGhaza;
                    var FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.Header == itemFinMavadGhaza.NameMavad);

                    if (FinCaleyGhaza == null)
                    {
                        FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubHeader == itemFinMavadGhaza.NameMavad);
                        if (FinCaleyGhaza == null)
                        {
                            FinCaleyGhaza = _FamilyManaerDBEntities.TreeKalas.FirstOrDefault(x => x.SubSubHeader == itemFinMavadGhaza.NameMavad);
                        }
                    }
                    FoodCall += (FinCaleyGhaza.IekCallery).Value * itemFinMavadGhaza.Meghdar.Value;
                    int i = 0;
                    foreach (var itemFinGheimatGhaza in FinGheimatGhaza)
                    {


                        if (i == 0)
                        {
                            FoodCostt += (itemFinGheimatGhaza.Cost).Value * itemFinMavadGhaza.Meghdar.Value;
                        }
                        i++;
                    }
                    var FinMojodiNafar = from m in _FamilyManaerDBEntities.MojodiKalaTbls
                                         where m.Onvan == itemFinMavadGhaza.NameMavad
                                         select m;
                    foreach (var itemFinMojodiNafar in FinMojodiNafar)
                    {
                        if (MojodiNafar > itemFinMojodiNafar.Meghdar.Value / itemFinMavadGhaza.Meghdar.Value)
                        {
                            MojodiNafar = itemFinMojodiNafar.Meghdar.Value / itemFinMavadGhaza.Meghdar.Value;

                        }
                    }

                }
                MojodiNafar = Math.Round(MojodiNafar, 0);
                ModiriatChckGrid1.Items.Add(new
                {
                    MojodiNafar = MojodiNafar,
                    Foodname = itemFinGhazaName.Name,
                    FoodCal = FoodCall.ToString("N0"),
                    FoodCost = FoodCostt.ToString("N0")
                });
            }
            //}
        }
        public void BarnamehGhaza(System.Windows.Controls.Label FoodName, System.Windows.Controls.Label Tarikh, System.Windows.Controls.TextBox Nafarat, int day)
        {
            try
            {

                var date = Par._DateTimeVariableStart.Value.AddDays(day);
                var ispersent = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date);

                if ((FoodName.Content == null) && (ispersent == null))
                {
                    MajMessageBox.show("لطفاً نام غذا را مشخص کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    Nafarat.IsEnabled = false;
                    return;
                }
                if ((Nafarat.Text == "0") || (string.IsNullOrEmpty(Nafarat.Text)) && (ispersent == null))
                {
                    MajMessageBox.show("لطفاً تعداد نفرات را مشخص کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    Nafarat.IsEnabled = false;
                    return;
                }
                GhazaBarnamehTbl _GhazaBarnamehTbl = new GhazaBarnamehTbl();

                if (ispersent != null)
                {
                    Par.FoodName = ispersent.Onvan;
                    Par.Nafarat = ispersent.Nafar.Value;
                }

                if ((ispersent != null) && (string.IsNullOrEmpty(Nafarat.Text) && (FoodName.Content == null))) // حذف عنوان غذا

                {
                    if (ispersent.KasrMavad != "Yes") // مواد غذایی برگشت داده شود
                    {
                        TaghirMojodiKala("اضافه");
                    }
                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent);
                    _FamilyManaerDBEntities.SaveChanges();
                    EmptyPar();
                    Nafarat.IsEnabled = false;
                    CreateEnTekhabGhazaTbl();
                    return;

                }

                if ((ispersent != null) && (Par.FoodName != FoodName.Content.ToString()) && (FoodName.Content != null) && (!string.IsNullOrEmpty(Nafarat.Text))) // تغییر عنوان غذایی
                {
                    if (ispersent.KasrMavad != "Yes") // مواد غذایی برگشت داده شود
                    {
                        TaghirMojodiKala("اضافه");
                    }
                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent);
                    _FamilyManaerDBEntities.SaveChanges();
                    ispersent = null;
                }



                if ((ispersent != null) && (Par.FoodName == ispersent.Onvan) && (Par.Nafarat != int.Parse(Nafarat.Text))) // تغییر نفرات

                {

                    if (ispersent.KasrMavad != "Yes") // مواد غذایی برگشت داده شود
                    {
                        TaghirMojodiKala("اضافه");
                    }
                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent);
                    _FamilyManaerDBEntities.SaveChanges();
                    ispersent = null;

                }

                if (ispersent == null)
                {
                    Par.FoodName = FoodName.Content.ToString();
                    Par.Nafarat = int.Parse(Nafarat.Text);
                    string result = (AyaMavadGhazaKafie(Par.FoodName, Par.Nafarat));


                    if (result == "Yes")  // غذا از موجودی کسر نشود
                    {
                        _GhazaBarnamehTbl.Nafar = Par.Nafarat;
                        _GhazaBarnamehTbl.Onvan = Par.FoodName;
                        _GhazaBarnamehTbl.Gdate = date;
                        _GhazaBarnamehTbl.PersainDate = Tarikh.Content.ToString();
                        _GhazaBarnamehTbl.Vade = GozareshHazinehCombo10.Text;
                        _GhazaBarnamehTbl.KasrMavad = result;
                        _GhazaBarnamehTbl.Gheimat = GheimatGhaza(FoodName.Content.ToString());
                        _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                    if (result == "Ok") // عنوان غذا ذخیره شود و موجودی غذا کسر شود.
                    {
                        _GhazaBarnamehTbl.Nafar = Par.Nafarat;
                        _GhazaBarnamehTbl.Onvan = Par.FoodName;
                        _GhazaBarnamehTbl.Gdate = date;
                        _GhazaBarnamehTbl.KasrMavad = result;
                        _GhazaBarnamehTbl.PersainDate = Tarikh.Content.ToString();
                        _GhazaBarnamehTbl.Vade = GozareshHazinehCombo10.Text;
                        _GhazaBarnamehTbl.Gheimat = GheimatGhaza(FoodName.Content.ToString());
                        _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
                        _FamilyManaerDBEntities.SaveChanges();
                        TaghirMojodiKala("کسر");
                    }
                    else if (result == "No") // غذا ذخیره نشود
                    {
                        FoodName.Content = null;
                        Nafarat.Text = string.Empty;
                    }
                }



                Nafarat.IsEnabled = false;
            }
            catch (Exception error)
            {
                SaveError(error);
            }
            EmptyPar();
            CreateEnTekhabGhazaTbl();
        }
        private void ModiriatChckBut6_Click(object sender, RoutedEventArgs e) // پاک کردن پنل جدول غذایی
        {

            CoockRightToolbarBut9_Click(sender, e);

        }

        private void ModiriatChckGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e) // انتخاب غذا از گرید غذا
        {
            var item = ModiriatChckGrid1.SelectedItem;

            if (item != null)
            {
                string FoodName = (ModiriatChckGrid1.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                if (toggleButtonModiriatChck1.IsChecked == true)
                {
                    LBL11.Content = FoodName;
                }
                else if (toggleButtonModiriatChck2.IsChecked == true)
                {
                    LBL22.Content = FoodName;
                }
                else if (toggleButtonModiriatChck3.IsChecked == true)
                {
                    LBL33.Content = FoodName;

                }
                else if (toggleButtonModiriatChck4.IsChecked == true)
                {
                    LBL44.Content = FoodName;

                }
                else if (toggleButtonModiriatChck5.IsChecked == true)
                {
                    LBL55.Content = FoodName;

                }
                else if (toggleButtonModiriatChck6.IsChecked == true)
                {
                    LBL66.Content = FoodName;

                }
                else if (toggleButtonModiriatChck7.IsChecked == true)
                {
                    LBL77.Content = FoodName;

                }
                else
                {
                    MajMessageBox.show("تاریخ مد نظر خود را انتخاب نمایید.", MajMessageBox.MajMessageBoxBut.OK); return;
                }
                ModiriatChckGrid1.SelectedItem = null;
            }
        }

        private void ModiriatChckTextBox10_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox10.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox10.Text = string.Format("{0}", number);
                    ModiriatChckTextBox10.SelectionStart = ModiriatChckTextBox10.Text.Length;
                    if (int.Parse(ModiriatChckTextBox10.Text) > 99999) { ModiriatChckTextBox10.Text = "99999"; }
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox11_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox11.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox11.Text = string.Format("{0}", number);
                    ModiriatChckTextBox11.SelectionStart = ModiriatChckTextBox11.Text.Length;
                    if (int.Parse(ModiriatChckTextBox11.Text) > 99999) { ModiriatChckTextBox11.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox16_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox16.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox16.Text = string.Format("{0}", number);
                    ModiriatChckTextBox16.SelectionStart = ModiriatChckTextBox16.Text.Length;
                    if (int.Parse(ModiriatChckTextBox16.Text) > 99999) { ModiriatChckTextBox16.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox12_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox12.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox12.Text = string.Format("{0}", number);
                    ModiriatChckTextBox12.SelectionStart = ModiriatChckTextBox12.Text.Length;
                    if (int.Parse(ModiriatChckTextBox12.Text) > 99999) { ModiriatChckTextBox12.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox13_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox13.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox13.Text = string.Format("{0}", number);
                    ModiriatChckTextBox13.SelectionStart = ModiriatChckTextBox13.Text.Length;
                    if (int.Parse(ModiriatChckTextBox13.Text) > 99999) { ModiriatChckTextBox13.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox14_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox14.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox14.Text = string.Format("{0}", number);
                    ModiriatChckTextBox14.SelectionStart = ModiriatChckTextBox14.Text.Length;
                    if (int.Parse(ModiriatChckTextBox14.Text) > 99999) { ModiriatChckTextBox14.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckTextBox15_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(ModiriatChckTextBox15.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    ModiriatChckTextBox15.Text = string.Format("{0}", number);
                    ModiriatChckTextBox15.SelectionStart = ModiriatChckTextBox15.Text.Length;
                    if (int.Parse(ModiriatChckTextBox15.Text) > 99999) { ModiriatChckTextBox15.Text = "99999"; }

                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatChckBut7_Click(object sender, RoutedEventArgs e)// دکمه پاک کردن آیتم های انتخاب غذا
        {
            if (toggleButtonModiriatChck1.IsChecked == true)
            {
                Par.FoodName = LBL11.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox10.Text);
                LBL11.Content = null;
                ModiriatChckTextBox10.Text = string.Empty;
            }
            else if (toggleButtonModiriatChck2.IsChecked == true)
            {
                Par.FoodName = LBL22.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox11.Text);
                LBL22.Content = null;
                ModiriatChckTextBox11.Text = string.Empty;

            }
            else if (toggleButtonModiriatChck3.IsChecked == true)
            {
                Par.FoodName = LBL33.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox16.Text);
                LBL33.Content = null;
                ModiriatChckTextBox16.Text = string.Empty;


            }
            else if (toggleButtonModiriatChck4.IsChecked == true)
            {
                Par.FoodName = LBL44.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox12.Text);
                LBL44.Content = null;
                ModiriatChckTextBox12.Text = string.Empty;


            }
            else if (toggleButtonModiriatChck5.IsChecked == true)
            {
                Par.FoodName = LBL55.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox13.Text);
                LBL55.Content = null;
                ModiriatChckTextBox13.Text = string.Empty;


            }
            else if (toggleButtonModiriatChck6.IsChecked == true)
            {
                Par.FoodName = LBL66.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox14.Text);
                LBL66.Content = null;
                ModiriatChckTextBox14.Text = string.Empty;

            }
            else if (toggleButtonModiriatChck7.IsChecked == true)
            {
                Par.FoodName = LBL77.Content.ToString();
                Par.Nafarat = int.Parse(ModiriatChckTextBox15.Text);
                LBL77.Content = null;
                ModiriatChckTextBox15.Text = string.Empty;

            }
            else
            {
                MajMessageBox.show("تاریخ مد نظر خود را انتخاب نمایید.", MajMessageBox.MajMessageBoxBut.OK); return;
            }
        }

        private void ModiriatChckBut5_Click(object sender, RoutedEventArgs e) // دکمه ذخیره برنامه غذایی
        {
            //        try
            //        {
            //            GhazaBarnamehTbl _GhazaBarnamehTbl = new GhazaBarnamehTbl();
            //            //if (PersianCalendarModiriatChckBut2.Visibility!=Visibility.Hidden)
            //            //{
            //            //    { MajMessageBox.show("لطفاً تاریخ را مشخص کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

            //            //}
            //            var date22 = Par._DateTimeVariableStart.Value.AddDays(1);
            //            var date33 = Par._DateTimeVariableStart.Value.AddDays(2);
            //            var date44 = Par._DateTimeVariableStart.Value.AddDays(3);
            //            var date55 = Par._DateTimeVariableStart.Value.AddDays(4);
            //            var date66 = Par._DateTimeVariableStart.Value.AddDays(5);
            //            var date77 = Par._DateTimeVariableStart.Value.AddDays(6);

            //            var ispersent22 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date22);
            //            var ispersent33 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date33);
            //            var ispersent44 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date44);
            //            var ispersent55 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date55);
            //            var ispersent66 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date66);
            //            var ispersent77 = _FamilyManaerDBEntities.GhazaBarnamehTbls.FirstOrDefault(x => x.Gdate == date77);
            //        if (LBL22.Content != null)
            //        {
            //            if (ispersent22 != null)
            //            {
            //                ispersent22.Nafar = int.Parse(ModiriatChckTextBox11.Text);
            //                ispersent22.Onvan = LBL22.Content.ToString();
            //                _FamilyManaerDBEntities.SaveChanges();
            //            }
            //            else
            //            {
            //                _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox11.Text);
            //                _GhazaBarnamehTbl.Onvan = LBL22.Content.ToString();
            //                _GhazaBarnamehTbl.Gdate = date22;
            //                _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                _FamilyManaerDBEntities.SaveChanges();

            //            }

            //            else
            //            {
            //                if (ispersent22 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent22);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }
            //            if (LBL33.Content != null)
            //            {
            //                if (ispersent33 != null)
            //                {
            //                    ispersent33.Nafar = int.Parse(ModiriatChckTextBox16.Text);
            //                    ispersent33.Onvan = LBL33.Content.ToString();
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //                else
            //                {
            //                    _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox16.Text);
            //                    _GhazaBarnamehTbl.Onvan = LBL33.Content.ToString();
            //                    _GhazaBarnamehTbl.Gdate = date33;
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                    _FamilyManaerDBEntities.SaveChanges();

            //                }
            //            }
            //            else
            //            {
            //                if (ispersent33 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent33);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }
            //            if (LBL44.Content != null)
            //            {
            //                if (ispersent44 != null)
            //                {
            //                    ispersent44.Nafar = int.Parse(ModiriatChckTextBox12.Text);
            //                    ispersent44.Onvan = LBL44.Content.ToString();
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //                else
            //                {
            //                    _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox12.Text);
            //                    _GhazaBarnamehTbl.Onvan = LBL44.Content.ToString();
            //                    _GhazaBarnamehTbl.Gdate = date44;
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                    _FamilyManaerDBEntities.SaveChanges();

            //                }
            //            }
            //            else
            //            {
            //                if (ispersent44 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent44);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }
            //            if (LBL55.Content != null)
            //            {
            //                if (ispersent55 != null)
            //                {
            //                    ispersent55.Nafar = int.Parse(ModiriatChckTextBox13.Text);
            //                    ispersent55.Onvan = LBL55.Content.ToString();
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //                else
            //                {
            //                    _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox13.Text);
            //                    _GhazaBarnamehTbl.Onvan = LBL55.Content.ToString();
            //                    _GhazaBarnamehTbl.Gdate = date55;
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                    _FamilyManaerDBEntities.SaveChanges();

            //                }
            //            }
            //            else
            //            {
            //                if (ispersent55 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent55);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }
            //            if (LBL66.Content != null)
            //            {
            //                if (ispersent66 != null)
            //                {
            //                    ispersent66.Nafar = int.Parse(ModiriatChckTextBox14.Text);
            //                    ispersent66.Onvan = LBL66.Content.ToString();
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //                else
            //                {
            //                    _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox14.Text);
            //                    _GhazaBarnamehTbl.Onvan = LBL66.Content.ToString();
            //                    _GhazaBarnamehTbl.Gdate = date66;
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                    _FamilyManaerDBEntities.SaveChanges();

            //                }
            //            }
            //            else
            //            {
            //                if (ispersent66 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent66);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }
            //            if (LBL77.Content != null)
            //            {
            //                if (ispersent77 != null)
            //                {
            //                    ispersent77.Nafar = int.Parse(ModiriatChckTextBox15.Text);
            //                    ispersent77.Onvan = LBL77.Content.ToString();
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //                else
            //                {
            //                    _GhazaBarnamehTbl.Nafar = int.Parse(ModiriatChckTextBox15.Text);
            //                    _GhazaBarnamehTbl.Onvan = LBL77.Content.ToString();
            //                    _GhazaBarnamehTbl.Gdate = date77;
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Add(_GhazaBarnamehTbl);
            //                    _FamilyManaerDBEntities.SaveChanges();

            //                }
            //            }
            //            else
            //            {
            //                if (ispersent77 != null)
            //                {
            //                    _FamilyManaerDBEntities.GhazaBarnamehTbls.Remove(ispersent77);
            //                    _FamilyManaerDBEntities.SaveChanges();
            //                }
            //            }



            //            ModiriatChckBut6_Click(sender, e);
            //            MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
            //            CleanOldDataEnteredTXT();
            //        EmptyPar();
            //    }
            //        catch (Exception error) { SaveError(error);
            //}
        }

        private void ModiriatChckTextBox8_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateEnTekhabGhazaTbl();
        }

        private void MojodiGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // انتخاب از موجودی غذا
        {
            var item = MojodiGrid.SelectedItem;

            if (item != null)
            {
                MojodiTextBox1.Text = (MojodiGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var ispresent = _FamilyManaerDBEntities.MojodiKalaTbls.FirstOrDefault(_ => _.Onvan == MojodiTextBox1.Text);
                MojodiTextBox3.Text = ispresent.Vahed;
            }
        }

        private void SabteMavadGhazaTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(SabteMavadGhazaTextBox3.Text, out number))
                {
                    if (number >= 999999)
                    {
                        number = 999999;
                    }
                    SabteMavadGhazaTextBox3.Text = string.Format("{0:N0}", number);
                    SabteMavadGhazaTextBox3.SelectionStart = SabteMavadGhazaTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void GozareshHazinehBut4_Click(object sender, RoutedEventArgs e) // گزارش نمودار غذا تعداد
        {
            if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
            {
                MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                return;

            }
            pieChartGozareshHazineh2.DataContext = null;

            List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();   // اپل پای چارت : گزارش کالری غذاها
            var Fin1 = from p in _FamilyManaerDBEntities.GhazaBarnamehTbls
                       where p.Gdate > Par._DateTimeVariableStart && p.Gdate < Par._DateTimeVariableFinish
                       group p by new { p.Onvan } into g
                       select new
                       {
                           Onvan = g.FirstOrDefault().Onvan,
                           Tedad = g.Count()

                       };
            foreach (var item1 in Fin1)
            {
                chartvalue.Add(new KeyValuePair<string, decimal>(item1.Onvan, item1.Tedad));

            }
            pieChartGozareshHazineh2.DataContext = chartvalue;



        }

        private void PersianCalendarGozareshHazineh3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL13.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL14.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void CoockRightToolbarBut10_Click(object sender, RoutedEventArgs e) // دکمه سمت راست: گزارش تعداد مصرف غذاها
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            GozareshGhazaTdadPanel.Visibility = Visibility.Visible; GozareshGhazaTdadPanel.IsEnabled = true;
        }

        private void CoockRightToolbarBut11_Click(object sender, RoutedEventArgs e) // دکمه سمت راست: کالری مصرفی
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            GozareshGhazaCaleryPanel.Visibility = Visibility.Visible; GozareshGhazaCaleryPanel.IsEnabled = true;
            GozareshHazinehCombo7.SelectedIndex = 0;
        }

        private void PersianCalendarGozareshHazineh4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL15.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL16.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshHazinehBut5_Click(object sender, RoutedEventArgs e) // دکمه گزارش کالری غذاها
        {
            if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
            {
                MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                return;

            }
            if (GozareshHazinehCombo7.Text == "روزانه")
            {
                if (Par._DateTimeVariableStart.Value.AddMonths(1) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک ماه باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            else if (GozareshHazinehCombo7.Text == "ماهانه")
            {
                if (Par._DateTimeVariableStart.Value.AddYears(1) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک سال باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            else if (GozareshHazinehCombo7.Text == "ماهانه")
            {
                if (Par._DateTimeVariableStart.Value.AddYears(100) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک قرن باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            pieChartGozareshHazineh3.DataContext = null;
            ColumnChart1.DataContext = null;

            List<KeyValuePair<string, decimal>> chartvalue2 = new List<KeyValuePair<string, decimal>>();  // کالری غذا
            List<Ghaza> CaleryList = new List<Ghaza>();
            List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();   // اپل پای چارت : گزارش کالری غذاها
            var Fin1 = from p in _FamilyManaerDBEntities.GhazaBarnamehTbls
                       where p.Gdate > Par._DateTimeVariableStart && p.Gdate < Par._DateTimeVariableFinish
                       group p by new { p.Onvan } into g
                       select new
                       {
                           Onvan = g.FirstOrDefault().Onvan,
                       };
            foreach (var item1 in Fin1)
            {
                decimal calery = 0;

                var Fin2 = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                           where p.NameGhaza == item1.Onvan
                           select p;

                foreach (var item2 in Fin2)
                {
                    var Fin3 = from p in _FamilyManaerDBEntities.TreeKalas
                               where p.Header == item2.NameMavad || p.SubHeader == item2.NameMavad || p.SubSubHeader == item2.NameMavad
                               select p;

                    foreach (var item3 in Fin3)
                    {
                        calery += item3.IekCallery.Value;
                    }

                }
                chartvalue.Add(new KeyValuePair<string, decimal>(item1.Onvan, calery));
            }
            pieChartGozareshHazineh3.DataContext = chartvalue;






            var FinGhaza = from p in _FamilyManaerDBEntities.GhazaBarnamehTbls
                           where p.Gdate > Par._DateTimeVariableStart && p.Gdate < Par._DateTimeVariableFinish
                           select p;
            foreach (var item1 in FinGhaza)
            {
                decimal calery = 0;

                var FinGhaza2 = from p in _FamilyManaerDBEntities.MavadGhzaNameTbls
                                where p.NameGhaza == item1.Onvan
                                select p;

                foreach (var item2 in FinGhaza2)
                {
                    var FinGhaza3 = from p in _FamilyManaerDBEntities.TreeKalas
                                    where p.Header == item2.NameMavad || p.SubHeader == item2.NameMavad || p.SubSubHeader == item2.NameMavad
                                    select p;

                    foreach (var item3 in FinGhaza3)
                    {
                        calery += item3.IekCallery.Value * item2.Meghdar.Value;
                    }

                }
                Ghaza _Ghaza = new Ghaza();
                _Ghaza.year = item1.PersainDate.Substring(0, 4);
                _Ghaza.month = item1.PersainDate.Substring(5, 2);
                _Ghaza.Day = item1.PersainDate.Substring(8, 2);
                _Ghaza.NameGhaza = item1.Onvan;
                _Ghaza.PersianDate = item1.PersainDate;
                _Ghaza.Calery = calery;
                CaleryList.Add(_Ghaza);
            }




            if (GozareshHazinehCombo7.Text == "روزانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.Day } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().Day,
                                Calery = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Calery));
                }
            }

            else if (GozareshHazinehCombo7.Text == "ماهانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.month } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().month,
                                Calery = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Calery));
                }
            }
            else if (GozareshHazinehCombo7.Text == "سالانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.year } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().year,
                                Calery = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Calery));
                }
            }
            GhazaTedad1.DataContext = chartvalue2;
        }

        private void PPieGozareshHazineh2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPieGozareshHazineh2.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان غذا:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPieGozareshHazineh3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPieGozareshHazineh1.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان هزینه:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void CoockRightToolbarBut12_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            CoockRightToolbarVisible();
            GozareshGhazaGheimatPanel.Visibility = Visibility.Visible; GozareshGhazaGheimatPanel.IsEnabled = true;
            GozareshHazinehCombo4.SelectedIndex = 0;
        }

        private void GozareshHazinehBut6_Click(object sender, RoutedEventArgs e)
        {
            if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
            {
                MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                return;

            }
            if (GozareshHazinehCombo4.Text == "روزانه")
            {
                if (Par._DateTimeVariableStart.Value.AddMonths(1) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک ماه باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            else if (GozareshHazinehCombo4.Text == "ماهانه")
            {
                if (Par._DateTimeVariableStart.Value.AddYears(1) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک سال باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            else if (GozareshHazinehCombo4.Text == "ماهانه")
            {
                if (Par._DateTimeVariableStart.Value.AddYears(100) < Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("بازه انتخابی شما باید کمتر از یک قرن باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
            }
            pieChartGozareshHazineh4.DataContext = null;
            ColumnChart1.DataContext = null;

            List<KeyValuePair<string, decimal>> chartvalue2 = new List<KeyValuePair<string, decimal>>();  // قیمت غذا
            List<Ghaza> CaleryList = new List<Ghaza>();
            List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();   // اپل پای چارت : گزارش قیمت غذاها
            var Fin1 = from p in _FamilyManaerDBEntities.GhazaBarnamehTbls
                       where p.Gdate > Par._DateTimeVariableStart && p.Gdate < Par._DateTimeVariableFinish
                       group p by new { p.Onvan } into g
                       select new
                       {
                           Onvan = g.FirstOrDefault().Onvan,
                           Gheimat = g.Sum(_ => _.Gheimat)
                       };
            foreach (var item1 in Fin1)
            {

                chartvalue.Add(new KeyValuePair<string, decimal>(item1.Onvan, item1.Gheimat.Value));
            }
            pieChartGozareshHazineh4.DataContext = chartvalue;






            var FinGhaza = from p in _FamilyManaerDBEntities.GhazaBarnamehTbls
                           where p.Gdate > Par._DateTimeVariableStart && p.Gdate < Par._DateTimeVariableFinish
                           select p;
            foreach (var item1 in FinGhaza)
            {

                Ghaza _Ghaza = new Ghaza();
                _Ghaza.year = item1.PersainDate.Substring(0, 4);
                _Ghaza.month = item1.PersainDate.Substring(5, 2);
                _Ghaza.Day = item1.PersainDate.Substring(8, 2);
                _Ghaza.NameGhaza = item1.Onvan;
                _Ghaza.PersianDate = item1.PersainDate;
                _Ghaza.Calery = item1.Gheimat.Value; // قیمت غذا
                CaleryList.Add(_Ghaza);
            }




            if (GozareshHazinehCombo4.Text == "روزانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.Day } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().Day,
                                Gheimat = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Gheimat));
                }
            }

            else if (GozareshHazinehCombo4.Text == "ماهانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.month } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().month,
                                Gheimat = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Gheimat));
                }
            }
            else if (GozareshHazinehCombo4.Text == "سالانه")
            {
                var Fin22 = from p in CaleryList
                            orderby p.PersianDate
                            group p by new { p.year } into g
                            select new
                            {
                                PersainDate = g.FirstOrDefault().year,
                                Gheimat = g.Sum(x => x.Calery)
                            };
                foreach (var item2 in Fin22)
                {
                    chartvalue2.Add(new KeyValuePair<string, decimal>(item2.PersainDate, item2.Gheimat));
                }
            }
            GhazaTedad2.DataContext = chartvalue2;

        }

        private void PersianCalendarGozareshHazineh5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL17.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL18.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void MoshakhasatManBut1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                DarooLeftToolbarPanel.Visibility = Visibility.Visible; DarooLeftToolbarPanel.IsEnabled = true;
                IadAvarDaroPanel.Visibility = Visibility.Visible; IadAvarDaroPanel.IsEnabled = true;
                CreateDaroTable();
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarIadAvarDaroBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Par._DateTimeVariable = PerCalendar.Date.start();
                IadAvarDaroTextBox2.Text = Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        public int HesabDafeBaghiAzDaro(DateTime StartTime, DateTime Moghaiese, int Dore, int Dafe, string NoeDore)
        {
            //try { 

            int DoreGozashte = 0;
            int DafeBaghi = 0;
            switch (NoeDore)
            {
                case "ساعت":
                    for (DoreGozashte = 0; true; DoreGozashte++)
                    {
                        if (Moghaiese < StartTime.AddHours(DoreGozashte * Dore))
                        {
                            break;
                        }
                        else if (Dafe < DoreGozashte)
                        {
                            break;
                        }
                    }
                    break;
                case "روز":
                    for (DoreGozashte = 0; true; DoreGozashte++)
                    {
                        if (Moghaiese < StartTime.AddDays(DoreGozashte * Dore))
                        {
                            break;
                        }
                        else if (Dafe < DoreGozashte)
                        {
                            break;
                        }
                    }
                    break;
                case "ماه":
                    for (DoreGozashte = 0; true; DoreGozashte++)
                    {
                        if (Moghaiese < StartTime.AddMonths(DoreGozashte * Dore))
                        {
                            break;
                        }
                        else if (Dafe < DoreGozashte)
                        {
                            break;
                        }
                    }
                    break;
                case "سال":
                    for (DoreGozashte = 0; true; DoreGozashte++)
                    {
                        if (Moghaiese < StartTime.AddYears(DoreGozashte * Dore))
                        {
                            break;
                        }
                        else if (Dafe < DoreGozashte)
                        {
                            break;
                        }
                    }
                    break;
            }
            if (DoreGozashte > Dafe)
            {
                DafeBaghi = 0;
            }
            else
            {
                DafeBaghi = Dafe - DoreGozashte;
            }

            return DafeBaghi;
            //}
            //catch (Exception error)
            //{
            //    SaveError(error);
            //}
        }
        public void CreateDaroTable()
        {
            try
            {

                IadAvarDaroDataaGrid.Items.Clear();
                var FinDaroName = from daro in _FamilyManaerDBEntities.IadAvarDaroTbls
                                  orderby daro.ID descending
                                  select daro;
                foreach (var item in FinDaroName)
                {
                    bool etebarr = true;
                    string time = item.Saat.Value.TimeOfDay.ToHHMM().ToString(); ;
                    DateTime starttime = new DateTime(item.GDate.Value.Year, item.GDate.Value.Month, item.GDate.Value.Day, item.Saat.Value.Hour, item.Saat.Value.Minute, 0);
                    string x = "این دارو هر " + item.Dore + " " + item.NoeDore + " یک بار، به مدت " + item.Dafe + "دفعه دیگر از ساعت " + time + " تاریخ " + item.PersianDate + " مصرف می شود.";
                    string ID = item.ID.ToString();
                    int DoreMandr = HesabDafeBaghiAzDaro(starttime, DateTime.Now, item.Dore.Value, item.Dafe.Value, item.NoeDore);
                    if (DoreMandr == 0)
                    {
                        etebarr = false;

                    }
                    else
                    {
                        etebarr = true;
                    }

                    IadAvarDaroDataaGrid.Items.Add(new { ID, NameDaro = item.OnvanDaro, shive = x, etebar = etebarr, item.Tozihat });
                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        private void IadAvarDaroBut1_Click(object sender, RoutedEventArgs e) //دکمه ثبت دارو
        {
            try
            {
                if ((string.IsNullOrEmpty(IadAvarDaroTextBox4.Text)) || (string.IsNullOrEmpty(IadAvarDaroCombo1.Text)) || (string.IsNullOrEmpty(IadAvarDaroCombo2.Text)) || (string.IsNullOrEmpty(IadAvarDaroTextBox1.Text)) || (string.IsNullOrEmpty(IadAvarDaroTimePicker1.Text)) || (string.IsNullOrEmpty(Par.Tarikh)))
                {
                    MajMessageBox.show("لطفاً تمامی مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                IadAvarDaroTbl _IadAvarDaroTbl = new IadAvarDaroTbl();
                _IadAvarDaroTbl.Dafe = int.Parse(IadAvarDaroTextBox4.Text);
                _IadAvarDaroTbl.Dore = int.Parse(IadAvarDaroCombo1.Text);
                _IadAvarDaroTbl.NoeDore = IadAvarDaroCombo2.Text;
                _IadAvarDaroTbl.OnvanDaro = IadAvarDaroTextBox1.Text;
                _IadAvarDaroTbl.PersianDate = Par.Tarikh;
                _IadAvarDaroTbl.Saat = IadAvarDaroTimePicker1.Value;
                _IadAvarDaroTbl.Tozihat = IadAvarDaroTextBox3.Text;
                _IadAvarDaroTbl.GDate = Par._DateTimeVariable.Value;
                _FamilyManaerDBEntities.IadAvarDaroTbls.Add(_IadAvarDaroTbl);
                _FamilyManaerDBEntities.SaveChanges();
                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateDaroTable();


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void IadAvarDaroBut2_Click(object sender, RoutedEventArgs e) // حذف برنامه دارو
        {

            try
            {

                object item = (object)IadAvarDaroDataaGrid.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                int ID = int.Parse((IadAvarDaroDataaGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                var ispresent = _FamilyManaerDBEntities.IadAvarDaroTbls.Where(check => check.ID == ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف دارو زیر اطمینان دارید؟" + Environment.NewLine + ispresent.OnvanDaro, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.IadAvarDaroTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();

                        MajMessageBox.show("اطلاعات با موفقیت پاک شد.", MajMessageBox.MajMessageBoxBut.OK);
                        CreateDaroTable();
                    }


                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarIadAvarDaroBut1_Click(object sender, RoutedEventArgs e) // روز مصرف دارو
        {
            try
            {

                Par._DateTimeVariable = PerCalendar.Date.start();
                IadAvarDaroTextBox5.Text = Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

                IadAvarDaroDataaGrid1.Items.Clear();
                var FinDaroName = from daro in _FamilyManaerDBEntities.IadAvarDaroTbls
                                  orderby daro.ID descending
                                  select daro;
                foreach (var item in FinDaroName)
                {
                    string time = item.Saat.Value.TimeOfDay.ToHHMM().ToString(); ;
                    DateTime starttime = new DateTime(item.GDate.Value.Year, item.GDate.Value.Month, item.GDate.Value.Day, item.Saat.Value.Hour, item.Saat.Value.Minute, 0);
                    string x = "این دارو هر " + item.Dore + " " + item.NoeDore + " یک بار، به مدت " + item.Dafe + "دفعه دیگر از ساعت " + time + " تاریخ " + item.PersianDate + " مصرف می شود.";
                    int DafeBaghi = HesabDafeBaghiAzDaro(starttime, Par._DateTimeVariable.Value, item.Dore.Value, item.Dafe.Value, item.NoeDore);

                    for (int i = 0; i < DafeBaghi; i++)
                    {


                        switch (item.NoeDore)
                        {
                            case "ساعت":
                                starttime = starttime.AddHours((item.Dore.Value) * ((item.Dafe.Value) - (DafeBaghi) + i));
                                break;
                            case "روز":
                                starttime = starttime.AddDays((item.Dore.Value) * ((item.Dafe.Value) - (DafeBaghi) + i));
                                break;
                            case "ماه":
                                starttime = starttime.AddMonths((item.Dore.Value) * ((item.Dafe.Value) - (DafeBaghi) + i));
                                break;
                            case "سال":
                                starttime = starttime.AddYears((item.Dore.Value) * ((item.Dafe.Value) - (DafeBaghi) + i));
                                break;
                        }

                        if ((Par._DateTimeVariable.Value <= starttime) && (Par._DateTimeVariable.Value.AddHours(24) >= starttime))
                        {
                            IadAvarDaroDataaGrid1.Items.Add(new { NameDaro = item.OnvanDaro, Saat = item.Saat.Value.ToShortTimeString().ToString(), Baghi = DafeBaghi.ToString(), item.Tozihat });

                        }
                        if (starttime > Par._DateTimeVariable.Value.AddHours(24))
                        {
                            break;
                        }
                    }


                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void IadAvarDaroTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal number;
                if (decimal.TryParse(IadAvarDaroTextBox4.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    IadAvarDaroTextBox4.Text = string.Format("{0}", number);
                    IadAvarDaroTextBox4.SelectionStart = IadAvarDaroTextBox4.Text.Length;
                    if (int.Parse(IadAvarDaroTextBox4.Text) > 99999) { IadAvarDaroTextBox4.Text = "99999"; }
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void TaghirRamzBut1_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            DarooLeftToolbarPanel.Visibility = Visibility.Visible; DarooLeftToolbarPanel.IsEnabled = true;
            MasrafeDaroPanel.Visibility = Visibility.Visible; MasrafeDaroPanel.IsEnabled = true;
        }

        private void PersianCalendarGozareshDaramad2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshDaramadLBL3.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshDaramad1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshDaramadLBL4.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshDaramadBut1_Click(object sender, RoutedEventArgs e) // گزارش مصرف دارو
        {
            try
            {
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                ClusteredChart.DataContext = null;
                List<KeyValuePair<string, int>> chartvalue = new List<KeyValuePair<string, int>>();

                var FinDaroName = from daro in _FamilyManaerDBEntities.IadAvarDaroTbls
                                  orderby daro.ID descending
                                  select daro;
                foreach (var item in FinDaroName)
                {
                    string time = item.Saat.Value.TimeOfDay.ToHHMM().ToString(); ;
                    DateTime starttime = new DateTime(item.GDate.Value.Year, item.GDate.Value.Month, item.GDate.Value.Day, item.Saat.Value.Hour, item.Saat.Value.Minute, 0);
                    int DafeMasrrafStart = HesabDafeBaghiAzDaro(starttime, Par._DateTimeVariableStart.Value, item.Dore.Value, item.Dafe.Value, item.NoeDore);
                    int DafeMasrrafFinish = HesabDafeBaghiAzDaro(starttime, Par._DateTimeVariableFinish.Value, item.Dore.Value, item.Dafe.Value, item.NoeDore);
                    if (DafeMasrrafStart != 0)
                    {
                        chartvalue.Add(new KeyValuePair<string, int>(item.OnvanDaro, (DafeMasrrafStart - DafeMasrrafFinish)));
                    }


                }
                ClusteredChart.DataContext = chartvalue;



            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void TaghirRamzBut2_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            DarooLeftToolbarPanel.Visibility = Visibility.Visible; DarooLeftToolbarPanel.IsEnabled = true;
            GozareshDaroPanel.Visibility = Visibility.Visible; GozareshDaroPanel.IsEnabled = true;
        }



        private void PersianCalendarSbteKharjkardVahed1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariable = PerCalendar.Date.start();
                SbteKharjkardVahedTextBox6.Text = Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                Par.Year = Date.PYear.ToString();
                GiveMePersianYear(Par._DateTimeVariable.Value, SbteKharjkardVahedCombo9);

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        public void GiveMePersianYear(DateTime GDate, System.Windows.Controls.ComboBox comboBox) // دریافت سال شمسی از تاریخ میلادی
        {

            PersianCalendar pc = new PersianCalendar();
            int Year = 0;
            Year = pc.GetYear(GDate);
            comboBox.Items.Clear();
            comboBox.Items.Add(Year - 2);
            comboBox.Items.Add(Year - 1);
            comboBox.Items.Add(Year);
            comboBox.Items.Add(Year + 1);
            comboBox.Items.Add(Year + 2);
            comboBox.SelectedIndex = 2;

        }
        private void SbteKharjkardVahedTextBox7_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(SbteKharjkardVahedTextBox7.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    SbteKharjkardVahedTextBox7.Text = string.Format("{0:N0}", number);
                    SbteKharjkardVahedTextBox7.SelectionStart = SbteKharjkardVahedTextBox7.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }
        public void crateSharjSakheman()  // ساخت جدول شارژ ساختمان
        {
            try
            {
                SbteKharjkardVahedGrid1.Items.Clear();
                var IDVahesd = from _ in _FamilyManaerDBEntities.HamsaieTbls
                               where _.Tasfieh != true && _.NameVahed == SbteKharjkardVahedCombo4.Text
                               select _;

                foreach (var item in IDVahesd)
                {
                    var Fin = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                              orderby _.ID
                              where _.IDVahed.Value == item.ID && _.Income.Value > 0 && _.Year == SbteKharjkardVahedCombo9.Text
                              select _;
                    foreach (var item2 in Fin)
                    {
                        SbteKharjkardVahedGrid1.Items.Add(new { ID = item2.ID, Onvan = item2.TitleCost, Mablagh = item2.Income.Value.ToString("N0"), Tarikh = item2.PersianDate, Mah = item2.mmonth, Tozihat = item2.Description });
                    }
                }




            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        private void SbteKharjkardVahedCombo4_DropDownClosed(object sender, EventArgs e)
        {
            crateSharjSakheman();
        }

        private void PersianCalendarSbteKharjkardVahed22_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                SabteHamsaieTextBox22.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarSbteKharjkardVahd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                SabteHamsaieTextBox522.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void SbteKharjkardVahedBut5_Click(object sender, RoutedEventArgs e) // ثبت شارژ ساختمان
        {
            try
            {
                if ((string.IsNullOrEmpty(SbteKharjkardVahedTextBox5.Text)) || string.IsNullOrEmpty(SbteKharjkardVahedTextBox6.Text) || string.IsNullOrEmpty(SbteKharjkardVahedTextBox7.Text) || string.IsNullOrEmpty(SbteKharjkardVahedCombo9.Text) || string.IsNullOrEmpty(SbteKharjkardVahedCombo3.Text) || string.IsNullOrEmpty(SbteKharjkardVahedCombo4.Text))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var IDVahesd = from _ in _FamilyManaerDBEntities.HamsaieTbls
                               where (string.IsNullOrEmpty(_.FinishPersianDate) && _.NameVahed == SbteKharjkardVahedCombo4.Text)
                               select _;

                foreach (var item in IDVahesd)
                {


                    _SabteHazinehSakhtemanTbl.TitleCost = SbteKharjkardVahedTextBox5.Text;
                    _SabteHazinehSakhtemanTbl.PersianDate = SbteKharjkardVahedTextBox6.Text;
                    _SabteHazinehSakhtemanTbl.GDate = Par._DateTimeVariable.Value;
                    _SabteHazinehSakhtemanTbl.Cost = 0;
                    _SabteHazinehSakhtemanTbl.ShiveTaghsim = string.Empty;
                    _SabteHazinehSakhtemanTbl.StartGdat = null;
                    _SabteHazinehSakhtemanTbl.FinishGdate = null;
                    _SabteHazinehSakhtemanTbl.startPersianDate = string.Empty;
                    _SabteHazinehSakhtemanTbl.FinishPersianDate = string.Empty;
                    _SabteHazinehSakhtemanTbl.Income = decimal.Parse(SbteKharjkardVahedTextBox7.Text);
                    _SabteHazinehSakhtemanTbl.mmonth = SbteKharjkardVahedCombo3.Text;
                    _SabteHazinehSakhtemanTbl.IDVahed = int.Parse(item.ID.ToString());
                    _SabteHazinehSakhtemanTbl.VahedName = SbteKharjkardVahedCombo4.Text;
                    _SabteHazinehSakhtemanTbl.Description = SbteKharjkardVahedTextBox8.Text;
                    _SabteHazinehSakhtemanTbl.Year = SbteKharjkardVahedCombo9.Text;
                    _SabteHazinehSakhtemanTbl.Enteghali = false;

                    _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Add(_SabteHazinehSakhtemanTbl);
                    _FamilyManaerDBEntities.SaveChanges();

                    MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                    crateSharjSakheman();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedCombo3_Loaded(object sender, RoutedEventArgs e)
        {
            SbteKharjkardVahedCombo3.SelectedIndex = 0;
        }

        private void SbteKharjkardVahedBut6_Click(object sender, RoutedEventArgs e) // حذف شارژ ساختمان
        {

            try
            {
                object item = (object)SbteKharjkardVahedGrid1.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                int ID = int.Parse((IadAvarDaroDataaGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                var ispresent = _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Where(check => check.ID == ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.TitleCost, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                EmptyPar();
                CleanOldDataEnteredTXT();
                crateSharjSakheman();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void ModiriatSakhtemanRightToolbarBut2_Click_1(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
            SbteKharjkardVahedTextBox5.Text = "شارژ ساختمان";
            SbteSharjPanel.Visibility = Visibility.Visible; SbteSharjPanel.IsEnabled = true;
            Par._DateTimeVariable = DateTime.Now;
            PersianCalendarToday(null, SbteKharjkardVahedTextBox6);
            GiveMePersianYear(Par._DateTimeVariable.Value, SbteKharjkardVahedCombo9);
            SbteKharjkardVahedCombo3.SelectedIndex = 0;
            SbteKharjkardVahedCombo4.SelectedIndex = 0;

            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.NoeHazinehIaDaramadSakhteman == "درآمد"
                      select _.OnvanHazinehIaDaramadSakhteman;
            SbteKharjkardVahedTextBox5.ItemsSource = Fin.ToList();
            SbteKharjkardVahedTextBox5.SelectedIndex = 0;
            DateTime d = DateTime.Now.AddYears(-1);
            var Fin1 = from x in _FamilyManaerDBEntities.HamsaieTbls
                       where x.Tasfieh == false
                       select x.NameVahed;
            SbteKharjkardVahedCombo4.ItemsSource = Fin1.ToList();
            if (Fin != null)
            {
                SbteKharjkardVahedCombo4.SelectedIndex = 0;

            }

        }

        private void SbteKharjkardVahedCombo2_DropDownClosed(object sender, EventArgs e)
        {
            CreateHazinehVahedGrid();

        }

        private void ModiriatSakhtemanRightToolbarBut7_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            ModiriatSakhtemanRightToolbarVisibile();
            MohasebehHazinehSakhtemanPanel.Visibility = Visibility.Visible; MohasebehHazinehSakhtemanPanel.IsEnabled = true;

            var FinYear = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                          orderby _.Year descending
                          select _.Year;

            if (FinYear != null)
            {
                MohasebehHazinehSakhsdfgtemanPanelcombo.ItemsSource = FinYear.Distinct().ToList();
                MohasebehHazinehSakhsdfgtemanPanelcombo.SelectedIndex = 0;
            }

            MohasebehHazinehSakhtemanPansdfgelComBo.SelectedIndex = 0;

        }

        private void MohasebehHazinehSakhtemanPanelCombo2_Loaded(object sender, RoutedEventArgs e)
        {
            //var Fin = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
            //          select p.Year;
            //if (Fin != null)
            //{
            //    MohasebehHazinehSakhtemanPanelCombo2.ItemsSource = Fin.Distinct().ToList();
            //}
        }



        public void PersianCalendarToday(System.Windows.Controls.Label Persianlabel, System.Windows.Controls.TextBox PersianTextBox) // ثبت تاریخ امروز برای تقویم
        {

            PersianCalendar pc = new PersianCalendar();
            int Year = pc.GetYear(DateTime.Now);
            int Month = pc.GetMonth(DateTime.Now);
            int Day = pc.GetDayOfMonth(DateTime.Now);
            if (Persianlabel != null)
            {
                Persianlabel.Content = Year.ToString() + "/" + Month.ToString().PadLeft(2, '0') + "/" + Day.ToString().PadLeft(2, '0');

            }
            if (PersianTextBox != null)
            {
                PersianTextBox.Text = Year.ToString() + "/" + Month.ToString().PadLeft(2, '0') + "/" + Day.ToString().PadLeft(2, '0');

            }
        }

        private void PersianCalendarSbteKhdfgarjkardVahed1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                SbteKharjkardVahedTextBلox8.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarSbteKdfgharjkardVahed1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                SbteKharjkardVahedTextBoلx6.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void ModiriatSakhtemanRightToolbddarBut1_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();

            ModiriatSakhtemanRightToolbarVisibile();
            SabteOnvanHazinehSakhteman.Visibility = Visibility.Visible; SabteOnvanHazinehSakhteman.IsEnabled = true;
            SbteKharjkardVahedGrid2.Items.Clear();
            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.OnvanHazinehIaDaramadSakhteman != null
                      select _;
            foreach (var item in Fin)
            {
                SbteKharjkardVahedGrid2.Items.Add(new { Onvan = item.OnvanHazinehIaDaramadSakhteman, category = item.NoeHazinehIaDaramadSakhteman });
            }
        }

        private void SbteKharjkardVahedBut4_Click(object sender, RoutedEventArgs e) // دکمه ثبت عنوان درآمد و هزینه ساختمان
        {
            try
            {
                if ((SbteKharjkardVahedTextBox9.Text == "") || (SbteKharjkardVahedCombo5.Text == ""))
                {
                    MajMessageBox.show("لطفاً مقادیر مد نظر را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                var ispresent = _FamilyManaerDBEntities.ComboBoxTbls.Where(check => check.OnvanHazinehIaDaramadSakhteman == SbteKharjkardVahedTextBox9.Text).FirstOrDefault();
                if (ispresent != null)
                {
                    MajMessageBox.show("این عنوان تکراری است.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                _ComboBoxTbl.OnvanHazinehIaDaramadSakhteman = SbteKharjkardVahedTextBox9.Text;
                _ComboBoxTbl.NoeHazinehIaDaramadSakhteman = SbteKharjkardVahedCombo5.Text;
                _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                _FamilyManaerDBEntities.SaveChanges();

                MajMessageBox.show("اطلاعات با موفقیت ذخیره شد.", MajMessageBox.MajMessageBoxBut.OK);
                ModiriatSakhtemanRightToolbddarBut1_Click(sender, e);


            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedBut7_Click(object sender, RoutedEventArgs e) // حذف عنوان و نوع هزینه ساختمان
        {
            try
            {
                object item = (object)SbteKharjkardVahedGrid2.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                string ID = (SbteKharjkardVahedGrid2.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var ispresent = _FamilyManaerDBEntities.ComboBoxTbls.Where(check => check.OnvanHazinehIaDaramadSakhteman == ID).FirstOrDefault();
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.OnvanHazinehIaDaramadSakhteman, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.ComboBoxTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                ModiriatSakhtemanRightToolbddarBut1_Click(sender, e);



            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedTextBox5_DropDownClosed(object sender, EventArgs e)
        {
            CreateSabteKharjkardSade();
        }

        private void SbteKharjkardVahedCombo3_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                SbteKharjkardVahedGrid1.Items.Clear();
                var Fin = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                          orderby _.ID
                          where _.Income.Value > 0 && _.Year == SbteKharjkardVahedCombo9.Text && _.mmonth == SbteKharjkardVahedCombo3.Text
                          select _;
                foreach (var item2 in Fin)
                {
                    SbteKharjkardVahedGrid1.Items.Add(new { ID = item2.ID, AB1 = item2.VahedName, Onvan = item2.TitleCost, Mablagh = item2.Income.Value.ToString("N0"), Tarikh = item2.PersianDate, Mah = item2.mmonth, Tozihat = item2.Description });
                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void ModirdiatSakhtemanRightToolbarBut2_Click(object sender, RoutedEventArgs e) // دکمه سمت راست: کزارش شارژ ساختمان
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
            GozareshSbteSharjPanel.Visibility = Visibility.Visible; GozareshSbteSharjPanel.IsEnabled = true;
            var Fin = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                      where _.Income > 0
                      orderby _.Year
                      select _.Year;
            SbteKharjkardVahedCombo8.ItemsSource = Fin.Distinct().ToList();



        }

        private void SbteKharjkardVahedCombo6_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                SbteKharjkardVahedGrid3.Items.Clear();
                ListBox1.Items.Clear();
                var Fin = from _ in _FamilyManaerDBEntities.HamsaieTbls
                          orderby _.ID
                          where _.Tasfieh != true
                          select _;
                if (Fin != null)
                {
                    foreach (var item1 in Fin)
                    {
                        var Fin2 = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                                   where _.VahedName == item1.NameVahed && _.mmonth == SbteKharjkardVahedCombo6.Text && _.Year == SbteKharjkardVahedCombo8.Text && _.Income > 0
                                   select _;
                        if (Fin2 != null)
                        {
                            if (Fin2.Count() == 0)
                            {
                                SbteKharjkardVahedGrid3.Items.Add(new { AB1 = item1.NameVahed });
                            }
                            foreach (var item2 in Fin2)
                            {
                                SbteKharjkardVahedGrid3.Items.Add(new { AB1 = item2.VahedName, AB2 = item2.Income.Value.ToString("N0"), AB3 = item2.PersianDate });
                            }
                        }

                    }
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKhafrjkardVahedBut8_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception error) { SaveError(error); }
        }

        private void SbteKharjkardVahedGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                object item = (object)SbteKharjkardVahedGrid3.SelectedItem;
                if (item != null)
                {
                    string onvan = (SbteKharjkardVahedGrid3.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    ListBox1.Items.Add(onvan);
                }
                SbteKharjkardVahedGrid3.SelectedItem = null;
            }
            catch (Exception error) { SaveError(error); }

        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)ListBox1.SelectedItem;
                // string onvan = (ListBox1.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                if (item != null)
                {
                    ListBox1.Items.Remove(item.ToString());

                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void MohasebehHazinehSakhtdemanPanelBut3_Click(object sender, RoutedEventArgs e) // حذف همه تصفیه ها
        {
            try
            {

                var result = MajMessageBox.show("آیا از حذف تمامی تصفیه ها اطمینان دارید؟", MajMessageBox.MajMessageBoxBut.IadAvar);
                if (result.ToString() == "Yes")
                {
                    var Fin = from _ in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                              where (string.IsNullOrEmpty(_.PersianDate))
                              select _;
                    foreach (var item in Fin)
                    {
                        _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls.Remove(item);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                    MajMessageBox.show("همه تصفی ها حذف شد.", MajMessageBox.MajMessageBoxBut.OK);
                }



            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarGozareshDaramad3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshDaramadLBL5.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshDaramad3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshDaramadLBL6.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshDaramadBut2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }

                List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();

                var Fin1 = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                           where p.Cost > 0 && p.GDate > Par._DateTimeVariableStart && p.GDate < Par._DateTimeVariableFinish && p.Enteghali != true
                           group p by p.TitleCost into g
                           select new
                           {
                               onvan = g.FirstOrDefault().TitleCost,
                               Meghdar = g.Sum(x => x.Cost),
                               Tedad = g.Count()


                           };
                foreach (var item in Fin1)
                {
                    chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    chartvalue2.Add(new KeyValuePair<string, int>(item.onvan, item.Tedad));

                }
                pieChart2.DataContext = chartvalue;


                PPieGozareshDaramad1.DataContext = chartvalue2;


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshDaramad3_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshDaramadLBL6.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPie2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPie2.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان هزینه:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPieGozareshDaramad1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPie2.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان هزینه:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void ModirdiatSakhtdemanRightToolbarBut2_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
            NemodarHazinehSakhteman.Visibility = Visibility.Visible; NemodarHazinehSakhteman.IsEnabled = true;
        }

        private void ModirdiatSakhddtemanRightToolbarBut2_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ModiriatSakhtemanRightToolbarVisibile();
            NemodarDaramadSakhteman.Visibility = Visibility.Visible; NemodarDaramadSakhteman.IsEnabled = true;
        }

        private void PersianCalendarGozareshDaramad4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshDaramadLBL7.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshDaramad4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshDaramadLBL8.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshDaramadBut4_Click(object sender, RoutedEventArgs e) // محاسبه نمودار درآمد ساختمان
        {
            try
            {
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }

                List<KeyValuePair<string, decimal>> chartvalue = new List<KeyValuePair<string, decimal>>();
                List<KeyValuePair<string, int>> chartvalue2 = new List<KeyValuePair<string, int>>();

                var Fin1 = from p in _FamilyManaerDBEntities.SabteHazinehSakhtemanTbls
                           where p.Income > 0 && p.GDate > Par._DateTimeVariableStart && p.GDate < Par._DateTimeVariableFinish && p.Enteghali != true
                           group p by p.TitleCost into g
                           select new
                           {
                               onvan = g.FirstOrDefault().TitleCost,
                               Meghdar = g.Sum(x => x.Income),
                               Tedad = g.Count()


                           };
                foreach (var item in Fin1)
                {
                    chartvalue.Add(new KeyValuePair<string, decimal>(item.onvan, item.Meghdar.Value));
                    chartvalue2.Add(new KeyValuePair<string, int>(item.onvan, item.Tedad));

                }
                pieChart3.DataContext = chartvalue;


                PPieGozareshDaramad3.DataContext = chartvalue2;


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPie3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPie3.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                    MajMessageBox.show("عنوان درآمدی:" + Environment.NewLine + Onvan, MajMessageBox.MajMessageBoxBut.OK);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PPieGozareshDaramad3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)PPieGozareshDaramad3.SelectedItem;
                if (item != null)
                {
                    string itemstring = item.ToString();
                    int foundS1 = itemstring.IndexOf(",");
                    string Onvan = itemstring.Substring(1, foundS1 - 1);
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void NameVamGirandehCombo1_DropDownClosed(object sender, EventArgs e)
        {
            CreateVamNafarGrid();
        }

        private void PersianCalendarSbteKasshdfgarjkardVahed1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                OnvanVamTextssBox1.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarSbteasKdfgharjkardVahed1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gridOnvanVam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)gridOnvanVam.SelectedItem;
                if (item != null)
                {
                    int ID = int.Parse((gridOnvanVam.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    var ispersent = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.ID == ID);
                    OnvanVamTextBox1.Text = ispersent.Title;
                    OnvanVamTextssBox1.Text = ispersent.StartPersianDate;
                    Par._DateTimeVariableStart = ispersent.StarGdate;
                    TakhsiddsHazinehTextBox3.Text = ispersent.TedadAghsat.ToString();
                    MablaghVamTextBox.Text = ispersent.MablaghVam.ToString();
                    TakhddsiddsHazinehTextBdox3.Text = ispersent.MablaghGhest.ToString();
                    toggleBddutton.IsChecked = ispersent.FAAL;
                    OnvanVamTeddxtBox1.Text = ispersent.Description;
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void OnvalknVamBut1_Click(object sender, RoutedEventArgs e) //ویراش عنوان وام
        {
            try
            {
                object item = (object)gridOnvanVam.SelectedItem;
                if (item == null)
                {
                    MajMessageBox.show("لطفاً از جدول وام یک عنوان را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                int ID = int.Parse((gridOnvanVam.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                var ispersent = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.ID == ID);
                if (ispersent.Title != OnvanVamTextBox1.Text)
                {
                    var Fin1 = from _ in _FamilyManaerDBEntities.TakhsisVamTbls
                               where _.Onvan == ispersent.Title
                               select _;
                    var Fin2 = from _ in _FamilyManaerDBEntities.PardakhtVamTbls
                               where _.OnvanVam == ispersent.Title
                               select _;
                    var Fin3 = from _ in _FamilyManaerDBEntities.OnvanVamNafarTbls
                               where _.VamTitle == ispersent.Title
                               select _;
                    foreach (var item1 in Fin1)
                    {
                        item1.Onvan = OnvanVamTextBox1.Text;
                    }
                    foreach (var item2 in Fin2)
                    {
                        item2.OnvanVam = OnvanVamTextBox1.Text;
                    }
                    foreach (var item3 in Fin3)
                    {
                        item3.VamTitle = OnvanVamTextBox1.Text;
                    }
                }
                ispersent.Title = OnvanVamTextBox1.Text;
                ispersent.StartPersianDate = OnvanVamTextssBox1.Text;
                ispersent.StarGdate = Par._DateTimeVariableStart;
                ispersent.TedadAghsat = int.Parse(TakhsiddsHazinehTextBox3.Text);
                ispersent.MablaghVam = decimal.Parse(MablaghVamTextBox.Text);
                ispersent.MablaghGhest = decimal.Parse(TakhddsiddsHazinehTextBdox3.Text);
                ispersent.FAAL = toggleBddutton.IsChecked;
                ispersent.Description = OnvanVamTeddxtBox1.Text;
                _FamilyManaerDBEntities.SaveChanges();

                EmptyPar();
                CleanOldDataEnteredTXT();
                CreateOnvanVamGrid();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void TakhsiddsHazinehTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(TakhsiddsHazinehTextBox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    TakhsiddsHazinehTextBox3.Text = string.Format("{0:N0}", number);
                    TakhsiddsHazinehTextBox3.SelectionStart = TakhsiddsHazinehTextBox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void TakhsiddsHazinehTextBdox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(MablaghVamTextBox.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    MablaghVamTextBox.Text = string.Format("{0:N0}", number);
                    MablaghVamTextBox.SelectionStart = MablaghVamTextBox.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void TakhddsiddsHazinehTextBdox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(TakhddsiddsHazinehTextBdox3.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    TakhddsiddsHazinehTextBdox3.Text = string.Format("{0:N0}", number);
                    TakhddsiddsHazinehTextBdox3.SelectionStart = TakhddsiddsHazinehTextBdox3.Text.Length;
                }
            }
            catch (Exception error) { SaveError(error); }
        }

        private void gridNameVamGirandeh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                object item = (object)gridNameVamGirandeh.SelectedItem;
                if (item != null)
                {
                    int ID = int.Parse((gridNameVamGirandeh.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    var ispersent = _FamilyManaerDBEntities.OnvanVamNafarTbls.FirstOrDefault(x => x.ID == ID);
                    NameVamGirandehCombo1.Text = ispersent.VamTitle;
                    NameVamGirandehTextBox1.Text = ispersent.Nafar;
                    SabteIafdAvarTextBox2.Text = ispersent.Mobile;
                    NameVamGirandehCombo1.IsReadOnly = true;
                    NameVamGirandehTextBox1.IsReadOnly = true;
                    NameVamGirandehCombo1.IsEnabled = false;
                }


            }
            catch (Exception error) { SaveError(error); }
        }

        private void NameVamGirandehBut4_Click(object sender, RoutedEventArgs e) // دکمه ویرایش نفر و وام
        {
            try
            {
                object item = (object)gridNameVamGirandeh.SelectedItem;
                int ID = int.Parse((gridNameVamGirandeh.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                var ispersent = _FamilyManaerDBEntities.OnvanVamNafarTbls.FirstOrDefault(x => x.ID == ID);
                var result = MajMessageBox.show("آیا از ویرایش زیر اطمینان دارید" + Environment.NewLine + ispersent.Nafar, MajMessageBox.MajMessageBoxBut.YESNO);
                if (result.ToString() == "Yes")
                {
                    ispersent.Mobile = SabteIafdAvarTextBox2.Text;
                    NameVamGirandehCombo1.IsReadOnly = false;
                    NameVamGirandehTextBox1.IsReadOnly = false;
                    NameVamGirandehCombo1.IsEnabled = true;
                    _FamilyManaerDBEntities.SaveChanges();
                    CreateVamNafarGrid();
                    CleanOldDataEnteredTXT();
                    EmptyPar();
                }




            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void NameVamGirandehBut5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NameVamGirandehCombo1.IsReadOnly = false;
                NameVamGirandehTextBox1.IsReadOnly = false;
                NameVamGirandehCombo1.IsEnabled = true;

                gridNameVamGirandeh.SelectedItem = null;
                EmptyPar();
                CleanOldDataEnteredTXT();

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        public void MojodiSandoghVam() // گزارش موجودی صندوق وام
        {
            decimal AghsatPardakhti = 0;
            decimal VamPardakhtShode = 0;
            var ispersent = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.Title == NobatVamGirandehCombo1.Text);
            var FinAghsatPardakhti = from _ in _FamilyManaerDBEntities.PardakhtVamTbls
                                     where _.OnvanVam == NobatVamGirandehCombo1.Text
                                     group _ by _.OnvanVam into g
                                     select (new { MojodiSandogh = g.Sum(_ => _.MablaghPardakhti) });
            foreach (var item in FinAghsatPardakhti)
            {
                AghsatPardakhti = item.MojodiSandogh.Value;
            }

            var FinPardakhti = from _ in _FamilyManaerDBEntities.TakhsisVamTbls
                               where _.Onvan == NobatVamGirandehCombo1.Text
                               group _ by _.Onvan into g
                               select (new { MojodiSandogh = g.Count(_ => _.GTarikh != null) });
            foreach (var item in FinPardakhti)
            {
                VamPardakhtShode = item.MojodiSandogh * ispersent.MablaghVam.Value;
            }
            PardakhtVamLBL1.Content = AghsatPardakhti.ToString("N0");
            PardakhtVamLBL2.Content = VamPardakhtShode.ToString("N0");
            PardakhtVamLBL3.Content = (AghsatPardakhti - VamPardakhtShode).ToString("N0");
        }

        private void PersianCalendarBut2_Click(object sender, RoutedEventArgs e)
        {
            var irspresent = _FamilyManaerDBEntities.OnvanVamTbls.FirstOrDefault(_ => _.Title == NobatVamGirandehCombo1.Text);
            decimal Mojodi = decimal.Parse(PardakhtVamLBL3.Content.ToString());
            if (Mojodi < irspresent.MablaghVam)
            {
                MajMessageBox.show("موجودی صندوق کافی نیست", MajMessageBox.MajMessageBoxBut.OK);
                return;
            }
            Par._DateTimeVariable = PerCalendar.Date.start();
            NobatVamGirandehTextBox5.Text = Par.Tarikh = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
            Par.Year = Date.PYear.ToString();
        }

        private void gridNameVacmGirandeh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)gridNameVacmGirandeh.SelectedItem;
                if (item != null)
                {
                    string onvan = (gridNameVacmGirandeh.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    ListBox2.Items.Add(onvan);
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = (object)ListBox2.SelectedItem;
                if (item != null)
                {
                    ListBox2.Items.Remove(item.ToString());
                }

            }
            catch (Exception error) { SaveError(error); }
        }

        private void ListBox4_SelectionChanged(object sender, SelectionChangedEventArgs e) // انتخاب از لیست باکس کارها
        {
            if (ListBox4.SelectedItem != null)
            {
                SabteIadAvarTextBox1.Text = ListBox4.SelectedItem.ToString();

            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var isperesent = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.OnvanKar == ListBox4.SelectedItem.ToString());
            _FamilyManaerDBEntities.ComboBoxTbls.Remove(isperesent);
            _FamilyManaerDBEntities.SaveChanges();
            SabteIadAvarTextBox1.Text = string.Empty;
            ListBox4.SelectedItem = null;
            CreaeListBoxKar();
        }

        private void PersianCalendarGozareshHazineh6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableStart = PerCalendar.Date.start();
                GozareshHazinehLBL19.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL20.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshHazinehBut7_Click(object sender, RoutedEventArgs e) // گزارش ساعت انجام کار
        {
            try
            {
                if ((GozareshHazinehLBL19.Content == null) || (GozareshHazinehLBL20.Content == null))
                {
                    MajMessageBox.show("تاریخ شروع و پایان را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }




                List<KeyValuePair<string, int>> chartvalue = new List<KeyValuePair<string, int>>();

                List<SaatOnvanKar> saatOnvanKarsList = new List<SaatOnvanKar>();

                TimeSpan timeSpan = Par._DateTimeVariableFinish.Value - Par._DateTimeVariableStart.Value;
                DateTime startdate = Par._DateTimeVariableStart.Value;
                DateTime Yesterdaystartdate = Par._DateTimeVariableStart.Value.AddDays(-1);
                DateTime TommorowFinishdate = Par._DateTimeVariableFinish.Value.AddDays(2);
                PersianCalendar Pc = new PersianCalendar();

                DateTime Realdate = Par._DateTimeVariableStart.Value;
                int StartMonth = Par._DateTimeVariableStart.Value.Month;
                int StartYear = Par._DateTimeVariableStart.Value.Year;



                var FinOnvanKar = from _ in _FamilyManaerDBEntities.IadAvarTbls
                                  where (((Yesterdaystartdate <= _.StartDateTime) && (_.EndDateTime <= TommorowFinishdate) && (_.Periodic == false))
                                        || ((_.StartDateTime <= Par._DateTimeVariableStart) && (Par._DateTimeVariableStart <= _.PeriodicEndTime) && (_.Periodic == true))
                                        || ((_.StartDateTime <= TommorowFinishdate) && (TommorowFinishdate <= _.PeriodicEndTime) && (_.Periodic == true))
                                        || ((_.StartDateTime >= Par._DateTimeVariableStart) && (TommorowFinishdate >= _.PeriodicEndTime) && (_.Periodic == true)))
                                  select _;


                foreach (var itemOnvanKar in FinOnvanKar)
                {

                    if (itemOnvanKar.Periodic == true)
                    {
                        DateTime? NextStartPeriodTime = null, NexEndPeriodTime = null;



                        for (int i = 0; i <= itemOnvanKar.PeriodNumBer; i++) // پیدا کردن شماره شروع دوره تلاقی یافته با بازه زمانی
                        {

                            switch (itemOnvanKar.PeriodocKind)
                            {
                                case "روز":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "ماه":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "سال":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                            }


                            // کار دوره ایی است و شروع و پایان در یک روز است
                            if (itemOnvanKar.StartDateTime.Value.Day == itemOnvanKar.EndDateTime.Value.Day)
                            {
                                saatOnvanKarsList.Add(AddSaat(NextStartPeriodTime.Value, itemOnvanKar.EndDateTime.Value.Hour - itemOnvanKar.StartDateTime.Value.Hour));

                            }
                            // کار دوره ایی است و شروع و پایان در یک روز نیست
                            else if (itemOnvanKar.StartDateTime.Value.Day != itemOnvanKar.EndDateTime.Value.Day)
                            {
                                saatOnvanKarsList.Add(AddSaat(NextStartPeriodTime.Value, 24 - itemOnvanKar.StartDateTime.Value.Hour));
                                saatOnvanKarsList.Add(AddSaat(NexEndPeriodTime.Value, itemOnvanKar.EndDateTime.Value.Hour));
                            }

                        }
                    }
                    else if (itemOnvanKar.Periodic == false)
                    {

                        // کار دوره ایی نیست و شروع و پایان در یک روز است
                        if (itemOnvanKar.StartDateTime.Value.Day == itemOnvanKar.EndDateTime.Value.Day)
                        {
                            saatOnvanKarsList.Add(AddSaat(itemOnvanKar.StartDateTime.Value, itemOnvanKar.EndDateTime.Value.Hour - itemOnvanKar.StartDateTime.Value.Hour));

                        }
                        // کار دوره ایی نیست و شروع و پایان در یک روز نیست
                        else if (itemOnvanKar.StartDateTime.Value.Day != itemOnvanKar.EndDateTime.Value.Day)
                        {
                            saatOnvanKarsList.Add(AddSaat(itemOnvanKar.StartDateTime.Value, 24 - itemOnvanKar.StartDateTime.Value.Hour));
                            saatOnvanKarsList.Add(AddSaat(itemOnvanKar.EndDateTime.Value, itemOnvanKar.EndDateTime.Value.Hour));
                        }
                    }
                }


                if ((Pc.GetYear(Par._DateTimeVariableFinish.Value) == Pc.GetYear(Par._DateTimeVariableStart.Value)) && (Pc.GetMonth(Par._DateTimeVariableFinish.Value) == Pc.GetMonth(Par._DateTimeVariableStart.Value)))
                {
                    var FinFinal = from _ in saatOnvanKarsList
                                   where Par._DateTimeVariableStart <= _.Tarikh && _.Tarikh <= Par._DateTimeVariableFinish
                                   group _ by Pc.GetDayOfMonth(_.Tarikh) into g
                                   orderby g.FirstOrDefault().Tarikh
                                   select new
                                   {

                                       tarikh = Pc.GetMonth(g.FirstOrDefault().Tarikh).ToString() + "/" + Pc.GetDayOfMonth(g.FirstOrDefault().Tarikh).ToString()
                                       ,
                                       Saat = g.Sum(_ => _.SaatKar)
                                   };
                    foreach (var item in FinFinal)
                    {
                        chartvalue.Add(new KeyValuePair<string, int>(item.tarikh, item.Saat));
                    }
                }
                else if ((Pc.GetYear(Par._DateTimeVariableFinish.Value) == Pc.GetYear(Par._DateTimeVariableStart.Value)))
                {
                    var FinFinal = from _ in saatOnvanKarsList
                                   where Par._DateTimeVariableStart <= _.Tarikh && _.Tarikh <= Par._DateTimeVariableFinish
                                   group _ by Pc.GetMonth(_.Tarikh) into g
                                   orderby g.FirstOrDefault().Tarikh
                                   select new
                                   {

                                       tarikh = Pc.GetYear(g.FirstOrDefault().Tarikh).ToString() + "/" + Pc.GetMonth(g.FirstOrDefault().Tarikh).ToString()
                                          ,
                                       Saat = g.Sum(_ => _.SaatKar)
                                   };

                    foreach (var item in FinFinal)
                    {
                        chartvalue.Add(new KeyValuePair<string, int>(item.tarikh, item.Saat));

                    }
                }
                else
                {
                    var FinFinal = from _ in saatOnvanKarsList
                                   where Par._DateTimeVariableStart <= _.Tarikh && _.Tarikh <= Par._DateTimeVariableFinish
                                   group _ by Pc.GetYear(_.Tarikh) into g
                                   orderby g.FirstOrDefault().Tarikh
                                   select new
                                   {

                                       tarikh = Pc.GetYear(g.FirstOrDefault().Tarikh).ToString()
                                          ,
                                       Saat = g.Sum(_ => _.SaatKar)
                                   };

                    foreach (var item in FinFinal)
                    {
                        chartvalue.Add(new KeyValuePair<string, int>(item.tarikh, item.Saat));
                    }
                }

                SaatOnvanKar AddSaat(DateTime Tarikh, int saat)
                {
                    SaatOnvanKar _SaatOnvanKar = new SaatOnvanKar();
                    _SaatOnvanKar.Tarikh = Tarikh;
                    _SaatOnvanKar.SaatKar = saat;
                    return (_SaatOnvanKar);
                }

                DarAmadClusteredChart22.DataContext = chartvalue;



            }
            catch (Exception error)
            {
                SaveError(error);
            }

        }

        private void ddshahedefaaliatIadAvarBut2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                IadAvarRightToolbar.Visibility = Visibility.Visible;
                IadAvarRightToolbar.IsEnabled = true;
                GozareshSaatKarkardPanel.Visibility = Visibility.Visible;
                GozareshSaatKarkardPanel.IsEnabled = true;

            }
            catch (Exception error) { SaveError(error); }
        }

        private void PersianCalendarGozareshHazineh8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariable = PerCalendar.Date.start();
                akhsisHazinehTextBox3.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
                var ispresnt = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(x => x.Description == "تخصیص هزینه");
                if (ispresnt != null)
                {
                    ispresnt.Date = Par._DateTimeVariable;
                    ispresnt.PersinanDate = akhsisHazinehTextBox3.Text;
                    _FamilyManaerDBEntities.SaveChanges();
                }
                else
                {
                    _ComboBoxTbl.PersinanDate = akhsisHazinehTextBox3.Text;
                    _ComboBoxTbl.Date = Par._DateTimeVariable;
                    _ComboBoxTbl.Description = "تخصیص هزینه";
                    _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void PersianCalendarButGozareshHazineh9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Par._DateTimeVariableFinish = PerCalendar.Date.start();
                GozareshHazinehLBL24.Content = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void GozareshHazinehBut9_Click(object sender, RoutedEventArgs e) // ایجاد گزارش کارکرد - عنوان
        {
            try
            {
                if ((string.IsNullOrEmpty(GozareshHazinehLBL23.Content.ToString())) || (string.IsNullOrEmpty(GozareshHazinehLBL24.Content.ToString())))
                {
                    MajMessageBox.show("تاریخ شروع و پایان را مشخص نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                if (Par._DateTimeVariableStart > Par._DateTimeVariableFinish)
                {
                    MajMessageBox.show("تاریخ پایان باید بعد از تاریخ شروع باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;

                }
                List<KeyValuePair<string, int>> chartvaluePie = new List<KeyValuePair<string, int>>();
                List<KeyValuePair<string, int>> chartvalueLine = new List<KeyValuePair<string, int>>();
                List<SaatOnvanKar> saatOnvanKarsList = new List<SaatOnvanKar>();

                TimeSpan timeSpan = Par._DateTimeVariableFinish.Value - Par._DateTimeVariableStart.Value;
                DateTime startdate = Par._DateTimeVariableStart.Value;
                DateTime Yesterdaystartdate = Par._DateTimeVariableStart.Value.AddDays(-1);
                DateTime TommorowFinishdate = Par._DateTimeVariableFinish.Value.AddDays(2);


                DateTime Realdate = Par._DateTimeVariableStart.Value;
                int StartMonth = Par._DateTimeVariableStart.Value.Month;
                int StartYear = Par._DateTimeVariableStart.Value.Year;



                var FinOnvanKar = from _ in _FamilyManaerDBEntities.IadAvarTbls
                                  where (((Yesterdaystartdate <= _.StartDateTime) && (_.EndDateTime <= TommorowFinishdate) && (_.Periodic == false))
                                        || ((_.StartDateTime <= Par._DateTimeVariableStart) && (Par._DateTimeVariableStart <= _.PeriodicEndTime) && (_.Periodic == true))
                                        || ((_.StartDateTime <= TommorowFinishdate) && (TommorowFinishdate <= _.PeriodicEndTime) && (_.Periodic == true))
                                       || ((_.StartDateTime >= Par._DateTimeVariableStart) && (TommorowFinishdate >= _.PeriodicEndTime) && (_.Periodic == true)))
                                  select _;
                foreach (var itemOnvanKar in FinOnvanKar)
                {
                    int Hours = 0;
                    TimeSpan TimeSpanPeriod;
                    if (itemOnvanKar.Periodic == true)
                    {
                        int StartPeriodNumber = 0, FinishPeriodNumber = 0;
                        Hours = 0;
                        DateTime? NextStartPeriodTime = null, NexEndPeriodTime = null;

                        int HalfHours = 0;

                        for (int i = 0; i <= itemOnvanKar.PeriodNumBer; i++) // پیدا کردن شماره شروع دوره تلاقی یافته با بازه زمانی
                        {

                            switch (itemOnvanKar.PeriodocKind)
                            {
                                case "روز":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "ماه":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);


                                    break;
                                case "سال":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);


                                    break;
                            }

                            if (itemOnvanKar.StartDateTime.Value.Day != itemOnvanKar.EndDateTime.Value.Day)
                            {
                                if (NexEndPeriodTime.Value.ToShortDateString() == Par._DateTimeVariableStart.Value.ToShortDateString())  // کار دیروز شروع شده است
                                {
                                    HalfHours = NexEndPeriodTime.Value.Hour;
                                    StartPeriodNumber++;
                                    FinishPeriodNumber = StartPeriodNumber;
                                    break;
                                }
                            }


                            if (NextStartPeriodTime >= Par._DateTimeVariableStart)
                            {
                                FinishPeriodNumber = StartPeriodNumber;
                                break;
                            }
                            StartPeriodNumber++;


                        }
                        for (int i = StartPeriodNumber; i <= itemOnvanKar.PeriodNumBer - 1; i++) // پیدا کردن شماره پایان دوره تلاقی افته با بازه زمانی
                        {

                            switch (itemOnvanKar.PeriodocKind)
                            {
                                case "روز":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    //    NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddDays(itemOnvanKar.MeasurePeriodic.Value * i);
                                    break;
                                case "ماه":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);
                                    //     NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddMonths(itemOnvanKar.MeasurePeriodic.Value * i);


                                    break;
                                case "سال":
                                    NextStartPeriodTime = itemOnvanKar.StartDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);
                                    //      NexEndPeriodTime = itemOnvanKar.EndDateTime.Value.AddYears(itemOnvanKar.MeasurePeriodic.Value * i);


                                    break;
                            }
                            if (itemOnvanKar.StartDateTime.Value.Day != itemOnvanKar.EndDateTime.Value.Day)
                            {
                                if (NextStartPeriodTime.Value.ToShortDateString() == Par._DateTimeVariableFinish.Value.ToShortDateString()) // کار فردا تمام می شود
                                {
                                    HalfHours += 24 - itemOnvanKar.StartDateTime.Value.Hour;
                                    //   FinishPeriodNumber--;
                                    break;
                                }
                            }

                            //       if (FinishPeriodNumber==itemOnvanKar.PeriodNumBer)
                            //       {
                            //           break;
                            //       }
                            if (NextStartPeriodTime > Par._DateTimeVariableFinish)
                            {
                                //         FinishPeriodNumber++;
                                break;
                            }

                            FinishPeriodNumber++;
                        }
                        TimeSpanPeriod = (itemOnvanKar.EndDateTime.Value - itemOnvanKar.StartDateTime.Value);
                        Hours = ((TimeSpanPeriod.Hours) * (FinishPeriodNumber - StartPeriodNumber)) + HalfHours;


                    }
                    else if (itemOnvanKar.Periodic == false)
                    {

                        // کار دوره ایی نیست و شروع و پایان در یک روز است
                        if (itemOnvanKar.StartDateTime.Value.Day == itemOnvanKar.EndDateTime.Value.Day)
                        {
                            if (Par._DateTimeVariableStart.Value <= itemOnvanKar.StartDateTime.Value)
                            {
                                TimeSpanPeriod = (itemOnvanKar.EndDateTime.Value - itemOnvanKar.StartDateTime.Value);
                                Hours = TimeSpanPeriod.Hours;
                            }
                        }
                        // کار دوره ایی نیست و شروع و پایان در یک روز نیست
                        else if (itemOnvanKar.StartDateTime.Value.Day != itemOnvanKar.EndDateTime.Value.Day)
                        {
                            // کار امروز شروع شده و فردا تمام می شود -  تاریخ شروع امروز است - تاریخ پایان امروز است
                            if ((Par._DateTimeVariableFinish.Value.ToShortDateString() == Par._DateTimeVariableStart.Value.ToShortDateString()) && (Par._DateTimeVariableStart.Value.ToShortDateString() == itemOnvanKar.StartDateTime.Value.ToShortDateString()))
                            {
                                Hours = 24 - itemOnvanKar.StartDateTime.Value.Hour;
                            }

                            // کار دیروز شروع شده و امروز تمام می شود - تاریخ شروع امروز است
                            else if (Par._DateTimeVariableStart.Value.ToShortDateString() == itemOnvanKar.EndDateTime.Value.ToShortDateString())
                            {
                                Hours = itemOnvanKar.EndDateTime.Value.Hour;
                            }
                            // تاریخ شروع قبل از شروع کار و تاریخ  پایان بعد از پایان  کار است
                            else
                            {
                                TimeSpanPeriod = (itemOnvanKar.EndDateTime.Value - itemOnvanKar.StartDateTime.Value);
                                Hours = TimeSpanPeriod.Hours;
                            }
                        }




                    }
                    SaatOnvanKar _SaatOnvanKar = new SaatOnvanKar();
                    _SaatOnvanKar.OnvanKar = itemOnvanKar.TitleActivity;
                    _SaatOnvanKar.SaatKar = Hours;
                    saatOnvanKarsList.Add(_SaatOnvanKar);
                }
                var FinFinal = from _ in saatOnvanKarsList
                               group _ by _.OnvanKar into g
                               select new
                               {
                                   Onvan = g.FirstOrDefault().OnvanKar
                                   ,
                                   Saat = g.Sum(_ => _.SaatKar)
                               };
                foreach (var item in FinFinal)
                {
                    chartvaluePie.Add(new KeyValuePair<string, int>(item.Onvan, item.Saat));
                    chartvalueLine.Add(new KeyValuePair<string, int>(item.Onvan, item.Saat));

                }

                DarAmadClusteredChart2.DataContext = chartvaluePie;
                DarAmadClusteredChart3.DataContext = chartvalueLine;

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void DarAmadClusteredChart2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                MajMessageBox.show(DarAmadClusteredChart2.SelectedItem.ToString(), MajMessageBox.MajMessageBoxBut.OK);


            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }

        private void ddshahedefaaliatIadAvarBut223_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                IadAvarRightToolbar.Visibility = Visibility.Visible;
                IadAvarRightToolbar.IsEnabled = true;
                GozareshOnvanKarKardPanel.Visibility = Visibility.Visible;
                GozareshOnvanKarKardPanel.IsEnabled = true;

            }
            catch (Exception error) { SaveError(error); }
        }

        private void AmozeshUPToolBar_Click(object sender, RoutedEventArgs e)
        {
            Toolbarinvisible();
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            TakhsisRightToolbar.Visibility = Visibility.Visible;
            TakhsisRightToolbar.IsEnabled = true;
        }

        private void SabtefaaliatIadAvarBut1_Click(object sender, RoutedEventArgs e)
        {

            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            TakhsisPanel.Visibility = Visibility.Visible;
            TakhsisPanel.IsEnabled = true;
            KhosheBandi();

        }
        public void KhosheBandi()
        {
            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.khosheBandi != null
                      orderby _.ID descending
                      select _.khosheBandi;
            GozareshHazinehCombo8.ItemsSource= ListSabteOnvanDaramadP1.ItemsSource = Fin.ToList();
        }

        private void SabteOnvanDaramadPBut2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SabteOnvanDaramadPTextBox2.Text == "")
                {
                    MajMessageBox.show("لطفاً عنوان خوشه بندی را وارد نمایید.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }
                var ispersent = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.khosheBandi == SabteOnvanDaramadPTextBox2.Text);
                if (ispersent != null)
                {
                    MajMessageBox.show("این عنوان تکراری می باشد.", MajMessageBox.MajMessageBoxBut.OK);
                    return;
                }

                _ComboBoxTbl.khosheBandi = SabteOnvanDaramadPTextBox2.Text;
                _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                _FamilyManaerDBEntities.SaveChanges();
                EmptyPar();
                CleanOldDataEnteredTXT();
                KhosheBandi();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void SabteOnvanDaramadPBut4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object item = (object)ListSabteOnvanDaramadP1.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                // string onvan = (gridSabteOnvanDaramadP.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                var ispresent = _FamilyManaerDBEntities.ComboBoxTbls.Where(check => check.khosheBandi == item.ToString()).FirstOrDefault();
                ListSabteOnvanDaramadP1.SelectedItem = null;
                if (ispresent != null)
                {
                    var result = MajMessageBox.show("آیا از حذف عنوان زیر اطمینان دارید؟" + Environment.NewLine + ispresent.khosheBandi, MajMessageBox.MajMessageBoxBut.YESNO);
                    if (result == MajMessageBox.MajMessageBoxButResult.Yes)
                    {
                        _FamilyManaerDBEntities.ComboBoxTbls.Remove(ispresent);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
                KhosheBandi();
                EmptyPar();
            }
            catch (Exception error) { SaveError(error); }
        }


        private void MoshahedefaaliatIadAvarBut3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                ekhtesaskhsisHazinehPanel.Visibility = Visibility.Visible;
                ekhtesaskhsisHazinehPanel.IsEnabled = true;
                var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                          where _.khosheBandi != null
                          orderby _.ID descending
                          select _.khosheBandi;
                dGozareshHazinehCombo1.ItemsSource = Fin.ToList();
                if (dGozareshHazinehCombo1.Items.Count > 0)
                {
                    dGozareshHazinehCombo1.SelectedIndex = 0;
                }
                CreateTakhsisHszineh();
                CreateTakhsishazineh();
            }
            catch (Exception error)
            {
                SaveError(error);
            }

        }

        public void CreateTakhsisHszineh()
        {
            gridGozaeshVam2.Items.Clear();
            var Fin = from _ in _FamilyManaerDBEntities.FinancialTbls
                      where _.Takhsis == null && _.Cost != 0
                      orderby _.Datee descending
                      select _;
            foreach (var item in Fin)
            {
                gridGozaeshVam2.Items.Add(new { ID = item.ID, A1 = item.Title, A2 = item.PersianDate, A3 = item.Cost.Value.ToString("N0") });
            }
        }
        public void CreateTakhsishazineh()
        {
            gridNameVacmGirandeh2.Items.Clear();
            var Fin = from _ in _FamilyManaerDBEntities.FinancialTbls
                      where _.Takhsis == dGozareshHazinehCombo1.Text && _.Cost != 0
                      orderby _.Datee descending
                      select _;
            foreach (var item in Fin)
            {
                gridNameVacmGirandeh2.Items.Add(new { ID = item.ID, A1 = item.Title, A2 = item.PersianDate, A3 = item.Cost.Value.ToString("N0") });
            }
        }

        private void gridGozaeshVam2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                object item = (object)gridGozaeshVam2.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                int ID = int.Parse((gridGozaeshVam2.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text.ToString());
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridGozaeshVam2.SelectedItem = null;
                if (ispresent != null)
                {
                    ispresent.Takhsis = dGozareshHazinehCombo1.Text;
                    _FamilyManaerDBEntities.SaveChanges();
                }

                CreateTakhsisHszineh();
                CreateTakhsishazineh();

            }
            catch (Exception error) { SaveError(error); }
        }

        private void gridNameVacmGirandeh2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                object item = (object)gridNameVacmGirandeh2.SelectedItem;
                if (item == null) { MajMessageBox.show("لطفاً عنوان مورد نظر خود را انتخاب کنید.", MajMessageBox.MajMessageBoxBut.OK); return; }

                int ID = int.Parse((gridNameVacmGirandeh2.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text.ToString());
                var ispresent = _FamilyManaerDBEntities.FinancialTbls.Where(check => check.ID == ID).FirstOrDefault();
                gridNameVacmGirandeh2.SelectedItem = null;
                if (ispresent != null)
                {
                    ispresent.Takhsis = null;
                    _FamilyManaerDBEntities.SaveChanges();
                }
                CreateTakhsisHszineh();
                CreateTakhsishazineh();


            }
            catch (Exception error) { SaveError(error); }
        }

        private void dGozareshHazinehCombo1_DropDownClosed(object sender, EventArgs e)
        {
            CreateTakhsishazineh();
        }

        private void TanzimatIadAvarBut1_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            GzareshTakhsisHazinehPanel.Visibility = Visibility.Visible;
            GzareshTakhsisHazinehPanel.IsEnabled = true;
            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.khosheBandi != null
                      orderby _.ID descending
                      select _.khosheBandi;
            TakhsisHazinehCombo5.ItemsSource = Fin.ToList();
            PersianCalendar pc = new PersianCalendar();
            var Fin2 = from _ in _FamilyManaerDBEntities.FinancialTbls
                       orderby _.Datee.Value.Year
                       select _;
            int year = 0;
            foreach (var item in Fin2)
            {
                if (year != pc.GetYear(item.Datee.Value))
                {
                    year = pc.GetYear(item.Datee.Value);
                    TakhsisHazinehCombo4.Items.Add(year);
                }

            }
            if (TakhsisHazinehCombo4.Items.Count > 0)
            {
                TakhsisHazinehCombo4.SelectedIndex = TakhsisHazinehCombo4.Items.Count - 1;

            }

            var ispresnt = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(x => x.Description == "تخصیص هزینه");
            if (ispresnt != null)
            {
                akhsisHazinehTextBox3.Text = ispresnt.PersinanDate;
            }
            else
            {

            }

        }
        
        public void CreateGozareshTakhsis()
        {
            try
            {
                if ((TakhsisHazinehCombo5.Text != "") && (TakhsisHazinehCombo4.Text != "") && (TakhsisHazinehCombo3.Text != "") )
                {
                    int Mah = 0;
                    decimal Hazineh = 0, KolHazineh = 0, Daramad = 0;
                    string Tarikh = "";
                    switch (TakhsisHazinehCombo3.Text)
                    {
                        case "فروردین":
                            Mah = 1;
                            break;
                        case "اردیبهشت":
                            Mah = 2;
                            break;
                        case "خرداد":
                            Mah = 3;
                            break;
                        case "تیر":
                            Mah = 4;
                            break;
                        case "مرداد":
                            Mah = 6;
                            break;
                        case "شهریور":
                            Mah = 6;
                            break;
                        case "مهر":
                            Mah = 7;
                            break;
                        case "آبان":
                            Mah = 8;
                            break;
                        case "آذر":
                            Mah = 9;
                            break;
                        case "دی":
                            Mah = 10;
                            break;
                        case "بهمن":
                            Mah = 11;
                            break;
                        case "اسفند":
                            Mah = 12;
                            break;
                    }

                    SolidColorBrush RowBrush = new SolidColorBrush();


                    PersianCalendar pc = new PersianCalendar();
                    PersianCalendar pc2 = new PersianCalendar();
                    DateTime startDate = DateTime.Now;
                    DateTime today = DateTime.Now;
                    DateTime PersianstartDate = DateTime.Now;
                    DateTime startstartDate = DateTime.Now;
                    DateTime finishtDate = DateTime.Now;
                    DateTime shamsi = DateTime.Now;



                    startstartDate = pc.ToDateTime(int.Parse(TakhsisHazinehCombo4.Text), Mah, 1, 12, 30, 0, 0);
                    startDate = startstartDate;

                    if (today < startstartDate)
                    {
                        MajMessageBox.show("تاریخ مذکور فراتر از تاریخ امروز است.", MajMessageBox.MajMessageBoxBut.OK);
                        return;
                    }
                    finishtDate = startDate.AddDays(1);

                    TakhsisHazinehGrid1.Columns.Clear();
                    TakhsisHazinehGrid1.Items.Clear();
                    TakhsisHazinehGrid1.Columns.Add(new DataGridTextColumn { Header = "تاریخ", Binding = new System.Windows.Data.Binding("تاریخ") });
                    TakhsisHazinehGrid1.Columns.Add(new DataGridTextColumn { Header = "خرج کرد", Binding = new System.Windows.Data.Binding("Cost") });
                    TakhsisHazinehGrid1.Columns.Add(new DataGridTextColumn { Header = "سهمیه روزانه", Binding = new System.Windows.Data.Binding("سهمیه") });
                    TakhsisHazinehGrid1.Columns.Add(new DataGridTextColumn { Header = "مانده", Binding = new System.Windows.Data.Binding("مانده") });

                    int DayOfMonth = 0;
                    while (pc.GetMonth(startstartDate) == pc.GetMonth(startDate))
                    {
                        DayOfMonth++;
                        startDate = startDate.AddDays(1);
                    }
                    startDate = startstartDate;
                    while (pc.GetMonth(startstartDate) == pc.GetMonth(startDate))
                    {


                        var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                    where startDate.Year == p.Datee.Value.Year && startDate.Month == p.Datee.Value.Month && startDate.Day == p.Datee.Value.Day && p.Takhsis == TakhsisHazinehCombo5.Text
                                    orderby p.ID descending
                                    select p;

                        foreach (var F1 in Fin11)
                        {
                            if (F1.Income == 0)
                            {
                                Hazineh += F1.Cost.Value;
                                KolHazineh += Hazineh;
                                Tarikh = F1.PersianDate;
                            }

                        }
                        var FinDarAmad = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                                         where _.Month == TakhsisHazinehCombo3.Text && _.Year == TakhsisHazinehCombo4.Text && _.Onvan == TakhsisHazinehCombo5.Text
                                         select _;
                        foreach (var item in FinDarAmad)
                        {
                            Daramad += item.Income.Value;
                        }


                        string sahm;
                        int NumDay = pc2.GetDayOfMonth(shamsi);
                        shamsi = new DateTime(startDate.Year, startDate.Month, startDate.Day, 10, 35, 0);
                        string tarikh = pc2.GetYear(shamsi) + "/" + pc2.GetMonth(shamsi) + "/" + pc2.GetDayOfMonth(shamsi);
                        decimal Dessahm = (Daramad - KolHazineh) / DayOfMonth;
                        if (Dessahm <= 0)
                        {
                            sahm = "0";
                        }
                        else
                        {
                            sahm = Dessahm.ToString("N0");
                        }
                        DayOfMonth--;

                        if (Dessahm < Hazineh)
                        {
                            RowBrush.Color = Colors.Red;

                        }
                        else
                        {
                            RowBrush.Color = Colors.Green;

                        }


                        TakhsisHazinehGrid1.RowBackground = RowBrush;

                        TakhsisHazinehGrid1.Items.Add(new { تاریخ = tarikh, Cost = Hazineh.ToString("N0"), سهمیه = sahm, مانده = (Daramad - KolHazineh).ToString("N0") });
                        TakhsisHazinehTextBox1.Text = KolHazineh.ToString("N0");
                        Hazineh = 0;
                        Daramad = 0;

                        if ((today.Year == startDate.Year) && (today.Month == startDate.Month) && (today.Day == startDate.Day))
                        {
                            break;
                        }
                        startDate = startDate.AddDays(1);
                    }



                    var TarikhShoro = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(x => x.Description == "تخصیص هزینه");
                    if (TarikhShoro != null)
                    {
                        Hazineh = 0;
                        Daramad = 0;

                        var HazinehKole = from _ in _FamilyManaerDBEntities.FinancialTbls
                                          where _.Datee >= TarikhShoro.Date && _.Cost != 0 && _.Takhsis == TakhsisHazinehCombo5.Text
                                          select _;
                        foreach (var item in HazinehKole)
                        {
                            Hazineh += item.Cost.Value;
                        }



                        var DaramadKole = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                                          where _.GdateStart >= TarikhShoro.Date && _.Onvan == TakhsisHazinehCombo5.Text
                                          select _;
                        foreach (var item in DaramadKole)
                        {
                            Daramad += item.Income.Value;
                        }
                        TakhsisHazinehTextBox2.Text = (Daramad - Hazineh).ToString("N0");
                    }

                }

              

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        public void creatTakhsis()
        {
            try
            {
                int Mah = 0;
                if (TakhsisHazinehCombo8.Text != string.Empty && TakhsisHazinehCombo7.Text != string.Empty && TakhsisHazinehCombo6.Text != string.Empty)
                {
                    switch (TakhsisHazinehCombo6.Text)
                    {
                        case "فروردین":
                            Mah = 1;
                            break;
                        case "اردیبهشت":
                            Mah = 2;
                            break;
                        case "خرداد":
                            Mah = 3;
                            break;
                        case "تیر":
                            Mah = 4;
                            break;
                        case "مرداد":
                            Mah = 6;
                            break;
                        case "شهریور":
                            Mah = 6;
                            break;
                        case "مهر":
                            Mah = 7;
                            break;
                        case "آبان":
                            Mah = 8;
                            break;
                        case "آذر":
                            Mah = 9;
                            break;
                        case "دی":
                            Mah = 10;
                            break;
                        case "بهمن":
                            Mah = 11;
                            break;
                        case "اسفند":
                            Mah = 12;
                            break;
                    }

                    PersianCalendar pc = new PersianCalendar();
                    var FinDaramad = from _ in _FamilyManaerDBEntities.FinancialTbls
                                     where _.Income != 0
                                     select _;
                    decimal DarAmad = 0, DarAmadTakhssi = 0;
                    foreach (var item in FinDaramad)
                    {
                        if (pc.GetYear(item.Datee.Value) == int.Parse(TakhsisHazinehCombo7.Text) && pc.GetMonth(item.Datee.Value) == Mah)
                        {
                            DarAmad += item.Income.Value;

                        }
                    }
                    TakhsisHazinehTextBox4.Text = DarAmad.ToString("N0");

                    var FinDaramadTakhsisi = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                                             where _.Year == TakhsisHazinehCombo7.Text && _.Month == TakhsisHazinehCombo6.Text && _.Onvan== TakhsisHazinehCombo8.Text
                                             select _;
                    foreach (var item in FinDaramadTakhsisi)
                    {
                        DarAmadTakhssi += item.Income.Value;
                    }
                    TakhsisHazinehTextBox5.Text = DarAmadTakhssi.ToString("N0");




                    TakhsisHazinehGrid2.Columns.Clear();
                    TakhsisHazinehGrid2.Items.Clear();
                    TakhsisHazinehGrid2.Columns.Add(new DataGridTextColumn { Header = "عنوان خوشه", Binding = new System.Windows.Data.Binding("A") });
                    TakhsisHazinehGrid2.Columns.Add(new DataGridTextColumn { Header = "سقف هزینه", Binding = new System.Windows.Data.Binding("B") });
                    TakhsisHazinehGrid2.Columns.Add(new DataGridTextColumn { Header = "هزینه انجام شده", Binding = new System.Windows.Data.Binding("C") });
                    var Fin = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                              where _.Month == TakhsisHazinehCombo6.Text && _.Year == TakhsisHazinehCombo7.Text 
                              select _;
                    foreach (var item in Fin)
                    {
                        switch (TakhsisHazinehCombo6.Text)
                        {
                            case "فروردین":
                                Mah = 1;
                                break;
                            case "اردیبهشت":
                                Mah = 2;
                                break;
                            case "خرداد":
                                Mah = 3;
                                break;
                            case "تیر":
                                Mah = 4;
                                break;
                            case "مرداد":
                                Mah = 6;
                                break;
                            case "شهریور":
                                Mah = 6;
                                break;
                            case "مهر":
                                Mah = 7;
                                break;
                            case "آبان":
                                Mah = 8;
                                break;
                            case "آذر":
                                Mah = 9;
                                break;
                            case "دی":
                                Mah = 10;
                                break;
                            case "بهمن":
                                Mah = 11;
                                break;
                            case "اسفند":
                                Mah = 12;
                                break;
                        }

                        var FinDaramad1 = from _ in _FamilyManaerDBEntities.FinancialTbls
                                         where _.Income == 0 && _.Takhsis==item.Onvan
                                         select _;
                        DarAmadTakhssi = 0;
                        foreach (var item1 in FinDaramad1)
                        {
                            if (pc.GetYear(item1.Datee.Value) == int.Parse(TakhsisHazinehCombo7.Text) && pc.GetMonth(item1.Datee.Value) == Mah )
                            {
                                DarAmadTakhssi += item1.Cost.Value;

                            }

                        }
                        TakhsisHazinehGrid2.Items.Add(new { A = item.Onvan, B = item.Income, C = DarAmadTakhssi.ToString("N0") });

                    }


                }
            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        private void MoshahedefaaliatIadAvarBut1_Click(object sender, RoutedEventArgs e)

        {
            int Year = 0;
            int OldYear = 0;
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            TakhsisDaramadPanel.Visibility = Visibility.Visible;
            TakhsisDaramadPanel.IsEnabled = true;
            var Fin = from _ in _FamilyManaerDBEntities.ComboBoxTbls
                      where _.khosheBandi != null
                      select _.khosheBandi;
            TakhsisHazinehCombo8.ItemsSource = Fin.ToList();
            TakhsisHazinehCombo7.Items.Clear();
            var FinYear = from _ in _FamilyManaerDBEntities.FinancialTbls
                          where _.Income != 0
                          orderby _.Datee
                          select _;
            foreach (var item in FinYear)
            {
                PersianCalendar pc = new PersianCalendar();
                Year = pc.GetYear(item.Datee.Value);
                if (Year != OldYear)
                {
                    TakhsisHazinehCombo7.Items.Add(Year);
                    OldYear = Year;
                }


            }

        }

        private void TakhsisHazinehCombo8_DropDownClosed(object sender, EventArgs e)
        {
            creatTakhsis();
        }

        private void TakhsisHazinehCombo7_DropDownClosed(object sender, EventArgs e)
        {
            creatTakhsis();
        }

        private void TakhsisHazinehCombo6_DropDownClosed(object sender, EventArgs e)
        {
            creatTakhsis();
        }

     

        private void TakhsisHazinehTextBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                decimal number;
                if (decimal.TryParse(TakhsisHazinehTextBox5.Text, out number))
                {
                    if (number >= 99999999999)
                    {
                        number = 99999999999;
                    }
                    TakhsisHazinehTextBox5.Text = string.Format("{0:N0}", number);
                    TakhsisHazinehTextBox5.SelectionStart = TakhsisHazinehTextBox5.Text.Length;
                }


                
            }
            catch (Exception error) { SaveError(error); }
        }

        private void TakhsisHazinehTextBox5_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (TakhsisHazinehCombo8.Text != string.Empty && TakhsisHazinehCombo7.Text != string.Empty && TakhsisHazinehCombo6.Text != string.Empty)
                {
                    int Mah = 0;
                    switch (TakhsisHazinehCombo6.Text)
                    {
                        case "فروردین":
                            Mah = 1;
                            break;
                        case "اردیبهشت":
                            Mah = 2;
                            break;
                        case "خرداد":
                            Mah = 3;
                            break;
                        case "تیر":
                            Mah = 4;
                            break;
                        case "مرداد":
                            Mah = 6;
                            break;
                        case "شهریور":
                            Mah = 6;
                            break;
                        case "مهر":
                            Mah = 7;
                            break;
                        case "آبان":
                            Mah = 8;
                            break;
                        case "آذر":
                            Mah = 9;
                            break;
                        case "دی":
                            Mah = 10;
                            break;
                        case "بهمن":
                            Mah = 11;
                            break;
                        case "اسفند":
                            Mah = 12;
                            break;
                    }
                    Boolean Ok = true;
                    PersianCalendar _PersianCalendar = new PersianCalendar();
                    
                    DateTime SaveDate = _PersianCalendar.ToDateTime(int.Parse(TakhsisHazinehCombo7.Text), Mah, 1, 0, 0, 0, 0); //تبدیل شمسی به میلادی
                    var Fin = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                              where _.Month == TakhsisHazinehCombo6.Text && _.Year == TakhsisHazinehCombo7.Text && _.Onvan == TakhsisHazinehCombo8.Text
                              select _;

                    foreach (var item in Fin)
                    {
                        Ok = false;
                        decimal pool = 0;
                         decimal.TryParse(TakhsisHazinehTextBox5.Text, out pool);
                        item.Income = pool;

                        _FamilyManaerDBEntities.SaveChanges();
                    }

                    if (Ok)
                    {
                        _TakhsisDaramadKhoshe.IntPersianMonth = Mah;
                        _TakhsisDaramadKhoshe.Income = decimal.Parse(TakhsisHazinehTextBox5.Text);
                        _TakhsisDaramadKhoshe.Month = TakhsisHazinehCombo6.Text;
                        _TakhsisDaramadKhoshe.Year = TakhsisHazinehCombo7.Text;
                        _TakhsisDaramadKhoshe.Onvan = TakhsisHazinehCombo8.Text;
                        _TakhsisDaramadKhoshe.GdateStart = SaveDate;
                        _FamilyManaerDBEntities.TakhsisDaramadKhoshes.Add(_TakhsisDaramadKhoshe);
                        _FamilyManaerDBEntities.SaveChanges();
                    }
                }
            }
            catch (Exception error)
            {
                SaveError(error);

            }
            }

        private void TakhsisHazinehCombo5_DropDownClosed(object sender, EventArgs e)
        {
            CreateGozareshTakhsis();
        }

        private void TakhsisHazinehCombo4_DropDownClosed(object sender, EventArgs e)
        {
            CreateGozareshTakhsis();
        }

        private void TakhsisHazinehCombo3_DropDownClosed(object sender, EventArgs e)
        {
            CreateGozareshTakhsis();
        }

        private void PersianCalendarRepeatBut_Click(object sender, RoutedEventArgs e)
        {

            Par._DateTimeVariableStart = PerCalendar.Date.start();
            RepeatSabteIadAvarPanelTextBox2.Text = Date.PYear + "/" + Date.PMonth.ToString().PadLeft(2, '0') + "/" + Date.PDay.ToString().PadLeft(2, '0');
        }

        private void ProfileLeftToolbarPanelBut4_Click(object sender, RoutedEventArgs e)
        {
            EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
            ProfileLeftToolbarPanelVisible();
            UpdatePanel.Visibility = Visibility.Visible; UpdatePanel.IsEnabled = true;

        }
        public bool CheckInternetConnection()
        {
            try
            {
                bool check = true;
                string[] sitesList = new string[3] { "www.google.com", "www.microsoft.com", "www.yahoo.com" };
                Ping ping = new Ping();
                PingReply reply;
                int count = 0;
                for (int i = 0; i < sitesList.Length; i++)
                {
                    reply = ping.Send(sitesList[i]);
                    if (reply.Status == IPStatus.Success)
                        count += 1;
                }
                if (count > 0)
                    check = true;
                else
                    check = false;
                return check;
            }
            catch (Exception)
            {
                return false;
            }

        }


        private void ProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
        }
        private void Completed(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MajMessageBox.show("بروز رسانی انجام شد", MajMessageBox.MajMessageBoxBut.OK);
            System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }
        public Boolean update()
        {
            Boolean Updatee = false;
            if (CheckInternetConnection())
            {
                //    Version version = Assembly.GetExecutingAssembly().GetName().Version;
                //    string currentVer = version.ToString();

                //    //string xmlUrl = "http://update.xml";
                //    //XDocument xml = XDocument.Load(xmlUrl);
                //    //string newVer = string.Empty;
                //    //string description = string.Empty;

                //    //foreach (XElement element in xml.Descendants("Version"))
                //    //{
                //    //    newVer = element.Value.ToString();
                //    //}
                //    //foreach (XElement element in xml.Descendants("Description"))
                //    //{
                //    //    description = element.Value.ToString();
                //    //}
                //    if (currentVer == newVer)
                //        MoshakhasatManlabel77.Text = "نسخه جدید موجود نیست";
                //    else
                //    {
                //        Updatee = true;

                //        MoshakhasatManlabel33.Content = currentVer;
                //        MoshakhasatManlabel44.Content = newVer;
                //        MoshakhasatManlabel77.Text = description;


                //    }
            }
            else
                MajMessageBox.show("اتصال به اینرتنت را برقرار کنید", MajMessageBox.MajMessageBoxBut.OK);
            return Updatee;
        }




        private void Button_Click(object sender, RoutedEventArgs e) // دکمه به روز رسانی
        {
            if (update())
            {
                UpdateButton.IsEnabled = false;
                //lblCheck.Content = "نسخه جدید موجود است،در حال بروز رسانی . . .";
                WebClient webClient = new WebClient();

                webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager_old.exe"))
                {
                    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager_old.exe");
                    File.Move(System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager.exe", System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager_old.exe");
                }
                else
                    File.Move(System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager.exe", System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager_old.exe");

                webClient.DownloadFileAsync(new Uri("http://AbiAccounting.exe"), System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager.exe");
                File.SetAttributes(System.AppDomain.CurrentDomain.BaseDirectory + "\\FamilyManager_old.exe", FileAttributes.Hidden);
            } 


        }

        private void ddshahedefaaliatIadAvarBut1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Year = 0, OldYear = 0;
                EmptyPar(); Panelinvisible(); CleanOldDataEnteredTXT();
                GozareshTakhsisBodjePanel.Visibility = Visibility.Visible;
                GozareshTakhsisBodjePanel.IsEnabled = true;
                KhosheBandi();
                GozareshHazinehCombo11.Items.Clear();
                var FinYear = from _ in _FamilyManaerDBEntities.FinancialTbls
                              where _.Income != 0
                              orderby _.Datee
                              select _;
                foreach (var item in FinYear)
                {
                    PersianCalendar pc = new PersianCalendar();
                    Year = pc.GetYear(item.Datee.Value);
                    if (Year != OldYear)
                    {
                        GozareshHazinehCombo11.Items.Add(Year);
                        OldYear = Year;
                    }

                }
                GozarshNahaiyKhoshe();
            }
            catch (Exception error) { SaveError(error); }
        }

        private void GozareshHazinehCombo8_DropDownClosed(object sender, EventArgs e)
        {
            GozarshNahaiyKhoshe();
        }

        private void GozareshHazinehCombo11_DropDownClosed(object sender, EventArgs e)
        {
            GozarshNahaiyKhoshe();
        }
        private void GozarshNahaiyKhoshe()
        {
            try
            {
                if ((GozareshHazinehCombo8.Text != "") && (GozareshHazinehCombo11.Text != ""))
                {
                    List<KeyValuePair<int, decimal>> chartvalueDaramad = new List<KeyValuePair<int, decimal>>();
                    List<KeyValuePair<int, decimal>> chartvaluehazineh = new List<KeyValuePair<int, decimal>>();

                    decimal Hazineh = 0,  Daramad = 0;
                    PersianCalendar pc = new PersianCalendar();
                    for (int i = 0; i < 12; i++)
                    {
                        string Date = GozareshHazinehCombo11.Text + "/" + (i+1).ToString("00");

                           Hazineh = 0;
                        Daramad = 0;

                        var Fin11 = from p in _FamilyManaerDBEntities.FinancialTbls
                                    where p.PersianDate.Contains(Date) && p.Takhsis == GozareshHazinehCombo8.Text
                                    select p;
                        foreach (var item in Fin11)
                        {
                            Hazineh += item.Cost.Value;
                        }
                        chartvaluehazineh.Add(new KeyValuePair<int, decimal>(i+1, Hazineh));


                        var FinDarAmad = from _ in _FamilyManaerDBEntities.TakhsisDaramadKhoshes
                                         where _.Year == GozareshHazinehCombo11.Text && _.IntPersianMonth == i + 1 && _.Onvan == GozareshHazinehCombo8.Text
                                         select _ ; 
                        foreach (var item in FinDarAmad)
                        {
                            Daramad += item.Income.Value;
                        }
                        chartvalueDaramad.Add(new KeyValuePair<int, decimal>(i+1, Daramad));
                    }
                    DarAmadClusteredChart5.DataContext = chartvalueDaramad;
                    HazinehClusteredChart1.DataContext = chartvaluehazineh;
                }

            }
            catch (Exception error)
            {
                SaveError(error);
            }
        }
        private void GozareshHazinehCombo11_DropDownClosed_1(object sender, EventArgs e)
        {
            GozarshNahaiyKhoshe();
        }

        private void UpdateButton1_Click(object sender, RoutedEventArgs e) // انجام خرید نرم افزار
        {
            if (CheckInternetConnection())
            {
                Boolean reg = false;


                /// تابع خرید اینجا قرار گیرد
                //  MoshakhasatManlabel13.Text= مشخصات خرید انجام شده


                reg = true;
                if (reg)
                {
                    var ispresentGUID = _FamilyManaerDBEntities.ComboBoxTbls.FirstOrDefault(_ => _.Description == "GUID");

                    _ComboBoxTbl.Description = "HardwareCode";
                  //  _ComboBoxTbl.SpecialCode = Convert.ToBase64String(HashMe(GetHardwarSerial()));
                    _ComboBoxTbl.File = HashMe(ispresentGUID.SpecialCode + GetHardwarSerial());
                    _FamilyManaerDBEntities.ComboBoxTbls.Add(_ComboBoxTbl);
                    _FamilyManaerDBEntities.SaveChanges();
                    System.Windows.Forms.Application.Restart();
                    System.Windows.Application.Current.Shutdown();

                }

            }
            else
            {
                MajMessageBox.show("به اینترنت وصل نیستید", MajMessageBox.MajMessageBoxBut.OK);
            }

        }
    }
}
    






//            try
//            {

//            }
//            catch (Exception error)
//            {
//             SaveError(error);
//            }






    