using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public class LoopHorizontalScrollRect : LoopScrollRect
    {
        GridLayoutGroup mLayout;
        protected override float GetSize(RectTransform item)
        {
           // return LayoutUtility.GetPreferredWidth(item) + contentSpacing;
            return GetCellSize(item) + contentSpacing;
        }

        float GetCellSize(RectTransform item)
        {
            if (mLayout != null)
            {
                return mLayout.cellSize.x;
            }
            return LayoutUtility.GetPreferredHeight(item);
        }

        protected override float GetDimension(Vector2 vector)
        {
            return vector.x;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(-value, 0);
        }

        protected override void Awake()
        {
            base.Awake();
            directionSign = 1;

            mLayout = content.GetComponent<GridLayoutGroup>();
            if (mLayout != null && mLayout.constraint != GridLayoutGroup.Constraint.FixedRowCount)
            {
                Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.max.x > contentBounds.max.x)
            {
                float size = NewItemAtEnd();
                if (size > 0)
                {
                    if (threshold < size)
                    {
                        // Preventing new and delete repeatly...
                        threshold = size * 1.1f;
                    }
                    changed = true;
                }
            }
            else if (viewBounds.max.x < contentBounds.max.x - threshold)
            {
                float size = DeleteItemAtEnd();
                if (size > 0)
                {
                    changed = true;
                }
            }

            if (viewBounds.min.x < contentBounds.min.x)
            {
                float size = NewItemAtStart();
                if (size > 0)
                {
                    if (threshold < size)
                    {
                        threshold = size * 1.1f;
                    }
                    changed = true;
                }
            }
            else if (viewBounds.min.x > contentBounds.min.x + threshold)
            {
                float size = DeleteItemAtStart();
                if (size > 0)
                {
                    changed = true;
                }
            }
            return changed;
        }
    }
}