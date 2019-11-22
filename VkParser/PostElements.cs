using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkParser
{
    public class TextPost
    {
        public string id { get; set; }
        public string text { get; set; }

        public TextPost() { id = ""; text = ""; }
        public TextPost(string id, string text) { this.id = id; this.text = text; }

        public void print()
        {
            Console.WriteLine($"id: {id} , text: {text}");
        }
    }

    public class LinkPost
    {
        public string id { get; set; }
        public string link { get; set; }

        public LinkPost() { id = ""; link = ""; }
        public LinkPost(string id, string link) { this.id = id; this.link = link; }

        public void print()
        {
            Console.WriteLine($"id: {id} , link: {link}");
        }
    }

    public class ImgPost
    {

        public string id { get; set; }
        public string img { get; set; }

        public ImgPost() { id = ""; img = ""; }
        public ImgPost(string id, string img) { this.id = id; this.img = img; }

        public void print()
        {
            Console.WriteLine($"id: {id} , img: {img}");
        }
    }
}