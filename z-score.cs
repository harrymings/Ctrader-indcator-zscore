using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ZScoreIndicator : Indicator
    {
        [Parameter("Period", DefaultValue = 20, MinValue = 1)]
        public int Period { get; set; }

        [Parameter("Data Source")]
        public DataSeries Source { get; set; }

        [Output("Z-Score", PlotType = PlotType.Line, Color = Colors.DodgerBlue, Thickness = 2)]
        public IndicatorDataSeries ZScore { get; set; }

        [Output("Level +2", PlotType = PlotType.Line, Color = Colors.Red, Thickness = 2)]
        public IndicatorDataSeries LevelPlus2 { get; set; }

        [Output("Level +1", PlotType = PlotType.Line, Color = Colors.Orange, Thickness = 1)]
        public IndicatorDataSeries LevelPlus1 { get; set; }

        [Output("Level 0", PlotType = PlotType.Line, Color = Colors.Gray, Thickness = 1)]
        public IndicatorDataSeries Level0 { get; set; }

        [Output("Level -1", PlotType = PlotType.Line, Color = Colors.GreenYellow, Thickness = 1)]
        public IndicatorDataSeries LevelMinus1 { get; set; }

        [Output("Level -2", PlotType = PlotType.Line, Color = Colors.Green, Thickness = 2)]
        public IndicatorDataSeries LevelMinus2 { get; set; }

        private SimpleMovingAverage _sma;
        private StandardDeviation _stdDev;

        protected override void Initialize()
        {
            _sma    = Indicators.SimpleMovingAverage(Source, Period);
            _stdDev = Indicators.StandardDeviation(Source, Period, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            double avg   = _sma.Result[index];
            double sd    = _stdDev.Result[index];
            double price = Source[index];

            // Compute Z-Score
            double z = sd > 0 ? (price - avg) / sd : 0;
            ZScore[index] = z;

            // Plot fixed levels
            LevelPlus2[index]  =  2;
            LevelPlus1[index]  =  1;
            Level0[index]      =  0;
            LevelMinus1[index] = -1;
            LevelMinus2[index] = -2;
        }
    }
}
