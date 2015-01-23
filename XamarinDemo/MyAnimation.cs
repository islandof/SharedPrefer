using Android.Views;
using Android.Views.Animations;

namespace XamarinDemo
{
    internal class MyAnimation : Animation
    {
        private readonly int mGrowBy;
        private readonly int mOriginalHeight;
        private readonly View mView;
        private int mTargetHeight;

        public MyAnimation(View view, int targetHeight)
        {
            mView = view;
            mOriginalHeight = view.Height;
            mTargetHeight = targetHeight;
            mGrowBy = mTargetHeight - mOriginalHeight;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            mView.LayoutParameters.Height = (int) (mOriginalHeight + (mGrowBy*interpolatedTime));
            mView.RequestLayout();
        }

        public override bool WillChangeBounds()
        {
            return true;
        }
    }
}