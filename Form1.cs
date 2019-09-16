using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Barmen
{
    public partial class Form1 : Form
    {
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

        void DateRenew()
        {
            comboBox_Month.SelectedIndex = date.month - 1;
            comboBox_Day.SelectedIndex = date.day - 1;
            textBox_Year.Text = Convert.ToString(date.year);
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
                    //Experience
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (progressBar_Trade.Value >= progressBar_Trade.Maximum) { int d = d2; if (d == 2) Trade++; progressBar_Trade.Value = 0; } }
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
                        //Experience
                        try { progressBar_Persuasiveness.Value += Experience(Persuasiveness, PersuasivenessCheck); }
                        catch { if (progressBar_Persuasiveness.Value >= progressBar_Persuasiveness.Maximum) { int d = d2; if (d == 2) Persuasiveness++; progressBar_Persuasiveness.Value = 0; } }

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
                            //Experience
                            try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                            catch { if (progressBar_Trade.Value >= progressBar_Trade.Maximum) { int d = d2; if (d == 2) Trade++; progressBar_Trade.Value = 0; } }
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
                        //Experience
                        try { progressBar_Vigilance.Value += Experience(Vigilance, VigilanceCheck); }
                        catch { if (progressBar_Vigilance.Value >= progressBar_Vigilance.Maximum) { int d = d2; if (d == 2) Vigilance++; progressBar_Vigilance.Value = 0; } }

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
                        if (dicePer - diceСheck == 5) Tips+=10;
                    }
                    //Experience
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (progressBar_Trade.Value >= progressBar_Trade.Maximum) if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
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

                date.NextDay(); DateRenew();
                //Конец рабочего дня
            }
            //Денюжка
            Money += Salary + Tips;
        }

        private void textBox_LevelPerformance_TextChanged(object sender, EventArgs e)
        {
            Performance = Convert.ToInt32(textBox_LevelPerformance.Text);
            progressBar_Performance.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Performance - 1));

        }
        private void textBox_LevelVigilance_TextChanged(object sender, EventArgs e)
        {
            Vigilance = Convert.ToInt32(textBox_LevelVigilance.Text);
            progressBar_Vigilance.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Vigilance - 1));

        }
        private void textBox_LevelTrade_TextChanged(object sender, EventArgs e)
        {
            Trade = Convert.ToInt32(textBox_LevelTrade.Text);
            progressBar_Trade.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Trade - 1));
        }

        private void textBox_Popularity_TextChanged(object sender, EventArgs e)
        {
            Popularity = Convert.ToInt32(textBox_Popularity.Text);
        }

        private void button_WorkAWeek_Click(object sender, EventArgs e)
        {
            Days = 6;
            button_Work.PerformClick();
        }

        //Только цифры
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
        public Form1()
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
                date.NextDay(); DateRenew();

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
            date.year = Convert.ToInt32(textBox_Year.Text);
        }

        private void comboBox_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            date.month = comboBox_Month.SelectedIndex +1;
        }

        private void comboBox_Day_SelectedIndexChanged(object sender, EventArgs e)
        {
            date.day = comboBox_Month.SelectedIndex+1;
        }

        private void button_WorkWeekend_Click(object sender, EventArgs e)
        {
            if(Weekends >0)
            {
                Weekends--;
                int buf = Days;
                Days = 1;
                WorkDayBaforeWeekend++;
                button_Work.PerformClick();
                Days = buf;

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
            }

        }

        private void button_MakeShow_Click(object sender, EventArgs e)
        {
            if (Weekends > 0 && progressBar_Damage.Value < 3)
            {
                Weekends--;
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

                //Experience
                try { progressBar_Performance.Value += 5 * Experience(Performance, PerformanceCheck); }
                catch
                {
                    if (progressBar_Performance.Value >= progressBar_Performance.Maximum)
                        if (d2 == 2) Performance++; progressBar_Performance.Value = 0;
                }


                if (resPers > resСheck)
                {
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
                        Money += Convert.ToInt32(CostSoldBarels / PartSoldBarels);
                        if (dicePer - diceСheck == 5) Money += 10;
                    }
                    //Experience
                    try { progressBar_Trade.Value += Experience(Trade, TradeСheck); }
                    catch { if (progressBar_Trade.Value >= progressBar_Trade.Maximum) if (d2 == 2) Trade++; progressBar_Trade.Value = 0; }
                }

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
            }
        }

        private void button_Marketing_Click(object sender, EventArgs e)
        {
            if (Weekends > 0 && Popularity <= citiPopular && progressBar_Damage.Value < 3)
            {
                Weekends--;
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
                if (resPers > resСheck) hit = true;

                //Experience
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
                    PerformanceCheck = 4 + d3; //Внимательность
                    resPers = dicePer + Performance;
                    resСheck = PerformanceCheck + diceСheck;
                }
                while (resPers == resСheck);
                if (resPers < resСheck) damage = true;

                //Experience
                try { progressBar_Performance.Value += 2 * Experience(Performance, PerformanceCheck); }
                catch
                {
                    if (progressBar_Performance.Value >= progressBar_Performance.Maximum)
                        if (d2 == 2) Performance++; progressBar_Performance.Value = 0;
                }

                if (hit && !damage) { Popularity++; }
                if (damage) progressBar_Damage.Value++;

                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity);
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
                if (resPers > resTheir) damage = true;

                //Experience
                try { progressBar_Vigilance.Value += 5 * Experience(Vigilance, TheirPerformance); }
                catch
                {
                    if (progressBar_Vigilance.Value >= progressBar_Vigilance.Maximum)
                        if (d2 == 2) Vigilance++; progressBar_Vigilance.Value = 0;
                }
            }

            if (hit && !damage) { Popularity--; }
        }

        private void button_LookAtBar_Click(object sender, EventArgs e)
        {
            if (Weekends > 0)
            {
                Weekends--;
                //Диверсанты
                if (citiPopular - Popularity < 5)
                    AntiMarketing(8 - citiPopular + Popularity,true,Vigilance);
            }
        }


    }


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
                + day.ToString() + " " + months[month+1];
        }
    }


}
