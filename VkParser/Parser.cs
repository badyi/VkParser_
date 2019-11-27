using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace VkParser
{
    class Parser
    {
        private List<TextPost> textPostList;
        private List<LinkPost> linksPostList;
        private List<ImgPost> imgsPostList;
        ChromeDriver chr;

        public Parser()
        {
            textPostList = new List<TextPost>();
            linksPostList = new List<LinkPost>();
            imgsPostList = new List<ImgPost>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=C:/Users/badyi/AppData/Local/Google/Chrome/User Data");
            chr = new ChromeDriver(options);
            chr.Navigate().GoToUrl("https://vk.com/feed");
            Parse();
        }

        private string get_text(ChromeDriver chr, string id)
        {
            try
            {
                return chr.FindElementByCssSelector("#wpt-" + id + " > div.wall_post_text").Text;
            }
            catch
            {
                return null;
            }
        }

        private List<string> get_Image_Links(ChromeDriver chr, string id)
        {
            char[] ch = { '"', ')' };
            List<string> listOfImageLinks = (from item in chr.FindElementsByCssSelector("#wpt-" + id + " > div.page_post_sized_thumbs.clear_fix > a") where item.Displayed select item.GetCssValue("background-image").Remove(0, 5).Trim(ch)).ToList();
            return listOfImageLinks;
        }

        private List<string> get_Links_Inside(ChromeDriver chr, string id)
        {
            List<string> ListOfLinks = (from item in chr.FindElementsByCssSelector("#wpt-" + id + " > div.wall_post_text > a") where item.Displayed select item.GetAttribute("href")).ToList();
            return ListOfLinks;
        }


        public void Parse()
        {
            List<string> idList = (from item in chr.FindElements(By.ClassName("post_link")) where item.Displayed select item.GetAttribute("href").Remove(0, 20)).ToList();

            for (int i = 0; i < idList.Count; i++)
            {

                string post_text = get_text(chr, idList[i]);
                TextPost tp = new TextPost(idList[i], post_text);

                if (tp.text != null || tp.text == "")
                    textPostList.Add(tp); //object without text wont be added

                List<string> listOfLinks = get_Links_Inside(chr, idList[i]);
                List<string> listOfImgs = get_Image_Links(chr, idList[i]);

                foreach (var item in listOfLinks)
                {
                    LinkPost lp = new LinkPost(idList[i], item);
                    if (lp.link != null) // null link wont be added
                        linksPostList.Add(lp);
                }

                foreach (var item in listOfImgs)
                {
                    ImgPost ip = new ImgPost(idList[i], item);
                    imgsPostList.Add(ip);
                }
            }
        }

        public void RefreshAndParse()
        {
            textPostList = new List<TextPost>();
            linksPostList = new List<LinkPost>();
            imgsPostList = new List<ImgPost>();

                chr.Navigate().Refresh();
                Parse();

        }

        //private int tryNum = 0;

        //public void RefreshAndParse()
        //{
        //    try
        //    {
        //        RefreshAndParse();
        //        tryNum = 0;
        //    }
        //    catch
        //    {
        //        tryNum++;
        //    }
        //}

        public void RefreshAndParse(ref bool completed1, ref bool completed2, ref bool completed3) {
            completed1 = false;
            completed2 = false;
            completed3 = false;

            chr.Navigate().Refresh();
            Parse();
        }
        public void close()
        {
            chr.Close();
        }

        public List<TextPost> get_list_texts()
        {
            return textPostList;
        }

        public List<LinkPost> get_list_links()
        {
            return linksPostList;
        }

        public List<ImgPost> get_list_imgs()
        {
            return imgsPostList;
        }

        public void printAll()
        {
            foreach (var item in textPostList)
            {
                item.print();
            }
            foreach (var item in linksPostList)
            {
                item.print();
            }
            foreach (var item in imgsPostList)
            {
                item.print();
            }
        }
    }
}
