using Microsoft.Maui.Controls;

namespace gesturerecognizer
{
    public partial class MainPage : ContentPage
    {
        double startScale, currentScale;
        double startX, startY;

        public MainPage()
        {
            InitializeComponent();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                bot.TranslationX = startX + e.TotalX;
                bot.TranslationY = startY + e.TotalY;
            }
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = bot.Scale;
                startX = bot.TranslationX;
                startY = bot.TranslationY;
            }
            else if (e.Status == GestureStatus.Running)
            {
                currentScale = Math.Max(1, startScale * e.Scale);
                var renderedX = bot.X + startX;
                var deltaX = renderedX / Width;
                var deltaWidth = Width / (bot.Width * startScale);
                var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
                var targetX = startX - (originX * bot.Width) * (currentScale - startScale);

                var renderedY = bot.Y + startY;
                var deltaY = renderedY / Height;
                var deltaHeight = Height / (bot.Height * startScale);
                var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
                var targetY = startY - (originY * bot.Height) * (currentScale - startScale);

                bot.TranslationX = Clamp(targetX, -bot.Width * (currentScale - 1), 0);
                bot.TranslationY = Clamp(targetY, -bot.Height * (currentScale - 1), 0);
                bot.Scale = currentScale;
            }
            else if (e.Status == GestureStatus.Completed)
            {
                startX = bot.TranslationX;
                startY = bot.TranslationY;
                startScale = bot.Scale;
            }
        }

        private double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
