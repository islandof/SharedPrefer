using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XamarinDemo
{
    public sealed class SlidingTabStrip : LinearLayout
    {
        //Copy and paste from here................................................................
        private const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
        private const byte DEFAULT_BOTTOM_BORDER_COLOR_ALPHA = 0X26;
        private const int SELECTED_INDICATOR_THICKNESS_DIPS = 8;

        private const int DEFAULT_DIVIDER_THICKNESS_DIPS = 1;
        private const float DEFAULT_DIVIDER_HEIGHT = 0.5f;
        private readonly int[] DIVIDER_COLORS = {0xC5C5C5};
        private readonly int[] INDICATOR_COLORS = {0x19A319, 0x0000FC};

        //Bottom border
        private readonly Paint mBottomBorderPaint;
        private readonly int mBottomBorderThickness;
        private readonly float mDividerHeight;
        private readonly Paint mDividerPaint;
        private readonly Paint mSelectedIndicatorPaint;
        private readonly int mSelectedIndicatorThickness;

        //Selected position and offset

        //Tab colorizer
        private SlidingTabScrollView.TabColorizer mCustomTabColorizer;
        private int mDefaultBottomBorderColor;
        private SimpleTabColorizer mDefaultTabColorizer;
        private int mSelectedPosition;
        private float mSelectionOffset;
        //Stop copy and paste here........................................................................

        //Constructors
        public SlidingTabStrip(Context context) : this(context, null)
        {
        }

        public SlidingTabStrip(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);

            float density = Resources.DisplayMetrics.Density;

            var outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outValue, true);
            int themeForeGround = outValue.Data;
            mDefaultBottomBorderColor = SetColorAlpha(themeForeGround, DEFAULT_BOTTOM_BORDER_COLOR_ALPHA);

            mDefaultTabColorizer = new SimpleTabColorizer();
            mDefaultTabColorizer.IndicatorColors = INDICATOR_COLORS;
            mDefaultTabColorizer.DividerColors = DIVIDER_COLORS;

            mBottomBorderThickness = (int) (DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS*density);
            mBottomBorderPaint = new Paint();
            mBottomBorderPaint.Color = GetColorFromInteger(0xC5C5C5); //Gray

            mSelectedIndicatorThickness = (int) (SELECTED_INDICATOR_THICKNESS_DIPS*density);
            mSelectedIndicatorPaint = new Paint();

            mDividerHeight = DEFAULT_DIVIDER_HEIGHT;
            mDividerPaint = new Paint();
            mDividerPaint.StrokeWidth = (int) (DEFAULT_DIVIDER_THICKNESS_DIPS*density);
        }

        public SlidingTabScrollView.TabColorizer CustomTabColorizer
        {
            set
            {
                mCustomTabColorizer = value;
                Invalidate();
            }
        }

        public int[] SelectedIndicatorColors
        {
            set
            {
                mCustomTabColorizer = null;
                mDefaultTabColorizer.IndicatorColors = value;
                Invalidate();
            }
        }

        public int[] DividerColors
        {
            set
            {
                mDefaultTabColorizer = null;
                mDefaultTabColorizer.DividerColors = value;
                Invalidate();
            }
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private int SetColorAlpha(int color, byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color),
                Color.GetBlueComponent(color));
        }

        public void OnViewPagerPageChanged(int position, float positionOffset)
        {
            mSelectedPosition = position;
            mSelectionOffset = positionOffset;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            int height = Height;
            int tabCount = ChildCount;
            var dividerHeightPx = (int) (Math.Min(Math.Max(0f, mDividerHeight), 1f)*height);
            SlidingTabScrollView.TabColorizer tabColorizer = mCustomTabColorizer != null
                ? mCustomTabColorizer
                : mDefaultTabColorizer;

            //Thick colored underline below the current selection
            if (tabCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectedPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabColorizer.GetIndicatorColor(mSelectedPosition);

                if (mSelectionOffset > 0f && mSelectedPosition < (tabCount - 1))
                {
                    int nextColor = tabColorizer.GetIndicatorColor(mSelectedPosition + 1);
                    if (color != nextColor)
                    {
                        color = blendColor(nextColor, color, mSelectionOffset);
                    }

                    View nextTitle = GetChildAt(mSelectedPosition + 1);
                    left = (int) (mSelectionOffset*nextTitle.Left + (1.0f - mSelectionOffset)*left);
                    right = (int) (mSelectionOffset*nextTitle.Right + (1.0f - mSelectionOffset)*right);
                }

                mSelectedIndicatorPaint.Color = GetColorFromInteger(color);

                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);

                //Creat vertical dividers between tabs
                int separatorTop = (height - dividerHeightPx)/2;
                for (int i = 0; i < ChildCount; i++)
                {
                    View child = GetChildAt(i);
                    mDividerPaint.Color = GetColorFromInteger(tabColorizer.GetDividerColor(i));
                    canvas.DrawLine(child.Right, separatorTop, child.Right, separatorTop + dividerHeightPx,
                        mDividerPaint);
                }

                canvas.DrawRect(0, height - mBottomBorderThickness, Width, height, mBottomBorderPaint);
            }
        }

        private int blendColor(int color1, int color2, float ratio)
        {
            float inverseRatio = 1f - ratio;
            float r = (Color.GetRedComponent(color1)*ratio) + (Color.GetRedComponent(color2)*inverseRatio);
            float g = (Color.GetGreenComponent(color1)*ratio) + (Color.GetGreenComponent(color2)*inverseRatio);
            float b = (Color.GetBlueComponent(color1)*ratio) + (Color.GetBlueComponent(color2)*inverseRatio);

            return Color.Rgb((int) r, (int) g, (int) b);
        }

        private class SimpleTabColorizer : SlidingTabScrollView.TabColorizer
        {
            private int[] mDividerColors;
            private int[] mIndicatorColors;

            public int[] IndicatorColors
            {
                set { mIndicatorColors = value; }
            }

            public int[] DividerColors
            {
                set { mDividerColors = value; }
            }

            public int GetIndicatorColor(int position)
            {
                return mIndicatorColors[position%mIndicatorColors.Length];
            }

            public int GetDividerColor(int position)
            {
                return mDividerColors[position%mDividerColors.Length];
            }
        }
    }
}