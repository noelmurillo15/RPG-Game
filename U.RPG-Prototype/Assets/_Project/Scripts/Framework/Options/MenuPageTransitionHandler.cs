/*
 * MenuPageTransitionHandler - Handles transitions between MenuPages
 * Created by : Allan N. Murillo
 * Last Edited : 3/17/2022
 */

using UnityEngine;
using System.Collections;
using ANM.Framework.Utils;

namespace ANM.Framework.Options
{
    public class MenuPageTransitionHandler : MonoBehaviour
    {
        public MenuPageType entryPage;
        private MenuPage[] _pages;
        private Hashtable _pageHashtable;
        private MenuPageType _currentPageType;


        #region Unity Funcs

        private void Start()
        {
            _pages = FindObjectsOfType<MenuPage>();
            RegisterAllMenuPages();
            _currentPageType = MenuPageType.None;
            if (entryPage == _currentPageType) return;
            TurnOffAllPages();
            TurnMenuPageOn(entryPage);
        }

        #endregion

        #region Public Funcs

        public void TurnMenuPageOn(MenuPageType pageType)
        {
            if (pageType == MenuPageType.None) return;
            if (!DoesMenuPageExists(pageType)) return;
            
            // Debug.Log($"[MenuPageController]: Turning on page - {pageType}");
            
            _currentPageType = pageType;
            MenuPage menuPage = GetMenuPage(pageType);
            menuPage.gameObject.SetActive(true);
            menuPage.Animate(true);

            if (pageType != MenuPageType.Credits) return;
            menuPage.GetComponent<GameManagerCleanup>().enabled = true;
        }

        public void TurnMenuPageOff(MenuPageType off, MenuPageType on = MenuPageType.None, bool waitForEnd = false)
        {
            if (off == MenuPageType.None) return;
            if (!DoesMenuPageExists(off)) return;

            MenuPage offPage = GetMenuPage(off);
            if (offPage.gameObject.activeSelf) offPage.Animate(false);
            if (on == MenuPageType.None) return;

            MenuPage onPage = GetMenuPage(on);
            if (waitForEnd) StartCoroutine(WaitForTransitionEnd(onPage, offPage));
            else TurnMenuPageOn(on);
        }

        public MenuPageType GetCurrentMenuPageType()
        {
            return _currentPageType;
        }

        #endregion

        #region Private Funcs

        private IEnumerator WaitForTransitionEnd(MenuPage on, MenuPage off)
        {
            while (off.TargetState != MenuPage.FlagNone) yield return null;
            TurnMenuPageOn(on.type);
        }

        private void RegisterAllMenuPages()
        {
            _pageHashtable = new Hashtable();
            foreach (var menuPage in _pages) RegisterMenuPage(menuPage);
        }

        private void RegisterMenuPage(MenuPage page)
        {
            if (DoesMenuPageExists(page.type)) return;
            _pageHashtable.Add(page.type, page);
        }

        private MenuPage GetMenuPage(MenuPageType type)
        {
            if (DoesMenuPageExists(type)) return (MenuPage) _pageHashtable[type];
            return null;
        }

        private bool DoesMenuPageExists(MenuPageType type)
        {
            return _pageHashtable.ContainsKey(type);
        }

        private void TurnOffAllPages()
        {
            foreach (var page in _pages)
            {
                page.TurnOff();
            }
        }

        #endregion
    }
}
