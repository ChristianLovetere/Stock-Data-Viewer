namespace COP_4365_C__Demo
{
    partial class Form_StockViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Button_pickStock = new System.Windows.Forms.Button();
            this.openFileDialog_TickerChooser = new System.Windows.Forms.OpenFileDialog();
            this.DateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.DateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.label_StartDate = new System.Windows.Forms.Label();
            this.label_EndDate = new System.Windows.Forms.Label();
            this.chart_Candlesticks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Button_Update = new System.Windows.Forms.Button();
            this.comboBox_CandlestickPatterns = new System.Windows.Forms.ComboBox();
            this.label_Patterns = new System.Windows.Forms.Label();
            this.BindingSource_Candlesticks = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Candlesticks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource_Candlesticks)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_pickStock
            // 
            this.Button_pickStock.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Button_pickStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Button_pickStock.Location = new System.Drawing.Point(12, 582);
            this.Button_pickStock.Name = "Button_pickStock";
            this.Button_pickStock.Size = new System.Drawing.Size(130, 72);
            this.Button_pickStock.TabIndex = 3;
            this.Button_pickStock.Text = "Pick a Stock";
            this.Button_pickStock.UseVisualStyleBackColor = false;
            this.Button_pickStock.Click += new System.EventHandler(this.button_pickStock_Click);
            // 
            // openFileDialog_TickerChooser
            // 
            this.openFileDialog_TickerChooser.FileName = "openFileDialog_TickerChooser";
            this.openFileDialog_TickerChooser.Filter = "Daily CSVs|*-Day.csv|Weekly CSVs|*-Week.csv|Monthly CSVs|*-Month.csv|All files|*." +
    "*";
            this.openFileDialog_TickerChooser.InitialDirectory = "This PC";
            this.openFileDialog_TickerChooser.Multiselect = true;
            this.openFileDialog_TickerChooser.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_TickerChooser_FileOk);
            // 
            // DateTimePicker_StartDate
            // 
            this.DateTimePicker_StartDate.Location = new System.Drawing.Point(315, 608);
            this.DateTimePicker_StartDate.Name = "DateTimePicker_StartDate";
            this.DateTimePicker_StartDate.Size = new System.Drawing.Size(200, 22);
            this.DateTimePicker_StartDate.TabIndex = 5;
            this.DateTimePicker_StartDate.Value = new System.DateTime(2022, 1, 1, 0, 0, 0, 0);
            // 
            // DateTimePicker_EndDate
            // 
            this.DateTimePicker_EndDate.Location = new System.Drawing.Point(885, 608);
            this.DateTimePicker_EndDate.Name = "DateTimePicker_EndDate";
            this.DateTimePicker_EndDate.Size = new System.Drawing.Size(200, 22);
            this.DateTimePicker_EndDate.TabIndex = 6;
            // 
            // label_StartDate
            // 
            this.label_StartDate.AutoSize = true;
            this.label_StartDate.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label_StartDate.Location = new System.Drawing.Point(231, 613);
            this.label_StartDate.Name = "label_StartDate";
            this.label_StartDate.Size = new System.Drawing.Size(69, 16);
            this.label_StartDate.TabIndex = 7;
            this.label_StartDate.Text = "Start Date:";
            // 
            // label_EndDate
            // 
            this.label_EndDate.AutoSize = true;
            this.label_EndDate.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label_EndDate.Location = new System.Drawing.Point(804, 613);
            this.label_EndDate.Name = "label_EndDate";
            this.label_EndDate.Size = new System.Drawing.Size(66, 16);
            this.label_EndDate.TabIndex = 8;
            this.label_EndDate.Text = "End Date:";
            // 
            // chart_Candlesticks
            // 
            this.chart_Candlesticks.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chart_Candlesticks.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea_OHLC";
            chartArea2.AlignWithChartArea = "ChartArea_OHLC";
            chartArea2.Name = "ChartArea_Volume";
            this.chart_Candlesticks.ChartAreas.Add(chartArea1);
            this.chart_Candlesticks.ChartAreas.Add(chartArea2);
            this.chart_Candlesticks.Location = new System.Drawing.Point(12, 12);
            this.chart_Candlesticks.Name = "chart_Candlesticks";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Color = System.Drawing.Color.Navy;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=LimeGreen";
            series1.IsXValueIndexed = true;
            series1.Name = "Series_OHLC";
            series1.ShadowColor = System.Drawing.Color.Black;
            series1.XValueMember = "date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series1.YValueMembers = "high, low, open, close";
            series1.YValuesPerPoint = 4;
            series2.ChartArea = "ChartArea_Volume";
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series2.IsXValueIndexed = true;
            series2.Name = "Series_Volume";
            series2.XValueMember = "date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series2.YValueMembers = "volume";
            this.chart_Candlesticks.Series.Add(series1);
            this.chart_Candlesticks.Series.Add(series2);
            this.chart_Candlesticks.Size = new System.Drawing.Size(1291, 569);
            this.chart_Candlesticks.TabIndex = 9;
            this.chart_Candlesticks.Text = "chart_Candlesticks";
            // 
            // Button_Update
            // 
            this.Button_Update.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Button_Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Button_Update.Location = new System.Drawing.Point(1173, 581);
            this.Button_Update.Name = "Button_Update";
            this.Button_Update.Size = new System.Drawing.Size(130, 73);
            this.Button_Update.TabIndex = 10;
            this.Button_Update.Text = "Update";
            this.Button_Update.UseVisualStyleBackColor = false;
            this.Button_Update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // comboBox_CandlestickPatterns
            // 
            this.comboBox_CandlestickPatterns.FormattingEnabled = true;
            this.comboBox_CandlestickPatterns.Location = new System.Drawing.Point(654, 608);
            this.comboBox_CandlestickPatterns.Name = "comboBox_CandlestickPatterns";
            this.comboBox_CandlestickPatterns.Size = new System.Drawing.Size(121, 24);
            this.comboBox_CandlestickPatterns.TabIndex = 11;
            // 
            // label_Patterns
            // 
            this.label_Patterns.AutoSize = true;
            this.label_Patterns.Location = new System.Drawing.Point(547, 613);
            this.label_Patterns.Name = "label_Patterns";
            this.label_Patterns.Size = new System.Drawing.Size(92, 16);
            this.label_Patterns.TabIndex = 12;
            this.label_Patterns.Text = "Pick a Pattern:";
            // 
            // BindingSource_Candlesticks
            // 
            this.BindingSource_Candlesticks.DataSource = typeof(COP_4365_C__Demo.smartCandlestick);
            // 
            // Form_StockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1315, 666);
            this.Controls.Add(this.label_Patterns);
            this.Controls.Add(this.comboBox_CandlestickPatterns);
            this.Controls.Add(this.Button_Update);
            this.Controls.Add(this.chart_Candlesticks);
            this.Controls.Add(this.label_EndDate);
            this.Controls.Add(this.label_StartDate);
            this.Controls.Add(this.DateTimePicker_EndDate);
            this.Controls.Add(this.DateTimePicker_StartDate);
            this.Controls.Add(this.Button_pickStock);
            this.Name = "Form_StockViewer";
            this.Text = "Stock Viewer";
            this.Load += new System.EventHandler(this.Form_StockViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Candlesticks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSource_Candlesticks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_pickStock;
        private System.Windows.Forms.OpenFileDialog openFileDialog_TickerChooser;
        private System.Windows.Forms.BindingSource BindingSource_Candlesticks;
        private System.Windows.Forms.DateTimePicker DateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker DateTimePicker_EndDate;
        private System.Windows.Forms.Label label_StartDate;
        private System.Windows.Forms.Label label_EndDate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Candlesticks;
        private System.Windows.Forms.Button Button_Update;
        private System.Windows.Forms.ComboBox comboBox_CandlestickPatterns;
        private System.Windows.Forms.Label label_Patterns;
    }
}

