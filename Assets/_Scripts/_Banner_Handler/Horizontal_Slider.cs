using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace D11
{
	[RequireComponent(typeof(ScrollRect))]
	public class Horizontal_Slider : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public ScrollRect ScrollRect;
		public GameObject Dots;
		public int scrollforce = 40;
		public float scrolltime = 0.5f;

		private Toggle[] _dotToggles;

		private bool _pull;
		private bool _lerp;
		private int _dots;
		private float _pulltime;

	
		public void Begin(bool random = false)
		{
			ScrollRect.horizontalNormalizedPosition = 0;
			_dotToggles = Dots.GetComponentsInChildren<Toggle>(true);

			//if (random)
			//{
			//	ShowRandom();
			//}

			UpdateBanner(_dots);
			enabled = true;
		}

	
		public void Update()
		{
			if (!_lerp || _pull) return;

			if (Dots)
			{
				var _banner = GetCurrentPage();
				Debug.Log("<color=green>Out of Range</color>" + _dots);
				if (!_dotToggles[_dots].isOn)
				{
					UpdateBanner(_banner);
				}
			}

			var horizontalNormalizedPosition = (float)_dots / (ScrollRect.content.childCount - 1);

			ScrollRect.horizontalNormalizedPosition = Mathf.Lerp(ScrollRect.horizontalNormalizedPosition, horizontalNormalizedPosition, 5 * Time.deltaTime);

			if (Math.Abs(ScrollRect.horizontalNormalizedPosition - horizontalNormalizedPosition) < 0.001f)
			{
				ScrollRect.horizontalNormalizedPosition = horizontalNormalizedPosition;

				Debug.Log(ScrollRect.horizontalNormalizedPosition);
				_lerp = false;
			}
            
        }

		public void ShowRandom()
		{
			if (ScrollRect.content.childCount <= 1) return;

			int Dotzz;

			do
			{
				Dotzz = UnityEngine.Random.Range(0, ScrollRect.content.childCount);
			}
			while (Dotzz == _dots);

			_lerp = false;
			_dots = Dotzz;
			ScrollRect.horizontalNormalizedPosition = (float)_dots / (ScrollRect.content.childCount - 1);
		}


		public void SwipeNext()
		{
			Slide(1);
		}

		
		public void Swipeprev()
		{
			Slide(-1);
		}

		private void Slide(int direction)
		{
			direction = Math.Sign(direction);

            //	if (_dots == 0 && direction == -1 || _dots == ScrollRect.content.childCount - 1 && direction == 1) return;
           



            if (_dots == 0)
			{
				_dots = ScrollRect.content.childCount - 1;
				ScrollRect.horizontalNormalizedPosition = 1;
            }
			else if(_dots == ScrollRect.content.childCount - 1)
			{
                _dots = 0;
                ScrollRect.horizontalNormalizedPosition = 0;
            }
            _lerp = true;
            _dots += direction;
			_dots = Mathf.Clamp(_dots,0, ScrollRect.content.childCount - 1);


        }

		private int GetCurrentPage()
		{
			return Mathf.RoundToInt(ScrollRect.horizontalNormalizedPosition * (ScrollRect.content.childCount - 1));
		}

		private void UpdateBanner(int _banner)
		{
			if (Dots)
			{
				_dotToggles[_banner].isOn = true;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_pull = true;
			_pulltime = Time.time;
		}

		public void OnDrag(PointerEventData eventData)
		{
			//var _banner = GetCurrentPage();

			//if (_banner != _dots)
			//{
			//	_dots = _banner;
			//	UpdateBanner(_banner);
			//}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			var delta = eventData.pressPosition.x - eventData.position.x;

			if (Mathf.Abs(delta) > scrollforce && Time.time - _pulltime < scrolltime)
			{
				var direction = Math.Sign(delta);

				Slide(direction);
			}

			_pull = false;
			_lerp = true;
		}
	}
}