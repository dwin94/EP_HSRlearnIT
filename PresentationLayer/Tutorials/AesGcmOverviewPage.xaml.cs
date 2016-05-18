﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Page to briefly inform about AES-GCM
    /// </summary>
    public partial class AesGcmOverviewPage
    {
        #region Constructors
        /// <summary>
        /// Method to initialize the XAML and the Content
        /// </summary>
        public AesGcmOverviewPage()
        {
            InitializeComponent();
            InitCanvas(OverviewCanvas);
        }
        #endregion

        #region Private Members
        private readonly ToolTip _toolTip = new ToolTip();

        private readonly Dictionary<string, Path> _backPaths = new Dictionary<string, Path>();
        private readonly Path[] _areaPaths = new Path[NumOfAreas];
        private Path _lastEnteredAreaPath;

        private const int NumOfAreas = 6;
        private const int NumOfStepPaths = 24;

        #endregion Private Members

        #region Private Methods
        private void InitCanvas(Canvas canvas)
        {
            LoadAreaPaths(canvas);
            LoadStepPaths(canvas);
            LoadBackground(canvas);
        }

        private void LoadAreaPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfAreas; i++)
            {
                Path ressourcePath = Application.Current.FindResource("AreaPath" + i) as Path;
                if (ressourcePath == null) continue;

                //Create a copy of the Ressource AreaPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path areaPath = Clone(ressourcePath) as Path;
                if (areaPath == null)
                {
                    ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: LoadAreaPaths");
                    return;
                }

                areaPath.SetValue(Panel.ZIndexProperty, 2);
                areaPath.MouseEnter += AreaPathOnMouseEnter;
                areaPath.MouseLeave += AreaPathOnMouseLeave;

                canvas.Children.Add(areaPath);
                _areaPaths[i - 1] = areaPath;
            }
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null || !ressourcePath.Name.Contains("_overview")) continue;

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = Clone(ressourcePath) as Path;
                if (stepPath == null)
                {
                    ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: LoadStepPaths");
                    return;
                }

                stepPath.SetValue(Panel.ZIndexProperty, 3);
                stepPath.MouseEnter += StepPathOnMouseEnter;
                stepPath.MouseLeave += StepPathOnMouseLeave;
                stepPath.MouseDown += StepPathOnClick;

                canvas.Children.Add(stepPath);
            }
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("BackgroundImage") as Image;
            if (background == null) return;

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas) background.Parent).Children.Remove(background);
            }

            background.SetValue(Panel.ZIndexProperty, 1);
            
            canvas.Children.Add(background);
        }

        private Path AddBackPath(Path frontPath)
        {
            Path backPath = Clone(frontPath) as Path;
            if (backPath == null)
            {
                ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: AddBackPath");
                return null;
            }

            backPath.SetValue(Panel.ZIndexProperty, 0);
            _backPaths.Add(frontPath.Name, backPath);

            //Add backPath to the OverviewCanvas
            (frontPath.Parent as Canvas)?.Children.Add(backPath);

            return backPath;
        }

        private void StepPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            Path frontPath = sender as Path;
            if (frontPath == null) return;
            Path backPath;

            //When MouseOver the second time, the backPath already exists
            if (!_backPaths.ContainsKey(frontPath.Name))
            {
                backPath = AddBackPath(frontPath);
                if(backPath == null) { return; }
            }
            else
            {
                backPath = _backPaths[frontPath.Name];
            }

            backPath.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;

            FindArea(frontPath, e);
        }

        private void FindArea(Path frontPath, MouseEventArgs e)
        {
            Geometry stepInfo = frontPath.Data;

            foreach (Path area in _areaPaths)
            {
                Geometry areaInfo = area.Data;
                IntersectionDetail detail = stepInfo.FillContainsWithDetail(areaInfo);

                if (detail == IntersectionDetail.FullyContains
                    || detail == IntersectionDetail.FullyInside
                    || detail == IntersectionDetail.Intersects)
                {
                    AreaPathOnMouseEnter(area, e);
                    _lastEnteredAreaPath = area;
                }
            }
        }

        private void StepPathOnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

            Path frontPath = sender as Path;
            if (frontPath == null) return;

            Path backPath = _backPaths[frontPath.Name];
            backPath.Fill = Application.Current.FindResource("NoBackAreaBrush") as SolidColorBrush;

            AreaPathOnMouseLeave(_lastEnteredAreaPath, e);
        }

        private void AreaPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            string text = Application.Current.FindResource(path?.Name + "Text") as string;
            ShowExplanation(text);
        }

        private void AreaPathOnMouseLeave(object sender, MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void StepPathOnClick(object sender, MouseEventArgs e)
        {
            string pathName = (sender as Path)?.Name;

            //Example Path-Name: Step11_overview
            if (pathName != null)
            {
                int last = pathName.IndexOf("_", StringComparison.Ordinal);
                string stepNumber = pathName.Substring(4, last - 4);
                int step = int.Parse(stepNumber);
                
                NavigationService?.Navigate(new StepByStepPage(step));
            }
            NavigationService?.Navigate(new StepByStepPage());
        }

        private void ShowExplanation(string text)
        {
            OverviewTextBlock.Text = text + "\n<<Klick auf das Feld für weitere Infos>>";
            if (!OverviewTextBlock.ClipToBounds)
            {
                TextScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
        }

        #endregion Private Methods
    }
}
