using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Imagin.Controls.Common
{
    public class PageBase : Page
    {
        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        public event EventHandler<NavigatingCancelEventArgs> NavigatingFrom;

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        public PageBase() : base()
        {
            var Transitions = new TransitionCollection();

            var Theme = new NavigationThemeTransition();
            Theme.DefaultNavigationTransitionInfo = new ContinuumNavigationTransitionInfo();

            Transitions.Add(Theme);

            this.Transitions = Transitions;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            NavigatingFrom?.Invoke(this, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigatedTo?.Invoke(this, e);
        }
    }
}
