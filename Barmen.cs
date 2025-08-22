using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;


namespace Barmen
{
    public partial class Barmen : Form
    {
        XDocument SaveFilePers = new XDocument();
        WorldDate date = new WorldDate();

        //Навыки
        int Trade
        {
            get { return Convert.ToInt32(textBox_LevelTrade.Text); }
            set { textBox_LevelTrade.Text = value.ToString(); }
        }
        int Performance
        {
            get { return Convert.ToInt32(textBox_LevelPerformance.Text); }
            set { textBox_LevelPerformance.Text = value.ToString(); }
        }
        int Vigilance
        {
            get { return Convert.ToInt32(textBox_LevelVigilance.Text); }
            set { textBox_LevelVigilance.Text = value.ToString(); }
        }
        int Persuasiveness
        {
            get { return Convert.ToInt32(textBox_LevelPersuasiveness.Text); }
            set { textBox_LevelPersuasiveness.Text = value.ToString(); }
        }
        //Насисление опыта
        int Experience(int MyLevel, int HisLivel) { return (Convert.ToInt32((HisLivel - MyLevel + 5) / 2)); }
        
        //Деньги
        int Money
        {
            get { return Convert.ToInt32(textBox_Money.Text); }
            set { textBox_Money.Text = value.ToString(); }
        }
        int CostFlat
        {
            get { return Convert.ToInt32(textBox_CostFlat.Text); }
            set { textBox_CostFlat.Text = value.ToString(); }
        }
        int CostFood
        {
            get { return Convert.ToInt32(textBox_CostFood.Text); }
            set { textBox_CostFood.Text = value.ToString(); }
        }
        //Работа
        int Days
        {
            get { return Convert.ToInt32(textBox_Days.Text); }
            set { textBox_Days.Text = value.ToString(); }
        }
        int Weekends
        {
            get { return Convert.ToInt32(textBox_Weekends.Text); }
            set { textBox_Weekends.Text = value.ToString(); }
        }
        int Popularity
        {
            get { return Convert.ToInt32(textBox_Popularity.Text); }
            set { textBox_Popularity.Text = value.ToString(); }
        }
        int CostSoldMugs
        {
            get { return Convert.ToInt32(textBox_CostSoldMugs.Text); }
            set { textBox_CostSoldMugs.Text = value.ToString(); }
        }  

        int CostSoldBarels
        {
            get { return Convert.ToInt32(textBox_CostBarelsCost.Text); }
            set { textBox_CostBarelsCost.Text = value.ToString(); }
        }
        int PartSoldMugs
        {
            get { return Convert.ToInt32(textBox_PartSoldMugs.Text); }
            set { textBox_PartSoldMugs.Text = value.ToString(); }
        }
        int PartSoldBarels
        {
            get { return Convert.ToInt32(textBox_PartSoldBarels.Text); }
            set { textBox_PartSoldBarels.Text = value.ToString(); }
        }
        int Prize
        {
            get { return Convert.ToInt32(textBox_Prize.Text); }
            set { textBox_Prize.Text = value.ToString(); }
        }
        string WRes
        {
            get { return label_WeekendResult.Text; }
            set { label_WeekendResult.Text = value; }
        }

        const int citiPopular = 12;
        const double PopularK = 1.6;
        int WorkDayBaforeWeekend = 6;

        Random random = new Random();
        public int d2 { get { return random.Next(1, 3); } }
        public int d3 { get { return random.Next(1, 4); } }
        public int d6 { get { return random.Next(1, 7); } }
        public int d100 { get { return random.Next(1, 101); } }

        public int Dices(int num)
        {
            int result = 0;
            for (int i = 0; i < num; i++)
                result += d6;
            return result;
        }

        void NextDay() {date.NextDay();DateRenew();}

        void DateRenew()
        {
            int _day = date.day - 1, _month = date.month - 1, _year = date.year;
            comboBox_Month.Visible = false;
            comboBox_Day.Visible = false;
            textBox_Year.Visible = false;

            comboBox_Month.SelectedIndex = _month ;
            comboBox_Day.SelectedIndex = _day;
            textBox_Year.Text = _year.ToString();

            comboBox_Month.Visible = true;
            comboBox_Day.Visible = true;
            textBox_Year.Visible = true;
        }


        //Работать несколько дней
        private void button_Work_Click(object sender, EventArgs e)
        {
            //Активация элементов формы
            label_SoldMug.Visible = true;
            label_SoldMug.Text = "0";
            label_SoldBarels.Visible = true;
            label_SoldBarels.Text = "0";
            label_Parties.Visible = true;
            label_Parties.Text = "0";
            label_Tips.Visible = true;
            label_Tips.Text = "0";
            label_Salaray.Visible = true;
            label_Salaray.Text = "0";
            label_Bayers.Visible = true;
            label_Bayers.Text = "0";

            int NumDices = Convert.ToInt32(Convert.ToDouble(Popularity) * PopularK);
            int SoldMugs = 0, soldBarels = 0, Tips = 0, Salary = 0;
            //Получение стоимости


            for (int i = 1; i <= Days; i++)
            {//Рабочий день
                int NumBuyes = Dices(NumDices), Party=0; //Число посетителей
                int dicePer = 0, diceСheck = 0, TradeСheck = 0, resPers = 0, resСheck = 0;
                //Торговля
                for (int j=0; j<NumBuyes; j++)
                {
                    do
                    {
                        dicePer = d6;
                        diceСheck = d6;
                        TradeСheck = d3 + 1;
                        resPers = dicePer + Trade;
                        resСheck = TradeСheck + diceСheck;
                    }
                    while (resPers == resСheck);
                    //Experience Trade
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
                    //Продажа кружки и чаевые
                    if (resPers > resСheck)
                    {
                        SoldMugs++;
                        if (dicePer - diceСheck == 5) Tips++;
                        if (resPers - resСheck == 8) Tips++;
                    }
                    //Убеждение всё-таки купить
                    else if(checkBox_ConvinceSome.Checked)
                    {
                        if (d100 == 1) Popularity--; //Штраф
                        int PersuasivenessCheck = 0;
                        do
                        {
                            dicePer = d6;
                            diceСheck = d6;
                            PersuasivenessCheck = d3 + 2;
                            resPers = dicePer + Persuasiveness;
                            resСheck = PersuasivenessCheck + diceСheck;
                        }
                        while (resPers == resСheck);
                        //Experience Persuasiveness
                        try { progressBar_Persuasiveness.Value += Experience(Persuasiveness, PersuasivenessCheck); }
                        catch { { if (d2 == 2) Persuasiveness++; progressBar_Persuasiveness.Value = 0; } }

                        if (resPers > resСheck)
                        {
                            do
                            {
                                dicePer = d6;
                                diceСheck = d6;
                                TradeСheck = d3 + 1;
                                resPers = dicePer + Trade;
                                resСheck = TradeСheck + diceСheck;
                            }
                            while (resPers == resСheck);
                            //Experience Trade
                            try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                            catch {  if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
                            //Продажа кружки
                            if (resPers > resСheck) SoldMugs++;                            
                        }
                    }

                    //Убеждение Купить вторую
                    if (checkBox_ConvinceAll.Checked)
                    {
                        if (d100 == 1) Popularity--; //Штраф
                        int VigilanceCheck = 0;
                        do
                        {
                            dicePer = d6;
                            diceСheck = d6;
                            VigilanceCheck = d3 + 2;
                            resPers = dicePer + Vigilance;
                            resСheck = VigilanceCheck + diceСheck;
                        }
                        while (resPers == resСheck);
                        //Experience Vigilance
                        try { progressBar_Vigilance.Value += 2 * Experience(Vigilance, VigilanceCheck); }
                        catch { if (d2 == 2) Vigilance++; progressBar_Vigilance.Value = 0; }

                        if (resPers > resСheck)
                        {
                            do
                            {
                                dicePer = d6;
                                diceСheck = d6;
                                TradeСheck = d3 + 1;
                                resPers = dicePer + Trade;
                                resСheck = TradeСheck + diceСheck;
                            }
                            while (resPers == resСheck);
                            //Продажа кружки
                            if (resPers > resСheck) SoldMugs++;
                        }
                    }                    
                }
                // Приход компании
                if (d6 > 4)
                {
                    Party++;
                    do
                    {
                        dicePer = d6;
                        diceСheck = d6;
                        TradeСheck = d3 + 4;
                        resPers = dicePer + Trade;
                        resСheck = TradeСheck + diceСheck;
                    }
                    while (resPers == resСheck);

                    //Продажа бочёнка и чаевые
                    if (resPers > resСheck)
                    {
                        soldBarels++;
                        if (dicePer - diceСheck == 5) Tips+=2*d6;
                    }
                    //Experience Trade
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
                }

                //Обновление результатов
                label_Bayers.Text = Convert.ToString(Convert.ToInt32(label_Bayers.Text) + NumBuyes);
                label_SoldMug.Text = SoldMugs.ToString();
                label_Parties.Text = Convert.ToString(Convert.ToInt32(label_Parties.Text) + Party);
                label_SoldBarels.Text = soldBarels.ToString();
                label_Tips.Text = "Чаевые:" + Tips.ToString();
                //Зарплата
                Salary = Convert.ToInt32(SoldMugs / PartSoldMugs * CostSoldMugs) +
                    Convert.ToInt32(soldBarels / PartSoldBarels * CostSoldBarels);
                label_Salaray.Text = "Зарплата: "+Salary.ToString();
                //Отсчёт выходных                
                if (--WorkDayBaforeWeekend <= 0) { WorkDayBaforeWeekend = 6; Weekends++; }
                //Бытовые расходы
                Money -= CostFlat + CostFood;

                NextDay();
                //Конец рабочего дня
            }
            //Денюжка
            Money += Salary + Tips;
        }

        private void textBox_LevelPerformance_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox && (sender as TextBox).TextLength != 0)
            {
            Performance = Convert.ToInt32(textBox_LevelPerformance.Text);
            progressBar_Performance.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Performance - 1));
            }
        }
        private void textBox_LevelVigilance_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox && (sender as TextBox).TextLength != 0)
            {
                Vigilance = Convert.ToInt32(textBox_LevelVigilance.Text);
                progressBar_Vigilance.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Vigilance - 1));
            }
        }
        private void textBox_LevelTrade_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox && (sender as TextBox).TextLength != 0)
            {
            Trade = Convert.ToInt32(textBox_LevelTrade.Text);
            progressBar_Trade.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Trade - 1));
            }
        }

        private void textBox_Popularity_TextChanged(object sender, EventArgs e)
        {
            if(sender is TextBox && (sender as TextBox).TextLength != 0)
                Popularity = Convert.ToInt32(textBox_Popularity.Text);
        }

        private void button_WorkAWeek_Click(object sender, EventArgs e)
        {
            Days = 6;
            button_Work.PerformClick();
        }

        //Контроль ввода только цифр
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }

        }
        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
                if ((sender as TextBox).Text.Length == 0)
                    (sender as TextBox).Text = "0";
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
                if ((sender as TextBox).Text.Length == 0)
                    (sender as TextBox).Text = "0";
        }
        public Barmen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Спрятать элементы
            label_SoldMug.Visible = false;
            label_SoldBarels.Visible = false;
            label_Parties.Visible = false;
            label_Tips.Visible = false;
            label_Salaray.Visible = false;
            label_Bayers.Visible = false;
            comboBox_Month.SelectedIndex = 0;
            comboBox_Day.SelectedIndex = 0;
        }

        private void button_Prize_Click(object sender, EventArgs e)
        {
            Money += Prize;
        }

        private void button_BuyHillTea_Click(object sender, EventArgs e)
        {
            if (Money > 9)
            {
                Money -= 10;
                if (progressBar_Damage.Value > 0) progressBar_Damage.Value--;
            }
        }

        private void button_RestDay_Click(object sender, EventArgs e)
        {
            if (Weekends > 0)
            {
                Weekends--;
                if (progressBar_Damage.Value > 0) progressBar_Damage.Value--;
                NextDay();

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
            }
        }

        private void button_BuyHillOil_Click(object sender, EventArgs e)
        {
            if (Money > 29)
            {
                Money -= 30;
                progressBar_Damage.Value = 0;
            }
        }

        private void textBox_Year_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox && (sender as TextBox).TextLength != 0)
                date.year = Convert.ToInt32(textBox_Year.Text);
        }

        private void comboBox_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Month.Visible == true)
                date.month = comboBox_Month.SelectedIndex +1;
        }

        private void comboBox_Day_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox_Day.Visible == true)
                date.day = comboBox_Day.SelectedIndex+1;
        }

        private void button_WorkWeekend_Click(object sender, EventArgs e)
        {
            if(Weekends >0)
            {
                WRes = "Славно отдохнул ";
                Weekends--; NextDay();
                int buf = Days;
                Days = 1;
                WorkDayBaforeWeekend++;
                button_Work.PerformClick();
                Days = buf;

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
            }
            else WRes = "Некогда ";

        }

        private void button_MakeShow_Click(object sender, EventArgs e)
        {
            if (Weekends > 0 && progressBar_Damage.Value < 3)
            {
                Weekends--; NextDay();
                int dicePer = 0, diceСheck = 0, PerformanceCheck = 0, TradeСheck = 0, resPers = 0, resСheck = 0;

                do
                {
                    dicePer = d6;
                    diceСheck = d6;
                    PerformanceCheck = d3 + 4;
                    resPers = dicePer + Performance;
                    resСheck = PerformanceCheck + diceСheck;
                }
                while (resPers == resСheck);

                //Experience Performance
                try { progressBar_Performance.Value += 5 * Experience(Performance, PerformanceCheck); }
                catch
                {
                    if (progressBar_Performance.Value >= progressBar_Performance.Maximum)
                        if (d2 == 2) Performance++; progressBar_Performance.Value = 0;
                }


                if (resPers > resСheck)
                {
                    WRes = "Заманил клиентов ";
                    do
                    {
                        dicePer = d6;
                        diceСheck = d6;
                        TradeСheck = d3 + 4;
                        resPers = dicePer + Trade;
                        resСheck = TradeСheck + diceСheck;
                    }
                    while (resPers == resСheck);

                    //Продажа бочёнка и чаевые
                    if (resPers > resСheck)
                    {
                        WRes += "и втюхал :)";
                        Money += Convert.ToInt32(CostSoldBarels / PartSoldBarels);
                        if (dicePer - diceСheck == 5) Money += 10;
                    }
                    else WRes += "но не втюхал :'(";
                    //Experience Trade
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
                }

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
            }
            else WRes = "Не получится ";
        }

        private void button_Marketing_Click(object sender, EventArgs e)
        {
            if (Weekends > 0 && Popularity <= citiPopular && progressBar_Damage.Value < 3)
            {
                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);

                Weekends--; NextDay();
                bool damage = false, hit = false;
                int dicePer = 0, diceСheck = 0, PerformanceCheck = 0, resPers = 0, resСheck = 0;

                //Убедил ли
                do
                {
                    dicePer = d6;
                    diceСheck = d6;
                    PerformanceCheck = 1 + d3;
                    resPers = dicePer + Performance;
                    resСheck = PerformanceCheck + diceСheck;
                }
                while (resPers == resСheck);
                if (resPers > resСheck) { hit = true; WRes = "Убедил "; }
                else WRes = "Не убедил ";

                //Experience Performance
                try { progressBar_Performance.Value += 2 * Experience(Performance, PerformanceCheck); }
                catch
                {
                    if (progressBar_Performance.Value >= progressBar_Performance.Maximum)
                        if (d2 == 2) Performance++; progressBar_Performance.Value = 0;
                }

                //Заметили ли
                do
                {
                    dicePer = d6;
                    diceСheck = d6;
                    PerformanceCheck = 5 + Popularity - citiPopular; //Внимательность
                    resPers = dicePer + Performance;
                    resСheck = PerformanceCheck + diceСheck;
                }
                while (resPers == resСheck);
                if (resPers < resСheck) {damage = true; WRes += "и побили ";}
                else WRes += "и не заметили ";

                //Experience Performance
                try { progressBar_Performance.Value += 2 * Experience(Performance, PerformanceCheck); }
                catch
                {
                    if (progressBar_Performance.Value >= progressBar_Performance.Maximum)
                        if (d2 == 2) Performance++; progressBar_Performance.Value = 0;
                }

                if (hit && !damage) { Popularity++; WRes = "Успех "; }
                if (damage) progressBar_Damage.Value++;
            }
        }

        void AntiMarketing(int TheirPerformance, bool Look = false, int OurVigilance = 0)
        {
            bool damage = false, hit = false;
            int dicePer = 0, diceСheck = 0, PerformanceCheck = 0, resPers = 0, resСheck = 0;
            int diceTheir = 0, resTheir = 0;
            //Убедили ли
            do
            {
                diceTheir = d6;
                diceСheck = d6;
                PerformanceCheck = 2 + d3;
                resTheir = diceTheir + TheirPerformance;
                resСheck = PerformanceCheck + diceСheck;
            }
            while (resTheir == resСheck);
            if (resTheir > resСheck) hit = true;

            //Заметили ли
            if (Look)
            {
                do
                {
                    dicePer = d6;
                    diceTheir = d6;
                    resPers = dicePer + Vigilance;
                    resTheir = TheirPerformance + diceTheir;
                }
                while (resPers == resTheir);
                if (resPers > resTheir) { damage = true; WRes += "они отгребли ;)"; }

                    //Experience Vigilance
                    try { progressBar_Vigilance.Value += 5 * Experience(Vigilance, TheirPerformance); }
                catch
                {
                    if (progressBar_Vigilance.Value >= progressBar_Vigilance.Maximum)
                        if (d2 == 2) Vigilance++; progressBar_Vigilance.Value = 0;
                }
            }

            if (hit && !damage) { Popularity--; WRes += "Нас опустили "; }
        }

        private void button_LookAtBar_Click(object sender, EventArgs e)
        {
            if (Weekends > 0)
            {
                WRes = "Смотрю за баром ";
                Weekends--; NextDay();
                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity,true,Vigilance);
            }
            else WRes = "Не получится ";
        }

        private void button_SaveFile_Click(object sender, EventArgs e)
        {
            if (File.Exists("Pers.xml")) File.Delete("Pers.xml");

            XElement Pers = new XElement("Persone");
            Pers.Add(new XAttribute("Trade", Trade));
            Pers.Add(new XAttribute("Performance", Performance));
            Pers.Add(new XAttribute("Vigilance", Vigilance));
            Pers.Add(new XAttribute("Persuasiveness", Persuasiveness));
            Pers.Add(new XAttribute("progressBar_Trade", progressBar_Trade.Value));
            Pers.Add(new XAttribute("progressBar_Performance", progressBar_Performance.Value));
            Pers.Add(new XAttribute("progressBar_Vigilance", progressBar_Vigilance.Value));
            Pers.Add(new XAttribute("progressBar_Persuasiveness", progressBar_Persuasiveness.Value));

            Pers.Add(new XAttribute("Popularity", Popularity));
            Pers.Add(new XAttribute("PartSoldMugs", PartSoldMugs));
            Pers.Add(new XAttribute("PartSoldBarels", PartSoldBarels));
            Pers.Add(new XAttribute("CostSoldMugs", CostSoldMugs));
            Pers.Add(new XAttribute("CostSoldBarels", CostSoldBarels));

            Pers.Add(new XAttribute("textBox_Year", textBox_Year.Text));
            Pers.Add(new XAttribute("comboBox_Month", comboBox_Month.SelectedIndex));
            Pers.Add(new XAttribute("comboBox_Day", comboBox_Day.SelectedIndex));

            Pers.Add(new XAttribute("progressBar_Damage", progressBar_Damage.Value));
            Pers.Add(new XAttribute("Weekends", Weekends));
            Pers.Add(new XAttribute("Money", Money));
            Pers.Add(new XAttribute("CostFlat", CostFlat));
            Pers.Add(new XAttribute("CostFood", CostFood));

            SaveFilePers = new XDocument();
            SaveFilePers.Add(Pers);
            SaveFilePers.Save("Pers.xml");
        }

        private void button_LoadFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists("Pers.xml")) return;

            SaveFilePers = XDocument.Load("Pers.xml");
            XElement Pers = SaveFilePers.Root;

            Trade = Convert.ToInt32(Pers.Attribute("Trade").Value);
            Performance = Convert.ToInt32(Pers.Attribute("Performance").Value);
            Vigilance = Convert.ToInt32(Pers.Attribute("Vigilance").Value);
            Persuasiveness = Convert.ToInt32(Pers.Attribute("Persuasiveness").Value);
            progressBar_Trade.Value = Convert.ToInt32(Pers.Attribute("progressBar_Trade").Value);
            progressBar_Performance.Value = Convert.ToInt32(Pers.Attribute("progressBar_Performance").Value);
            progressBar_Vigilance.Value = Convert.ToInt32(Pers.Attribute("progressBar_Vigilance").Value);
            progressBar_Persuasiveness.Value = Convert.ToInt32(Pers.Attribute("progressBar_Persuasiveness").Value);

            Popularity = Convert.ToInt32(Pers.Attribute("Popularity").Value);
            PartSoldMugs = Convert.ToInt32(Pers.Attribute("PartSoldMugs").Value);
            PartSoldBarels = Convert.ToInt32(Pers.Attribute("PartSoldBarels").Value);
            CostSoldMugs = Convert.ToInt32(Pers.Attribute("CostSoldMugs").Value);
            CostSoldBarels = Convert.ToInt32(Pers.Attribute("CostSoldBarels").Value);

            textBox_Year.Text = Convert.ToString(Pers.Attribute("textBox_Year").Value);
            comboBox_Month.SelectedIndex = Convert.ToInt32(Pers.Attribute("comboBox_Month").Value);
            comboBox_Day.SelectedIndex = Convert.ToInt32(Pers.Attribute("comboBox_Day").Value);

            progressBar_Damage.Value = Convert.ToInt32(Pers.Attribute("progressBar_Damage").Value);
            Weekends = Convert.ToInt32(Pers.Attribute("Weekends").Value);
            Money = Convert.ToInt32(Pers.Attribute("Money").Value);
            CostFlat = Convert.ToInt32(Pers.Attribute("CostFlat").Value);
            CostFood = Convert.ToInt32(Pers.Attribute("CostFood").Value);

            
        }
    }

    // Используется немного отличный от нашего календарь. 
    // Во всех месяцах 27 дней, временная линия разделена на неравномерные циклы привязанные к историческим событиям 
    class WorldDate
    {
        static string[] months = { "Январь", "Февраль", "Март", "Апрель",
                                  "Май", "Июнь", "Июль", "Август",
                                  "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public int cycle;
        public int year;
        public int month;
        public int day;

        public WorldDate(int Cycle = 6,int Year = 182, int Month=9, int Day=3)
        {
            cycle = Cycle;
            year = Year;
            month = Month;
            day = Day;
        }

        public void NextDay()
        {
            if (day == 27)
            {
                day = 1;
                NextMonth();
            }
            else day++;
        }
        public void NextMonth()
        {
            if (month == 12)
            {
                month = 1;
                NextYear();
            }
            else month++;
        }
        public void NextYear()
        {
            year++;
        }

        public string Date()
        {
            return cycle.ToString() + "." + year.ToString() + "г. "
                + day.ToString() + " " + months[month-1];
        }
    }


}
