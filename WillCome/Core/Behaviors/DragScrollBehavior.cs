using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace WillCome.Core.Behaviors
{
    public static class DragScrollBehavior
    {
        #region Dependency Property: IsEnabled
        /// <summary>
        /// Прикрепленное свойство позволяет включать или отключать режим прокрутки перетаскиванием.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(DragScrollBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

        #endregion

        #region Private Fields

        private static bool _isScrollingHorizontally = false;
        private static bool _isScrollingVertically = false;

        private static CancellationTokenSource _horizontalScrollCancellationTokenSource;
        private static CancellationTokenSource _verticalScrollCancellationTokenSource;

        #endregion

        #region Event Handlers for IsEnabled
        /// <summary>
        /// Присоединяет или отсоединяет обработчики событий в зависимости от значения IsEnabled.
        /// </summary>
        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                if ((bool)e.NewValue)
                {
                    scrollViewer.PreviewDragOver += ScrollViewer_DragOver;
                    scrollViewer.PreviewDrop += ScrollViewer_Drop;
                    scrollViewer.DragLeave += ScrollViewer_DragLeave;
                }
                else
                {
                    scrollViewer.PreviewDragOver -= ScrollViewer_DragOver;
                    scrollViewer.PreviewDrop -= ScrollViewer_Drop;
                    scrollViewer.DragLeave -= ScrollViewer_DragLeave;
                }
            }
        }
        #endregion

        #region DragOver Logic
        /// <summary>
        /// Обрабатывает событие PreviewDragOver, чтобы включить прокрутку при перетаскивании вблизи краев.
        /// </summary>
        private static void ScrollViewer_DragOver(object sender, DragEventArgs e)
        {
            const double scrollMargin = 80;
            const double scrollSpeed = 10; 

            if (sender is ScrollViewer scrollViewer)
            {
                Point position = e.GetPosition(scrollViewer);

                double viewportWidth = scrollViewer.ViewportWidth;
                double viewportHeight = scrollViewer.ViewportHeight;

                double horizontalOffset = scrollViewer.HorizontalOffset;
                double maxHorizontalOffset = scrollViewer.ExtentWidth - viewportWidth;

                double verticalOffset = scrollViewer.VerticalOffset;
                double maxVerticalOffset = scrollViewer.ExtentHeight - viewportHeight;

                // Horizontal scrolling
                if (position.X > viewportWidth - scrollMargin && horizontalOffset < maxHorizontalOffset)
                {
                    if (!_isScrollingHorizontally)
                    {
                        _isScrollingHorizontally = true;
                        StartHorizontalScroll(scrollViewer, scrollSpeed);
                    }
                }
                else if (position.X < scrollMargin && horizontalOffset > 0)
                {
                    if (!_isScrollingHorizontally)
                    {
                        _isScrollingHorizontally = true;
                        StartHorizontalScroll(scrollViewer, -scrollSpeed);
                    }
                }
                else
                {
                    _isScrollingHorizontally = false;
                }

                // Vertical scrolling
                if (position.Y > viewportHeight - scrollMargin && verticalOffset < maxVerticalOffset)
                {
                    if (!_isScrollingVertically)
                    {
                        _isScrollingVertically = true;
                        StartVerticalScroll(scrollViewer, scrollSpeed);
                    }
                }
                else if (position.Y < scrollMargin && verticalOffset > 0)
                {
                    if (!_isScrollingVertically)
                    {
                        _isScrollingVertically = true;
                        StartVerticalScroll(scrollViewer, -scrollSpeed);
                    }
                }
                else
                {
                    _isScrollingVertically = false;
                }
            }
        }

        #endregion

        #region Scrolling Methods
        /// <summary>
        /// Запускает горизонтальную прокрутку с заданной скоростью.
        /// </summary>
        private static void StartHorizontalScroll(ScrollViewer scrollViewer, double speed)
        {
            CancelHorizontalScroll();

            _horizontalScrollCancellationTokenSource = new CancellationTokenSource();
            var token = _horizontalScrollCancellationTokenSource.Token;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    double horizontalOffset = scrollViewer.HorizontalOffset;
                    double maxHorizontalOffset = scrollViewer.ExtentWidth - scrollViewer.ViewportWidth;

                    scrollViewer.Dispatcher.Invoke(() =>
                    {
                        scrollViewer.ScrollToHorizontalOffset(
                            Math.Max(0, Math.Min(horizontalOffset + speed, maxHorizontalOffset))
                        );
                    });

                    await Task.Delay(16, token);
                }
            }, token);
        }

        /// <summary>
        /// Запускает вертикальную прокрутку с заданной скоростью.
        /// </summary>
        private static void StartVerticalScroll(ScrollViewer scrollViewer, double speed)
        {
            CancelVerticalScroll();

            _verticalScrollCancellationTokenSource = new CancellationTokenSource();
            var token = _verticalScrollCancellationTokenSource.Token;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    double verticalOffset = scrollViewer.VerticalOffset;
                    double maxVerticalOffset = scrollViewer.ExtentHeight - scrollViewer.ViewportHeight;

                    scrollViewer.Dispatcher.Invoke(() =>
                    {
                        scrollViewer.ScrollToVerticalOffset(
                            Math.Max(0, Math.Min(verticalOffset + speed, maxVerticalOffset))
                        );
                    });

                    await Task.Delay(16, token); // Smooth scrolling at ~60 FPS
                }
            }, token);
        }

        #endregion

        #region Stop Scrolling and Cancellation
        /// <summary>
        /// Останавливает как горизонтальную, так и вертикальную прокрутку.
        /// </summary>
        private static void StopScrolling()
        {
            _isScrollingHorizontally = false;
            _isScrollingVertically = false;

            CancelHorizontalScroll();
            CancelVerticalScroll();
        }

        /// <summary>
        /// Отменяет задачу горизонтальной прокрутки.
        /// </summary>
        private static void CancelHorizontalScroll()
        {
            _horizontalScrollCancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Отменяет задачу вертикальной прокрутки.
        /// </summary>
        private static void CancelVerticalScroll()
        {
            _verticalScrollCancellationTokenSource?.Cancel();
        }

        #endregion

        #region Drop and DragLeave Handlers

        /// <summary>
        /// Обрабатывает событие удаления предварительного просмотра, чтобы остановить прокрутку.
        /// </summary>
        private static void ScrollViewer_Drop(object sender, DragEventArgs e)
        {
            StopScrolling();
        }

        /// <summary>
        /// Обрабатывает событие DragLeave, чтобы остановить прокрутку, когда мышь покидает ScrollViewer.
        /// </summary>
        private static void ScrollViewer_DragLeave(object sender, DragEventArgs e)
        {
            StopScrolling();
        }

        #endregion
    }
}
