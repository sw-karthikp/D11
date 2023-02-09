﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace D11
{
    public abstract class BaseDroppable : MonoBehaviour, IDropHandler
    {

        /// =========================== System funtions ==================== ///

        #region System funcitons
        protected virtual void OnEnable()
        {
            ///Subcribes delegates
            /// Handle any item drag start
            BaseDraggable.OnItemDragStartEvent += OnAnyItemDragStart;
            /// Handle any item drag end
            BaseDraggable.OnItemDragEndEvent += OnAnyItemDragEnd;
        }

        protected virtual void OnDisable()
        {
            ///Unsubcribes delegates
            BaseDraggable.OnItemDragStartEvent -= OnAnyItemDragStart;
            BaseDraggable.OnItemDragEndEvent -= OnAnyItemDragEnd;
        }
        #endregion

        /// =========================== Callback funtions ==================== ///

        #region Callback functions
        /// <summary>
        /// On any item drag start need to disable all items raycast for correct drop operation
        /// </summary>
        /// <param name="item"> dragged item </param>
        public virtual void OnAnyItemDragStart(BaseDraggable item)
        {
        }

        /// <summary>
        /// On any item drag end enable all items raycast
        /// </summary>
        /// <param name="item"> dragged item </param>
        public virtual void OnAnyItemDragEnd(BaseDraggable item)
        {
        }

        #endregion

        /// <summary>
        /// OnDrop is called everytime when drag is stop moving and before OnEndDrag is called.
        /// </summary>
        public abstract void OnDrop(PointerEventData eventData);
    }

}