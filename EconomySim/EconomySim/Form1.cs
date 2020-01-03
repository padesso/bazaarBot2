﻿using EconomySim.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EconomySim
{
    public partial class Form1 : Form
    {
        private Economy economy;
	    private Market market;
        private Timer autoStepTimer;
        private int iterationCount = 0;

        public ChartValues<PriceModel> FoodPriceValues { get; set; }
        public ChartValues<PriceModel> WoodPriceValues { get; set; }
        public ChartValues<PriceModel> OrePriceValues { get; set; }
        public ChartValues<PriceModel> MetalPriceValues { get; set; }
        public ChartValues<PriceModel> ToolsPriceValues { get; set; }
        public ChartValues<PriceModel> WorkPriceValues { get; set; }

        public Form1()
        {
            InitializeComponent();

            SetupChart();
            SetupTimer();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            economy = new DoranAndParberryEconomy();

            market = economy.GetMarket("default");

            dataGridView1.DataSource = market.agents;
            //dataGridView2.DataSource = market._book.dbook;
        }

        private void SetupChart()
        {
            FoodPriceValues = new ChartValues<PriceModel>();
            WoodPriceValues = new ChartValues<PriceModel>();
            OrePriceValues  = new ChartValues<PriceModel>();
            MetalPriceValues = new ChartValues<PriceModel>();
            ToolsPriceValues = new ChartValues<PriceModel>();
            WorkPriceValues = new ChartValues<PriceModel>();

            var mapper = Mappers.Xy<PriceModel>()
                .X(model => model.Iteration)        //use accumulated iteration count X
                .Y(model => model.Price);           //use the Price property as Y

            //lets save the mapper globally.
            Charting.For<PriceModel>(mapper);

            lineChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Food Price",
                    Values = FoodPriceValues
                },
                new LineSeries
                {
                    Title = "Wood Price",
                    Values = WoodPriceValues
                },
                new LineSeries
                {
                    Title = "Ore Price",
                    Values = OrePriceValues
                },
                new LineSeries
                {
                    Title = "Metal Price",
                    Values = MetalPriceValues
                },
                new LineSeries
                {
                    Title = "Tools Price",
                    Values = ToolsPriceValues
                },
                new LineSeries
                {
                    Title = "Work Price",
                    Values = WorkPriceValues
                },
            };

            //lineChart.AxisX.Add(new Axis
            //{
            //    Title = "Iterations",
            //    Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            //});

            //lineChart.AxisY.Add(new Axis
            //{
            //    Title = "Price",
            //    LabelFormatter = value => value.ToString("C")
            //});
            

            lineChart.LegendLocation = LegendLocation.Right;
        }

        private void SetupTimer()
        {
            autoStepTimer = new Timer();
            autoStepTimer.Tick += AutoStepTimer_Tick;
            autoStepTimer.Interval = 1000;
            autoStepTimer.Enabled = true;
            autoStepTimer.Start();
        }

        private void AutoStepTimer_Tick(object sender, EventArgs e)
        {
            if (market != null && autoRunCbx.Checked)
                run(100);
        }

        private void run(int rounds)
        {
            for (int round = 1; round <= rounds; round++)
            {
                iterationCount++;
                market.Simulate(1);
                var res = market.GetMarketReport(1);
                textBox1.Clear();
                textBox1.Text = res.strListGood.Replace("\n", "\t") + Environment.NewLine;
                textBox1.Text += res.strListGoodPrices.Replace("\n", "\t") + Environment.NewLine;
                textBox1.Text += res.strListGoodTrades.Replace("\n", "\t") + Environment.NewLine;
                textBox1.Text += res.strListGoodBids.Replace("\n", "\t") + Environment.NewLine;
                textBox1.Text += res.strListGoodAsks.Replace("\n", "\t") + Environment.NewLine;

                //populate the food graph
                FoodPriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[2])
                });

                WoodPriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[3])
                });

                OrePriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[4])
                });

                MetalPriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[5])
                });

                ToolsPriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[6])
                });

                WorkPriceValues.Add(new PriceModel
                {
                    Iteration = iterationCount,
                    Price = Double.Parse(res.strListGoodPrices.Split('\n')[7])
                });
            }

            //data grid only shows latest, so only update after all runs
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            run(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            run(20);
        }
    }
}
