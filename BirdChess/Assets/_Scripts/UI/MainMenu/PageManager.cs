/*
PageGeneric script need to be put in each of the UI pages
then insert those pages into the PageManager pageGeneric list/arrays
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private PageGeneric homePage;
    [SerializeField] private PageGeneric[] pageGenerics;
    private void Start()
    {
        OpenPage(homePage);
    }
    public void Btn_SelectPage(PageGeneric selectedPage)
    {
        OpenPage(selectedPage);
    }
    private void OpenPage(PageGeneric selectedPage)
    {
        //checks if the page exists in the list/array assigned in the script
        bool pageExist = false;
        if(selectedPage == null)
        {
            Debug.Log("no page was assigned");
            return;
        }
        foreach(PageGeneric page in pageGenerics)
        {
            if(page == selectedPage)
            {
                pageExist = true;
            }
        }
        if(!pageExist)
        {
            Debug.Log("page not found in the array: " + selectedPage);
            return;
        }

        //checks for the selected page in the list
        foreach(PageGeneric page in pageGenerics)
        {
            if(page != selectedPage)
            {
                page.ClosePage();
            }
            else
            {
                page.OpenPage();
            }
        }
    }
}
