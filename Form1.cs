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
        //Навыки
        int Trade
        {
            get {return Convert.ToInt32(textBox_LevelTrade.Text);}
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

        const double PopularK = 1.6;
        int WorkDayBaforeWeekend = 6;

        Random random = new Random();
        public int d2 { get { return random.Next(1, 2); } }
        public int d3 { get { return random.Next(1, 3); } }
        public int d6 { get { return random.Next(1, 6); } }
        //        public int d6() { return random.Next(1, 6); }
        public int Dices(int num)
        {
            int result = 0;
            for (int i = 0; i < num; i++)
                result += d6;
            return result;
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
                int dicePer = 0, diceСheck, resPers = 0, resСheck = 0;
                //Торговля
                for (int j=0; j<NumBuyes; j++)
                {
                    do
                    {
                        dicePer = d6;
                        diceСheck = d3;
                        resPers = dicePer + Trade;
                        resСheck = diceСheck + 1 + d3;
                    }
                    while (resPers == resСheck);

                    //Продажа кружки и чаевые
                    if (resPers > resСheck)
                    {
                        SoldMugs++;
                        if (dicePer - diceСheck == 5) Tips++;
                        if (resPers - resСheck == 8) Tips++;
                    }
                }
                // Приход компании
                if (d6 > 4)
                {
                    Party++;
                    do
                    {
                        dicePer = d6;
                        diceСheck = d3;
                        resPers = dicePer + Trade;
                        resСheck = diceСheck + 4 + d3;
                    }
                    while (resPers == resСheck);

                    //Продажа бочёнка и чаевые
                    if (resPers > resСheck)
                    {
                        soldBarels++;
                        if (dicePer - diceСheck == 5) Tips+=10;
                    }
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

                //Конец рабочего дня
            }
            //Денюжка
            Money += Salary + Tips;
        }

        private void textBox_LevelPerformance_TextChanged(object sender, EventArgs e)
        {
            Performance = Convert.ToInt32(textBox_LevelPerformance.Text);
            progressBar_Performance.Maximum = Convert.ToInt32(7 * Math.Pow(1.5, Performance - 1));

        }

        private void textBox_LevelVigilance_TextChanged(object sender, EventArgs e)
        {
            Vigilance = Convert.ToInt32(textBox_LevelVigilance.Text);
            progressBar_Vigilance.Maximum = Convert.ToInt32(10 * Math.Pow(1.5, Vigilance - 1));

        }

        private void textBox_Popularity_TextChanged(object sender, EventArgs e)
        {
            Popularity = Convert.ToInt32(textBox_Popularity.Text);            
        }

        private void textBox_LevelTrade_TextChanged(object sender, EventArgs e)
        {
            Trade = Convert.ToInt32(textBox_LevelTrade.Text);
            progressBar_Trade.Maximum = Convert.ToInt32(300 * Math.Pow(1.5, Trade - 1));
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
        }

        private void button_Prize_Click(object sender, EventArgs e)
        {
            Money += Prize;
        }
    }
}
