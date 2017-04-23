using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WPFPageTransitions
{
    public partial class PageTransition : UserControl
    {
        public Stack<UserControl> Pages { get; } = new Stack<UserControl>();

        public UserControl CurrentPage { get; set; }

        public static readonly DependencyProperty TransitionTypeProperty = DependencyProperty.Register("TransitionType",
            typeof(PageTransitionType),
            typeof(PageTransition), new PropertyMetadata(PageTransitionType.Slide));

        public PageTransitionType TransitionType
        {
            get => (PageTransitionType)GetValue(TransitionTypeProperty);
            set => SetValue(TransitionTypeProperty, value);
        }

        public PageTransition() => InitializeComponent();

        public void ShowPage(UserControl newPage)
        {
            Pages.Push(newPage);
            Task.Factory.StartNew(ShowNewPage);
        }

        public void ShowPage(UserControl newPage, PageTransitionType ptType)
        {
            TransitionType = ptType;
            ShowPage(newPage);
        }

        private void ShowNewPage()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                if (contentPresenter.Content != null)
                {
                    UserControl oldPage = contentPresenter.Content as UserControl;

                    if (oldPage == null) return;

                    oldPage.Loaded -= newPage_Loaded;
                    UnloadPage(oldPage);
                }
                else ShowNextPage();
            }));
        }

        private void ShowNextPage()
        {
            UserControl newPage = Pages.Pop();
            newPage.Loaded += newPage_Loaded;
            contentPresenter.Content = newPage;
        }

        private void UnloadPage(UserControl page)
        {
            Storyboard hidePage = (Resources[$"{TransitionType}Out"] as Storyboard)?.Clone();

            if (hidePage == null) return;

            hidePage.Completed += hidePage_Completed;
            hidePage.Begin(contentPresenter);
        }

        private void newPage_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard showNewPage = Resources[$"{TransitionType}In"] as Storyboard;

            showNewPage?.Begin(contentPresenter);
            CurrentPage = sender as UserControl;
        }

        private void hidePage_Completed(object sender, EventArgs e)
        {
            contentPresenter.Content = null;
            ShowNextPage();
        }
    }
}
