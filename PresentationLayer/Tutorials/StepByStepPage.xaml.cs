﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Games;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    public partial class StepByStepPage
    {
        #region Private Members
        private int _step;
        private int _title;
        private const int StepMin = 0;
        private const int StepMax = 18;

        private const int NumOfStepPaths = 24;
        private readonly Dictionary<int, Path> _stepPaths = new Dictionary<int, Path>();
        private int _highlightedPath;

        #endregion

        #region Constructors
        public StepByStepPage(int stepNumber)
        {
            InitializeComponent();

            if (StepMin <= stepNumber && stepNumber <= StepMax)
            {
                _title = stepNumber;
                Progress.SaveProgress("StepByStepPage_CurrentStep", stepNumber);
            }

            NavigationService?.Navigate(new StepByStepPage());
        }

        public StepByStepPage()
        {
            InitializeComponent();
            LoadBackground(StepByStepCanvas);
            LoadStepPaths(StepByStepCanvas);

            var progressCurrentStep = Progress.GetProgress("StepByStepPage_CurrentStep");
            if (progressCurrentStep != null)
            {
                _step = Convert.ToInt32(progressCurrentStep);
            }

            var progressTitleStep = Progress.GetProgress("StepByStepPage_Title");
            if (progressTitleStep != null)
            {
                _title = Convert.ToInt32(progressTitleStep);
            }

            ReplaceContent(_step);

            var progressActivateGame = Progress.GetProgress("StepByStepPage_Game");
            if (progressActivateGame != null)
            {
                ActivateGameButton();
            }
        }

        #endregion

        #region Private Methods
        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(--_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
            Progress.SaveProgress("StepByStepPage_Title", _title);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(++_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
            Progress.SaveProgress("StepByStepPage_Title", _title);
        }

        private void OnStartDragDropButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDropPage());
        }

        private void ActivateGameButton()
        {
            StartDragDrop.Visibility = Visibility.Visible;
            Progress.SaveProgress("StepByStepPage_GameButton", StartDragDrop.IsVisible);
        }

        private void ReplaceContent(int stepNumber)
        {
            switch (stepNumber)
            {
                case StepMin:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case StepMax:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    ActivateGameButton();
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            if (!StepDescriptionBox.ClipToBounds)
            {
                TextScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
            StepTitle.Text = WriteTitle(stepNumber);

            if (_stepPaths.ContainsKey(stepNumber))
            {
                StepViewBox.Visibility = Visibility.Visible;
                StepImage.Visibility = Visibility.Hidden;

                Path toFillPath;
                Path toEmptyPath;
                bool available = _stepPaths.TryGetValue(stepNumber, out toFillPath);

                int toEmpty = _highlightedPath - stepNumber;
                bool avail = _stepPaths.TryGetValue(stepNumber + toEmpty, out toEmptyPath);

                if (available)
                {
                    FillPath(toFillPath);
                }
                if (avail)
                {
                    TransparentPath(toEmptyPath);
                }

                _highlightedPath = stepNumber;
            }
            else
            {
                StepViewBox.Visibility = Visibility.Hidden;
                StepImage.Visibility = Visibility.Visible;
                StepImage.Source = new BitmapImage(new Uri(@"/Images/Step" + stepNumber + ".png", UriKind.RelativeOrAbsolute));
                _highlightedPath = stepNumber;
            }
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null || !ressourcePath.Name.Contains("_stepByStep")) continue;

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = CopyPath(ressourcePath);
                _stepPaths.Add(i, stepPath);

                stepPath.SetValue(Panel.ZIndexProperty, 0);
                canvas.Children.Add(stepPath);
            }
        }

        private void FillPath(Path path)
        {
            path.Fill = Application.Current.FindResource("TileSolidColor") as SolidColorBrush;
        }

        private void TransparentPath(Path path)
        {
            path.Fill = null;
        }

        private Path CopyPath(Path originalPath)
        {
            Path copy = new Path
            {
                Data = originalPath.Data.Clone(),
                Name = originalPath.Name,
                Style = originalPath.Style
            };
            return copy;
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("StepByStepBackground") as Image;
            if (background == null) return;

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas) background.Parent).Children.Remove(background);
            }

            background.SetValue(Panel.ZIndexProperty, 1);

            canvas.Children.Add(background);
        }

        private string WriteTitle(int stepNumber)
        {
            string title = GetTitle(stepNumber);
            if (title == null)
            {
                string oldTitle = GetTitle(_title);
                if (oldTitle == null || stepNumber != _title)
                {
                    for (int i = stepNumber; i >= StepMin; i--)
                    {
                        title = GetTitle(i);
                        if (title != null)
                        {
                            return title;
                        }
                    }
                }
                return oldTitle;
            }
            _title = stepNumber;
            return title;
        }

        private string GetTitle(int titleNumber)
        {
            string titleName = "Step" + titleNumber + "Title";
            if (Application.Current.Resources.Contains(titleName))
            {
                return Application.Current.FindResource(titleName) as string;
            }
            return null;
        }

        #endregion
    }
}